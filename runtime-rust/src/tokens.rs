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

use super::symbols::SemanticElementTrait;
use super::symbols::Symbol;
use super::text::Text;
use super::text::TextContext;
use super::text::TextPosition;
use super::text::TextSpan;
use super::utils::EitherMut;
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

/// Implementation data of a repository of matched tokens
pub struct TokenRepositoryImpl {
    /// The token data in this content
    cells: BigList<TokenRepositoryCell>
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
pub struct TokenRepository<'a> {
    /// The table of grammar terminals
    terminals: &'static Vec<Symbol>,
    /// The input text
    text: &'a Text,
    /// The table of matched tokens
    data: EitherMut<'a, TokenRepositoryImpl>
}

/// Represents a token as an output element of a lexer
pub struct Token<'a> {
    /// The repository containing this token
    repository: &'a TokenRepository<'a>,
    /// The index of this token in the text
    index: usize
}

/// Implementation of `Clone` for `Token`
impl<'a> Clone for Token<'a> {
    fn clone(&self) -> Self {
        Token {
            repository: self.repository,
            index: self.index
        }
    }
}

/// Implementation of `Copy` for `Token`
impl<'a> Copy for Token<'a> {}

/// the iterator over the tokens in a repository
pub struct TokenRepositoryIterator<'a> {
    /// The repository containing this token
    repository: &'a TokenRepository<'a>,
    /// The current index within the repository
    index: usize
}

/// Implementation of `Iterator` for `TokenRepositoryIterator`
impl<'a> Iterator for TokenRepositoryIterator<'a> {
    type Item = Token<'a>;
    fn next(&mut self) -> Option<Self::Item> {
        if self.index >= self.repository.data.get().cells.size() {
            None
        } else {
            let result = Token {
                repository: self.repository,
                index: self.index
            };
            self.index = self.index + 1;
            Some(result)
        }
    }
}

/// Implementation of `Iterable` for `TokenRepository`
impl<'a> Iterable<'a> for TokenRepository<'a> {
    type Item = Token<'a>;
    type IteratorType = TokenRepositoryIterator<'a>;
    fn iter(&'a self) -> Self::IteratorType {
        TokenRepositoryIterator {
            repository: self,
            index: 0
        }
    }
}

impl<'a> TokenRepository<'a> {
    /// Creates a new repository
    pub fn new(
        terminals: &'static Vec<Symbol>,
        text: &'a Text,
        tokens: &'a TokenRepositoryImpl
    ) -> TokenRepository<'a> {
        TokenRepository {
            terminals,
            text,
            data: EitherMut::Immutable(tokens)
        }
    }

    /// Creates a new mutable repository
    pub fn new_mut(
        terminals: &'static Vec<Symbol>,
        text: &'a Text,
        tokens: &'a mut TokenRepositoryImpl
    ) -> TokenRepository<'a> {
        TokenRepository {
            terminals,
            text,
            data: EitherMut::Mutable(tokens)
        }
    }

    /// Registers a new token in this repository
    pub fn add(&mut self, terminal: usize, index: usize, length: usize) -> usize {
        let x = self.data.get_mut();
        match x {
            None => panic!("Got a mutable token repository with an immutable implementation"),
            Some(data) => data.cells.add(TokenRepositoryCell {
                terminal,
                span: TextSpan { index, length }
            })
        }
    }

    /// Gets the terminals
    pub fn get_terminals(&self) -> &Vec<Symbol> {
        &self.terminals
    }

    /// Gets the input text
    pub fn get_input(&self) -> &Text {
        &self.text
    }

    /// Gets the number of tokens in this repository
    pub fn get_tokens_count(&self) -> usize {
        self.data.get().cells.size()
    }

    /// Gets the terminal's identifier for the i-th token
    pub fn get_symbol_id_for(&self, index: usize) -> u32 {
        self.terminals[self.data.get().cells[index].terminal].id
    }

    /// Gets the i-th token
    pub fn get_token(&'a self, index: usize) -> Token<'a> {
        Token {
            repository: &self,
            index
        }
    }
}

impl<'a> SemanticElementTrait for Token<'a> {
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
    fn get_symbol(&self) -> Symbol {
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
