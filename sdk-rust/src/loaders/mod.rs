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

//! Loodaing facilities for grammars

pub mod parser;

use crate::automata::fa::{FinalItem, NFA};
use crate::grammars::{Grammar, DEFAULT_CONTEXT_NAME};
use crate::CharSpan;
use ansi_term::Colour::{Blue, Red};
use ansi_term::Style;
use hime_redist::ast::AstNode;
use hime_redist::errors::ParseErrorDataTrait;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;
use hime_redist::text::TextContext;
use hime_redist::text::TextPosition;
use std::fs;
use std::io;

/// Represents an error for the loader
#[derive(Clone)]
pub struct LoaderError {
    /// The name of the originating file
    pub filename: String,
    /// The position in the file
    pub position: TextPosition,
    /// The context for the error
    pub context: TextContext,
    /// The error message
    pub message: String
}

impl LoaderError {
    /// Get the width of the line number of this error in number of characters
    pub fn line_number_width(&self) -> usize {
        if self.position.line < 10 {
            1
        } else if self.position.line < 100 {
            2
        } else if self.position.line < 1000 {
            3
        } else if self.position.line < 10_000 {
            4
        } else if self.position.line < 100_000 {
            5
        } else {
            6
        }
    }

    /// Prints this error
    pub fn print(&self, max_width: usize) {
        error!(
            "{}{} {}",
            Red.bold().paint("error"),
            Style::new().bold().paint(":"),
            Style::new().bold().paint(&self.message)
        );
        let pad = String::from_utf8(vec![0x20; max_width]).unwrap();
        error!(
            "{}{} {}:{}",
            &pad,
            Blue.bold().paint("-->"),
            &self.filename,
            self.position
        );
        let pad = String::from_utf8(vec![0x20; max_width + 1]).unwrap();
        error!("{}{}", &pad, Blue.bold().paint("|"));
        let pad = String::from_utf8(vec![0x20; max_width + 1 - self.line_number_width()]).unwrap();
        error!(
            "{}{}{}  {}",
            Blue.bold().paint(format!("{}", self.position.line)),
            &pad,
            Blue.bold().paint("|"),
            &self.context.content
        );
        let pad = String::from_utf8(vec![0x20; max_width + 1]).unwrap();
        error!(
            "{}{}  {}",
            &pad,
            Blue.bold().paint("|"),
            Red.bold().paint(&self.context.pointer)
        );
        error!("");
    }
}

/// Prints the specified errors
pub fn print_errors(errors: &[LoaderError]) {
    if let Some(max_width) = errors.iter().map(|error| error.line_number_width()).max() {
        for error in errors.iter() {
            error.print(max_width);
        }
    }
}

/// Loads all inputs into grammars
pub fn load(inputs: &[String]) -> Result<Vec<Grammar>, ()> {
    let results = parse_inputs(inputs)?;
    let mut errors = Vec::new();
    let (mut completed, mut to_resolve): (Vec<Loader>, Vec<Loader>) = inputs
        .iter()
        .zip(results.into_iter())
        .map(|(input, result)| Loader::new(input.clone(), result, &mut errors))
        .partition(|loader| loader.is_solved());
    resolve_inheritance(&mut completed, &mut to_resolve, &mut errors);
    if errors.is_empty() {
        Ok(completed.into_iter().map(|loader| loader.grammar).collect())
    } else {
        print_errors(&errors);
        Err(())
    }
}

/// Resolves inheritance and load grammars
fn resolve_inheritance(
    completed: &mut Vec<Loader>,
    to_resolve: &mut Vec<Loader>,
    errors: &mut Vec<LoaderError>
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
                loader.collect_errors(&to_resolve, errors);
            }
            return;
        }
    }
}

/// Parses the specified input
fn parse_input(input: &str) -> Result<ParseResult, ()> {
    info!("Reading input {} ...", input);
    let file = match fs::File::open(input) {
        Ok(file) => file,
        Err(error) => {
            error!("Failed to open {}: {:?}", input, &error);
            return Err(());
        }
    };
    let mut reader = io::BufReader::new(file);
    let result = parser::parse_utf8(&mut reader);
    let errors: Vec<LoaderError> = result
        .errors
        .errors
        .iter()
        .map(|error| {
            let position = error.get_position();
            let context = result.text.get_context_for(position, error.get_length());
            LoaderError {
                filename: input.to_string(),
                position,
                context,
                message: error.get_message()
            }
        })
        .collect();
    print_errors(&errors);
    if errors.is_empty() {
        Ok(result)
    } else {
        Err(())
    }
}

