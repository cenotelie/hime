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
using System.IO;
using Hime.Redist.Utils;

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Text provider that uses a stream as a backend
	/// </summary>
	/// <remarks>
	/// All line numbers and column numbers are 1-based.
	/// Indices in the content are 0-based.
	/// </remarks>
	class StreamingText : BaseText
	{
		/// <summary>
		/// The size of text block
		/// </summary>
		private const int BLOCK_SIZE = 256;

		/// <summary>
		/// The input to use
		/// </summary>
		private readonly TextReader input;
		/// <summary>
		/// The content read so far
		/// </summary>
		private readonly BigList<char> content;
		/// <summary>
		/// A buffer for reading text
		/// </summary>
		private char[] buffer;
		/// <summary>
		/// Whether the complete input has been read
		/// </summary>
		private bool atEnd;

		/// <summary>
		/// Initializes this text
		/// </summary>
		/// <param name="input">The input text</param>
		public StreamingText(TextReader input)
		{
			this.input = input;
			this.content = new BigList<char>();
			this.buffer = new char[BLOCK_SIZE];
			this.atEnd = false;
		}

		/// <summary>
		/// Reads the input so as to make the specified index available
		/// </summary>
		/// <param name="index">An index from the start</param>
		private void MakeAvailable(int index)
		{
			if (atEnd)
				return;
			while (index > content.Size)
			{
				int read = input.Read(buffer, 0, BLOCK_SIZE);
				if (read == 0)
				{
					atEnd = true;
					return;
				}
				content.Add(buffer, 0, read);
			}
		}

		/// <summary>
		/// Gets the character at the specified index
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns>The character at the specified index</returns>
		public override char GetValue(int index)
		{
			MakeAvailable(index);
			return content[index];
		}

		/// <summary>
		/// Gets whether the specified index is after the end of the text represented by this object
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns><c>true</c> if the index is after the end of the text</returns>
		public override bool IsEnd(int index)
		{
			MakeAvailable(index + 1);
			return (index >= content.Size);
		}

		/// <summary>
		/// Finds all the lines in this content
		/// </summary>
		protected override void FindLines()
		{
			if (!atEnd)
				MakeAvailable(System.Int32.MaxValue);
			lines = new int[INIT_LINE_COUNT_CACHE_SIZE];
			lines[0] = 0;
			line = 1;
			char c1 = '\0';
			char c2 = '\0';
			for (int i = 0; i != content.Size; i++)
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
		/// Gets the size in number of characters
		/// </summary>
		public override int Size { get { return content.Size; } }

		/// <summary>
		/// Gets the substring beginning at the given index with the given length
		/// </summary>
		/// <param name="index">Index of the substring from the start</param>
		/// <param name="length">Length of the substring</param>
		/// <returns>The substring</returns>
		public override string GetValue(int index, int length)
		{
			if (length == 0)
				return string.Empty;
			if (buffer.Length < length)
				buffer = new char[length];
			content.CopyTo(index, length, buffer, 0);
			return new string(buffer, 0, length);
		}

		/// <summary>
		/// Gets the length of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The length of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public override int GetLineLength(int line)
		{
			if (lines == null)
				FindLines();
			return line == this.line ? (content.Size - lines [this.line - 1]) : (lines [line] - lines [line - 1]);
		}
	}
}
