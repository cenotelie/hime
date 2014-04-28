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
	class TokenizedContent : Content, TokenizedText
	{
		/// <summary>
		/// Represents the metadata of a token
		/// </summary>
		private struct Cell
		{
			/// <summary>
			/// The terminal's index
			/// </summary>
			public int terminal;// The terminal's index

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
		private SymbolDictionary terminals;

		/// <summary>
		/// The token data in this content
		/// </summary>
		private Utils.BigList<Cell> cells;

		/// <summary>
		/// Initializes this text
		/// </summary>
		/// <param name="terminals">The terminal symbols</param>
		public TokenizedContent(SymbolDictionary terminals)
		{
			this.terminals = terminals;
			this.cells = new Utils.BigList<Cell>();
		}

		/// <summary>
		/// Adds a detected token in this text
		/// </summary>
		/// <param name="terminal">Index of the matched terminal</param>
		/// <param name="start">Start index in the text</param>
		/// <param name="length">Length of the token</param>
		/// <returns></returns>
		internal Token OnToken(int terminal, int start, int length)
		{
			return new Token(terminals[terminal].ID, cells.Add(new Cell(terminal, start, length)));
		}

        #region Implementation of IEnumerable
		/// <summary>
		/// Gets an enumerator of the contained tokens
		/// </summary>
		/// <returns>An enumerator of tokens</returns>
		public IEnumerator<Token> GetEnumerator()
		{
			return new TokenEnumerator(this);
		}

		/// <summary>
		/// Gets an enumerator of the contained tokens
		/// </summary>
		/// <returns>An enumerator of tokens</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new TokenEnumerator(this);
		}

		/// <summary>
		/// Represents an enumerator of tokens
		/// </summary>
		private class TokenEnumerator : IEnumerator<Token>
		{
			private TokenizedContent text;
			private int index;

			public TokenEnumerator(TokenizedContent text)
			{
				this.text = text;
				this.index = -1;
			}

			public Token Current {
				get {
					Cell cell = text.cells[index];
					return new Token(text.terminals[cell.terminal].ID, index);
				}
			}

			object System.Collections.IEnumerator.Current {
				get {
					Cell cell = text.cells[index];
					return new Token(text.terminals[cell.terminal].ID, index);
				}
			}

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
		/// Gets the number of tokens in this text
		/// </summary>
		public int TokenCount { get { return cells.Size; } }

		/// <summary>
		/// Gets the token at the given index
		/// </summary>
		/// <param name="index">An index</param>
		/// <returns>The token</returns>
		public Token this[int index] {
			get {
				Cell cell = cells[index];
				Symbol terminal = terminals[cell.terminal];
				return new Token(terminal.ID, index);
			}
		}

		/// <summary>
		/// Gets the value of the token at the given index
		/// </summary>
		/// <param name="index">An index</param>
		/// <returns>The corresponding symbol</returns>
		public Symbol GetSymbolAt(int index)
		{
			Cell cell = cells[index];
			Symbol terminal = terminals[cell.terminal];
			string value = GetValue(cell.start, cell.length);
			return new Symbol(terminal.ID, terminal.Name, value);
		}

		/// <summary>
		/// Gets the value of the given token
		/// </summary>
		/// <param name="token">A token in this text</param>
		/// <returns>The token's value as a string</returns>
		public Symbol GetSymbol(Token token)
		{
			return GetSymbolAt(token.Index);
		}

		/// <summary>
		/// Gets the string value of the given token
		/// </summary>
		/// <param name="token">A token</param>
		/// <returns>The string value of the given token</returns>
		public string GetValue(Token token)
		{
			Cell cell = cells[token.Index];
			return GetValue(cell.start, cell.length);
		}

		/// <summary>
		/// Gets the line number of the given token
		/// </summary>
		/// <param name="token">A token</param>
		/// <returns>The line number of the given token</returns>
		public int GetLineOf(Token token)
		{
			Cell cell = cells[token.Index];
			return GetLineAt(cell.start);
		}

		/// <summary>
		/// Gets the column number of the given token
		/// </summary>
		/// <param name="token">A token</param>
		/// <returns>The column number of the given token</returns>
		public int GetColumnOf(Token token)
		{
			Cell cell = cells[token.Index];
			return GetColumnAt(cell.start);
		}

		/// <summary>
		/// Gets the position of the given token
		/// </summary>
		/// <param name="token">A token</param>
		/// <returns>The position (line and column) of the given token</returns>
		public TextPosition GetPositionOf(Token token)
		{
			Cell cell = cells[token.Index];
			return GetPositionAt(cell.start);
		}
        #endregion
	}
}
