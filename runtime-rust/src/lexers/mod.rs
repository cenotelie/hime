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

//! Module for lexers API

pub mod automaton;
pub mod fuzzy;
pub mod impls;

use std::usize;

use super::errors::ParseErrors;
use super::symbols::Symbol;
use super::text::Text;
use super::tokens::TokenRepository;

/// Identifier of the default context
pub const DEFAULT_CONTEXT: u16 = 0;

/// Provides context information to a lexer
pub trait ContextProvider {
    /// Gets the priority of the specified context required by the specified terminal
    /// The priority is an unsigned integer. The lesser the value the higher the priority.
    /// The absence of value represents the unavailability of the required context.
    fn get_context_priority(
        &self,
        token_count: usize,
        context: u16,
        terminal_id: u32
    ) -> Option<usize>;
}

/// Implementation of the default context provider
pub struct DefaultContextProvider {}

impl ContextProvider for DefaultContextProvider {
    fn get_context_priority(
        &self,
        _token_count: usize,
        context: u16,
        _terminal_id: u32
    ) -> Option<usize> {
        if context == DEFAULT_CONTEXT {
            Some(usize::MAX)
        } else {
            Some(0)
        }
    }
}

/// Represents the kernel of a token, i.e. the identifying information of a token
pub struct TokenKernel {
    /// The identifier of the matched terminal
    pub terminal_id: u32,
    /// The token's index in its repository
    pub index: u32
}

/// The public interface of a lexer
pub trait Lexer<'a> {
    /// Gets the terminals matched by this lexer
    fn get_terminals(&self) -> &[Symbol];

    /// Gets the lexer's input text
    fn get_input(&self) -> &Text;

    /// Gets the lexer's output stream of tokens
    fn get_output(&self) -> &TokenRepository<'a>;

    /// Gets the lexer's errors
    fn get_errors(&self) -> &ParseErrors;

    /// Gets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn get_recovery_distance(&self) -> usize;

    /// Sets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn set_recovery_distance(&mut self, distance: usize);

    /// Gets the next token in the input
    fn get_next_token(&mut self, contexts: &ContextProvider) -> Option<TokenKernel>;
}
