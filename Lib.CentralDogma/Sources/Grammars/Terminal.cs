using System.Collections.Generic;
/*
 * Author: Charles Hymans
 * Date: 19/07/2011
 * Time: 19:03
 * 
 */
using System.Xml;

namespace Hime.CentralDogma.Grammars
{
    abstract class Terminal : Symbol, Automata.FinalItem
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

        public sealed class PriorityComparer : IComparer<Terminal>
        {
            public int Compare(Terminal x, Terminal y) { return (y.Priority - x.Priority); }
            private PriorityComparer() { }
            private static PriorityComparer instance = new PriorityComparer();
            public static PriorityComparer Instance { get { return instance; } }
        }
    }
}
