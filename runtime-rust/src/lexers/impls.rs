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

//! Module for lexers' implementation

use super::automaton::{run_dfa, Automaton, TokenMatch};
use super::fuzzy::FuzzyMatcher;
use super::{ContextProvider, LexerData, TokenKernel};
use crate::errors::{ParseErrorUnexpectedChar, ParseErrors};
use crate::symbols::SID_DOLLAR;
use crate::tokens::TokenRepository;

/// The default maximum Levenshtein distance to go to for the recovery of a matching failure
const DEFAULT_RECOVERY_MATCHING_DISTANCE: usize = 3;

/// Runs the fuzzy DFA matcher
fn run_fuzzy_matcher<'s, 't, 'a>(
    repository: &TokenRepository<'s, 't, 'a>,
    automaton: &'a Automaton,
    separator_id: u32,
    recovery: usize,
    errors: &'a mut ParseErrors<'s>,
    origin_index: usize
) -> Option<TokenMatch> {
    if recovery == 0 {
        errors.push_error_unexpected_char(ParseErrorUnexpectedChar::new(
            repository.text.get_position_at(origin_index),
            repository.text.at(origin_index)
        ));
        None
    } else {
        let mut separator_index = 0;
        for i in 0..repository.terminals.len() {
            let terminal = repository.terminals[i];
            if terminal.id == separator_id {
                separator_index = i;
                break;
            }
        }
        let mut matcher = FuzzyMatcher::new(
            automaton,
            separator_index as u32,
            repository.text,
            errors,
            recovery,
            origin_index
        );
        matcher.run()
    }
}

/// Represents a context-free lexer (lexing rules do not depend on the context)
pub struct ContextFreeLexer<'s, 't, 'a> {
    /// The lexer's innner data
    data: LexerData<'s, 't, 'a>
}

impl<'s, 't, 'a> ContextFreeLexer<'s, 't, 'a> {
    /// Creates a new lexer
    pub fn new(
        repository: TokenRepository<'s, 't, 'a>,
        errors: &'a mut ParseErrors<'s>,
        automaton: Automaton,
        separator_id: u32
    ) -> ContextFreeLexer<'s, 't, 'a> {
        ContextFreeLexer {
            data: LexerData {
                repository,
                errors,
                automaton,
                has_run: false,
                separator_id,
                index: 0,
                recovery: DEFAULT_RECOVERY_MATCHING_DISTANCE
            }
        }
    }

    /// Gets the next token in the input
    fn get_next_token(&mut self) -> Option<TokenKernel> {
        if !self.data.has_run {
            // lex all tokens now
            self.find_tokens();
            self.data.has_run = true;
        }
        if self.data.index >= self.data.repository.get_tokens_count() {
            return None;
        }
        let id = self.data.repository.get_symbol_id_for(self.data.index);
        let result = TokenKernel {
            terminal_id: id,
            index: self.data.index as u32
        };
        self.data.index += 1;
        Some(result)
    }

    /// Finds all the tokens in the lexer's input
    fn find_tokens(&mut self) {
        let mut index = 0;
        loop {
            let mut result = run_dfa(&self.data.automaton, self.data.repository.text, index);
            if result.is_none() {
                // failed to match, retry with error handling
                result = run_fuzzy_matcher(
                    &self.data.repository,
                    &self.data.automaton,
                    self.data.separator_id,
                    self.data.recovery,
                    self.data.errors,
                    index
                );
            }
            if let Some(the_match) = result {
                if the_match.state == 0 {
                    // this is the dollar terminal, at the end of the input
                    // the index of the $ symbol is always 1
                    self.data.repository.add(1, index, 0);
                    // exit here
                    return;
                } else {
                    // matched something
                    let terminal = self
                        .data
                        .automaton
                        .get_state(the_match.state)
                        .get_terminal(0)
                        .index as usize;
                    if self.data.repository.terminals[terminal].id != self.data.separator_id {
                        self.data
                            .repository
                            .add(terminal, index, the_match.length as usize);
                    }
                    index += the_match.length as usize;
                }
            } else {
                // skip this character
                index += 1;
            }
        }
    }
}

/// Represents a context-sensitive lexer (lexing rules do not depend on the context)
pub struct ContextSensitiveLexer<'s, 't, 'a> {
    /// The lexer's innner data
    data: LexerData<'s, 't, 'a>,
    /// The current index in the input
    input_index: usize
}

