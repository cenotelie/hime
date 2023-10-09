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

use alloc::fmt::{Display, Error, Formatter};
use core::iter::FusedIterator;

use serde::ser::{Serialize, SerializeSeq, SerializeStruct, Serializer};

use crate::symbols::{SemanticElementTrait, Symbol};
use crate::text::{TextContext, TextPosition, TextSpan};
use crate::tokens::{Token, TokenRepository};
use crate::utils::biglist::BigList;

/// Represents a type of symbol table
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub enum TableType {
    /// Marks as other (used for SPPF nodes)
    None = 0,
    /// Table of tokens
    Token = 1,
    /// Table of variables
    Variable = 2,
    /// Tables of virtuals
    Virtual = 3,
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
#[derive(Debug, Default, Copy, Clone, Eq, PartialEq, Hash)]
pub struct TableElemRef {
    /// The backend data
    data: usize,
}

impl TableElemRef {
    /// Initializes this reference
    #[must_use]
    pub fn new(t: TableType, index: usize) -> TableElemRef {
        TableElemRef {
            data: ((t as usize) << 30) | index,
        }
    }

    /// Gets the element's type
    #[must_use]
    pub fn table_type(self) -> TableType {
        TableType::from(self.data >> 30)
    }

    /// Gets the element's index in its respective table
    #[must_use]
    pub fn index(self) -> usize {
        self.data & 0x3FFF_FFFF
    }

    /// Formats this table reference
    ///
    /// # Errors
    ///
    /// Propagates the error from `write!`
    pub fn fmt(
        &self,
        f: &mut Formatter<'_>,
        variables: &[Symbol],
        virtuals: &[Symbol],
    ) -> Result<(), Error> {
        match self.table_type() {
            TableType::None => write!(f, "none[{}]", self.index())?,
            TableType::Token => write!(f, "token[{}]", self.index())?,
            TableType::Variable => write!(f, "{}", variables[self.index()].name)?,
            TableType::Virtual => write!(f, "{}", virtuals[self.index()].name)?,
        }

        Ok(())
    }
}

/// Represents a cell in an AST inner structure
#[derive(Debug, Default, Copy, Clone)]
pub struct AstCell {
    /// The node's label
    pub label: TableElemRef,
    /// The number of children
    pub count: u32,
    /// The index of the first child
    pub first: u32,
}

impl AstCell {
    /// Initializes this node
    #[must_use]
    pub fn new_empty(label: TableElemRef) -> AstCell {
        AstCell {
            label,
            count: 0,
            first: 0,
        }
    }

    /// Initializes this node
    #[must_use]
    pub fn new(label: TableElemRef, count: u32, first: u32) -> AstCell {
        AstCell {
            label,
            count,
            first,
        }
    }
}

/// Implementation of a simple AST with a tree structure
/// The nodes are stored in sequential arrays where the children of a node are an inner sequence.
/// The linkage is represented by each node storing its number of children and the index of its first child.
#[derive(Debug, Default, Clone)]
pub struct AstImpl {
    /// The nodes' labels
    nodes: BigList<AstCell>,
    /// The index of the tree's root node
    root: Option<usize>,
}

impl AstImpl {
    /// Gets whether a root has been defined for this AST
    #[must_use]
    pub fn has_root(&self) -> bool {
        self.root.is_some()
    }

    /// Stores the root of this tree
    pub fn store_root(&mut self, node: AstCell) {
        self.root = Some(self.nodes.push(node));
    }

    /// Stores some children nodes in this AST
    #[must_use]
    pub fn store(&mut self, nodes: &[AstCell], index: usize, count: usize) -> usize {
        if count == 0 {
            0
        } else {
            let result = self.nodes.push(nodes[index]);
            for i in 1..count {
                self.nodes.push(nodes[index + i]);
            }
            result
        }
    }
}

/// Represents a simple AST with a tree structure
/// The nodes are stored in sequential arrays where the children of a node are an inner sequence.
/// The linkage is represented by each node storing its number of children and the index of its first child.
pub struct Ast<'s, 't, 'a> {
    /// The table of tokens
    tokens: TokenRepository<'s, 't, 'a>,
    /// The table of variables
    variables: &'a [Symbol<'s>],
    /// The table of virtuals
    virtuals: &'a [Symbol<'s>],
    /// The data of the implementation
    data: &'a AstImpl,
}

