using System.IO;

namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Fast rewindable reader of text
    /// </summary>
    class RewindableTextReader
    {
        /// <summary>
        /// Represents a sinle character in a stream of text
        /// </summary>
        public struct Single
        {
            private char value;
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

        private TextReader reader;
        private TextContent content;
        private char[] previous;
        private char[] next;
        private int nextLength;
        private char[] buffer;
        private int bufferStart;
        private int bufferLength;

        /// <summary>
        /// Creates a new Rewindable Text Reader encapsulating the given TextReader
        /// </summary>
        /// <param name="reader">The text reader to encapsulate</param>
        /// <param name="content">The container that will store all read text</param>
        public RewindableTextReader(TextReader reader, TextContent content)
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
                next = buffer;
                nextLength = bufferLength;
                buffer = previous;
                bufferStart += TextContent.chunksSize;
                bufferLength = TextContent.chunksSize;
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
                int start = bufferStart;
                bufferStart = bufferLength;
                return new TextBuffer(buffer, start, bufferLength);
            }
            // We need to go to the next chunck
            previous = buffer;
            // Is-il already here?
            if (next != null)
            {
                buffer = next;
                bufferLength = nextLength;
                next = null;
            }
            else
            {
                buffer = new char[TextContent.chunksSize];
                bufferLength = reader.Read(buffer, 0, TextContent.chunksSize);
                if (bufferLength != 0)
                    content.Append(buffer, bufferLength);
            }
            bufferStart = bufferLength;
            return new TextBuffer(buffer, 0, bufferLength);
        }

        /// <summary>
        /// Reads a single character in the input
        /// </summary>
        /// <returns>The single character with meta-data</returns>
        public Single ReadOne()
        {
            if (bufferStart != bufferLength)
                return new Single(buffer[bufferStart++], false);
            // We need to go to the next chunck
            previous = buffer;
            // Is-il already here?
            if (next != null)
            {
                buffer = next;
                bufferLength = nextLength;
                next = null;
                bufferStart = 1;
                return new Single(buffer[0], false);
            }
            else
            {
                buffer = new char[TextContent.chunksSize];
                bufferLength = reader.Read(buffer, 0, TextContent.chunksSize);
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
