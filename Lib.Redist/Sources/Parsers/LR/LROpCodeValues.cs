using System;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Enumeration of the LR op codes
    /// </summary>
    [CLSCompliant(false)]
    public enum LROpCodeValues : ushort
    {
        /// <summary>
        /// Pop an AST from the stack without applying any tree action
        /// </summary>
        PopNoAction = 0,
        /// <summary>
        /// Pop an AST from the stack and apply the drop tree action
        /// </summary>
        PopDrop = 2,
        /// <summary>
        /// Pop an AST from the stack and apply the promote tree action
        /// </summary>
        PopPromote = 3,

        /// <summary>
        /// Add a virtual symbol without tree action
        /// </summary>
        VirtualNoAction = 4 + PopNoAction,
        /// <summary>
        /// Add a virtual symbol and apply the drop tree action
        /// </summary>
        /// <remarks>
        /// This doesn't make any sense, but it is possible!
        /// </remarks>
        VirtualDrop = 4 + PopDrop,
        /// <summary>
        /// Add a virtual symbol and apply the promote tree action
        /// </summary>
        VirtualPromote = 4 + PopPromote,
        
        /// <summary>
        /// Execute a semantic action
        /// </summary>
        SemanticAction = 8,

        /// <summary>
        /// Add a null variable without any tree action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
        NullVariableNoAction = 16,
        /// <summary>
        /// Add a null variable and apply the drop tree action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
        NullVariableDrop = 16 + PopDrop,
        /// <summary>
        /// Add a null variable and apply the promote action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
        NullVariablePromote = 16 + PopPromote
    }
}
