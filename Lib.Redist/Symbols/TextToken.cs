namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a piece of text matched by a lexer
    /// </summary>
    public sealed class TextToken : Token
    {
        private string value;
        private int line;
        private int column;

        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override string Value { get { return value; } }
        /// <summary>
        /// Gets the line number where the text begins
        /// </summary>
        public int Line { get { return line; } }
        /// <summary>
        /// Gets the column number where the text begins
        /// </summary>
        public int Column { get { return column; } }

        /// <summary>
        /// Initializes a new instance of the TextToken class with the given name, id, value and line number
        /// </summary>
        /// <param name="sid">ID of the terminal matched by this Token</param>
        /// <param name="name">Name of the terminal matched by this Token</param>
        /// <param name="value">Token's value</param>
        /// <param name="line">Token's line number</param>
        /// <param name="column">Token's starting column in the line</param>
        public TextToken(int sid, string name, string value, int line, int column)
            : base(sid, name)
        {
            this.value = value;
            this.line = line;
            this.column = column;
        }
    }
}