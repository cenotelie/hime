/*
 * Author: Charles Hymans
 * Date: 31/07/2011
 * Time: 14:43
 * 
 */
using System;
using System.Collections.Generic;
using Hime.Kernel.Naming;
using Hime.Kernel.Reporting;
using Hime.Redist.Parsers;

namespace Hime.Kernel.Resources
{
    class Resource
    {
        private Naming.Symbol symbol;
        private SyntaxTreeNode syntaxNode;
        private List<KeyValuePair<string, Resource>> dependencies;
        private LoaderPlugin loader;
        private bool isLoaded;

        public Naming.Symbol Symbol { get { return symbol; } }
        public SyntaxTreeNode SyntaxNode { get { return syntaxNode; } }
        public List<KeyValuePair<string, Resource>> Dependencies { get { return dependencies; } }
        public LoaderPlugin Loader { get { return loader; } }
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }

        public Resource(Naming.Symbol symbol, SyntaxTreeNode syntaxNode, LoaderPlugin loader)
        {
            this.symbol = symbol;
            this.syntaxNode = syntaxNode;
            this.dependencies = new List<KeyValuePair<string, Resource>>();
            this.loader = loader;
            this.isLoaded = false;
        }

        public void AddDependency(string tag, Resource resource)
        {
            dependencies.Add(new KeyValuePair<string, Resource>(tag, resource));
        }
        
        public void CreateDependencies(ResourceGraph graph, Reporter logger)
        {
        	this.Loader.CreateDependencies(this, graph, logger);
        }
    }
}
