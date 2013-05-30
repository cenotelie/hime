namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a special token for the absence of data in a stream
    /// </summary>
    public sealed class Epsilon : Token
    {
        private static Epsilon instance = new Epsilon();
        private Epsilon() : base(1, "ε") { }
        /// <summary>
        /// Gets the epsilon token
        /// </summary>
        public static Epsilon Instance { get { return instance; } }
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override string Value { get { return string.Empty; } }
        /// <summary>
        /// Gets the position of this token in the input
        /// </summary>
        public override TextPosition Position { get { return new TextPosition(); } }
        /// <summary>
        /// Gets the length of the text in this token
        /// </summary>
        public override int Length { get { return 0; } }
    }
}