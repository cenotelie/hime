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

use super::symbols::Symbol;
use super::text::Text;
use super::tokens::TokenRepository;
use super::utils::biglist::BigList;

/// Represents a type of symbol table
#[derive(Copy, Clone, Eq, PartialEq)]
pub enum TableType {
    /// Marks as other (used for SPPF nodes)
    None = 0,
    /// Table of tokens
    Token = 1,
    /// Table of variables
    Variable = 2,
    /// Tables of virtuals
    Virtual = 3
}

impl From<usize> for TableType {
    fn from(x: usize) -> Self {
        match x {
            1 => TableType::Token,
            2 => TableType::Variable,
            3 => TableType::Virtual,
            _ => TableType::None,
        }
    }
}

/// Represents a compact reference to an element in a table
#[derive(Copy, Clone, Eq, PartialEq)]
pub struct TableElemRef {
    /// The backend data
    data: usize
}

impl TableElemRef {
    /// Initializes this reference
    pub fn new(t: TableType, index: usize) -> TableElemRef {
        TableElemRef {
            data: ((t as usize) << 30) | index
        }
    }

    /// Gets the element's type
    pub fn get_type(&self) -> TableType {
        TableType::from(self.data >> 30)
    }

    /// Gets the element's index in its respective table
    pub fn get_index(&self) -> usize {
        (self.data & 0x3FFFFFFF)
    }
}

/// Represents a cell in an AST inner structure
#[derive(Copy, Clone)]
pub struct AstCell {
    /// The node's label
    pub label: TableElemRef,
    /// The number of children
    pub count: u32,
    /// The index of the first child
    pub first: u32
}

impl AstCell {
    /// Initializes this node
    pub fn new_empty(label: TableElemRef) -> AstCell {
        AstCell { label, count: 0, first: 0 }
    }

    /// Initializes this node
    pub fn new(label: TableElemRef, count: u32, first: u32) -> AstCell {
        AstCell { label, count, first }
    }
}

/// Represents a simple AST with a tree structure
/// The nodes are stored in sequential arrays where the children of a node are an inner sequence.
/// The linkage is represented by each node storing its number of children and the index of its first child.
pub struct Ast<T: Text> {
    /// The table of tokens
    tokens: TokenRepository<T>,
    /// The table of variables
    variables: Vec<Symbol>,
    /// The table of virtuals
    virtuals: Vec<Symbol>,
    /// The nodes' labels
    nodes: BigList<AstCell>,
    /// The index of the tree's root node
    root: usize
}