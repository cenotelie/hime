using System.Collections.Generic;

namespace Hime.Kernel.Resources
{
    class Resource
    {
        private Symbol symbol;
        private Redist.Parsers.SyntaxTreeNode syntaxNode;
        private List<KeyValuePair<string, Resource>> dependencies;
        private IResourceCompiler compiler;
        private bool isCompiled;

        public Symbol Symbol { get { return symbol; } }
        public Redist.Parsers.SyntaxTreeNode SyntaxNode { get { return syntaxNode; } }
        public List<KeyValuePair<string, Resource>> Dependencies { get { return dependencies; } }
        public IResourceCompiler Compiler { get { return compiler; } }
        public bool IsCompiled
        {
            get { return isCompiled; }
            set { isCompiled = value; }
        }

        public Resource(Symbol Symbol, Redist.Parsers.SyntaxTreeNode SyntaxNode, IResourceCompiler Compiler)
        {
            symbol = Symbol;
            syntaxNode = SyntaxNode;
            dependencies = new List<KeyValuePair<string, Resource>>();
            compiler = Compiler;
            isCompiled = false;
        }

        public void AddDependency(string Tag, Resource Resource)
        {
            dependencies.Add(new KeyValuePair<string, Resource>(Tag, Resource));
        }
    }


    class ResourceGraph
    {
        private Dictionary<Symbol, Resource> resources;

        public ICollection<Resource> Resources { get { return resources.Values; } }

        public ResourceGraph()
        {
            resources = new Dictionary<Symbol, Resource>();
        }

        public void AddResource(Resource Resource) { resources.Add(Resource.Symbol, Resource); }
        public Resource GetResource(Symbol Symbol) { return resources[Symbol]; }
    }
}
