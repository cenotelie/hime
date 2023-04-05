/*******************************************************************************
 * Copyright (c) 2020 Association Cénotélie (cenotelie.fr)
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this program.
 * If not, see <http://www.gnu.org/licenses/>.
 ******************************************************************************/

//! Module for the definition of a server-side workspace

use std::fs::File;
use std::io::{self, BufReader, ErrorKind, Read};
use std::path::{Path, PathBuf};

use hime_redist::text::TextPosition;
use hime_sdk::errors::Error;
use hime_sdk::grammars::{
    Grammar, RuleBodyElement, Symbol, SymbolRef, OPTION_AXIOM, OPTION_SEPARATOR
};
use hime_sdk::{CompilationTask, Input, InputReference, LoadedData, LoadedInput};
use serde_json::Value;
use tower_lsp::jsonrpc::Error as JsonRpcError;
use tower_lsp::lsp_types::{
    CodeLens, Command, Diagnostic, DiagnosticRelatedInformation, DiagnosticSeverity,
    DidChangeTextDocumentParams, FileChangeType, FileEvent, GotoDefinitionResponse, Hover,
    HoverContents, Location, MarkedString, Position, Range, SymbolInformation, SymbolKind, Url
};

use crate::symbols::{SymbolRegistry, SymbolRegistryElement};

/// Represents a document in a workspace
#[derive(Debug, Clone)]
pub struct Document {
    /// The document's URL
    pub url: Url,
    /// The content of the document in this version
    pub content: Option<String>,
    /// The current version
    pub version: Option<i32>,
    /// The diagnostics for the document
    pub diagnostics: Vec<Diagnostic>
}

impl Document {
    /// Creates a new document
    pub fn new(url: Url, content: String) -> Document {
        Document {
            url,
            content: Some(content),
            version: None,
            diagnostics: Vec::new()
        }
    }
}

/// The data associated to the workspace
#[derive(Debug, Clone, Default)]
pub struct WorkspaceData {
    /// The loaded inputs
    pub inputs: Vec<LoadedInput<'static>>,
    /// The loaded grammars
    pub grammars: Vec<Grammar>,
    /// The registry of symbols
    pub symbols: SymbolRegistry
}

impl WorkspaceData {
    /// Translate an input reference to a LSP range
    pub fn get_range(&self, input_reference: InputReference) -> Range {
        WorkspaceData::to_range(&self.inputs, input_reference)
    }

    /// Translate an input reference to a LSP range
    fn to_range(inputs: &[LoadedInput], input_reference: InputReference) -> Range {
        let end = inputs[input_reference.input_index]
            .content
            .get_position_for(input_reference.position, input_reference.length);
        Range::new(
            Position::new(
                (input_reference.position.line - 1) as u32,
                (input_reference.position.column - 1) as u32
            ),
            Position::new((end.line - 1) as u32, (end.column - 1) as u32)
        )
    }

    /// Gets the symbol at a location in an input
    fn find_symbol_at(&self, location: InputReference) -> Option<&SymbolRegistryElement> {
        for symbols in self.symbols.grammars.iter() {
            for symbol in symbols.values() {
                if symbol.is_at_location(&location) {
                    return Some(symbol);
                }
            }
        }
        None
    }
}

/// Represents the current workspace for a server
#[derive(Debug, Clone, Default)]
pub struct Workspace {
    /// The root URL for the workspace
    pub root: Option<Url>,
    /// The documents in the workspace
    pub documents: Vec<Document>,
    /// The currently loaded data, if any
    pub data: Option<WorkspaceData>
}

impl Workspace {
    /// Scans the current workspace for relevant documents
    pub fn scan_workspace(&mut self, root: Url) -> io::Result<()> {
        let path = PathBuf::from(root.path());
        if path.exists() {
            self.scan_workspace_in(&path)?;
        }
        self.root = Some(root);
        Ok(())
    }

