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

//! Module for the management of errors in the SDK

use crate::grammars::{Grammar, RuleRef, SymbolRef, TerminalRef, OPTION_AXIOM, OPTION_SEPARATOR};
use crate::lr::{Conflict, ConflictKind, ContextError, Item, Phrase};
use crate::{InputReference, LoadedData};
use ansi_term::Colour::{Blue, Red};
use ansi_term::Style;
use hime_redist::text::TextContext;
use std::io;

/// Represents an error where a token is used by cannot be produced by the lexer
#[derive(Debug, Clone)]
pub struct UnmatchableTokenError {
    /// The problematic terminal
    pub terminal: TerminalRef,
    /// The terminals that override the problematic one
    pub overriders: Vec<TerminalRef>
}

/// The global error type
#[derive(Debug)]
pub enum Error {
    /// An IO error (file not found, etc.)
    Io(io::Error),
    /// A simple message
    Msg(String),
    /// Parsing error
    Parsing(InputReference, String),
    /// The specified grammar was not found
    GrammarNotFound(String),
    /// The value for the option is invalid
    /// (grammar_index, option_name, valid_options)
    InvalidOption(usize, String, Vec<String>),
    /// The grammar's axiom has not been specified in the options
    /// (grammar_index)
    AxiomNotSpecified(usize),
    /// The grammar's axiom is not defined (does not exist)
    /// (grammar_index)
    AxiomNotDefined(usize),
    /// The separator token specified by a grammar is not defined
    /// (grammar_index)
    SeparatorNotDefined(usize),
    /// The separator token is contextual
    /// (grammar_index, separator)
    SeparatorIsContextual(usize, TerminalRef),
    /// The separator token cannot be matched, it may be overriden by others
    /// (grammar_index, separator, overriders)
    SeparatorCannotBeMatched(usize, UnmatchableTokenError),
    /// The template rule could not be found
    TemplateRuleNotFound(InputReference, String),
    /// When instantiating a template rule, the wrong number of arguments were supplied (expected, supplied)
    TemplateRuleWrongNumberOfArgs(InputReference, usize, usize),
    /// The specifiec symbol was not found
    SymbolNotFound(InputReference, String),
    /// Invalid character span
    InvalidCharacterSpan(InputReference),
    /// The unicode block is not known
    UnknownUnicodeBlock(InputReference, String),
    /// The unicode category is not known
    UnknownUnicodeCategory(InputReference, String),
    /// A unicode character not in plane 0 was used in a character class, which is not supported
    UnsupportedNonPlane0InCharacterClass(InputReference, usize),
    /// The specified value is not a valid unicode code point
    InvalidCodePoint(InputReference, u32),
    /// A terminal override a previous definition
    OverridingPreviousTerminal(InputReference, String),
    /// The inherited grammar cannot be found
    GrammarNotDefined(InputReference, String),
    /// A conflict in a grammar
    LrConflict(usize, Conflict),
    /// A contextual terminal is used outside of its context
    TerminalOutsideContext(usize, ContextError),
    /// A terminal is used by the parser but cannot be produced by the lexer
    TerminalCannotBeMatched(usize, UnmatchableTokenError),
    /// A terminal matches the empty string
    /// (grammar_index, terminal)
    TerminalMatchesEmpty(usize, TerminalRef)
}

impl From<io::Error> for Error {
    fn from(err: io::Error) -> Self {
        Error::Io(err)
    }
}

const MSG_AXIOM_NOT_SPECIFIED: &str = "Grammar axiom has not been specified";

/// Gets the length of the line number for a reference to the value of a grammar option
fn line_number_width_option_value(
    data: &LoadedData,
    grammar_index: usize,
    option_name: &str
) -> usize {
    data.grammars[grammar_index]
        .get_option(option_name)
        .unwrap()
        .value_input_ref
        .get_line_number_width()
}

/// Gets the length of the line number for a reference to the definition of a terminal
fn line_number_width_terminal(
    data: &LoadedData,
    grammar_index: usize,
    terminal_ref: TerminalRef
) -> usize {
    data.grammars[grammar_index]
        .get_terminal(terminal_ref.priority())
        .unwrap()
        .input_ref
        .get_line_number_width()
}

