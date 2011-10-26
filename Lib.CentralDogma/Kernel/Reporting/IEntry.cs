/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.Kernel.Reporting
{
    public interface IEntry
    {
        ELevel Level { get; }
        string Component { get; }
        string Message { get; }

        XmlNode GetMessageNode(XmlDocument doc);
    }
}