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
    internal class DataInput
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
		
        internal int Length { get { return dataSegment.Length; } }
        
        private bool IsAtBegin { get { return (currentPosition == dataSegment.PositionBegin); } }
        
		internal bool IsAtEnd { get { return (currentPosition == dataSegment.PositionEnd + 1); } }

        private DataInput(byte[] data) : this(data, 0, data.Length)
        {
        }
		
        private DataInput(byte[] data, int originOffset, int length)
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

        private DataInput Clone()
        {
            DataInput result = new DataInput(this.data);
            result.performCheckPosition = this.performCheckPosition;
            result.performCheckUnread = this.performCheckUnread;
            result.dataSegment = this.dataSegment;
            result.currentBegin = this.currentBegin;
            result.currentPosition = this.currentPosition;

            return result;
        }

        private void Reset()
        {
            readSegments.Clear();
            previousBegin.Clear();
            previousEnd.Clear();
            currentBegin = dataSegment.PositionBegin;
            currentPosition = currentBegin;
        }

        internal bool CanRead(int length)
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

        internal Byte ReadByte()
        {
            if (this.performCheckPosition)
                CheckPosition_OnRead(1);
            return data[currentPosition];
        }
		
        private UInt16 ReadUInt16()
        {
            if (this.performCheckPosition)
                CheckPosition_OnRead(2);
            UInt16 part8 = (UInt16)((UInt16)data[currentPosition + 1] << 8);
            UInt16 part0 = (UInt16)data[currentPosition];
            return (UInt16)(part8 | part0);
        }
		
        private UInt32 ReadUInt32()
        {
            if (this.performCheckPosition)
                CheckPosition_OnRead(4);
            UInt32 part24 = (UInt32)data[currentPosition + 3] << 24;
            UInt32 part16 = (UInt32)data[currentPosition + 2] << 16;
            UInt32 part8 = (UInt32)data[currentPosition + 1] << 8;
            UInt32 part0 = (UInt32)data[currentPosition];
            return part24 | part16 | part8 | part0;
        }
		
        private UInt64 ReadUInt64()
        {
			int size = sizeof(UInt64);
            if (this.performCheckPosition) CheckPosition_OnRead(size);
			UInt64 result = 0;
			for (int i = 0; i < size; i++)
			{
				UInt64 part = (UInt64)data[currentPosition + i] << (8*i);
				result = result | part;
			}
            return result;
        }
		
        internal Byte ReadAndAdvanceByte()
        {
            Byte CurrentData = ReadByte();
            currentPosition++;
            return CurrentData;
        }
						
        internal UInt16 ReadAndAdvanceUInt16()
        {
            UInt16 CurrentData = ReadUInt16();
            currentPosition += 2;
            return CurrentData;
        }

        internal UInt32 ReadAndAdvanceUInt32()
        {
            System.UInt32 CurrentData = ReadUInt32();
            currentPosition += 4;
            return CurrentData;
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
