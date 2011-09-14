/*
 * Author: Charles Hymans
 * Date: 31/07/2011
 * Time: 14:43
 * 
 */
using System;
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Kernel.Resources
{
    class Resource
    {
        private Hime.Kernel.Naming.Symbol symbol;
        private Redist.Parsers.SyntaxTreeNode syntaxNode;
        private List<KeyValuePair<string, Resource>> dependencies;
        private IResourceCompiler compiler;
        private bool isCompiled;

        public Hime.Kernel.Naming.Symbol Symbol { get { return symbol; } }
        public Redist.Parsers.SyntaxTreeNode SyntaxNode { get { return syntaxNode; } }
        public List<KeyValuePair<string, Resource>> Dependencies { get { return dependencies; } }
        public IResourceCompiler Compiler { get { return compiler; } }
        public bool IsCompiled
        {
            get { return isCompiled; }
            set { isCompiled = value; }
        }

        public Resource(Naming.Symbol Symbol, Redist.Parsers.SyntaxTreeNode SyntaxNode, IResourceCompiler Compiler)
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
        
        public void CreateDependencies(ResourceGraph graph, Reporter logger)
        {
        	this.Compiler.CreateDependencies(this, graph, logger);
        }
    }
}
