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

//! Module for the definition of a parse result

use super::ast::Ast;
use super::ast::AstImpl;
use super::errors::ParseErrors;
use super::symbols::Symbol;
use super::text::Text;
use super::tokens::TokenRepository;
use super::tokens::TokenRepositoryImpl;

/// Represents the output of a parser
pub struct ParseResult<T: Text> {
    /// The table of grammar terminals
    terminals: &'static Vec<Symbol>,
    /// The table of grammar variables
    variables: &'static Vec<Symbol>,
    /// The table of grammar virtuals
    virtuals: &'static Vec<Symbol>,
    /// The input text
    text: T,
    /// The errors found in the input
    errors: ParseErrors,
    /// The table of matched tokens
    tokens: TokenRepositoryImpl,
    /// The produced AST
    ast: AstImpl
}

impl<T: Text> ParseResult<T> {
    /// Initialize a new parse result
    pub fn new(terminals: &'static Vec<Symbol>, variables: &'static Vec<Symbol>, virtuals: &'static Vec<Symbol>, text: T) -> ParseResult<T> {
        ParseResult {
            terminals,
            variables,
            virtuals,
            text,
            errors: ParseErrors::new(),
            tokens: TokenRepositoryImpl::new(),
            ast: AstImpl::new()
        }
    }

    /// Gets the grammar terminals
    pub fn get_terminals(&self) -> &'static Vec<Symbol> {
        self.terminals
    }

    /// Gets the grammar variables
    pub fn get_variables(&self) -> &'static Vec<Symbol> {
        self.variables
    }

    /// Gets the grammar virtuals
    pub fn get_virtuals(&self) -> &'static Vec<Symbol> {
        self.virtuals
    }

    pub fn get_input(&self) -> &T {
        &self.text
    }

    /// Gets the collection of errors
    pub fn get_errors(&self) -> &ParseErrors {
        &self.errors
    }

    /// Gets the collection of errors
    pub fn get_errors_mut(&mut self) -> &mut ParseErrors {
        &mut self.errors
    }

    /// Gets the token repository associated with this result
    pub fn get_tokens(&mut self) -> TokenRepository<T> {
        TokenRepository::new(&self.terminals, &self.text, &mut self.tokens)
    }

    /*/// Gets the resulting AST
    pub fn get_ast(&mut self) -> Ast<T> {
        Ast::new(
            self.get_tokens(),
            self.variables,
            self.virtuals,
            &mut self.ast
        )
    }*/
}