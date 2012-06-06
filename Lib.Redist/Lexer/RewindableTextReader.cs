/*
 * Author: Laurent Wouters
 * Date: 03/06/2012
 * Time: 09:20
 * 
 */

using System.IO;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Fast rewindable reader of text
    /// </summary>
    public sealed class RewindableTextReader
    {
        private const int bufferInitSize = 1024;

        private TextReader reader;  // Encapsulated text reader
        private char[] buffer;      // First stage buffer for reading batch reading of the stream
        private int bufferStart;    // Index where the next character shall be read in the buffer
        private int bufferSize;     // Current size of the buffer
        private char[] ring;        // Ring memory of this reader storing the already read characters
        private int ringStart;      // Start index of the ring in case the stream in rewinded
        private int ringNextEntry;  // Index for inserting new characters in the ring

        /// <summary>
        /// Creates a new Rewindable Text Reader encapsulating the given TextReader
        /// </summary>
        /// <param name="reader">Text reader to encapsulate</param>
        public RewindableTextReader(TextReader reader)
        {
            this.reader = reader;
            this.buffer = new char[bufferInitSize];
            this.bufferStart = 0;
            this.bufferSize = 0;
            this.ring = new char[bufferInitSize];
            this.ringStart = 0;
            this.ringNextEntry = 0;
        }

        private bool IsRingEmpty() { return (ringStart == ringNextEntry); }

        private char ReadRing()
        {
            char value = ring[ringStart];
            ringStart++;
            if (ringStart == ring.Length)
                ringStart = 0;
            return value;
        }

        private void PushInRing(char value)
        {
            ring[ringNextEntry++] = value;
            if (ringNextEntry == bufferInitSize)
                ringNextEntry = 0;
            ringStart = ringNextEntry;
        }

        /// <summary>
        /// Goes back in the stream of text already read
        /// </summary>
        /// <param name="count">Number of characters to rewind</param>
        public void Rewind(int count)
        {
            ringStart -= count;
            if (ringStart < 0)
                ringStart += ring.Length;
        }

        private char ReadBuffer(out bool atEnd)
        {
            if (bufferStart == bufferSize)
            {
                bufferSize = reader.Read(buffer, 0, buffer.Length);
                bufferStart = 0;
                if (bufferSize == 0)
                {
                    atEnd = true;
                    return char.MinValue;
                }
            }
            atEnd = false;
            return buffer[bufferStart++];
        }

        /// <summary>
        /// Reads the next character
        /// </summary>
        /// <param name="atEnd">Out parameter set to true if no more character can be read</param>
        /// <returns>The read character</returns>
        public char Read(out bool atEnd)
        {
            if (!IsRingEmpty())
            {
                atEnd = false;
                return ReadRing();
            }
            char value = ReadBuffer(out atEnd);
            if (!atEnd)
                PushInRing(value);
            return value;
        }
    }
}
