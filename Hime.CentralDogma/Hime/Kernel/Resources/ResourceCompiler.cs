/*
 * Author: Charles Hymans
 * Date: 21/07/2011
 * Time: 22:24
 * 
 */
using System;
using System.Collections.Generic;
using Hime.Kernel.Reporting;
using Hime.Redist.Parsers;

namespace Hime.Kernel.Resources
{
	public class ResourceCompiler
    {
        private Dictionary<string, string> inputNamedResources;
        private List<string> inputRawResources;
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

        public ResourceCompiler(Reporter outputLog)
        {
            inputNamedResources = new Dictionary<string, string>();
            inputRawResources = new List<string>();
            outputErrors = new List<CompilerError>();
            intermediateRoot = new Redist.Parsers.SyntaxTreeNode(null);
            intermediateResources = new ResourceGraph();
            pluginRegister = new ResourceCompilerRegister();
            pluginRegister.RegisterCompiler(new Hime.Parsers.CF.CFGrammarCompiler());
            this.OutputRootNamespace = new Namespace(null, "global");
            this.outputLog = outputLog;
        }

        public bool AddInputFile(string FileName)
        {
            string Data = null;
            try { Data = System.IO.File.ReadAllText(FileName); }
            catch { return false; }
            inputNamedResources.Add(FileName, Data);
            return true;
        }
        
		// TODO: instead of having AddInputRawText and AddInputFile, should have only one method
		// on a generic input (a stream??)
        public void AddInputRawText(string Data)
        {
            inputRawResources.Add(Data);
        }

        public bool Compile()
        {
        	// TODO: simplify: this is not really necessary because of the reporter?
            bool hasErrors = false;

            outputLog.BeginSection("Compiler " + CompilerName);
            outputLog.Info("Compiler", CompilerName + " " + CompilerVersionMajor.ToString() + "." + CompilerVersionMinor.ToString());
            foreach (IResourceCompiler Plugin in pluginRegister.Compilers)
                outputLog.Info("Compiler", "Register plugin " + Plugin.ToString());
            foreach (string ResourceName in inputNamedResources.Keys)
                outputLog.Info("Compiler", "Compilation unit " + ResourceName);
            if (inputRawResources.Count != 0)
                outputLog.Info("Compiler", "Compilation unit " + inputRawResources.Count.ToString() + " raw resources");

            // Parse
            foreach (string ResourceName in inputNamedResources.Keys)
                CompileData(inputNamedResources[ResourceName]);
            foreach (string Data in inputRawResources)
                CompileData(Data);

            // Build resources
            foreach (Redist.Parsers.SyntaxTreeNode file in intermediateRoot.Children)
                Compile_file(file);

            // Build dependencies
            foreach (Resource R in intermediateResources.Resources)
                R.Compiler.CreateDependencies(R, intermediateResources, outputLog);

            // Solve dependencies and compile
            int Unsolved = 1;
            while (Unsolved != 0)
            {
                Unsolved = 0;
                int Solved = 0;
                foreach (Resource R in intermediateResources.Resources)
                {
                    if (R.IsCompiled) continue;
                    Solved += R.Compiler.CompileSolveDependencies(R, outputLog);
                    Unsolved += R.Dependencies.Count;
                    if (R.Dependencies.Count == 0)
                        if (!R.Compiler.Compile(R, outputLog))
                            hasErrors = true;
                }
                if (Solved == 0)
                {  }
            }
            outputLog.EndSection();
            return (!hasErrors);
        }



        public static QualifiedName CompileQualifiedName(Redist.Parsers.SyntaxTreeNode Node)
        {
            List<string> Path = new List<string>();
            foreach (Redist.Parsers.SyntaxTreeNode Child in Node.Children)
                Path.Add(((Redist.Parsers.SymbolTokenText)Child.Symbol).ValueText);
            return new QualifiedName(Path);
        }
        public static SymbolAccess CompileSymbolAccess(Redist.Parsers.SyntaxTreeNode Node)
        {
            if (Node.Symbol.Name == "access_internal") return SymbolAccess.Internal;
            else if (Node.Symbol.Name == "access_public") return SymbolAccess.Public;
            else if (Node.Symbol.Name == "access_protected") return SymbolAccess.Protected;
            else if (Node.Symbol.Name == "access_private") return SymbolAccess.Private;
            return SymbolAccess.Public;
        }


        private bool CompileData(string Data)
        {
            bool IsError = false;
            Parser.FileCentralDogma_Lexer UnitLexer = new Parser.FileCentralDogma_Lexer(Data);
            Parser.FileCentralDogma_Parser UnitParser = new Parser.FileCentralDogma_Parser(UnitLexer);
            Redist.Parsers.SyntaxTreeNode UnitRoot = null;
            try { UnitRoot = UnitParser.Analyse(); }
            catch (System.Exception e) {
                outputLog.Fatal("Parser", "encountered a fatal error. Exception thrown: " + e.Message);
                return false;
            }

            foreach (Redist.Parsers.LexerTextError Error in UnitLexer.Errors)
            {
                outputLog.Report(new Reporting.BaseEntry(Reporting.Level.Error, "Lexer", Error.Message));
                IsError = true;
            }
            foreach (Redist.Parsers.ParserError Error in UnitParser.Errors)
            {
                outputLog.Report(new Reporting.BaseEntry(Reporting.Level.Error, "Parser", Error.Message));
                IsError = true;
            }

            if (UnitRoot == null)
            {
                outputLog.Error("Parser", "encountered an unrecoverable error.");
                IsError = true;
            }
            else
            {
                intermediateRoot.AppendChild(UnitRoot);
            }
            return (!IsError);
        }


        private void Compile_file(Redist.Parsers.SyntaxTreeNode Node)
        {
            foreach (Redist.Parsers.SyntaxTreeNode Child in Node.Children)
            {
                if (Child.Symbol.Name == "Namespace")
                    Compile_namespace(Child, this.OutputRootNamespace);
                else
                {
                    IResourceCompiler Compiler = pluginRegister.GetCompilerFor(Child.Symbol.Name);
                    if (Compiler == null)
                        throw new NoResourceCompilerFoundException("Missing compiler for resource " + Child.Symbol.Name);
                    Compiler.CreateResource(this.OutputRootNamespace, Child, intermediateResources, outputLog);
                }
            }
        }
        private void Compile_namespace(Redist.Parsers.SyntaxTreeNode Node, Namespace CurrentNamespace)
        {
            QualifiedName Name = CompileQualifiedName(Node.Children[0]);
            CurrentNamespace = CurrentNamespace.AddSubNamespace(Name);
            foreach (Redist.Parsers.SyntaxTreeNode Child in Node.Children[1].Children)
            {
                if (Child.Symbol.Name == "Namespace")
                    Compile_namespace(Child, CurrentNamespace);
                else
                {
                    IResourceCompiler Compiler = pluginRegister.GetCompilerFor(Child.Symbol.Name);
                    if (Compiler == null)
                        throw new NoResourceCompilerFoundException("Missing compiler for resource " + Child.Symbol.Name);
                    Compiler.CreateResource(CurrentNamespace, Child, intermediateResources, outputLog);
                }
            }
        }
    }
}