impl<'s, 't, 'a> Ast<'s, 't, 'a> {
    /// Creates a new AST proxy structure
    #[must_use]
    pub fn new(
        tokens: TokenRepository<'s, 't, 'a>,
        variables: &'a [Symbol<'s>],
        virtuals: &'a [Symbol<'s>],
        data: &'a AstImpl,
    ) -> Ast<'s, 't, 'a> {
        Ast {
            tokens,
            variables,
            virtuals,
            data,
        }
    }

    /// Gets the i-th token in the associated repository
    fn get_token(&'a self, index: usize) -> Token<'s, 't, 'a> {
        self.tokens.get_token(index)
    }

    /// Gets whether a root has been defined for this AST
    #[must_use]
    pub fn has_root(&self) -> bool {
        self.data.has_root()
    }

    /// Gets the root node of this tree
    ///
    /// # Panics
    ///
    /// Raise a panic when the AST has no root.
    /// This can happen when a `ParseResult` contains some errors,
    /// but the AST `get_ast` is called to get an AST but it will have not root.
    #[must_use]
    pub fn get_root(&'a self) -> AstNode<'s, 't, 'a> {
        self.data
            .root
            .map(|x| AstNode {
                tree: self,
                index: x,
            })
            .expect("No root defined!")
    }

    /// Gets a specific node in this tree
    #[must_use]
    pub fn get_node(&'a self, id: usize) -> AstNode<'s, 't, 'a> {
        AstNode {
            tree: self,
            index: id,
        }
    }

    /// Gets the AST node (if any) that has the specified token as label
    #[must_use]
    pub fn find_node_for(&'a self, token: &Token<'s, 't, 'a>) -> Option<AstNode<'s, 't, 'a>> {
        self.data
            .nodes
            .iter()
            .enumerate()
            .find(|(_, node)| {
                node.label.table_type() == TableType::Token && node.label.index() == token.index
            })
            .map(|(index, _)| AstNode { tree: self, index })
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    #[must_use]
    pub fn find_node_at_index(&'a self, index: usize) -> Option<AstNode<'s, 't, 'a>> {
        self.tokens
            .find_token_at(index)
            .and_then(|token| self.find_node_for(&token))
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    #[must_use]
    pub fn find_node_at_position(&'a self, position: TextPosition) -> Option<AstNode<'s, 't, 'a>> {
        let index = self.tokens.text.get_line_index(position.line) + position.column - 1;
        self.tokens
            .find_token_at(index)
            .and_then(|token| self.find_node_for(&token))
    }

    /// Gets the parent of the specified node, if any
    #[must_use]
    pub fn find_parent_of(&'a self, node: usize) -> Option<AstNode<'s, 't, 'a>> {
        self.data
            .nodes
            .iter()
            .enumerate()
            .find(|(_, candidate)| {
                candidate.count > 0
                    && node >= candidate.first as usize
                    && node < (candidate.first + candidate.count) as usize
            })
            .map(|(index, _)| AstNode { tree: self, index })
    }

    /// Gets the total span of sub-tree given its root and its position
    #[must_use]
    pub fn get_total_position_and_span(&self, node: usize) -> Option<(TextPosition, TextSpan)> {
        let mut total_span: Option<TextSpan> = None;
        let mut position = TextPosition {
            line: usize::MAX,
            column: usize::MAX,
        };
        self.traverse(node, |current| {
            if let Some(p) = self.get_position_at(current) {
                if p < position {
                    position = p;
                }
            }
            if let Some(total_span) = total_span.as_mut() {
                if let Some(span) = self.get_span_at(current) {
                    if span.index + span.length > total_span.index + total_span.length {
                        let margin =
                            (span.index + span.length) - (total_span.index + total_span.length);
                        total_span.length += margin;
                    }
                    if span.index < total_span.index {
                        let margin = total_span.index - span.index;
                        total_span.length += margin;
                        total_span.index -= margin;
                    }
                }
            } else {
                total_span = self.get_span_at(current);
            }
        });
        total_span.map(|span| (position, span))
    }

    /// Gets the total span of sub-tree given its root
    #[must_use]
    pub fn get_total_span(&self, node: usize) -> Option<TextSpan> {
        self.get_total_position_and_span(node).map(|(_, span)| span)
    }

    /// Traverses the AST from the specified node
    fn traverse<F: FnMut(usize)>(&self, from: usize, mut action: F) {
        let mut stack = alloc::vec![from];
        while let Some(current) = stack.pop() {
            let cell = self.data.nodes[current];
            for i in (0..cell.count).rev() {
                stack.push((cell.first + i) as usize);
            }
            action(current);
        }
    }

    /// Get the span of the symbol on a node
    #[must_use]
    fn get_span_at(&self, node: usize) -> Option<TextSpan> {
        let cell = self.data.nodes[node];
        match cell.label.table_type() {
            TableType::Token => {
                let token = self.get_token(cell.label.index());
                token.get_span()
            }
            _ => None,
        }
    }

    /// Get the position of the symbol on a node
    #[must_use]
    fn get_position_at(&self, node: usize) -> Option<TextPosition> {
        let cell = self.data.nodes[node];
        match cell.label.table_type() {
            TableType::Token => {
                let token = self.get_token(cell.label.index());
                token.get_position()
            }
            _ => None,
        }
    }
}

/// Represents a node in an Abstract Syntax Tree
#[derive(Copy, Clone)]
pub struct AstNode<'s, 't, 'a> {
    /// The original parse tree
    tree: &'a Ast<'s, 't, 'a>,
    /// The index of this node in the parse tree
    index: usize,
}

impl<'s, 't, 'a> AstNode<'s, 't, 'a> {
    /// Gets the identifier of this node
    #[must_use]
    pub fn id(&self) -> usize {
        self.index
    }

    /// Gets the index of the token born by this node, if any
    #[must_use]
    pub fn get_token_index(&self) -> Option<usize> {
        let cell = self.tree.data.nodes[self.index];
        match cell.label.table_type() {
            TableType::Token => Some(cell.label.index()),
            _ => None,
        }
    }

    /// Gets the parent of this node, if any
    #[must_use]
    pub fn parent(&self) -> Option<AstNode<'s, 't, 'a>> {
        self.tree.find_parent_of(self.index)
    }

    /// Gets the children of this node
    #[must_use]
    pub fn children(&self) -> AstFamily<'s, 't, 'a> {
        AstFamily {
            tree: self.tree,
            parent: self.index,
        }
    }

    /// Gets the i-th child
    #[must_use]
    pub fn child(&self, index: usize) -> AstNode<'s, 't, 'a> {
        let cell = self.tree.data.nodes[self.index];
        AstNode {
            tree: self.tree,
            index: cell.first as usize + index,
        }
    }

    /// Gets the number of children
    #[must_use]
    pub fn children_count(&self) -> usize {
        self.tree.data.nodes[self.index].count as usize
    }

    /// Gets the total span for the sub-tree at this node
    #[must_use]
    pub fn get_total_span(&self) -> Option<TextSpan> {
        self.tree.get_total_span(self.index)
    }

    /// Gets the total position and span for the sub-tree at this node
    #[must_use]
    pub fn get_total_position_and_span(&self) -> Option<(TextPosition, TextSpan)> {
        self.tree.get_total_position_and_span(self.index)
    }
}

impl<'s, 't, 'a> SemanticElementTrait<'s, 'a> for AstNode<'s, 't, 'a> {
    /// Gets the position in the input text of this element
    fn get_position(&self) -> Option<TextPosition> {
        self.tree.get_position_at(self.index)
    }

    /// Gets the span in the input text of this element
    fn get_span(&self) -> Option<TextSpan> {
        self.tree.get_span_at(self.index)
    }

    /// Gets the context of this element in the input
    fn get_context(&self) -> Option<TextContext<'a>> {
        let cell = self.tree.data.nodes[self.index];
        match cell.label.table_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.index());
                token.get_context()
            }
            _ => None,
        }
    }

    /// Gets the grammar symbol associated to this element
    fn get_symbol(&self) -> Symbol<'s> {
        let cell = self.tree.data.nodes[self.index];
        match cell.label.table_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.index());
                token.get_symbol()
            }
            TableType::Variable => self.tree.variables[cell.label.index()],
            TableType::Virtual => self.tree.virtuals[cell.label.index()],
            TableType::None => {
                // terminal epsilon
                self.tree.tokens.terminals[0]
            }
        }
    }

    /// Gets the value of this element, if any
    fn get_value(&self) -> Option<&'a str> {
        let cell = self.tree.data.nodes[self.index];
        match cell.label.table_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.index());
                token.get_value()
            }
            _ => None,
        }
    }
}

