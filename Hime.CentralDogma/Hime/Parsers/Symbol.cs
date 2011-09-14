/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers
{
    public abstract class Symbol : Hime.Kernel.Naming.Symbol
    {
        protected string localName;
        protected Hime.Kernel.Naming.QualifiedName completeName;

        internal protected ushort SID { get; private set; }
        public override string LocalName { get { return localName; } }
        public override Hime.Kernel.Naming.QualifiedName CompleteName { get { return completeName; } }

        public Symbol(Grammar parent, ushort SID, string Name)
            : base()
        {
            this.Parent = parent;
            this.SID = SID;
            localName = Name;
            if (Parent == null)
                completeName = new Hime.Kernel.Naming.QualifiedName(localName);
            else
                completeName = Parent.CompleteName + localName;
        }

        protected override void SymbolSetParent(Hime.Kernel.Naming.Symbol symbol)
        {
            if (symbol is Grammar)
                this.Parent = (Grammar)symbol;
            else
                throw new Kernel.Naming.WrongParentSymbolException(this, symbol.GetType(), typeof(Grammar));
        }
        protected override void SymbolSetCompleteName(Hime.Kernel.Naming.QualifiedName name) { completeName = name; }
        public override void SymbolAddChild(Hime.Kernel.Naming.Symbol symbol) { throw new Kernel.Naming.CannotAddChildException(this, symbol); }
        public abstract System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document);

        internal class Comparer : IEqualityComparer<Symbol>
        {
            public bool Equals(Symbol x, Symbol y) { return (x.SID == y.SID); }
            public int GetHashCode(Symbol obj) { return obj.SID; }
            private Comparer() { }
            private static Comparer instance = new Comparer();
            public static Comparer Instance { get { return instance; } }
        }
    }
}