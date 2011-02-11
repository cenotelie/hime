namespace Hime.Kernel.Resources
{
    public sealed class Resource
    {
        private Symbol p_Symbol;
        private Redist.Parsers.SyntaxTreeNode p_SyntaxNode;
        private System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, Resource>> p_Dependencies;
        private IResourceCompiler p_Compiler;
        private bool p_IsCompiled;

        public Symbol Symbol { get { return p_Symbol; } }
        public Redist.Parsers.SyntaxTreeNode SyntaxNode { get { return p_SyntaxNode; } }
        public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, Resource>> Dependencies { get { return p_Dependencies; } }
        public IResourceCompiler Compiler { get { return p_Compiler; } }
        public bool IsCompiled
        {
            get { return p_IsCompiled; }
            set { p_IsCompiled = value; }
        }

        public Resource(Symbol Symbol, Redist.Parsers.SyntaxTreeNode SyntaxNode, IResourceCompiler Compiler)
        {
            p_Symbol = Symbol;
            p_SyntaxNode = SyntaxNode;
            p_Dependencies = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, Resource>>();
            p_Compiler = Compiler;
            p_IsCompiled = false;
        }

        public void AddDependency(string Tag, Resource Resource)
        {
            p_Dependencies.Add(new System.Collections.Generic.KeyValuePair<string, Resource>(Tag, Resource));
        }
    }


    public sealed class ResourceGraph
    {
        private System.Collections.Generic.Dictionary<Symbol, Resource> p_Resources;

        public System.Collections.Generic.IEnumerable<Resource> Resources { get { return p_Resources.Values; } }

        public ResourceGraph()
        {
            p_Resources = new System.Collections.Generic.Dictionary<Symbol, Resource>();
        }

        public void AddResource(Resource Resource) { p_Resources.Add(Resource.Symbol, Resource); }
        public Resource GetResource(Symbol Symbol) { return p_Resources[Symbol]; }
    }
}