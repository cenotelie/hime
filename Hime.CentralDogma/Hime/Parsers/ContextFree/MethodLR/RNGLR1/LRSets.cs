using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represent a right nulled reduction action for a set of items
    /// </summary>
    class ItemSetActionRNReduce : ItemSetActionReduce
    {
        /// <summary>
        /// Length of the reduction
        /// </summary>
        protected int p_ReduceLength;

        /// <summary>
        /// Get the length of the reduction
        /// </summary>
        public int ReduceLength { get { return p_ReduceLength; } }

        /// <summary>
        /// Constructs the action
        /// </summary>
        /// <param name="OnSymbol">The lookahead</param>
        /// <param name="ToReduce">The rule to be reduced on lookahead</param>
        /// <param name="ReduceLength"></param>
        public ItemSetActionRNReduce(Terminal Lookahead, CFRule ToReduce, int ReduceLength) : base(Lookahead, ToReduce) { p_ReduceLength = ReduceLength; }
    }
}
