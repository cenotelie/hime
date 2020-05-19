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

use crate::grammars::{Grammar, SymbolRef, TerminalRef};
use crate::lr::{Conflict, ConflictKind, ContextError, Item, Phrase};
use crate::InputReference;
use ansi_term::Colour::{Blue, Red};
use ansi_term::Style;
use std::io;

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
    /// The grammar's axiom has not been specified in the options
    AxiomNotSpecified(InputReference),
    /// The grammar's axiom is not defined (does not exist)
    AxiomNotDefined(InputReference, String),
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
    LrConflict(Conflict, Vec<Phrase>),
    /// A contextual terminal is used outside of its context
    TerminalOutsideContext(ContextError)
}

impl From<io::Error> for Error {
    fn from(err: io::Error) -> Self {
        Error::Io(err)
    }
}

const MSG_AXIOM_NOT_SPECIFIED: &str = "Grammar axiom has not been specified";

impl Error {
    /// Gets the length of the line number for this error
    pub fn get_line_number_width(&self) -> usize {
        match self {
            Error::Io(_) => 0,
            Error::Msg(_) => 0,
            Error::Parsing(input, _) => input.get_line_number_width(),
            Error::GrammarNotFound(_) => 0,
            Error::AxiomNotSpecified(input) => input.get_line_number_width(),
            Error::AxiomNotDefined(input, _) => input.get_line_number_width(),
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
            Error::LrConflict(_, _) => 0,
            Error::TerminalOutsideContext(_) => 0
        }
    }