/// Gets the context for an input reference
fn context_for(data: &LoadedData, input_ref: &InputReference) -> TextContext {
    data.inputs[input_ref.input_index]
        .content
        .get_context_for(input_ref.position, input_ref.length)
}

impl Error {
    /// Gets the length of the line number for this error
    pub fn get_line_number_width(&self, data: &LoadedData) -> usize {
        match self {
            Error::Io(_) => 0,
            Error::Msg(_) => 0,
            Error::Parsing(input, _) => input.get_line_number_width(),
            Error::GrammarNotFound(_) => 0,
            Error::InvalidOption(grammar_index, name, _) => {
                line_number_width_option_value(data, *grammar_index, name)
            }
            Error::AxiomNotSpecified(grammar_index) => data.grammars[*grammar_index]
                .input_ref
                .get_line_number_width(),
            Error::AxiomNotDefined(grammar_index) => {
                line_number_width_option_value(data, *grammar_index, OPTION_AXIOM)
            }
            Error::SeparatorNotDefined(grammar_index) => {
                line_number_width_option_value(data, *grammar_index, OPTION_SEPARATOR)
            }
            Error::SeparatorIsContextual(grammar_index, terminal_ref) => {
                line_number_width_terminal(data, *grammar_index, *terminal_ref)
            }
            Error::SeparatorCannotBeMatched(grammar_index, error) => {
                line_number_width_terminal(data, *grammar_index, error.terminal).max(
                    error
                        .overriders
                        .iter()
                        .map(|overrider| {
                            line_number_width_terminal(data, *grammar_index, *overrider)
                        })
                        .max()
                        .unwrap_or(0)
                )
            }
            Error::TemplateRuleNotFound(input, _) => input.get_line_number_width(),
            Error::TemplateRuleWrongNumberOfArgs(input, _, _) => input.get_line_number_width(),
            Error::SymbolNotFound(input, _) => input.get_line_number_width(),
            Error::InvalidCharacterSpan(input) => input.get_line_number_width(),
            Error::UnknownUnicodeBlock(input, _) => input.get_line_number_width(),
            Error::UnknownUnicodeCategory(input, _) => input.get_line_number_width(),
            Error::UnsupportedNonPlane0InCharacterClass(input, _) => input.get_line_number_width(),
            Error::InvalidCodePoint(input, _) => input.get_line_number_width(),
            Error::OverridingPreviousTerminal(input, _) => input.get_line_number_width(),
            Error::GrammarNotDefined(input, _) => input.get_line_number_width(),
            Error::LrConflict(grammar_index, conflict) => {
                let max_shift = conflict
                    .shift_items
                    .iter()
                    .map(|item| {
                        let rule = item.rule.get_rule_in(&data.grammars[*grammar_index]);
                        let choice = &rule.body.choices[0];
                        choice.elements[item.position]
                            .input_ref
                            .unwrap()
                            .get_line_number_width()
                    })
                    .max()
                    .unwrap_or(0);
                let max_reduce = conflict
                    .reduce_items
                    .iter()
                    .map(|item| {
                        let rule = item.rule.get_rule_in(&data.grammars[*grammar_index]);
                        let choice = &rule.body.choices[0];
                        if item.position >= choice.elements.len() {
                            // at the end
                            choice.elements[choice.elements.len() - 1]
                                .input_ref
                                .unwrap()
                                .get_line_number_width()
                        } else {
                            choice.elements[item.position]
                                .input_ref
                                .unwrap()
                                .get_line_number_width()
                        }
                    })
                    .max()
                    .unwrap_or(0);
                max_shift.max(max_reduce)
            }
            Error::TerminalOutsideContext(_, _) => 0,
            Error::TerminalCannotBeMatched(grammar_index, error) => {
                line_number_width_terminal(data, *grammar_index, error.terminal).max(
                    error
                        .overriders
                        .iter()
                        .map(|overrider| {
                            line_number_width_terminal(data, *grammar_index, *overrider)
                        })
                        .max()
                        .unwrap_or(0)
                )
            }
            Error::TerminalMatchesEmpty(grammar_index, terminal_ref) => {
                line_number_width_terminal(data, *grammar_index, *terminal_ref)
            }
        }
    }