/// Parses all inputs
fn parse_inputs(inputs: &[String]) -> Result<Vec<ParseResult>, ()> {
    let mut results = Vec::new();
    let mut has_errors = false;
    for input in inputs.iter() {
        match parse_input(input) {
            Ok(result) => {
                results.push(result);
            }
            Err(_) => {
                has_errors = true;
            }
        }
    }
    if has_errors {
        Err(())
    } else {
        Ok(results)
    }
}

/// Represents a loader for a grammar
struct Loader {
    /// The name of the resource containing the data that are loaded by this instance
    resource: String,
    /// The parse result
    result: ParseResult,
    /// Lists of the inherited grammars
    inherited: Vec<String>,
    /// The resulting grammar
    grammar: Grammar,
    /// Flag for the global casing of the grammar
    case_insensitive: bool
}

impl Loader {
    /// Creates a new loader
    fn new(resource: String, result: ParseResult, errors: &mut Vec<LoaderError>) -> Loader {
        let ast = result.get_ast();
        let root = ast.get_root();
        let name = root.children().at(0).get_value().unwrap();
        let inherited = root
            .children()
            .at(1)
            .children()
            .iter()
            .map(|node| node.get_value().unwrap())
            .collect();
        let mut loader = Loader {
            resource,
            result,
            inherited,
            grammar: Grammar::new(name),
            case_insensitive: false
        };
        if loader.is_solved() {
            loader.load_content(errors);
        }
        loader
    }

    /// Prints errors for the unresolved inherited grammars
    fn collect_errors(&self, unresolved: &[Loader], errors: &mut Vec<LoaderError>) {
        let ast = self.result.get_ast();
        let root = ast.get_root();
        for node in root.children().at(1).children().iter() {
            let name = node.get_value().unwrap();
            if self.inherited.contains(&name) {
                // was not resolved
                if unresolved.iter().all(|l| l.grammar.name != name) {
                    // the dependency does not exist
                    errors.push(LoaderError {
                        filename: self.resource.clone(),
                        position: node.get_position().unwrap(),
                        context: node.get_context().unwrap(),
                        message: format!("Grammar `{}` is not defined", &name)
                    });
                }
            }
        }
    }

    /// Gets a value indicating whether all dependencies are solved
    fn is_solved(&self) -> bool {
        self.inherited.is_empty()
    }

    /// Attempts to load data for the grammar
    fn load(&mut self, completed: &[Loader], errors: &mut Vec<LoaderError>) -> bool {
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
    fn load_content(&mut self, errors: &mut Vec<LoaderError>) {
        info!("Loading grammar {} ...", self.grammar.name);
        let ast = &self.result.get_ast();
        let root = ast.get_root();
        for node in root.children().iter() {
            let id = node.get_symbol().id;
            if id == parser::ID_TERMINAL_BLOCK_OPTIONS {
                load_options(&mut self.grammar, node);
                if let Some(value) = self.grammar.options.get("CaseSensitive") {
                    if value == "false" {
                        self.case_insensitive = true;
                    }
                }
            } else if id == parser::ID_TERMINAL_BLOCK_TERMINALS {
                load_terminals(&self.resource, errors, &mut self.grammar, node);
            }
        }
    }
}

/// Loads the options block of a grammar
fn load_options<'a>(grammar: &mut Grammar, node: AstNode<'a>) {
    for child in node.children().iter() {
        load_option(grammar, child);
    }
}

/// Loads the grammar option in the given AST
fn load_option<'a>(grammar: &mut Grammar, node: AstNode<'a>) {
    let name = node.children().at(0).get_value().unwrap();
    let value = replace_escapees(node.children().at(1).get_value().unwrap());
    let value = value[1..(value.len() - 1)].to_string();
    grammar.add_option(name, value);
}

