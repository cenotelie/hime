namespace Hime.Redist
{
    /// <summary>
    /// Represents a position in term of line and column in a text input
    /// </summary>
    public struct TextPosition
    {
        private int line;
        private int column;
        
        /// <summary>
        /// Gets the line number
        /// </summary>
        public int Line { get { return line; } }
        
        /// <summary>
        /// Gets the column number
        /// </summary>
        public int Column { get { return column; } }
        
        /// <summary>
        /// Initializes this position with the given line and column numbers
        /// </summary>
        /// <param name="line">The line number</param>
        /// <param name="column">The column number</param>
        public TextPosition(int line, int column)
        {
            this.line = line;
            this.column = column;
        }

        /// <summary>
        /// Gets a string representation of this position
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + line + ", " + column + ")";
        }
    }
}
