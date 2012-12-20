using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hime.Redist.Utils
{
    interface BinaryBlob<T> : IEnumerable<T>
    {
        byte[] Blob { get; }
        int SizeBlob { get; }
        int SizeData { get; }
        T this[int index] { get; set; }
        void Clear();
    }

    [StructLayout(LayoutKind.Explicit)]
    class BinaryBlobUInt : BinaryBlob<uint>
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private uint[] data;
        [FieldOffset(8)]
        private int sizeBlob;

        public byte[] Blob { get { return blob; } }
        public int SizeBlob { get { return sizeBlob; } }
        public int SizeData { get { return sizeBlob / 4; } }
        public uint this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public BinaryBlobUInt(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
        }
        public void Clear() { Array.Clear(data, 0, sizeBlob / 4); }

        public IEnumerator<uint> GetEnumerator() { return (IEnumerator<uint>)data.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return data.GetEnumerator(); }
    }

    [StructLayout(LayoutKind.Explicit)]
    class BinaryBlobUShort : BinaryBlob<ushort>
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private ushort[] data;
        [FieldOffset(8)]
        private int sizeBlob;

        public byte[] Blob { get { return blob; } }
        public int SizeBlob { get { return sizeBlob; } }
        public int SizeData { get { return sizeBlob / 2; } }
        public ushort this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public BinaryBlobUShort(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
        }
        public void Clear() { Array.Clear(data, 0, sizeBlob / 2); }

        public IEnumerator<ushort> GetEnumerator() { return (IEnumerator<ushort>)data.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return data.GetEnumerator(); }
    }

    class BinaryBlobByte : BinaryBlob<byte>
    {
        private byte[] blob;
        private int sizeBlob;

        public byte[] Blob { get { return blob; } }
        public int SizeBlob { get { return sizeBlob; } }
        public int SizeData { get { return sizeBlob; } }
        public byte this[int index]
        {
            get { return blob[index]; }
            set { blob[index] = value; }
        }

        public BinaryBlobByte(int sizeBlob)
        {
            this.blob = new byte[sizeBlob];
            this.sizeBlob = sizeBlob;
        }
        public void Clear() { Array.Clear(blob, 0, sizeBlob); }

        public IEnumerator<byte> GetEnumerator() { return (IEnumerator<byte>)blob.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return blob.GetEnumerator(); }
    }
}
