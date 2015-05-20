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
using System.Collections.Generic;
using Hime.Redist.Utils;

namespace Hime.Redist
{
	/// <summary>
	/// Represents a simple AST with a tree structure
	/// </summary>
	/// <remarks>
	/// The nodes are stored in sequentials arrays where the children of a node are an inner sequence.
	/// The linkage is represented by each node storing its number of children and the index of its first child.
	/// </remarks>
	class ASTSimpleTree : AST
	{
		/// <summary>
		/// Initializes this AST
		/// </summary>
		/// <param name="tokens">The table of tokens</param>
		/// <param name="variables">The table of variables</param>
		/// <param name="virtuals">The table of virtuals</param>
		public ASTSimpleTree(TokenRepository tokens, ROList<Symbol> variables, ROList<Symbol> virtuals)
			: base(tokens, variables, virtuals)
		{
		}

		/// <summary>
		/// Gets the i-th child of the given node
		/// </summary>
		/// <param name="parent">A node</param>
		/// <param name="i">The child's number</param>
		/// <returns>The i-th child</returns>
		public override ASTNode GetChild(int parent, int i)
		{
			return new ASTNode(this, nodes[parent].first + i);
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

		/// <summary>
		/// Stores the root of this tree
		/// </summary>
		/// <param name="node">The root</param>
		public void StoreRoot(Node node)
		{
			root = nodes.Add(node);
		}

		/// <summary>
		/// Represents and iterator for adjacents in this graph
		/// </summary>
		private class ChildEnumerator : IEnumerator<ASTNode>
		{
			private ASTSimpleTree ast;
			private int first;
			private int current;
			private int end;

			public ChildEnumerator(ASTSimpleTree ast, int node)
			{
				this.ast = ast;
				Node n = ast.nodes[node];
				this.first = n.first;
				this.current = this.first - 1;
				this.end = this.first + n.count;
			}

			/// <summary>
			/// Gets the current node
			/// </summary>
			public ASTNode Current { get { return new ASTNode(ast, current); } }

			/// <summary>
			/// Gets the current node
			/// </summary>
			object System.Collections.IEnumerator.Current { get { return new ASTNode(ast, current); } }

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
	}
}
