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
        public int Length { get { return sizeBlob / 4; } }
        public int this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public BlobInt(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    class BlobUInt
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private uint[] data;
        [FieldOffset(8)]
        private int sizeBlob;

        public byte[] Raw { get { return blob; } }
        public int RawSize { get { return sizeBlob; } }
        public int Length { get { return sizeBlob / 4; } }
        public uint this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public BlobUInt(int sizeBlob)
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
            set { data[index] = value; }
        }

        public BlobUShort(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
        }
    }
}
