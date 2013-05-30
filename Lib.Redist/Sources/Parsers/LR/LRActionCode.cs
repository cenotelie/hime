using System;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an action in a LR parser
    /// </summary>
    [CLSCompliant(false)]
    public enum LRActionCode : ushort
    {
        /// <summary>
        /// No possible action => Error
        /// </summary>
        None = 0,
        /// <summary>
        /// Apply a reduction
        /// </summary>
        Reduce = 1,
        /// <summary>
        /// Shift to another state
        /// </summary>
        Shift = 2,
        /// <summary>
        /// Accept the input
        /// </summary>
        Accept = 3
    }
}
