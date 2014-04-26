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

using System.IO;

namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Fast rewindable reader of text
    /// </summary>
    class RewindableReader
    {
        /// <summary>
        /// Represents a sinle character in a stream of text
        /// </summary>
        public struct Single
        {
			/// <summary>
			/// The represented character
			/// </summary>
            private char value;

			/// <summary>
			/// Flags wether this character is beyond the end of a stream meaning the value is meaningless
			/// </summary>
            private bool atEnd;

            /// <summary>
            /// Gets the character's value
            /// </summary>
            public char Value { get { return value; } }

            /// <summary>
            /// Gets whether the end of the stream has been reached
            /// </summary>
            public bool AtEnd { get { return atEnd; } }

            /// <summary>
            /// Initializes the character data
            /// </summary>
            /// <param name="value">The character's value</param>
            /// <param name="atEnd">True if the end of the stream has been reached</param>
            public Single(char value, bool atEnd)
            {
                this.value = value;
                this.atEnd = atEnd;
            }
        }

		/// <summary>
		/// The input text reader
		/// </summary>
        private TextReader reader;

		/// <summary>
		/// The text content read so fat
		/// </summary>
        private Content content;

		/// <summary>
		/// The previous buffer
		/// </summary>
        private char[] previous;

		/// <summary>
		/// The next buffer
		/// </summary>
        private char[] next;

		/// <summary>
		/// The length of the next buffer
		/// </summary>
        private int nextLength;

		/// <summary>
		/// The current buffer
		/// </summary>
        private char[] buffer;

		/// <summary>
		/// Start index in the current buffer
		/// </summary>
        private int bufferStart;

		/// <summary>
		/// The length of the current buffer
		/// </summary>
        private int bufferLength;

        /// <summary>
        /// Creates a new Rewindable Text Reader encapsulating the given TextReader
        /// </summary>
        /// <param name="reader">The text reader to encapsulate</param>
        /// <param name="content">The container that will store all read text</param>
        public RewindableReader(TextReader reader, Content content)
        {
            this.reader = reader;
            this.content = content;
        }

        /// <summary>
        /// Goes back in the stream of text already read
        /// </summary>
        /// <param name="index">Index in the current buffer to go back to</param>
        public void GoTo(int index)
        {
            bufferStart = index;
            if (bufferStart < 0)
            {
                // The index was negative, go back to the previous buffer
                // Save the current buffer as the next buffer
                next = buffer;
                nextLength = bufferLength;
                // Reset the previous buffer as the current buffer
                buffer = previous;
                bufferStart += Content.chunksSize;
                bufferLength = Content.chunksSize;
            }
        }

        /// <summary>
        /// Reads text in the input
        /// </summary>
        /// <returns>A buffer of text</returns>
        public TextBuffer Read()
        {
            if (bufferStart != bufferLength)
            {
                // Still not at the end of the current buffer
                // Return the content from the start to the end of the current buffer
                int start = bufferStart;
                bufferStart = bufferLength;
                return new TextBuffer(buffer, start, bufferLength);
            }
            // We need to go to the next chunck
            previous = buffer;
            // Is-il already here (from a previous call to GoTo)?
            if (next != null)
            {
                // Reset the current buffer from the saved next buffer
                buffer = next;
                bufferLength = nextLength;
                next = null;
            }
            else
            {
                // Read the next buffer from the input
                buffer = new char[Content.chunksSize];
                bufferLength = reader.Read(buffer, 0, Content.chunksSize);
                if (bufferLength != 0)
                    content.Append(buffer, bufferLength);
            }
            // Return the entire current buffer
            bufferStart = bufferLength;
            return new TextBuffer(buffer, 0, bufferLength);
        }

        /// <summary>
        /// Reads a single character in the input
        /// </summary>
        /// <returns>The single character with meta-data</returns>
        public Single ReadOne()
        {
            // Still not at the end of the current buffer => return a single character
            if (bufferStart != bufferLength)
                return new Single(buffer[bufferStart++], false);
            // We need to go to the next chunck
            previous = buffer;
            // Is-il already here (from a previous call to GoTo)?
            if (next != null)
            {
                // Reset the current buffer from the saved next buffer
                buffer = next;
                bufferLength = nextLength;
                next = null;
                bufferStart = 1;
                return new Single(buffer[0], false);
            }
            else
            {
                // Read the next buffer from the input
                buffer = new char[Content.chunksSize];
                bufferLength = reader.Read(buffer, 0, Content.chunksSize);
                if (bufferLength != 0)
                {
                    content.Append(buffer, bufferLength);
                    bufferStart = 1;
                    return new Single(buffer[0], false);
                }
                else
                {
                    bufferStart = 0;
                    return new Single('\0', true);
                }
            }
        }
    }
}
