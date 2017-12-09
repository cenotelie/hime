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

use ::std::io;
use super::utils;
use super::*;

/// Determines whether [c1, c2] form a line ending sequence
/// Recognized sequences are:
/// [U+000D, U+000A] (this is Windows-style \r \n)
/// [U+????, U+000A] (this is unix style \n)
/// [U+000D, U+????] (this is MacOS style \r, without \n after)
/// Others:
/// [?, U+000B], [?, U+000C], [?, U+0085], [?, U+2028], [?, U+2029]
fn is_line_ending(c1: Utf16C, c2: Utf16C) -> bool {
    (c2 == 0x000B || c2 == 0x000C || c2 == 0x0085 || c2 == 0x2028 || c2 == 0x2029)
        || (c1 == 0x000D || c2 == 0x000A)
}

/// Determines whether the character is a whitespace
fn is_white_space(c: Utf16C) -> bool {
    c == 0x0020 || c == 0x0009 || c == 0x000B || c == 0x000C
}

/// Finds all the lines in this content
fn find_lines_in<'a, T: utils::Iterable<'a, Item=Utf16C>>(iterable: &'a T) -> Vec<usize> {
    let mut result = Vec::<usize>::new();
    let mut c1;
    let mut c2 = 0;
    let mut i = 0;
    result.push(0);
    for x in iterable.iter() {
        c1 = c2;
        c2 = x;
        if is_line_ending(c1, c2) {
            result.push(if c1 == 0x000D && c2 != 0x000A { i } else { i + 1 });
        }
        i = i + 1;
    }
    result
}

/// Finds the index of the line at the given input index in the content
fn find_line_at(lines: &Vec<usize>, index: usize) -> usize {
    for i in 1..lines.len() {
        if index < lines[i] {
            return i - 1;
        }
    }
    return lines.len() - 1;
}

/// Converts an excerpt of a UTF-16 buffer to a string
fn utf16_to_string(content: &utils::BigList<Utf16C>, start: usize, length: usize) -> String {
    let mut buffer = Vec::<Utf16C>::with_capacity(length);
    for i in start..(start + length) {
        buffer.push(content[i]);
    }
    let result = String::from_utf16(&buffer);
    if result.is_ok() {
        result.unwrap()
    } else {
        String::new()
    }
}


/// Text provider that fetches and stores the full content of an input lexer
/// All line numbers and column numbers are 1-based.
/// Indices in the content are 0-based.
pub struct PrefetchedText {
    /// The full content of the input
    content: utils::BigList<Utf16C>,
    /// Cache of the starting indices of each line within the text
    lines: Vec<usize>
}

impl PrefetchedText {
    /// Initializes this text
    pub fn new(input: &str) -> PrefetchedText {
        let mut content = utils::BigList::<Utf16C>::new(0);
        for c in input.chars() {
            let value = c as u32;
            if value <= 0xFFFF {
                content.add(value as u16);
            } else {
                let temp = value - 0x10000;
                let lead = (temp >> 10) + 0xD800;
                let trail = (temp & 0x03FF) + 0xDC00;
                content.add(lead as Utf16C);
                content.add(trail as Utf16C);
            }
        }
        let lines = find_lines_in(&content);
        PrefetchedText {
            content,
            lines
        }
    }

    /// Initializes this text from a UTF-16 stream
    pub fn from_utf16_stream(input: &mut io::Read, big_endian: bool) -> PrefetchedText {
        let reader = &mut io::BufReader::new(input);
        let mut content = utils::BigList::<Utf16C>::new(0);
        let iterator = utils::Utf16IteratorRaw::new(reader, big_endian);
        for c in iterator {
            content.add(c);
        }
        let lines = find_lines_in(&content);
        PrefetchedText {
            content,
            lines
        }
    }

    /// Initializes this text from a UTF-8 stream
    pub fn from_utf8_stream(input: &mut io::Read) -> PrefetchedText {
        let reader = &mut io::BufReader::new(input);
        let mut content = utils::BigList::<Utf16C>::new(0);
        let iterator = utils::Utf16IteratorOverUtf8::new(reader);
        for c in iterator {
            content.add(c);
        }
        let lines = find_lines_in(&content);
        PrefetchedText {
            content,
            lines
        }
    }
}

impl Text for PrefetchedText {
    /// Gets the number of lines
    fn get_line_count(&self) -> usize {
        self.lines.len()
    }

    /// Gets the size in number of characters
    fn get_size(&self) -> usize {
        self.content.size()
    }

    /// Gets whether the specified index is after the end of the text represented by this object
    fn is_end(&self, index: usize) -> bool {
        index >= self.content.size()
    }

    /// Gets the character at the specified index
    fn get_at(&self, index: usize) -> Utf16C {
        self.content[index]
    }

    /// Gets the substring beginning at the given index with the given length
    fn get_value(&self, index: usize, length: usize) -> String {
        utf16_to_string(&self.content, index, length)
    }

