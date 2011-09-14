/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers
{
    public sealed class Action : Symbol
    {
        public Action(Grammar parent, string name) : base(parent, 0, name) { }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("SymbolAction");
            node.Attributes.Append(document.CreateAttribute("Name"));
            node.Attributes["Name"].Value = localName;
            return node;
        }

        public override string ToString() { return "{" + localName + "}"; }
    }
}