impl<'s, 't, 'a> ContextSensitiveLexer<'s, 't, 'a> {
    /// Creates a new lexer
    pub fn new(
        repository: TokenRepository<'s, 't, 'a>,
        errors: &'a mut ParseErrors<'s>,
        automaton: Automaton,
        separator_id: u32
    ) -> ContextSensitiveLexer<'s, 't, 'a> {
        ContextSensitiveLexer {
            data: LexerData {
                repository,
                errors,
                automaton,
                has_run: false,
                separator_id,
                index: 0,
                recovery: DEFAULT_RECOVERY_MATCHING_DISTANCE
            },
            input_index: 0
        }
    }

    /// Gets the next token in the input
    fn get_next_token(&mut self, contexts: &dyn ContextProvider) -> Option<TokenKernel> {
        if self.data.has_run {
            return None;
        }
        loop {
            let mut result = run_dfa(
                &self.data.automaton,
                self.data.repository.text,
                self.input_index
            );
            if result.is_none() {
                // failed to match, retry with error handling
                result = run_fuzzy_matcher(
                    &self.data.repository,
                    &self.data.automaton,
                    self.data.separator_id,
                    self.data.recovery,
                    self.data.errors,
                    self.input_index
                );
            }
            if let Some(the_match) = result {
                if the_match.state == 0 {
                    // this is the dollar terminal, at the end of the input
                    // the index of the $ symbol is always 1
                    let token_index = self.data.repository.add(1, self.input_index, 0);
                    self.data.has_run = true;
                    return Some(TokenKernel {
                        terminal_id: SID_DOLLAR,
                        index: token_index as u32
                    });
                } else {
                    // matched something
                    let terminal_index = self.get_terminal_for(the_match.state, contexts);
                    let terminal_id = self.data.repository.terminals[terminal_index as usize].id;
                    if terminal_id != self.data.separator_id {
                        let token_index = self.data.repository.add(
                            terminal_index as usize,
                            self.input_index,
                            the_match.length as usize
                        );
                        self.input_index += the_match.length as usize;
                        return Some(TokenKernel {
                            terminal_id,
                            index: token_index as u32
                        });
                    } else {
                        self.input_index += the_match.length as usize;
                    }
                }
            } else {
                // skip this character
                self.input_index += 1;
            }
        }
    }

    /// Gets the index of the terminal with the highest priority that is possible in the contexts
    fn get_terminal_for(&self, state: u32, contexts: &dyn ContextProvider) -> u16 {
        let state_data = self.data.automaton.get_state(state);
        let mut matched = state_data.get_terminal(0);
        let mut result = matched.index;
        let mut id = self.data.repository.terminals[result as usize].id;
        if id == self.data.separator_id {
            // the separator trumps all
            return result;
        }
        let mut priority =
            contexts.get_context_priority(self.data.repository.get_count(), matched.context, id);
        for i in 1..state_data.get_terminals_count() {
            matched = state_data.get_terminal(i);
            id = self.data.repository.terminals[matched.index as usize].id;
            if id == self.data.separator_id {
                // the separator trumps all
                return matched.index;
            }
            let priority_candidate = contexts.get_context_priority(
                self.data.repository.get_count(),
                matched.context,
                id
            );
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

/// Represents a lexer
pub enum Lexer<'s, 't, 'a> {
    /// A context-free lexer
    ContextFree(ContextFreeLexer<'s, 't, 'a>),
    /// A context-sensitive lexer
    ContextSensitive(ContextSensitiveLexer<'s, 't, 'a>)
}

impl<'s, 't, 'a> Lexer<'s, 't, 'a> {
    /// Gets the data for the lexer
    pub fn get_data(&self) -> &LexerData<'s, 't, 'a> {
        match self {
            Lexer::ContextFree(lexer) => &lexer.data,
            Lexer::ContextSensitive(lexer) => &lexer.data
        }
    }

    /// Gets the data for the lexer
    pub fn get_data_mut(&mut self) -> &mut LexerData<'s, 't, 'a> {
        match self {
            Lexer::ContextFree(ref mut lexer) => &mut lexer.data,
            Lexer::ContextSensitive(ref mut lexer) => &mut lexer.data
        }
    }

    /// Gets the next token in the input
    pub fn get_next_token(&mut self, contexts: &dyn ContextProvider) -> Option<TokenKernel> {
        match self {
            Lexer::ContextFree(ref mut lexer) => lexer.get_next_token(),
            Lexer::ContextSensitive(ref mut lexer) => lexer.get_next_token(contexts)
        }
    }
}
