using System.Collections.Generic;

namespace Hime.Redist.Binary
{
    public sealed class BinaryException : System.Exception
    {
        public BinaryException() : base() { }
        public BinaryException(string message) : base(message) { }
        public BinaryException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    public sealed class DataSegment
    {
        private int positionBegin;
        private int positionEnd;

        public int PositionBegin { get { return positionBegin; } }
        public int PositionEnd { get { return positionEnd; } }
        public int Length { get { return positionEnd - positionBegin + 1; } }

        public DataSegment(int begin, int end)
        {
            positionBegin = begin;
            positionEnd = end;
        }
        public bool Contains(int position) { return ((position >= positionBegin) && (position <= positionEnd)); }
        public bool Intersects(int position, int length)
        {
            if (Contains(position)) return true;
            if (Contains(position + length)) return true;
            if ((positionBegin >= position) && (positionBegin <= position + length)) return true;
            return false;
        }
        public int CompareTo(DataSegment segment) { return positionBegin.CompareTo(segment.positionBegin); }
    }

    public sealed class DataInput
    {
        private bool performCheckPosition;
        private bool performCheckUnread;
        private byte[] data;
        private DataSegment dataSegment;
        private List<DataSegment> readSegments;
        private Stack<int> previousBegin;
        private Stack<int> previousEnd;
        private int currentBegin;
        private int currentPosition;

        public bool PerformCheckPosition
        {
            get { return performCheckPosition; }
            set { performCheckPosition = value; }
        }
        public bool PerformCheckUnread
        {
            get { return performCheckUnread; }
            set { performCheckUnread = value; }
        }
        public int Length { get { return dataSegment.Length; } }
        public int CurrentPosition { get { return currentPosition; } }
        public bool IsAtBegin { get { return (currentPosition == dataSegment.PositionBegin); } }
        public bool IsAtEnd { get { return (currentPosition == dataSegment.PositionEnd + 1); } }

        public DataInput(byte[] data)
        {
            this.performCheckPosition = true;
            this.data = data;
            this.dataSegment = new DataSegment(0, data.Length - 1);
            this.readSegments = new List<DataSegment>();
            this.previousBegin = new Stack<int>();
            this.previousEnd = new Stack<int>();
        }
        public DataInput(byte[] data, int originOffset, int length)
        {
            this.performCheckPosition = true;
            this.data = data;
            this.dataSegment = new DataSegment(originOffset, length - originOffset - 1);
            this.readSegments = new List<DataSegment>();
            this.previousBegin = new Stack<int>();
            this.previousEnd = new Stack<int>();
            this.currentBegin = originOffset;
            this.currentPosition = originOffset;
        }

        public DataInput Clone()
        {
            DataInput Clone = new DataInput(data);
            Clone.performCheckPosition = performCheckPosition;
            Clone.performCheckUnread = performCheckUnread;
            Clone.dataSegment = dataSegment;
            Clone.currentBegin = currentBegin;
            Clone.currentPosition = currentPosition;

            return Clone;
        }

        public void Reset()
        {
            readSegments.Clear();
            previousBegin.Clear();
            previousEnd.Clear();
            currentBegin = dataSegment.PositionBegin;
            currentPosition = currentBegin;
        }

        public bool CanRead(int length)
        {
            // Check [in data]
            if (!dataSegment.Contains(currentPosition) || !dataSegment.Contains(currentPosition + length))
                return false;
            // Check [read segments]
            foreach (DataSegment Segment in readSegments)
            {
                if (Segment.Intersects(currentPosition, length))
                    return false;
            }
            return true;
        }
        private void CheckPosition_OnRead(int Length)
        {
            // Check [in data]
            if (!dataSegment.Contains(currentPosition) || !dataSegment.Contains(currentPosition + Length - 1))
            {
                string Begin = currentPosition.ToString("X", System.Globalization.NumberFormatInfo.InvariantInfo);
                string End = ((int)currentPosition + Length).ToString("X", System.Globalization.NumberFormatInfo.InvariantInfo);
                throw new BinaryException("Segment [0x" + Begin + ", 0x" + End + "] is not in the data segment.");
            }
            // Check [read segments]
            foreach (DataSegment Segment in readSegments)
            {
                if (Segment.Intersects(currentPosition, Length))
                {
                    string Begin = currentPosition.ToString("X", System.Globalization.NumberFormatInfo.InvariantInfo);
                    string End = ((int)currentPosition + Length).ToString("X", System.Globalization.NumberFormatInfo.InvariantInfo);
                    throw new BinaryException("Segment [0x" + Begin + ", 0x" + End + "] intersects with an already read segment.");
                }
            }
        }

