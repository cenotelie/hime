using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hime.Redist.Utils
{
    [StructLayout(LayoutKind.Explicit)]
    class BlobInt
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private int[] data;
        [FieldOffset(8)]
        private int sizeBlob;

        public byte[] Raw { get { return blob; } }
        public int RawSize { get { return sizeBlob; } }
        public int this[int index]
        {
            get { return data[index]; }
        }

        public BlobInt(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    class BlobUShort
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private ushort[] data;
        [FieldOffset(8)]
        private int sizeBlob;

        public byte[] Raw { get { return blob; } }
        public int RawSize { get { return sizeBlob; } }
        public int Length { get { return sizeBlob / 2; } }
        public ushort this[int index]
        {
            get { return data[index]; }
        }

        public BlobUShort(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
        }
    }
}
