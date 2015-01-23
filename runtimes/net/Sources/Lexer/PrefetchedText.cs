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
	/// Text provider that fetches and stores the full content of an input lexer
	/// </summary>
	/// <remarks>
	/// All line numbers and column numbers are 1-based.
	/// Indices in the content are 0-based.
	/// </remarks>
	class PrefetchedText : BaseText
	{
		/// <summary>
		/// The full content of the input
		/// </summary>
		private string content;

		/// <summary>
		/// Initializes this text
		/// </summary>
		/// <param name="content">The full lexer's input as a string</param>
		public PrefetchedText(string content)
		{
			this.content = content;
		}

		/// <summary>
		/// Gets the character at the specified index
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns>The character at the specified index</returns>
		public override char GetValue(int index)
		{
			return content[index];
		}

		/// <summary>
		/// Gets whether the specified index is after the end of the text represented by this object
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns><c>true</c> if the index is after the end of the text</returns>
		public override bool IsEnd(int index)
		{
			return (index >= content.Length);
		}

		/// <summary>
		/// Finds all the lines in this content
		/// </summary>
		protected override void FindLines()
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
		/// Gets the size in number of characters
		/// </summary>
		public override int Size { get { return content.Length; } }

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
			return content.Substring(index, length);
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
			if (line == this.line)
				return (content.Length - lines[this.line - 1]);
			return (lines[line] - lines[line - 1]);
		}
	}
}