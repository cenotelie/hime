using System.Runtime.InteropServices;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a LR action in a LR parse table
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct LRAction
    {
        [FieldOffset(0)]
        private LRActionCode code;
        [FieldOffset(2)]
        private ushort data;

        /// <summary>
        /// Gets the action code
        /// </summary>
        public LRActionCode Code { get { return code; } }
        /// <summary>
        /// Gets the data associated with the action
        /// </summary>
        /// <remarks>
        /// If the code is Reduce, it is the index of the LRProduction
        /// If the code is Shift, it is the index of the next state
        /// </remarks>
        public int Data { get { return data; } }
    }
}
