/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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

//! Module for RNGLR parsers

use std::usize;

use super::*;
use super::subtree::SubTree;
use super::super::ast::Ast;
use super::super::ast::TableElemRef;
use super::super::ast::TableType;
use super::super::errors::ParseErrorUnexpectedToken;
use super::super::lexers::DEFAULT_CONTEXT;
use super::super::lexers::Lexer;
use super::super::lexers::TokenKernel;
use super::super::symbols::SemanticBody;
use super::super::symbols::SemanticElement;
use super::super::symbols::SemanticElementTrait;


/// Represents the RNGLR parsing table and productions
pub struct RNGLRAutomaton {
    /// The number of columns in the LR table
    columns_count: usize,
    /// The number of states in the LR table
    states_count: usize
}

impl RNGLRAutomaton {
    /// Initializes a new automaton from the given binary data
    pub fn new(data: &[u8]) -> RNGLRAutomaton {
        RNGLRAutomaton {
            columns_count: 0,
            states_count: 0
        }
    }
}

/// Represents the builder of Parse Trees for RNGLR parsers
struct RNGLRAstBuilder<'l> {
    /// Lexer associated to this parser
    lexer: &'l mut Lexer<'l>,
    /// The AST being built
    result: Ast<'l>
}

impl<'l> RNGLRAstBuilder<'l> {
    /// Initializes the builder with the given stack size
    pub fn new(lexer: &'l mut Lexer<'l>, result: Ast<'l>) -> RNGLRAstBuilder<'l> {
        RNGLRAstBuilder {
            lexer,
            result
        }
    }
}

struct RNGLRParserData<'a> {
    /// The parser's automaton
    automaton: RNGLRAutomaton,
    /// The semantic actions
    actions: &'a mut FnMut(usize, Symbol, &SemanticBody)
}

/// Represents a base for all RNGLR parsers
pub struct RNGLRParser<'l, 'a : 'l> {
    /// The parser's data
    data: RNGLRParserData<'a>,
    /// The AST builder
    builder: RNGLRAstBuilder<'l>
}

impl<'l, 'a : 'l> RNGLRParser<'l, 'a> {
    /// Initializes a new instance of the parser
    pub fn new(
        lexer: &'l mut Lexer<'l>,
        automaton: RNGLRAutomaton,
        ast: Ast<'l>,
        actions: &'a mut FnMut(usize, Symbol, &SemanticBody)
    ) -> RNGLRParser<'l, 'a> {
        RNGLRParser {
            data: RNGLRParserData {
                automaton,
                actions
            },
            builder: RNGLRAstBuilder::new(lexer, ast)
        }
    }
}

impl<'l, 'a> Parser for RNGLRParser<'l, 'a> {
    fn parse(&mut self) {
    }
}