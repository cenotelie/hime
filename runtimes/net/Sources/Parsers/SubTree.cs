/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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
*
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/
using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a sub-tree in an AST
	/// </summary>
	/// <remarks>
	/// A sub-tree is composed of a root with its children.
	/// The children may also have children.
	/// The maximum depth of a sub-tree is 2 (root, children and children's children), in which case the root is always a replaceable node.
	/// The internal representation of a sub-tree is based on arrays.
	/// The organization is that a node's children are immediately following it in the array.
	/// For example, the tree A(B(CD)E(FG)) is represented as [ABCDEFG].
	/// </remarks>
	class SubTree
	{
		/// <summary>
		/// The pool containing this object
		/// </summary>
		private Pool<SubTree> pool;
		/// <summary>
		/// The nodes in this buffer
		/// </summary>
		private ASTSimpleTree.Node[] nodes;
		/// <summary>
		/// The tree actions for the nodes
		/// </summary>
		private TreeAction[] actions;

		/// <summary>
		/// Gets the label of the node at the given index
		/// </summary>
		/// <param name="index">The index within the buffer</param>
		/// <returns>The label in the buffer</returns>
		public TableElemRef GetLabelAt(int index)
		{
			return nodes[index].label;
		}

		/// <summary>
		/// Gets the tree action applied onto the node at the given index
		/// </summary>
		/// <param name="index">The index within the buffer</param>
		/// <returns>The tree action in the buffer</returns>
		public TreeAction GetActionAt(int index)
		{
			return actions[index];
		}

		/// <summary>
		/// Sets the tree action applied onto the node at the given index
		/// </summary>
		/// <param name="index">The index within the buffer</param>
		/// <param name="action">The tree action to apply</param>
		public void SetActionAt(int index, TreeAction action)
		{
			actions[index] = action;
		}

		/// <summary>
		/// Gets the number of children of the node at the given index
		/// </summary>
		/// <param name="index">The index within the buffer</param>
		/// <returns>The number of children</returns>
		public int GetChildrenCountAt(int index)
		{
			return nodes[index].count;
		}

		/// <summary>
		/// Sets the number of children of the node at the given index
		/// </summary>
		/// <param name="index">The index within the buffer</param>
		/// <param name="count">The number of children</param>
		public void SetChildrenCountAt(int index, int count)
		{
			nodes[index].count = count;
		}

		/// <summary>
		/// Gets the total number of nodes in this sub-tree
		/// </summary>
		/// <returns>The total number of nodes in this sub-tree</returns>
		public int GetSize()
		{
			if (actions[0] != TreeAction.Replace)
				return nodes[0].count + 1;
			int size = 1;
			for (int i = 0; i != nodes[0].count; i++)
				size += nodes[size].count + 1;
			return size;
		}

		/// <summary>
		/// Instantiates a new sub-tree attached to the given pool, with the given capacity
		/// </summary>
		/// <param name="pool">The pool to which this sub-tree is attached</param>
		/// <param name="capacity">The capacity of the internal buffer of this sub-tree</param>
		public SubTree(Pool<SubTree> pool, int capacity)
		{
			this.pool = pool;
			this.nodes = new ASTSimpleTree.Node[capacity];
			this.actions = new TreeAction[capacity];
		}

		/// <summary>
		/// Clones this sub-tree
		/// </summary>
		/// <returns>The clone</returns>
		public SubTree Clone()
		{
			SubTree result = null;
			if (this.pool != null)
				result = this.pool.Acquire();
			else
				result = new SubTree(null, this.nodes.Length);
			int size = GetSize();
			Array.Copy(this.nodes, result.nodes, size);
			Array.Copy(this.actions, result.actions, size);
			return result;
		}

		/// <summary>
		/// Initializes the root of this sub-tree
		/// </summary>
		/// <param name="symbol">The root's symbol</param>
		/// <param name="action">The tree action applied on the root</param>
		public void SetupRoot(TableElemRef symbol, TreeAction action)
		{
			nodes[0] = new ASTSimpleTree.Node(symbol);
			actions[0] = action;
		}

		/// <summary>
		/// Copy the content of this sub-tree to the given sub-tree's buffer beginning at the given index
		/// </summary>
		/// <param name="destination">The sub-tree to copy to</param>
		/// <param name="index">The starting index in the destination's buffer</param>
		/// <remarks>
		/// This methods only applies in the case of a depth 1 sub-tree (only a root and its children).
		/// The results of this method in the case of a depth 2 sub-tree is undetermined.
		/// </remarks>
		public void CopyTo(SubTree destination, int index)
		{
			if (this.nodes[0].count == 0)
			{
				destination.nodes[index] = this.nodes[0];
				destination.actions[index] = this.actions[0];
			}
			else
			{
				int size = this.nodes[0].count + 1;
				Array.Copy(this.nodes, 0, destination.nodes, index, size);
				Array.Copy(this.actions, 0, destination.actions, index, size);
			}
		}

		/// <summary>
		/// Copy the root's children of this sub-tree to the given sub-tree's buffer beginning at the given index
		/// </summary>
		/// <param name="destination">The sub-tree to copy to</param>
		/// <param name="index">The starting index in the destination's buffer</param>
		/// <remarks>
		/// This methods only applies in the case of a depth 1 sub-tree (only a root and its children).
		/// The results of this method in the case of a depth 2 sub-tree is undetermined.
		/// </remarks>
		public void CopyChildrenTo(SubTree destination, int index)
		{
			if (this.nodes[0].count == 0)
				return;
			int size = GetSize() - 1;
			Array.Copy(this.nodes, 1, destination.nodes, index, size);
			Array.Copy(this.actions, 1, destination.actions, index, size);
		}

		/// <summary>
		/// Commits the children of a sub-tree in this buffer to the final ast
		/// </summary>
		/// <param name="index">The starting index of the sub-tree</param>
		/// <param name="ast">The ast to commit to</param>
		/// <remarks>
		/// If the index is 0, the root's children are commited, assuming this is a depth-1 sub-tree.
		/// If not, the children of the child at the given index are commited.
		/// </remarks>
		public void CommitChildrenOf(int index, ASTSimpleTree ast)
		{
			if (nodes[index].count != 0)
				nodes[index].first = ast.Store(nodes, index + 1, nodes[index].count);
		}

		/// <summary>
		/// Commits this buffer to the final ast
		/// </summary>
		/// <param name="ast">The ast to commit to</param>
		public void Commit(ASTSimpleTree ast)
		{
			CommitChildrenOf(0, ast);
			ast.StoreRoot(nodes[0]);
		}

		/// <summary>
		/// Sets the content of the i-th item
		/// </summary>
		/// <param name="index">The index of the item to set</param>
		/// <param name="symbol">The symbol</param>
		/// <param name="action">The tree action</param>
		public void SetAt(int index, TableElemRef symbol, TreeAction action)
		{
			nodes[index] = new ASTSimpleTree.Node(symbol);
			actions[index] = action;
		}

		/// <summary>
		/// Moves an item within the buffer
		/// </summary>
		/// <param name="from">The index of the item to move</param>
		/// <param name="to">The destination index for the item</param>
		public void Move(int from, int to)
		{
			this.nodes[to] = this.nodes[from];
		}

		/// <summary>
		/// Moves a range of items within the buffer
		/// </summary>
		/// <param name="from">The starting index of the items to move</param>
		/// <param name="to">The destination index for the items</param>
		/// <param name="length">The number of items to move</param>
		public void MoveRange(int from, int to, int length)
		{
			if (length != 0)
			{
				Array.Copy(nodes, from, nodes, to, length);
				Array.Copy(actions, from, actions, to, length);
			}
		}

		/// <summary>
		/// Releases this sub-tree to the pool
		/// </summary>
		public void Free()
		{
			if (pool != null)
				pool.Return(this);
		}
	}
}