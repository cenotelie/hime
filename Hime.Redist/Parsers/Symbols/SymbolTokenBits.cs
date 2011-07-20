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
        /// <param name="ClassName">Token's class name</param>
        /// <param name="ClassSID">Token's class ID</param>
        /// <param name="Value">Token binary value</param>
        public SymbolTokenBits(string ClassName, ushort ClassSID, byte Value) : base(ClassSID, ClassName) { value = Value; }
    }
}