impl<'s, 't, 'a> PartialEq<AstNode<'s, 't, 'a>> for AstNode<'s, 't, 'a> {
    fn eq(&self, other: &AstNode<'s, 't, 'a>) -> bool {
        self.index == other.index
    }
}

impl<'s, 't, 'a> Eq for AstNode<'s, 't, 'a> {}

impl<'s, 't, 'a> IntoIterator for AstNode<'s, 't, 'a> {
    type Item = AstNode<'s, 't, 'a>;
    type IntoIter = AstFamilyIterator<'s, 't, 'a>;

    fn into_iter(self) -> Self::IntoIter {
        let cell = self.tree.data.nodes[self.index];
        AstFamilyIterator {
            tree: self.tree,
            current: cell.first as usize,
            end: (cell.first + cell.count) as usize,
        }
    }
}

impl<'s, 't, 'a> Display for AstNode<'s, 't, 'a> {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        let cell = self.tree.data.nodes[self.index];
        match cell.label.table_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.index());
                let symbol = token.get_symbol();
                let value = token.get_value();
                write!(f, "{} = {}", symbol.name, value.unwrap())
            }
            TableType::Variable => {
                let symbol = self.tree.variables[cell.label.index()];
                write!(f, "{}", symbol.name)
            }
            TableType::Virtual => {
                let symbol = self.tree.virtuals[cell.label.index()];
                write!(f, "{}", symbol.name)
            }
            TableType::None => {
                let symbol = self.tree.tokens.terminals[0];
                write!(f, "{}", symbol.name)
            }
        }
    }
}

