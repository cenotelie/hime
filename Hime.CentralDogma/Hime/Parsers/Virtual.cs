/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Parsers
{
    public sealed class Virtual : GrammarSymbol
    {
        public Virtual(Grammar parent, string name) : base(parent, 0, name) { }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("SymbolVirtual");
            node.Attributes.Append(document.CreateAttribute("Name"));
            node.Attributes["Name"].Value = localName;
            return node;
        }

        public override string ToString() { return "\"" + localName + "\""; }
    }
}