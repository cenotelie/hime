namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Represents a piece of text
    /// </summary>
    struct TextBuffer
    {
        private char[] data;    // The contained data
        private int start;      // The start index in the data
        private int end;        // The end index in the data

        /// <summary>
        /// Gets the character at the given index
        /// </summary>
        /// <param name="index">Index in the data</param>
        /// <returns>The corresponding character</returns>
        public char this[int index] { get { return data[index]; } }
        
        /// <summary>
        /// Gets the start index
        /// </summary>
        public int Start { get { return start; } }
        
        /// <summary>
        /// Gets the end index
        /// </summary>
        public int End { get { return end; } }
        
        /// <summary>
        /// Gets whether this buffer is empty
        /// </summary>
        public bool IsEmpty { get { return (end <= start); } }

        /// <summary>
        /// Initializes this buffer
        /// </summary>
        /// <param name="data">The underlyin data</param>
        /// <param name="start">The start index in the data</param>
        /// <param name="end">The end index in the data</param>
        public TextBuffer(char[] data, int start, int end)
        {
            this.data = data;
            this.start = start;
            this.end = end;
        }
    }
}
