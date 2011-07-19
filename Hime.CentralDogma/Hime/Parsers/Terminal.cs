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

        public Terminal(Grammar Parent, ushort SID, string Name, int Priority) : base(Parent, SID, Name)
        {
            priority = Priority;
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolTerminal");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes.Append(Doc.CreateAttribute("Priority"));
            Node.Attributes.Append(Doc.CreateAttribute("Value"));
            Node.Attributes["SID"].Value = sID.ToString();
            Node.Attributes["Name"].Value = localName.Replace("\"", "\\\"");
            Node.Attributes["Priority"].Value = priority.ToString();
            Node.Attributes["Value"].Value = this.ToString();
            return Node;
        }
    }
}
