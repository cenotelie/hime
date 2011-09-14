/*
 * Author: Charles Hymans
 * Date: 21/07/2011
 * Time: 22:24
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
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

        public Naming.Namespace OutputRootNamespace { get; private set; }
        public string CompilerName { get { return "HimeSystems.CentralDogma Compiler"; } }
        public int CompilerVersionMajor { get { return 1; } }
        public int CompilerVersionMinor { get { return 0; } }
        ResourceCompilerRegister PluginRegister { get { return pluginRegister; } }

        public ResourceCompiler(Reporter reporter)
        {
            inputNamedResources = new Dictionary<string, TextReader>();
            inputAnonResources = new List<TextReader>();
            outputErrors = new List<CompilerError>();
            intermediateRoot = new Redist.Parsers.SyntaxTreeNode(null);
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
            foreach (Redist.Parsers.SyntaxTreeNode file in intermediateRoot.Children)
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
                foreach (Resource R in intermediateResources.Resources)
                {
                    if (R.IsCompiled) continue;
                    solved += R.Compiler.CompileSolveDependencies(R, outputLog);
                    unsolved += R.Dependencies.Count;
                    if (R.Dependencies.Count == 0)
                        if (!R.Compiler.Compile(R, outputLog))
                            hasErrors = true;
                }
                // FIXME: what the hell is this ?
                if (solved == 0)
                {  }
            }
            outputLog.EndSection();
            return (!hasErrors);
        }



        public static Naming.QualifiedName CompileQualifiedName(Redist.Parsers.SyntaxTreeNode node)
        {
            List<string> path = new List<string>();
            foreach (Redist.Parsers.SyntaxTreeNode Child in node.Children)
                path.Add(((Redist.Parsers.SymbolTokenText)Child.Symbol).ValueText);
            return new Naming.QualifiedName(path);
        }
        public static Naming.SymbolAccess CompileSymbolAccess(Redist.Parsers.SyntaxTreeNode node)
        {
            if (node.Symbol.Name == "access_internal") return Naming.SymbolAccess.Internal;
            else if (node.Symbol.Name == "access_public") return Naming.SymbolAccess.Public;
            else if (node.Symbol.Name == "access_protected") return Naming.SymbolAccess.Protected;
            else if (node.Symbol.Name == "access_private") return Naming.SymbolAccess.Private;
            return Naming.SymbolAccess.Public;
        }
        
		// TODO: made this method public for test, but maybe this is a sign of not optimal architecture, think about it
        public void CompileData(TextReader input)
        {
            Parser.FileCentralDogma_Lexer UnitLexer = new Parser.FileCentralDogma_Lexer(input);
            Parser.FileCentralDogma_Parser UnitParser = new Parser.FileCentralDogma_Parser(UnitLexer);
            // TODO: rewrite this code, it is a bit strange
            Redist.Parsers.SyntaxTreeNode root = null;
            try { root = UnitParser.Analyse(); }
            catch (System.Exception e) {
                outputLog.Fatal("Parser", "encountered a fatal error. Exception thrown: " + e.Message);
                return;
            }

            foreach (Redist.Parsers.LexerTextError Error in UnitLexer.Errors)
                outputLog.Report(new Reporting.BaseEntry(Reporting.ELevel.Error, "Lexer", Error.Message));
            foreach (Redist.Parsers.ParserError Error in UnitParser.Errors)
                outputLog.Report(new Reporting.BaseEntry(Reporting.ELevel.Error, "Parser", Error.Message));

            if (root == null)
            {
                outputLog.Error("Parser", "encountered an unrecoverable error.");
                return;
            }
            intermediateRoot.AppendChild(root);
        }


        private void Compile_file(Redist.Parsers.SyntaxTreeNode node)
        {
            foreach (Redist.Parsers.SyntaxTreeNode child in node.Children)
            {
                if (child.Symbol.Name == "Namespace")
                    Compile_namespace(child, this.OutputRootNamespace);
                else
                {
                    IResourceCompiler Compiler = pluginRegister.GetCompilerFor(child.Symbol.Name);
                    if (Compiler == null)
                        throw new NoResourceCompilerFoundException("Missing compiler for resource " + child.Symbol.Name);
                    Compiler.CreateResource(this.OutputRootNamespace, child, intermediateResources, outputLog);
                }
            }
        }
        private void Compile_namespace(Redist.Parsers.SyntaxTreeNode node, Naming.Namespace currentNamespace)
        {
            Naming.QualifiedName Name = CompileQualifiedName(node.Children[0]);
            currentNamespace = currentNamespace.AddSubNamespace(Name);
            foreach (Redist.Parsers.SyntaxTreeNode Child in node.Children[1].Children)
            {
                if (Child.Symbol.Name == "Namespace")
                    Compile_namespace(Child, currentNamespace);
                else
                {
                    IResourceCompiler Compiler = pluginRegister.GetCompilerFor(Child.Symbol.Name);
                    if (Compiler == null)
                        throw new NoResourceCompilerFoundException("Missing compiler for resource " + Child.Symbol.Name);
                    Compiler.CreateResource(currentNamespace, Child, intermediateResources, outputLog);
                }
            }
        }
    }
}
