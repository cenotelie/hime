/*
 * Author: Charles Hymans
 * 
 */
using System;

namespace Hime.Redist
{
    public class BinaryException : Exception
    {
        public BinaryException() : base() { }
        public BinaryException(string message) : base(message) { }
        public BinaryException(string message, Exception innerException) : base(message, innerException) { }
    }
}

