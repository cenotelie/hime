using System.Runtime.InteropServices;

namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents a blob of binary data that can be accessed as an array of signed 32-bits integers
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    class BlobInt
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private int[] data;
        [FieldOffset(8)]
        private int sizeBlob;

        /// <summary>
        /// Gets the raw array of bytes
        /// </summary>
        public byte[] Raw { get { return blob; } }
        /// <summary>
        /// Gets the size of the binary blob in bytes
        /// </summary>
        public int RawSize { get { return sizeBlob; } }
        /// <summary>
        /// Gets the 32-bits signed integer at the given index
        /// </summary>
        /// <param name="index">Index of the integer</param>
        /// <returns>The 32-bits signed integer at the given index</returns>
        public int this[int index]
        {
            get { return data[index]; }
        }

        /// <summary>
        /// Initializes a new blob with the given size in bytes
        /// </summary>
        /// <param name="sizeBlob">The size of the blob in bytes</param>
        public BlobInt(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
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
        private int sizeBlob;

        /// <summary>
        /// Gets the raw array of bytes
        /// </summary>
        public byte[] Raw { get { return blob; } }
        /// <summary>
        /// Gets the size of the binary blob in bytes
        /// </summary>
        public int RawSize { get { return sizeBlob; } }
        /// <summary>
        /// Gets the number of unsigned 16-bits integer in this blob
        /// </summary>
        public int Length { get { return sizeBlob / 2; } }
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
        /// <param name="sizeBlob">The size of the blob in bytes</param>
        public BlobUShort(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
        }
    }
}
