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

namespace Hime.Kernel.Resources
{
	public class ResourceCompiler
    {
        private Dictionary<string, TextReader> inputNamedResources;
        private List<TextReader> inputAnonResources;
        private List<CompilerError> outputErrors;
        private Reporter outputLog;
        private SyntaxTreeNode intermediateRoot;
        private ResourceGraph intermediateResources;
        private ResourceCompilerRegister pluginRegister;

        public Namespace OutputRootNamespace { get; private set; }
        public string CompilerName { get { return "HimeSystems.CentralDogma Compiler"; } }
        public int CompilerVersionMajor { get { return 1; } }
        public int CompilerVersionMinor { get { return 0; } }
        ResourceCompilerRegister PluginRegister { get { return pluginRegister; } }

        public ResourceCompiler(Reporter reporter)
        {
            inputNamedResources = new Dictionary<string, TextReader>();
            inputAnonResources = new List<TextReader>();
            outputErrors = new List<CompilerError>();
            intermediateRoot = new SyntaxTreeNode(null);
            intermediateResources = new ResourceGraph();
            pluginRegister = new ResourceCompilerRegister();
            pluginRegister.RegisterCompiler(new Hime.Parsers.ContextFree.CFGrammarCompiler());
            this.OutputRootNamespace = new Naming.Namespace(null, "global");
            this.outputLog = reporter;
        }

        public void AddInput(TextReader input)
        {
            inputAnonResources.Add(input);
        }
        public void AddInput(TextReader input, string name)
        {
            inputNamedResources.Add(name, input);
        }

        public bool Compile()
        {
        	// TODO: simplify: this is not really necessary because of the reporter?
        	// TODO: make a test for the case where hasErrors and reporter.HasErrors do not coincide
            bool hasErrors = false;

            outputLog.BeginSection("Compiler " + CompilerName);
            outputLog.Info("Compiler", CompilerName + " " + CompilerVersionMajor.ToString() + "." + CompilerVersionMinor.ToString());
            foreach (IResourceCompiler Plugin in pluginRegister.Compilers)
                outputLog.Info("Compiler", "Register plugin " + Plugin.ToString());
            foreach (string ResourceName in inputNamedResources.Keys)
                outputLog.Info("Compiler", "Compilation unit " + ResourceName);
            if (inputAnonResources.Count != 0)
                outputLog.Info("Compiler", "Compilation unit " + inputAnonResources.Count.ToString() + " raw resources");

            // Parse
            // TODO: find a way to merge these two lines
            foreach (string resourceName in inputNamedResources.Keys)
                CompileData(inputNamedResources[resourceName]);
            foreach (TextReader data in inputAnonResources)
                CompileData(data);

            // Build resources
            foreach (SyntaxTreeNode file in intermediateRoot.Children)
                Compile_file(file);

            // Build dependencies
            foreach (Resource resource in intermediateResources.Resources)
                resource.Compiler.CreateDependencies(resource, intermediateResources, outputLog);

            // Solve dependencies and compile
            int unsolved = 1;
            while (unsolved != 0)
            {
                unsolved = 0;
                int solved = 0;
                foreach (Resource resource in intermediateResources.Resources)
                {
                    if (resource.IsCompiled) continue;
                    solved += resource.Compiler.CompileSolveDependencies(resource, outputLog);
                    unsolved += resource.Dependencies.Count;
                    if (resource.Dependencies.Count == 0)
                        if (!resource.Compiler.Compile(resource, outputLog))
                            hasErrors = true;
                }
                // FIXME: what the hell is this ?
                if (solved == 0)
                {  }
            }
            outputLog.EndSection();
            return (!hasErrors);
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
            Parser.FileCentralDogma_Lexer lexer = new Parser.FileCentralDogma_Lexer(input);
            Parser.FileCentralDogma_Parser parser = new Parser.FileCentralDogma_Parser(lexer);
            // TODO: rewrite this code, it is a bit strange
            SyntaxTreeNode root = null;
            try { root = parser.Analyse(); }
            catch (System.Exception e) {
                outputLog.Fatal("Parser", "encountered a fatal error. Exception thrown: " + e.Message);
                return;
            }

            foreach (LexerTextError error in lexer.Errors)
                outputLog.Report(new BaseEntry(ELevel.Error, "Lexer", error.Message));
            foreach (ParserError error in parser.Errors)
                outputLog.Report(new BaseEntry(ELevel.Error, "Parser", error.Message));

            if (root == null)
            {
                outputLog.Error("Parser", "encountered an unrecoverable error.");
                return;
            }
            intermediateRoot.AppendChild(root);
        }


        private void Compile_file(SyntaxTreeNode node)
        {
            foreach (SyntaxTreeNode child in node.Children)
            {
                if (child.Symbol.Name == "Namespace")
                    Compile_namespace(child, this.OutputRootNamespace);
                else
                {
                    IResourceCompiler plugin = pluginRegister.GetCompilerFor(child.Symbol.Name);
                    if (plugin == null)
                        throw new NoResourceCompilerFoundException("Missing compiler for resource " + child.Symbol.Name);
                    plugin.CreateResource(this.OutputRootNamespace, child, intermediateResources, outputLog);
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
                    IResourceCompiler plugin = pluginRegister.GetCompilerFor(child.Symbol.Name);
                    if (plugin == null)
                        throw new NoResourceCompilerFoundException("Missing compiler for resource " + child.Symbol.Name);
                    plugin.CreateResource(currentNamespace, child, intermediateResources, outputLog);
                }
            }
        }
    }
}
