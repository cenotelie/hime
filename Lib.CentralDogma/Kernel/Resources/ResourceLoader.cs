/*
 * Author: Charles Hymans
 * Date: 21/07/2011
 * Time: 22:24
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using Hime.Kernel.Naming;
using Hime.Kernel.Reporting;
using Hime.Redist.Parsers;
using Hime.Kernel.Resources.Parser;

namespace Hime.Kernel.Resources
{
	// TODO: think about it, but maybe this class is doing too many things?
	class ResourceLoader
    {
        private static Dictionary<string, LoaderPlugin> defaultPlugins = new Dictionary<string, LoaderPlugin>();
        internal static void RegisterPlugin(LoaderPlugin plugin)
        {
            for (int i = 0; i != plugin.ResourceNames.Length; i++)
                defaultPlugins.Add(plugin.ResourceNames[i], plugin);
        }

        private Dictionary<string, LoaderPlugin> plugins;
        private Dictionary<string, TextReader> inputNamedResources;
        private List<TextReader> inputAnonResources;
        private Reporter log;
        private SyntaxTreeNode intermediateRoot;
        private ResourceGraph intermediateResources;

        public Dictionary<string, LoaderPlugin> Plugins { get { return plugins; } }
        private Namespace outputRootNamespace;
        
        public ResourceLoader(Reporter reporter)
        {
            plugins = new Dictionary<string, LoaderPlugin>(defaultPlugins);
            inputNamedResources = new Dictionary<string, TextReader>();
            inputAnonResources = new List<TextReader>();
            intermediateRoot = new SyntaxTreeNode(null);
            intermediateResources = new ResourceGraph();
            this.outputRootNamespace = new Naming.Namespace(null, "global");
            this.log = reporter;
        }

        public void AddInput(TextReader input)
        {
            inputAnonResources.Add(input);
        }
        public void AddInput(TextReader input, string name)
        {
            inputNamedResources.Add(name, input);
        }

        public Namespace Load()
        {
        	// TODO: simplify: this is not really necessary because of the reporter?
        	// TODO: make a test for the case where hasErrors and reporter.HasErrors do not coincide
            bool hasErrors = false;
            log.Info("Loader", "CentralDogma " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            foreach (string name in plugins.Keys)
                log.Info("Loader", "Register plugin " + plugins[name].ToString() + " for " + name);

            // Parse
            // TODO: find a way to merge these two lines
            foreach (string resourceName in inputNamedResources.Keys)
            {
                log.Info("Loader", "Loading compilation unit " + resourceName);
                CompileData(inputNamedResources[resourceName]);
            }
            foreach (TextReader data in inputAnonResources)
            {
                log.Info("Loader", "Loading compilation unit from raw resources");
                CompileData(data);
            }

            // Build resources
            foreach (SyntaxTreeNode file in intermediateRoot.Children)
                Compile_file(file);

            // Build dependencies
            foreach (Resource resource in intermediateResources.Resources)
                resource.Loader.CreateDependencies(resource, intermediateResources, log);

            // Solve dependencies and compile
            int unsolved = 1;
            while (unsolved != 0)
            {
                unsolved = 0;
                int solved = 0;
                foreach (Resource resource in intermediateResources.Resources)
                {
                    if (resource.IsLoaded) continue;
                    solved += resource.Loader.CompileSolveDependencies(resource, log);
                    unsolved += resource.Dependencies.Count;
                    if (resource.Dependencies.Count == 0)
                        if (!resource.Loader.Compile(resource, log))
                            hasErrors = true;
                }
                if (unsolved != 0 && solved == 0)
                {
                    log.Fatal("Loader", "Unable to solve all resource depedencies!");
                    hasErrors = true;
                    break;
                }
            }
			if (hasErrors) return null;
			return this.outputRootNamespace;
        }



        public static QualifiedName CompileQualifiedName(SyntaxTreeNode node)
        {
            List<string> path = new List<string>();
            foreach (SyntaxTreeNode child in node.Children)
                path.Add(((SymbolTokenText)child.Symbol).ValueText);
            return new QualifiedName(path);
        }
        public static SymbolAccess CompileSymbolAccess(SyntaxTreeNode node)
        {
            if (node.Symbol.Name == "access_internal") return SymbolAccess.Internal;
            else if (node.Symbol.Name == "access_public") return SymbolAccess.Public;
            else if (node.Symbol.Name == "access_protected") return SymbolAccess.Protected;
            else if (node.Symbol.Name == "access_private") return SymbolAccess.Private;
            return SymbolAccess.Public;
        }
        
		// TODO: made this method public for test, but maybe this is a sign of not optimal architecture, think about it
        public void CompileData(TextReader input)
        {
            FileCentralDogmaLexer lexer = new FileCentralDogmaLexer(input);
            FileCentralDogmaParser parser = new FileCentralDogmaParser(lexer);
            // TODO: rewrite this code, it is a bit strange
            SyntaxTreeNode root = null;
            try { root = parser.Analyse(); }
            catch (System.Exception e) {
                log.Fatal("Parser", "encountered a fatal error. Exception thrown: " + e.Message);
                return;
            }

            foreach (ParserError error in parser.Errors)
                log.Report(new BaseEntry(ELevel.Error, "Parser", error.Message));

            if (root == null)
            {
                log.Error("Parser", "encountered an unrecoverable error.");
                return;
            }
            intermediateRoot.AppendChild(root);
        }


        private void Compile_file(SyntaxTreeNode node)
        {
            foreach (SyntaxTreeNode child in node.Children)
            {
                if (child.Symbol.Name == "Namespace")
                    Compile_namespace(child, this.outputRootNamespace);
                else
                {
                    LoaderPlugin plugin = GetPluginFor(child.Symbol.Name);
                    plugin.CreateResource(this.outputRootNamespace, child, intermediateResources, log);
                }
            }
        }
        private void Compile_namespace(Redist.Parsers.SyntaxTreeNode node, Naming.Namespace currentNamespace)
        {
            QualifiedName name = CompileQualifiedName(node.Children[0]);
            currentNamespace = currentNamespace.AddSubNamespace(name);
            foreach (SyntaxTreeNode child in node.Children[1].Children)
            {
                if (child.Symbol.Name == "Namespace")
                    Compile_namespace(child, currentNamespace);
                else
                {
                    LoaderPlugin plugin = GetPluginFor(child.Symbol.Name);
                    plugin.CreateResource(currentNamespace, child, intermediateResources, log);
                }
            }
        }

        private LoaderPlugin GetPluginFor(string name)
        {
            if (plugins.ContainsKey(name))
                return plugins[name];
            throw new NoResourceLoaderFoundException("Missing loader for resource " + name);
        }
    }
}
