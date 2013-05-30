using System.Runtime.InteropServices;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a LRBytecode stored as a binary blob
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    class LRBytecode
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private LROpCode[] data;
        [FieldOffset(8)]
        private int length;

        /// <summary>
        /// Gets the raw array of bytes
        /// </summary>
        public byte[] Raw { get { return blob; } }
        /// <summary>
        /// Gets the length of the bytecode
        /// </summary>
        public int Length { get { return length; } }
        /// <summary>
        /// Gets the opcode at the given index
        /// </summary>
        /// <param name="index">Index of the opcode</param>
        /// <returns>The opcode at the given index</returns>
        public LROpCode this[int index]
        {
            get { return data[index]; }
        }

        /// <summary>
        /// Initializes a new blob of the given length
        /// </summary>
        /// <param name="length">The length of the bytecode</param>
        public LRBytecode(int length)
        {
            this.blob = new byte[length * 2];
            this.length = length;
        }
    }
}
