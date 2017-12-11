/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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

use std::mem::replace;

use super::automaton::Automaton;
use super::automaton::DEAD_STATE;
use super::automaton::TokenMatch;
use super::super::errors::ParseError;
use super::super::errors::ParseErrorType;
use super::super::errors::ParseErrorUnexpectedChar;
use super::super::errors::ParseErrorEndOfInput;
use super::super::errors::ParseErrorIncorrectEncodingSequence;
use super::super::text::interface::Text;
use super::super::text::utf16::Utf16C;

/// Represents a DFA stack head
#[derive(Clone)]
struct FuzzyMatcherHead {
    /// The associated DFA state
    state: u32,
    /// The data representing this head
    errors: Option<Vec<u32>>
}

impl FuzzyMatcherHead {
    /// Initializes this head with a state and a 0 distance
    pub fn new(state: u32) -> FuzzyMatcherHead {
        FuzzyMatcherHead { state, errors: None }
    }

    /// Initializes this head from a previous one
    pub fn new_previous(previous: &FuzzyMatcherHead, state: u32) -> FuzzyMatcherHead {
        FuzzyMatcherHead {
            state,
            errors: previous.errors.clone()
        }
    }

    /// Initializes this erroneous head from a previous one
    pub fn new_error(previous: &FuzzyMatcherHead, state: u32, offset: usize, distance: usize) -> FuzzyMatcherHead {
        let mut errors = Vec::<u32>::with_capacity(distance);
        if previous.errors.is_some() {
            let others = previous.errors.as_ref().unwrap();
            for i in 0..others.len() {
                errors.push(others[i]);
            }
            for _i in others.len()..distance {
                errors.push(offset as u32);
            }
        }
        FuzzyMatcherHead { state, errors: Some(errors) }
    }

    /// Gets the Levenshtein distance of this head form the input
    pub fn get_distance(&self) -> usize {
        if self.errors.is_none() { 0 } else { self.errors.as_ref().unwrap().len() }
    }

    /// Gets the offset in the input of the i-th lexical error on this head
    pub fn get_error(&self, i: usize) -> u32 {
        self.errors.as_ref().unwrap()[i]
    }
}

/// A fuzzy DFA matcher
/// This matcher uses the Levenshtein distance to match the input ahead against the current DFA automaton.
/// The matcher favors solutions that are the closest to the original input.
/// When multiple solutions are at the same Levenshtein distance to the input, the longest one is preferred.
struct FuzzyMatcher<'a> {
    /// This lexer's automaton
    automaton: Automaton,
    /// Terminal index of the SEPARATOR terminal
    separator: u32,
    /// The input text
    text: &'a Text,
    /// Delegate for raising errors
    errors: fn(Box<ParseError>),
    /// The maximum Levenshtein distance between the input and the DFA
    max_distance: usize,
    /// The index in the input from which the error was raised
    origin_index: usize,
    /// The current heads
    heads: Vec<FuzzyMatcherHead>,
    /// The current matching head, if any
    match_head: Option<FuzzyMatcherHead>,
    /// The current matching length
    match_length: u32
}

