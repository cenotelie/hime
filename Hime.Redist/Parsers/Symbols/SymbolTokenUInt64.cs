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
        /// <param name="ClassName">Token's class name</param>
        /// <param name="ClassSID">Token's class ID</param>
        /// <param name="Value">Token binary value</param>
        public SymbolTokenUInt64(string ClassName, ushort ClassSID, ulong Value) : base(ClassSID, ClassName) { value = Value; }
    }
}