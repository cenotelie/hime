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
        internal int Priority { get; set; }

        public Terminal(Grammar parent, ushort sid, string name, int priority) : base(parent, sid, name)
        {
            this.Priority = priority;
        }

        public override XmlNode GetXMLNode(XmlDocument document)
        {
			XmlNode node = base.GetXMLNode(document);
            this.AddAttributeToNode(document, node, "SID", SID.ToString());
            this.AddAttributeToNode(document, node, "Priority", this.Priority.ToString());
            this.AddAttributeToNode(document, node, "Value", this.ToString());
            return node;
        }
    }
}
