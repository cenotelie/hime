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
	/// Represents a label an AST node
	/// </summary>
	struct ASTLabel : SemanticElement
	{
		/// <summary>
		/// The symbol being referenced
		/// </summary>
		private readonly Symbol symbol;
		/// <summary>
		/// The type of this symbol
		/// </summary>
		private readonly SymbolType type;

		/// <summary>
		/// Gets the type of symbol this element represents
		/// </summary>
		public SymbolType SymbolType { get { return type; } }

		/// <summary>
		/// Gets the position in the input text of this element
		/// </summary>
		public TextPosition Position { get { return new TextPosition(0, 0); } }

		/// <summary>
		/// Gets the span in the input text of this element
		/// </summary>
		public TextSpan Span { get { return new TextSpan(0, 0); } }

		/// <summary>
		/// Gets the context of this element in the input
		/// </summary>
		public TextContext Context { get { return new TextContext(); } }

		/// <summary>
		/// Gets the grammar symbol associated to this element
		/// </summary>
		public Symbol Symbol { get { return symbol; } }

		/// <summary>
		/// Gets the value of this element, if any
		/// </summary>
		public string Value { get { return null; } }

		/// <summary>
		/// Initializes this reference
		/// </summary>
		/// <param name="symbol">The symbol being referenced</param>
		/// <param name="type">The type of this symbol</param>
		public ASTLabel(Symbol symbol, SymbolType type)
		{
			this.symbol = symbol;
			this.type = type;
		}

		/// <summary>
		/// Gets a string representation of this token
		/// </summary>
		/// <returns>The string representation of the token</returns>
		public override string ToString()
		{
			return symbol.Name;
		}
	}
}
