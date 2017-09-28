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

using System.Collections.Generic;
using Hime.Redist.Utils;

namespace Hime.Redist
{
	/// <summary>
	/// Represents a base class for AST implementations
	/// </summary>
	abstract class AST
	{
		/// <summary>
		/// Represents a node in this AST
		/// </summary>
		public struct Node
		{
			/// <summary>
			/// The node's label
			/// </summary>
			public TableElemRef label;
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
			/// <param name="label">The node's label</param>
			public Node(TableElemRef label)
			{
				this.label = label;
				this.count = 0;
				this.first = -1;
			}

			/// <summary>
			/// Initializes this node
			/// </summary>
			/// <param name="label">The node's label</param>
			/// <param name="count">The number of children</param>
			/// <param name="first">The index of the first child</param>
			public Node(TableElemRef label, int count, int first)
			{
				this.label = label;
				this.count = count;
				this.first = first;
			}
		}

		/// <summary>
		/// The table of tokens
		/// </summary>
		protected readonly TokenRepository tableTokens;
		/// <summary>
		/// The table of variables
		/// </summary>
		protected readonly ROList<Symbol> tableVariables;
		/// <summary>
		/// The table of virtuals
		/// </summary>
		protected readonly ROList<Symbol> tableVirtuals;
		/// <summary>
		/// The nodes' labels
		/// </summary>
		protected readonly BigList<Node> nodes;
		/// <summary>
		/// The index of the tree's root node
		/// </summary>
		protected int root;


		/// <summary>
		/// Initializes this AST
		/// </summary>
		/// <param name="tokens">The table of tokens</param>
		/// <param name="variables">The table of variables</param>
		/// <param name="virtuals">The table of virtuals</param>
		protected AST(TokenRepository tokens, ROList<Symbol> variables, ROList<Symbol> virtuals)
		{
			tableTokens = tokens;
			tableVariables = variables;
			tableVirtuals = virtuals;
			nodes = new BigList<Node>();
			root = -1;
		}

		/// <summary>
		/// Gets the root node of this tree
		/// </summary>
		public ASTNode Root { get { return new ASTNode(this, root); } }

		/// <summary>
		/// Gets the number of children of the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The node's number of children</returns>
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
			TableElemRef sym = nodes[node].label;
			return sym.Type == TableType.Token ? tableTokens.GetPosition(sym.Index) : new TextPosition(0, 0);
		}

		/// <summary>
		/// Gets the span in the input text of the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The span in the text</returns>
		public TextSpan GetSpan(int node)
		{
			TableElemRef sym = nodes[node].label;
			return sym.Type == TableType.Token ? tableTokens.GetSpan(sym.Index) : new TextSpan(0, 0);
		}

		/// <summary>
		/// Gets the context in the input of the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The context</returns>
		public TextContext GetContext(int node)
		{
			TableElemRef sym = nodes[node].label;
			return sym.Type == TableType.Token ? tableTokens.GetContext(sym.Index) : new TextContext();
		}

		/// <summary>
		/// Gets the grammar symbol associated to the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The associated symbol</returns>
		public Symbol GetSymbol(int node)
		{
			return GetSymbolFor(nodes[node].label);
		}

		/// <summary>
		/// Gets the value of the given node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The associated value</returns>
		public string GetValue(int node)
		{
			TableElemRef sym = nodes[node].label;
			return sym.Type == TableType.Token ? tableTokens.GetValue(sym.Index) : null;
		}

		/// <summary>
		/// Gets the symbol corresponding to the specified label
		/// </summary>
		/// <param name="label">A node label</param>
		/// <returns>The corresponding symbol</returns>
		public Symbol GetSymbolFor(TableElemRef label)
		{
			switch (label.Type)
			{
			case TableType.Token:
				return tableTokens.GetSymbol(label.Index);
			case TableType.Variable:
				return tableVariables[label.Index];
			case TableType.Virtual:
				return tableVirtuals[label.Index];
			}
			// This cannot happen
			return new Symbol(0, string.Empty);
		}

		/// <summary>
		/// Gets the semantic element corresponding to the specified node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The corresponding semantic element</returns>
		public SemanticElement GetSemanticElementForNode(int node)
		{
			return GetSemanticElementForLabel(nodes[node].label);
		}

		/// <summary>
		/// Gets the semantic element corresponding to the specified label
		/// </summary>
		/// <param name="label">The label of an AST node</param>
		/// <returns>The corresponding semantic element</returns>
		public SemanticElement GetSemanticElementForLabel(TableElemRef label)
		{
			switch (label.Type)
			{
			case TableType.Token:
				return tableTokens[label.Index];
			case TableType.Variable:
				return new SymbolRef(tableVariables[label.Index]);
			case TableType.Virtual:
				return new SymbolRef(tableVirtuals[label.Index]);
			}
			// This cannot happen
			return null;
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
