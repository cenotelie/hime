/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using System.Xml;

namespace Hime.Parsers
{
    abstract class GrammarSymbol
    {
        public ushort SID { get; private set; }
        public string Name { get; private set; }

        protected abstract string Type { get; }

        public GrammarSymbol(ushort sid, string name)
        {
            this.SID = sid;
            this.Name = name;
        }
        
		public virtual XmlNode GetXMLNode(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Symbol");
            this.AddAttributeToNode(document, node, "type", Type);
            this.AddAttributeToNode(document, node, "name", Name);
            this.AddAttributeToNode(document, node, "sid", SID.ToString("X"));
            return node;
        }
		
		// TODO: maybe should wrap Node in our own class and put this method in there instead
		protected void AddAttributeToNode(XmlDocument document, XmlNode node, string attributeName, string attributeValue)
		{
			XmlAttribute attribute = document.CreateAttribute(attributeName);
			node.Attributes.Append(attribute);
			node.Attributes[attributeName].Value = attributeValue;
		}

        public sealed class Comparer : IEqualityComparer<GrammarSymbol>
        {
            public bool Equals(GrammarSymbol x, GrammarSymbol y) { return (x.SID == y.SID); }
            public int GetHashCode(GrammarSymbol obj) { return obj.SID; }
            private Comparer() { }
            private static Comparer instance = new Comparer();
            public static Comparer Instance { get { return instance; } }
        }

        public override string ToString() { return Name; }
    }
}