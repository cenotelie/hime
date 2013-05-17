using System.Runtime.InteropServices;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represent an opcode for a LR production
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    struct LROpCode
    {
        private const ushort MaskAction = 3;
        private const ushort Virtual = 4;
        private const ushort SemanticAction = 8;
        private const ushort NullVariable = 16;
        
        [FieldOffset(0)]
        private ushort code;

        /// <summary>
        /// Gets the code's value
        /// </summary>
        public int Value { get { return code; } }

        /// <summary>
        /// Gets the tree action included in this code
        /// </summary>
        public LRTreeAction TreeAction { get { return (LRTreeAction)(code & MaskAction); } }

        /// <summary>
        /// Gets whether is a Pop action
        /// </summary>
        public bool IsPop { get { return (code < Virtual); } }

        /// <summary>
        /// Gets whether this is a Add Virtual action
        /// </summary>
        public bool IsAddVirtual { get { return ((code & Virtual) == Virtual); } }

        /// <summary>
        /// Gets whether this is a Semantic Action
        /// </summary>
        public bool IsSemAction { get { return (code == SemanticAction); } }

        /// <summary>
        /// Gets whether this is a Add Null Variable action
        /// </summary>
        public bool IsAddNullVar { get { return ((code & NullVariable) == NullVariable); } }
    }
}
