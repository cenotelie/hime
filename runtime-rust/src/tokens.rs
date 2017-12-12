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

//! Structures and APIs for handling lexical tokens.

use super::symbols::SemanticElement;
use super::symbols::Symbol;
use super::symbols::SymbolType;
use super::text::Text;
use super::text::TextContext;
use super::text::TextPosition;
use super::text::TextSpan;
use super::utils::biglist::BigList;
use super::utils::iterable::Iterable;

/// Represents the metadata of a token
#[derive(Copy, Clone)]
struct TokenRepositoryCell {
    /// The terminal's index
    terminal: usize,
    /// The span of this token
    span: TextSpan
}

/// A repository of matched tokens
pub struct TokenRepository<T: Text> {
    /// The terminal symbols matched in this content
    terminals: Vec<Symbol>,
    /// The base text
    text: T,
    /// The token data in this content
    cells: BigList<TokenRepositoryCell>
}

/// Represents a token as an output element of a lexer
pub struct Token<'a, T: 'a + Text> {
    /// The repository containing this token
    repository: &'a TokenRepository<T>,
    /// The index of this token in the text
    index: usize
}

/// Implementation of `Clone` for `Token`
impl<'a, T: 'a + Text> Clone for Token<'a, T> {
    fn clone(&self) -> Self {
        Token {
            repository: self.repository,
            index: self.index
        }
    }
}

/// Implementation of `Copy` for `Token`
impl<'a, T: 'a + Text> Copy for Token<'a, T> {}

/// the iterator over the tokens in a repository
pub struct TokenRepositoryIterator<'a, T: 'a + Text> {
    /// The repository containing this token
    repository: &'a TokenRepository<T>,
    /// The current index within the repository
    index: usize
}

/// Implementation of `Iterator` for `TokenRepositoryIterator`
impl<'a, T: 'a + Text> Iterator for TokenRepositoryIterator<'a, T> {
    type Item = Token<'a, T>;
    fn next(&mut self) -> Option<Self::Item> {
        if self.index >= self.repository.cells.size() {
            None
        } else {
            let result = Token { repository: self.repository, index: self.index };
            self.index = self.index + 1;
            Some(result)
        }
    }
}

/// Implementation of `Iterable` for `TokenRepository`
impl<'a, T: 'a + Text> Iterable<'a> for TokenRepository<T> {
    type Item = Token<'a, T>;
    type IteratorType = TokenRepositoryIterator<'a, T>;
    fn iter(&'a self) -> Self::IteratorType {
        TokenRepositoryIterator {
            repository: self,
            index: 0
        }
    }
}

impl<T: Text> TokenRepository<T> {
    /// Creates a new repository
    pub fn new(terminals: Vec<Symbol>, text: T) -> TokenRepository<T> {
        TokenRepository {
            terminals,
            text,
            cells: BigList::<TokenRepositoryCell>::new(TokenRepositoryCell { terminal: 0, span: TextSpan { index: 0, length: 0 } })
        }
    }

    /// Registers a new token in this repository
    pub fn add(&mut self, terminal: usize, index: usize, length: usize) -> usize {
        self.cells.add(TokenRepositoryCell {
            terminal,
            span: TextSpan { index, length }
        })
    }

    /// Gets the terminals
    pub fn get_terminals(&self) -> &Vec<Symbol> {
        &self.terminals
    }

    /// Gets the input text
    pub fn get_input(&self) -> &T {
        &self.text
    }

    /// Gets the number of tokens in this repository
    pub fn get_tokens_count(&self) -> usize {
        self.cells.size()
    }

    /// Gets the terminal's identifier for the i-th token
    pub fn get_symbol_id_for(&self, index: usize) -> u32 {
        self.terminals[self.cells[index].terminal].id
    }
}

impl<'a, T: 'a + Text> SemanticElement for Token<'a, T> {
    /// Gets the type of symbol this element represents
    fn get_symbol_type(&self) -> SymbolType {
        SymbolType::Token
    }

    /// Gets the position in the input text of this element
    fn get_position(&self) -> TextPosition {
        self.repository.text.get_position_at(self.repository.cells[self.index].span.index)
    }

    /// Gets the span in the input text of this element
    fn get_span(&self) -> TextSpan {
        self.repository.cells[self.index].span
    }

    /// Gets the context of this element in the input
    fn get_context(&self) -> TextContext {
        self.repository.text.get_context_for(self.get_position(), self.repository.cells[self.index].span.length)
    }

    /// Gets the grammar symbol associated to this element
    fn get_symbol(&self) -> Symbol {
        self.repository.terminals[self.repository.cells[self.index].terminal]
    }

    /// Gets the value of this element, if any
    fn get_value(&self) -> Option<String> {
        Some(self.repository.text.get_value_for(self.repository.cells[self.index].span))
    }
}