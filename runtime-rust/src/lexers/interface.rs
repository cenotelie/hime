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

use super::context::ContextProvider;
use super::super::symbols::Symbol;
use super::super::text::interface::Text;
use super::super::tokens::TokenRepository;

/// Represents the kernel of a token, i.e. the identifying information of a token
pub struct TokenKernel {
    /// The identifier of the matched terminal
    pub terminal_id: u32,
    /// The token's index in its repository
    pub index: u32
}

/// The public interface of a lexer
pub trait Lexer<T: Text> {
    /// Gets the terminals matched by this lexer
    fn get_terminals(&self) -> &Vec<Symbol>;

    /// Gets the lexer's input text
    fn get_input(&self) -> &Text;

    /// Gets the lexer's output stream of tokens
    fn get_output(&self) -> &TokenRepository<T>;

    /// Gets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn get_recovery_distance(&self) -> u32;

    /// Sets the maximum Levenshtein distance to go to for the recovery of a matching failure.
    /// A distance of 0 indicates no recovery.
    fn set_recovery_distance(&mut self, distance: u32);

    /// Gets the next token in the input
    fn get_next_token(&mut self, contexts: ContextProvider) -> Option<TokenKernel>;
}