impl<'s, 't, 'a> Serialize for AstNode<'s, 't, 'a> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        let mut state = serializer.serialize_struct("AstNode", 5)?;
        state.serialize_field("symbol", &self.get_symbol())?;
        state.serialize_field("position", &self.get_position())?;
        state.serialize_field("span", &self.get_span())?;
        state.serialize_field("value", &self.get_value())?;
        state.serialize_field("children", &self.children())?;
        state.end()
    }
}

/// Represents a family of children for an `ASTNode`
#[derive(Clone)]
pub struct AstFamily<'s, 't, 'a> {
    /// The original parse tree
    tree: &'a Ast<'s, 't, 'a>,
    /// The index of the parent node in the parse tree
    parent: usize,
}

/// Represents and iterator for adjacents in this graph
pub struct AstFamilyIterator<'s, 't, 'a> {
    /// The original parse tree
    tree: &'a Ast<'s, 't, 'a>,
    /// The index of the current child in the parse tree
    current: usize,
    /// The index of the last child (excluded) in the parse tree
    end: usize,
}

/// Implementation of the `Iterator` trait for `AstFamilyIterator`
impl<'s, 't, 'a> Iterator for AstFamilyIterator<'s, 't, 'a> {
    type Item = AstNode<'s, 't, 'a>;
    fn next(&mut self) -> Option<Self::Item> {
        if self.current >= self.end {
            None
        } else {
            let result = AstNode {
                tree: self.tree,
                index: self.current,
            };
            self.current += 1;
            Some(result)
        }
    }

    fn size_hint(&self) -> (usize, Option<usize>) {
        let c = self.end - self.current;
        (c, Some(c))
    }
}

impl<'s, 't, 'a> DoubleEndedIterator for AstFamilyIterator<'s, 't, 'a> {
    fn next_back(&mut self) -> Option<Self::Item> {
        if self.current >= self.end {
            None
        } else {
            let result = AstNode {
                tree: self.tree,
                index: self.end - 1,
            };
            self.end -= 1;
            Some(result)
        }
    }
}

impl<'s, 't, 'a> ExactSizeIterator for AstFamilyIterator<'s, 't, 'a> {}
impl<'s, 't, 'a> FusedIterator for AstFamilyIterator<'s, 't, 'a> {}

impl<'s, 't, 'a> IntoIterator for AstFamily<'s, 't, 'a> {
    type Item = AstNode<'s, 't, 'a>;
    type IntoIter = AstFamilyIterator<'s, 't, 'a>;

    fn into_iter(self) -> Self::IntoIter {
        let cell = self.tree.data.nodes[self.parent];
        AstFamilyIterator {
            tree: self.tree,
            current: cell.first as usize,
            end: (cell.first + cell.count) as usize,
        }
    }
}

impl<'s, 't, 'a> AstFamily<'s, 't, 'a> {
    /// Gets whether the family is empty
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.tree.data.nodes[self.parent].count == 0
    }

    /// Gets the number of children in this family
    #[must_use]
    pub fn len(&self) -> usize {
        self.tree.data.nodes[self.parent].count as usize
    }

    /// Gets the i-th child
    #[must_use]
    pub fn at(&self, index: usize) -> AstNode<'s, 't, 'a> {
        let cell = self.tree.data.nodes[self.parent];
        AstNode {
            tree: self.tree,
            index: cell.first as usize + index,
        }
    }

    /// Gets an iterator over this family
    #[must_use]
    pub fn iter(&self) -> AstFamilyIterator<'s, 't, 'a> {
        let cell = self.tree.data.nodes[self.parent];
        AstFamilyIterator {
            tree: self.tree,
            current: cell.first as usize,
            end: (cell.first + cell.count) as usize,
        }
    }
}

impl<'s, 't, 'a> Serialize for AstFamily<'s, 't, 'a> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        let mut seq = serializer.serialize_seq(Some(self.len()))?;
        for node in self.iter() {
            seq.serialize_element(&node)?;
        }
        seq.end()
    }
}
