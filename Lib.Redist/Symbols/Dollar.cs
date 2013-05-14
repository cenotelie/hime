/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

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
    }
}