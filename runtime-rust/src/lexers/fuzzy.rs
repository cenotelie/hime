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

//! Module for the lexers' fuzzy DFA matcher

use std::mem::take;

use super::automaton::{Automaton, AutomatonState, TokenMatch, DEAD_STATE};
use crate::errors::{ParseErrorEndOfInput, ParseErrorUnexpectedChar, ParseErrors};
use crate::text::{Text, Utf16C};

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
        FuzzyMatcherHead {
            state,
            errors: None
        }
    }

    /// Initializes this head from a previous one
    pub fn new_previous(previous: &FuzzyMatcherHead, state: u32) -> FuzzyMatcherHead {
        FuzzyMatcherHead {
            state,
            errors: previous.errors.clone()
        }
    }

    /// Initializes this erroneous head from a previous one
    pub fn new_error(
        previous: &FuzzyMatcherHead,
        state: u32,
        offset: usize,
        distance: usize
    ) -> FuzzyMatcherHead {
        let mut errors = Vec::with_capacity(distance);
        if previous.errors.is_some() {
            let others = previous.errors.as_ref().unwrap();
            for x in others {
                errors.push(*x);
            }
        }
        while errors.len() < distance {
            errors.push(offset as u32);
        }
        FuzzyMatcherHead {
            state,
            errors: Some(errors)
        }
    }

    /// Gets the Levenshtein distance of this head from the input
    pub fn get_distance(&self) -> usize {
        if self.errors.is_none() {
            0
        } else {
            self.errors.as_ref().unwrap().len()
        }
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
pub struct FuzzyMatcher<'s, 't, 'a> {
    /// This lexer's automaton
    automaton: &'a Automaton,
    /// Terminal index of the SEPARATOR terminal
    separator: u32,
    /// The input text
    text: &'a Text<'t>,
    /// Delegate for raising errors
    errors: &'a mut ParseErrors<'s>,
    /// The maximum Levenshtein distance between the input and the DFA
    max_distance: usize,
    /// The index in the input from which the error was raised
    origin_index: usize
}

/// The current state of a matcher
struct FuzzyMatcherResult {
    /// The current heads
    pub heads: Vec<FuzzyMatcherHead>,
    /// The current matching head, if any
    pub match_head: Option<FuzzyMatcherHead>,
    /// The current matching length
    pub match_length: usize,
    /// The current insertions
    pub insertions: Vec<u32>
}

impl FuzzyMatcherResult {
    /// Creates a new matcher state
    pub fn new() -> FuzzyMatcherResult {
        FuzzyMatcherResult {
            heads: Vec::new(),
            match_head: None,
            match_length: 0,
            insertions: Vec::new()
        }
    }

    /// Pushes a new head onto the the queue
    pub fn push_head(&mut self, previous: &FuzzyMatcherHead, state: u32) {
        let distance = previous.get_distance();
        // try to find a pre-existing head with the same state at a lesser distance
        for x in self.heads.iter().rev() {
            if x.state == state && x.get_distance() <= distance {
                return;
            }
        }
        self.heads
            .push(FuzzyMatcherHead::new_previous(previous, state));
    }

    /// Pushes a new head onto the the queue for an error's fix
    pub fn push_head_error(&mut self, previous: &FuzzyMatcherHead, state: u32, offset: usize) {
        self.push_head_long_error(previous, state, offset, previous.get_distance() + 1);
    }

    /// Pushes a new head onto the the queue for an error's fix
    pub fn push_head_long_error(
        &mut self,
        previous: &FuzzyMatcherHead,
        state: u32,
        offset: usize,
        distance: usize
    ) {
        // try to find a pre-existing head with the same state at a lesser distance
        for x in self.heads.iter().rev() {
            if x.state == state && x.get_distance() <= distance {
                return;
            }
        }
        self.heads.push(FuzzyMatcherHead::new_error(
            previous, state, offset, distance
        ));
    }
}