    /// Prints this error
    pub fn print(&self, max_width: usize, data: &LoadedData) {
        match self {
            Error::Io(err) => print_io(err),
            Error::Msg(msg) => print_msg(msg.as_ref()),
            Error::Parsing(input, msg) => print_msg_with_input_ref(max_width, data, input, msg),
            Error::GrammarNotFound(name) => print_msg(&format!("Cannot find grammar `{}`", name)),
            Error::InvalidOption(grammar_index, name, valid) => {
                let option = data.grammars[*grammar_index].get_option(name).unwrap();
                if valid.is_empty() {
                    print_msg_with_input_ref(
                        max_width,
                        data,
                        &option.value_input_ref,
                        &format!("Invalid value for grammar option `{}`", name)
                    );
                } else {
                    let sub_message = format!("expected one of: {}", valid.join(", "));
                    print_msg_with_input_ref_with_sub(
                        max_width,
                        data,
                        &option.value_input_ref,
                        &format!("Invalid value for grammar option `{}`", name),
                        &sub_message
                    );
                }
            }
            Error::AxiomNotSpecified(grammar_index) => print_msg_with_input_ref(
                max_width,
                data,
                &data.grammars[*grammar_index].input_ref,
                MSG_AXIOM_NOT_SPECIFIED
            ),
            Error::AxiomNotDefined(grammar_index) => {
                let option = data.grammars[*grammar_index]
                    .get_option(OPTION_AXIOM)
                    .unwrap();
                print_msg_with_input_ref(
                    max_width,
                    data,
                    &option.value_input_ref,
                    &format!("Grammar axiom `{}` is not defined", &option.value)
                )
            }
            Error::SeparatorNotDefined(grammar_index) => {
                let option = data.grammars[*grammar_index]
                    .get_option(OPTION_SEPARATOR)
                    .unwrap();
                print_msg_with_input_ref(
                    max_width,
                    data,
                    &option.value_input_ref,
                    &format!("Grammar separator token `{}` is not defined", &option.value)
                )
            }
            Error::SeparatorIsContextual(grammar_index, terminal_ref) => {
                let separator = data.grammars[*grammar_index]
                    .get_terminal(terminal_ref.priority())
                    .unwrap();
                print_msg_with_input_ref(
                    max_width,
                    data,
                    &separator.input_ref,
                    &format!(
                        "Grammar separator token `{}` is only defined for context `{}`",
                        &separator.name, &data.grammars[*grammar_index].contexts[separator.context]
                    )
                )
            }
            Error::SeparatorCannotBeMatched(grammar_index, error) => {
                print_token_not_matched(max_width, data, *grammar_index, error)
            }
            Error::TemplateRuleNotFound(input, name) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                &format!("Cannot find template rule `{}`", name)
            ),
            Error::TemplateRuleWrongNumberOfArgs(input, expected, provided) => {
                print_msg_with_input_ref(
                    max_width,
                    data,
                    input,
                    &format!(
                        "Template expected {} arguments, {} given",
                        expected, provided
                    )
                )
            }
            Error::SymbolNotFound(input, name) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                &format!("Cannot find symbol `{}`", name)
            ),
            Error::InvalidCharacterSpan(input) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                "Invalid character span, end is before begin"
            ),
            Error::UnknownUnicodeBlock(input, name) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                &format!("Unknown unicode block `{}`", name)
            ),
            Error::UnknownUnicodeCategory(input, name) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                &format!("Unknown unicode category `{}`", name)
            ),
            Error::UnsupportedNonPlane0InCharacterClass(input, c) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                &format!(
                    "Unsupported non-plane 0 Unicode character ({0}) in character class",
                    c
                )
            ),
            Error::InvalidCodePoint(input, c) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                &format!("The value U+{:0X} is not a supported unicode code point", c)
            ),
            Error::OverridingPreviousTerminal(input, name) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                &format!("Overriding the previous definition of `{}`", name)
            ),
            Error::GrammarNotDefined(input, name) => print_msg_with_input_ref(
                max_width,
                data,
                input,
                &format!("Grammar `{}` is not defined", name)
            ),
            Error::LrConflict(grammar_index, conflict) => {
                print_lr_conflict(max_width, data, *grammar_index, conflict)
            }
            Error::TerminalOutsideContext(grammar_index, error) => {
                print_context_error(max_width, data, *grammar_index, error)
            }
            Error::TerminalCannotBeMatched(grammar_index, error) => {
                print_token_not_matched(max_width, data, *grammar_index, error)
            }
            Error::TerminalMatchesEmpty(grammar_index, terminal_ref) => {
                let terminal = data.grammars[*grammar_index]
                    .get_terminal(terminal_ref.priority())
                    .unwrap();
                print_msg_with_input_ref(
                    max_width,
                    data,
                    &terminal.input_ref,
                    &format!(
                        "Terminal `{}` matches empty string, which is not allowed",
                        &terminal.name
                    )
                )
            }
        }
    }
}

