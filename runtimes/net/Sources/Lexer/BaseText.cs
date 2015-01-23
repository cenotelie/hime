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
	/// Represents the base implementation of Text
	/// </summary>
	/// <remarks>
	/// All line numbers and column numbers are 1-based.
	/// Indices in the content are 0-based.
	/// </remarks>
	abstract class BaseText : Text
	{
		/// <summary>
		/// The initiaal size of the cache of line start indices
		/// </summary>
		protected const int INIT_LINE_COUNT_CACHE_SIZE = 10000;

		/// <summary>
		/// Cache of the starting indices of each line within the text
		/// </summary>
		protected int[] lines;
		/// <summary>
		/// Index of the next line
		/// </summary>
		protected int line;


		/// <summary>
		/// Gets the character at the specified index
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns>The character at the specified index</returns>
		public abstract char GetValue(int index);

		/// <summary>
		/// Gets whether the specified index is after the end of the text represented by this object
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns><c>true</c> if the index is after the end of the text</returns>
		public abstract bool IsEnd(int index);

		/// <summary>
		/// Finds all the lines in this content
		/// </summary>
		protected abstract void FindLines();

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
		protected bool IsLineEnding(char c1, char c2)
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
		protected void AddLine(int index)
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
		/// Finds the index in the cache of the line at the given input index in the content
		/// </summary>
		/// <param name="index">The index within this content</param>
		/// <returns>The index of the corresponding line in the cache</returns>
		protected int FindLineAt(int index)
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
		public abstract int Size { get; }

		/// <summary>
		/// Gets the substring beginning at the given index with the given length
		/// </summary>
		/// <param name="index">Index of the substring from the start</param>
		/// <param name="length">Length of the substring</param>
		/// <returns>The substring</returns>
		public abstract string GetValue(int index, int length);

		/// <summary>
		/// Get the substring corresponding to the specified span
		/// </summary>
		/// <param name="span">A span in this text</param>
		/// <returns>The substring</returns>
		public string GetValue(TextSpan span)
		{
			return GetValue(span.Index, span.Length);
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
		public abstract int GetLineLength(int line);

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
		public TextContext GetContext(TextPosition position)
		{
			return GetContext(position, 1);
		}

		/// <summary>
		/// Gets the context description for the current text at the specified position
		/// </summary>
		/// <param name="position">The position in this text</param>
		/// <param name="length">The length of the element to contextualize</param>
		/// <returns>The context description</returns>
		public TextContext GetContext(TextPosition position, int length)
		{
			string content = GetLineContent(position.Line);
			if (content.Length == 0)
				return new TextContext("", "^");
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
			for (int i = start; i != position.Column - 1; i++)
				builder.Append(content[i] == '\t' ? '\t' : ' ');
			for (int i = 0; i != length; i++)
				builder.Append("^");
			return new TextContext(content.Substring(start, end - start + 1), builder.ToString());
		}

		/// <summary>
		/// Gets the context description for the current text at the specified span
		/// </summary>
		/// <param name="span">The span of text to contextualize</param>
		/// <returns>The context description</returns>
		public TextContext GetContext(TextSpan span)
		{
			TextPosition position = GetPositionAt(span.Index);
			return GetContext(position, span.Length);
		}
	}
}
