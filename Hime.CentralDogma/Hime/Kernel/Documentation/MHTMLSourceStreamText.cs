namespace Hime.Kernel.Documentation
{
    public class MHTMLSourceStreamText : MHTMLSourceStream
    {
        private string charset;
        public override string ContentType { get { return mime + "; charset=\"" + charset + "\""; } }
        public MHTMLSourceStreamText(string mime, string charset, string location, System.IO.Stream stream)
            : base(mime, location, stream)
        {
            this.charset = charset;
        }
    }
}