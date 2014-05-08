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

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents an AST using a graph structure
	/// </summary>
	class GraphAST : AST
	{
		/// <summary>
		/// The table of tokens
		/// </summary>
		private TokenizedText tableTokens;
		/// <summary>
		/// The table of variables
		/// </summary>
		private IList<Symbol> tableVariables;
		/// <summary>
		/// The table of virtuals
		/// </summary>
		private IList<Symbol> tableVirtuals;
		/// <summary>
		/// The nodes' labels
		/// </summary>
		private Utils.BigList<SimpleAST.Node> nodes;
		/// <summary>
		/// The adjacency table
		/// </summary>
		private Utils.BigList<int> adjacency;
		/// <summary>
		/// The index of the tree's root node
		/// </summary>
		private int root;

		/// <summary>
		/// Initializes this SPPF
		/// </summary>
		public GraphAST(TokenizedText text, IList<Symbol> variables, IList<Symbol> virtuals)
		{
			this.tableTokens = text;
			this.tableVariables = variables;
			this.tableVirtuals = virtuals;
			this.nodes = new Utils.BigList<SimpleAST.Node>();
			this.adjacency = new Utils.BigList<int>();
			this.root = -1;
		}

		/// <summary>
		/// Gets the root node of this tree
		/// </summary>
		public ASTNode Root { get { return new ASTNode(this, this.root); } }

		/// <summary>
		/// Gets the symbol of the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The node's symbol</returns>
		public Symbol GetSymbol(int node)
		{
			return GetSymbolFor(nodes[node].symbol);
		}

		/// <summary>
		/// Gets the number of children of the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The node's numer of children</returns>
		public int GetChildrenCount(int node)
		{
			return nodes[node].count;
		}

		/// <summary>
		/// Gets the i-th child of the given node
		/// </summary>
		/// <param name="parent">A node</param>
		/// <param name="i">The child's number</param>
		/// <returns>The i-th child</returns>
		public ASTNode GetChild(int parent, int i)
		{
			return new ASTNode(this, adjacency[nodes[parent].first + i]);
		}

		/// <summary>
		/// Gets an enumerator for the children of the given node
		/// </summary>
		/// <param name="parent">A node</param>
		/// <returns>An enumerator for the children</returns>
		public IEnumerator<ASTNode> GetChildren(int parent)
		{
			return new ChildEnumerator(this, parent);
		}

		/// <summary>
		/// Gets the position in the input text of the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The position in the text</returns>
		public TextPosition GetPosition(int node)
		{
			SymbolRef sym = nodes[node].symbol;
			if (sym.Type == SymbolType.Token)
				return tableTokens.GetPositionOf(sym.Index);
			return new TextPosition(0, 0);
		}

		#region Internal API
		public int Store(SymbolRef symbol)
		{
			return nodes.Add(new SimpleAST.Node(symbol));
		}

		public int Store(SymbolRef symbol, int first, int count)
		{
			return nodes.Add(new SimpleAST.Node(symbol, first, count));
		}

		public int Store(int[] adjacents, int count)
		{
			return adjacency.Add(adjacents, 0, count);
		}

		public int CopyNode(int node)
		{
			int result = nodes.Add(nodes[node]);
			SimpleAST.Node copy = nodes[result];
			if (copy.count != 0)
			{
				copy.first = adjacency.Duplicate(copy.first, copy.count);
				nodes[result] = copy;
			}
			return result;
		}

		public SymbolRef GetLabel(int index)
		{
			return nodes[index].symbol;
		}

		public int GetAdjacency(int node, int[] buffer, int index)
		{
			SimpleAST.Node temp = nodes[node];
			for (int i=0; i!=temp.count; i++)
				buffer[index + i] = adjacency[temp.first + i];
			return temp.count;
		}

		public void SetAdjacency(int node, int first, int count)
		{
			SimpleAST.Node temp = nodes[node];
			temp.first = first;
			temp.count = count;
			nodes[node] = temp;
		}

		public void SetRoot(int node)
		{
			this.root = node;
		}

		/// <summary>
		/// Gets the symbol corresponding to the given symbol reference
		/// </summary>
		/// <param name="symRef">A symbol reference</param>
		/// <returns>The corresponding symbol</returns>
		public Symbol GetSymbolFor(SymbolRef symRef)
		{
			switch (symRef.Type)
			{
				case SymbolType.Token:
					return tableTokens[symRef.Index];
				case SymbolType.Variable:
					return tableVariables[symRef.Index];
				case SymbolType.Virtual:
					return tableVirtuals[symRef.Index];
			}
			// This cannot happen
			return new Symbol(0, string.Empty);
		}
		#endregion

		/// <summary>
		/// Represents and iterator for adjacents in this graph
		/// </summary>
		private class ChildEnumerator : IEnumerator<ASTNode>
		{
			private GraphAST ast;
			private int first;
			private int current;
			private int end;

			public ChildEnumerator(GraphAST ast, int node)
			{
				this.ast = ast;
				SimpleAST.Node n = ast.nodes[node];
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
	}
}
