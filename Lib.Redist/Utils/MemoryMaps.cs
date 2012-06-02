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
        public ushort[] data;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    internal class BlobInt
    {
        public BlobInt(byte[] bytes) { this.bytes = bytes; }
        [System.Runtime.InteropServices.FieldOffset(0)]
        private byte[] bytes;
        [System.Runtime.InteropServices.FieldOffset(0)]
        public int[] data;
    }
}
