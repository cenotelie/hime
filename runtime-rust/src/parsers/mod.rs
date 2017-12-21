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

//! Module for parsers API

pub mod automaton;
pub mod subtree;

/// Represents a tree action
pub type TreeAction = u16;

/// Keep the node as is
pub const TREE_ACTION_NONE: TreeAction = 0;
/// Replace the node with its children
pub const TREE_ACTION_REPLACE: TreeAction = 1;
/// Drop the node and all its descendants
pub const TREE_ACTION_DROP: TreeAction = 2;
/// Promote the node, i.e. replace its parent with it and insert its children where it was
pub const TREE_ACTION_PROMOTE: TreeAction = 3;

/// Represent an op-code for a LR production
/// An op-code can be either an instruction or raw data
pub type LROpCode = u16;

/// Pop an AST from the stack
pub const LR_OP_CODE_BASE_POP_STACK: LROpCode = 0;
/// Add a virtual symbol
pub const LR_OP_CODE_BASE_ADD_VIRTUAL: LROpCode = 1 << 2;
/// Execute a semantic action
pub const LR_OP_CODE_BASE_SEMANTIC_ACTION: LROpCode = 1 << 3;
/// Add a null variable
/// This can be found only in RNGLR productions
pub const LR_OP_CODE_BASE_ADD_NULLABLE_VARIABLE: LROpCode = 1 << 4;

/// Gets the base LR op-code
pub fn get_op_code_base(op_code: LROpCode) -> LROpCode {
    op_code & 0xFFFC
}

/// Gets the tree action encoded in the specified LR op-code
pub fn get_op_code_tree_action(op_code: LROpCode) -> TreeAction {
    op_code & 0x0003
}

/// Represents an action in a LR parser
pub type LRActionCode = u16;

/// No possible action => Error
pub const LR_ACTION_CODE_NONE: LRActionCode = 0;
/// Apply a reduction
pub const LR_ACTION_CODE_REDUCE: LRActionCode = 1;
/// Shift to another state
pub const LR_ACTION_CODE_SHIFT: LRActionCode = 2;
/// Accept the input
pub const LR_ACTION_CODE_ACCEPT: LRActionCode = 3;