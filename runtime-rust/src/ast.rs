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

use serde::ser::{Serialize, SerializeSeq, SerializeStruct, Serializer};
use std::fmt::Display;
use std::fmt::Error;
use std::fmt::Formatter;

use crate::symbols::SemanticElementTrait;
use crate::symbols::Symbol;
use crate::text::TextContext;
use crate::text::TextPosition;
use crate::text::TextSpan;
use crate::tokens::Token;
use crate::tokens::TokenRepository;
use crate::utils::biglist::BigList;
use crate::utils::EitherMut;

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
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
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
    pub fn get_type(self) -> TableType {
        TableType::from(self.data >> 30)
    }

    /// Gets the element's index in its respective table
    pub fn get_index(self) -> usize {
        self.data & 0x3FFF_FFFF
    }
}

/// Represents a cell in an AST inner structure
#[derive(Debug, Copy, Clone)]
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
            nodes: BigList::new(AstCell::new(TableElemRef::new(TableType::None, 0), 0, 0)),
            root: None
        }
    }

    /// Gets whether a root has been defined for this AST
    pub fn has_root(&self) -> bool {
        self.root.is_some()
    }
}

impl Default for AstImpl {
    fn default() -> Self {
        Self::new()
    }
}

/// Represents a simple AST with a tree structure
/// The nodes are stored in sequential arrays where the children of a node are an inner sequence.
/// The linkage is represented by each node storing its number of children and the index of its first child.
pub struct Ast<'a: 'b, 'b, 'c> {
    /// The table of tokens
    tokens: Option<TokenRepository<'a, 'b, 'c>>,
    /// The table of variables
    variables: &'b [Symbol<'a>],
    /// The table of virtuals
    virtuals: &'b [Symbol<'a>],
    /// The data of the implementation
    data: EitherMut<'c, AstImpl>
}

