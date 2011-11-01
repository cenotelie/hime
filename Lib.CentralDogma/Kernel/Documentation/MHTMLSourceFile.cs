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
        private string file;
        private Stream stream;
        private byte[] buffer;

        internal MHTMLSourceFile(string mime, string location, string file) : base(mime, location)
        {
            this.file = file;
        }

        internal override string Read()
        {
            if (stream == null)
            {
                stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                buffer = new byte[bufferSize];
            }

            int read = stream.Read(buffer, 0, bufferSize);
            if (read == 0)
                return null;
            return Convert.ToBase64String(buffer, 0, read);
        }
		
        internal override void Close()
        {
            if (stream != null)
                stream.Close();
        }
    }
}