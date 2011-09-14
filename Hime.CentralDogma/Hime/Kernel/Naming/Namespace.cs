/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Kernel.Naming
{
    public class Namespace : Symbol
    {
        private string localName;
        private QualifiedName completeName;
        private Dictionary<string, Namespace> namespaces;

        public override string LocalName { get { return localName; } }
        public override QualifiedName CompleteName { get { return completeName; } }

        public Namespace(Namespace parent, string name)
            : base()
        {
            this.Parent = parent;
            localName = name;
            if (parent == null)
                completeName = new QualifiedName(new List<string>());
            else
                completeName = parent.completeName + localName;
            namespaces = new Dictionary<string, Namespace>();
        }

        public override void SymbolAddChild(Symbol child)
        {
            base.SymbolAddChild(child);
            if (child is Namespace)
                namespaces.Add(child.LocalName, (Namespace)child);
        }
        protected override void SymbolSetParent(Symbol symbol)
        {
            if (!(symbol is Namespace))
            {
                throw new WrongParentSymbolException(this, symbol.GetType(), typeof(Namespace));
            }
            this.Parent = (Namespace)symbol;
        }
        protected override void SymbolSetCompleteName(QualifiedName name) { completeName = name; }

        public Namespace AddSubNamespace(QualifiedName name)
        {
            if (!children.ContainsKey(name.PrefixOrder0))
            {
                Namespace New = new Namespace(this, name.PrefixOrder0);
                children.Add(name.PrefixOrder0, New);
                namespaces.Add(name.PrefixOrder0, New);
            }
            if (name.HasPrefix)
                return namespaces[name.PrefixOrder0].AddSubNamespace(name.SubName);
            return namespaces[name.PrefixOrder0];
        }
    }
}