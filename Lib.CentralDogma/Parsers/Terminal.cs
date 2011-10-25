/*
 * Author: Charles Hymans
 * Date: 19/07/2011
 * Time: 19:03
 * 
 */
using System;
using System.Xml;

namespace Hime.Parsers
{
    public abstract class Terminal : GrammarSymbol
    {
        internal int Priority
        {
            get;
            set;
        }

        public Terminal(Grammar parent, ushort sid, string name, int priority) : base(parent, sid, name)
        {
            this.Priority = priority;
        }

        public override XmlNode GetXMLNode(XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("SymbolTerminal");
            node.Attributes.Append(document.CreateAttribute("SID"));
            node.Attributes.Append(document.CreateAttribute("Name"));
            node.Attributes.Append(document.CreateAttribute("Priority"));
            node.Attributes.Append(document.CreateAttribute("Value"));
            node.Attributes["SID"].Value = SID.ToString();
            node.Attributes["Name"].Value = localName.Replace("\"", "\\\"");
            node.Attributes["Priority"].Value = this.Priority.ToString();
            node.Attributes["Value"].Value = this.ToString();
            return node;
        }
    }
}
