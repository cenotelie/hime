/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using System.Xml;

namespace Hime.CentralDogma.Grammars
{
    abstract class Symbol
    {
        public ushort SID { get; private set; }
        public string Name { get; private set; }

        protected abstract string Type { get; }

        public Symbol(ushort sid, string name)
        {
            this.SID = sid;
            this.Name = name;
        }
        
		public virtual XmlNode GetXMLNode(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Symbol");
            this.AddAttributeToNode(document, node, "type", Type);
            this.AddAttributeToNode(document, node, "name", ToString());
            this.AddAttributeToNode(document, node, "sid", SID.ToString("X"));
            return node;
        }
		
		protected void AddAttributeToNode(XmlDocument document, XmlNode node, string attributeName, string attributeValue)
		{
			XmlAttribute attribute = document.CreateAttribute(attributeName);
			node.Attributes.Append(attribute);
			node.Attributes[attributeName].Value = attributeValue;
		}

        public sealed class Comparer : IEqualityComparer<Symbol>
        {
            public bool Equals(Symbol x, Symbol y) { return (x.SID == y.SID); }
            public int GetHashCode(Symbol obj) { return obj.SID; }
            private Comparer() { }
            private static Comparer instance = new Comparer();
            public static Comparer Instance { get { return instance; } }
        }

        public override string ToString() { return Name; }
    }
}