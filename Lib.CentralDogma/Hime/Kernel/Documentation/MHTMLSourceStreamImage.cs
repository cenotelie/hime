/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Documentation
{
    public class MHTMLSourceStreamImage : MHTMLSourceStream
    {
        public MHTMLSourceStreamImage(string mime, string location, System.IO.Stream stream)
            : base(mime, location, stream)
        { }
    }
}