namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Bytecode values for the LR productions
    /// </summary>
    public static class LRBytecode
    {
        /// <summary>
        /// Gets the next element in the stack and apply no action
        /// </summary>
        public const ushort PopNoAction = 0;
        /// <summary>
        /// Gets the next element in the stack and apply the drop action
        /// </summary>
        public const ushort PopDrop = 2;
        /// <summary>
        /// Gets the next element in the stack and apply the promote action
        /// </summary>
        public const ushort PopPromote = 3;

        /// <summary>
        /// Radical for bytecodes dealing with virtual symbols
        /// </summary>
        public const ushort Virtual = 4;
        /// <summary>
        /// Add a virtual symbol and apply no action
        /// </summary>
        public const ushort VirtualNoAction = Virtual + PopNoAction;
        /// <summary>
        /// Add a virtual symbol and apply the drop action
        /// </summary>
        public const ushort VirtualDrop = Virtual + PopDrop;
        /// <summary>
        /// Add a virtual symbol and apply the promote action
        /// </summary>
        public const ushort VirtualPromote = Virtual + PopPromote;

        /// <summary>
        /// Apply a semantic action
        /// </summary>
        public const ushort SemanticAction = 8;

        /// <summary>
        /// Radical for bytecodes dealing with nullable variables in a RNGLR parser
        /// </summary>
        public const ushort NullVariable = 16;
        /// <summary>
        /// Add a nullable variable and apply no action
        /// </summary>
        public const ushort NullVariableNoAction = NullVariable;
        /// <summary>
        /// Add a nullable variable and apply the drop action
        /// </summary>
        public const ushort NullVariableDrop = NullVariable + PopDrop;
        /// <summary>
        /// Add a nullable variable and apply the promote action
        /// </summary>
        public const ushort NullVariablePromote = NullVariable + PopPromote;

        /// <summary>
        /// Determines whether the given bytecode is a Pop action
        /// </summary>
        /// <param name="code">The bytecode</param>
        /// <returns>True if this is a Pop action</returns>
        public static bool IsPop(ushort code) { return (code <= PopPromote); }
        /// <summary>
        /// Determines whether the given bytecode is to add a virtual symbol
        /// </summary>
        /// <param name="code">The bytecode</param>
        /// <returns>True if this is a Add Virtual action</returns>
        public static bool IsAddVirtual(ushort code) { return ((code & Virtual) == Virtual); }
        /// <summary>
        /// Determines whether the given bytecode is a Semantic action
        /// </summary>
        /// <param name="code">The bytecode</param>
        /// <returns>True if this is a Semantic action</returns>
        public static bool IsSemAction(ushort code) { return (code == SemanticAction); }
        /// <summary>
        /// Determines whether the given bytecode is to add a null variable
        /// </summary>
        /// <param name="code">The bytecode</param>
        /// <returns>True if this is a Add Null Variable action</returns>
        public static bool IsAddNullVariable(ushort code) { return ((code & NullVariable) == NullVariable); }

        /// <summary>
        /// Gets the CST node action corresponding to the given bytecode
        /// </summary>
        /// <param name="code">The bytecode</param>
        /// <returns>The CST node action</returns>
        public static AST.CSTAction GetAction(ushort code) { return (AST.CSTAction)(code & 0x3); }
    }
}
