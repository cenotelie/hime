﻿/**********************************************************************
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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// A repository of matched tokens
	/// </summary>
	class TokenRepository : TokenDataProvider, IEnumerable<Token>
	{
		/// <summary>
		/// Represents the metadata of a token
		/// </summary>
		protected struct Cell
		{
			/// <summary>
			/// The terminal's index
			/// </summary>
			public int terminal;
			/// <summary>
			/// The span of this token
			/// </summary>
			public TextSpan span;

			/// <summary>
			/// Initializes this cell
			/// </summary>
			/// <param name="terminal">The terminal's index</param>
			/// <param name="span">The token's span in the input</param>
			public Cell(int terminal, TextSpan span)
			{
				this.terminal = terminal;
				this.span = span;
			}
		}

		/// <summary>
		/// Represents an iterator over all the tokens in this repository
		/// </summary>
		protected class LinearEnumerator : IEnumerator<Token>
		{
			/// <summary>
			/// The repository
			/// </summary>
			private TokenRepository repo;
			/// <summary>
			/// The index of the current token
			/// </summary>
			private int current;

			/// <summary>
			/// Initializes this iterator
			/// </summary>
			/// <param name="repo">The repository</param>
			public LinearEnumerator(TokenRepository repo)
			{
				this.repo = repo;
				this.current = -1;
			}

			/// <summary>
			/// Gets the current token
			/// </summary>
			public Token Current { get { return repo[current]; } }

			/// <summary>
			/// Gets the current token
			/// </summary>
			object System.Collections.IEnumerator.Current { get { return repo[current]; } }

			/// <summary>
			/// Moves to the next elmeent
			/// </summary>
			/// <returns>true if there is a next element</returns>
			public bool MoveNext()
			{
				if (current >= repo.Size)
					return false;
				current++;
				return (current >= repo.Size);
			}

			/// <summary>
			/// Resets this iterator
			/// </summary>
			public void Reset()
			{
				current = -1;
			}

			/// <summary>
			/// Disposes of this iterator
			/// </summary>
			public void Dispose()
			{
				repo = null;
			}
		}

		/// <summary>
		/// The terminal symbols matched in this content
		/// </summary>
		protected ROList<Symbol> terminals;
		/// <summary>
		/// The base text
		/// </summary>
		protected Text text;
		/// <summary>
		/// The token data in this content
		/// </summary>
		protected Utils.BigList<Cell> cells;

		/// <summary>
		/// Gets the number of tokens in this repository
		/// </summary>
		public int Size { get { return cells.Size; } }

		/// <summary>
		/// Gets the token at the specified index
		/// </summary>
		/// <param name="index">An index in this repository</param>
		/// <returns>The token at the specified index</returns>
		public Token this[int index] { get { return new Token(this, index); } }

		/// <summary>
		/// Initializes this text
		/// </summary>
		/// <param name="terminals">The terminal symbols</param>
		/// <param name="text">The base text</param>
		public TokenRepository(ROList<Symbol> terminals, Text text)
		{
			this.terminals = terminals;
			this.text = text;
			this.cells = new Hime.Redist.Utils.BigList<Cell>();
		}

		/// <summary>
		/// Gets the position in the input text of the given token
		/// </summary>
		/// <param name="token">A token's index</param>
		/// <returns>The position in the text</returns>
		public TextPosition GetPosition(int token)
		{
			return text.GetPositionAt(cells[token].span.Index);
		}

		/// <summary>
		/// Gets the span in the input text of the given token
		/// </summary>
		/// <param name="token">A token's index</param>
		/// <returns>The span in the text</returns>
		public TextSpan GetSpan(int token)
		{
			return cells[token].span;
		}

		/// <summary>
		/// Gets the context in the input of the given token
		/// </summary>
		/// <param name="node">A token's index</param>
		/// <returns>The context</returns>
		public TextContext GetContext(int token)
		{
			return text.GetContext(cells[token].span);
		}

		/// <summary>
		/// Gets the grammar symbol associated to the given token
		/// </summary>
		/// <param name="token">A token's index</param>
		/// <returns>The associated symbol</returns>
		public Symbol GetSymbol(int token)
		{
			return terminals[cells[token].terminal];
		}

		/// <summary>
		/// Gets the value of the given token
		/// </summary>
		/// <param name="token">A token's index</param>
		/// <returns>The associated value</returns>
		public string GetValue(int token)
		{
			return text.GetValue(cells[token].span);
		}

		/// <summary>
		/// Gets an enumerator of the tokens in this repository
		/// </summary>
		/// <returns>An enumerator of the tokens in this repository</returns>
		public IEnumerator<Token> GetEnumerator()
		{
			return new LinearEnumerator(this);
		}

		/// <summary>
		/// Gets an enumerator of the tokens in this repository
		/// </summary>
		/// <returns>An enumerator of the tokens in this repository</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new LinearEnumerator(this);
		}

		/// <summary>
		/// Registers a new token in this repository
		/// </summary>
		/// <param name="terminal">The index of the matched terminal</param>
		/// <param name="index">The starting index of the matched value in the input</param>
		/// <param name="length">The length of the matched value in the input</param>
		/// <returns>The index of the added token</returns>
		public int Add(int terminal, int index, int length)
		{
			return cells.Add(new Cell(terminal, new TextSpan(index, length)));
		}
	}
}