        public System.Byte ReadByte()
        {
            if (performCheckPosition)
                CheckPosition_OnRead(1);
            return data[currentPosition];
        }
        public System.Byte ReadAndAdvanceByte()
        {
            System.Byte CurrentData = ReadByte();
            currentPosition++;
            return CurrentData;
        }
        public System.UInt16 ReadUInt16()
        {
            if (performCheckPosition)
                CheckPosition_OnRead(2);
            System.UInt16 part8 = (System.UInt16)((System.UInt16)data[currentPosition + 1] << 8);
            System.UInt16 part0 = (System.UInt16)data[currentPosition];
            return (System.UInt16)(part8 | part0);
        }
        public System.UInt16 ReadAndAdvanceUInt16()
        {
            System.UInt16 CurrentData = ReadUInt16();
            currentPosition += 2;
            return CurrentData;
        }
        public System.UInt32 ReadUInt32()
        {
            if (performCheckPosition)
                CheckPosition_OnRead(4);
            System.UInt32 part24 = (System.UInt32)data[currentPosition + 3] << 24;
            System.UInt32 part16 = (System.UInt32)data[currentPosition + 2] << 16;
            System.UInt32 part8 = (System.UInt32)data[currentPosition + 1] << 8;
            System.UInt32 part0 = (System.UInt32)data[currentPosition];
            return part24 | part16 | part8 | part0;
        }
        public System.UInt32 ReadAndAdvanceUInt32()
        {
            System.UInt32 CurrentData = ReadUInt32();
            currentPosition += 4;
            return CurrentData;
        }
        public System.UInt64 ReadUInt64()
        {
            if (performCheckPosition)
                CheckPosition_OnRead(8);
            System.UInt64 part56 = (System.UInt64)data[currentPosition + 7] << 56;
            System.UInt64 part48 = (System.UInt64)data[currentPosition + 6] << 48;
            System.UInt64 part40 = (System.UInt64)data[currentPosition + 5] << 40;
            System.UInt64 part32 = (System.UInt64)data[currentPosition + 4] << 32;
            System.UInt64 part24 = (System.UInt64)data[currentPosition + 3] << 24;
            System.UInt64 part16 = (System.UInt64)data[currentPosition + 2] << 16;
            System.UInt64 part8 = (System.UInt64)data[currentPosition + 1] << 8;
            System.UInt64 part0 = (System.UInt64)data[currentPosition];
            return part56 | part48 | part40 | part32 | part24 | part16 | part8 | part0;
        }
        public System.UInt64 ReadAndAdvanceUInt64()
        {
            System.UInt64 CurrentData = ReadUInt64();
            currentPosition += 8;
            return CurrentData;
        }

        public void PositionShift(int offset)
        {
            previousBegin.Push(currentBegin);
            previousEnd.Push(currentPosition);
            currentBegin = currentPosition + offset;
            currentPosition = currentBegin;
        }
        public void PositionMoveTo(int position)
        {
            previousBegin.Push(currentBegin);
            previousEnd.Push(currentPosition);
            currentBegin = position;
            currentPosition = position;
        }
        public void PositionReturn()
        {
            readSegments.Add(new DataSegment(currentBegin, currentPosition - 1));
            currentBegin = previousBegin.Pop();
            currentPosition = previousEnd.Pop();
        }
    }
}
