using System.Collections.Generic;

namespace Hime.Kernel
{
    public abstract class NamingException : System.Exception
    {
        public NamingException() : base() { }
        public NamingException(string message) : base(message) { }
        public NamingException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    public class NameResolutionException : NamingException
    {
        private Symbol p_Origin;
        private QualifiedName p_Name;

        public Symbol Origin { get { return p_Origin; } }
        public QualifiedName Name { get { return p_Name; } }

        public NameResolutionException(Symbol origin, QualifiedName name) : base("Cannot resolve name " + name.ToString() + " from symbol " + origin.CompleteName.ToString())
        {
            p_Origin = origin;
            p_Name = name;
        }
    }

    public class AmbiguousNameException : NamingException
    {
        private Symbol p_Origin;
        private QualifiedName p_Name;

        public Symbol Origin { get { return p_Origin; } }
        public QualifiedName Name { get { return p_Name; } }

        public AmbiguousNameException(Symbol origin, QualifiedName name) : base("Ambiguous name " + name.ToString() + " from symbol " + origin.CompleteName.ToString())
        {
            p_Origin = origin;
            p_Name = name;
        }
    }

    public class NameCollisionException : NamingException
    {
        private Symbol p_Container;
        private string p_Name;

        public Symbol Container { get { return p_Container; } }
        public string Name { get { return p_Name; } }

        public NameCollisionException(Symbol container, string name) : base("Name collision in symbol " + container.CompleteName.ToString() + " for " + name)
        {
            p_Container = container;
            p_Name = name;
        }
    }

    public class WrongParentSymbolException : NamingException
    {
        private Symbol p_Current;
        private System.Type p_GivenType;
        private System.Type p_ExpectedType;

        public Symbol CurrentSymbol { get { return p_Current; } }
        public System.Type GivenType { get { return p_GivenType; } }
        public System.Type ExpectedType { get { return p_ExpectedType; } }

        public WrongParentSymbolException(Symbol current, System.Type givenType, System.Type expectedType) : base("Wrong parent type for " + current.CompleteName.ToString() + "; expected " + expectedType.Name)
        {
            p_Current = current;
            p_GivenType = givenType;
            p_ExpectedType = expectedType;
        }
    }

    public class CannotAddChildException : NamingException
    {
        private Symbol p_Parent;
        private Symbol p_Child;

        public Symbol ParentSymbol { get { return p_Parent; } }
        public Symbol ChildSymbol { get { return p_Child; } }

        public CannotAddChildException(Symbol parent, Symbol child) : base("Cannot add " + child.LocalName + " in symbol " + parent.CompleteName.ToString())
        {
            p_Parent = parent;
            p_Child = child;
        }
    }
    


    public class QualifiedName
    {
        private static char p_SeparatorChar = '.';
        private List<string> p_Path;

        public static char Separator { get { return p_SeparatorChar; } }
        public string PrefixOrder0 { get { return p_Path[0]; } }
        public string NakedName { get { return p_Path[p_Path.Count - 1]; } }
        public QualifiedName Prefix { get { return new QualifiedName(p_Path.GetRange(0, p_Path.Count - 1)); } }
        public QualifiedName SubName { get { return new QualifiedName(p_Path.GetRange(1, p_Path.Count - 1)); } }
        public bool IsEmpty { get { return (p_Path.Count == 0); } }
        public bool HasPrefix { get { return (p_Path.Count > 1); } }
        public bool IsNaked { get { return (p_Path.Count == 1); } }

        public QualifiedName(string name)
        {
            p_Path = new List<string>();
            p_Path.Add(name);
        }
        public QualifiedName(List<string> path) { p_Path = path; }

        public static QualifiedName ParseName(string completeName)
        {
            List<string> Path = new List<string>();
            int Begin = 0;
            int End = 0;
            while (End != completeName.Length)
            {
                if (completeName[End] == p_SeparatorChar)
                {
                    Path.Add(completeName.Substring(Begin, End - Begin));
                    Begin = End + 1;
                    End = Begin;
                }
                else
                    End++;
            }
            Path.Add(completeName.Substring(Begin, End - Begin));
            return new QualifiedName(Path);
        }

