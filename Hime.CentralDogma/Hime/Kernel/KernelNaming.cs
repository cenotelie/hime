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
        private Symbol origin;
        private QualifiedName name;

        public Symbol Origin { get { return origin; } }
        public QualifiedName Name { get { return name; } }

        public NameResolutionException(Symbol origin, QualifiedName name) : base("Cannot resolve name " + name.ToString() + " from symbol " + origin.CompleteName.ToString())
        {
            this.origin = origin;
            this.name = name;
        }
    }

    public class AmbiguousNameException : NamingException
    {
        private Symbol origin;
        private QualifiedName name;

        public Symbol Origin { get { return origin; } }
        public QualifiedName Name { get { return name; } }

        public AmbiguousNameException(Symbol origin, QualifiedName name) : base("Ambiguous name " + name.ToString() + " from symbol " + origin.CompleteName.ToString())
        {
            this.origin = origin;
            this.name = name;
        }
    }

    public class NameCollisionException : NamingException
    {
        private Symbol container;
        private string name;

        public Symbol Container { get { return container; } }
        public string Name { get { return name; } }

        public NameCollisionException(Symbol container, string name) : base("Name collision in symbol " + container.CompleteName.ToString() + " for " + name)
        {
            this.container = container;
            this.name = name;
        }
    }

    public class WrongParentSymbolException : NamingException
    {
        private Symbol current;
        private System.Type givenType;
        private System.Type expectedType;

        public Symbol CurrentSymbol { get { return current; } }
        public System.Type GivenType { get { return givenType; } }
        public System.Type ExpectedType { get { return expectedType; } }

        public WrongParentSymbolException(Symbol current, System.Type givenType, System.Type expectedType) : base("Wrong parent type for " + current.CompleteName.ToString() + "; expected " + expectedType.Name)
        {
            this.current = current;
            this.givenType = givenType;
            this.expectedType = expectedType;
        }
    }

    public class CannotAddChildException : NamingException
    {
        private Symbol parent;
        private Symbol child;

        public Symbol ParentSymbol { get { return parent; } }
        public Symbol ChildSymbol { get { return child; } }

        public CannotAddChildException(Symbol parent, Symbol child) : base("Cannot add " + child.LocalName + " in symbol " + parent.CompleteName.ToString())
        {
            this.parent = parent;
            this.child = child;
        }
    }
    


    public class QualifiedName
    {
        private static char separatorChar = '.';
        private List<string> path;

        public static char Separator { get { return separatorChar; } }
        public string PrefixOrder0 { get { return path[0]; } }
        public string NakedName { get { return path[path.Count - 1]; } }
        public QualifiedName Prefix { get { return new QualifiedName(path.GetRange(0, path.Count - 1)); } }
        public QualifiedName SubName { get { return new QualifiedName(path.GetRange(1, path.Count - 1)); } }
        public bool IsEmpty { get { return (path.Count == 0); } }
        public bool HasPrefix { get { return (path.Count > 1); } }
        public bool IsNaked { get { return (path.Count == 1); } }

        public QualifiedName(string name)
        {
            path = new List<string>();
            path.Add(name);
        }
        public QualifiedName(List<string> path) { this.path = path; }

        public static QualifiedName ParseName(string completeName)
        {
            List<string> Path = new List<string>();
            int Begin = 0;
            int End = 0;
            while (End != completeName.Length)
            {
                if (completeName[End] == separatorChar)
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

        public override string ToString() { return ToString(separatorChar); }
        public string ToString(char separator)
        {
            if (path.Count == 0)
                return string.Empty;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder(path[0]);
            for (int i = 1; i != path.Count; i++)
            {
                Builder.Append(separator);
                Builder.Append(path[i]);
            }
            return Builder.ToString();
        }

        public static QualifiedName operator +(QualifiedName prefix, string localName)
        {
            List<string> Path = new List<string>();
            Path.AddRange(prefix.path);
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

    public class Namespace : Symbol
    {
        private string localName;
        private QualifiedName completeName;
        private Dictionary<string, Namespace> namespaces;

        public override string LocalName { get { return localName; } }
        public override QualifiedName CompleteName { get { return completeName; } }

        public Namespace(Namespace parent, string name) : base()
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
                throw new WrongParentSymbolException(this, symbol.GetType(),typeof(Namespace));
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
