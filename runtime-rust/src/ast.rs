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

//! Module for Abstract-Syntax Trees

use super::symbols::SemanticElementTrait;
use super::symbols::Symbol;
use super::text::TextContext;
use super::text::TextPosition;
use super::text::TextSpan;
use super::tokens::Token;
use super::tokens::TokenRepository;
use super::utils::EitherMut;
use super::utils::biglist::BigList;
use super::utils::iterable::Iterable;

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
            _ => TableType::None
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
        AstCell {
            label,
            count: 0,
            first: 0
        }
    }

    /// Initializes this node
    pub fn new(label: TableElemRef, count: u32, first: u32) -> AstCell {
        AstCell {
            label,
            count,
            first
        }
    }
}

/// Implementation of a simple AST with a tree structure
/// The nodes are stored in sequential arrays where the children of a node are an inner sequence.
/// The linkage is represented by each node storing its number of children and the index of its first child.
pub struct AstImpl {
    /// The nodes' labels
    nodes: BigList<AstCell>,
    /// The index of the tree's root node
    root: Option<usize>
}

impl AstImpl {
    /// Creates a new implementation
    pub fn new() -> AstImpl {
        AstImpl {
            nodes: BigList::<AstCell>::new(AstCell::new(
                TableElemRef::new(TableType::None, 0),
                0,
                0
            )),
            root: None
        }
    }
}

/// Represents a simple AST with a tree structure
/// The nodes are stored in sequential arrays where the children of a node are an inner sequence.
/// The linkage is represented by each node storing its number of children and the index of its first child.
pub struct Ast<'a> {
    /// The table of tokens
    tokens: Option<TokenRepository<'a>>,
    /// The table of variables
    variables: &'static [Symbol],
    /// The table of virtuals
    virtuals: &'static [Symbol],
    /// The data of the implementation
    data: EitherMut<'a, AstImpl>
}

impl<'a> Ast<'a> {
    /// Creates a new AST proxy structure
    pub fn new(
        tokens: TokenRepository<'a>,
        variables: &'static [Symbol],
        virtuals: &'static [Symbol],
        data: &'a AstImpl
    ) -> Ast<'a> {
        Ast {
            tokens: Some(tokens),
            variables,
            virtuals,
            data: EitherMut::Immutable(data)
        }
    }

    /// Creates a new AST proxy structure
    pub fn new_mut(
        variables: &'static [Symbol],
        virtuals: &'static [Symbol],
        data: &'a mut AstImpl
    ) -> Ast<'a> {
        Ast {
            tokens: None,
            variables,
            virtuals,
            data: EitherMut::Mutable(data)
        }
    }

    /// Gets the i-th token in the associated repository
    fn get_token(&self, index: usize) -> Token {
        match self.tokens {
            None => panic!("Missing token repository"),
            Some(ref x) => x.get_token(index)
        }
    }

    /// Gets the grammar variables for this AST
    pub fn get_variables(&self) -> &'static [Symbol] {
        &self.variables
    }

    /// Gets the grammar virtuals for this AST
    pub fn get_virtuals(&self) -> &'static [Symbol] {
        &self.virtuals
    }

    /// Gets the root node of this tree
    pub fn get_root(&self) -> AstNode {
        let data = self.data.get();
        match data.root {
            None => panic!("No root defined!"),
            Some(x) => AstNode {
                tree: self,
                index: x
            }
        }
    }

    /// Stores some children nodes in this AST
    pub fn store(&mut self, nodes: &Vec<AstCell>, index: usize, count: usize) -> usize {
        if count == 0 {
            0
        } else {
            match self.data.get_mut() {
                None => panic!("Got a mutable AST with an immutable implementation"),
                Some(data) => {
                    let result = data.nodes.add(nodes[index]);
                    for i in 1..count {
                        data.nodes.add(nodes[index + i]);
                    }
                    result
                }
            }
        }
    }

    /// Stores the root of this tree
    pub fn store_root(&mut self, node: AstCell) {
        match self.data.get_mut() {
            None => panic!("Got a mutable AST with an immutable implementation"),
            Some(data) => data.root = Some(data.nodes.add(node))
        }
    }
}

