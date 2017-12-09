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

use std::io;
use super::utils;
use super::*;










/// Runs the lexer's DFA to match a terminal in the input ahead
fn run_dfa<T: Text>(input: &Text, index: usize, automaton: &Automaton) -> TokenMatch {
    if input.is_end(index) {
        return TokenMatch { state: 0, length: 0 };
    }
    let mut result = TokenMatch { state: DEAD_STATE, length: 0 };
    let mut state: u32 = 0;
    let mut i = index;
    while state != DEAD_STATE {
        let state_data = automaton.get_state(state);
        // Is this state a matching state ?
        if state_data.get_terminals_count() != 0 {
            result = TokenMatch { state, length: (i - index) as u32 };
        }
        // No further transition => exit
        if state_data.is_dead_end() { break; }
        // At the end of the buffer
        if input.is_end(i) { break; }
        let current = input.get_at(i);
        i = i + 1;
        // Try to find a transition from this state with the read character
        state = state_data.get_target_by(current);
    }
    result
}

/// Represents a DFA stack head
struct FuzzyMatcherHead {
    /// The associated DFA state
    state: u32,
    /// The data representing this head
    errors: Option<Vec<u32>>
}

/// Implementation of `Clone` for `FuzzyMatcherHead`
impl std::clone::Clone for FuzzyMatcherHead {
    fn clone(&self) -> Self {
        FuzzyMatcherHead {
            state: self.state,
            errors: self.errors.clone()
        }
    }
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
    errors: &'a mut Vec<Box<ParseError>>,
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
    pub fn new(automaton: Automaton, separator: u32, text: &'a Text, errors: &'a mut Vec<Box<ParseError>>, max_distance: usize, origin_index: usize) -> FuzzyMatcher<'a> {
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
            let generation = std::mem::replace(&mut self.heads, Vec::<FuzzyMatcherHead>::new());
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
    fn on_error(&mut self, index: usize) {}

    /// Constructs the solution when failed to fix the error
    fn on_failure(&mut self) -> TokenMatch {
        self.errors.push(Box::new(ParseErrorUnexpectedChar {
            unexpected: [self.text.get_at(self.origin_index), 0],
            position: self.text.get_position_at(self.origin_index)
        }));
        TokenMatch { state: DEAD_STATE, length: 1 }
    }

    /// Inspects a head while at the end of the input
    fn inspect_at_end(&mut self, head: &FuzzyMatcherHead, offset: usize) {}

    /// Inspects a head while at the end of the input
    fn inspect(&mut self, head: &FuzzyMatcherHead, offset: usize, current: Utf16C) {}
}