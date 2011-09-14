namespace Hime.Kernel.Documentation
{
    public interface MHTMLSource
    {
        string ContentType { get; }
        string ContentTransferEncoding { get; }
        string ContentLocation { get; }
        string Read();
        void Close();
    }
}