/*
 * Author: Charles Hymans
 * 
 */
using System;

namespace Hime.Redist
{
	public class DataSegment
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
}

