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

//! Module for AST subtree in parsers

use super::TREE_ACTION_REPLACE;
use super::TreeAction;
use super::super::ast::Ast;
use super::super::ast::AstCell;
use super::super::ast::TableElemRef;

/// Represents a sub-tree in an AST
/// A sub-tree is composed of a root with its children.
/// The children may also have children.
/// The maximum depth of a sub-tree is 2 (root, children and children's children), in which case the root is always a replaceable node.
/// The internal representation of a sub-tree is based on arrays.
/// The organization is that a node's children are immediately following it in the array.
/// For example, the tree `A(B(CD)E(FG))` is represented as `[ABCDEFG]`.
#[derive(Clone)]
pub struct SubTree {
    /// The nodes in this buffer
    nodes: Vec<AstCell>,
    /// The tree actions for the nodes
    actions: Vec<TreeAction>
}

impl SubTree {
    /// Creates a new sub-tree with the expected size
    pub fn new(size: usize) -> SubTree {
        SubTree {
            nodes: Vec::<AstCell>::with_capacity(size),
            actions: Vec::<TreeAction>::with_capacity(size)
        }
    }

    /// Gets the label of the node at the given index
    pub fn get_label_at(&self, index: usize) -> TableElemRef {
        self.nodes[index].label
    }

    /// Gets the tree action applied onto the node at the given index
    pub fn get_action_at(&self, index: usize) -> TreeAction {
        self.actions[index]
    }

    /// Sets the tree action applied onto the node at the given index
    pub fn set_action_at(&mut self, index: usize, action: TreeAction) {
        self.actions[index] = action;
    }

    /// Gets the number of children of the node at the given index
    pub fn get_children_count_at(&self, index: usize) -> usize {
        self.nodes[index].count as usize
    }

    /// Sets the number of children of the node at the given index
    pub fn set_children_count_at(&mut self, index: usize, count: usize) {
        self.nodes[index].count = count as u32;
    }

    /// Gets the total number of nodes in this sub-tree
    pub fn get_size(&self) -> usize {
        self.nodes.len()
    }

    /// Initializes the root of this sub-tree
    pub fn setup_root(&mut self, symbol: TableElemRef, action: TreeAction) {
        self.nodes.push(AstCell {
            label: symbol,
            count: 0,
            first: 0
        });
        self.actions.push(action);
    }

    /// Copy the content of this sub-tree to the given sub-tree's buffer beginning at the given index
    /// This methods only applies in the case of a depth 1 sub-tree (only a root and its children).
    /// The results of this method in the case of a depth 2 sub-tree is undetermined.
    pub fn copy_to(&self, destination: &mut SubTree) {
        for i in 0..self.nodes.len() as usize {
            destination.nodes.push(self.nodes[i]);
            destination.actions.push(self.actions[i]);
        }
    }

    /// Copy the root's children of this sub-tree to the given sub-tree's buffer beginning at the given index
    /// This methods only applies in the case of a depth 1 sub-tree (only a root and its children).
    pub fn copy_children_to(&self, destination: &mut SubTree) {
        for i in 1..self.nodes.len() as usize {
            destination.nodes.push(self.nodes[i]);
            destination.actions.push(self.actions[i]);
        }
    }

    /// Commits the children of a sub-tree in this buffer to the final ast
    /// If the index is 0, the root's children are committed, assuming this is a depth-1 sub-tree.
    /// If not, the children of the child at the given index are committed.
    pub fn commit_children_of(&mut self, index: usize, ast: &mut Ast) {
        self.nodes[index].first =
            ast.store(&self.nodes, index + 1, self.nodes[index].count as usize) as u32;
    }

    /// Commits this buffer to the final ast
    pub fn commit(&mut self, ast: &mut Ast) {
        self.commit_children_of(0, ast);
        ast.store_root(self.nodes[0]);
    }

    /// Pushes a new node into this buffer
    pub fn push(&mut self, symbol: TableElemRef, action: TreeAction) {
        self.nodes.push(AstCell {
            label: symbol,
            count: 0,
            first: 0
        });
        self.actions.push(action);
    }

    /// Moves an item within the buffer
    pub fn move_node(&mut self, from: usize, to: usize) {
        self.nodes[to] = self.nodes[from];
    }

    /// Moves a range of items within the buffer
    pub fn move_range(&mut self, from: usize, to: usize, length: usize) {
        for i in 0..length {
            self.nodes[to + i] = self.nodes[from + i];
            self.actions[to + i] = self.actions[from + i];
        }
    }
}
