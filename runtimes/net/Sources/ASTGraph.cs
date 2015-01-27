/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
using System.Collections.Generic;
using Hime.Redist.Utils;

namespace Hime.Redist
{
	/// <summary>
	/// Represents an AST using a graph structure
	/// </summary>
	class ASTGraph : ASTBaseImpl
	{
		/// <summary>
		/// The adjacency table
		/// </summary>
		private Utils.BigList<int> adjacency;

		/// <summary>
		/// Initializes this AST
		/// </summary>
		/// <param name="tokens">The table of tokens</param>
		/// <param name="variables">The table of variables</param>
		/// <param name="virtuals">The table of virtuals</param>
		public ASTGraph(TokenRepository tokens, ROList<Symbol> variables, ROList<Symbol> virtuals)
			: base(tokens, variables, virtuals)
		{
			this.adjacency = new Utils.BigList<int>();
		}

		#region Implementation of AST interface
		/// <summary>
		/// Gets the i-th child of the given node
		/// </summary>
		/// <param name="parent">A node</param>
		/// <param name="i">The child's number</param>
		/// <returns>The i-th child</returns>
		public override ASTNode GetChild(int parent, int i)
		{
			return new ASTNode(this, adjacency[nodes[parent].first + i]);
		}

		/// <summary>
		/// Gets an enumerator for the children of the given node
		/// </summary>
		/// <param name="parent">A node</param>
		/// <returns>An enumerator for the children</returns>
		public override IEnumerator<ASTNode> GetChildren(int parent)
		{
			return new ChildEnumerator(this, parent);
		}
		#endregion

		/// <summary>
		/// Represents and iterator for adjacents in this graph
		/// </summary>
		private class ChildEnumerator : IEnumerator<ASTNode>
		{
			private ASTGraph ast;
			private int first;
			private int current;
			private int end;

			public ChildEnumerator(ASTGraph ast, int node)
			{
				this.ast = ast;
				ASTSimpleTree.Node n = ast.nodes[node];
				this.first = n.first;
				this.current = this.first - 1;
				this.end = this.first + n.count;
			}

			/// <summary>
			/// Gets the current node
			/// </summary>
			public ASTNode Current { get { return new ASTNode(ast, ast.adjacency[current]); } }

			/// <summary>
			/// Gets the current node
			/// </summary>
			object System.Collections.IEnumerator.Current { get { return new ASTNode(ast, ast.adjacency[current]); } }

			/// <summary>
			/// Disposes this enumerator
			/// </summary>
			public void Dispose()
			{
				ast = null;
			}

			/// <summary>
			/// Moves to the next node
			/// </summary>
			/// <returns>true if there are more nodes</returns>
			public bool MoveNext()
			{
				current++;
				return (current != end);
			}

			/// <summary>
			/// Resets this enumerator to the beginning
			/// </summary>
			public void Reset()
			{
				current = first - 1;
			}
		}

		/// <summary>
		/// Stores the specified symbol in this AST as a new node
		/// </summary>
		/// <param name="symbol">The symbol to store</param>
		/// <returns>The index of the new node</returns>
		public int Store(TableElemRef symbol)
		{
			return nodes.Add(new ASTSimpleTree.Node(symbol));
		}

		/// <summary>
		/// Stores some adjacency data in this graph AST
		/// </summary>
		/// <param name="adjacents">A buffer of adjacency data</param>
		/// <param name="count">The number of adjacents to store</param>
		/// <returns>The index of the data stored in this graph</returns>
		public int Store(int[] adjacents, int count)
		{
			return adjacency.Add(adjacents, 0, count);
		}

		/// <summary>
		/// Copies the provided node (and its adjacency data)
		/// </summary>
		/// <param name="node">The node to copy</param>
		/// <returns>The index of the copy</returns>
		public int CopyNode(int node)
		{
			int result = nodes.Add(nodes[node]);
			ASTSimpleTree.Node copy = nodes[result];
			if (copy.count != 0)
			{
				copy.first = adjacency.Duplicate(copy.first, copy.count);
				nodes[result] = copy;
			}
			return result;
		}

		/// <summary>
		/// Gets the adjacency data for the specified node
		/// </summary>
		/// <param name="node">The node to retrieve the adjacency data of</param>
		/// <param name="buffer">The buffer to store the retrieved data in</param>
		/// <param name="index">The starting index in the provided buffer</param>
		/// <returns>The number of adjacents</returns>
		public int GetAdjacency(int node, int[] buffer, int index)
		{
			ASTSimpleTree.Node temp = nodes[node];
			for (int i = 0; i != temp.count; i++)
				buffer[index + i] = adjacency[temp.first + i];
			return temp.count;
		}

		/// <summary>
		/// Sets the adjacencydata for the specified node
		/// </summary>
		/// <param name="node">The node to set the adjacency data of</param>
		/// <param name="first">The index of the first adjacency item</param>
		/// <param name="count">The number of adjacency items</param>
		public void SetAdjacency(int node, int first, int count)
		{
			ASTSimpleTree.Node temp = nodes[node];
			temp.first = first;
			temp.count = count;
			nodes[node] = temp;
		}

		/// <summary>
		/// Sets the root of this AST
		/// </summary>
		/// <param name="node">Index of the root node</param>
		public void SetRoot(int node)
		{
			this.root = node;
		}
	}
}