    /// Scans the workspace in the specified folder
    fn scan_workspace_in(&mut self, path: &Path) -> io::Result<()> {
        if Workspace::scan_workspace_is_dir_excluded(path) {
            return Ok(());
        }
        for element in std::fs::read_dir(path)? {
            let sub_path = element?.path();
            if sub_path.is_dir() {
                self.scan_workspace_in(&sub_path)?;
            } else if Workspace::scan_workspace_is_file_included(&sub_path) {
                self.resolve_document_path(&sub_path)?;
            }
        }
        Ok(())
    }

    /// Determines whether the specified file should be analyzed
    fn scan_workspace_is_file_included(path: &Path) -> bool {
        match path.extension() {
            None => false,
            Some(name) => name == "gram"
        }
    }

    /// Determines whether the specified file or directory is excluded
    fn scan_workspace_is_dir_excluded(path: &Path) -> bool {
        match path.file_name() {
            None => true,
            Some(name) => name == ".git" || name == ".hg" || name == ".svn"
        }
    }

    /// Resolves a document
    fn resolve_document_path(&mut self, path: &Path) -> io::Result<()> {
        let uri = match Url::from_file_path(path.canonicalize()?) {
            Ok(uri) => uri,
            Err(_) => {
                return Err(io::Error::new(
                    ErrorKind::NotFound,
                    String::from("Path cannot be converted to Url")
                ))
            }
        };
        self.resolve_document(uri, path)
    }

    /// Resolves a document
    fn resolve_document_url(&mut self, uri: Url) -> io::Result<()> {
        let path = PathBuf::from(uri.path());
        self.resolve_document(uri, &path)
    }

    /// Resolves a document
    fn resolve_document(&mut self, uri: Url, path: &Path) -> io::Result<()> {
        let mut reader = BufReader::new(File::open(path)?);
        let mut content = String::new();
        reader.read_to_string(&mut content)?;
        if self.documents.iter().all(|doc| doc.url != uri) {
            self.documents.push(Document::new(uri, content));
        }
        Ok(())
    }

    /// Synchronises on file events
    pub fn on_file_events(&mut self, events: &[FileEvent]) -> io::Result<()> {
        for event in events.iter() {
            match event.typ {
                FileChangeType::CREATED => {
                    self.resolve_document_url(event.uri.clone())?;
                }
                FileChangeType::CHANGED => {
                    // TODO: handle this
                }
                FileChangeType::DELETED => {
                    self.documents.retain(|doc| doc.url != event.uri);
                }
                _ => {}
            }
        }
        Ok(())
    }

    /// Synchronizes on changes
    pub fn on_file_changes(&mut self, event: DidChangeTextDocumentParams) {
        if let Some(document) = self
            .documents
            .iter_mut()
            .find(|doc| doc.url == event.text_document.uri)
        {
            for change in event.content_changes.into_iter() {
                if change.range.is_none() && change.range_length.is_none() {
                    document.content = Some(change.text);
                }
            }
        }
    }

    /// Runs the diagnostics
    pub fn lint(&mut self) {
        self.data = None;
        let mut task = CompilationTask::default();
        for doc in self.documents.iter_mut() {
            doc.diagnostics.clear();
            if let Some(content) = doc.content.as_ref() {
                task.inputs.push(Input::Raw(content));
            }
        }
        match task.load() {
            Ok(data) => {
                let mut data = data.into_static();
                let mut errors = Vec::new();
                for (index, grammar) in data.grammars.iter_mut().enumerate() {
                    if let Err(mut errs) = task.generate_in_memory(grammar, index) {
                        errors.append(&mut errs);
                    }
                }
                for error in errors.iter() {
                    if let Some((index, diag)) = to_diagnostic(&mut self.documents, &data, error) {
                        self.documents[index].diagnostics.push(diag);
                    }
                }
                let symbols = SymbolRegistry::from(&data.grammars);
                self.data = Some(WorkspaceData {
                    inputs: data.inputs,
                    grammars: data.grammars,
                    symbols
                });
            }
            Err(errors) => {
                let errors = errors.into_static();
                for error in errors.errors.iter() {
                    if let Some((index, diag)) =
                        to_diagnostic(&mut self.documents, &errors.context, error)
                    {
                        self.documents[index].diagnostics.push(diag);
                    }
                }
            }
        }
    }

