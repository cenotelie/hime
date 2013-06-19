using System.IO;

namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Fast rewindable reader of text
    /// </summary>
    class RewindableTextReader
    {
        private TextReader reader;  // Encapsulated text reader
        private TextContent content;// Full text already read
        private char[] buffer;      // First stage buffer for batch reading of the stream
        private int bufferStart;    // Index where the next character shall be read in the buffer
        private int bufferLength;   // Current length of the buffer
        private char[] ring;        // Ring memory of this reader storing the already read characters
        private int ringStart;      // Start index of the ring in case the stream in rewinded
        private int ringNextEntry;  // Index for inserting new characters in the ring
        private int ringSize;       // Size of the ring

        /// <summary>
        /// Creates a new Rewindable Text Reader encapsulating the given TextReader
        /// </summary>
        /// <param name="reader">The text reader to encapsulate</param>
        /// <param name="content">The container that will store all read text</param>
        /// <param name="ringSize">The maximum number of characters that can be rewound</param>
        public RewindableTextReader(TextReader reader, TextContent content, int ringSize)
        {
            this.reader = reader;
            this.content = content;
            this.bufferStart = 0;
            this.bufferLength = 0;
            this.ring = new char[ringSize];
            this.ringStart = 0;
            this.ringNextEntry = 0;
            this.ringSize = ringSize;
        }

        /// <summary>
        /// Goes back in the stream of text already read
        /// </summary>
        /// <param name="count">Number of characters to rewind</param>
        public void Rewind(int count)
        {
            ringStart -= count;
            if (ringStart < 0)
                ringStart += ringSize;
        }

        private char ReadBuffer(out bool atEnd)
        {
            if (bufferStart == bufferLength)
            {
                buffer = new char[TextContent.chunksSize];
                bufferLength = reader.Read(buffer, 0, TextContent.chunksSize);
                bufferStart = 0;
                if (bufferLength == 0)
                {
                    atEnd = true;
                    return char.MinValue;
                }
                content.Append(buffer, bufferLength);
            }
            atEnd = false;
            char c = buffer[bufferStart++];
            ring[ringNextEntry++] = c;
            if (ringNextEntry == ringSize)
                ringNextEntry = 0;
            ringStart = ringNextEntry;
            return c;
        }

        /// <summary>
        /// Reads the next character
        /// </summary>
        /// <param name="atEnd">Out parameter set to true if no more character can be read</param>
        /// <returns>The read character</returns>
        public char Read(out bool atEnd)
        {
            if (ringStart != ringNextEntry)
            {
                atEnd = false;
                char value = ring[ringStart++];
                if (ringStart == ringSize)
                    ringStart = 0;
                return value;
            }
            return ReadBuffer(out atEnd);
        }
    }
}
