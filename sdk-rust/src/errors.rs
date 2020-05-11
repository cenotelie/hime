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

use ansi_term::Colour::{Blue, Red};
use ansi_term::Style;
use hime_redist::text::TextContext;
use hime_redist::text::TextPosition;

/// A generic error
pub trait Error {
    /// Get the width of the line number of this error in number of characters
    fn line_number_width(&self) -> usize;

    /// Prints this error
    fn print(&self, max_width: usize);
}

/// Prints the specified errors
pub fn print_errors<T: Error>(errors: &[T]) {
    if let Some(max_width) = errors.iter().map(|error| error.line_number_width()).max() {
        for error in errors.iter() {
            error.print(max_width);
        }
    }
}

/// A simple error with a message
#[derive(Clone)]
pub struct MessageError {
    /// The error message
    pub message: String
}

impl<E> From<E> for MessageError
where
    E: ToString
{
    fn from(err: E) -> MessageError {
        MessageError {
            message: err.to_string()
        }
    }
}

impl Error for MessageError {
    /// Get the width of the line number of this error in number of characters
    fn line_number_width(&self) -> usize {
        0
    }

    /// Prints this error
    fn print(&self, _max_width: usize) {
        eprintln!(
            "{}{} {}",
            Red.bold().paint("error"),
            Style::new().bold().paint(":"),
            Style::new().bold().paint(&self.message)
        );
        eprintln!("");
    }
}

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

impl Error for LoaderError {
    /// Get the width of the line number of this error in number of characters
    fn line_number_width(&self) -> usize {
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
    fn print(&self, max_width: usize) {
        eprintln!(
            "{}{} {}",
            Red.bold().paint("error"),
            Style::new().bold().paint(":"),
            Style::new().bold().paint(&self.message)
        );
        let pad = String::from_utf8(vec![0x20; max_width]).unwrap();
        eprintln!(
            "{}{} {}:{}",
            &pad,
            Blue.bold().paint("-->"),
            &self.filename,
            self.position
        );
        let pad = String::from_utf8(vec![0x20; max_width + 1]).unwrap();
        eprintln!("{}{}", &pad, Blue.bold().paint("|"));
        let pad = String::from_utf8(vec![0x20; max_width + 1 - self.line_number_width()]).unwrap();
        eprintln!(
            "{}{}{}  {}",
            Blue.bold().paint(format!("{}", self.position.line)),
            &pad,
            Blue.bold().paint("|"),
            &self.context.content
        );
        let pad = String::from_utf8(vec![0x20; max_width + 1]).unwrap();
        eprintln!(
            "{}{}  {}",
            &pad,
            Blue.bold().paint("|"),
            Red.bold().paint(&self.context.pointer)
        );
        eprintln!("");
    }
}