    /// Lookups information for symbols matching the query
    pub fn lookup_symbols(&self, query: &str) -> Vec<SymbolInformation> {
        let mut result = Vec::new();
        if let Some(data) = self.data.as_ref() {
            let parts = query.split('.').collect::<Vec<_>>();
            if parts.len() == 2 {
                // lookup in a specific grammar
                if let Some(grammar) = data.grammars.iter().find(|g| g.name == parts[0]) {
                    self.lookup_symbols_in(grammar, &mut result, parts[1]);
                }
            } else if parts.len() == 1 {
                // lookup in all grammars
                for grammar in data.grammars.iter() {
                    self.lookup_symbols_in(grammar, &mut result, parts[0]);
                }
            }
        }
        result
    }

    /// Lookup symbols matching the specified name in the grammar
    fn lookup_symbols_in(
        &self,
        grammar: &Grammar,
        buffer: &mut Vec<SymbolInformation>,
        query: &str
    ) {
        if grammar.name.contains(query) {
            buffer.push(self.new_symbol(
                grammar.name.to_string(),
                SymbolKind::CLASS,
                grammar.input_ref
            ));
        }
        for terminal in grammar.terminals.iter() {
            if !terminal.is_anonymous && terminal.name.contains(query) {
                buffer.push(self.new_symbol(
                    format!("{}.{}", grammar.name, terminal.name),
                    SymbolKind::FIELD,
                    terminal.input_ref
                ));
            }
        }
        for variable in grammar.variables.iter() {
            if variable.generated_for.is_none() && variable.name.contains(query) {
                if let Some(rule) = variable.rules.first() {
                    buffer.push(self.new_symbol(
                        format!("{}.{}", grammar.name, variable.name),
                        SymbolKind::PROPERTY,
                        rule.head_input_ref
                    ));
                }
            }
        }
        for symbol in grammar.virtuals.iter() {
            if symbol.name.contains(query) {
                if let Some(element) =
                    self.lookup_symbol_in_rules(grammar, SymbolRef::Virtual(symbol.id))
                {
                    buffer.push(self.new_symbol(
                        format!("{}.{}", grammar.name, symbol.name),
                        SymbolKind::CONSTANT,
                        element.input_ref.unwrap()
                    ));
                }
            }
        }
        for symbol in grammar.actions.iter() {
            if symbol.name.contains(query) {
                if let Some(element) =
                    self.lookup_symbol_in_rules(grammar, SymbolRef::Action(symbol.id))
                {
                    buffer.push(self.new_symbol(
                        format!("{}.{}", grammar.name, symbol.name),
                        SymbolKind::METHOD,
                        element.input_ref.unwrap()
                    ));
                }
            }
        }
    }

    /// Gets the definition of a symbol at a location
    pub fn get_definition_at(
        &self,
        doc_uri: &str,
        line: u32,
        character: u32
    ) -> Option<GotoDefinitionResponse> {
        let doc_index = self
            .documents
            .iter()
            .enumerate()
            .find(|(_, doc)| doc.url.as_str() == doc_uri)?
            .0;
        let input_ref = InputReference {
            input_index: doc_index,
            position: TextPosition {
                line: line as usize + 1,
                column: character as usize + 1
            },
            length: 0
        };
        let data = self.data.as_ref()?;
        let symbol = data.find_symbol_at(input_ref)?;
        if symbol.definitions.is_empty() {
            None
        } else if symbol.definitions.len() == 1 {
            Some(GotoDefinitionResponse::Scalar(
                self.get_location(symbol.definitions[0])
            ))
        } else {
            Some(GotoDefinitionResponse::Array(
                symbol
                    .definitions
                    .iter()
                    .map(|input_ref| self.get_location(*input_ref))
                    .collect()
            ))
        }
    }

