using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Utils
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    internal class BlobUShort
    {
        public BlobUShort(byte[] bytes) { this.bytes = bytes; }
        [System.Runtime.InteropServices.FieldOffset(0)]
        private byte[] bytes;
        [System.Runtime.InteropServices.FieldOffset(0)]
        private ushort[] data;
        public ushort[] Data { get { return data; } }
        public ushort this[int index] { get { return data[index]; } }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    internal class BlobInt
    {
        public BlobInt(byte[] bytes) { this.bytes = bytes; }
        [System.Runtime.InteropServices.FieldOffset(0)]
        private byte[] bytes;
        [System.Runtime.InteropServices.FieldOffset(0)]
        private int[] data;
        public int[] Data { get { return data; } }
        public int this[int index] { get { return data[index]; } }
    }
}
