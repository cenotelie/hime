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

use super::automaton::Automaton;
use super::automaton::TokenMatch;
use super::automaton::run_dfa;
use super::context::ContextProvider;
use super::fuzzy::FuzzyMatcher;
use super::interface::Lexer;
use super::interface::TokenKernel;
use super::super::errors::ParseErrors;
use super::super::errors::ParseErrorUnexpectedChar;
use super::super::symbols::Symbol;
use super::super::symbols::SID_DOLLAR;
use super::super::text::Text;
use super::super::tokens::TokenRepository;

/// The default maximum Levenshtein distance to go to for the recovery of a matching failure
const DEFAULT_RECOVERY_MATCHING_DISTANCE: usize = 3;

/// Represents a context-free lexer (lexing rules do not depend on the context)
pub struct ContextFreeLexer<T: Text> {
    /// The token repository for this lexer
    repository: TokenRepository<T>,
    /// The DFA automaton for this lexer
    automaton: Automaton,
    /// Whether the lexer has run yet
    has_run: bool,
    /// Symbol ID of the SEPARATOR terminal
    separator_id: u32,
    /// The next token in this repository
    index: usize,
    /// The maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    recovery: usize,
    /// Delegate for raising errors
    errors: ParseErrors
}

/// Runs the fuzzy DFA matcher
fn run_fuzzy_matcher<T: Text>(repository: &TokenRepository<T>, automaton: &Automaton, separator_id: u32, recovery: usize, errors: &mut ParseErrors, origin_index: usize) -> Option<TokenMatch> {
    if recovery <= 0 {
        errors.push_error_unexpected_char(ParseErrorUnexpectedChar::new(
            repository.get_input().get_position_at(origin_index),
            [repository.get_input().get_at(origin_index), 0]
        ));
        None
    } else {
        let mut separator_index = 0;
        for i in 0..repository.get_terminals().len() {
            let terminal = repository.get_terminals()[i];
            if terminal.id == separator_id {
                separator_index = i;
                break;
            }
        }
        let mut matcher = FuzzyMatcher::new(
            automaton,
            separator_index as u32,
            repository.get_input(),
            errors,
            recovery,
            origin_index
        );
        Some(matcher.run())
    }
}

impl<T: Text> Lexer<T> for ContextFreeLexer<T> {
    /// Gets the terminals matched by this lexer
    fn get_terminals(&self) -> &Vec<Symbol> {
        self.repository.get_terminals()
    }

    /// Gets the lexer's input text
    fn get_input(&self) -> &Text {
        self.repository.get_input()
    }

    /// Gets the lexer's output stream of tokens
    fn get_output(&self) -> &TokenRepository<T> {
        &self.repository
    }

    /// Gets the lexer's errors
    fn get_errors(&self) -> &ParseErrors {
        &self.errors
    }

    /// Gets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn get_recovery_distance(&self) -> usize {
        self.recovery
    }

    /// Sets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn set_recovery_distance(&mut self, distance: usize) {
        self.recovery = distance;
    }

    /// Gets the next token in the input
    fn get_next_token(&mut self, _contexts: ContextProvider) -> Option<TokenKernel> {
        if !self.has_run {
            // lex all tokens now
            self.find_tokens();
            self.has_run = true;
        }
        if self.index >= self.repository.get_tokens_count() {
            return None;
        }
        let id = self.repository.get_symbol_id_for(self.index);
        let result = TokenKernel { terminal_id: id, index: self.index as u32 };
        self.index += 1;
        Some(result)
    }
}

impl<T: Text> ContextFreeLexer<T> {
    /// Creates a new lexer
    pub fn new(terminals: Vec<Symbol>, text: T, automaton: Automaton, separator_id: u32) -> ContextFreeLexer<T> {
        ContextFreeLexer {
            repository: TokenRepository::new(terminals, text),
            automaton,
            has_run: false,
            separator_id,
            index: 0,
            recovery: DEFAULT_RECOVERY_MATCHING_DISTANCE,
            errors: ParseErrors::new()
        }
    }

    /// Finds all the tokens in the lexer's input
    fn find_tokens(&mut self) {
        let mut index = 0;
        loop {
            let mut result = run_dfa(&self.automaton, self.repository.get_input(), index);
            if result.is_none() {
                // failed to match, retry with error handling
                result = run_fuzzy_matcher(&self.repository, &self.automaton, self.separator_id, self.recovery, &mut self.errors, index);
            }
            if result.is_none() {
                // skip this character
                index += 1;
            } else {
                let the_match = result.unwrap();
                if the_match.state == 0 {
                    // this is the dollar terminal, at the end of the input
                    // the index of the $ symbol is always 1
                    self.repository.add(1, index, 0);
                    // exit here
                    return;
                } else {
                    // matched something
                    let terminal = self.automaton.get_state(the_match.state).get_terminal(0).index as usize;
                    if self.repository.get_terminals()[terminal].id != self.separator_id {
                        self.repository.add(terminal, index, the_match.length as usize);
                    }
                    index += the_match.length as usize;
                }
            }
        }
    }
}

