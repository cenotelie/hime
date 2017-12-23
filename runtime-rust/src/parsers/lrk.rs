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

//! Module for LR(k) parsers

use super::subtree::SubTree;
use super::super::ast::Ast;
use super::super::symbols::SemanticBody;
use super::super::symbols::SemanticElement;

/// Represents the builder of Parse Trees for LR(k) parsers
struct LRkAstBuilder<'a> {
    /// The stack of semantic objects
    stack: Vec<SubTree>,
    /// The sub-tree build-up cache
    cache: Option<SubTree>,
    /// The number of items popped from the stack
    pop_count: usize,
    /// The reduction handle represented as the indices of the sub-trees in the cache
    handle: Vec<usize>,
    /// The AST being built
    result: Ast<'a>
}

impl<'a> SemanticBody for LRkAstBuilder<'a> {
    fn length(&self) -> usize {
        self.handle.len()
    }

    fn get_element_at(&self, index: usize) -> SemanticElement {
        match self.cache {
            None => panic!("Not in a reduction"),
            Some(ref subtree) => {
                let label = subtree.get_label_at(self.handle[index]);
                self.result.get_semantic_element_for_label(label)
            }
        }
    }
}
