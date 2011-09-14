namespace Hime.Kernel.Documentation
{
    public class MHTMLSourceFileText : MHTMLSourceFile
    {
        private string charset;
        public override string ContentType { get { return mime + "; charset=\"" + charset + "\""; } }
        public MHTMLSourceFileText(string mime, string charset, string location, string file)
            : base(mime, location, file)
        {
            this.charset = charset;
        }
    }
}