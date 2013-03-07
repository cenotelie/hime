namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Actions values for LR parsers
    /// </summary>
    public static class LRActions
    {
        /// <summary>
        /// No possible action => Error
        /// </summary>
        public const ushort None = 0;
        /// <summary>
        /// Apply a reduction
        /// </summary>
        public const ushort Reduce = 1;
        /// <summary>
        /// Shift to another state
        /// </summary>
        public const ushort Shift = 2;
        /// <summary>
        /// Accept the input
        /// </summary>
        public const ushort Accept = 3;
    }
}
