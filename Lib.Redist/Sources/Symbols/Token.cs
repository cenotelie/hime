namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a piece of data matched by a lexer
    /// </summary>
    public abstract class Token : Symbol
    {
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public abstract string Value { get; }
        /// <summary>
        /// Gets the position of this token in the input
        /// </summary>
        public abstract TextPosition Position { get; }
        /// <summary>
        /// Gets the length of the text in this token
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// Initializes a new instance of the Token class with the given ID and name
        /// </summary>
        /// <param name="sid">ID of the terminal matched by this Token</param>
        /// <param name="name">Name of the terminal matched by this Token</param>
        protected Token(int sid, string name): base(sid, name) { }
    }
}