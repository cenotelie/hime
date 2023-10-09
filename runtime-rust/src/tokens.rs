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
#[derive(Debug, Copy, Clone, Default)]
struct TokenRepositoryCell {
    /// The terminal's index
    terminal: usize,
    /// The span of this token
    span: TextSpan,
}

/// Implementation data of a repository of matched tokens
#[derive(Debug, Default, Clone)]
pub struct TokenRepositoryImpl {
    /// The token data in this content
    cells: BigList<TokenRepositoryCell>,
}

/// The proxy structure for a repository of matched tokens
pub struct TokenRepository<'s, 't, 'a> {
    /// The table of grammar terminals
    pub terminals: &'a [Symbol<'s>],
    /// The input text
    pub text: &'a Text<'t>,
    /// The table of matched tokens
    data: EitherMut<'a, TokenRepositoryImpl>,
}

/// Represents a token as an output element of a lexer
#[derive(Copy, Clone)]
pub struct Token<'s, 't, 'a> {
    /// The repository containing this token
    repository: &'a TokenRepository<'s, 't, 'a>,
    /// The index of this token in the text
    pub index: usize,
}

/// the iterator over the tokens in a repository
pub struct TokenRepositoryIterator<'s, 't, 'a> {
    /// The repository containing this token
    repository: &'a TokenRepository<'s, 't, 'a>,
    /// The current index within the repository
    index: usize,
}

/// Implementation of `Iterator` for `TokenRepositoryIterator`
impl<'s, 't, 'a> Iterator for TokenRepositoryIterator<'s, 't, 'a> {
    type Item = Token<'s, 't, 'a>;
    fn next(&mut self) -> Option<Self::Item> {
        if self.index >= self.repository.data.cells.len() {
            None
        } else {
            let result = Token {
                repository: self.repository,
                index: self.index,
            };
            self.index += 1;
            Some(result)
        }
    }
}

impl<'s, 't, 'a> TokenRepository<'s, 't, 'a> {
    /// Creates a new repository
    #[must_use]
    pub fn new(
        terminals: &'a [Symbol<'s>],
        text: &'a Text<'t>,
        tokens: &'a TokenRepositoryImpl,
    ) -> TokenRepository<'s, 't, 'a> {
        TokenRepository {
            terminals,
            text,
            data: EitherMut::Immutable(tokens),
        }
    }

    /// Creates a new mutable repository
    #[must_use]
    pub fn new_mut(
        terminals: &'a [Symbol<'s>],
        text: &'a Text<'t>,
        tokens: &'a mut TokenRepositoryImpl,
    ) -> TokenRepository<'s, 't, 'a> {
        TokenRepository {
            terminals,
            text,
            data: EitherMut::Mutable(tokens),
        }
    }

    /// Gets an iterator over the tokens
    #[must_use]
    pub fn iter(&self) -> TokenRepositoryIterator {
        TokenRepositoryIterator {
            repository: self,
            index: 0,
        }
    }

    /// Registers a new token in this repository
    pub fn add(&mut self, terminal: usize, index: usize, length: usize) -> usize {
        self.data.cells.push(TokenRepositoryCell {
            terminal,
            span: TextSpan { index, length },
        })
    }

    /// Gets the number of tokens in this repository
    #[must_use]
    pub fn get_tokens_count(&self) -> usize {
        self.data.cells.len()
    }

    /// Gets the terminal's identifier for the i-th token
    #[must_use]
    pub fn get_symbol_id_for(&self, index: usize) -> u32 {
        self.terminals[self.data.cells[index].terminal].id
    }

    /// Gets the i-th token
    #[must_use]
    pub fn get_token(&'a self, index: usize) -> Token<'s, 't, 'a> {
        Token {
            repository: self,
            index,
        }
    }

    /// Gets the number of tokens
    #[must_use]
    pub fn get_count(&self) -> usize {
        self.data.cells.len()
    }

    /// Gets the token (if any) that contains the specified index in the input text
    #[must_use]
    pub fn find_token_at(&'a self, index: usize) -> Option<Token<'s, 't, 'a>> {
        let count = self.data.cells.len();
        if count == 0 {
            return None;
        }
        let mut l: usize = 0;
        let mut r = count - 1;
        while l <= r {
            let m = (l + r) / 2;
            let cell = self.data.cells[m];
            if index < cell.span.index {
                // look on the left
                r = m - 1;
            } else if index < cell.span.index + cell.span.length {
                // within the token
                return Some(Token {
                    repository: self,
                    index: m,
                });
            } else {
                // look on the right
                l = m + 1;
            }
        }
        None
    }
}

impl<'s, 't, 'a> SemanticElementTrait<'s, 'a> for Token<'s, 't, 'a> {
    /// Gets the position in the input text of this element
    #[must_use]
    fn get_position(&self) -> Option<TextPosition> {
        Some(
            self.repository
                .text
                .get_position_at(self.repository.data.cells[self.index].span.index),
        )
    }

    /// Gets the span in the input text of this element
    #[must_use]
    fn get_span(&self) -> Option<TextSpan> {
        Some(self.repository.data.cells[self.index].span)
    }

    /// Gets the context of this element in the input
    #[must_use]
    fn get_context(&self) -> Option<TextContext<'a>> {
        Some(self.repository.text.get_context_for(
            self.get_position().unwrap(),
            self.repository.data.cells[self.index].span.length,
        ))
    }

    /// Gets the grammar symbol associated to this element
    #[must_use]
    fn get_symbol(&self) -> Symbol<'s> {
        self.repository.terminals[self.repository.data.cells[self.index].terminal]
    }

    /// Gets the value of this element, if any
    #[must_use]
    fn get_value(&self) -> Option<&'a str> {
        Some(
            self.repository
                .text
                .get_value_for(self.repository.data.cells[self.index].span),
        )
    }
}
