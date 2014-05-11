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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents the output of a lexer as a tokenized text
	/// </summary>
	abstract class TokenizedInput : TokenizedText
	{
		/// <summary>
		/// Represents the metadata of a token
		/// </summary>
		private struct Cell
		{
			/// <summary>
			/// The terminal's index
			/// </summary>
			public int terminal;

			/// <summary>
			/// Start index of the text
			/// </summary>
			public int start;

			/// <summary>
			/// Length of the token
			/// </summary>
			public int length;

			/// <summary>
			/// Initializes this cell
			/// </summary>
			/// <param name="terminal">The terminal's index</param>
			/// <param name="start">Start index of the text</param>
			/// <param name="length">Length of the token</param>
			public Cell(int terminal, int start, int length)
			{
				this.terminal = terminal;
				this.start = start;
				this.length = length;
			}
		}

		/// <summary>
		/// The terminal symbols matched in this content
		/// </summary>
		private IList<Symbol> terminals;
		/// <summary>
		/// The token data in this content
		/// </summary>
		private Utils.BigList<Cell> cells;

		/// <summary>
		/// Initializes this text
		/// </summary>
		/// <param name="terminals">The terminal symbols</param>
		public TokenizedInput(IList<Symbol> terminals)
		{
			this.terminals = terminals;
			this.cells = new Utils.BigList<Cell>();
		}


		#region Internal API
		/// <summary>
		/// Adds a detected token in this text
		/// </summary>
		/// <param name="terminal">Index of the matched terminal</param>
		/// <param name="start">Start index in the text</param>
		/// <param name="length">Length of the token</param>
		/// <returns></returns>
		public Token AddToken(int terminal, int start, int length)
		{
			return new Token(terminals[terminal].ID, cells.Add(new Cell(terminal, start, length)));
		}
		#endregion


		#region Implementation of IEnumerable
		/// <summary>
		/// Gets an enumerator of the contained tokens
		/// </summary>
		/// <returns>An enumerator of tokens</returns>
		public IEnumerator<Symbol> GetEnumerator()
		{
			return new SymbolEnumerator(this);
		}

		/// <summary>
		/// Gets an enumerator of the contained tokens
		/// </summary>
		/// <returns>An enumerator of tokens</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new SymbolEnumerator(this);
		}

		/// <summary>
		/// Represents an enumerator of tokens
		/// </summary>
		private class SymbolEnumerator : IEnumerator<Symbol>
		{
			private TokenizedInput text;
			private int index;

			public SymbolEnumerator(TokenizedInput text)
			{
				this.text = text;
				this.index = -1;
			}

			public Symbol Current { get { return text[index]; } }

			object System.Collections.IEnumerator.Current { get { return text[index]; } }

			public bool MoveNext()
			{
				index++;
				return (index == text.cells.Size);
			}

			public void Reset()
			{
				index = -1;
			}

			public void Dispose()
			{
				text = null;
			}
		}
		#endregion


		#region Implementation of TokenizedText
		/// <summary>
		/// Gets the number of lines
		/// </summary>
		public abstract int LineCount { get; }

		/// <summary>
		/// Gets the size in number of characters
		/// </summary>
		public abstract int Size { get; }

		/// <summary>
		/// Gets the substring beginning at the given index with the given length
		/// </summary>
		/// <param name="index">Index of the substring from the start</param>
		/// <param name="length">Length of the substring</param>
		/// <returns>The substring</returns>
		public abstract string GetValue(int index, int length);

		/// <summary>
		/// Gets the starting index of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The starting index of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public abstract int GetLineIndex(int line);

		/// <summary>
		/// Gets the length of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The length of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public abstract int GetLineLength(int line);

		/// <summary>
		/// Gets the string content of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The string content of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public abstract string GetLineContent(int line);

		/// <summary>
		/// Gets the position at the given index
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns>The position (line and column) at the index</returns>
		public abstract TextPosition GetPositionAt(int index);
		#endregion


		#region Implementation of TokenizedText
		/// <summary>
		/// Gets the number of tokens in this text
		/// </summary>
		public int TokenCount { get { return cells.Size; } }

		/// <summary>
		/// Gets the token at the given index
		/// </summary>
		/// <param name="index">An index</param>
		/// <returns>The token</returns>
		public Symbol this[int index]
		{
			get
			{
				Cell cell = cells[index];
				Symbol terminal = terminals[cell.terminal];
				return new Symbol(terminal.ID, terminal.Name, GetValue(cell.start, cell.length));
			}
		}

		/// <summary>
		/// Gets the position of the token at the given index
		/// </summary>
		/// <param name="tokenIndex">The index of a token</param>
		/// <returns>The position (line and column) of the token</returns>
		public TextPosition GetPositionOf(int tokenIndex)
		{
			Cell cell = cells[tokenIndex];
			return GetPositionAt(cell.start);
		}
		#endregion
	}
}
