namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a special token for the end of a data stream
    /// </summary>
    public sealed class Dollar : Token
    {
        private static Dollar instance = new Dollar();
        private Dollar() : base(2, "$") { }
        /// <summary>
        /// Gets the dollar token
        /// </summary>
        public static Dollar Instance { get { return instance; } }
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