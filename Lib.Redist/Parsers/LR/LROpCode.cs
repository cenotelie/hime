using System.Runtime.InteropServices;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represent an opcode for a LR production
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct LROpCode
    {
        public const ushort MaskAction = 3;
        public const ushort PopNoAction = 0;
        public const ushort PopDrop = 2;
        public const ushort PopPromote = 3;
        public const ushort Virtual = 4;
        public const ushort VirtualNoAction = Virtual + PopNoAction;
        public const ushort VirtualDrop = Virtual + PopDrop;
        public const ushort VirtualPromote = Virtual + PopPromote;
        public const ushort SemanticAction = 8;
        public const ushort NullVariable = 16;
        public const ushort NullVariableNoAction = NullVariable;
        public const ushort NullVariableDrop = NullVariable + PopDrop;
        public const ushort NullVariablePromote = NullVariable + PopPromote;

        [FieldOffset(0)]
        private ushort code;

        /// <summary>
        /// Gets the code's value
        /// </summary>
        public ushort Value { get { return code; } }

        /// <summary>
        /// Gets the tree action included in this code
        /// </summary>
        public LRTreeAction TreeAction { get { return (LRTreeAction)(code & MaskAction); } }

        /// <summary>
        /// Gets whether is a Pop action
        /// </summary>
        public bool IsPop { get { return (code <= PopPromote); } }

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
