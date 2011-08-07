/*
 * Author: Charles Hymans
 * Date: 07/08/2011
 * Time: 15:05
 * 
 */
using System;
using System.Xml;

namespace Hime.Parsers
{
	public abstract class Variable : Symbol
    {
        public Variable(Grammar Parent, ushort SID, string Name) : base(Parent, SID, Name) { }

        public override XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            XmlNode Node = Doc.CreateElement("SymbolVariable");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["SID"].Value = SID.ToString("X");
            Node.Attributes["Name"].Value = localName;
            return Node;
        }
    }
}
