/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.Kernel.Reporting
{
    public abstract class Entry
    {
		// TODO: put a private set
        internal virtual ELevel Level { get; set; }
        internal virtual string Component { get; set; }
        internal virtual string Message { get; set; }

        internal abstract XmlNode GetMessageNode(XmlDocument doc);
    }
}