impl<'a> FuzzyMatcher<'a> {
    /// Initializes this matcher
    pub fn new(automaton: Automaton, separator: u32, text: &'a Text, errors: fn(Box<ParseError>), max_distance: usize, origin_index: usize) -> FuzzyMatcher<'a> {
        FuzzyMatcher {
            automaton,
            separator,
            text,
            errors,
            max_distance,
            origin_index,
            heads: Vec::<FuzzyMatcherHead>::new(),
            match_head: None,
            match_length: 0
        }
    }

    /// Pushes a new head onto the the queue
    fn push_head(&mut self, previous: &FuzzyMatcherHead, state: u32) {
        let distance = previous.get_distance();
        // try to find a pre-existing head with the same state at a lesser distance
        for x in self.heads.iter().rev() {
            if x.state == state && x.get_distance() <= distance {
                return;
            }
        }
        self.heads.push(FuzzyMatcherHead::new_previous(previous, state));
    }

    /// Pushes a new head onto the the queue for an error's fix
    fn push_head_error(&mut self, previous: &FuzzyMatcherHead, state: u32, offset: usize) {
        self.push_head_long_error(previous, state, offset, previous.get_distance() + 1);
    }

    /// Pushes a new head onto the the queue for an error's fix
    fn push_head_long_error(&mut self, previous: &FuzzyMatcherHead, state: u32, offset: usize, distance: usize) {
        // try to find a pre-existing head with the same state at a lesser distance
        for x in self.heads.iter().rev() {
            if x.state == state && x.get_distance() <= distance {
                return;
            }
        }
        self.heads.push(FuzzyMatcherHead::new_error(previous, state, offset, distance));
    }

    /// Runs this matcher
    pub fn run(&mut self) -> TokenMatch {
        let mut offset = 0;
        let mut at_end = self.text.is_end(self.origin_index + offset);
        let mut current = if at_end { 0 as Utf16C } else { self.text.get_at(self.origin_index + offset) };
        {
            let head_begin = FuzzyMatcherHead::new(0);
            if at_end {
                self.inspect_at_end(&head_begin, offset);
            } else {
                self.inspect(&head_begin, offset, current);
            }
        }
        while self.heads.len() > 0 {
            offset += 1;
            at_end = self.text.is_end(self.origin_index + offset);
            current = if at_end { 0 as Utf16C } else { self.text.get_at(self.origin_index + offset) };
            let generation = replace(&mut self.heads, Vec::<FuzzyMatcherHead>::new());
            for head in generation {
                if at_end {
                    self.inspect_at_end(&head, offset);
                } else {
                    self.inspect(&head, offset, current);
                }
            }
        }
        if self.match_length == 0 {
            self.on_failure()
        } else {
            self.on_success()
        }
    }

    /// Constructs the solution when succeeded to fix the error
    fn on_success(&mut self) -> TokenMatch {
        let mut last_error_index = self.max_distance + 1;
        for i in 0..self.match_head.as_ref().unwrap().get_distance() {
            let error_index = self.origin_index + self.match_head.as_ref().unwrap().get_error(i) as usize;
            if error_index != last_error_index {
                self.on_error(error_index);
            }
            last_error_index = error_index;
        }
        TokenMatch {
            state: self.match_head.as_ref().unwrap().state,
            length: self.match_length
        }
    }

    /// Reports on the lexical error at the specified index
    fn on_error(&mut self, index: usize) {
        if self.text.is_end(index) {
            // the end of input was not expected
            // there is necessarily some input before because an empty input would have matched the $
            let c = self.text.get_at(index - 1);
            if c >= 0xD800 && c <= 0xDBFF {
                // a trailing UTF-16 high surrogate
                (self.errors)(Box::new(ParseErrorIncorrectEncodingSequence::new(
                    self.text.get_position_at(index - 1),
                    ParseErrorType::IncorrectUTF16NoLowSurrogate,
                    c
                )));
            } else {
                // usual unexpected end of input
                (self.errors)(Box::new(ParseErrorEndOfInput::new(
                    self.text.get_position_at(index)
                )));
            }
        } else {
            let c = self.text.get_at(index);
            if c >= 0xD800 && c <= 0xDBFF && !self.text.is_end(index + 1) {
                // a UTF-16 high surrogate
                // if next next character is a low surrogate, also get it
                let c2 = self.text.get_at(index + 1);
                if c2 >= 0xDC00 && c2 <= 0xDFFF {
                    // an unexpected high and low surrogate pair
                    (self.errors)(Box::new(ParseErrorUnexpectedChar::new(
                        self.text.get_position_at(index),
                        [c, c2]
                    )));
                } else {
                    // high surrogate without the low surrogate
                    (self.errors)(Box::new(ParseErrorIncorrectEncodingSequence::new(
                        self.text.get_position_at(index),
                        ParseErrorType::IncorrectUTF16NoLowSurrogate,
                        c
                    )));
                }
            } else if c >= 0xDC00 && c <= 0xDFFF && index > 0 {
                // a UTF-16 low surrogate
                // if the previous character is a high surrogate, also get it
                let c2 = self.text.get_at(index - 1);
                if c2 >= 0xD800 && c2 <= 0xDBFF {
                    // an unexpected high and low surrogate pair
                    (self.errors)(Box::new(ParseErrorUnexpectedChar::new(
                        self.text.get_position_at(index - 1),
                        [c2, c]
                    )));
                } else {
                    // a low surrogate without the high surrogate
                    (self.errors)(Box::new(ParseErrorIncorrectEncodingSequence::new(
                        self.text.get_position_at(index),
                        ParseErrorType::IncorrectUTF16NoHighSurrogate,
                        c
                    )));
                }
            } else {
                // a simple unexpected character
                (self.errors)(Box::new(ParseErrorUnexpectedChar::new(
                    self.text.get_position_at(index),
                    [c, 0]
                )));
            }
        }
    }

    /// Constructs the solution when failed to fix the error
    fn on_failure(&mut self) -> TokenMatch {
        (self.errors)(Box::new(ParseErrorUnexpectedChar::new(
            self.text.get_position_at(self.origin_index),
            [self.text.get_at(self.origin_index), 0]
        )));
        TokenMatch { state: DEAD_STATE, length: 1 }
    }

    /// Inspects a head while at the end of the input
    fn inspect_at_end(&mut self, head: &FuzzyMatcherHead, offset: usize) {}

    /// Inspects a head while at the end of the input
    fn inspect(&mut self, head: &FuzzyMatcherHead, offset: usize, current: Utf16C) {}
}