    /// Gets all the references to a symbol at a location
    pub fn get_references_at(
        &self,
        doc_uri: &str,
        line: u32,
        character: u32
    ) -> Option<Vec<Location>> {
        let doc_index = self
            .documents
            .iter()
            .enumerate()
            .find(|(_, doc)| doc.url.as_str() == doc_uri)?
            .0;
        let input_ref = InputReference {
            input_index: doc_index,
            position: TextPosition {
                line: line as usize + 1,
                column: character as usize + 1
            },
            length: 0
        };
        let data = self.data.as_ref()?;
        let symbol = data.find_symbol_at(input_ref)?;
        let mut references = Vec::new();
        for input_ref in symbol.definitions.iter() {
            references.push(self.get_location(*input_ref));
        }
        for input_ref in symbol.references.iter() {
            references.push(self.get_location(*input_ref));
        }
        Some(references)
    }

    /// Gets the description of the symbol at a location
    pub fn get_symbol_description_at(
        &self,
        doc_uri: &str,
        line: u32,
        character: u32
    ) -> Option<Hover> {
        let doc_index = self
            .documents
            .iter()
            .enumerate()
            .find(|(_, doc)| doc.url.as_str() == doc_uri)?
            .0;
        let input_ref = InputReference {
            input_index: doc_index,
            position: TextPosition {
                line: line as usize + 1,
                column: character as usize + 1
            },
            length: 0
        };
        let data = self.data.as_ref()?;
        let symbol = data.find_symbol_at(input_ref)?;
        let content = match symbol.symbol_ref {
            SymbolRef::Dummy => String::from("<dummy>"),
            SymbolRef::Epsilon => String::from("<epsilon>"),
            SymbolRef::Dollar => String::from("<dollar>"),
            SymbolRef::NullTerminal => String::from("<null>"),
            SymbolRef::Terminal(sid) => data.grammars[symbol.grammar_index]
                .get_terminal(sid)
                .unwrap()
                .get_description(),
            SymbolRef::Variable(sid) => data.grammars[symbol.grammar_index]
                .get_variable(sid)
                .unwrap()
                .get_description(),
            SymbolRef::Virtual(sid) => data.grammars[symbol.grammar_index]
                .get_virtual(sid)
                .unwrap()
                .get_description(),
            SymbolRef::Action(sid) => data.grammars[symbol.grammar_index]
                .get_action(sid)
                .unwrap()
                .get_description()
        };
        Some(Hover {
            contents: HoverContents::Scalar(MarkedString::String(content)),
            range: symbol
                .get_full_reference(&input_ref)
                .map(|input_ref| self.get_location(input_ref).range)
        })
    }

    /// Gets the code lens for a document
    pub fn get_code_lens(&self, doc_uri: &str) -> Option<Vec<CodeLens>> {
        let doc_index = self
            .documents
            .iter()
            .enumerate()
            .find(|(_, doc)| doc.url.as_str() == doc_uri)?
            .0;
        let data = self.data.as_ref()?;
        let mut result = Vec::new();
        for grammar in data.grammars.iter() {
            if grammar.input_ref.input_index == doc_index {
                result.push(CodeLens {
                    range: self.get_location(grammar.input_ref).range,
                    command: Some(Command {
                        command: String::from("hime.playground"),
                        title: String::from("Test grammar on input"),
                        arguments: Some(vec![Value::String(grammar.name.clone())])
                    }),
                    data: None
                });
            }
        }
        if result.is_empty() {
            None
        } else {
            Some(result)
        }
    }

    /// Tests an input against a grammar
    pub fn parse_input(
        &self,
        grammar_name: &str,
        input: &str
    ) -> Result<Option<Value>, JsonRpcError> {
        match &self.data {
            Some(data) => match data
                .grammars
                .iter()
                .enumerate()
                .find(|(_, grammar)| grammar.name == grammar_name)
            {
                Some((grammar_index, grammar)) => {
                    let mut grammar = grammar.clone();
                    let task = CompilationTask::default();
                    match task.generate_in_memory(&mut grammar, grammar_index) {
                        Ok(parser) => {
                            let result = parser.parse(input);
                            Ok(Some(serde_json::to_value(&result).unwrap()))
                        }
                        Err(_) => Ok(None)
                    }
                }
                None => Err(JsonRpcError::invalid_request())
            },
            None => Err(JsonRpcError::invalid_request())
        }
    }

