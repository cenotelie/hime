/*******************************************************************************
 * Copyright (c) 2019 Association Cénotélie (cenotelie.fr)
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

//! Loading facilities for grammars

pub mod hime_grammar;

use std::io::{self, Read};

use hime_redist::ast::{Ast, AstNode};
use hime_redist::errors::ParseErrorDataTrait;
use hime_redist::lexers::DEFAULT_CONTEXT;
use hime_redist::parsers::{
    TREE_ACTION_DROP, TREE_ACTION_NONE, TREE_ACTION_PROMOTE, TREE_ACTION_REPLACE_BY_CHILDREN,
    TREE_ACTION_REPLACE_BY_EPSILON
};
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;

use crate::errors::{Error, Errors};
use crate::finite::{FinalItem, NFA};
use crate::grammars::{
    BodySet, Grammar, Rule, RuleBody, SymbolRef, TemplateRuleBody, TemplateRuleRef,
    TemplateRuleSymbol, DEFAULT_CONTEXT_NAME
};
use crate::unicode::{Span, BLOCKS, CATEGORIES};
use crate::{CharSpan, Input, InputReference, LoadedData, LoadedInput};

/// Represents a generalised input for a loader
pub struct LoadInput<'a>(String, Box<dyn Read + 'a>);

/// Open all inputs
pub fn open_all<'a>(inputs: &[Input<'a>]) -> Result<Vec<LoadInput<'a>>, Errors> {
    let mut errors = Vec::new();
    let mut result = Vec::new();
    for input in inputs.iter() {
        match input.open() {
            Ok(stream) => result.push(LoadInput(input.name(), stream)),
            Err(error) => errors.push(Error::Io(error))
        }
    }
    if errors.is_empty() {
        Ok(result)
    } else {
        Err(Errors::from(LoadedData::default(), errors))
    }
}

/// Build the loaded data structure
fn build_loaded_data(
    names: Vec<String>,
    parse_results: Vec<ParseResult>,
    grammars: Vec<Grammar>
) -> LoadedData {
    LoadedData {
        inputs: names
            .into_iter()
            .zip(parse_results.into_iter())
            .map(|(name, result)| LoadedInput {
                name,
                content: result.text
            })
            .collect(),
        grammars
    }
}

/// Loads all inputs into grammars
pub fn load_inputs(inputs: &[Input]) -> Result<LoadedData, Errors> {
    load(open_all(inputs)?)
}

/// Loads all inputs into grammars
pub fn load(inputs: Vec<LoadInput>) -> Result<LoadedData, Errors> {
    // parse
    let (names, results) = parse_inputs(inputs)?;
    // extract grammar roots
    let asts: Vec<Ast> = results.iter().map(|r| r.get_ast()).collect();
    let doc_roots: Vec<AstNode> = asts.iter().map(|ast| ast.get_root()).collect();
    let roots: Vec<(usize, AstNode)> = doc_roots
        .iter()
        .enumerate()
        .map(|(index, &doc_root)| doc_root.into_iter().map(move |root| (index, root)))
        .flatten()
        .collect();
    // get the grammars
    let (grammars, errors) = do_load_grammars(&roots);
    let data = build_loaded_data(names, results, grammars);
    if errors.is_empty() {
        Ok(data)
    } else {
        Err(Errors::from(data, errors))
    }
}

/// Loads grammars from AST roots
pub fn load_parsed(roots: &[(usize, AstNode)]) -> Result<Vec<Grammar>, Vec<Error>> {
    let (grammars, errors) = do_load_grammars(roots);
    if errors.is_empty() {
        Ok(grammars)
    } else {
        Err(errors)
    }
}

/// Loads grammars from AST roots
fn do_load_grammars(roots: &[(usize, AstNode)]) -> (Vec<Grammar>, Vec<Error>) {
    let mut errors = Vec::new();
    let mut completed = Vec::new();
    let mut to_resolve = Vec::new();
    for &(input_index, grammar_root) in roots.iter() {
        let loader = Loader::new(input_index, grammar_root, &mut errors);
        if loader.is_solved() {
            completed.push(loader);
        } else {
            to_resolve.push(loader);
        }
    }
    resolve_inheritance(&mut completed, &mut to_resolve, &mut errors);
    (
        completed.into_iter().map(|loader| loader.grammar).collect(),
        errors
    )
}

/// Resolves inheritance and load grammars
fn resolve_inheritance<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd>(
    completed: &mut Vec<Loader<'a, 'b, 'c, 'd>>,
    to_resolve: &mut Vec<Loader<'a, 'b, 'c, 'd>>,
    errors: &mut Vec<Error>
) {
    loop {
        let mut modified = false;
        let mut finished_on_round = Vec::new();
        for mut target in to_resolve.drain(0..to_resolve.len()) {
            modified |= target.load(completed, errors);
            if target.is_solved() {
                completed.push(target);
            } else {
                finished_on_round.push(target);
            }
        }
        to_resolve.append(&mut finished_on_round);
        if to_resolve.is_empty() {
            return;
        }
        if !modified {
            for loader in to_resolve.iter() {
                loader.collect_errors(to_resolve, errors);
            }
            return;
        }
    }
}

/// Parses the specified input stream
fn parse_input_stream<'a>(
    content: Box<dyn Read + 'a>,
    input_index: usize
) -> Result<ParseResult<'static, 'static>, (ParseResult<'static, 'static>, Vec<Error>)> {
    let mut reader = io::BufReader::new(content);
    let result = hime_grammar::parse_utf8(&mut reader);
    let errors: Vec<Error> = result
        .errors
        .errors
        .iter()
        .map(|error| {
            let position = error.get_position();
            Error::Parsing(
                InputReference {
                    input_index,
                    position,
                    length: error.get_length()
                },
                error.get_message()
            )
        })
        .collect();
    if errors.is_empty() {
        Ok(result)
    } else {
        Err((result, errors))
    }
}

/// Parses all inputs
fn parse_inputs(inputs: Vec<LoadInput>) -> Result<(Vec<String>, Vec<ParseResult>), Errors> {
    let mut names = Vec::new();
    let mut results = Vec::new();
    let mut has_errors = false;
    let mut errors = Vec::new();
    for (index, input) in inputs.into_iter().enumerate() {
        names.push(input.0);
        match parse_input_stream(input.1, index) {
            Ok(result) => {
                results.push(result);
            }
            Err((result, mut sub_errors)) => {
                results.push(result);
                has_errors = true;
                errors.append(&mut sub_errors)
            }
        }
    }
    if !has_errors {
        Ok((names, results))
    } else {
        Err(Errors::from(
            build_loaded_data(names, results, Vec::new()),
            errors
        ))
    }
}

/// Represents a loader for a grammar
struct Loader<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> {
    /// index of the input
    input_index: usize,
    /// The parse result
    root: AstNode<'a, 'b, 'c, 'd>,
    /// Lists of the inherited grammars
    inherited: Vec<String>,
    /// The resulting grammar
    grammar: Grammar,
    /// Flag for the global casing of the grammar
    case_insensitive: bool
}

impl<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> Loader<'a, 'b, 'c, 'd> {
    /// Creates a new loader
    fn new(
        input_index: usize,
        root: AstNode<'a, 'b, 'c, 'd>,
        errors: &mut Vec<Error>
    ) -> Loader<'a, 'b, 'c, 'd> {
        let input_ref = InputReference::from(input_index, &root.child(0));
        let name = root.child(0).get_value().unwrap();
        let inherited = root
            .child(1)
            .into_iter()
            .map(|node| node.get_value().unwrap())
            .collect();
        let mut loader = Loader {
            input_index,
            root,
            inherited,
            grammar: Grammar::new(input_ref, name),
            case_insensitive: false
        };
        if loader.is_solved() {
            loader.load_content(errors);
        }
        loader
    }

    /// Prints errors for the unresolved inherited grammars
    fn collect_errors(&self, unresolved: &[Loader], errors: &mut Vec<Error>) {
        for node in self.root.child(1) {
            let name = node.get_value().unwrap();
            if self.inherited.contains(&name) {
                // was not resolved
                if unresolved.iter().all(|l| l.grammar.name != name) {
                    // the dependency does not exist
                    errors.push(Error::GrammarNotDefined(
                        InputReference::from(self.input_index, &node),
                        name
                    ));
                }
            }
        }
    }

    /// Gets a value indicating whether all dependencies are solved
    fn is_solved(&self) -> bool {
        self.inherited.is_empty()
    }

    /// Attempts to load data for the grammar
    fn load(&mut self, completed: &[Loader], errors: &mut Vec<Error>) -> bool {
        let mut modified = false;
        let grammar = &mut self.grammar;
        self.inherited.retain(|parent| {
            if let Some(loader) = completed.iter().find(|l| &l.grammar.name == parent) {
                grammar.inherit(&loader.grammar);
                modified = true;
                false
            } else {
                // not found => retain
                true
            }
        });
        if self.is_solved() {
            self.load_content(errors);
        }
        modified
    }

    /// Loads the content of the grammar
    fn load_content(&mut self, errors: &mut Vec<Error>) {
        for node in self.root {
            let id = node.get_symbol().id;
            match id {
                hime_grammar::ID_TERMINAL_BLOCK_OPTIONS => {
                    load_options(self.input_index, &mut self.grammar, node);
                    if let Some(option) = self.grammar.options.get("CaseSensitive") {
                        if &option.value == "false" {
                            self.case_insensitive = true;
                        }
                    }
                }
                hime_grammar::ID_TERMINAL_BLOCK_TERMINALS => {
                    load_terminals(self.input_index, errors, &mut self.grammar, node);
                }
                hime_grammar::ID_TERMINAL_BLOCK_RULES => {
                    load_rules(self.input_index, errors, &mut self.grammar, node);
                }
                hime_grammar::ID_TERMINAL_NAME => {}
                hime_grammar::ID_VARIABLE_GRAMMAR_PARENCY => {}
                _ => {
                    panic!("Unrecognized symbol: {}", node.get_symbol().name);
                }
            }
        }
    }
}

/// Loads the options block of a grammar
fn load_options(input_index: usize, grammar: &mut Grammar, node: AstNode) {
    for child in node {
        load_option(input_index, grammar, child);
    }
}

/// Loads the grammar option in the given AST
fn load_option(input_index: usize, grammar: &mut Grammar, node: AstNode) {
    let name = node.child(0).get_value().unwrap();
    let value = replace_escapees(node.child(1).get_value().unwrap());
    let value = value[1..(value.len() - 1)].to_string();
    grammar.add_option(
        InputReference::from(input_index, &node.child(0)),
        InputReference::from(input_index, &node.child(1)),
        name,
        value
    );
}

/// Loads the terminal blocks of a grammar
fn load_terminals(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    node: AstNode
) {
    for child in node {
        let id = child.get_symbol().id;
        if id == hime_grammar::ID_TERMINAL_BLOCK_CONTEXT {
            load_terminal_rule_context(input_index, errors, grammar, child);
        } else if id == hime_grammar::ID_VARIABLE_TERMINAL_FRAGMENT {
            load_terminal_rule(
                input_index,
                errors,
                grammar,
                child,
                DEFAULT_CONTEXT_NAME,
                true
            );
        } else if id == hime_grammar::ID_VARIABLE_TERMINAL_RULE {
            load_terminal_rule(
                input_index,
                errors,
                grammar,
                child,
                DEFAULT_CONTEXT_NAME,
                false
            );
        } else {
            panic!("Unrecognized symbol: {}", node.get_symbol().name);
        }
    }
}

/// Loads the terminal context in the given AST
fn load_terminal_rule_context(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    node: AstNode
) {
    let name = node.child(0).get_value().unwrap();
    grammar.resolve_context(&name);
    for child in node.into_iter().skip(1) {
        load_terminal_rule(input_index, errors, grammar, child, &name, false);
    }
}

/// Loads the terminal rule in the given AST
fn load_terminal_rule(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    node: AstNode,
    context: &str,
    is_fragment: bool
) {
    let node_name = node.child(0);
    let name = node_name.get_value().unwrap();
    if grammar.get_terminal_for_name(&name).is_some() {
        errors.push(Error::OverridingPreviousTerminal(
            InputReference::from(input_index, &node_name),
            name
        ));
        return;
    }
    let nfa = load_nfa(input_index, errors, grammar, node.child(1));
    let terminal = grammar.add_terminal_named(
        name,
        InputReference::from(input_index, &node_name),
        nfa,
        context,
        is_fragment
    );
    terminal.nfa.states[terminal.nfa.exit]
        .add_item(FinalItem::Terminal(terminal.id, terminal.context));
}

/// Builds the NFA represented by the AST node
fn load_nfa(input_index: usize, errors: &mut Vec<Error>, grammar: &Grammar, node: AstNode) -> NFA {
    match node.get_symbol().id {
        hime_grammar::ID_TERMINAL_LITERAL_TEXT => load_nfa_simple_text(&node),
        hime_grammar::ID_TERMINAL_UNICODE_CODEPOINT => {
            load_nfa_codepoint(input_index, errors, node)
        }
        hime_grammar::ID_TERMINAL_LITERAL_CLASS => load_nfa_class(input_index, errors, node),
        hime_grammar::ID_TERMINAL_UNICODE_CATEGORY => {
            load_nfa_unicode_category(input_index, errors, node)
        }
        hime_grammar::ID_TERMINAL_UNICODE_BLOCK => {
            load_nfa_unicode_block(input_index, errors, node)
        }
        hime_grammar::ID_TERMINAL_UNICODE_SPAN_MARKER => {
            load_nfa_unicode_span(input_index, errors, node)
        }
        hime_grammar::ID_TERMINAL_LITERAL_ANY => load_nfa_any(),
        hime_grammar::ID_TERMINAL_NAME => load_nfa_reference(input_index, errors, grammar, node),
        hime_grammar::ID_TERMINAL_OPERATOR_OPTIONAL => {
            let inner = load_nfa(input_index, errors, grammar, node.child(0));
            inner.into_optional()
        }
        hime_grammar::ID_TERMINAL_OPERATOR_ZEROMORE => {
            let inner = load_nfa(input_index, errors, grammar, node.child(0));
            inner.into_zero_or_more()
        }
        hime_grammar::ID_TERMINAL_OPERATOR_ONEMORE => {
            let inner = load_nfa(input_index, errors, grammar, node.child(0));
            inner.into_one_or_more()
        }
        hime_grammar::ID_TERMINAL_OPERATOR_UNION => {
            let left = load_nfa(input_index, errors, grammar, node.child(0));
            let right = load_nfa(input_index, errors, grammar, node.child(1));
            left.into_union_with(right)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_DIFFERENCE => {
            let left = load_nfa(input_index, errors, grammar, node.child(0));
            let right = load_nfa(input_index, errors, grammar, node.child(1));
            left.into_difference(right)
        }
        hime_grammar::ID_VIRTUAL_RANGE => {
            let inner = load_nfa(input_index, errors, grammar, node.child(0));
            let min = node.child(1).get_value().unwrap().parse::<usize>().unwrap();
            let max = if node.children_count() > 2 {
                node.child(2).get_value().unwrap().parse::<usize>().unwrap()
            } else {
                min
            };
            inner.into_repeat_range(min, max)
        }
        hime_grammar::ID_VIRTUAL_CONCAT => {
            let left = load_nfa(input_index, errors, grammar, node.child(0));
            let right = load_nfa(input_index, errors, grammar, node.child(1));
            left.into_concatenation(right)
        }
        _ => {
            panic!("Unrecognized symbol: {}", node.get_symbol().name)
        }
    }
}

/// Builds a NFA from a piece of text
fn load_nfa_simple_text(node: &AstNode) -> NFA {
    // build the raw piece of text
    let mut value = node.get_value().unwrap();
    let mut insensitive = false;
    if value.starts_with('~') {
        insensitive = true;
        value = value[2..(value.len() - 1)].to_string();
    } else {
        value = value[1..(value.len() - 1)].to_string();
    }
    let value = replace_escapees(value);

    // build the result
    let mut nfa = NFA::new_minimal();
    let mut buffer = [0; 2];
    nfa.exit = nfa.entry;
    for c in value.chars() {
        if insensitive && c.is_ascii_alphabetic() {
            let c2 = if c.is_ascii_lowercase() {
                c.to_ascii_uppercase()
            } else {
                c.to_ascii_lowercase()
            };
            let temp = nfa.add_state().id;
            let encoded1 = c.encode_utf16(&mut buffer)[0];
            let encoded2 = c2.encode_utf16(&mut buffer)[0];
            nfa.add_transition(nfa.exit, CharSpan::new(encoded1, encoded1), temp);
            nfa.add_transition(nfa.exit, CharSpan::new(encoded2, encoded2), temp);
            nfa.exit = temp;
        } else {
            for encoded in c.encode_utf16(&mut buffer).iter() {
                let temp = nfa.add_state().id;
                nfa.add_transition(nfa.exit, CharSpan::new(*encoded, *encoded), temp);
                nfa.exit = temp;
            }
        }
    }
    nfa
}

/// Builds a NFA from a unicode code point
fn load_nfa_codepoint(input_index: usize, errors: &mut Vec<Error>, node: AstNode) -> NFA {
    // extract the code point value
    let value = node.get_value().unwrap();
    let value = u32::from_str_radix(&value[2..], 16).unwrap();
    let value = match std::char::from_u32(value) {
        Some(v) => v,
        None => {
            errors.push(Error::InvalidCodePoint(
                InputReference::from(input_index, &node),
                value
            ));
            return NFA::new_minimal();
        }
    };
    // build the NFA
    let mut nfa = NFA::new_minimal();
    let mut buffer = [0; 2];
    nfa.exit = nfa.entry;
    for encoded in value.encode_utf16(&mut buffer).iter() {
        let temp = nfa.add_state().id;
        nfa.add_transition(nfa.exit, CharSpan::new(*encoded, *encoded), temp);
        nfa.exit = temp;
    }
    nfa
}

/// Builds a NFA from a character class
fn load_nfa_class(input_index: usize, errors: &mut Vec<Error>, node: AstNode) -> NFA {
    // extract the value
    let node_value = node.get_value().unwrap();
    let value = &node_value[1..(node_value.len() - 1)];
    let mut positive = true;
    let chars: Vec<char> = if value.starts_with('^') {
        positive = false;
        value.chars().skip(1).collect()
    } else {
        value.chars().collect()
    };
    let mut spans = Vec::new();
    let mut i = 0;
    while i < chars.len() {
        let (b, l) = get_char_value(&chars, i);
        let b = b as usize;
        i += l;
        if b >= 0xFFFF {
            errors.push(Error::UnsupportedNonPlane0InCharacterClass(
                InputReference::from(input_index, &node.clone()),
                b
            ));
        }
        if i + 2 <= chars.len() && chars[i] == '-' {
            // this is range, match the -
            i += 1;
            let (e, l2) = get_char_value(&chars, i);
            let e = e as usize;
            if b >= 0xFFFF {
                errors.push(Error::UnsupportedNonPlane0InCharacterClass(
                    InputReference::from(input_index, &node.clone()),
                    e
                ));
            }
            i += l2;
            if b < 0x8D00 && e > 0xDFFF {
                // oooh you ...
                spans.push(CharSpan::new(b as u16, 0xD7FF));
                spans.push(CharSpan::new(0xE000, e as u16));
            } else {
                spans.push(CharSpan::new(b as u16, e as u16));
            }
        } else {
            // this is a normal character
            spans.push(CharSpan::new(b as u16, b as u16));
        }
    }
    let mut nfa = NFA::new_minimal();
    if positive {
        for span in spans.into_iter() {
            nfa.add_transition(nfa.entry, span, nfa.exit);
        }
    } else {
        spans.sort();
        // TODO: Check for span intersections and overflow of b (when a span ends on 0xFFFF)
        let mut b = 0;
        for span in spans.into_iter() {
            if span.begin > b {
                nfa.add_transition(nfa.entry, CharSpan::new(b, span.begin - 1), nfa.exit);
            }
            b = span.end + 1;
            // skip the surrogate encoding points
            if (0xD800..=0xDFFF).contains(&b) {
                b = 0xE000;
            }
        }
        if b <= 0xD700 {
            nfa.add_transition(nfa.entry, CharSpan::new(b, 0xD7FF), nfa.exit);
            nfa.add_transition(nfa.entry, CharSpan::new(0xE000, 0xFFFF), nfa.exit);
        } else if b != 0xFFFF {
            // here b >= 0xE000
            nfa.add_transition(nfa.entry, CharSpan::new(b, 0xFFFF), nfa.exit);
        }
        // surrogate pairs
        let inter = nfa.add_state().id;
        nfa.add_transition(nfa.entry, CharSpan::new(0xD800, 0xDEFF), inter);
        nfa.add_transition(inter, CharSpan::new(0xDC00, 0xDFFF), nfa.exit);
    }
    nfa
}

/// Builds a NFA from a unicode category
fn load_nfa_unicode_category(input_index: usize, errors: &mut Vec<Error>, node: AstNode) -> NFA {
    // extract the value
    let node_value = node.get_value().unwrap();
    let value = &node_value[3..(node_value.len() - 1)];
    match CATEGORIES.get(value) {
        Some(category) => {
            let mut nfa = NFA::new_minimal();
            for span in category.spans.iter() {
                add_unicode_span_to_nfa(&mut nfa, *span);
            }
            nfa
        }
        None => {
            errors.push(Error::UnknownUnicodeCategory(
                InputReference::from(input_index, &node),
                value.to_string()
            ));
            NFA::new_minimal()
        }
    }
}

/// Builds a NFA from a unicode block
fn load_nfa_unicode_block(input_index: usize, errors: &mut Vec<Error>, node: AstNode) -> NFA {
    // extract the value
    let node_value = node.get_value().unwrap();
    let value = &node_value[3..(node_value.len() - 1)];
    match BLOCKS.get(value) {
        Some(block) => {
            let mut nfa = NFA::new_minimal();
            add_unicode_span_to_nfa(&mut nfa, block.span);
            nfa
        }
        None => {
            errors.push(Error::UnknownUnicodeBlock(
                InputReference::from(input_index, &node),
                value.to_string()
            ));
            NFA::new_minimal()
        }
    }
}

/// Builds a NFA from a unicode span
fn load_nfa_unicode_span(input_index: usize, errors: &mut Vec<Error>, node: AstNode) -> NFA {
    // extract the value
    let begin = node.child(0).get_value().unwrap();
    let begin = u32::from_str_radix(&begin[2..], 16).unwrap();
    let end = node.child(1).get_value().unwrap();
    let end = u32::from_str_radix(&end[2..], 16).unwrap();
    if begin > end {
        errors.push(Error::InvalidCharacterSpan(InputReference::from(
            input_index,
            &node
        )));
        return NFA::new_minimal();
    }
    let mut nfa = NFA::new_minimal();
    add_unicode_span_to_nfa(&mut nfa, Span::new(begin, end));
    nfa
}

/// Builds a NFA that matches everything (a single character)
fn load_nfa_any() -> NFA {
    let mut nfa = NFA::new_minimal();
    // plane 0 transitions
    nfa.add_transition(nfa.entry, CharSpan::new(0x0000, 0xD7FF), nfa.exit);
    nfa.add_transition(nfa.entry, CharSpan::new(0xE000, 0xFFFF), nfa.exit);
    // surrogate pairs
    let intermediate = nfa.add_state().id;
    nfa.add_transition(nfa.entry, CharSpan::new(0xD800, 0xDBFF), intermediate);
    nfa.add_transition(intermediate, CharSpan::new(0xDC00, 0xDFFF), nfa.exit);
    nfa
}

/// Builds a NFA from a referenced terminal
fn load_nfa_reference(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &Grammar,
    node: AstNode
) -> NFA {
    let value = node.get_value().unwrap();
    match grammar.get_terminal_for_name(&value) {
        Some(terminal) => terminal.nfa.clone_no_finals(),
        None => {
            errors.push(Error::SymbolNotFound(
                InputReference::from(input_index, &node),
                value
            ));
            NFA::new_minimal()
        }
    }
}

/// Adds a unicode character span to an existing NFA automaton
fn add_unicode_span_to_nfa(nfa: &mut NFA, span: Span) {
    let b = span.begin.get_utf16();
    let e = span.end.get_utf16();
    if span.is_plane0() {
        // this span is entirely in plane 0
        nfa.add_transition(nfa.entry, CharSpan::new(b[0], e[0]), nfa.exit);
    } else if span.begin.is_plane0() {
        // this span has only a part in plane 0
        if b[0] < 0xD800 {
            nfa.add_transition(nfa.entry, CharSpan::new(b[0], 0xD7FF), nfa.exit);
            nfa.add_transition(nfa.entry, CharSpan::new(0xE000, 0xFFFF), nfa.exit);
        } else {
            nfa.add_transition(nfa.entry, CharSpan::new(b[0], 0xFFFF), nfa.exit);
        }
        let intermediate = nfa.add_state().id;
        nfa.add_transition(nfa.entry, CharSpan::new(0xD800, e[0]), intermediate);
        nfa.add_transition(intermediate, CharSpan::new(0xDC00, e[1]), nfa.exit);
    } else {
        // there is at least one surrogate value between the first surrogates of begin and end
        // build lower part
        let ia = nfa.add_state().id;
        nfa.add_transition(nfa.entry, CharSpan::new(b[0], b[0]), ia);
        nfa.add_transition(ia, CharSpan::new(b[1], 0xDFFF), nfa.exit);
        // build intermediate part
        let im = nfa.add_state().id;
        nfa.add_transition(nfa.entry, CharSpan::new(b[0] + 1, e[0] - 1), im);
        nfa.add_transition(im, CharSpan::new(0xDC00, 0xDFFF), nfa.exit);
        // build upper part
        let iz = nfa.add_state().id;
        nfa.add_transition(nfa.entry, CharSpan::new(e[0], e[0]), iz);
        nfa.add_transition(iz, CharSpan::new(0xDC00, e[1]), nfa.exit);
    }
}

/// Loads the rules block of a grammar
fn load_rules(input_index: usize, errors: &mut Vec<Error>, grammar: &mut Grammar, node: AstNode) {
    // load new variables for the rule's head
    for child in node {
        let id = child.get_symbol().id;
        if id == hime_grammar::ID_VARIABLE_CF_RULE_SIMPLE {
            let name = child.child(0).get_value().unwrap();
            grammar.add_variable(&name);
        } else if id == hime_grammar::ID_VARIABLE_CF_RULE_TEMPLATE {
            let name = child.child(0).get_value().unwrap();
            let arguments: Vec<String> = child
                .child(1)
                .into_iter()
                .map(|n| n.get_value().unwrap())
                .collect();
            grammar.add_template_rule(&name, arguments);
        } else {
            panic!("Unrecognized symbol: {}", node.get_symbol().name);
        }
    }
    // load template rules
    for child in node {
        let id = child.get_symbol().id;
        if id == hime_grammar::ID_VARIABLE_CF_RULE_TEMPLATE {
            load_template_rule(input_index, errors, grammar, child);
        }
    }
    // load simple rules
    for child in node {
        let id = child.get_symbol().id;
        if id == hime_grammar::ID_VARIABLE_CF_RULE_SIMPLE {
            load_simple_rule(input_index, errors, grammar, child);
        }
    }
}

/// Loads the syntactic rule in the given AST
fn load_simple_rule(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    node: AstNode
) {
    let name = node.child(0).get_value().unwrap();
    let head_sid = grammar.add_variable(&name).id;
    let definitions =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(1));
    let variable = grammar.add_variable(&name);
    for body in definitions.bodies.into_iter() {
        variable.add_rule(Rule::new(
            variable.id,
            TREE_ACTION_NONE,
            body,
            DEFAULT_CONTEXT as usize
        ));
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_definitions(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    match node.get_symbol().id {
        hime_grammar::ID_VARIABLE_RULE_DEF_CONTEXT => {
            load_simple_rule_context(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_VARIABLE_RULE_DEF_SUB => {
            load_simple_rule_sub_rule(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_OPTIONAL => {
            load_simple_rule_optional(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_ZEROMORE => {
            load_simple_rule_zero_or_more(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_ONEMORE => {
            load_simple_rule_one_or_more(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_UNION => {
            load_simple_rule_union(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_TERMINAL_TREE_ACTION_PROMOTE => {
            load_simple_rule_tree_action_promote(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_TERMINAL_TREE_ACTION_DROP => {
            load_simple_rule_tree_action_drop(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_VIRTUAL_CONCAT => {
            load_simple_rule_concat(input_index, errors, grammar, head_sid, node)
        }
        hime_grammar::ID_VIRTUAL_EMPTYPART => load_simple_rule_empty_part(),
        _ => load_simple_rule_atomic(input_index, errors, grammar, node)
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_context(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    let name = node.child(0).get_value().unwrap();
    let context_id = grammar.resolve_context(&name);
    let definitions =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(1));
    let sub_var = grammar.generate_variable(head_sid);
    for body in definitions.bodies.into_iter() {
        sub_var.add_rule(Rule::new(
            sub_var.id,
            TREE_ACTION_REPLACE_BY_CHILDREN,
            body,
            context_id
        ));
    }
    BodySet {
        bodies: vec![RuleBody::single(
            SymbolRef::Variable(sub_var.id),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_sub_rule(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    let definitions =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(0));
    let sub_var = grammar.generate_variable(head_sid);
    for body in definitions.bodies.into_iter() {
        sub_var.add_rule(Rule::new(
            sub_var.id,
            TREE_ACTION_REPLACE_BY_EPSILON,
            body,
            DEFAULT_CONTEXT as usize
        ));
    }
    BodySet {
        bodies: vec![RuleBody::single(
            SymbolRef::Variable(sub_var.id),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_optional(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    let mut definitions =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(0));
    definitions.bodies.push(RuleBody::empty());
    definitions
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_zero_or_more(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    // get definitions
    let set_inner =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(0));
    // generate the sub variables
    let sub_var = grammar.generate_variable(head_sid);
    // build all rules
    // sub_var -> definition
    for body in set_inner.bodies.iter() {
        sub_var.add_rule(Rule::new(
            sub_var.id,
            TREE_ACTION_REPLACE_BY_CHILDREN,
            body.clone(),
            DEFAULT_CONTEXT as usize
        ));
    }
    // Produce single defition [sub_var]
    let set_var = BodySet {
        bodies: vec![RuleBody::single(
            SymbolRef::Variable(sub_var.id),
            InputReference::from(input_index, &node)
        )]
    };
    let set_var = BodySet::product(set_var, set_inner);
    // build all rules
    // sub_var -> sub_var definition
    for body in set_var.bodies.into_iter() {
        sub_var.add_rule(Rule::new(
            sub_var.id,
            TREE_ACTION_REPLACE_BY_CHILDREN,
            body,
            DEFAULT_CONTEXT as usize
        ));
    }
    BodySet {
        bodies: vec![
            RuleBody::empty(),
            RuleBody::single(
                SymbolRef::Variable(sub_var.id),
                InputReference::from(input_index, &node)
            ),
        ]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_one_or_more(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    // get definitions
    let set_inner =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(0));
    // generate the sub variables
    let sub_var = grammar.generate_variable(head_sid);
    // build all rules
    // sub_var -> definition
    for body in set_inner.bodies.iter() {
        sub_var.add_rule(Rule::new(
            sub_var.id,
            TREE_ACTION_REPLACE_BY_CHILDREN,
            body.clone(),
            DEFAULT_CONTEXT as usize
        ));
    }
    // Produce single defition [sub_var]
    let set_var = BodySet {
        bodies: vec![RuleBody::single(
            SymbolRef::Variable(sub_var.id),
            InputReference::from(input_index, &node)
        )]
    };
    let set_var = BodySet::product(set_var, set_inner);
    // build all rules
    // sub_var -> sub_var definition
    for body in set_var.bodies.into_iter() {
        sub_var.add_rule(Rule::new(
            sub_var.id,
            TREE_ACTION_REPLACE_BY_CHILDREN,
            body,
            DEFAULT_CONTEXT as usize
        ));
    }
    BodySet {
        bodies: vec![RuleBody::single(
            SymbolRef::Variable(sub_var.id),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_union(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    let set_left =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(0));
    let set_right =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(1));
    BodySet::union(set_left, set_right)
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_tree_action_promote(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    let mut set_inner =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(0));
    set_inner.apply_action(TREE_ACTION_PROMOTE);
    set_inner
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_tree_action_drop(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    let mut set_inner =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(0));
    set_inner.apply_action(TREE_ACTION_DROP);
    set_inner
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_concat(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    head_sid: usize,
    node: AstNode
) -> BodySet<RuleBody> {
    let set_left =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(0));
    let set_right =
        load_simple_rule_definitions(input_index, errors, grammar, head_sid, node.child(1));
    BodySet::product(set_left, set_right)
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_empty_part() -> BodySet<RuleBody> {
    BodySet {
        bodies: vec![RuleBody::empty()]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_simple_rule_atomic(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<RuleBody> {
    match node.get_symbol().id {
        hime_grammar::ID_VARIABLE_RULE_SYM_ACTION => {
            load_simple_rule_atomic_action(input_index, grammar, node)
        }
        hime_grammar::ID_VARIABLE_RULE_SYM_VIRTUAL => {
            load_simple_rule_atomic_virtual(input_index, grammar, node)
        }
        hime_grammar::ID_VARIABLE_RULE_SYM_REF_SIMPLE => {
            load_simple_rule_atomic_simple_ref(input_index, errors, grammar, node)
        }
        hime_grammar::ID_VARIABLE_RULE_SYM_REF_TEMPLATE => {
            load_simple_rule_atomic_template_ref(input_index, errors, grammar, node)
        }
        hime_grammar::ID_TERMINAL_LITERAL_TEXT => {
            load_simple_rule_atomic_inline_text(input_index, grammar, node)
        }
        _ => {
            panic!("Unrecognized symbol: {}", node.get_symbol().name)
        }
    }
}

/// Builds the set of rule definitions that represents a single semantic action
fn load_simple_rule_atomic_action(
    input_index: usize,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<RuleBody> {
    let name = node.child(0).get_value().unwrap();
    let id = grammar.add_action(&name).id;
    BodySet {
        bodies: vec![RuleBody::single(
            SymbolRef::Action(id),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that represents a single virtual symbol
fn load_simple_rule_atomic_virtual(
    input_index: usize,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<RuleBody> {
    let name = node.child(0).get_value().unwrap();
    let name = replace_escapees(name[1..(name.len() - 1)].to_string());
    let id = grammar.add_virtual(&name).id;
    BodySet {
        bodies: vec![RuleBody::single(
            SymbolRef::Virtual(id),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that represents a single reference to a simple variable
fn load_simple_rule_atomic_simple_ref(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<RuleBody> {
    let name = node.child(0).get_value().unwrap();
    match grammar.get_symbol(&name) {
        Some(symbol_ref) => BodySet {
            bodies: vec![RuleBody::single(
                symbol_ref,
                InputReference::from(input_index, &node)
            )]
        },
        None => {
            errors.push(Error::SymbolNotFound(
                InputReference::from(input_index, &node.child(0)),
                name
            ));
            BodySet { bodies: Vec::new() }
        }
    }
}

/// Builds the set of rule definitions that represents a single reference to a template variable
fn load_simple_rule_atomic_template_ref(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<RuleBody> {
    let name = node.child(0).get_value().unwrap();
    let arguments: Vec<SymbolRef> = node
        .child(1)
        .into_iter()
        .map(|n| {
            load_simple_rule_atomic(input_index, errors, grammar, n).bodies[0].elements[0].symbol
        })
        .collect();
    let symbol_ref = match grammar.instantiate_template_rule(
        &name,
        InputReference::from(input_index, &node.child(0)),
        arguments
    ) {
        Ok(symbol_ref) => symbol_ref,
        Err(err) => {
            errors.push(err);
            return BodySet { bodies: Vec::new() };
        }
    };
    BodySet {
        bodies: vec![RuleBody::single(
            symbol_ref,
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that represents a single inline piece of text
fn load_simple_rule_atomic_inline_text(
    input_index: usize,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<RuleBody> {
    // Construct the terminal name
    let value = node.get_value().unwrap();
    let start = if value.starts_with('~') { 2 } else { 1 };
    let value = replace_escapees(value[start..(value.len() - 1)].to_string());
    // Check for previous instance in the grammar
    let id = match grammar.get_terminal_for_value(&value) {
        None => {
            // Create the terminal
            let nfa = load_nfa_simple_text(&node);
            let terminal = grammar.add_terminal_anonymous(
                value,
                InputReference::from(input_index, &node),
                nfa
            );
            terminal.nfa.states[terminal.nfa.exit]
                .add_item(FinalItem::Terminal(terminal.id, terminal.context));
            terminal.id
        }
        Some(terminal) => terminal.id
    };
    // Create the definition set
    BodySet {
        bodies: vec![RuleBody::single(
            SymbolRef::Terminal(id),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Loads the syntactic rule in the given AST
fn load_template_rule(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    node: AstNode
) {
    let name = node.child(0).get_value().unwrap();
    let template_index = grammar
        .template_rules
        .iter()
        .position(|r| r.name == name)
        .unwrap();
    let parameters = grammar.template_rules[template_index].parameters.clone();
    let definitions =
        load_template_rule_definitions(input_index, errors, grammar, &parameters, node.child(2));
    grammar.template_rules[template_index].bodies = definitions.bodies;
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_definitions(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    match node.get_symbol().id {
        hime_grammar::ID_VARIABLE_RULE_DEF_CONTEXT => {
            load_template_rule_context(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_VARIABLE_RULE_DEF_SUB => {
            load_template_rule_sub_rule(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_OPTIONAL => {
            load_template_rule_optional(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_ZEROMORE => {
            load_template_rule_zero_or_more(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_ONEMORE => {
            load_template_rule_one_or_more(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_TERMINAL_OPERATOR_UNION => {
            load_template_rule_union(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_TERMINAL_TREE_ACTION_PROMOTE => {
            load_template_rule_tree_action_promote(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_TERMINAL_TREE_ACTION_DROP => {
            load_template_rule_tree_action_drop(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_VIRTUAL_CONCAT => {
            load_template_rule_concat(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_VIRTUAL_EMPTYPART => load_template_rule_empty_part(),
        _ => load_template_rule_atomic(input_index, errors, grammar, parameters, node)
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_context(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let name = node.child(0).get_value().unwrap();
    let context_id = grammar.resolve_context(&name);
    let definitions =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(1));

    let template_index = grammar.template_rules.len();
    let sub_template = grammar.generate_template_rule(parameters.to_vec());
    sub_template.head_action = TREE_ACTION_REPLACE_BY_CHILDREN;
    sub_template.context = context_id;
    sub_template.bodies = definitions.bodies;
    BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Template(TemplateRuleRef {
                template: template_index,
                arguments: (0..parameters.len())
                    .map(TemplateRuleSymbol::Parameter)
                    .collect()
            }),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_sub_rule(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let definitions =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(0));
    let template_index = grammar.template_rules.len();
    let sub_template = grammar.generate_template_rule(parameters.to_vec());
    sub_template.head_action = TREE_ACTION_REPLACE_BY_EPSILON;
    sub_template.bodies = definitions.bodies;
    BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Template(TemplateRuleRef {
                template: template_index,
                arguments: (0..parameters.len())
                    .map(TemplateRuleSymbol::Parameter)
                    .collect()
            }),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_optional(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let mut definitions =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(0));
    definitions.bodies.push(TemplateRuleBody::empty());
    definitions
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_zero_or_more(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    // get definitions
    let set_inner =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(0));
    // generate the sub variables
    let template_index = grammar.template_rules.len();
    let sub_template = grammar.generate_template_rule(parameters.to_vec());
    sub_template.head_action = TREE_ACTION_REPLACE_BY_CHILDREN;
    // build all rules
    // sub_var -> definition
    for body in set_inner.bodies.iter() {
        sub_template.bodies.push(body.clone());
    }
    // Produce single defition [sub_var]
    let set_var = BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Template(TemplateRuleRef {
                template: template_index,
                arguments: (0..parameters.len())
                    .map(TemplateRuleSymbol::Parameter)
                    .collect()
            }),
            InputReference::from(input_index, &node)
        )]
    };
    let set_var = BodySet::product(set_var, set_inner);
    // build all rules
    // sub_var -> sub_var definition
    for body in set_var.bodies.into_iter() {
        sub_template.bodies.push(body);
    }
    BodySet {
        bodies: vec![
            TemplateRuleBody::empty(),
            TemplateRuleBody::single(
                TemplateRuleSymbol::Template(TemplateRuleRef {
                    template: template_index,
                    arguments: (0..parameters.len())
                        .map(TemplateRuleSymbol::Parameter)
                        .collect()
                }),
                InputReference::from(input_index, &node)
            ),
        ]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_one_or_more(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    // get definitions
    let set_inner =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(0));
    // generate the sub variables
    let template_index = grammar.template_rules.len();
    let sub_template = grammar.generate_template_rule(parameters.to_vec());
    sub_template.head_action = TREE_ACTION_REPLACE_BY_CHILDREN;
    // build all rules
    // sub_var -> definition
    for body in set_inner.bodies.iter() {
        sub_template.bodies.push(body.clone());
    }
    // Produce single defition [sub_var]
    let set_var = BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Template(TemplateRuleRef {
                template: template_index,
                arguments: (0..parameters.len())
                    .map(TemplateRuleSymbol::Parameter)
                    .collect()
            }),
            InputReference::from(input_index, &node)
        )]
    };
    let set_var = BodySet::product(set_var, set_inner);
    // build all rules
    // sub_var -> sub_var definition
    for body in set_var.bodies.into_iter() {
        sub_template.bodies.push(body);
    }
    BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Template(TemplateRuleRef {
                template: template_index,
                arguments: (0..parameters.len())
                    .map(TemplateRuleSymbol::Parameter)
                    .collect()
            }),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_union(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let set_left =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(0));
    let set_right =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(1));
    BodySet::union(set_left, set_right)
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_tree_action_promote(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let mut set_inner =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(0));
    set_inner.apply_action(TREE_ACTION_PROMOTE);
    set_inner
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_tree_action_drop(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let mut set_inner =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(0));
    set_inner.apply_action(TREE_ACTION_DROP);
    set_inner
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_concat(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let set_left =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(0));
    let set_right =
        load_template_rule_definitions(input_index, errors, grammar, parameters, node.child(1));
    BodySet::product(set_left, set_right)
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_empty_part() -> BodySet<TemplateRuleBody> {
    BodySet {
        bodies: vec![TemplateRuleBody::empty()]
    }
}

/// Builds the set of rule definitions that are represented by the given AST
fn load_template_rule_atomic(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    match node.get_symbol().id {
        hime_grammar::ID_VARIABLE_RULE_SYM_ACTION => {
            load_template_rule_atomic_action(input_index, grammar, node)
        }
        hime_grammar::ID_VARIABLE_RULE_SYM_VIRTUAL => {
            load_template_rule_atomic_virtual(input_index, grammar, node)
        }
        hime_grammar::ID_VARIABLE_RULE_SYM_REF_SIMPLE => {
            load_template_rule_atomic_simple_ref(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_VARIABLE_RULE_SYM_REF_TEMPLATE => {
            load_template_rule_atomic_template_ref(input_index, errors, grammar, parameters, node)
        }
        hime_grammar::ID_TERMINAL_LITERAL_TEXT => {
            load_template_rule_atomic_inline_text(input_index, grammar, node)
        }
        _ => {
            panic!("Unrecognized symbol: {}", node.get_symbol().name);
        }
    }
}

/// Builds the set of rule definitions that represents a single semantic action
fn load_template_rule_atomic_action(
    input_index: usize,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let name = node.child(0).get_value().unwrap();
    let id = grammar.add_action(&name).id;
    BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Symbol(SymbolRef::Action(id)),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that represents a single virtual symbol
fn load_template_rule_atomic_virtual(
    input_index: usize,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let name = node.child(0).get_value().unwrap();
    let name = replace_escapees(name[1..(name.len() - 1)].to_string());
    let id = grammar.add_virtual(&name).id;
    BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Symbol(SymbolRef::Virtual(id)),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that represents a single reference to a simple variable
fn load_template_rule_atomic_simple_ref(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let name = node.child(0).get_value().unwrap();
    if let Some(index) = parameters.iter().position(|p| p == &name) {
        return BodySet {
            bodies: vec![TemplateRuleBody::single(
                TemplateRuleSymbol::Parameter(index),
                InputReference::from(input_index, &node)
            )]
        };
    }
    match grammar.get_symbol(&name) {
        Some(symbol_ref) => BodySet {
            bodies: vec![TemplateRuleBody::single(
                TemplateRuleSymbol::Symbol(symbol_ref),
                InputReference::from(input_index, &node)
            )]
        },
        None => {
            errors.push(Error::SymbolNotFound(
                InputReference::from(input_index, &node.child(0)),
                name
            ));
            BodySet { bodies: Vec::new() }
        }
    }
}

/// Builds the set of rule definitions that represents a single reference to a template variable
fn load_template_rule_atomic_template_ref(
    input_index: usize,
    errors: &mut Vec<Error>,
    grammar: &mut Grammar,
    parameters: &[String],
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    let name = node.child(0).get_value().unwrap();
    let template_index = match grammar
        .template_rules
        .iter()
        .position(|rule| rule.name == name)
    {
        Some(index) => index,
        None => {
            errors.push(Error::TemplateRuleNotFound(
                InputReference::from(input_index, &node.child(0)),
                String::from("Undefined template rule")
            ));
            return BodySet { bodies: Vec::new() };
        }
    };
    let arguments: Vec<TemplateRuleSymbol> = node
        .child(1)
        .into_iter()
        .map(|n| {
            let mut definitions =
                load_template_rule_atomic(input_index, errors, grammar, parameters, n);
            definitions
                .bodies
                .pop()
                .unwrap()
                .elements
                .pop()
                .unwrap()
                .symbol
        })
        .collect();
    let expected_count = grammar.template_rules[template_index].parameters.len();
    if arguments.len() != expected_count {
        errors.push(Error::TemplateRuleWrongNumberOfArgs(
            InputReference::from(input_index, &node.child(0)),
            expected_count,
            arguments.len()
        ));
        return BodySet { bodies: Vec::new() };
    }
    BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Template(TemplateRuleRef {
                template: template_index,
                arguments
            }),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Builds the set of rule definitions that represents a single inline piece of text
fn load_template_rule_atomic_inline_text(
    input_index: usize,
    grammar: &mut Grammar,
    node: AstNode
) -> BodySet<TemplateRuleBody> {
    // Construct the terminal name
    let value = node.get_value().unwrap();
    let start = if value.starts_with('~') { 2 } else { 1 };
    let value = replace_escapees(value[start..(value.len() - 1)].to_string());
    // Check for previous instance in the grammar
    let id = match grammar.get_terminal_for_value(&value) {
        None => {
            // Create the terminal
            let nfa = load_nfa_simple_text(&node);
            let terminal = grammar.add_terminal_anonymous(
                value,
                InputReference::from(input_index, &node),
                nfa
            );
            terminal.nfa.states[terminal.nfa.exit]
                .add_item(FinalItem::Terminal(terminal.id, terminal.context));
            terminal.id
        }
        Some(terminal) => terminal.id
    };
    // Create the definition set
    BodySet {
        bodies: vec![TemplateRuleBody::single(
            TemplateRuleSymbol::Symbol(SymbolRef::Terminal(id)),
            InputReference::from(input_index, &node)
        )]
    }
}

/// Gets the char at the given index
fn get_char_value(value: &[char], i: usize) -> (char, usize) {
    let mut c = value[i];
    if c != '\\' {
        return (c, 1);
    }
    c = value[i + 1];
    match c {
        '0' => (0 as char, 2),  // null
        'a' => (2 as char, 2),  // alert
        'b' => (8 as char, 2),  // backspace
        'f' => (12 as char, 2), // form feed
        'n' => ('\n', 2),       //new line
        'r' => ('\r', 2),       // carriage return
        't' => ('\t', 2),       // horizontal tab
        'v' => (11 as char, 2), // vertical tab
        'u' => {
            let mut l = 0;
            while i + 2 + l < value.len() {
                c = value[i + 2 + l];
                if ('0'..='9').contains(&c) || ('a'..='f').contains(&c) || ('A'..='F').contains(&c)
                {
                    l += 1;
                } else {
                    break;
                }
            }
            if l >= 8 {
                l = 8;
            } else if l >= 4 {
                l = 4;
            }
            let char_hexa: String = value[(i + 2)..(i + 2 + l)].iter().collect();
            let char_value = u32::from_str_radix(&char_hexa, 16).unwrap();
            (std::char::from_u32(char_value).unwrap(), l + 2)
        }
        _ => (c, 2)
    }
}

/// Replaces the escape sequences in the given piece of text by their value
pub fn replace_escapees(value: String) -> String {
    if !value.contains('\\') {
        return value;
    }
    let chars: Vec<char> = value.chars().collect();
    let mut result = String::new();
    let mut i = 0;
    while i < chars.len() {
        let (c, l) = get_char_value(&chars, i);
        result.push(c);
        i += l;
    }
    result
}
