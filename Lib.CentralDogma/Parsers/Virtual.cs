/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.Parsers
{
    public sealed class Virtual : GrammarSymbol
    {
        public Virtual(Grammar parent, string name) : base(parent, 0, name) { }

        public override XmlNode GetXMLNode(XmlDocument document)
        {
            XmlNode node = document.CreateElement("SymbolVirtual");
            this.AddAttributeToNode(document, node, "Name", localName);
            return node;
        }

        public override string ToString() { return "\"" + localName + "\""; }
    }
}