        public override string ToString() { return ToString(p_SeparatorChar); }
        public string ToString(char separator)
        {
            if (p_Path.Count == 0)
                return string.Empty;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder(p_Path[0]);
            for (int i = 1; i != p_Path.Count; i++)
            {
                Builder.Append(separator);
                Builder.Append(p_Path[i]);
            }
            return Builder.ToString();
        }

        public static QualifiedName operator +(QualifiedName prefix, string localName)
        {
            List<string> Path = new List<string>();
            Path.AddRange(prefix.p_Path);
            Path.Add(localName);
            return new QualifiedName(Path);
        }
    }

    public enum SymbolAccess
    {
        Public,
        Protected,
        Private,
        Internal
    }

    public abstract class Symbol
    {
        protected SymbolAccess p_Access;
        protected Dictionary<string, Symbol> p_Children;

        public abstract Symbol Parent { get; }
        public abstract string LocalName { get; }
        public abstract QualifiedName CompleteName { get; }
        public SymbolAccess Access
        {
            get { return p_Access; }
            set { p_Access = value; }
        }
        public ICollection<Symbol> Children { get { return p_Children.Values; } }

        public Symbol()
        {
            p_Children = new Dictionary<string, Symbol>();
            p_Access = SymbolAccess.Public;
        }

        protected abstract void SymbolSetParent(Symbol symbol);
        protected abstract void SymbolSetCompleteName(QualifiedName name);

        public virtual void SymbolAddChild(Symbol child)
        {
            if (p_Children.ContainsKey(child.LocalName))
                throw new NameCollisionException(this, child.LocalName);
            child.SymbolSetParent(this);
            child.SymbolSetCompleteName(CompleteName + child.LocalName);
            p_Children.Add(child.LocalName, child);
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
            if (p_Children.ContainsKey(name.PrefixOrder0))
                return p_Children[name.PrefixOrder0].ResolveName_DeepCheck(name.SubName);
            return null;
        }

        public override string ToString() { return this.LocalName; }
    }

    public class Namespace : Symbol
    {
        private Namespace p_Parent;
        private string p_LocalName;
        private QualifiedName p_CompleteName;
        private Dictionary<string, Namespace> p_Namespaces;

        public override Symbol Parent { get { return p_Parent; } }
        public override string LocalName { get { return p_LocalName; } }
        public override QualifiedName CompleteName { get { return p_CompleteName; } }

        public Namespace(Namespace parent, string name) : base()
        {
            p_Parent = parent;
            p_LocalName = name;
            if (p_Parent == null)
                p_CompleteName = new QualifiedName(new List<string>());
            else
                p_CompleteName = p_Parent.p_CompleteName + p_LocalName;
            p_Namespaces = new Dictionary<string, Namespace>();
        }

        public static Namespace CreateRoot()
        {
            return new Namespace(null, "global");
        }

        public override void SymbolAddChild(Symbol child)
        {
            base.SymbolAddChild(child);
            if (child is Namespace)
                p_Namespaces.Add(child.LocalName, (Namespace)child);
        }
        protected override void SymbolSetParent(Symbol symbol)
        {
            if (!(symbol is Namespace))
            {
                throw new WrongParentSymbolException(this, symbol.GetType(),typeof(Namespace));
            }
            this.p_Parent = (Namespace)symbol;
        }
        protected override void SymbolSetCompleteName(QualifiedName name) { p_CompleteName = name; }

        public Namespace AddSubNamespace(QualifiedName name)
        {
            if (!p_Children.ContainsKey(name.PrefixOrder0))
            {
                Namespace New = new Namespace(this, name.PrefixOrder0);
                p_Children.Add(name.PrefixOrder0, New);
                p_Namespaces.Add(name.PrefixOrder0, New);
            }
            if (name.HasPrefix)
                return p_Namespaces[name.PrefixOrder0].AddSubNamespace(name.SubName);
            return p_Namespaces[name.PrefixOrder0];
        }
    }
}