    /// Gets the starting index of the i-th line
    fn get_line_index(&self, line: usize) -> usize {
        self.lines[line]
    }

    /// Gets the length of the i-th line
    fn get_line_length(&self, line: usize) -> usize {
        if line == self.lines.len() - 1 {
            self.content.size() - self.lines[line - 1]
        } else {
            self.lines[line] - self.lines[line - 1]
        }
    }

    /// Gets the position at the given index
    fn get_position_at(&self, index: usize) -> TextPosition {
        let line = find_line_at(&self.lines, index);
        TextPosition {
            line: line + 1,
            column: index - self.lines[line] + 1
        }
    }

    /// Gets the context description for the current text at the specified position
    fn get_context_for(&self, position: TextPosition, length: usize) -> TextContext {
        // gather the data for the line
        let line_index = self.get_line_index(position.line);
        let line_length = self.get_line_length(position.line);
        if line_length == 0 {
            return TextContext {
                content: String::from(""),
                pointer: String::from("^")
            };
        }

        // gather the start and end indices of the line's content to output
        let mut end = line_index + line_length - 1;
        while end != line_index + 1 && (self.content[end] == 0x000A || self.content[end] == 0x000B || self.content[end] == 0x000C || self.content[end] == 0x000D || self.content[end] == 0x0085 || self.content[end] == 0x2028 || self.content[end] == 0x2029) {
            end = end - 1;
        }
        let mut start = line_index;
        while start < end && is_white_space(self.content[start]) {
            start = start + 1;
        }
        if line_index + position.column - 1 < start {
            start = line_index;
        }
        if line_index + position.column - 1 > end {
            end = line_index + line_length - 1;
        }

        // build the pointer
        let mut pointer = String::new();
        for i in start..(line_index + position.column) {
            pointer.push(if self.content[i] == 0x0009 { '\t' } else { ' ' });
        }
        pointer.push('^');
        for _i in 1..length {
            pointer.push('^');
        }

        // return the output
        TextContext {
            content: utf16_to_string(&self.content, start, end - start + 1),
            pointer
        }
    }
}

#[test]
fn test_prefetched_text_lines() {
    let prefetched = PrefetchedText::new("this is\na new line");
    assert_eq!(prefetched.lines.len(), 2);
    assert_eq!(prefetched.lines[0], 0);
    assert_eq!(prefetched.lines[1], 8);
}

#[test]
fn test_prefetched_text_substring() {
    let prefetched = PrefetchedText::new("this is\na new line");
    assert_eq!(utf16_to_string(&prefetched.content, 8, 5), "a new");
}

/// Represents the information of a terminal matched at the state of a lexer's automaton
pub struct MatchedTerminal {
    /// The context
    pub context: u16,
    /// The terminal's index
    pub index: u16
}


/// Identifier of in-existant state in an automaton
const DEAD_STATE: u32 = 0xFFFF;
/// Identifier of the default context
const DEFAULT_CONTEXT: u16 = 0;

/// reads a u16 from an array of bytes
fn read_u16(buffer: &[u8], index: usize) -> u16 {
    ((buffer[index + 1] as u16) << 8 | (buffer[index] as u16))
}

/// reads a u32 from an array of bytes
fn read_u32(buffer: &[u8], index: usize) -> u32 {
    ((buffer[index + 3] as u32) << 24 | (buffer[index + 2] as u32) << 16 | (buffer[index + 1] as u32) << 8 | (buffer[index] as u32))
}

/// Reads a table of u16 from a byte buffer
fn read_table_u16(buffer: &[u8], start: usize, count: usize) -> Vec<u16> {
    let mut result = Vec::<u16>::with_capacity(count);
    for i in 0..count {
        result.push(read_u16(buffer, start + i * 2));
    }
    result
}

/// Reads a table of u32 from a byte buffer
fn read_table_u32(buffer: &[u8], start: usize, count: usize) -> Vec<u32> {
    let mut result = Vec::<u32>::with_capacity(count);
    for i in 0..count {
        result.push(read_u32(buffer, start + i * 4));
    }
    result
}

/// Represents a transition in the automaton of a lexer
/// A transition is matched by a range of UTF-16 code points
/// Its target is a state in the automaton
pub struct AutomatonTransition {
    /// Start of the range
    start: Utf16C,
    /// End of the range
    end: Utf16C,
    /// The transition's target
    target: usize
}

/// Represents a state in the automaton of a lexer
/// Binary data structure:
/// u16: number of matched terminals
/// u16: total number of transitions
/// u16: number of non-cached transitions
/// -- matched terminals
/// u16: context identifier
/// u16: index of the matched terminal
/// -- cache: 256 entries
/// u16: next state's index for index of the entry
/// -- transitions
/// each transition is of the form:
/// u16: start of the range
/// u16: end of the range
/// u16: next state's index
pub struct AutomatonState<'a> {
    /// The automaton table
    table: &'a [u16],
    /// The offset of this state within the table
    offset: usize
}