/// Loads the terminal blocks of a grammar
fn load_terminals<'a>(
    filename: &str,
    errors: &mut Vec<LoaderError>,
    grammar: &mut Grammar,
    node: AstNode<'a>
) {
    for child in node.children().iter() {
        let id = child.get_symbol().id;
        if id == parser::ID_TERMINAL_BLOCK_CONTEXT {
            load_terminal_rule_context(filename, errors, grammar, child);
        } else if id == parser::ID_VARIABLE_TERMINAL_FRAGMENT {
            load_terminal_rule(filename, errors, grammar, child, DEFAULT_CONTEXT_NAME, true);
        } else if id == parser::ID_VARIABLE_TERMINAL_RULE {
            load_terminal_rule(
                filename,
                errors,
                grammar,
                child,
                DEFAULT_CONTEXT_NAME,
                false
            );
        }
    }
}

/// Loads the terminal context in the given AST
fn load_terminal_rule_context<'a>(
    filename: &str,
    errors: &mut Vec<LoaderError>,
    grammar: &mut Grammar,
    node: AstNode<'a>
) {
    let children = node.children();
    let name = children.at(0).get_value().unwrap();
    for child in children.iter().skip(1) {
        load_terminal_rule(filename, errors, grammar, child, &name, false);
    }
}

/// Loads the terminal rule in the given AST
fn load_terminal_rule<'a>(
    filename: &str,
    errors: &mut Vec<LoaderError>,
    grammar: &mut Grammar,
    node: AstNode<'a>,
    context: &str,
    is_fragment: bool
) {
    let children = node.children();
    let node_name = children.at(0);
    let name = node_name.get_value().unwrap();
    if grammar.get_terminal_for_name(&name).is_some() {
        errors.push(LoaderError {
            filename: filename.to_string(),
            position: node_name.get_position().unwrap(),
            context: node_name.get_context().unwrap(),
            message: format!("Overriding the previous definition of `{}`", &name)
        });
        return;
    }
    let nfa = load_nfa(filename, errors, grammar, children.at(1));
    let terminal = grammar.add_terminal_named(name, nfa, context, is_fragment);
    terminal.nfa.states[terminal.nfa.exit].add_item(FinalItem::Terminal(terminal.id));
}

/// Builds the NFA represented by the AST node
fn load_nfa<'a>(
    filename: &str,
    errors: &mut Vec<LoaderError>,
    grammar: &Grammar,
    node: AstNode<'a>
) -> NFA {
    match node.get_symbol().id {
        parser::ID_TERMINAL_LITERAL_TEXT => load_nfa_simple_text(node),
        parser::ID_TERMINAL_UNICODE_CODEPOINT => load_nfa_codepoint(filename, errors, node),
        parser::ID_TERMINAL_LITERAL_CLASS => load_nfa_class(filename, errors, node),
        _ => NFA::new_minimal()
    }
}

/// Builds a NFA from a piece of text
fn load_nfa_simple_text(node: AstNode) -> NFA {
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
fn load_nfa_codepoint(filename: &str, errors: &mut Vec<LoaderError>, node: AstNode) -> NFA {
    // extract the code point value
    let value = node.get_value().unwrap();
    let value = u32::from_str_radix(&value[2..(value.len() - 1)], 16).unwrap();
    let value = match std::char::from_u32(value) {
        Some(v) => v,
        None => {
            errors.push(LoaderError {
                filename: filename.to_string(),
                position: node.get_position().unwrap(),
                context: node.get_context().unwrap(),
                message: format!(
                    "The value U+{:0X} is not a supported unicode code point",
                    value
                )
            });
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
fn load_nfa_class(filename: &str, errors: &mut Vec<LoaderError>, node: AstNode) -> NFA {
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
            errors.push(LoaderError {
                filename: filename.to_string(),
                position: node.get_position().unwrap(),
                context: node.get_context().unwrap(),
                message: format!(
                    "Unsupported non-plane 0 Unicode character ({0}) in character class",
                    b
                )
            });
        }
        if i <= chars.len() - 2 && chars[i] == '-' {
            // this is range, match the -
            i += 1;
            let (e, l2) = get_char_value(&chars, i);
            let e = e as usize;
            if b >= 0xFFFF {
                errors.push(LoaderError {
                    filename: filename.to_string(),
                    position: node.get_position().unwrap(),
                    context: node.get_context().unwrap(),
                    message: format!(
                        "Unsupported non-plane 0 Unicode character ({0}) in character class",
                        e
                    )
                });
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
            if b >= 0xD800 && b <= 0xDFFF {
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
                if (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') {
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
fn replace_escapees(value: String) -> String {
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
