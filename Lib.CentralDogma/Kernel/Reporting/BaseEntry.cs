/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.Kernel.Reporting
{
    public class BaseEntry : IEntry
    {
        public ELevel Level { get; private set; }
        public string Component { get; private set; }
        public string Message { get; private set; }

        public BaseEntry(ELevel level, string component, string message)
        {
            this.Level = level;
            this.Component = component;
            this.Message = message;
        }

        public XmlNode GetMessageNode(XmlDocument doc)
        {
            XmlNode element = doc.CreateElement("Message");
            element.InnerText = this.Message;
            return element;
        }
    }
}