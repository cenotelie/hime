/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.Kernel.Reporting
{
    public class Entry
    {
        internal virtual ELevel Level { get; set; }
        internal virtual string Component { get; set; }
        internal virtual string Message { get; private set; }
		
		public Entry(ELevel level, string component, string message)
		{
            this.Level = level;
            this.Component = component;
            this.Message = message;
        }

        internal virtual XmlNode GetMessageNode(XmlDocument document)
        {
            XmlNode element = document.CreateElement("Message");
            element.InnerText = this.Message;
            return element;
        }
    }
}