/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Naming;
using System.Xml;

namespace Hime.Parsers
{
    public abstract class GrammarSymbol : Symbol
    {
        protected string localName;
        protected QualifiedName completeName;

        internal protected ushort SID { get; private set; }
        public override string LocalName { get { return localName; } }
        public override QualifiedName CompleteName { get { return completeName; } }

        public GrammarSymbol(Grammar parent, ushort SID, string Name)
            : base()
        {
            this.Parent = parent;
            this.SID = SID;
            localName = Name;
            if (Parent == null)
                completeName = new QualifiedName(localName);
            else
                completeName = Parent.CompleteName + localName;
        }

        protected override void SymbolSetParent(Symbol symbol)
        {
            if (symbol is Grammar)
                this.Parent = (Grammar)symbol;
			// TODO: this code is really not nice here
            else
                throw new Kernel.Naming.WrongParentSymbolException(this, symbol.GetType(), typeof(Grammar));
        }
        protected override void SymbolSetCompleteName(QualifiedName name) { completeName = name; }
        public override void SymbolAddChild(Symbol symbol) { throw new Kernel.Naming.CannotAddChildException(this, symbol); }
        
		public virtual XmlNode GetXMLNode(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Symbol");
            this.AddAttributeToNode(document, node, "type", this.GetType().Name);
            this.AddAttributeToNode(document, node, "name", localName.Replace("\"", "\\\""));
            this.AddAttributeToNode(document, node, "sid", SID.ToString("X"));
            return node;
        }
		
		// TODO: maybe should wrap Node in our own class and put this method in there instead
		internal protected void AddAttributeToNode(XmlDocument document, XmlNode node, string attributeName, string attributeValue)
		{
			XmlAttribute attribute = document.CreateAttribute(attributeName);
			node.Attributes.Append(attribute);
			node.Attributes[attributeName].Value = attributeValue;
		}

        internal class Comparer : IEqualityComparer<GrammarSymbol>
        {
            public bool Equals(GrammarSymbol x, GrammarSymbol y) { return (x.SID == y.SID); }
            public int GetHashCode(GrammarSymbol obj) { return obj.SID; }
            private Comparer() { }
            private static Comparer instance = new Comparer();
            public static Comparer Instance { get { return instance; } }
        }		
    }
}