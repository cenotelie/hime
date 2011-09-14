/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Reporting
{
    public interface IEntry
    {
        ELevel Level { get; }
        string Component { get; }
        string Message { get; }

        System.Xml.XmlNode GetMessageNode(System.Xml.XmlDocument doc);
    }
}