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
    /// Represents a special token for the end of data stream
    /// </summary>
    sealed class SymbolTokenDollar : SymbolToken
    {
        private static SymbolTokenDollar instance = new SymbolTokenDollar();
        private SymbolTokenDollar() : base(2, "EOF") { }
        /// <summary>
        /// Gets the dollar token
        /// </summary>
        public static SymbolTokenDollar Instance { get { return instance; } }
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override object Value { get { return "EOF"; } }
    }
}