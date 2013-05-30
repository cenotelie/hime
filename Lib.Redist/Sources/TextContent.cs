using System;
using System.Text;

namespace Hime.Redist
{
    /// <summary>
    /// Stores the content of the text read by a lexer
    /// </summary>
    class TextContent
    {
        // Actual text content
        private char[][] chunks;
        private int chunkIndex;
        // Start index of the line
        private int[] lines;
        private int line;
        // Line ending state
        private bool flagCR;

        /// <summary>
        /// Initializes the storage
        /// </summary>
        public TextContent()
        {
            this.chunks = new char[1024][];
            this.chunkIndex = 0;
            this.lines = new int[1024];
            this.line = 0;
        }

        /// <summary>
        /// Gets the substring beginning at the given index with the given length
        /// </summary>
        /// <param name="index">Index of the substring from the start</param>
        /// <param name="length">Length of the substring</param>
        /// <returns></returns>
        public string GetValue(int index, int length)
        {
            int chunck = index >> 10;  // index of the chunck
            int start = index & 0x3FF; // start index in the chunck

            if (start + length <= 1024)
            {
                // The substring is contained within only one chunck
                return new string(chunks[chunck], start, length);
            }

            // Now we need a string builder
            StringBuilder builder = new StringBuilder(length);
            // Finish the current chunck
            builder.Append(chunks[chunck], start, 1024 - start);
            int remaining = length - 1024 + start;
            chunck++;
            // While we can still add complete chuncks
            while (remaining > 1024)
            {
                builder.Append(chunks[chunck], 0, 1024);
                remaining -= 1024;
                chunck++;
            }
            // Add the last part and return
            builder.Append(chunks[chunck], 0, remaining);
            return builder.ToString();
        }

        /// <summary>
        /// Gets the line number at the given index
        /// </summary>
        /// <param name="index">Index from the start</param>
        /// <returns>The line number at the index</returns>
        public int GetLineAt(int index) { return Bisect(index) + 1; }

        /// <summary>
        /// Gets the column number at the given index
        /// </summary>
        /// <param name="index">Index from the start</param>
        /// <returns>The column number at the index</returns>
        public int GetColumnAt(int index) { return index - lines[Bisect(index)] + 1; }

        /// <summary>
        /// Gets the position at the given index
        /// </summary>
        /// <param name="index">Index from the start</param>
        /// <returns>The position (line and column) at the index</returns>
        public TextPosition GetPositionAt(int index)
        {
            int l = Bisect(index);
            return new TextPosition(l + 1, index - lines[l] + 1);
        }

        private int Bisect(int index)
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
            // Append the new chunck
            if (chunkIndex == chunks.Length - 1)
            {
                char[][] r = new char[chunks.Length + 1024][];
                Array.Copy(chunks, r, chunks.Length);
                chunks = r;
            }
            chunks[chunkIndex] = buffer;
            // Run the state-machine for line endings
            for (int i = 0; i != count; i++)
            {
                switch ((int)buffer[i])
                {
                    case 0x0D:
                        flagCR = true;
                        NextLine(i);
                        break;
                    case 0x0A:
                        if (!flagCR)
                            NextLine(i);
                        flagCR = false;
                        break;
                    case 0x0B:
                    case 0x0C:
                    case 0x85:
                    case 0x2028:
                    case 0x2029:
                        flagCR = false;
                        NextLine(i);
                        break;
                    default:
                        flagCR = false;
                        break;
                }
            }
            chunkIndex++;
        }

        private void NextLine(int sub)
        {
            int index = chunkIndex << 10 | sub;
            if (line == lines.Length - 1)
            {
                int[] t = new int[lines.Length + 1024];
                Buffer.BlockCopy(lines, 0, t, 0, lines.Length * 4);
                lines = t;
            }
            lines[++line] = index;
        }
    }
}
