using System.Collections.Generic;

namespace Hime.Kernel.Resources
{
    class Resource
    {
        private Symbol p_Symbol;
        private Redist.Parsers.SyntaxTreeNode p_SyntaxNode;
        private List<KeyValuePair<string, Resource>> p_Dependencies;
        private IResourceCompiler p_Compiler;
        private bool p_IsCompiled;

        public Symbol Symbol { get { return p_Symbol; } }
        public Redist.Parsers.SyntaxTreeNode SyntaxNode { get { return p_SyntaxNode; } }
        public List<KeyValuePair<string, Resource>> Dependencies { get { return p_Dependencies; } }
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
            p_Dependencies = new List<KeyValuePair<string, Resource>>();
            p_Compiler = Compiler;
            p_IsCompiled = false;
        }

        public void AddDependency(string Tag, Resource Resource)
        {
            p_Dependencies.Add(new KeyValuePair<string, Resource>(Tag, Resource));
        }
    }


    class ResourceGraph
    {
        private Dictionary<Symbol, Resource> p_Resources;

        public ICollection<Resource> Resources { get { return p_Resources.Values; } }

        public ResourceGraph()
        {
            p_Resources = new Dictionary<Symbol, Resource>();
        }

        public void AddResource(Resource Resource) { p_Resources.Add(Resource.Symbol, Resource); }
        public Resource GetResource(Symbol Symbol) { return p_Resources[Symbol]; }
    }
}