impl<'a: 'b, 'b, 'c> Ast<'a, 'b, 'c> {
    /// Creates a new AST proxy structure
    pub fn new(
        tokens: TokenRepository<'a, 'b, 'c>,
        variables: &'b [Symbol<'a>],
        virtuals: &'b [Symbol<'a>],
        data: &'c AstImpl
    ) -> Ast<'a, 'b, 'c> {
        Ast {
            tokens: Some(tokens),
            variables,
            virtuals,
            data: EitherMut::Immutable(data)
        }
    }

    /// Creates a new AST proxy structure
    pub fn new_mut(
        variables: &'b [Symbol<'a>],
        virtuals: &'b [Symbol<'a>],
        data: &'c mut AstImpl
    ) -> Ast<'a, 'b, 'c> {
        Ast {
            tokens: None,
            variables,
            virtuals,
            data: EitherMut::Mutable(data)
        }
    }

    /// Gets the i-th token in the associated repository
    fn get_token<'x>(&'x self, index: usize) -> Token<'a, 'b, 'c, 'x> {
        match self.tokens {
            None => panic!("Missing token repository"),
            Some(ref x) => x.get_token(index)
        }
    }

    /// Gets the grammar variables for this AST
    pub fn get_variables(&self) -> &'b [Symbol<'a>] {
        &self.variables
    }

    /// Gets the grammar virtuals for this AST
    pub fn get_virtuals(&self) -> &'b [Symbol<'a>] {
        &self.virtuals
    }

    /// Gets whether a root has been defined for this AST
    pub fn has_root(&self) -> bool {
        self.data.get().has_root()
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

    /// Gets a specific node in this tree
    pub fn get_node(&self, id: usize) -> AstNode {
        AstNode {
            tree: self,
            index: id
        }
    }

    /// Gets the AST node (if any) that has the specified token as label
    pub fn find_node_for(&self, token: &Token) -> Option<AstNode> {
        let data = self.data.get();
        for i in 0..data.nodes.len() {
            let node = data.nodes[i];
            if node.label.get_type() == TableType::Token && node.label.get_index() == token.index {
                return Some(AstNode {
                    tree: self,
                    index: i
                });
            }
        }
        None
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    pub fn find_node_at_index(&self, index: usize) -> Option<AstNode> {
        let token = self.tokens.as_ref().unwrap().find_token_at(index);
        match token {
            None => None,
            Some(token) => self.find_node_for(&token)
        }
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    pub fn find_node_at_position(&self, position: TextPosition) -> Option<AstNode> {
        let tokens = self.tokens.as_ref().unwrap();
        let index = tokens.text.get_line_index(position.line) + position.column - 1;
        let token = tokens.find_token_at(index);
        match token {
            None => None,
            Some(token) => self.find_node_for(&token)
        }
    }

    /// Gets the parent of the specified node, if any
    pub fn find_parent_of(&self, node: usize) -> Option<AstNode> {
        let data = self.data.get();
        data.root?;
        for i in 0..data.nodes.len() {
            let candidate = data.nodes[i];
            if candidate.count > 0
                && node >= candidate.first as usize
                && node < (candidate.first + candidate.count) as usize
            {
                return Some(AstNode {
                    tree: self,
                    index: i
                });
            }
        }
        None
    }

    /// Gets the total span of sub-tree given its root and its position
    pub fn get_total_position_and_span(&self, node: usize) -> Option<(TextPosition, TextSpan)> {
        let mut total_span: Option<TextSpan> = None;
        let mut position = TextPosition {
            line: std::usize::MAX,
            column: std::usize::MAX
        };
        self.traverse(node, |data, current| {
            if let Some(p) = self.get_position_at(data, current) {
                if p < position {
                    position = p;
                }
            }
            if total_span.is_none() {
                total_span = self.get_span_at(data, current);
            } else if let Some(span) = self.get_span_at(data, current) {
                let total_span = total_span.as_mut().unwrap();
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
        });
        total_span.map(|span| (position, span))
    }

    /// Gets the total span of sub-tree given its root
    pub fn get_total_span(&self, node: usize) -> Option<TextSpan> {
        self.get_total_position_and_span(node).map(|(_, span)| span)
    }

    /// Traverses the AST from the specified node
    fn traverse<F: FnMut(&AstImpl, usize)>(&self, from: usize, mut action: F) {
        let data = self.data.get();
        let mut stack = vec![from];
        while !stack.is_empty() {
            let current = stack.pop().unwrap();
            let cell = data.nodes[current];
            for i in (0..cell.count).rev() {
                stack.push((cell.first + i) as usize);
            }
            action(data, current);
        }
    }

    /// Get the span of the symbol on a node
    fn get_span_at(&self, data: &AstImpl, node: usize) -> Option<TextSpan> {
        let cell = data.nodes[node];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.get_token(cell.label.get_index());
                token.get_span()
            }
            _ => None
        }
    }

    /// Get the position of the symbol on a node
    fn get_position_at(&self, data: &AstImpl, node: usize) -> Option<TextPosition> {
        let cell = data.nodes[node];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.get_token(cell.label.get_index());
                token.get_position()
            }
            _ => None
        }
    }

    /// Stores some children nodes in this AST
    pub fn store(&mut self, nodes: &[AstCell], index: usize, count: usize) -> usize {
        if count == 0 {
            0
        } else {
            match self.data.get_mut() {
                None => panic!("Got a mutable AST with an immutable implementation"),
                Some(data) => {
                    let result = data.nodes.push(nodes[index]);
                    for i in 1..count {
                        data.nodes.push(nodes[index + i]);
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
            Some(data) => data.root = Some(data.nodes.push(node))
        }
    }
}

/// Represents a node in an Abstract Syntax Tree
#[derive(Clone)]
pub struct AstNode<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> {
    /// The original parse tree
    tree: &'d Ast<'a, 'b, 'c>,
    /// The index of this node in the parse tree
    index: usize
}

impl<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> AstNode<'a, 'b, 'c, 'd> {
    /// Gets the identifier of this node
    pub fn id(&self) -> usize {
        self.index
    }

    /// Gets the index of the token born by this node, if any
    pub fn get_token_index(&self) -> Option<usize> {
        let cell = self.tree.data.get().nodes[self.index];
        match cell.label.get_type() {
            TableType::Token => Some(cell.label.get_index()),
            _ => None
        }
    }

    /// Gets the parent of this node, if any
    pub fn parent(&self) -> Option<AstNode> {
        self.tree.find_parent_of(self.index)
    }

    /// Gets the children of this node
    pub fn children(&self) -> AstFamily {
        AstFamily {
            tree: self.tree,
            parent: self.index
        }
    }

    /// Gets the total span for the sub-tree at this node
    pub fn get_total_span(&self) -> Option<TextSpan> {
        self.tree.get_total_span(self.index)
    }

    /// Gets the total position and span for the sub-tree at this node
    pub fn get_total_position_and_span(&self) -> Option<(TextPosition, TextSpan)> {
        self.tree.get_total_position_and_span(self.index)
    }
}

impl<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> SemanticElementTrait<'a> for AstNode<'a, 'b, 'c, 'd> {
    /// Gets the position in the input text of this element
    fn get_position(&self) -> Option<TextPosition> {
        self.tree.get_position_at(self.tree.data.get(), self.index)
    }

    /// Gets the span in the input text of this element
    fn get_span(&self) -> Option<TextSpan> {
        self.tree.get_span_at(self.tree.data.get(), self.index)
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
    fn get_symbol(&self) -> Symbol<'a> {
        let cell = self.tree.data.get().nodes[self.index];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.get_index());
                token.get_symbol()
            }
            TableType::Variable => self.tree.variables[cell.label.get_index()],
            TableType::Virtual => self.tree.virtuals[cell.label.get_index()],
            TableType::None => {
                match self.tree.tokens {
                    None => panic!("Missing token repository"),
                    Some(ref repository) => repository.terminals[0] // terminal epsilon
                }
            }
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

impl<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> Display for AstNode<'a, 'b, 'c, 'd> {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        let cell = self.tree.data.get().nodes[self.index];
        match cell.label.get_type() {
            TableType::Token => {
                let token = self.tree.get_token(cell.label.get_index());
                let symbol = token.get_symbol();
                let value = token.get_value();
                write!(f, "{} = {}", symbol.name, value.unwrap())
            }
            TableType::Variable => {
                let symbol = self.tree.variables[cell.label.get_index()];
                write!(f, "{}", symbol.name)
            }
            TableType::Virtual => {
                let symbol = self.tree.virtuals[cell.label.get_index()];
                write!(f, "{}", symbol.name)
            }
            TableType::None => match self.tree.tokens {
                None => panic!("Missing token repository"),
                Some(ref repository) => {
                    let symbol = repository.terminals[0];
                    write!(f, "{}", symbol.name)
                }
            }
        }
    }
}

