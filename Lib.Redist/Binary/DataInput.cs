/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System;
using System.Collections.Generic;

namespace Hime.Redist.Binary
{
    public class DataInput
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
		
        public bool PerformCheckUnread
        {
            get { return performCheckUnread; }
            set { performCheckUnread = value; }
        }
		
        public int Length { get { return dataSegment.Length; } }
        public int CurrentPosition { get { return currentPosition; } }
        public bool IsAtBegin { get { return (currentPosition == dataSegment.PositionBegin); } }
        public bool IsAtEnd { get { return (currentPosition == dataSegment.PositionEnd + 1); } }

        public DataInput(byte[] data) : this(data, 0, data.Length)
        {
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
            DataInput result = new DataInput(this.data);
            result.performCheckPosition = this.performCheckPosition;
            result.performCheckUnread = this.performCheckUnread;
            result.dataSegment = this.dataSegment;
            result.currentBegin = this.currentBegin;
            result.currentPosition = this.currentPosition;

            return result;
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
            if (this.performCheckPosition)
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
            if (this.performCheckPosition)
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
            if (this.performCheckPosition)
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
		
        private UInt64 ReadUInt64()
        {
            if (this.performCheckPosition) CheckPosition_OnRead(8);
            UInt64 part56 = (UInt64)data[currentPosition + 7] << (8*7);
			UInt64 result = part56;
            UInt64 part48 = (UInt64)data[currentPosition + 6] << (8*6);
			result = result | part48;
            UInt64 part40 = (UInt64)data[currentPosition + 5] << (8*5);
			result = result | part40;
            UInt64 part32 = (UInt64)data[currentPosition + 4] << (8*4);
			result = result | part32;
            UInt64 part24 = (UInt64)data[currentPosition + 3] << (8*3);
			result = result | part24;
            UInt64 part16 = (UInt64)data[currentPosition + 2] << (8*2);
			result = result | part16;
            UInt64 part8 = (UInt64)data[currentPosition + 1] << 8;
			result = result | part8;
            UInt64 part0 = (UInt64)data[currentPosition];
			result = result | part0;
            return result;
        }
		
        internal UInt64 ReadAndAdvanceUInt64()
        {
            UInt64 CurrentData = ReadUInt64();
            currentPosition += 8;
            return CurrentData;
        }

        private void PositionShift(int offset)
        {
            previousBegin.Push(currentBegin);
            previousEnd.Push(currentPosition);
            currentBegin = currentPosition + offset;
            currentPosition = currentBegin;
        }
		
        private void PositionMoveTo(int position)
        {
            previousBegin.Push(currentBegin);
            previousEnd.Push(currentPosition);
            currentBegin = position;
            currentPosition = position;
        }
		// TODO: this method is dead code? remove?
        private void PositionReturn()
        {
            readSegments.Add(new DataSegment(currentBegin, currentPosition - 1));
            currentBegin = previousBegin.Pop();
            currentPosition = previousEnd.Pop();
        }
    }
}
