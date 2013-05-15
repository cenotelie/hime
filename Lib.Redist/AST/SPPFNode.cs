using System.Collections.Generic;

namespace Hime.Redist.AST
{
    /// <summary>
    /// Represents a node in the raw SPPF (Shared Packed Parse Forest) created by a RNGLR parser
    /// Tree actions are directly executed on this structure, producing the final AST
    /// </summary>
    /// <remarks>
    /// The full SPPF is never produced because the only the first family of each SPPf node is ever treated.
    /// This is an optimization based on the fact that user only want an AST and the final AST is arbitrarily produced by the first families.
    /// </remarks>
    class SPPFNode : BuildNode
    {
        /// <summary>
        /// The original SID retained by this SPPF node (even though the final AST node may change due to tree actions)
        /// </summary>
        private ushort originalSID;
        /// <summary>
        /// Gets the original SID of this SPPF node
        /// </summary>
        public ushort SymbolID { get { return originalSID; } }
        
        /// <summary>
        /// Initializes this node with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol for this node</param>
        public SPPFNode(Symbols.Symbol symbol)
            : base(symbol)
        {
            this.originalSID = symbol.SymbolID;
        }
    }
}
