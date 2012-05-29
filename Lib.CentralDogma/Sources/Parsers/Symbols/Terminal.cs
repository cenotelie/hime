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
    abstract class Terminal : GrammarSymbol
    {
        public int Priority { get; set; }

        public Terminal(ushort sid, string name, int priority) : base(sid, name)
        {
            this.Priority = priority;
        }

        protected override string Type { get { return "Terminal"; } }

        public override XmlNode GetXMLNode(XmlDocument document)
        {
			XmlNode node = base.GetXMLNode(document);
            this.AddAttributeToNode(document, node, "priority", this.Priority.ToString());
            return node;
        }
    }
}
