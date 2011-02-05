namespace Hime.Kernel.Binary
{
    /// <summary>
    /// Represents an error in a binary access operation
    /// </summary>
    public sealed class BinaryException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the ResourceException class
        /// </summary>
        public BinaryException() : base() { }
        /// <summary>
        /// Initializes a new instance of the ResourceException class with an error message
        /// </summary>
        /// <param name="message">Message describing the error</param>
        public BinaryException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the ResourceExcetion class with an error message and a reference to the inner exception that causes the error
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="innerException">Inner exception causing the error</param>
        public BinaryException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Represents a binary data segment with begin and end positions
    /// </summary>
    public sealed class DataSegment
    {
        /// <summary>
        /// Segment beginning position
        /// </summary>
        private int p_PositionBegin;
        /// <summary>
        /// Segment ending position
        /// </summary>
        private int p_PositionEnd;

        /// <summary>
        /// Get the segment beginning position
        /// </summary>
        /// <value>The segment beginning position</value>
        public int PositionBegin { get { return p_PositionBegin; } }
        /// <summary>
        /// Get the segment ending position
        /// </summary>
        /// <value>The segment ending position</value>
        public int PositionEnd { get { return p_PositionEnd; } }
        /// <summary>
        /// Get the segment length
        /// </summary>
        /// <value>The segment's length</value>
        public int Length { get { return p_PositionEnd - p_PositionBegin + 1; } }

        /// <summary>
        /// Initializes a new instance of the DataSegment class with a beginning and end position
        /// </summary>
        /// <param name="begin">Beginning position</param>
        /// <param name="end">Ending position</param>
        public DataSegment(int begin, int end)
        {
            p_PositionBegin = begin;
            p_PositionEnd = end;
        }
        /// <summary>
        /// Determines if the given position in within the current segment
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>Returns true if position is within the segment, false otherwise</returns>
        public bool Contains(int position) { return ((position >= p_PositionBegin) && (position <= p_PositionEnd)); }
        /// <summary>
        /// Determines if two segments intersects
        /// </summary>
        /// <param name="position">Segment position</param>
        /// <param name="length">Segment length</param>
        /// <returns>Returns true if the two segments intersect, false otherwise</returns>
        public bool Intersects(int position, int length)
        {
            if (Contains(position)) return true;
            if (Contains(position + length)) return true;
            if ((p_PositionBegin >= position) && (p_PositionBegin <= position + length)) return true;
            return false;
        }
        /// <summary>
        /// Compare two segments
        /// </summary>
        /// <param name="segment">The segment to compare with</param>
        /// <returns>Returns the segment comparison based on the beginning position</returns>
        public int CompareTo(DataSegment segment) { return p_PositionBegin.CompareTo(segment.p_PositionBegin); }
    }

    /// <summary>
    /// Represents a binary data input
    /// </summary>
    public sealed class DataInput
    {
        /// <summary>
        /// Perform a check on the position on each read  if at true
        /// </summary>
        private bool p_PerformCheckPosition;
        /// <summary>
        /// Perform a check for unread data at end if at true
        /// </summary>
        private bool p_PerformCheckUnread;
        /// <summary>
        /// Binary data
        /// </summary>
        private byte[] p_Data;
        /// <summary>
        /// Global data segment (all available data)
        /// </summary>
        private DataSegment p_DataSegment;
        /// <summary>
        /// List of the already read data segments
        /// </summary>
        private System.Collections.Generic.List<DataSegment> p_ReadSegments;
        /// <summary>
        /// Stack containing beginning position of the segments currently read
        /// </summary>
        private System.Collections.Generic.Stack<int> p_PreviousBegin;
        /// <summary>
        /// Stack containing ending position of the segments currently read
        /// </summary>
        private System.Collections.Generic.Stack<int> p_PreviousEnd;
        /// <summary>
        /// Beginning position of the segment currently read
        /// </summary>
        private int p_CurrentBegin;
        /// <summary>
        /// Ending position of the segment currently read
        /// </summary>
        private int p_CurrentPosition;

        /// <summary>
        /// Get or Set a value indicating if a chack is performed on the position each time data are read
        /// </summary>
        /// <value>True if the check is performed, false otherwise</value>
        public bool PerformCheckPosition
        {
            get { return p_PerformCheckPosition; }
            set { p_PerformCheckPosition = value; }
        }
        /// <summary>
        /// Get or Set a value indicating if a check for unread data is performed at end
        /// </summary>
        /// <value>True if the check is performed, false otherwise</value>
        public bool PerformCheckUnread
        {
            get { return p_PerformCheckUnread; }
            set { p_PerformCheckUnread = value; }
        }
        /// <summary>
        /// Get the total length of the data
        /// </summary>
        /// <value>Total length of the data</value>
        public int Length { get { return p_DataSegment.Length; } }
        /// <summary>
        /// Get the current position
        /// </summary>
        /// <value>Current position in the data</value>
        public int CurrentPosition { get { return p_CurrentPosition; } }
        /// <summary>
        /// Get a value indicating if is at beginning of the data
        /// </summary>
        /// <value>True if at beginning of the data</value>
        public bool IsAtBegin { get { return (p_CurrentPosition == p_DataSegment.PositionBegin); } }
        /// <summary>
        /// Get a value indicating if at end of the data
        /// </summary>
        /// <value>True if at end of the data</value>
        public bool IsAtEnd { get { return (p_CurrentPosition == p_DataSegment.PositionEnd + 1); } }

        /// <summary>
        /// Constructs input from the given data
        /// </summary>
        /// <param name="data">Data to interface</param>
        public DataInput(byte[] data)
        {
            p_PerformCheckPosition = true;
            p_Data = data;
            p_DataSegment = new DataSegment(0, data.Length - 1);
            p_ReadSegments = new System.Collections.Generic.List<DataSegment>();
            p_PreviousBegin = new System.Collections.Generic.Stack<int>();
            p_PreviousEnd = new System.Collections.Generic.Stack<int>();
        }
        /// <summary>
        /// Contructs the input from the given data, offset and length
        /// </summary>
        /// <param name="data">Data to interface</param>
        /// <param name="originOffset">Beginning offset in the data</param>
        /// <param name="length">Length in the data to interface</param>
        public DataInput(byte[] data, int originOffset, int length)
        {
            p_PerformCheckPosition = true;
            p_Data = data;
            p_DataSegment = new DataSegment(originOffset, length - originOffset - 1);
            p_ReadSegments = new System.Collections.Generic.List<DataSegment>();
            p_PreviousBegin = new System.Collections.Generic.Stack<int>();
            p_PreviousEnd = new System.Collections.Generic.Stack<int>();
            p_CurrentBegin = originOffset;
            p_CurrentPosition = originOffset;
        }

        /// <summary>
        /// Constructs a clone of the current data input with its own stack
        /// </summary>
        /// <returns>Returns the clone</returns>
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

        /// <summary>
        /// Reset the input
        /// </summary>
        /// <remarks>
        /// Remove all data about already read segments
        /// Go to the beginning of the data
        /// </remarks>
        public void Reset()
        {
            p_ReadSegments.Clear();
            p_PreviousBegin.Clear();
            p_PreviousEnd.Clear();
            p_CurrentBegin = p_DataSegment.PositionBegin;
            p_CurrentPosition = p_CurrentBegin;
        }

        /// <summary>
        /// Indicate if data of the following length can be read
        /// </summary>
        /// <param name="length">Length to read</param>
        /// <returns>Returns true if data of this length can be read, false otherwise</returns>
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
        /// <summary>
        /// Check current position for the given length to read on a reading operation
        /// </summary>
        /// <param name="Length">Length to read</param>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
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

        /// <summary>
        /// Read a byte from the data
        /// </summary>
        /// <returns>Next byte read</returns>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
        public System.Byte ReadByte()
        {
            if (p_PerformCheckPosition)
                CheckPosition_OnRead(1);
            return p_Data[p_CurrentPosition];
        }
        /// <summary>
        /// Read a byte and advance in the data
        /// </summary>
        /// <returns>Next byte read</returns>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
        public System.Byte ReadAndAdvanceByte()
        {
            System.Byte CurrentData = ReadByte();
            p_CurrentPosition++;
            return CurrentData;
        }
        /// <summary>
        /// Read a unsigned 16bits length integer from the data
        /// </summary>
        /// <returns>unsigned 16bits length integer</returns>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
        public System.UInt16 ReadUInt16()
        {
            if (p_PerformCheckPosition)
                CheckPosition_OnRead(2);
            System.UInt16 part8 = (System.UInt16)((System.UInt16)p_Data[p_CurrentPosition + 1] << 8);
            System.UInt16 part0 = (System.UInt16)p_Data[p_CurrentPosition];
            return (System.UInt16)(part8 | part0);
        }
        /// <summary>
        /// Read a unsigned 16bits length integer and advance in the data
        /// </summary>
        /// <returns>unsigned 16bits length integer</returns>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
        public System.UInt16 ReadAndAdvanceUInt16()
        {
            System.UInt16 CurrentData = ReadUInt16();
            p_CurrentPosition += 2;
            return CurrentData;
        }
        /// <summary>
        /// Read a unsigned 32bits length integer from the data
        /// </summary>
        /// <returns>unsigned 32bits length integer</returns>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
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
        /// <summary>
        /// Read a unsigned 32bits length integer and advance in the data
        /// </summary>
        /// <returns>unsigned 32bits length integer</returns>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
        public System.UInt32 ReadAndAdvanceUInt32()
        {
            System.UInt32 CurrentData = ReadUInt32();
            p_CurrentPosition += 4;
            return CurrentData;
        }
        /// <summary>
        /// Read a unsigned 64bits length integer from the data
        /// </summary>
        /// <returns>unsigned 64bits length integer</returns>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
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
        /// <summary>
        /// Read a unsigned 64bits length integer and advance in the data
        /// </summary>
        /// <returns>unsigned 64bits length integer</returns>
        /// <exception cref="BinaryException">Exception thrown if cannot read</exception>
        public System.UInt64 ReadAndAdvanceUInt64()
        {
            System.UInt64 CurrentData = ReadUInt64();
            p_CurrentPosition += 8;
            return CurrentData;
        }

        /// <summary>
        /// Push the current segment on a stack and begin a new one at the given distance of the current position
        /// </summary>
        /// <param name="offset">Offset from the current position</param>
        public void PositionShift(int offset)
        {
            p_PreviousBegin.Push(p_CurrentBegin);
            p_PreviousEnd.Push(p_CurrentPosition);
            p_CurrentBegin = p_CurrentPosition + offset;
            p_CurrentPosition = p_CurrentBegin;
        }
        /// <summary>
        /// Push the current segment on a stack and begin a new one at the given absolute position
        /// </summary>
        /// <param name="position">Absolute position for the new segment</param>
        public void PositionMoveTo(int position)
        {
            p_PreviousBegin.Push(p_CurrentBegin);
            p_PreviousEnd.Push(p_CurrentPosition);
            p_CurrentBegin = position;
            p_CurrentPosition = position;
        }
        /// <summary>
        /// Finalise the currently read segment and return to the previous one
        /// </summary>
        public void PositionReturn()
        {
            p_ReadSegments.Add(new DataSegment(p_CurrentBegin, p_CurrentPosition - 1));
            p_CurrentBegin = p_PreviousBegin.Pop();
            p_CurrentPosition = p_PreviousEnd.Pop();
        }
    }
}