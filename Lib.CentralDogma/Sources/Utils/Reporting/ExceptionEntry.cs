/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Xml;

namespace Hime.Utils.Reporting
{
    public class ExceptionEntry : Entry
    {
        private Exception exception;

        public ExceptionEntry(Exception exception) : base(ELevel.Error, "Compiler", "Exception " + exception.Message)
		{ 
			this.exception = exception;
		}

        public override XmlNode GetMessageNode(XmlDocument doc)
        {
            XmlNode element = doc.CreateElement("Exception");
            element.Attributes.Append(doc.CreateAttribute("EID"));
            element.Attributes["EID"].Value = exception.GetHashCode().ToString();
            XmlNode message = doc.CreateElement("Message");
            message.InnerText = Message;
            element.AppendChild(message);
            XmlNode method = doc.CreateElement("Method");
            method.InnerText = exception.TargetSite.ToString();
            element.AppendChild(method);
            XmlNode stack = doc.CreateElement("Stack");
            string data = exception.StackTrace;
            string[] lines = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                XmlNode nl = doc.CreateElement("Line");
                nl.InnerText = line;
                stack.AppendChild(nl);
            }
            element.AppendChild(stack);
            return element;
        }
    }
}