impl<'a> AutomatonState<'a> {
    /// Gets the number of matched terminals in this state
    pub fn get_terminals_count(&self) -> usize {
        self.table[self.offset] as usize
    }

    /// Gets the i-th matched terminal in this state
    pub fn get_terminal(&self, index: usize) -> MatchedTerminal {
        MatchedTerminal {
            context: self.table[self.offset + index * 2 + 3],
            index: self.table[self.offset + index * 2 + 4]
        }
    }

    /// Gets whether this state is a dead end (no more transition)
    pub fn is_dead_end(&self) -> bool {
        self.table[self.offset + 1] == 0
    }

    /// Gets the number of non-cached transitions in this state
    pub fn get_bulk_transitions_count(&self) -> usize {
        self.table[self.offset + 2] as usize
    }

    /// Gets the target of the cached transition for the specified value
    pub fn get_cached_transition(&self, value: Utf16C) -> u32 {
        self.table[self.offset + 3 + self.table[self.offset] as usize * 2 + value as usize] as u32
    }

    pub fn get_bulk_transition(&self, index: usize) -> AutomatonTransition {
        let offset = self.offset + 3 + self.table[self.offset] as usize * 2 + 256 + index * 3;
        AutomatonTransition {
            start: self.table[offset],
            end: self.table[offset + 1],
            target: self.table[offset + 2] as usize
        }
    }

    /// Gets the target of a transition from this state on the specified value
    pub fn get_target_by(&self, value: Utf16C) -> u32 {
        if value <= 255 {
            return self.get_cached_transition(value);
        }
        let mut current = self.offset + 3 + self.table[self.offset] as usize * 2 + 256;
        for _i in 0..(self.table[self.offset + 2] as usize) {
            if value >= self.table[current] && value <= self.table[current + 1] {
                return self.table[current + 2] as u32;
            }
            current = current + 3;
        }
        return DEAD_STATE;
    }
}

/// Represents the automaton of a lexer
/// Binary data structure of lexers:
/// u32: number of entries in the states index table
/// -- states offset table
/// each entry is of the form:
/// u32: offset of the state from the beginning of the states table in number of u16
/// -- states table
pub struct Automaton {
    /// Table of indices in the states table
    table: Vec<u32>,
    /// Lexer's DFA table of states
    states: Vec<u16>,
    /// The number of states in the automaton
    states_count: usize
}

impl Automaton {
    /// Initializes a new automaton from the given binary data
    pub fn new(data: &[u8]) -> Automaton {
        let states_count = read_u32(data, 0) as usize;
        let table = read_table_u32(data, 4, states_count);
        let rest = (data.len() - 4 - states_count * 4) / 2;
        let states = read_table_u16(data, 4 + states_count * 4, rest);
        Automaton { table, states, states_count }
    }

    /// Gets the number of states in the automaton
    pub fn get_states_count(&self) -> usize {
        self.states_count
    }

    /// Get the data of the specified state
    pub fn get_state(&self, state: u32) -> AutomatonState {
        AutomatonState {
            table: &self.states,
            offset: self.table[state as usize] as usize
        }
    }
}

/// Represents a match in the input
struct TokenMatch {
    /// The matching DFA state
    state: u32,
    /// Length of the matched input
    length: u32
}

/// Represents the kernel of a token, i.e. the identifying information of a token
pub struct TokenKernel {
    /// The identifier of the matched terminal
    terminal_id: u32,
    /// The token's index in its repository
    index: u32
}


pub trait ContextProvider {
    /// Gets the priority of the specified context required by the specified terminal
    /// The priority is a positive integer. The lesser the value the higher the priority.
    /// A negative value represents the unavailability of the required context.
    fn get_context_priority(&self, context: u16, terminal_id: u32) -> usize;
}

/// The default context provider
struct DefaultContextProvider {}

impl ContextProvider for DefaultContextProvider {
    fn get_context_priority(&self, context: u16, terminal_id: u32) -> usize {
        if context == DEFAULT_CONTEXT { ::std::usize::MAX } else { 0 }
    }
}

/// Represents a base lexer
pub trait Lexer<'a, T: 'a + Text, X: 'a + utils::Iterable<'a, Item=Token<'a, T>>> {
    /// Gets the terminals matched by this lexer
    fn get_terminals(&self) -> &[Symbol];

    /// Gets the lexer's input text
    fn get_input(&self) -> &Text;

    /// Gets the lexer's output stream of tokens
    fn get_output(&self) -> &X;

    /// Gets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn get_recovery_distance(&self) -> u32;

    /// Sets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn set_recovery_distance(&mut self, distance: u32);

    /// Gets the function for handling lexical errors
    fn get_lexical_error_handler(&self) -> fn(Box<ParseError>);

    /// Gets the next token in the input
    fn get_next_token() -> Token<'a, T>;
}

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
impl ::std::clone::Clone for FuzzyMatcherHead {
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
            let generation = self.heads.clone();
            self.heads.clear();
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