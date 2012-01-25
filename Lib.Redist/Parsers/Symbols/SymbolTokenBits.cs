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
    /// Represents a piece of binary data matched by a lexer
    /// </summary>
    public sealed class SymbolTokenBits : SymbolToken
    {
        private byte value;
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override object Value { get { return value; } }
        /// <summary>
        /// Get the binary data represented by this token
        /// </summary>
        public byte ValueBits { get { return value; } }
        /// <summary>
        /// Initializes a new instance of the SymbolTokenBits class
        /// </summary>
        /// <param name="sid">Token's unique type identifier</param>
        /// <param name="name">Token's type name</param>
        /// <param name="value">Token binary value</param>
        public SymbolTokenBits(ushort sid, string name, byte value) : base(sid, name) { this.value = value; }
    }
}