/*
 * Author: Charles Hymans
 * Date: 19/07/2011
 * Time: 19:03
 * 
 */
using System;

namespace Hime.Parsers
{
    public abstract class Terminal : Symbol
    {
        protected int priority;

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public Terminal(Grammar parent, ushort sid, string name, int priority) : base(parent, sid, name)
        {
            this.priority = priority;
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("SymbolTerminal");
            node.Attributes.Append(document.CreateAttribute("SID"));
            node.Attributes.Append(document.CreateAttribute("Name"));
            node.Attributes.Append(document.CreateAttribute("Priority"));
            node.Attributes.Append(document.CreateAttribute("Value"));
            node.Attributes["SID"].Value = SID.ToString();
            node.Attributes["Name"].Value = localName.Replace("\"", "\\\"");
            node.Attributes["Priority"].Value = priority.ToString();
            node.Attributes["Value"].Value = this.ToString();
            return node;
        }
    }
}
