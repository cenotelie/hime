/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Reporting
{
    public class BaseEntry : IEntry
    {
        protected ELevel level;
        protected string component;
        protected string message;

        public ELevel Level { get { return level; } }
        public string Component { get { return component; } }
        public string Message { get { return message; } }

        public BaseEntry(ELevel level, string component, string message)
        {
            this.level = level;
            this.component = component;
            this.message = message;
        }

        public System.Xml.XmlNode GetMessageNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode element = doc.CreateElement("Message");
            element.InnerText = message;
            return element;
        }
    }
}