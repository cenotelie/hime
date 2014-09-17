/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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
	/// Stores the full content of an input lexer
	/// </summary>
	/// <remarks>
	/// All line numbers and column numbers are 1-based.
	/// Indices in the content are 0-based.
	/// </remarks>
	class PrefetchedText : TokenizedText
	{
		/// <summary>
		/// The initiaal size of the cache of line start indices
		/// </summary>
		private const int INIT_LINE_COUNT_CACHE_SIZE = 10000;

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
		/// The full content of the input
		/// </summary>
		private string content;
		/// <summary>
		/// Cache of the starting indices of each line within the text
		/// </summary>
		private int[] lines;
		/// <summary>
		/// Index of the next line
		/// </summary>
		private int line;
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
		/// <param name="content">The full lexer's input as a string</param>
		public PrefetchedText(IList<Symbol> terminals, string content)
		{
			this.content = content;
			this.terminals = terminals;
			this.cells = new Hime.Redist.Utils.BigList<Cell>();
		}

		#region Internal API
		/// <summary>
		/// Finds all the lines in this content
		/// </summary>
		public void FindLines()
		{
			this.lines = new int[INIT_LINE_COUNT_CACHE_SIZE];
			this.lines[0] = 0;
			this.line = 1;
			char c1 = '\0';
			char c2 = '\0';
			for (int i = 0; i != content.Length; i++)
			{
				c1 = c2;
				c2 = content[i];
				// is c1 c2 a line ending sequence?
				if (IsLineEnding(c1, c2))
				{
					// are we late to detect MacOS style?
					if (c1 == '\u000D' && c2 != '\u000A')
						AddLine(i);
					else
						AddLine(i + 1);
				}
			}
		}

		/// <summary>
		/// Determines whether [c1, c2] form a line ending sequence
		/// </summary>
		/// <param name="c1">First character</param>
		/// <param name="c2">Second character</param>
		/// <returns><c>true</c> if this is a line ending sequence</returns>
		/// <remarks>
		/// Recognized sequences are:
		/// [U+000D, U+000A] (this is Windows-style \r \n)
		/// [U+????, U+000A] (this is unix style \n)
		/// [U+000D, U+????] (this is MacOS style \r, without \n after)
		/// Others:
		/// [?, U+000B], [?, U+000C], [?, U+0085], [?, U+2028], [?, U+2029]
		/// </remarks>
		private bool IsLineEnding(char c1, char c2)
		{
			// other characters
			if (c2 == '\u000B' || c2 == '\u000C' || c2 == '\u0085' || c2 == '\u2028' || c2 == '\u2029')
				return true;
			// matches [\r, \n] [\r, ??] and  [??, \n]
			if (c1 == '\u000D' || c2 == '\u000A')
				return true;
			return false;
		}

		/// <summary>
		/// Adds a line starting at the specified index
		/// </summary>
		/// <param name="index">An index in the content</param>
		private void AddLine(int index)
		{
			if (line >= lines.Length)
			{
				int[] t = new int[lines.Length + INIT_LINE_COUNT_CACHE_SIZE];
				System.Buffer.BlockCopy(lines, 0, t, 0, lines.Length * 4);
				lines = t;
			}
			lines[line] = index;
			line++;
		}

		/// <summary>
		/// Adds a detected token in this text
		/// </summary>
		/// <param name="terminal">Index of the matched terminal</param>
		/// <param name="start">Start index in the text</param>
		/// <param name="length">Length of the token</param>
		public void AddToken(int terminal, int start, int length)
		{
			cells.Add(new Cell(terminal, start, length));
		}

		/// <summary>
		/// Gets the token at the specified index
		/// </summary>
		/// <param name="index">A token's index</param>
		/// <returns>The token at the specified index</returns>
		public Token GetTokenAt(int index)
		{
			Cell cell = cells[index];
			return new Token(terminals[cell.terminal].ID, index);
		}
		#endregion


		#region Implementation of Text
		/// <summary>
		/// Gets the number of lines
		/// </summary>
		public int LineCount
		{
			get
			{
				if (lines == null)
					FindLines();
				return line;
			}
		}

		/// <summary>
		/// Gets the size in number of characters
		/// </summary>
		public int Size { get { return content.Length; } }

		/// <summary>
		/// Gets the substring beginning at the given index with the given length
		/// </summary>
		/// <param name="index">Index of the substring from the start</param>
		/// <param name="length">Length of the substring</param>
		/// <returns>The substring</returns>
		public string GetValue(int index, int length)
		{
			if (length == 0)
				return string.Empty;
			return content.Substring(index, length);
		}

		/// <summary>
		/// Gets the starting index of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The starting index of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public int GetLineIndex(int line)
		{
			if (lines == null)
				FindLines();
			return lines[line - 1];
		}

		/// <summary>
		/// Gets the length of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The length of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public int GetLineLength(int line)
		{
			if (lines == null)
				FindLines();
			if (line == this.line)
				return (content.Length - lines[this.line - 1]);
			return (lines[line] - lines[line - 1]);
		}

		/// <summary>
		/// Gets the string content of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The string content of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public string GetLineContent(int line)
		{
			return GetValue(GetLineIndex(line), GetLineLength(line));
		}

		/// <summary>
		/// Gets the position at the given index
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns>The position (line and column) at the index</returns>
		public TextPosition GetPositionAt(int index)
		{
			int l = FindLineAt(index);
			return new TextPosition(l + 1, index - lines[l] + 1);
		}

		/// <summary>
		/// Gets the context description for the current text at the specified position
		/// </summary>
		/// <param name="position">The position in this text</param>
		/// <returns>The context description</returns>
		public Context GetContext(TextPosition position)
		{
			string content = GetLineContent(position.Line);
			if (content.Length == 0)
				return new Context("", "^" );
			int end = content.Length - 1;
			while (end != 1 && (content[end] == '\n' || content[end] == '\r'))
				end--;
			int start = 0;
			while (start < end && char.IsWhiteSpace(content[start]))
				start++;
			if (position.Column - 1 < start)
				start = 0;
			if (position.Column - 1 > end)
				end = content.Length - 1;
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			for (int i=start; i!=position.Column - 1; i++)
				builder.Append(content[i] == '\t' ? '\t' : ' ');
			builder.Append("^");
			return new Context(content.Substring(start, end - start + 1), builder.ToString());
		}

		/// <summary>
		/// Finds the index in the cache of the line at the given input index in the content
		/// </summary>
		/// <param name="index">The index within this content</param>
		/// <returns>The index of the corresponding line in the cache</returns>
		private int FindLineAt(int index)
		{
			if (lines == null)
				FindLines();
			for (int i = 1; i != line; i++)
			{
				if (index < lines[i])
					return i - 1;
			}
			return line - 1;
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
			private PrefetchedText text;
			private int index;

			public SymbolEnumerator(PrefetchedText text)
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
				if (terminal.ID == Symbol.SID_DOLLAR || terminal.ID == Symbol.SID_EPSILON)
					return new Symbol(terminal.ID, terminal.Name, "<EOF>");
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