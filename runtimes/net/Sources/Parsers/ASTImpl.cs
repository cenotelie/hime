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
	/// Represents a base class for AST implementations
	/// </summary>
	abstract class ASTImpl : AST
	{
		/// <summary>
		/// Represents a node in this AST
		/// </summary>
		public struct Node
		{
			/// <summary>
			/// The node's symbol reference
			/// </summary>
			public TableElemRef symbol;
			/// <summary>
			/// The number of children
			/// </summary>
			public int count;
			/// <summary>
			/// The index of the first child
			/// </summary>
			public int first;

			/// <summary>
			/// Initializes this node
			/// </summary>
			/// <param name="symbol">The node's symbol</param>
			public Node(TableElemRef symbol)
			{
				this.symbol = symbol;
				this.count = 0;
				this.first = -1;
			}
		}

		/// <summary>
		/// The table of tokens
		/// </summary>
		protected TokenDataProvider tableTokens;
		/// <summary>
		/// The table of variables
		/// </summary>
		protected IList<Symbol> tableVariables;
		/// <summary>
		/// The table of virtuals
		/// </summary>
		protected IList<Symbol> tableVirtuals;
		/// <summary>
		/// The nodes' labels
		/// </summary>
		protected Utils.BigList<Node> nodes;
		/// <summary>
		/// The index of the tree's root node
		/// </summary>
		protected int root;


		/// <summary>
		/// Initializes this AST
		/// </summary>
		public ASTImpl(TokenDataProvider text, IList<Symbol> variables, IList<Symbol> virtuals)
		{
			this.tableTokens = text;
			this.tableVariables = variables;
			this.tableVirtuals = virtuals;
			this.nodes = new Utils.BigList<Node>();
			this.root = -1;
		}

		#region Implementation of AST interface
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
		public abstract ASTNode GetChild(int parent, int i);

		/// <summary>
		/// Gets an enumerator for the children of the given node
		/// </summary>
		/// <param name="parent">A node</param>
		/// <returns>An enumerator for the children</returns>
		public abstract IEnumerator<ASTNode> GetChildren(int parent);

		/// <summary>
		/// Gets the position in the input text of the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The position in the text</returns>
		public TextPosition GetPosition(int node)
		{
			TableElemRef sym = nodes[node].symbol;
			if (sym.Type == TableType.Token)
				return tableTokens.GetPositionOf(sym.Index);
			return new TextPosition(0, 0);
		}
		#endregion

		/// <summary>
		/// Gets the symbol corresponding to the given symbol reference
		/// </summary>
		/// <param name="symRef">A symbol reference</param>
		/// <returns>The corresponding symbol</returns>
		public Symbol GetSymbolFor(TableElemRef symRef)
		{
			switch (symRef.Type)
			{
				case TableType.Token:
					return tableTokens[symRef.Index];
				case TableType.Variable:
					return tableVariables[symRef.Index];
				case TableType.Virtual:
					return tableVirtuals[symRef.Index];
			}
			// This cannot happen
			return new Symbol(0, string.Empty);
		}

		/// <summary>
		/// Stores some children nodes in this AST
		/// </summary>
		/// <param name="nodes">The nodes to store</param>
		/// <param name="index">The starting index of the nodes in the data to store</param>
		/// <param name="count">The number of nodes to store</param>
		/// <returns>The index of the first inserted node in this tree</returns>
		public int Store(Node[] nodes, int index, int count)
		{
			return this.nodes.Add(nodes, index, count);
		}
	}
}
