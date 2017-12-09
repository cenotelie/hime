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
use super::automaton::run_dfa;
use super::context::ContextProvider;
use super::interface::Lexer;
use super::interface::TokenKernel;
use super::super::symbols::Symbol;
use super::super::text::interface::Text;
use super::super::tokens::TokenRepository;

/// The default maximum Levenshtein distance to go to for the recovery of a matching failure
const DEFAULT_RECOVERY_MATCHING_DISTANCE: u32 = 3;

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
    recovery: u32
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

    /// Gets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn get_recovery_distance(&self) -> u32 {
        self.recovery
    }

    /// Sets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn set_recovery_distance(&mut self, distance: u32) {
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
            recovery: DEFAULT_RECOVERY_MATCHING_DISTANCE
        }
    }

    /// Finds all the tokens in the lexer's input
    fn find_tokens(&mut self) {
        let mut index = 0;
        loop {
            let result = run_dfa(&self.automaton, self.repository.get_input(), index);
            if result.is_none() {
                // failed to match, retry with error handling
                // TODO: complete here
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