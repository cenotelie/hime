using System.Runtime.InteropServices;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represent an opcode for a LR production
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct LROpCode
    {
        private const ushort MaskAction = 3;
        
        /// <summary>
        /// Pop an AST from the stack without applying any tree action
        /// </summary>
        public const ushort PopNoAction = 0;
        /// <summary>
        /// Pop an AST from the stack and apply the drop tree action
        /// </summary>
        public const ushort PopDrop = 2;
        /// <summary>
        /// Pop an AST from the stack and apply the promote tree action
        /// </summary>
        public const ushort PopPromote = 3;
        
        private const ushort Virtual = 4;
        /// <summary>
        /// Add a virtual symbol without tree action
        /// </summary>
        public const ushort VirtualNoAction = Virtual + PopNoAction;
        /// <summary>
        /// Add a virtual symbol and apply the drop tree action
        /// </summary>
        /// <remarks>
        /// This doesn't make any sense, but it is possible!
        /// </remarks>
        public const ushort VirtualDrop = Virtual + PopDrop;
        /// <summary>
        /// Add a virtual symbol and apply the promote tree action
        /// </summary>
        public const ushort VirtualPromote = Virtual + PopPromote;
        /// <summary>
        /// Execute a semantic action
        /// </summary>
        public const ushort SemanticAction = 8;
        
        private const ushort NullVariable = 16;
        /// <summary>
        /// Add a null variable without any tree action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
        public const ushort NullVariableNoAction = NullVariable;
        /// <summary>
        /// Add a null variable and apply the drop tree action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
        public const ushort NullVariableDrop = NullVariable + PopDrop;
        /// <summary>
        /// Add a null variable and apply the promote action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
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
