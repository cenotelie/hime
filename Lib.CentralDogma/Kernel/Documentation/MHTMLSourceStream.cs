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
        protected string location;
        private Stream stream;
        private byte[] buffer;

        public override string ContentTransferEncoding { get { return "base64"; } }
        public override string ContentLocation { get { return location; } }

        public MHTMLSourceStream(string mime, string location, Stream stream) : base(mime)
        {
            this.location = location;
            this.stream = stream;
            this.buffer = new byte[bufferSize];
        }

        public override string Read()
        {
            int read = stream.Read(buffer, 0, bufferSize);
            if (read == 0)
                return null;
            return Convert.ToBase64String(buffer, 0, read);
        }
		
        public override void Close() { }
    }
}