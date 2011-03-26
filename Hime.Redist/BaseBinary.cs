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
        private int p_PositionBegin;
        private int p_PositionEnd;

        public int PositionBegin { get { return p_PositionBegin; } }
        public int PositionEnd { get { return p_PositionEnd; } }
        public int Length { get { return p_PositionEnd - p_PositionBegin + 1; } }

        public DataSegment(int begin, int end)
        {
            p_PositionBegin = begin;
            p_PositionEnd = end;
        }
        public bool Contains(int position) { return ((position >= p_PositionBegin) && (position <= p_PositionEnd)); }
        public bool Intersects(int position, int length)
        {
            if (Contains(position)) return true;
            if (Contains(position + length)) return true;
            if ((p_PositionBegin >= position) && (p_PositionBegin <= position + length)) return true;
            return false;
        }
        public int CompareTo(DataSegment segment) { return p_PositionBegin.CompareTo(segment.p_PositionBegin); }
    }

    public sealed class DataInput
    {
        private bool p_PerformCheckPosition;
        private bool p_PerformCheckUnread;
        private byte[] p_Data;
        private DataSegment p_DataSegment;
        private List<DataSegment> p_ReadSegments;
        private Stack<int> p_PreviousBegin;
        private Stack<int> p_PreviousEnd;
        private int p_CurrentBegin;
        private int p_CurrentPosition;

        public bool PerformCheckPosition
        {
            get { return p_PerformCheckPosition; }
            set { p_PerformCheckPosition = value; }
        }
        public bool PerformCheckUnread
        {
            get { return p_PerformCheckUnread; }
            set { p_PerformCheckUnread = value; }
        }
        public int Length { get { return p_DataSegment.Length; } }
        public int CurrentPosition { get { return p_CurrentPosition; } }
        public bool IsAtBegin { get { return (p_CurrentPosition == p_DataSegment.PositionBegin); } }
        public bool IsAtEnd { get { return (p_CurrentPosition == p_DataSegment.PositionEnd + 1); } }

        public DataInput(byte[] data)
        {
            p_PerformCheckPosition = true;
            p_Data = data;
            p_DataSegment = new DataSegment(0, data.Length - 1);
            p_ReadSegments = new List<DataSegment>();
            p_PreviousBegin = new Stack<int>();
            p_PreviousEnd = new Stack<int>();
        }
        public DataInput(byte[] data, int originOffset, int length)
        {
            p_PerformCheckPosition = true;
            p_Data = data;
            p_DataSegment = new DataSegment(originOffset, length - originOffset - 1);
            p_ReadSegments = new List<DataSegment>();
            p_PreviousBegin = new Stack<int>();
            p_PreviousEnd = new Stack<int>();
            p_CurrentBegin = originOffset;
            p_CurrentPosition = originOffset;
        }

        public DataInput Clone()
        {
            DataInput Clone = new DataInput(p_Data);
            Clone.p_PerformCheckPosition = p_PerformCheckPosition;
            Clone.p_PerformCheckUnread = p_PerformCheckUnread;
            Clone.p_DataSegment = p_DataSegment;
            Clone.p_CurrentBegin = p_CurrentBegin;
            Clone.p_CurrentPosition = p_CurrentPosition;

            return Clone;
        }

        public void Reset()
        {
            p_ReadSegments.Clear();
            p_PreviousBegin.Clear();
            p_PreviousEnd.Clear();
            p_CurrentBegin = p_DataSegment.PositionBegin;
            p_CurrentPosition = p_CurrentBegin;
        }

        public bool CanRead(int length)
        {
            // Check [in data]
            if (!p_DataSegment.Contains(p_CurrentPosition) || !p_DataSegment.Contains(p_CurrentPosition + length))
                return false;
            // Check [read segments]
            foreach (DataSegment Segment in p_ReadSegments)
            {
                if (Segment.Intersects(p_CurrentPosition, length))
                    return false;
            }
            return true;
        }
        private void CheckPosition_OnRead(int Length)
        {
            // Check [in data]
            if (!p_DataSegment.Contains(p_CurrentPosition) || !p_DataSegment.Contains(p_CurrentPosition + Length - 1))
            {
                string Begin = p_CurrentPosition.ToString("X", System.Globalization.NumberFormatInfo.InvariantInfo);
                string End = ((int)p_CurrentPosition + Length).ToString("X", System.Globalization.NumberFormatInfo.InvariantInfo);
                throw new BinaryException("Segment [0x" + Begin + ", 0x" + End + "] is not in the data segment.");
            }
            // Check [read segments]
            foreach (DataSegment Segment in p_ReadSegments)
            {
                if (Segment.Intersects(p_CurrentPosition, Length))
                {
                    string Begin = p_CurrentPosition.ToString("X", System.Globalization.NumberFormatInfo.InvariantInfo);
                    string End = ((int)p_CurrentPosition + Length).ToString("X", System.Globalization.NumberFormatInfo.InvariantInfo);
                    throw new BinaryException("Segment [0x" + Begin + ", 0x" + End + "] intersects with an already read segment.");
                }
            }
        }

        public System.Byte ReadByte()
        {
            if (p_PerformCheckPosition)
                CheckPosition_OnRead(1);
            return p_Data[p_CurrentPosition];
        }
        public System.Byte ReadAndAdvanceByte()
        {
            System.Byte CurrentData = ReadByte();
            p_CurrentPosition++;
            return CurrentData;
        }
        public System.UInt16 ReadUInt16()
        {
            if (p_PerformCheckPosition)
                CheckPosition_OnRead(2);
            System.UInt16 part8 = (System.UInt16)((System.UInt16)p_Data[p_CurrentPosition + 1] << 8);
            System.UInt16 part0 = (System.UInt16)p_Data[p_CurrentPosition];
            return (System.UInt16)(part8 | part0);
        }
        public System.UInt16 ReadAndAdvanceUInt16()
        {
            System.UInt16 CurrentData = ReadUInt16();
            p_CurrentPosition += 2;
            return CurrentData;
        }
        public System.UInt32 ReadUInt32()
        {
            if (p_PerformCheckPosition)
                CheckPosition_OnRead(4);
            System.UInt32 part24 = (System.UInt32)p_Data[p_CurrentPosition + 3] << 24;
            System.UInt32 part16 = (System.UInt32)p_Data[p_CurrentPosition + 2] << 16;
            System.UInt32 part8 = (System.UInt32)p_Data[p_CurrentPosition + 1] << 8;
            System.UInt32 part0 = (System.UInt32)p_Data[p_CurrentPosition];
            return part24 | part16 | part8 | part0;
        }
        public System.UInt32 ReadAndAdvanceUInt32()
        {
            System.UInt32 CurrentData = ReadUInt32();
            p_CurrentPosition += 4;
            return CurrentData;
        }
        public System.UInt64 ReadUInt64()
        {
            if (p_PerformCheckPosition)
                CheckPosition_OnRead(8);
            System.UInt64 part56 = (System.UInt64)p_Data[p_CurrentPosition + 7] << 56;
            System.UInt64 part48 = (System.UInt64)p_Data[p_CurrentPosition + 6] << 48;
            System.UInt64 part40 = (System.UInt64)p_Data[p_CurrentPosition + 5] << 40;
            System.UInt64 part32 = (System.UInt64)p_Data[p_CurrentPosition + 4] << 32;
            System.UInt64 part24 = (System.UInt64)p_Data[p_CurrentPosition + 3] << 24;
            System.UInt64 part16 = (System.UInt64)p_Data[p_CurrentPosition + 2] << 16;
            System.UInt64 part8 = (System.UInt64)p_Data[p_CurrentPosition + 1] << 8;
            System.UInt64 part0 = (System.UInt64)p_Data[p_CurrentPosition];
            return part56 | part48 | part40 | part32 | part24 | part16 | part8 | part0;
        }
        public System.UInt64 ReadAndAdvanceUInt64()
        {
            System.UInt64 CurrentData = ReadUInt64();
            p_CurrentPosition += 8;
            return CurrentData;
        }

        public void PositionShift(int offset)
        {
            p_PreviousBegin.Push(p_CurrentBegin);
            p_PreviousEnd.Push(p_CurrentPosition);
            p_CurrentBegin = p_CurrentPosition + offset;
            p_CurrentPosition = p_CurrentBegin;
        }
        public void PositionMoveTo(int position)
        {
            p_PreviousBegin.Push(p_CurrentBegin);
            p_PreviousEnd.Push(p_CurrentPosition);
            p_CurrentBegin = position;
            p_CurrentPosition = position;
        }
        public void PositionReturn()
        {
            p_ReadSegments.Add(new DataSegment(p_CurrentBegin, p_CurrentPosition - 1));
            p_CurrentBegin = p_PreviousBegin.Pop();
            p_CurrentPosition = p_PreviousEnd.Pop();
        }
    }
}
