namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a piece of text matched by a lexer
    /// </summary>
    public sealed class TextToken : Token
    {
        private TextContent content;
        private int index;
        private int length;

        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override string Value
        {
            get
            {
                if (length == 0)
                    return string.Empty;
                return content.GetValue(index, length);
            }
        }
        /// <summary>
        /// Gets the position of this token in the input
        /// </summary>
        public override TextPosition Position { get { return content.GetPositionAt(index); } }
        /// <summary>
        /// Gets the length of the text in this token
        /// </summary>
        public override int Length { get { return length; } }

        /// <summary>
        /// Initializes a new instance of the TextToken class
        /// </summary>
        /// <param name="sid">ID of the terminal matched by this Token</param>
        /// <param name="name">Name of the terminal matched by this Token</param>
        /// <param name="content">The data content for this token</param>
        /// <param name="index">The token's index</param>
        /// <param name="length">The token's length</param>
        internal TextToken(int sid, string name, TextContent content, int index, int length)
            : base(sid, name)
        {
            this.content = content;
            this.index = index;
            this.length = length;
        }
    }
}