/// Produces a string of spaces of the specified length
fn spaces(length: usize) -> String {
    String::from_utf8(vec![0x20; length]).unwrap()
}

/// Prints an IO error message
fn print_io(error: &io::Error) {
    let message = error.to_string();
    print_msg(&message);
}

/// Prints a simple message error
fn print_msg(message: &str) {
    eprintln!(
        "{}{} {}",
        Red.bold().paint("error"),
        Style::new().bold().paint(":"),
        Style::new().bold().paint(message)
    );
    eprintln!("");
}

/// Prints input using a reference
fn print_input(
    max_width: usize,
    data: &LoadedData,
    input_ref: &InputReference,
    pad: &str,
    sub_message: Option<&str>
) {
    let context = context_for(data, input_ref);
    let pad2 = spaces(max_width + 1 - input_ref.get_line_number_width());
    eprintln!(
        "{}{}{}  {}",
        Blue.bold().paint(format!("{}", input_ref.position.line)),
        &pad2,
        Blue.bold().paint("|"),
        &context.content
    );
    let trailer = if let Some(sub) = sub_message {
        format!("help: {}", sub)
    } else {
        String::new()
    };
    eprintln!(
        "{} {}  {} {}",
        pad,
        Blue.bold().paint("|"),
        Red.bold().paint(&context.pointer),
        Red.paint(&trailer)
    );
}

/// Prints an error with a message and an input reference
fn print_msg_with_input_ref_naked(
    max_width: usize,
    data: &LoadedData,
    input_ref: &InputReference,
    message: &str,
    sub_message: Option<&str>
) {
    eprintln!(
        "{}{} {}",
        Red.bold().paint("error"),
        Style::new().bold().paint(":"),
        Style::new().bold().paint(message)
    );
    let pad = spaces(max_width);
    eprintln!(
        "{}{} {}:{}",
        &pad,
        Blue.bold().paint("-->"),
        &data.inputs[input_ref.input_index].name,
        input_ref.position
    );
    eprintln!("{} {}", &pad, Blue.bold().paint("|"));
    print_input(max_width, data, input_ref, &pad, sub_message);
}

/// Prints an error with a message and an input reference
fn print_msg_with_input_ref(
    max_width: usize,
    data: &LoadedData,
    input_ref: &InputReference,
    message: &str
) {
    print_msg_with_input_ref_naked(max_width, data, input_ref, message, None);
    eprintln!("");
}

/// Prints an error with a message and an input reference
fn print_msg_with_input_ref_with_sub(
    max_width: usize,
    data: &LoadedData,
    input_ref: &InputReference,
    message: &str,
    sub_message: &str
) {
    print_msg_with_input_ref_naked(max_width, data, input_ref, message, Some(sub_message));
    eprintln!("");
}