    /// Finds a symbol in a rule
    fn lookup_symbol_in_rules(
        &self,
        grammar: &Grammar,
        symbol_ref: SymbolRef
    ) -> Option<RuleBodyElement> {
        for variable in grammar.variables.iter() {
            for rule in variable.rules.iter() {
                for element in rule.body.elements.iter() {
                    if element.symbol == symbol_ref && element.input_ref.is_some() {
                        return Some(*element);
                    }
                }
            }
        }
        None
    }

    /// Creates a new symbol information
    #[allow(deprecated)]
    fn new_symbol(
        &self,
        name: String,
        kind: SymbolKind,
        input_ref: InputReference
    ) -> SymbolInformation {
        SymbolInformation {
            name,
            kind,
            tags: None,
            deprecated: None,
            location: self.get_location(input_ref),
            container_name: None
        }
    }

    /// Transforms an input reference into a location
    fn get_location(&self, input_ref: InputReference) -> Location {
        // we expect to have loaded data when calling this method,
        // otherwise we would not have an input reference in argument
        let data = self.data.as_ref().unwrap();
        let document = &self.documents[input_ref.input_index];
        Location {
            range: data.get_range(input_ref),
            uri: document.url.clone()
        }
    }
}

/// Converts an error to a diagnostic
fn to_diagnostic(
    documents: &mut [Document],
    data: &LoadedData,
    error: &Error
) -> Option<(usize, Diagnostic)> {
    match error {
        Error::Parsing(input_reference, msg) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: msg.clone(),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::InvalidOption(grammar_index, name, valid) => {
            let option = data.grammars[*grammar_index].get_option(name).unwrap();
            let expected = if valid.is_empty() {
                String::new()
            } else {
                format!(" expected one of: {}", valid.join(", "))
            };
            let input_reference = option.value_input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!("Invalid value for grammar option `{}`{}", name, expected),
                    related_information: None,
                    tags: None,
                    data: None
                }
            ))
        }
        Error::AxiomNotSpecified(grammar_index) => {
            let input_reference = data.grammars[*grammar_index].input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: "Grammar axiom has not been specified".to_string(),
                    related_information: None,
                    tags: None,
                    data: None
                }
            ))
        }
        Error::AxiomNotDefined(grammar_index) => {
            let option = data.grammars[*grammar_index]
                .get_option(OPTION_AXIOM)
                .unwrap();
            let input_reference = option.value_input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!("Grammar axiom `{}` is not defined", &option.value),
                    related_information: None,
                    tags: None,
                    data: None
                }
            ))
        }
        Error::SeparatorNotDefined(grammar_index) => {
            let option = data.grammars[*grammar_index]
                .get_option(OPTION_SEPARATOR)
                .unwrap();
            let input_reference = option.value_input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!("Grammar separator token `{}` is not defined", &option.value),
                    related_information: None,
                    tags: None,
                    data: None
                }
            ))
        }
        Error::SeparatorIsContextual(grammar_index, terminal_ref) => {
            let separator = data.grammars[*grammar_index]
                .get_terminal(terminal_ref.sid())
                .unwrap();
            let input_reference = separator.input_ref;
            let context = &data.grammars[*grammar_index].contexts[separator.context];
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Grammar separator token `{}` is only defined for context `{}`",
                        &separator.name, context
                    ),
                    related_information: None,
                    tags: None,
                    data: None
                }
            ))
        }
        Error::SeparatorCannotBeMatched(grammar_index, error) => {
            let terminal = data.grammars[*grammar_index]
                .get_terminal(error.terminal.sid())
                .unwrap();
            let input_reference = terminal.input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Token `{}` is expected but can never be matched",
                        &terminal.name
                    ),
                    related_information: None,
                    tags: None,
                    data: None
                }
            ))
        }
        Error::TemplateRuleNotFound(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Cannot find template rule `{}`", name),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::TemplateRuleWrongNumberOfArgs(input_reference, expected, provided) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!(
                    "Template expected {} arguments, {} given",
                    expected, provided
                ),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::SymbolNotFound(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Cannot find symbol `{}`", name),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::InvalidCharacterSpan(input_reference) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: "Invalid character span, end is before begin".to_string(),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::UnknownUnicodeBlock(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Unknown unicode block `{}`", name),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::UnknownUnicodeCategory(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Unknown unicode category `{}`", name),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::UnsupportedNonPlane0InCharacterClass(input_reference, c) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!(
                    "Unsupported non-plane 0 Unicode character ({0}) in character class",
                    c
                ),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::InvalidCodePoint(input_reference, c) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("The value U+{:0X} is not a supported unicode code point", c),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::OverridingPreviousTerminal(input_reference, name, _previous) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Overriding the previous definition of `{}`", name),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::GrammarNotDefined(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: WorkspaceData::to_range(&data.inputs, *input_reference),
                severity: Some(DiagnosticSeverity::ERROR),
                code: None,
                code_description: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Grammar `{}` is not defined", name),
                related_information: None,
                tags: None,
                data: None
            }
        )),
        Error::LrConflict(grammar_index, conflict) => {
            let grammar = &data.grammars[*grammar_index];
            let _terminal = grammar.get_symbol_value(conflict.lookahead.terminal.into());
            None
        }
        Error::TerminalOutsideContext(grammar_index, error) => {
            let grammar = &data.grammars[*grammar_index];
            let terminal = grammar.get_terminal(error.terminal.sid()).unwrap();
            let input_reference = terminal.input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Contextual terminal `{}` is expected outside its context",
                        &terminal.name
                    ),
                    related_information: Some(
                        error
                            .items
                            .iter()
                            .map(|item| {
                                let rule = item.rule.get_rule_in(grammar);
                                let choice = &rule.body.choices[0];
                                let input_ref = choice.elements[item.position].input_ref.unwrap();
                                DiagnosticRelatedInformation {
                                    location: Location {
                                        uri: documents[input_ref.input_index].url.clone(),
                                        range: WorkspaceData::to_range(&data.inputs, input_ref)
                                    },
                                    message: String::from("Used outside required context")
                                }
                            })
                            .collect()
                    ),
                    tags: None,
                    data: None
                }
            ))
        }
        Error::TerminalCannotBeMatched(grammar_index, error) => {
            let terminal = data.grammars[*grammar_index]
                .get_terminal(error.terminal.sid())
                .unwrap();
            let input_reference = terminal.input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Token `{}` is expected but can never be matched",
                        &terminal.name
                    ),
                    related_information: None,
                    tags: None,
                    data: None
                }
            ))
        }
        Error::TerminalMatchesEmpty(grammar_index, terminal_ref) => {
            let terminal = data.grammars[*grammar_index]
                .get_terminal(terminal_ref.sid())
                .unwrap();
            let input_reference = terminal.input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: WorkspaceData::to_range(&data.inputs, input_reference),
                    severity: Some(DiagnosticSeverity::ERROR),
                    code: None,
                    code_description: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Terminal `{}` matches empty string, which is not allowed",
                        &terminal.name
                    ),
                    related_information: None,
                    tags: None,
                    data: None
                }
            ))
        }
        _ => None
    }
}

#[test]
fn test_scan_workspace_in() -> io::Result<()> {
    let mut workspace = Workspace::default();
    let root = std::env::current_dir()?.parent().unwrap().to_owned();
    workspace.scan_workspace_in(&root)?;
    for doc in workspace.documents.iter() {
        println!("{}", &doc.url);
    }
    assert_eq!(workspace.documents.is_empty(), false);
    Ok(())
}

#[test]
fn test_scan_workspace() -> io::Result<()> {
    let mut workspace = Workspace::default();
    let root = std::env::current_dir()?.parent().unwrap().to_owned();
    let url = match Url::from_file_path(root) {
        Ok(url) => url,
        Err(_) => panic!("Failed to convert current dir to Url")
    };
    workspace.scan_workspace(url)?;
    for doc in workspace.documents.iter() {
        println!("{}", &doc.url);
    }
    assert_eq!(workspace.documents.is_empty(), false);
    Ok(())
}
