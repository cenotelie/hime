namespace Hime.Kernel
{
    /// <summary>
    /// Base abstract class representing a naming exception
    /// </summary>
    public abstract class NamingException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the NamingException class
        /// </summary>
        public NamingException() : base() { }
        /// <summary>
        /// Initializes a new instance of the NamingException class with an error message
        /// </summary>
        /// <param name="message">Message describing the error</param>
        public NamingException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the NamingException class with an error message and a reference to the inner exception that causes the error
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="innerException">Inner exception causing the error</param>
        public NamingException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Represents a failure to resolve a name
    /// </summary>
    public class NameResolutionException : NamingException
    {
        /// <summary>
        /// Symbol from which the name was intended to be resolved
        /// </summary>
        private Symbol p_Origin;
        /// <summary>
        /// The name that failed to be resolved
        /// </summary>
        private QualifiedName p_Name;

        /// <summary>
        /// Get the symbol from which the name was intended to be resolved
        /// </summary>
        /// <value>Symbol from which the name was intended to be resolved</value>
        public Symbol Origin { get { return p_Origin; } }
        /// <summary>
        /// Get the name that failed to be resolved
        /// </summary>
        /// <value>The name that failed to be resolved</value>
        public QualifiedName Name { get { return p_Name; } }

        /// <summary>
        /// Initializes a new instance of the NamingResolutionException exception
        /// </summary>
        /// <param name="Origin">Symbol trying to resolve a name</param>
        /// <param name="Name">Name failing to be resolved</param>
        public NameResolutionException(Symbol origin, QualifiedName name) : base("Cannot resolve name " + name.ToString() + " from symbol " + origin.CompleteName.ToString())
        {
            p_Origin = origin;
            p_Name = name;
        }
    }

    /// <summary>
    /// Represents an error during an ambiguous named resolution
    /// </summary>
    public class AmbiguousNameException : NamingException
    {
        /// <summary>
        /// Symbol from which the name was intended to be resolved
        /// </summary>
        private Symbol p_Origin;
        /// <summary>
        /// The ambiguous name
        /// </summary>
        private QualifiedName p_Name;

        /// <summary>
        /// Get the symbol from which the name was intended to be resolved
        /// </summary>
        /// <value>Symbol from which the name was intended to be resolved</value>
        public Symbol Origin { get { return p_Origin; } }
        /// <summary>
        /// Get the ambiguous name
        /// </summary>
        /// <value>The ambiguous name</value>
        public QualifiedName Name { get { return p_Name; } }

        /// <summary>
        /// Initializes a new instance of the NamingAmbiguousException exception
        /// </summary>
        /// <param name="origin">Symbol trying to resolve a name</param>
        /// <param name="name">Ambiguous name</param>
        public AmbiguousNameException(Symbol origin, QualifiedName name) : base("Ambiguous name " + name.ToString() + " from symbol " + origin.CompleteName.ToString())
        {
            p_Origin = origin;
            p_Name = name;
        }
    }

    /// <summary>
    /// Represents a name collision error when adding a new name to a container
    /// </summary>
    public class NameCollisionException : NamingException
    {
        /// <summary>
        /// The container symbol
        /// </summary>
        private Symbol p_Container;
        /// <summary>
        /// The colliding name
        /// </summary>
        private string p_Name;

        /// <summary>
        /// Get the container symbol
        /// </summary>
        /// <value>The container symbol</value>
        public Symbol Container { get { return p_Container; } }
        /// <summary>
        /// Get the colliding name
        /// </summary>
        /// <value>The colliding name</value>
        public string Name { get { return p_Name; } }

        /// <summary>
        /// Initializes a new instance of the NamingCollisionException exception
        /// </summary>
        /// <param name="container">Container symbol</param>
        /// <param name="name">Colliding name</param>
        public NameCollisionException(Symbol container, string name) : base("Name collision in symbol " + container.CompleteName.ToString() + " for " + name)
        {
            p_Container = container;
            p_Name = name;
        }
    }

    /// <summary>
    /// Represents a error when setting the parent of a symbol with the wrong type
    /// </summary>
    public class WrongParentSymbolException : NamingException
    {
        /// <summary>
        /// Symbol expecting the parent
        /// </summary>
        private Symbol p_Current;
        /// <summary>
        /// Errornous type given as parent
        /// </summary>
        private System.Type p_GivenType;
        /// <summary>
        /// Expected type as parent
        /// </summary>
        private System.Type p_ExpectedType;

        /// <summary>
        /// Get the symbol expecting the parent
        /// </summary>
        public Symbol CurrentSymbol { get { return p_Current; } }
        /// <summary>
        /// Get the errornous type given as parent
        /// </summary>
        /// <value>Errornous type given as parent</value>
        public System.Type GivenType { get { return p_GivenType; } }
        /// <summary>
        /// Get the expected type
        /// </summary>
        /// <value>Expected type</value>
        public System.Type ExpectedType { get { return p_ExpectedType; } }

        /// <summary>
        /// Initializes a new instance of the NamingCollisionException exception
        /// </summary>
        /// <param name="current">Symbol expecting the parent</param>
        /// <param name="givenType">Errornous type</param>
        /// <param name="expectedType">Expected type</param>
        public WrongParentSymbolException(Symbol current, System.Type givenType, System.Type expectedType) : base("Wrong parent type for " + current.CompleteName.ToString() + "; expected " + expectedType.Name)
        {
            p_Current = current;
            p_GivenType = givenType;
            p_ExpectedType = expectedType;
        }
    }

    /// <summary>
    /// Represents an error when adding a child to a symbol
    /// </summary>
    public class CannotAddChildException : NamingException
    {
        /// <summary>
        /// Parent symbol
        /// </summary>
        private Symbol p_Parent;
        /// <summary>
        /// Child symbol
        /// </summary>
        private Symbol p_Child;

        /// <summary>
        /// Get the parent symbol
        /// </summary>
        /// <value>Parent symbol</value>
        public Symbol ParentSymbol { get { return p_Parent; } }
        /// <summary>
        /// Get the child symbol
        /// </summary>
        /// <value>Child symbol</value>
        public Symbol ChildSymbol { get { return p_Child; } }

        /// <summary>
        /// Initializes a new instance of the CannotAddChildException exception
        /// </summary>
        /// <param name="parent">Parent symbol</param>
        /// <param name="child">Child symbol</param>
        public CannotAddChildException(Symbol parent, Symbol child) : base("Cannot add " + child.LocalName + " in symbol " + parent.CompleteName.ToString())
        {
            p_Parent = parent;
            p_Child = child;
        }
    }
    


    /// <summary>
    /// Represents a name qualified by its enclosing symbols
    /// </summary>
    public class QualifiedName
    {
        /// <summary>
        /// Separator character between names
        /// </summary>
        private static char p_SeparatorChar = '.';
        /// <summary>
        /// Complete path for the name
        /// </summary>
        private System.Collections.Generic.List<string> p_Path;

        /// <summary>
        /// Get the separator used to separate local names in a path
        /// </summary>
        /// <value>Separator character between local names in a path</value>
        public static char Separator { get { return p_SeparatorChar; } }
        /// <summary>
        /// Get the first prefix (root local name)
        /// </summary>
        /// <value>The first prefix</value>
        public string PrefixOrder0 { get { return p_Path[0]; } }
        /// <summary>
        /// Get the name without the prefix
        /// </summary>
        /// <value>Local name without prefix</value>
        public string NakedName { get { return p_Path[p_Path.Count - 1]; } }
        /// <summary>
        /// Get the prefix
        /// </summary>
        /// <value>Prefix for the name</value>
        public QualifiedName Prefix { get { return new QualifiedName(p_Path.GetRange(0, p_Path.Count - 1)); } }
        /// <summary>
        /// Get the qualified name without the first prefix
        /// </summary>
        /// <value>Qualified name without the first prefix</value>
        public QualifiedName SubName { get { return new QualifiedName(p_Path.GetRange(1, p_Path.Count - 1)); } }
        /// <summary>
        /// Get a value indicating if the name is empty
        /// </summary>
        /// <value>True if the name is empty</value>
        public bool IsEmpty { get { return (p_Path.Count == 0); } }
        /// <summary>
        /// Get a value indicating if the name has a prefix
        /// </summary>
        /// <value>True if the name has a prefix</value>
        public bool HasPrefix { get { return (p_Path.Count > 1); } }
        /// <summary>
        /// Get a value indicating if the name is naked
        /// </summary>
        /// <value>True if the name is naked</value>
        public bool IsNaked { get { return (p_Path.Count == 1); } }

        /// <summary>
        /// Initializes a new instance of the QualifiedName class with a local name
        /// </summary>
        /// <param name="Name">Local name</param>
        public QualifiedName(string name)
        {
            p_Path = new System.Collections.Generic.List<string>();
            p_Path.Add(name);
        }
        /// <summary>
        /// Initializes a new instance of the QualifiedName class with a path
        /// </summary>
        /// <param name="Path">Path</param>
        public QualifiedName(System.Collections.Generic.List<string> path) { p_Path = path; }

        /// <summary>
        /// Creates a new qualified name from a string representation
        /// </summary>
        /// <param name="CompleteName">String representation of a qualified name</param>
        /// <returns>Returns the qualified name</returns>
        public static QualifiedName ParseName(string completeName)
        {
            System.Collections.Generic.List<string> Path = new System.Collections.Generic.List<string>();
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

        /// <summary>
        /// Creates a string representation of the name using the default separator in the path
        /// </summary>
        /// <returns>Returns a string representation of the name</returns>
        public override string ToString() { return ToString(p_SeparatorChar); }
        /// <summary>
        /// Creates a string representation of the name using the given separator in the path
        /// </summary>
        /// <param name="separator">Separator in the path</param>
        /// <returns>Returns a string representation of the name</returns>
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

        /// <summary>
        /// Concatenates a qualified name with a local name on the right
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <param name="localName">Local name</param>
        /// <returns>Returns a qualified name</returns>
        public static QualifiedName operator +(QualifiedName prefix, string localName)
        {
            System.Collections.Generic.List<string> Path = new System.Collections.Generic.List<string>();
            Path.AddRange(prefix.p_Path);
            Path.Add(localName);
            return new QualifiedName(Path);
        }
    }

    /// <summary>
    /// Represents the access to the symbol
    /// </summary>
    public enum SymbolAccess
    {
        /// <summary>
        /// Public access
        /// </summary>
        Public,
        /// <summary>
        /// Protected access
        /// </summary>
        Protected,
        /// <summary>
        /// Private access
        /// </summary>
        Private,
        /// <summary>
        /// Internal access
        /// </summary>
        Internal
    }

    /// <summary>
    /// Represents a symbol
    /// </summary>
    public abstract class Symbol
    {
        /// <summary>
        /// Symbol's access modifier
        /// </summary>
        protected SymbolAccess p_Access;
        /// <summary>
        /// Children symbols' dictionary
        /// </summary>
        protected System.Collections.Generic.Dictionary<string, Symbol> p_Children;

        /// <summary>
        /// Get the parent symbol
        /// </summary>
        public abstract Symbol Parent { get; }
        /// <summary>
        /// Get the local (naked) name
        /// </summary>
        public abstract string LocalName { get; }
        /// <summary>
        /// Get the complete (fully qualified) name
        /// </summary>
        public abstract QualifiedName CompleteName { get; }
        /// <summary>
        /// Get or set the access modifier for the current symbol
        /// </summary>
        public SymbolAccess Access
        {
            get { return p_Access; }
            set { p_Access = value; }
        }
        /// <summary>
        /// Get an enumeration of the children symbols
        /// </summary>
        public System.Collections.Generic.IEnumerable<Symbol> Children { get { return p_Children.Values; } }

        /// <summary>
        /// Initializes a new instance of the Symbol class
        /// </summary>
        public Symbol()
        {
            p_Children = new System.Collections.Generic.Dictionary<string, Symbol>();
            p_Access = SymbolAccess.Public;
        }

        /// <summary>
        /// Sets the parent symbol
        /// </summary>
        /// <param name="Symbol">New parent symbol</param>
        protected abstract void SymbolSetParent(Symbol symbol);
        /// <summary>
        /// Sets the complete name value
        /// </summary>
        /// <param name="Name">Complete name</param>
        protected abstract void SymbolSetCompleteName(QualifiedName name);

        /// <summary>
        /// Adds the given symbol as a child
        /// </summary>
        /// <param name="Symbol">New child</param>
        /// <remarks>Linkage is done by this function</remarks>
        public virtual void SymbolAddChild(Symbol child)
        {
            if (p_Children.ContainsKey(child.LocalName))
                throw new NameCollisionException(this, child.LocalName);
            child.SymbolSetParent(this);
            child.SymbolSetCompleteName(CompleteName + child.LocalName);
            p_Children.Add(child.LocalName, child);
        }

        /// <summary>
        /// Resolves the given qualified name
        /// </summary>
        /// <param name="Name">Name to resolve</param>
        /// <returns>Returns the symbol corresponding to the name</returns>
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
        /// <summary>
        /// Recursively match the name in children symbols
        /// </summary>
        /// <param name="Name">Name to match</param>
        /// <returns>Returns the symbol if found, null otherwise</returns>
        protected Symbol ResolveName_DeepCheck(QualifiedName name)
        {
            if (name.IsEmpty)
                return this;
            if (p_Children.ContainsKey(name.PrefixOrder0))
                return p_Children[name.PrefixOrder0].ResolveName_DeepCheck(name.SubName);
            return null;
        }

        /// <summary>
        /// Returns a string representation of the symbol
        /// </summary>
        /// <returns>Returns the local name</returns>
        public override string ToString() { return this.LocalName; }
    }

    /// <summary>
    /// Represents a namespace
    /// </summary>
    public class Namespace : Symbol
    {
        /// <summary>
        /// Parent (enclosing) namespace
        /// </summary>
        private Namespace p_Parent;
        /// <summary>
        /// Local name
        /// </summary>
        private string p_LocalName;
        /// <summary>
        /// Complete name
        /// </summary>
        private QualifiedName p_CompleteName;
        /// <summary>
        /// Children namespaces
        /// </summary>
        private System.Collections.Generic.Dictionary<string, Namespace> p_Namespaces;

        /// <summary>
        /// Get the parent symbol
        /// </summary>
        /// <value>Parent symbol</value>
        public override Symbol Parent { get { return p_Parent; } }
        /// <summary>
        /// Get the local (naked) name
        /// </summary>
        /// <value>Local name</value>
        public override string LocalName { get { return p_LocalName; } }
        /// <summary>
        /// Get the complete (fully qualified) name
        /// </summary>
        /// <value>Complete name with prefix</value>
        public override QualifiedName CompleteName { get { return p_CompleteName; } }

        /// <summary>
        /// Constructs the namespace with given parent and local name
        /// </summary>
        /// <param name="parent">Parent namespace (may be null)</param>
        /// <param name="name">Local name for the namespace</param>
        public Namespace(Namespace parent, string name) : base()
        {
            p_Parent = parent;
            p_LocalName = name;
            if (p_Parent == null)
                p_CompleteName = new QualifiedName(new System.Collections.Generic.List<string>());
            else
                p_CompleteName = p_Parent.p_CompleteName + p_LocalName;
            p_Namespaces = new System.Collections.Generic.Dictionary<string, Namespace>();
        }

        /// <summary>
        /// Create a new root namespace
        /// </summary>
        /// <returns>Returns a root namespace</returns>
        public static Namespace CreateRoot()
        {
            return new Namespace(null, "global");
        }

        /// <summary>
        /// Adds the given symbol as a child
        /// </summary>
        /// <param name="Symbol">New child</param>
        /// <remarks>Linkage is done by this function</remarks>
        public override void SymbolAddChild(Symbol child)
        {
            base.SymbolAddChild(child);
            if (child is Namespace)
                p_Namespaces.Add(child.LocalName, (Namespace)child);
        }
        /// <summary>
        /// Sets the parent symbol
        /// </summary>
        /// <param name="Symbol">New parent symbol</param>
        protected override void SymbolSetParent(Symbol symbol)
        {
            if (!(symbol is Namespace))
            {
                throw new WrongParentSymbolException(this, symbol.GetType(),typeof(Namespace));
            }
            this.p_Parent = (Namespace)symbol;
        }
        /// <summary>
        /// Sets the complete name value
        /// </summary>
        /// <param name="Name">Complete name</param>
        protected override void SymbolSetCompleteName(QualifiedName name) { p_CompleteName = name; }

        /// <summary>
        /// Create a sub-namespace of the given name if needed
        /// </summary>
        /// <param name="Name">The name for the sub-namespace</param>
        /// <returns>Returns the sub-namespace</returns>
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