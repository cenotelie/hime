/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.Kernel.Reporting
{
    public class BaseEntry : Entry
    {
        public BaseEntry(ELevel level, string component, string message)
        {
            this.Level = level;
            this.Component = component;
            this.Message = message;
        }

        internal override XmlNode GetMessageNode(XmlDocument doc)
        {
            XmlNode element = doc.CreateElement("Message");
            element.InnerText = this.Message;
            return element;
        }
    }
}