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
    internal abstract class MHTMLSource
    {
        private const int bufferSize = 900;

		private Stream stream;
        private byte[] buffer;

        internal string ContentType { get; private set; }

		internal string ContentLocation { get; private set; }
		
        internal string ContentTransferEncoding { get { return "base64"; } }

        internal string Read()
        {
            int read = stream.Read(buffer, 0, bufferSize);
            if (read == 0)
                return null;
            return Convert.ToBase64String(buffer, 0, read);
        }

		internal void Close() 
		{ 
			this.stream.Close();
		}
		
		internal MHTMLSource(string mime, string location, Stream stream)
		{
			this.ContentType = mime;
			this.ContentLocation = location;
			this.stream = stream;
            this.buffer = new byte[bufferSize];
		}
    }
}