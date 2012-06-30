using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents the label on a GSS Edge
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public sealed class GSSLabel
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        private SPPFNode node;
        [System.Runtime.InteropServices.FieldOffset(0)]
        private Symbol symbol;

        public SPPFNode Node { get { return node; } }
        public Symbol Symbol { get { return symbol; } }

        public GSSLabel(SPPFNode node) { this.node = node; }
        public GSSLabel(Symbol symbol) { this.symbol = symbol; }
    }
}
