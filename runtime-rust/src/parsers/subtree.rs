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

use super::TreeAction;
use super::super::ast::AstCell;

/// Represents a sub-tree in an AST
/// A sub-tree is composed of a root with its children.
/// The children may also have children.
/// The maximum depth of a sub-tree is 2 (root, children and children's children), in which case the root is always a replaceable node.
/// The internal representation of a sub-tree is based on arrays.
/// The organization is that a node's children are immediately following it in the array.
/// For example, the tree `A(B(CD)E(FG))` is represented as `[ABCDEFG]`.
pub struct SubTree {
    nodes: Vec<AstCell>,
    actions: Vec<TreeAction>
}
