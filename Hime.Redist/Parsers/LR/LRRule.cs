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
    /// Represents a LR(k) parser rule
    /// </summary>
    public sealed class LRRule
    {
        /// <summary>
        /// Callback to invoke when reducing this rule
        /// </summary>
        public LRParser.Production OnReduction;
        /// <summary>
        /// The rule's head variable
        /// </summary>
        public SymbolVariable Head;
        /// <summary>
        /// The rule's length
        /// </summary>
        public ushort Length;
        /// <summary>
        /// Initializes a new instance of the Rule structure with the given callback, variable and length
        /// </summary>
        /// <param name="prod">The callback for reductions</param>
        /// <param name="head">The head variable</param>
        /// <param name="length">The rule's length</param>
        public LRRule(LRParser.Production prod, SymbolVariable head, ushort length)
        {
            OnReduction = prod;
            Head = head;
            Length = length;
        }
    }
}