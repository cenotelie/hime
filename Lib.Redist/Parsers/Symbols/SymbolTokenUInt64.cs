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
    /// Represents an unsigned 64-bit integer matched by a lexer
    /// </summary>
    public sealed class SymbolTokenUInt64 : SymbolToken
    {
        private ulong value;
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override object Value { get { return value; } }
        /// <summary>
        /// Get the binary data represented by this token
        /// </summary>
        public ulong ValueUInt64 { get { return value; } }
        /// <summary>
        /// Initializes a new instance of the SymbolTokenUInt64 class
        /// </summary>
        /// <param name="sid">Token's class ID</param>
        /// <param name="name">Token's class name</param>
        /// <param name="value">Token binary value</param>
        public SymbolTokenUInt64(ushort sid, string name, ulong value) : base(sid, name) { this.value = value; }
    }
}