using System.Runtime.InteropServices;

namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents a blob of binary data that can be accessed as an array of unsigned 32-bits integers
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    class BlobUInt
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private uint[] data;
        [FieldOffset(8)]
        private int count;

        /// <summary>
        /// Gets the raw array of bytes
        /// </summary>
        public byte[] Raw { get { return blob; } }
        /// <summary>
        /// Gets the number of unsigned 32-bits integer in this blob
        /// </summary>
        public int Count { get { return count; } }
        /// <summary>
        /// Gets the 32-bits unsigned integer at the given index
        /// </summary>
        /// <param name="index">Index of the integer</param>
        /// <returns>The 32-bits unsigned integer at the given index</returns>
        public uint this[int index]
        {
            get { return data[index]; }
        }

        /// <summary>
        /// Initializes a new blob with the given size in bytes
        /// </summary>
        /// <param name="count">The number of items</param>
        public BlobUInt(int count)
        {
            this.blob = new byte[count * 4];
            this.count = count;
        }
    }

    /// <summary>
    /// Represents a blob of binary data that can be accessed as an array of unsigned 16-bits integers
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    class BlobUShort
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private ushort[] data;
        [FieldOffset(8)]
        private int count;

        /// <summary>
        /// Gets the raw array of bytes
        /// </summary>
        public byte[] Raw { get { return blob; } }
        /// <summary>
        /// Gets the number of unsigned 16-bits integer in this blob
        /// </summary>
        public int Count { get { return count; } }
        /// <summary>
        /// Gets the 16-bits unsigned integer at the given index
        /// </summary>
        /// <param name="index">Index of the integer</param>
        /// <returns>The 16-bits unsigned integer at the given index</returns>
        public ushort this[int index]
        {
            get { return data[index]; }
        }

        /// <summary>
        /// Initializes a new blob with the given size in bytes
        /// </summary>
        /// <param name="count">The number of items</param>
        public BlobUShort(int count)
        {
            this.blob = new byte[count * 2];
            this.count = count;
        }
    }
}
