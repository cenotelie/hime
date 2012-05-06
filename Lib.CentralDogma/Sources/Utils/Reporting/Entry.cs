/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.Utils.Reporting
{
    public class Entry
    {
		// TODO: maybe could add position in file
        public virtual ELevel Level { get; set; }
        public virtual string Component { get; set; }
        public virtual string Message { get; private set; }
		
		public Entry(ELevel level, string component, string message)
		{
            this.Level = level;
            this.Component = component;
            this.Message = message;
        }

        public virtual XmlNode GetMessageNode(XmlDocument document)
        {
            XmlNode element = document.CreateElement("Message");
            element.InnerText = this.Message;
            return element;
        }
		
		public override string ToString ()
		{
			return this.Level + ": " + this.Component + ": " + this.Message;
		}
    }
}