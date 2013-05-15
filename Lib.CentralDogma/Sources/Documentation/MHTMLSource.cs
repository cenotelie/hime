using System;
using System.IO;
using System.Text;

namespace Hime.CentralDogma.Documentation
{
    class MHTMLSource
    {
        private const int bufferSize = 900;
        private const string contentTransferEncoding = "base64";

		private Stream stream;
        private byte[] buffer;
        private string contentType;
		private string contentLocation;

        public MHTMLSource(string mime, string location, Stream stream)
		{
			this.contentType = mime;
			this.contentLocation = location;
			this.stream = stream;
            this.buffer = new byte[bufferSize];
		}

        public MHTMLSource(string mime, string location, string file)
            : this(mime, location, new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
        {
        }

        public string ToMHTML(int linebreak)
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("Content-Type: " + this.contentType);
			result.AppendLine("Content-Transfer-Encoding: " + contentTransferEncoding);
			result.AppendLine("Content-Location: " + this.contentLocation);
			result.AppendLine();
			
			int length = 0;
        	while (true)
            {
    	        int read = stream.Read(buffer, 0, bufferSize);
	            if (read == 0) break;
				string text = Convert.ToBase64String(buffer, 0, read);
            	while (linebreak < (length + text.Length))
				{
	            	string part1 = text.Substring(0, linebreak - length);
    	            text = text.Substring(linebreak - length);
        	        result.AppendLine(part1);
            	    length = 0;
               	}
                length += text.Length;
	            result.Append(text);
        	}
			
			this.stream.Close();
			return result.ToString();
		}
	}
}