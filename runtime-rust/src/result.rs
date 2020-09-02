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

use crate::ast::Ast;
use crate::ast::AstImpl;
use crate::errors::ParseErrors;
use crate::symbols::Symbol;
use crate::text::Text;
use crate::tokens::TokenRepository;
use crate::tokens::TokenRepositoryImpl;
use serde::ser::{Serialize, SerializeStruct, Serializer};

/// Represents the output of a parser
pub struct ParseResult<'a: 'b, 'b> {
    /// The table of grammar terminals
    pub terminals: &'b [Symbol<'a>],
    /// The table of grammar variables
    pub variables: &'b [Symbol<'a>],
    /// The table of grammar virtuals
    pub virtuals: &'b [Symbol<'a>],
    /// The input text
    pub text: Text,
    /// The errors found in the input
    pub errors: ParseErrors<'a>,
    /// The table of matched tokens
    pub tokens: TokenRepositoryImpl,
    /// The produced AST
    ast: AstImpl
}

impl<'a: 'b, 'b> ParseResult<'a, 'b> {
    /// Initialize a new parse result
    pub fn new(
        terminals: &'b [Symbol<'a>],
        variables: &'b [Symbol<'a>],
        virtuals: &'b [Symbol<'a>],
        text: Text
    ) -> ParseResult<'a, 'b> {
        ParseResult {
            terminals,
            variables,
            virtuals,
            text,
            errors: ParseErrors::default(),
            tokens: TokenRepositoryImpl::new(),
            ast: AstImpl::new()
        }
    }

    /// Gets whether this result denotes a successful parsing
    pub fn is_success(&self) -> bool {
        self.ast.has_root()
    }

    /// Gets the token repository associated with this result
    pub fn get_tokens(&self) -> TokenRepository {
        TokenRepository::new(&self.terminals, &self.text, &self.tokens)
    }

    /// Gets the resulting AST
    pub fn get_ast<'x>(&'x self) -> Ast<'a, 'b, 'x> {
        Ast::new(
            TokenRepository::new(&self.terminals, &self.text, &self.tokens),
            self.variables,
            self.virtuals,
            &self.ast
        )
    }

    /// Gets the mutable data required for parsing
    pub fn get_parsing_data<'x>(
        &'x mut self
    ) -> (
        TokenRepository<'a, 'b, 'x>,
        &mut ParseErrors<'a>,
        Ast<'a, 'b, 'x>
    ) {
        (
            TokenRepository::new_mut(&self.terminals, &self.text, &mut self.tokens),
            &mut self.errors,
            Ast::new_mut(self.variables, self.virtuals, &mut self.ast)
        )
    }
}

impl<'a: 'b, 'b> Serialize for ParseResult<'a, 'b> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer
    {
        let ast = self.get_ast();
        let root = if self.is_success() {
            Some(ast.get_root())
        } else {
            None
        };
        let mut state = serializer.serialize_struct("ParseResult", 2)?;
        state.serialize_field("errors", &self.errors.errors)?;
        state.serialize_field("root", &root)?;
        state.end()
    }
}