    /// Prints this error
    pub fn print(&self, max_width: usize, grammar: Option<&Grammar>) {
        match self {
            Error::Io(err) => print_io(err),
            Error::Msg(msg) => print_msg(msg.as_ref()),
            Error::Parsing(input, msg) => print_msg_with_input_ref(max_width, input, msg),
            Error::GrammarNotFound(name) => print_msg(&format!("Cannot find grammar `{}`", name)),
            Error::AxiomNotSpecified(input) => {
                print_msg_with_input_ref(max_width, input, MSG_AXIOM_NOT_SPECIFIED)
            }
            Error::AxiomNotDefined(input, name) => print_msg_with_input_ref(
                max_width,
                input,
                &format!("Grammar axiom `{}` is not defined", name)
            ),
            Error::TemplateRuleNotFound(input, name) => print_msg_with_input_ref(
                max_width,
                input,
                &format!("Cannot find template rule `{}`", name)
            ),
            Error::TemplateRuleWrongNumberOfArgs(input, expected, provided) => {
                print_msg_with_input_ref(
                    max_width,
                    input,
                    &format!(
                        "Template expected {} arguments, {} given",
                        expected, provided
                    )
                )
            }
            Error::SymbolNotFound(input, name) => print_msg_with_input_ref(
                max_width,
                input,
                &format!("Cannot find symbol `{}`", name)
            ),
            Error::InvalidCharacterSpan(input) => print_msg_with_input_ref(
                max_width,
                input,
                "Invalid character span, end is before begin"
            ),
            Error::UnknownUnicodeBlock(input, name) => print_msg_with_input_ref(
                max_width,
                input,
                &format!("Unknown unicode block `{}`", name)
            ),
            Error::UnknownUnicodeCategory(input, name) => print_msg_with_input_ref(
                max_width,
                input,
                &format!("Unknown unicode category `{}`", name)
            ),
            Error::UnsupportedNonPlane0InCharacterClass(input, c) => print_msg_with_input_ref(
                max_width,
                input,
                &format!(
                    "Unsupported non-plane 0 Unicode character ({0}) in character class",
                    c
                )
            ),
            Error::InvalidCodePoint(input, c) => print_msg_with_input_ref(
                max_width,
                input,
                &format!("The value U+{:0X} is not a supported unicode code point", c)
            ),
            Error::OverridingPreviousTerminal(input, name) => print_msg_with_input_ref(
                max_width,
                input,
                &format!("Overriding the previous definition of `{}`", name)
            ),
            Error::GrammarNotDefined(input, name) => print_msg_with_input_ref(
                max_width,
                input,
                &format!("Grammar `{}` is not defined", name)
            ),
            Error::LrConflict(conflict, phrases) => {
                print_lr_conflict(max_width, grammar.unwrap(), conflict, phrases)
            }
            Error::TerminalOutsideContext(error) => print_context_error(grammar.unwrap(), error)
        }
    }
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

/// Prints an error with a message and an input reference
fn print_msg_with_input_ref(max_width: usize, input_ref: &InputReference, message: &str) {
    eprintln!(
        "{}{} {}",
        Red.bold().paint("error"),
        Style::new().bold().paint(":"),
        Style::new().bold().paint(message)
    );
    let pad = String::from_utf8(vec![0x20; max_width]).unwrap();
    eprintln!(
        "{}{} {}:{}",
        &pad,
        Blue.bold().paint("-->"),
        &input_ref.name,
        input_ref.position
    );
    let pad = String::from_utf8(vec![0x20; max_width + 1]).unwrap();
    eprintln!("{}{}", &pad, Blue.bold().paint("|"));
    let pad = String::from_utf8(vec![
        0x20;
        max_width + 1 - input_ref.get_line_number_width()
    ])
    .unwrap();
    eprintln!(
        "{}{}{}  {}",
        Blue.bold().paint(format!("{}", input_ref.position.line)),
        &pad,
        Blue.bold().paint("|"),
        &input_ref.context.content
    );
    let pad = String::from_utf8(vec![0x20; max_width + 1]).unwrap();
    eprintln!(
        "{}{}  {}",
        &pad,
        Blue.bold().paint("|"),
        Red.bold().paint(&input_ref.context.pointer)
    );
    eprintln!("");
}

/// Prints a conflict error message
fn print_lr_conflict(max_width: usize, grammar: &Grammar, conflict: &Conflict, phrases: &[Phrase]) {
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
    for item in conflict.items.iter() {
        print_lr_item(&pad, grammar, item);
    }
    eprintln!(
        "{} {} {}: Examples of inputs that raise this conflict",
        &pad,
        Blue.bold().paint("="),
        Style::new().bold().paint("help")
    );
    eprintln!("{} {}", &pad, Blue.bold().paint("|"));
    for phrase in phrases.iter() {
        print_phrase(&pad, grammar, phrase);
    }
    eprintln!("");
}

/// Prints a LR item
fn print_lr_item(pad: &str, grammar: &Grammar, item: &Item) {
    let rule = item.rule.get_rule_in(grammar);
    let mut prefix = 0;
    eprint!("{} {} ", pad, Blue.bold().paint("|"));
    let head_name = grammar.get_symbol_value(SymbolRef::Variable(item.rule.variable));
    eprint!("{} ->", head_name);
    prefix += head_name.len() + 3;
    for i in 0..item.position {
        let name = grammar.get_symbol_value(rule.body.choices[0].parts[i].symbol);
        eprint!(" {}", name);
        prefix += name.len() + 1;
    }
    eprint!(" {}", Red.bold().paint("*"));
    for i in item.position..(rule.body.choices[0].len()) {
        let name = grammar.get_symbol_value(rule.body.choices[0].parts[i].symbol);
        eprint!(" {}", name);
    }
    eprintln!();
    let pad2 = String::from_utf8(vec![0x20; prefix]).unwrap();
    eprintln!(
        "{} {} {} {}",
        pad,
        Blue.bold().paint("|"),
        &pad2,
        Red.bold().paint("^ at this position")
    );
    eprintln!("{} {} ", pad, Blue.bold().paint("|"));
}

fn print_phrase(pad: &str, grammar: &Grammar, phrase: &Phrase) {
    eprint!("{} {} ", pad, Blue.bold().paint("|"));
    for symbol in phrase.0.iter() {
        let terminal = grammar.get_terminal(symbol.priority()).unwrap();
        eprint!(" {}", &terminal.value);
    }
    eprintln!();
}

/// Prints a context error
fn print_context_error(grammar: &Grammar, error: &ContextError) {}

/// A collection of errors
#[derive(Debug, Default)]
pub struct Errors {
    /// The errors
    pub errors: Vec<Error>
}

impl Errors {
    /// Encapsulate the errors
    pub fn from(errors: Vec<Error>) -> Errors {
        Errors { errors }
    }

    /// Prints the errors
    pub fn print(&self, grammar: Option<&Grammar>) {
        if let Some(max_width) = self
            .errors
            .iter()
            .map(|error| error.get_line_number_width())
            .max()
        {
            for error in self.errors.iter() {
                error.print(max_width, grammar);
            }
        }
    }
}

impl From<Error> for Errors {
    fn from(err: Error) -> Self {
        Errors { errors: vec![err] }
    }
}

impl From<io::Error> for Errors {
    fn from(err: io::Error) -> Self {
        Errors {
            errors: vec![Error::Io(err)]
        }
    }
}
