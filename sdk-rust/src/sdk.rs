/*******************************************************************************
 * Copyright (c) 2020 Association Cénotélie (cenotelie.fr)
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

//! Module for SDK utilities

use hime_redist::ast::Ast;
use hime_redist::errors::ParseErrors;
use hime_redist::lexers::automaton::Automaton;
use hime_redist::lexers::impls::ContextFreeLexer;
use hime_redist::lexers::impls::ContextSensitiveLexer;
use hime_redist::lexers::Lexer;
use hime_redist::parsers::lrk::LRkAutomaton;
use hime_redist::parsers::lrk::LRkParser;
use hime_redist::parsers::rnglr::RNGLRAutomaton;
use hime_redist::parsers::rnglr::RNGLRParser;
use hime_redist::parsers::Parser;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticBody;
use hime_redist::symbols::Symbol;
use hime_redist::text::Text;
use hime_redist::tokens::TokenRepository;

/// The automaton for a parser
#[derive(Clone)]
pub enum ParserAutomaton {
    /// A LR(k) automaton
    Lrk(LRkAutomaton),
    /// A RNGLR automaton
    Rnglr(RNGLRAutomaton)
}

/// Represents complete data for a parser
#[derive(Clone)]
pub struct InMemoryParser<'a> {
    /// The name of the original grammar
    pub name: &'a str,
    /// The expected terminals
    pub terminals: Vec<Symbol<'a>>,
    /// The variables
    pub variables: Vec<Symbol<'a>>,
    /// The virtuals
    pub virtuals: Vec<Symbol<'a>>,
    /// The identifier of the separator terminal, if any
    pub separator: u32,
    /// The lexer's automaton
    pub lexer_automaton: Automaton,
    /// Whether the lexer is context-sensitive
    pub lexer_is_context_sensitive: bool,
    /// The parser's automaton
    pub parser_automaton: ParserAutomaton
}

impl<'a> InMemoryParser<'a> {
    /// Parses an input parser
    pub fn parse<'b>(&'b self, input: &str) -> ParseResult<'a, 'b>
    where
        'a: 'b
    {
        let text = Text::new(input);
        let mut result: ParseResult<'a, 'b> =
            ParseResult::new(&self.terminals, &self.variables, &self.virtuals, text);
        let mut my_actions = |_index: usize, _head: Symbol, _body: &dyn SemanticBody| ();
        {
            let data = result.get_parsing_data();
            let mut lexer = self.new_lexer(data.0, data.1);
            self.do_parse(&mut lexer, data.2, &mut my_actions);
        }
        result
    }

    /// Execute the parser
    fn do_parse<'b, 'c>(
        &'b self,
        lexer: &'c mut Lexer<'a, 'b, 'c>,
        ast: Ast<'a, 'b, 'c>,
        actions: &'c mut dyn FnMut(usize, Symbol, &dyn SemanticBody)
    ) where
        'a: 'b + 'c,
        'b: 'c
    {
        let mut parser: Box<dyn Parser> = match &self.parser_automaton {
            ParserAutomaton::Lrk(ref automaton) => {
                Box::new(LRkParser::new(lexer, automaton.clone(), ast, actions))
            }
            ParserAutomaton::Rnglr(ref automaton) => {
                Box::new(RNGLRParser::new(lexer, automaton.clone(), ast, actions))
            }
        };
        parser.parse();
    }

    /// Creates a new lexer
    fn new_lexer<'b, 'c>(
        &'b self,
        repository: TokenRepository<'a, 'b, 'c>,
        errors: &'b mut ParseErrors<'a>
    ) -> Lexer<'a, 'b, 'c>
    where
        'a: 'b + 'c,
        'b: 'c
    {
        if self.lexer_is_context_sensitive {
            Lexer::ContextFree(ContextFreeLexer::new(
                repository,
                errors,
                self.lexer_automaton.clone(),
                self.separator
            ))
        } else {
            Lexer::ContextSensitive(ContextSensitiveLexer::new(
                repository,
                errors,
                self.lexer_automaton.clone(),
                self.separator
            ))
        }
    }
}
