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

namespace Hime.Redist
{
	/// <summary>
	/// Represents a node in an Abstract Syntax Tree
	/// </summary>
	public struct ASTNode : SemanticElement
	{
		/// <summary>
		/// The parent parse tree
		/// </summary>
		private readonly AST tree;
		/// <summary>
		/// The index of this node in the parse tree
		/// </summary>
		private readonly int index;

		/// <summary>
		/// Gets the type of symbol this element represents
		/// </summary>
		public SymbolType SymbolType { get { return tree.GetSymbolType(index); } }

		/// <summary>
		/// Gets the children of this node
		/// </summary>
		public ASTFamily Children { get { return new ASTFamily(tree, index); } }

		/// <summary>
		/// Gets the position in the input text of this node
		/// </summary>
		public TextPosition Position { get { return tree.GetPosition(index); } }

		/// <summary>
		/// Gets the span in the input text of this node
		/// </summary>
		public TextSpan Span { get { return tree.GetSpan(index); } }

		/// <summary>
		/// Gets the context of this node in the input
		/// </summary>
		public TextContext Context { get { return tree.GetContext(index); } }

		/// <summary>
		/// Gets the grammar symbol associated to this node
		/// </summary>
		public Symbol Symbol { get { return tree.GetSymbol(index); } }

		/// <summary>
		/// Gets the value of this node
		/// </summary>
		public string Value { get { return tree.GetValue(index); } }

		/// <summary>
		/// Initializes this node
		/// </summary>
		/// <param name="tree">The parent parse tree</param>
		/// <param name="index">The index of this node in the parse tree</param>
		internal ASTNode(AST tree, int index)
		{
			this.tree = tree;
			this.index = index;
		}

		/// <summary>
		/// Gets a string representation of this node
		/// </summary>
		/// <returns>The string representation of this node</returns>
		public override string ToString()
		{
			string name = tree.GetSymbol(index).Name;
			string value = tree.GetValue(index);
			if (value != null)
				return name + " = " + value;
			return name;
		}
	}
}