/// Prints the error of a token that canot be matched
fn print_token_not_matched(
    max_width: usize,
    data: &LoadedData,
    grammar_index: usize,
    error: &UnmatchableTokenError
) {
    let terminal = data.grammars[grammar_index]
        .get_terminal(error.terminal.priority())
        .unwrap();
    print_msg_with_input_ref_naked(
        max_width,
        data,
        &terminal.input_ref,
        &format!(
            "Token `{}` is expected but can never be matched",
            &terminal.value
        ),
        None
    );
    if !error.overriders.is_empty() {
        let pad = spaces(max_width);
        eprintln!("{} {}", &pad, Blue.bold().paint("|"));
        eprintln!(
            "{} {} {}: Token `{}` can be overriden by the following terminals",
            &pad,
            Blue.bold().paint("="),
            Style::new().bold().paint("help"),
            &terminal.value
        );
        eprintln!("{} {}", &pad, Blue.bold().paint("|"));
        for overrider in error.overriders.iter() {
            let terminal = data.grammars[grammar_index]
                .get_terminal(overrider.priority())
                .unwrap();
            print_input(max_width, data, &terminal.input_ref, &pad, None);
        }
    }
    eprintln!("");
}

/// Prints a conflict error message
fn print_lr_conflict(
    max_width: usize,
    data: &LoadedData,
    grammar_index: usize,
    conflict: &Conflict
) {
    let grammar = &data.grammars[grammar_index];
    let terminal = grammar.get_terminal(conflict.lookahead.priority()).unwrap();
    eprintln!(
        "{}{} {}",
        Red.bold().paint("error"),
        Style::new().bold().paint(":"),
        Style::new().bold().paint(format!(
            "{} conflict on lookahead `{}`",
            match conflict.kind {
                ConflictKind::ShiftReduce => "Shift/Reduce",
                ConflictKind::ReduceReduce => "Reduce/Reduce"
            },
            &terminal.value
        ))
    );
    let pad = String::from_utf8(vec![0x20; max_width]).unwrap();
    eprintln!("{}{} {}", &pad, Blue.bold().paint("-->"), &grammar.name);
    eprintln!("{} {}", &pad, Blue.bold().paint("|"));
    for item in conflict.shift_items.iter() {
        print_lr_conflict_item_shift(max_width, data, &pad, grammar, item);
    }
    for item in conflict.reduce_items.iter() {
        print_lr_conflict_item_reduce(max_width, data, &pad, grammar, item);
    }
    if !conflict.phrases.is_empty() {
        eprintln!(
            "{} {} {}: Examples of inputs that are ambiguous",
            &pad,
            Blue.bold().paint("="),
            Style::new().bold().paint("help")
        );
        eprintln!("{} {}", &pad, Blue.bold().paint("|"));
        for phrase in conflict.phrases.iter() {
            print_phrase(&pad, grammar, phrase);
        }
    }
    eprintln!("");
}

/// Prints a LR item
fn print_lr_conflict_item_shift(
    max_width: usize,
    data: &LoadedData,
    pad: &str,
    grammar: &Grammar,
    item: &Item
) {
    let rule = item.rule.get_rule_in(grammar);
    let choice = &rule.body.choices[0];
    let input_ref = choice.elements[item.position].input_ref.unwrap();
    print_input(
        max_width,
        data,
        &input_ref,
        pad,
        Some("Shifting for this symbol")
    );
    eprintln!("{} {} ", pad, Blue.bold().paint("|"));
}

/// Prints a LR item
fn print_lr_conflict_item_reduce(
    max_width: usize,
    data: &LoadedData,
    pad: &str,
    grammar: &Grammar,
    item: &Item
) {
    let rule = item.rule.get_rule_in(grammar);
    let choice = &rule.body.choices[0];
    if item.position >= choice.elements.len() {
        let input_ref = choice.elements[choice.elements.len() - 1]
            .input_ref
            .unwrap();
        let context = context_for(data, &input_ref);
        let pad2 = spaces(max_width + 1 - input_ref.get_line_number_width());
        eprintln!(
            "{}{}{}  {}",
            Blue.bold().paint(format!("{}", input_ref.position.line)),
            &pad2,
            Blue.bold().paint("|"),
            &context.content
        );
        let pad3 = spaces(context.pointer.len());
        eprintln!(
            "{} {}  {} {} {}",
            pad,
            Blue.bold().paint("|"),
            &pad3,
            Red.bold().paint("^"),
            Red.paint("help: Reducing at the rule's end")
        );
    } else {
        let input_ref = choice.elements[item.position].input_ref.unwrap();
        print_input(
            max_width,
            data,
            &input_ref,
            pad,
            Some("Reducing before this nullable part")
        );
    }
    eprintln!("{} {} ", pad, Blue.bold().paint("|"));
}

