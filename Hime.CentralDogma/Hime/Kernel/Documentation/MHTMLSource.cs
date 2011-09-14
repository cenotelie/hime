/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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