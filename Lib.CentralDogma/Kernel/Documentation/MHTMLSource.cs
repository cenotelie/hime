/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Documentation
{
    internal abstract class MHTMLSource
    {
        protected const int bufferSize = 900;

        internal string ContentType { get; private set; }
		internal string ContentLocation { get; private set; }
		
        public abstract string ContentTransferEncoding { get; }
        public abstract string Read();
        public abstract void Close();
		
		internal MHTMLSource(string mime, string location)
		{
			this.ContentType = mime;
			this.ContentLocation = location;
		}
    }
}