impl<'s, 't, 'a> FuzzyMatcher<'s, 't, 'a> {
    /// Initializes this matcher
    pub fn new(
        automaton: &'a Automaton,
        separator: u32,
        text: &'a Text<'t>,
        errors: &'a mut ParseErrors<'s>,
        max_distance: usize,
        origin_index: usize
    ) -> FuzzyMatcher<'s, 't, 'a> {
        FuzzyMatcher {
            automaton,
            separator,
            text,
            errors,
            max_distance,
            origin_index
        }
    }

    /// Runs this matcher
    pub fn run(&mut self) -> Option<TokenMatch> {
        let mut iter = self.text.iter_utf16_from(self.origin_index);
        let (current, length) = match iter.next() {
            None => (None, 0),
            Some((current, length)) => (Some(current), length)
        };
        let mut offset = 0;

        let mut result = FuzzyMatcherResult::new();
        {
            let head_begin = FuzzyMatcherHead::new(0);
            if let Some(current) = current {
                self.inspect(&mut result, &head_begin, offset, current);
            } else {
                self.inspect_at_end(&mut result, &head_begin, offset);
            }
        }
        offset += length;
        while !result.heads.is_empty() {
            let (current, length) = match iter.next() {
                None => (None, 0),
                Some((current, length)) => (Some(current), length)
            };
            let generation = take(&mut result.heads);
            for head in generation {
                if let Some(current) = current {
                    self.inspect(&mut result, &head, offset, current);
                } else {
                    self.inspect_at_end(&mut result, &head, offset);
                }
            }
            offset += length;
        }
        if result.match_length == 0 {
            self.on_failure()
        } else {
            Some(self.on_success(&result))
        }
    }

    /// Constructs the solution when succeeded to fix the error
    fn on_success(&mut self, result: &FuzzyMatcherResult) -> TokenMatch {
        let mut last_error_index = result.match_length + 1;
        for i in 0..result.match_head.as_ref().unwrap().get_distance() {
            let error_index =
                self.origin_index + result.match_head.as_ref().unwrap().get_error(i) as usize;
            if error_index != last_error_index {
                self.on_error(error_index);
            }
            last_error_index = error_index;
        }
        TokenMatch {
            state: result.match_head.as_ref().unwrap().state,
            length: result.match_length as u32
        }
    }

    /// Reports on the lexical error at the specified index
    fn on_error(&mut self, index: usize) {
        if self.text.is_end(index) {
            // the end of input was not expected
            // there is necessarily some input before because an empty input would have matched the $
            // usual unexpected end of input
            self.errors
                .push_error_eoi(ParseErrorEndOfInput::new(self.text.get_position_at(index)));
        } else {
            // a simple unexpected character
            self.errors
                .push_error_unexpected_char(ParseErrorUnexpectedChar::new(
                    self.text.get_position_at(index),
                    self.text.at(index)
                ));
        }
    }

    /// Constructs the solution when failed to fix the error
    fn on_failure(&mut self) -> Option<TokenMatch> {
        self.errors
            .push_error_unexpected_char(ParseErrorUnexpectedChar::new(
                self.text.get_position_at(self.origin_index),
                self.text.at(self.origin_index)
            ));
        None
    }

    /// Inspects a head while at the end of the input
    fn inspect_at_end(
        &self,
        result: &mut FuzzyMatcherResult,
        head: &FuzzyMatcherHead,
        offset: usize
    ) {
        let state_data = self.automaton.get_state(head.state);
        // is it a matching state
        if state_data.get_terminals_count() > 0
            && u32::from(state_data.get_terminal(0).index) != self.separator
        {
            FuzzyMatcher::on_matching_head(result, head, offset);
        }
        if head.get_distance() < self.max_distance && !state_data.is_dead_end() {
            // lookup transitions
            FuzzyMatcher::explore_transitions(result, head, &state_data, offset, false);
            self.explore_insertions(result, head, offset, false, 0);
        }
    }

    /// Inspects a head with a specified character ahead
    fn inspect(
        &self,
        result: &mut FuzzyMatcherResult,
        head: &FuzzyMatcherHead,
        offset: usize,
        current: Utf16C
    ) {
        let state_data = self.automaton.get_state(head.state);
        // is it a matching state
        if state_data.get_terminals_count() > 0
            && u32::from(state_data.get_terminal(0).index) != self.separator
        {
            FuzzyMatcher::on_matching_head(result, head, offset);
        }
        if head.get_distance() >= self.max_distance || state_data.is_dead_end() {
            // cannot stray further
            return;
        }
        // could be a straight match
        let target = state_data.get_target_by(current);
        if target != DEAD_STATE {
            // push it!
            result.push_head(head, target);
        }
        // could try a drop
        result.push_head_error(head, head.state, offset);
        // lookup transitions
        FuzzyMatcher::explore_transitions(result, head, &state_data, offset, false);
        self.explore_insertions(result, head, offset, false, current);
    }

    /// Explores a state transition
    fn explore_transitions(
        result: &mut FuzzyMatcherResult,
        head: &FuzzyMatcherHead,
        state_data: &AutomatonState,
        offset: usize,
        at_end: bool
    ) {
        for i in 0..256 {
            let target = state_data.get_cached_transition(i);
            if target != DEAD_STATE {
                FuzzyMatcher::explore_transition_to_target(result, head, target, offset, at_end);
            }
        }
        for i in 0..state_data.get_bulk_transitions_count() {
            FuzzyMatcher::explore_transition_to_target(
                result,
                head,
                state_data.get_bulk_transition(i).target,
                offset,
                at_end
            );
        }
    }

    /// Explores a state transition
    fn explore_transition_to_target(
        result: &mut FuzzyMatcherResult,
        head: &FuzzyMatcherHead,
        target: u32,
        offset: usize,
        at_end: bool
    ) {
        if !at_end {
            // try to replace
            result.push_head_error(head, target, offset);
        }
        // try to insert
        let mut found = false;
        for i in 0..result.insertions.len() {
            if result.insertions[i] == target {
                found = true;
                break;
            }
        }
        if !found {
            result.insertions.push(target);
        }
    }

    /// Explores the current insertions
    fn explore_insertions(
        &self,
        result: &mut FuzzyMatcherResult,
        head: &FuzzyMatcherHead,
        offset: usize,
        at_end: bool,
        current: Utf16C
    ) {
        let mut distance = head.get_distance() + 1;
        let mut end = result.insertions.len();
        let mut start = 0;
        // while there are insertions to examine in a round
        while start != end {
            for i in start..end {
                // examine insertion i
                let state = result.insertions[i];
                self.explore_insertion(result, head, offset, at_end, current, state, distance);
            }
            // prepare next round
            distance += 1;
            start = end;
            end = result.insertions.len();
        }
        result.insertions.clear();
    }

    /// Explores an insertion
    #[allow(clippy::too_many_arguments)]
    fn explore_insertion(
        &self,
        result: &mut FuzzyMatcherResult,
        head: &FuzzyMatcherHead,
        offset: usize,
        at_end: bool,
        current: Utf16C,
        state: u32,
        distance: usize
    ) {
        let state_data = self.automaton.get_state(head.state);
        if state_data.get_terminals_count() > 0
            && u32::from(state_data.get_terminal(0).index) != self.separator
        {
            FuzzyMatcher::on_matching_insertion(result, head, offset, state, distance);
        }
        if !at_end {
            let target = state_data.get_target_by(current);
            if target != DEAD_STATE {
                result.push_head_long_error(head, target, offset, distance);
            }
        }
        if distance < self.max_distance {
            // continue insertion
            FuzzyMatcher::explore_transitions(result, head, &state_data, offset, at_end);
        }
    }

    /// When a matching head is encountered
    fn on_matching_head(result: &mut FuzzyMatcherResult, head: &FuzzyMatcherHead, offset: usize) {
        if result.match_head.is_none() {
            result.match_head = Some(head.clone());
            result.match_length = offset;
        } else {
            let current_cl =
                get_comparable_length(result.match_head.as_ref().unwrap(), result.match_length);
            let candidate_cl = get_comparable_length(head, offset);
            if candidate_cl > current_cl {
                result.match_head = Some(head.clone());
                result.match_length = offset;
            }
        }
    }

    /// When a matching insertion is encountered
    fn on_matching_insertion(
        result: &mut FuzzyMatcherResult,
        previous: &FuzzyMatcherHead,
        offset: usize,
        target: u32,
        distance: usize
    ) {
        if result.match_head.is_none() {
            result.match_head = Some(FuzzyMatcherHead::new_error(
                previous, target, offset, distance
            ));
            result.match_length = offset;
        } else {
            let d = distance - previous.get_distance();
            let current_cl =
                get_comparable_length(result.match_head.as_ref().unwrap(), result.match_length);
            let candidate_cl = get_comparable_length(previous, offset - d);
            if candidate_cl > current_cl {
                result.match_head = Some(FuzzyMatcherHead::new_error(
                    previous, target, offset, distance
                ));
                result.match_length = offset;
            }
        }
    }
}

/// Computes the comparable length of the specified match
#[allow(clippy::cast_possible_wrap)]
fn get_comparable_length(head: &FuzzyMatcherHead, length: usize) -> isize {
    length as isize - head.get_distance() as isize
}
