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
    internal class MHTMLSourceFile : MHTMLSource
    {
        private const int bufferSize = 900;
        protected string mime;
        protected string location;
        protected string file;
        private Stream stream;
        private byte[] buffer;

        public virtual string ContentType { get { return mime; } }
        public string ContentTransferEncoding { get { return "base64"; } }
        public string ContentLocation { get { return location; } }

        public MHTMLSourceFile(string mime, string location, string file)
        {
            this.mime = mime;
            this.location = location;
            this.file = file;
        }

        public string Read()
        {
            if (stream == null)
            {
                stream = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
                buffer = new byte[bufferSize];
            }

            int read = stream.Read(buffer, 0, bufferSize);
            if (read == 0)
                return null;
            return Convert.ToBase64String(buffer, 0, read);
        }
		
        public void Close()
        {
            if (stream != null)
                stream.Close();
        }
    }
}