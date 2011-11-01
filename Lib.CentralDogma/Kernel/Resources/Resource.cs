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
    internal class Resource
    {
        internal Naming.Symbol Symbol { get; private set; }
        internal SyntaxTreeNode SyntaxNode { get; private set; }
        internal List<KeyValuePair<string, Resource>> Dependencies { get; private set; }
        internal LoaderPlugin Loader { get; private set; }
		
        internal bool IsLoaded { get; set; }

        internal Resource(Naming.Symbol symbol, SyntaxTreeNode syntaxNode, LoaderPlugin loader)
        {
            this.Symbol = symbol;
            this.SyntaxNode = syntaxNode;
            this.Dependencies = new List<KeyValuePair<string, Resource>>();
            this.Loader = loader;
            this.IsLoaded = false;
        }

        internal void AddDependency(string tag, Resource resource)
        {
            this.Dependencies.Add(new KeyValuePair<string, Resource>(tag, resource));
        }
    }
}
