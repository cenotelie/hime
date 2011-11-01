/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.IO;

namespace Hime.Kernel.Documentation
{
    internal class MHTMLSourceStream : MHTMLSource
    {
        private Stream stream;
        private byte[] buffer;

        internal MHTMLSourceStream(string mime, string location, Stream stream) : base(mime, location)
        {
            this.stream = stream;
            this.buffer = new byte[bufferSize];
        }

        internal override string Read()
        {
            int read = stream.Read(buffer, 0, bufferSize);
            if (read == 0)
                return null;
            return Convert.ToBase64String(buffer, 0, read);
        }
		
        internal override void Close() { }
    }
}