impl<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> Serialize for AstNode<'a, 'b, 'c, 'd> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer
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

/// Represents a family of children for an ASTNode
#[derive(Clone)]
pub struct AstFamily<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> {
    /// The original parse tree
    tree: &'d Ast<'a, 'b, 'c>,
    /// The index of the parent node in the parse tree
    parent: usize
}

/// Represents and iterator for adjacents in this graph
pub struct AstFamilyIterator<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> {
    /// The original parse tree
    tree: &'d Ast<'a, 'b, 'c>,
    /// The index of the current child in the parse tree
    current: usize,
    /// the index of the last child (excluded) in the parse tree
    end: usize
}

/// Implementation of the `Iterator` trait for `AstFamilyIterator`
impl<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> Iterator for AstFamilyIterator<'a, 'b, 'c, 'd> {
    type Item = AstNode<'a, 'b, 'c, 'd>;
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

impl<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> AstFamily<'a, 'b, 'c, 'd> {
    /// Gets whether the family is empty
    pub fn is_empty(&self) -> bool {
        self.tree.data.get().nodes[self.parent].count == 0
    }

    /// Gets the number of children in this family
    pub fn len(&self) -> usize {
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

    /// Gets an iterator over this family
    pub fn iter(&self) -> AstFamilyIterator {
        let cell = self.tree.data.get().nodes[self.parent];
        AstFamilyIterator {
            tree: self.tree,
            current: cell.first as usize,
            end: (cell.first + cell.count) as usize
        }
    }
}

impl<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd> Serialize for AstFamily<'a, 'b, 'c, 'd> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer
    {
        let mut seq = serializer.serialize_seq(Some(self.len()))?;
        for node in self.iter() {
            seq.serialize_element(&node)?;
        }
        seq.end()
    }
}
