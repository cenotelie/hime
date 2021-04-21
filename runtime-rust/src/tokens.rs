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

//! Module for the definition of lexical tokens

use crate::symbols::{SemanticElementTrait, Symbol};
use crate::text::{Text, TextContext, TextPosition, TextSpan};
use crate::utils::biglist::BigList;
use crate::utils::EitherMut;

/// Represents the metadata of a token
#[derive(Debug, Copy, Clone)]
struct TokenRepositoryCell {
    /// The terminal's index
    terminal: usize,
    /// The span of this token
    span: TextSpan
}

/// Implementation data of a repository of matched tokens
pub struct TokenRepositoryImpl {
    /// The token data in this content
    cells: BigList<TokenRepositoryCell>
}

impl Default for TokenRepositoryImpl {
    fn default() -> Self {
        Self::new()
    }
}

impl TokenRepositoryImpl {
    /// Creates a new implementation of a token repository
    pub fn new() -> TokenRepositoryImpl {
        TokenRepositoryImpl {
            cells: BigList::new(TokenRepositoryCell {
                terminal: 0,
                span: TextSpan {
                    index: 0,
                    length: 0
                }
            })
        }
    }
}

/// The proxy structure for a repository of matched tokens
pub struct TokenRepository<'a: 'b, 'b, 'c> {
    /// The table of grammar terminals
    pub terminals: &'b [Symbol<'a>],
    /// The input text
    pub text: &'c Text,
    /// The table of matched tokens
    data: EitherMut<'c, TokenRepositoryImpl>
}

/// Represents a token as an output element of a lexer
pub struct Token<'a: 'b + 'd, 'b: 'd, 'c, 'd> {
    /// The repository containing this token
    repository: &'d TokenRepository<'a, 'b, 'c>,
    /// The index of this token in the text
    pub index: usize
}

/// Implementation of `Clone` for `Token`
impl<'a: 'b + 'd, 'b: 'd, 'c, 'd> Clone for Token<'a, 'b, 'c, 'd> {
    fn clone(&self) -> Self {
        Token {
            repository: self.repository,
            index: self.index
        }
    }
}

/// Implementation of `Copy` for `Token`
impl<'a: 'b + 'd, 'b: 'd, 'c, 'd> Copy for Token<'a, 'b, 'c, 'd> {}

/// the iterator over the tokens in a repository
pub struct TokenRepositoryIterator<'a: 'b + 'd, 'b: 'd, 'c, 'd> {
    /// The repository containing this token
    repository: &'d TokenRepository<'a, 'b, 'c>,
    /// The current index within the repository
    index: usize
}

/// Implementation of `Iterator` for `TokenRepositoryIterator`
impl<'a: 'b + 'd, 'b: 'd, 'c, 'd> Iterator for TokenRepositoryIterator<'a, 'b, 'c, 'd> {
    type Item = Token<'a, 'b, 'c, 'd>;
    fn next(&mut self) -> Option<Self::Item> {
        if self.index >= self.repository.data.get().cells.len() {
            None
        } else {
            let result = Token {
                repository: self.repository,
                index: self.index
            };
            self.index += 1;
            Some(result)
        }
    }
}

impl<'a: 'b, 'b, 'c> TokenRepository<'a, 'b, 'c> {
    /// Creates a new repository
    pub fn new(
        terminals: &'b [Symbol<'a>],
        text: &'c Text,
        tokens: &'c TokenRepositoryImpl
    ) -> TokenRepository<'a, 'b, 'c> {
        TokenRepository {
            terminals,
            text,
            data: EitherMut::Immutable(tokens)
        }
    }

    /// Creates a new mutable repository
    pub fn new_mut(
        terminals: &'b [Symbol<'a>],
        text: &'c Text,
        tokens: &'c mut TokenRepositoryImpl
    ) -> TokenRepository<'a, 'b, 'c> {
        TokenRepository {
            terminals,
            text,
            data: EitherMut::Mutable(tokens)
        }
    }

    /// Gets an iterator over the tokens
    pub fn iter(&self) -> TokenRepositoryIterator {
        TokenRepositoryIterator {
            repository: self,
            index: 0
        }
    }

    /// Registers a new token in this repository
    pub fn add(&mut self, terminal: usize, index: usize, length: usize) -> usize {
        let x = self.data.get_mut();
        match x {
            None => panic!("Got a mutable token repository with an immutable implementation"),
            Some(data) => data.cells.push(TokenRepositoryCell {
                terminal,
                span: TextSpan { index, length }
            })
        }
    }

    /// Gets the number of tokens in this repository
    pub fn get_tokens_count(&self) -> usize {
        self.data.get().cells.len()
    }

    /// Gets the terminal's identifier for the i-th token
    pub fn get_symbol_id_for(&self, index: usize) -> u32 {
        self.terminals[self.data.get().cells[index].terminal].id
    }

    /// Gets the i-th token
    pub fn get_token<'x>(&'x self, index: usize) -> Token<'a, 'b, 'c, 'x> {
        Token {
            repository: &self,
            index
        }
    }

    /// Gets the number of tokens
    pub fn get_count(&self) -> usize {
        self.data.get().cells.len()
    }

    /// Gets the token (if any) that contains the specified index in the input text
    pub fn find_token_at(&self, index: usize) -> Option<Token> {
        let data = self.data.get();
        let count = data.cells.len();
        if count == 0 {
            return None;
        }
        let mut l: usize = 0;
        let mut r = count - 1;
        while l <= r {
            let m = (l + r) / 2;
            let cell = data.cells[m];
            if index < cell.span.index {
                // look on the left
                r = m - 1;
            } else if index < cell.span.index + cell.span.length {
                // within the token
                return Some(Token {
                    repository: self,
                    index: m
                });
            } else {
                // look on the right
                l = m + 1;
            }
        }
        None
    }
}

impl<'a: 'b + 'd, 'b: 'd, 'c, 'd> SemanticElementTrait<'a> for Token<'a, 'b, 'c, 'd> {
    /// Gets the position in the input text of this element
    fn get_position(&self) -> Option<TextPosition> {
        Some(
            self.repository
                .text
                .get_position_at(self.repository.data.get().cells[self.index].span.index)
        )
    }

    /// Gets the span in the input text of this element
    fn get_span(&self) -> Option<TextSpan> {
        Some(self.repository.data.get().cells[self.index].span)
    }

    /// Gets the context of this element in the input
    fn get_context(&self) -> Option<TextContext> {
        Some(self.repository.text.get_context_for(
            self.get_position().unwrap(),
            self.repository.data.get().cells[self.index].span.length
        ))
    }

    /// Gets the grammar symbol associated to this element
    fn get_symbol(&self) -> Symbol<'a> {
        self.repository.terminals[self.repository.data.get().cells[self.index].terminal]
    }

    /// Gets the value of this element, if any
    fn get_value(&self) -> Option<String> {
        Some(
            self.repository
                .text
                .get_value_for(self.repository.data.get().cells[self.index].span)
        )
    }
}
