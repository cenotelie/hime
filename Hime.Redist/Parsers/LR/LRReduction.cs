/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a rule reduction in a LR(1) parser
    /// </summary>
    public struct LRReduction
    {
        /// <summary>
        /// ID of the lookahead on which the reduction is triggered
        /// </summary>
        public ushort lookahead;
        /// <summary>
        /// Rule for the reduction
        /// </summary>
        public LRRule toReduce;
        /// <summary>
        /// Initializes a new instance of the Reduction structure with the given lookahead and rule
        /// </summary>
        /// <param name="lookahead">The reduction's lookahead ID</param>
        /// <param name="rule">The rule for the reduction</param>
        public LRReduction(ushort lookahead, LRRule rule)
        {
            this.lookahead = lookahead;
            this.toReduce = rule;
        }
    }
}