/// Represents a node in an Abstract Syntax Tree
#[derive(Clone)]
pub struct AstNode<'a> {
    /// The original parse tree
    tree: &'a Ast<'a>,
    /// The index of this node in the parse tree
    index: usize
}

impl<'a> AstNode<'a> {
    /// Gets the children of this node
    pub fn children(&self) -> AstFamily {
        AstFamily {
            tree: self.tree,
            parent: self.index
        }
    }
}

impl<'a> SemanticElementTrait for AstNode<'a> {
    /// Gets the position in the input text of this element
    fn get_position(&self) -> Option<TextPosition> {
        let cell = self.tree.data.get().nodes[self.index];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.get_index());
                token.get_position()
            }
            _ => None
        }
    }

    /// Gets the span in the input text of this element
    fn get_span(&self) -> Option<TextSpan> {
        let cell = self.tree.data.get().nodes[self.index];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.get_index());
                token.get_span()
            }
            _ => None
        }
    }

    /// Gets the context of this element in the input
    fn get_context(&self) -> Option<TextContext> {
        let cell = self.tree.data.get().nodes[self.index];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.get_index());
                token.get_context()
            }
            _ => None
        }
    }

    /// Gets the grammar symbol associated to this element
    fn get_symbol(&self) -> Symbol {
        let cell = self.tree.data.get().nodes[self.index];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.get_index());
                token.get_symbol()
            }
            TableType::Variable => self.tree.variables[cell.label.get_index()],
            TableType::Virtual => self.tree.virtuals[cell.label.get_index()],
            _ => panic!("Undefined symbol")
        }
    }

    /// Gets the value of this element, if any
    fn get_value(&self) -> Option<String> {
        let cell = self.tree.data.get().nodes[self.index];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.get_index());
                token.get_value()
            }
            _ => None
        }
    }
}

/// Represents a family of children for an ASTNode
#[derive(Clone)]
pub struct AstFamily<'a> {
    /// The original parse tree
    tree: &'a Ast<'a>,
    /// The index of the parent node in the parse tree
    parent: usize
}

/// Represents and iterator for adjacents in this graph
pub struct AstFamilyIterator<'a> {
    /// The original parse tree
    tree: &'a Ast<'a>,
    /// The index of the current child in the parse tree
    current: usize,
    /// the index of the last child (excluded) in the parse tree
    end: usize
}

/// Implementation of the `Iterator` trait for `AstFamilyIterator`
impl<'a> Iterator for AstFamilyIterator<'a> {
    type Item = AstNode<'a>;
    fn next(&mut self) -> Option<Self::Item> {
        if self.current >= self.end {
            None
        } else {
            let result = AstNode {
                tree: self.tree,
                index: self.current
            };
            self.current += 1;
            Some(result)
        }
    }
}

/// Implementation of the `Iterable` trait for `AstFamily`
impl<'a> Iterable<'a> for AstFamily<'a> {
    type Item = AstNode<'a>;
    type IteratorType = AstFamilyIterator<'a>;
    fn iter(&'a self) -> Self::IteratorType {
        let cell = self.tree.data.get().nodes[self.parent];
        AstFamilyIterator {
            tree: self.tree,
            current: cell.first as usize,
            end: (cell.first + cell.count) as usize
        }
    }
}

impl<'a> AstFamily<'a> {
    /// Gets the number of children in this family
    pub fn count(&self) -> usize {
        self.tree.data.get().nodes[self.parent].count as usize
    }

    /// Gets the i-th child
    pub fn at(&self, index: usize) -> AstNode {
        let cell = self.tree.data.get().nodes[self.parent];
        AstNode {
            tree: self.tree,
            index: cell.first as usize + index
        }
    }
}
