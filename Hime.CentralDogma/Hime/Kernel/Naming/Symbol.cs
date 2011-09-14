/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Kernel.Naming
{
    public abstract class Symbol
    {
        protected SymbolAccess access;
        protected Dictionary<string, Symbol> children;

        public Symbol Parent { get; protected set; }
        public abstract string LocalName { get; }
        public abstract QualifiedName CompleteName { get; }
        public SymbolAccess Access
        {
            get { return access; }
            set { access = value; }
        }
        public ICollection<Symbol> Children { get { return children.Values; } }

        public Symbol()
        {
            children = new Dictionary<string, Symbol>();
            access = SymbolAccess.Public;
        }

        protected abstract void SymbolSetParent(Symbol symbol);
        protected abstract void SymbolSetCompleteName(QualifiedName name);

        public virtual void SymbolAddChild(Symbol child)
        {
            if (children.ContainsKey(child.LocalName))
                throw new NameCollisionException(this, child.LocalName);
            child.SymbolSetParent(this);
            child.SymbolSetCompleteName(CompleteName + child.LocalName);
            children.Add(child.LocalName, child);
        }

        public Symbol ResolveName(QualifiedName name)
        {
            Symbol Found = null;
            Symbol Current = this;
            while (Current != null)
            {
                Symbol Target = Current.ResolveName_DeepCheck(name);
                if (Target != null)
                {
                    if (Found != null)
                        throw new AmbiguousNameException(this, name);
                    Found = Target;
                }
                Current = Current.Parent;
            }
            if (Found == null)
                throw new NameResolutionException(this, name);
            return Found;
        }
        protected Symbol ResolveName_DeepCheck(QualifiedName name)
        {
            if (name.IsEmpty)
                return this;
            if (children.ContainsKey(name.PrefixOrder0))
                return children[name.PrefixOrder0].ResolveName_DeepCheck(name.SubName);
            return null;
        }

        public override string ToString() { return this.LocalName; }
    }
}