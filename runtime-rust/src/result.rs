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

use serde::ser::{Serialize, SerializeStruct, Serializer};

use crate::ast::{Ast, AstImpl};
use crate::errors::ParseErrors;
use crate::sppf::{Sppf, SppfImpl};
use crate::symbols::Symbol;
use crate::text::Text;
use crate::tokens::{TokenRepository, TokenRepositoryImpl};

/// Represents the output of a parser
pub struct ParseResult<'s, 't, 'a, T> {
    /// The table of grammar terminals
    pub terminals: &'a [Symbol<'s>],
    /// The table of grammar variables
    pub variables: &'a [Symbol<'s>],
    /// The table of grammar virtuals
    pub virtuals: &'a [Symbol<'s>],
    /// The input text
    pub text: Text<'t>,
    /// The errors found in the input
    pub errors: ParseErrors<'s>,
    /// The table of matched tokens
    pub tokens: TokenRepositoryImpl,
    /// The produced AST
    parse_tree: T,
}

impl<'s, 't, 'a, T: Default> ParseResult<'s, 't, 'a, T> {
    /// Initialize a new parse result
    #[must_use]
    pub fn new(
        terminals: &'a [Symbol<'s>],
        variables: &'a [Symbol<'s>],
        virtuals: &'a [Symbol<'s>],
        text: Text<'t>,
    ) -> ParseResult<'s, 't, 'a, T> {
        ParseResult {
            terminals,
            variables,
            virtuals,
            text,
            errors: ParseErrors::default(),
            tokens: TokenRepositoryImpl::default(),
            parse_tree: T::default(),
        }
    }

    /// Gets the token repository associated with this result
    #[must_use]
    pub fn get_tokens(&self) -> TokenRepository {
        TokenRepository::new(self.terminals, &self.text, &self.tokens)
    }
}

impl<'s, 't, 'a> ParseResult<'s, 't, 'a, AstImpl> {
    /// Gets whether this result denotes a successful parsing
    #[must_use]
    pub fn is_success(&self) -> bool {
        self.parse_tree.has_root()
    }

    /// Gets the resulting AST
    #[must_use]
    pub fn get_ast<'x>(&'x self) -> Ast<'s, 't, 'x> {
        Ast::new(
            TokenRepository::new(self.terminals, &self.text, &self.tokens),
            self.variables,
            self.virtuals,
            &self.parse_tree,
        )
    }

    /// Gets the mutable data required for parsing
    #[must_use]
    pub fn get_parsing_data<'x>(
        &'x mut self,
    ) -> (
        TokenRepository<'s, 't, 'x>,
        &'x mut ParseErrors<'s>,
        &'x mut AstImpl,
    ) {
        (
            TokenRepository::new_mut(self.terminals, &self.text, &mut self.tokens),
            &mut self.errors,
            &mut self.parse_tree,
        )
    }
}

impl<'s, 't, 'a> ParseResult<'s, 't, 'a, SppfImpl> {
    /// Gets whether this result denotes a successful parsing
    #[must_use]
    pub fn is_success(&self) -> bool {
        self.parse_tree.has_root()
    }

    /// Gets the resulting AST
    #[must_use]
    pub fn get_ast<'x>(&'x self) -> Sppf<'s, 't, 'x> {
        Sppf::new(
            TokenRepository::new(self.terminals, &self.text, &self.tokens),
            self.variables,
            self.virtuals,
            &self.parse_tree,
        )
    }

    /// Gets the mutable data required for parsing
    #[must_use]
    pub fn get_parsing_data<'x>(
        &'x mut self,
    ) -> (
        TokenRepository<'s, 't, 'x>,
        &'x mut ParseErrors<'s>,
        &'x mut SppfImpl,
    ) {
        (
            TokenRepository::new_mut(self.terminals, &self.text, &mut self.tokens),
            &mut self.errors,
            &mut self.parse_tree,
        )
    }
}

impl<'s, 't, 'a> Serialize for ParseResult<'s, 't, 'a, AstImpl> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
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

impl<'s, 't, 'a> Serialize for ParseResult<'s, 't, 'a, SppfImpl> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
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

/// A parse result with an AST
pub type ParseResultAst = ParseResult<'static, 'static, 'static, AstImpl>;

/// A parse result with a SPPF
pub type ParseResultSppf = ParseResult<'static, 'static, 'static, SppfImpl>;
