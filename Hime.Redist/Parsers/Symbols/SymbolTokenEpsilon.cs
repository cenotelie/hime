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
    /// Represents a special token for the absence of data in a stream
    /// </summary>
    sealed class SymbolTokenEpsilon : SymbolToken
    {
        private static SymbolTokenEpsilon instance = new SymbolTokenEpsilon();
        private SymbolTokenEpsilon() : base(1, "ε") { }
        /// <summary>
        /// Gets the epsilon token
        /// </summary>
        public static SymbolTokenEpsilon Instance { get { return instance; } }
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override object Value { get { return string.Empty; } }
    }
}