/// Represents a context-sensitive lexer (lexing rules do not depend on the context)
pub struct ContextSensitiveLexer<T: Text> {
    /// The token repository for this lexer
    repository: TokenRepository<T>,
    /// The DFA automaton for this lexer
    automaton: Automaton,
    /// Whether the lexer has run yet
    has_run: bool,
    /// Symbol ID of the SEPARATOR terminal
    separator_id: u32,
    /// The current index in the input
    input_index: usize,
    /// The maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    recovery: usize,
    /// Delegate for raising errors
    errors: ParseErrors
}


impl<T: Text> Lexer<T> for ContextSensitiveLexer<T> {
    /// Gets the terminals matched by this lexer
    fn get_terminals(&self) -> &Vec<Symbol> {
        self.repository.get_terminals()
    }

    /// Gets the lexer's input text
    fn get_input(&self) -> &Text {
        self.repository.get_input()
    }

    /// Gets the lexer's output stream of tokens
    fn get_output(&self) -> &TokenRepository<T> {
        &self.repository
    }

    /// Gets the lexer's errors
    fn get_errors(&self) -> &ParseErrors {
        &self.errors
    }

    /// Gets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn get_recovery_distance(&self) -> usize {
        self.recovery
    }

    /// Sets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn set_recovery_distance(&mut self, distance: usize) {
        self.recovery = distance;
    }

    /// Gets the next token in the input
    fn get_next_token(&mut self, contexts: ContextProvider) -> Option<TokenKernel> {
        if self.has_run {
            return None;
        }
        loop {
            let mut result = run_dfa(&self.automaton, self.repository.get_input(), self.input_index);
            if result.is_none() {
                // failed to match, retry with error handling
                result = run_fuzzy_matcher(&self.repository, &self.automaton, self.separator_id, self.recovery, &mut self.errors, self.input_index);
            }
            if result.is_none() {
                // skip this character
                self.input_index += 1;
            } else {
                let the_match = result.unwrap();
                if the_match.state == 0 {
                    // this is the dollar terminal, at the end of the input
                    // the index of the $ symbol is always 1
                    let token_index = self.repository.add(1, self.input_index, 0);
                    self.has_run = true;
                    return Some(TokenKernel { terminal_id: SID_DOLLAR, index: token_index as u32 });
                } else {
                    // matched something
                    let terminal_index = self.get_terminal_for(the_match.state, contexts);
                    let terminal_id = self.repository.get_terminals()[terminal_index as usize].id;
                    self.input_index += the_match.length as usize;
                    if terminal_id != self.separator_id {
                        let token_index = self.repository.add(terminal_index as usize, self.input_index, the_match.length as usize);
                        return Some(TokenKernel { terminal_id, index: token_index as u32 });
                    }
                }
            }
        }
    }
}

impl<T: Text> ContextSensitiveLexer<T> {
    /// Creates a new lexer
    pub fn new(terminals: Vec<Symbol>, text: T, automaton: Automaton, separator_id: u32) -> ContextSensitiveLexer<T> {
        ContextSensitiveLexer {
            repository: TokenRepository::new(terminals, text),
            automaton,
            has_run: false,
            separator_id,
            input_index: 0,
            recovery: DEFAULT_RECOVERY_MATCHING_DISTANCE,
            errors: ParseErrors::new()
        }
    }

    /// Gets the index of the terminal with the highest priority that is possible in the contexts
    fn get_terminal_for(&self, state: u32, contexts: ContextProvider) -> u16 {
        let state_data = self.automaton.get_state(state);
        let mut matched = state_data.get_terminal(0);
        let mut result = matched.index;
        let mut id = self.repository.get_terminals()[result as usize].id;
        if id == self.separator_id {
            // the separator trumps all
            return result;
        }
        let mut priority = contexts(matched.context, id);
        for i in 1..state_data.get_terminals_count() {
            matched = state_data.get_terminal(i);
            id = self.repository.get_terminals()[matched.index as usize].id;
            if id == self.separator_id {
                // the separator trumps all
                return matched.index;
            }
            let priority_candidate = contexts(matched.context, id);
            if priority_candidate.is_none() {
                continue;
            }
            if priority.is_none() || priority_candidate.unwrap() < priority.unwrap() {
                result = matched.index;
                priority = priority_candidate;
            }
        }
        result
    }
}