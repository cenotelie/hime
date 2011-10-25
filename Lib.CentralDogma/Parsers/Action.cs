/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;
using System.Collections.Generic;

namespace Hime.Parsers
{
    public sealed class Action : GrammarSymbol
    {
        public Action(Grammar parent, string name) : base(parent, 0, name) { }

        public override XmlNode GetXMLNode(XmlDocument document)
        {
            XmlNode node = document.CreateElement("SymbolAction");
            this.AddAttributeToNode(document, node, "Name", localName);
            return node;
        }

        public override string ToString() { return "{" + localName + "}"; }
    }
}
