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
using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Stores the content of the text read by a lexer
	/// </summary>
	/// <remarks>
	/// All line numbers and column numbers are 1-based.
	/// Indices in the content are 0-based.
	/// </remarks>
	class StreamedText : TokenizedInput
	{
		/// <summary>
		/// The initiaal size of the cache of line start indices
		/// </summary>
		private const int initLineCount = 10000;
		/// <summary>
		/// The number of bits allocated to the lowest part of the index (within a chunk)
		/// </summary>
		private const int upperShift = 12;
		/// <summary>
		/// The size of the chunks
		/// </summary>
		public const int chunksSize = 1 << upperShift;
		/// <summary>
		/// Bit mask for the lowest part of the index (within a chunk)
		/// </summary>
		private const int lowerMask = chunksSize - 1;

		/// <summary>
		/// The chunks of data
		/// </summary>
		private char[][] chunks;

		/// <summary>
		/// The index of the next chunk
		/// </summary>
		private int chunkIndex;

		/// <summary>
		/// Cache of the starting indices of each line within the text
		/// </summary>
		private int[] lines;

		/// <summary>
		/// Index of the next line
		/// </summary>
		private int line;

		/// <summary>
		/// Line ending state, true if immediately after a CR character.
		/// If a LF character is found immediately afeter a CR character, this is a Windows-style line ending marker.
		/// </summary>
		private bool flagCR;

		/// <summary>
		/// The total size of this content in number of characters
		/// </summary>
		private int size;

		/// <summary>
		/// Gets the number of lines
		/// </summary>
		public override int LineCount { get { return line + 1; } }

		/// <summary>
		/// Gets the size in number of characters
		/// </summary>
		public override int Size { get { return size; } }

		/// <summary>
		/// Initializes this text
		/// </summary>
		/// <param name="terminals">The terminal symbols</param>
		public StreamedText(IList<Symbol> terminals) : base(terminals)
		{
			this.chunks = new char[chunksSize][];
			this.chunkIndex = 0;
			this.lines = new int[initLineCount];
			this.line = 0;
			this.size = 0;
		}

		/// <summary>
		/// Gets the substring beginning at the given index with the given length
		/// </summary>
		/// <param name="index">Index of the substring from the start</param>
		/// <param name="length">Length of the substring</param>
		/// <returns>The substring</returns>
		public override string GetValue(int index, int length)
		{
			if (length == 0)
				return "";

			int chunk = index >> upperShift;  // index of the chunk
			int start = index & lowerMask; // start index in the chunk

			if (start + length <= chunksSize)
			{
				// The substring is contained within only one chunk
				return new string(chunks[chunk], start, length);
			}

			// Now we need a string builder
			StringBuilder builder = new StringBuilder(length);
			// Finish the current chunk
			builder.Append(chunks[chunk], start, chunksSize - start);
			int remaining = length - chunksSize + start;
			chunk++;
			// While we can still add complete chunks
			while (remaining > chunksSize)
			{
				builder.Append(chunks[chunk], 0, chunksSize);
				remaining -= chunksSize;
				chunk++;
			}
			// Add the last part and return
			builder.Append(chunks[chunk], 0, remaining);
			return builder.ToString();
		}

		/// <summary>
		/// Gets the starting index of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The starting index of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public override int GetLineIndex(int line)
		{
			return lines[line - 1];
		}

		/// <summary>
		/// Gets the length of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The length of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public override int GetLineLength(int line)
		{
			if (this.line == line - 1)
				return (size - lines[line - 1]);
			return (lines[line] - lines[line - 1]);
		}

		/// <summary>
		/// Gets the string content of the i-th line
		/// </summary>
		/// <param name="line">The line number</param>
		/// <returns>The string content of the line</returns>
		/// <remarks>The line numbering is 1-based</remarks>
		public override string GetLineContent(int line)
		{
			return GetValue(GetLineIndex(line), GetLineLength(line));
		}

		/// <summary>
		/// Gets the position at the given index
		/// </summary>
		/// <param name="index">Index from the start</param>
		/// <returns>The position (line and column) at the index</returns>
		public override TextPosition GetPositionAt(int index)
		{
			int l = FindLineAt(index);
			return new TextPosition(l + 1, index - lines[l] + 1);
		}

		/// <summary>
		/// Finds the 0-based number of the line at the given index in the content
		/// </summary>
		/// <param name="index">The index within this content</param>
		private int FindLineAt(int index)
		{
			int start = 0;
			int end = line;
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

		/// <summary>
		/// Appends the given buffer with the given number of characters
		/// </summary>
		/// <param name="buffer">The buffer to append</param>
		/// <param name="count">The number of characters in the buffer</param>
		public void Append(char[] buffer, int count)
		{
			// Append the new chunk
			if (chunkIndex == chunks.Length - 1)
			{
				char[][] r = new char[chunks.Length + chunksSize][];
				Array.Copy(chunks, r, chunks.Length);
				chunks = r;
			}
			chunks[chunkIndex] = buffer;
			// Ensure enough storage for lines data
			if (line + chunksSize >= lines.Length)
			{
				int[] t = new int[lines.Length + initLineCount];
				Buffer.BlockCopy(lines, 0, t, 0, lines.Length * 4);
				lines = t;
			}
			// Run the state-machine for line endings
			int part = chunkIndex << upperShift;
			for (int i = 0; i != count; i++)
			{
				switch ((int)buffer[i])
				{
					case 0x0D:
						flagCR = true;
						lines[++line] = part | i;
						break;
					case 0x0A:
						if (!flagCR)
							lines[++line] = part | i;
						flagCR = false;
						break;
					case 0x0B:
					case 0x0C:
					case 0x85:
					case 0x2028:
					case 0x2029:
						flagCR = false;
						lines[++line] = part | i;
						break;
					default:
						flagCR = false;
						break;
				}
			}
			chunkIndex++;
			size += count;
		}
	}
}