/// Prints a context error
fn print_context_error(
    max_width: usize,
    data: &LoadedData,
    grammar_index: usize,
    error: &ContextError
) {
    let grammar = &data.grammars[grammar_index];
    let terminal = grammar.get_terminal(error.terminal.priority()).unwrap();
    eprintln!(
        "{}{} {}",
        Red.bold().paint("error"),
        Style::new().bold().paint(":"),
        Style::new().bold().paint(format!(
            "Contextual terminal `{}` is expected outside its context",
            &terminal.value
        ))
    );
    let pad = String::from_utf8(vec![0x20; max_width]).unwrap();
    eprintln!("{}{} {}", &pad, Blue.bold().paint("-->"), &grammar.name);
    eprintln!("{} {}", &pad, Blue.bold().paint("|"));
    for item in error.items.iter() {
        print_context_error_item(max_width, data, &pad, grammar, item);
    }
    if !error.phrases.is_empty() {
        eprintln!(
            "{} {} {}: Examples of inputs that pose this problem",
            &pad,
            Blue.bold().paint("="),
            Style::new().bold().paint("help")
        );
        eprintln!("{} {}", &pad, Blue.bold().paint("|"));
        for phrase in error.phrases.iter() {
            print_phrase(&pad, grammar, phrase);
        }
        eprintln!("");
    }
}

/// Prints a LR item
fn print_context_error_item(
    max_width: usize,
    data: &LoadedData,
    pad: &str,
    grammar: &Grammar,
    item: &Item
) {
    let rule = item.rule.get_rule_in(grammar);
    let choice = &rule.body.choices[0];
    let input_ref = choice.elements[item.position].input_ref.unwrap();
    print_input(
        max_width,
        data,
        &input_ref,
        pad,
        Some("Used outside required context")
    );
    eprintln!("{} {} ", pad, Blue.bold().paint("|"));
}

/// Prints a rule
fn _print_rule(pad: &str, grammar: &Grammar, rule_ref: RuleRef) {
    let rule = rule_ref.get_rule_in(grammar);
    eprint!("{} {} ", pad, Blue.bold().paint("|"));
    let head_name = grammar.get_symbol_value(SymbolRef::Variable(rule_ref.variable));
    eprint!("{} ->", head_name);
    for part in rule.body.elements.iter() {
        let name = grammar.get_symbol_value(part.symbol);
        eprint!(" {}", name);
    }
    eprintln!();
}

/// Prints an input phrase
fn print_phrase(pad: &str, grammar: &Grammar, phrase: &Phrase) {
    eprint!("{} {} ", pad, Blue.bold().paint("|"));
    for symbol in phrase.0.iter() {
        let terminal = grammar.get_terminal(symbol.priority()).unwrap();
        eprint!(" {}", &terminal.value);
    }
    eprintln!();
}

/// A collection of errors
#[derive(Debug)]
pub struct Errors {
    /// The associated data
    pub data: LoadedData,
    /// The errors
    pub errors: Vec<Error>
}

impl Errors {
    /// Encapsulate the errors
    pub fn from(data: LoadedData, errors: Vec<Error>) -> Errors {
        Errors { data, errors }
    }

    /// Prints the errors
    pub fn print(&self) {
        if let Some(max_width) = self
            .errors
            .iter()
            .map(|error| error.get_line_number_width(&self.data))
            .max()
        {
            for error in self.errors.iter() {
                error.print(max_width, &self.data)
            }
        }
    }
}
