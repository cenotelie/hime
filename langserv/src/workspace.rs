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

use hime_sdk::errors::Error;
use hime_sdk::grammars::OPTION_AXIOM;
use hime_sdk::grammars::OPTION_SEPARATOR;
use hime_sdk::{CompilationTask, Input, InputReference, LoadedData};
use std::collections::HashMap;
use std::fs::File;
use std::io::{self, BufReader, ErrorKind, Read};
use std::path::PathBuf;
use tower_lsp::lsp_types::{
    Diagnostic, DiagnosticRelatedInformation, DiagnosticSeverity, DidChangeTextDocumentParams,
    FileChangeType, FileEvent, Location, Position, Range, Url
};

/// Represents a document in a workspace
#[derive(Debug, Clone)]
pub struct Document {
    /// The document's URL
    pub url: Url,
    /// The content of the document in this version
    pub content: String,
    /// The current version
    pub version: Option<i64>,
    /// The diagnostics for the document
    pub diagnostics: Vec<Diagnostic>
}

impl Document {
    /// Creates a new document
    pub fn new(url: Url, content: String) -> Document {
        Document {
            url,
            content,
            version: None,
            diagnostics: Vec::new()
        }
    }
}

/// Represents the current workspace for a server
#[derive(Debug, Clone, Default)]
pub struct Workspace {
    /// The root URL for the workspace
    pub root: Option<Url>,
    /// The documents in the workspace
    pub documents: HashMap<Url, Document>,
    /// The currently loaded data, if any
    pub data: Option<LoadedData>
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
    fn scan_workspace_in(&mut self, path: &PathBuf) -> io::Result<()> {
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
    fn scan_workspace_is_file_included(path: &PathBuf) -> bool {
        match path.extension() {
            None => false,
            Some(name) => name == "gram"
        }
    }

    /// Determines whether the specified file or directory is excluded
    fn scan_workspace_is_dir_excluded(path: &PathBuf) -> bool {
        match path.file_name() {
            None => true,
            Some(name) => name == ".git" || name == ".hg" || name == ".svn"
        }
    }

    /// Resolves a document
    fn resolve_document_path(&mut self, path: &PathBuf) -> io::Result<()> {
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
    fn resolve_document(&mut self, uri: Url, path: &PathBuf) -> io::Result<()> {
        let mut reader = BufReader::new(File::open(path)?);
        let mut content = String::new();
        reader.read_to_string(&mut content)?;
        let uri2 = uri.clone();
        self.documents
            .entry(uri)
            .or_insert_with(move || Document::new(uri2, content));
        Ok(())
    }

    /// Synchronises on file events
    pub fn on_file_events(&mut self, events: &[FileEvent]) -> io::Result<()> {
        for event in events.iter() {
            match event.typ {
                FileChangeType::Created => {
                    self.resolve_document_url(event.uri.clone())?;
                }
                FileChangeType::Changed => {
                    // TODO: handle this
                }
                FileChangeType::Deleted => {
                    self.documents.remove(&event.uri);
                }
            }
        }
        Ok(())
    }

    /// Synchronizes on changes
    pub fn on_file_changes(&mut self, event: DidChangeTextDocumentParams) {
        if let Some(document) = self.documents.get_mut(&event.text_document.uri) {
            for change in event.content_changes.into_iter() {
                if change.range.is_none() && change.range_length.is_none() {
                    document.content = change.text;
                }
            }
        }
    }

    /// Runs the diagnostics
    pub fn lint(&mut self) {
        self.data = None;
        let mut task = CompilationTask::default();
        let mut documents: Vec<&mut Document> =
            self.documents.iter_mut().map(|(_, doc)| doc).collect();
        for doc in documents.iter_mut() {
            task.inputs.push(Input::Raw(&doc.content));
            doc.diagnostics.clear();
        }
        match task.load() {
            Ok(mut data) => {
                let mut errors = Vec::new();
                for (index, grammar) in data.grammars.iter_mut().enumerate() {
                    if let Err(mut errs) = task.generate_in_memory(grammar, index) {
                        errors.append(&mut errs);
                    }
                }
                for error in errors.iter() {
                    if let Some((index, diag)) = to_diagnostic(&documents, &data, error) {
                        documents[index].diagnostics.push(diag);
                    }
                }
                self.data = Some(data)
            }
            Err(errors) => {
                for error in errors.errors.iter() {
                    if let Some((index, diag)) = to_diagnostic(&documents, &errors.data, error) {
                        documents[index].diagnostics.push(diag);
                    }
                }
            }
        }
    }
}

/// Converts an error to a diagnostic
fn to_diagnostic(
    documents: &[&mut Document],
    data: &LoadedData,
    error: &Error
) -> Option<(usize, Diagnostic)> {
    match error {
        Error::Parsing(input_reference, msg) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: msg.clone(),
                related_information: None,
                tags: None
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
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!("Invalid value for grammar option `{}`{}", name, expected),
                    related_information: None,
                    tags: None
                }
            ))
        }
        Error::AxiomNotSpecified(grammar_index) => {
            let input_reference = data.grammars[*grammar_index].input_ref;
            Some((
                input_reference.input_index,
                Diagnostic {
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: "Grammar axiom has not been specified".to_string(),
                    related_information: None,
                    tags: None
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
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!("Grammar axiom `{}` is not defined", &option.value),
                    related_information: None,
                    tags: None
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
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!("Grammar separator token `{}` is not defined", &option.value),
                    related_information: None,
                    tags: None
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
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Grammar separator token `{}` is only defined for context `{}`",
                        &separator.name, context
                    ),
                    related_information: None,
                    tags: None
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
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Token `{}` is expected but can never be matched",
                        &terminal.name
                    ),
                    related_information: None,
                    tags: None
                }
            ))
        }
        Error::TemplateRuleNotFound(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Cannot find template rule `{}`", name),
                related_information: None,
                tags: None
            }
        )),
        Error::TemplateRuleWrongNumberOfArgs(input_reference, expected, provided) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!(
                    "Template expected {} arguments, {} given",
                    expected, provided
                ),
                related_information: None,
                tags: None
            }
        )),
        Error::SymbolNotFound(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Cannot find symbol `{}`", name),
                related_information: None,
                tags: None
            }
        )),
        Error::InvalidCharacterSpan(input_reference) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: "Invalid character span, end is before begin".to_string(),
                related_information: None,
                tags: None
            }
        )),
        Error::UnknownUnicodeBlock(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Unknown unicode block `{}`", name),
                related_information: None,
                tags: None
            }
        )),
        Error::UnknownUnicodeCategory(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Unknown unicode category `{}`", name),
                related_information: None,
                tags: None
            }
        )),
        Error::UnsupportedNonPlane0InCharacterClass(input_reference, c) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!(
                    "Unsupported non-plane 0 Unicode character ({0}) in character class",
                    c
                ),
                related_information: None,
                tags: None
            }
        )),
        Error::InvalidCodePoint(input_reference, c) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("The value U+{:0X} is not a supported unicode code point", c),
                related_information: None,
                tags: None
            }
        )),
        Error::OverridingPreviousTerminal(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Overriding the previous definition of `{}`", name),
                related_information: None,
                tags: None
            }
        )),
        Error::GrammarNotDefined(input_reference, name) => Some((
            input_reference.input_index,
            Diagnostic {
                range: to_range(data, *input_reference),
                severity: Some(DiagnosticSeverity::Error),
                code: None,
                source: Some(super::CRATE_NAME.to_string()),
                message: format!("Grammar `{}` is not defined", name),
                related_information: None,
                tags: None
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
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
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
                                        range: to_range(data, input_ref)
                                    },
                                    message: String::from("Used outside required context")
                                }
                            })
                            .collect()
                    ),
                    tags: None
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
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Token `{}` is expected but can never be matched",
                        &terminal.name
                    ),
                    related_information: None,
                    tags: None
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
                    range: to_range(data, input_reference),
                    severity: Some(DiagnosticSeverity::Error),
                    code: None,
                    source: Some(super::CRATE_NAME.to_string()),
                    message: format!(
                        "Terminal `{}` matches empty string, which is not allowed",
                        &terminal.name
                    ),
                    related_information: None,
                    tags: None
                }
            ))
        }
        _ => None
    }
}

/// Translate an input reference to a LSP range
fn to_range(data: &LoadedData, input_reference: InputReference) -> Range {
    let end = data.inputs[input_reference.input_index]
        .content
        .get_position_for(input_reference.position, input_reference.length);
    Range::new(
        Position::new(
            (input_reference.position.line - 1) as u64,
            (input_reference.position.column - 1) as u64
        ),
        Position::new((end.line - 1) as u64, (end.column - 1) as u64)
    )
}

#[test]
fn test_scan_workspace_in() -> io::Result<()> {
    let mut workspace = Workspace::default();
    let root = std::env::current_dir()?.parent().unwrap().to_owned();
    workspace.scan_workspace_in(&root)?;
    for (uri, _) in workspace.documents.iter() {
        println!("{}", uri);
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
    for (uri, _) in workspace.documents.iter() {
        println!("{}", uri);
    }
    assert_eq!(workspace.documents.is_empty(), false);
    Ok(())
}
