namespace Hime.Kernel.Documentation
{
    public class MHTMLSourceStreamImage : MHTMLSourceStream
    {
        public MHTMLSourceStreamImage(string mime, string location, System.IO.Stream stream)
            : base(mime, location, stream)
        { }
    }
}