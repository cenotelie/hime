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
		private const int initLineCount = 10000;

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
			this.lines = new int[initLineCount];
			this.line = 0;
			bool flagCR = false;
			for (int i = 0; i != content.Length; i++)
			{
				switch ((int)content[i])
				{
					case 0x0D:
						flagCR = true;
						AddLine(i);
						break;
					case 0x0A:
						if (!flagCR)
							AddLine(i);
						flagCR = false;
						break;
					case 0x0B:
					case 0x0C:
					case 0x85:
					case 0x2028:
					case 0x2029:
						flagCR = false;
						AddLine(i);
						break;
					default:
						flagCR = false;
						break;
				}
			}
			lines[line] = content.Length;
		}

		/// <summary>
		/// Adds a line starting at the specified index
		/// </summary>
		/// <param name="index">An index in the content</param>
		private void AddLine(int index)
		{
			if (line + 1 >= lines.Length)
			{
				int[] t = new int[lines.Length + initLineCount];
				System.Buffer.BlockCopy(lines, 0, t, 0, lines.Length * 4);
				lines = t;
			}
			lines[line++] = index;
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
			return new TextPosition(l + 1, index - lines[l]);
		}

		/// <summary>
		/// Finds the 0-based number of the line at the given index in the content
		/// </summary>
		/// <param name="index">The index within this content</param>
		private int FindLineAt(int index)
		{
			if (lines == null)
				FindLines();
			int start = 0;
			int end = line - 1;
			while (true)
			{
				if (end == start || end == start + 1)
					return start;
				int m = (start + end) / 2;
				int v = lines[m];
				if (index == v)
					return m;
				if (index < v)
					end = m;
				else
					start = m;
			}
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
				return new Symbol(terminal.ID, terminal.Name, GetValue(cell.start, cell.length));
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