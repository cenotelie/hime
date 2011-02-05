namespace Hime.Kernel.Resources
{
    public sealed class ResourceCompilerRegister
    {
        private System.Collections.Generic.Dictionary<string, IResourceCompiler> p_Compilers;

        public System.Collections.Generic.IEnumerable<IResourceCompiler> Compilers { get { return p_Compilers.Values; } }

        public ResourceCompilerRegister()
        {
            p_Compilers = new System.Collections.Generic.Dictionary<string, IResourceCompiler>();
        }

        public void RegisterCompiler(IResourceCompiler Compiler)
        {
            foreach (string Name in Compiler.ResourceNames)
                p_Compilers.Add(Name, Compiler);
        }

        public IResourceCompiler GetCompilerFor(string ResourceName) { return p_Compilers[ResourceName]; }
    }



    public sealed class ResourceCompiler
    {
        private System.Collections.Generic.Dictionary<string, string> p_InputNamedResources;
        private System.Collections.Generic.List<string> p_InputRawResources;
        private System.Collections.Generic.List<CompilerError> p_OutputErrors;
        private Namespace p_OutputRootNamespace;
        private Logs.Log p_OutputLog;
        private Parsers.SyntaxTreeNode p_IntermediateRoot;
        private ResourceGraph p_IntermediateResources;
        private ResourceCompilerRegister p_PluginRegister;

        public string CompilerName { get { return "HimeSystems.CentralDogma Compiler"; } }
        public int CompilerVersionMajor { get { return 1; } }
        public int CompilerVersionMinor { get { return 0; } }
        public ResourceCompilerRegister PluginRegister { get { return p_PluginRegister; } }

        public ResourceCompiler()
        {
            p_InputNamedResources = new System.Collections.Generic.Dictionary<string, string>();
            p_InputRawResources = new System.Collections.Generic.List<string>();
            p_OutputErrors = new System.Collections.Generic.List<CompilerError>();
            p_IntermediateRoot = new Parsers.SyntaxTreeNode(null);
            p_IntermediateResources = new ResourceGraph();
            p_PluginRegister = new ResourceCompilerRegister();
            p_PluginRegister.RegisterCompiler(new Generators.Parsers.ContextFree.CFGrammarCompiler());
        }

        public void AddInputFile(string FileName)
        {
            string Data = System.IO.File.ReadAllText(FileName);
            p_InputNamedResources.Add(FileName, Data);
        }

        public void AddInputRawText(string Data)
        {
            p_InputRawResources.Add(Data);
        }

        public void Compile(Namespace RootNamespace, Logs.Log Log)
        {
            p_OutputRootNamespace = RootNamespace;
            p_OutputLog = Log;

            p_OutputLog.SectionBegin(CompilerName);
            p_OutputLog.EntryBegin("Info"); p_OutputLog.EntryAddData("Compiler"); p_OutputLog.EntryAddData(CompilerName + " " + CompilerVersionMajor.ToString() + "." + CompilerVersionMinor.ToString()); p_OutputLog.EntryEnd();
            foreach (IResourceCompiler Plugin in p_PluginRegister.Compilers)
            {
                p_OutputLog.EntryBegin("Info");
                p_OutputLog.EntryAddData("Compiler");
                p_OutputLog.EntryAddData("Register plugin : " + Plugin.ToString());
                p_OutputLog.EntryEnd();
            }
            foreach (string ResourceName in p_InputNamedResources.Keys)
            {
                p_OutputLog.EntryBegin("Info");
                p_OutputLog.EntryAddData("Compiler");
                p_OutputLog.EntryAddData("Compilation unit : " + ResourceName);
                p_OutputLog.EntryEnd();
            }
            if (p_InputRawResources.Count != 0)
            {
                p_OutputLog.EntryBegin("Info");
                p_OutputLog.EntryAddData("Compiler");
                p_OutputLog.EntryAddData("Compilation unit : " + p_InputRawResources.Count.ToString() + " raw resources");
                p_OutputLog.EntryEnd();
            }

            // Parse
            foreach (string ResourceName in p_InputNamedResources.Keys)
                CompileData(p_InputNamedResources[ResourceName]);
            foreach (string Data in p_InputRawResources)
                CompileData(Data);

            // Build resources
            foreach (Parsers.SyntaxTreeNode file in p_IntermediateRoot.Children)
                Compile_file(file);

            // Build dependencies
            foreach (Resource R in p_IntermediateResources.Resources)
                R.Compiler.CreateDependencies(R, p_IntermediateResources, p_OutputLog);

            // Solve dependencies and compile
            int Unsolved = 1;
            while (Unsolved != 0)
            {
                Unsolved = 0;
                int Solved = 0;
                foreach (Resource R in p_IntermediateResources.Resources)
                {
                    if (R.IsCompiled) continue;
                    Solved += R.Compiler.CompileSolveDependencies(R, p_OutputLog);
                    Unsolved += R.Dependencies.Count;
                    if (R.Dependencies.Count == 0)
                        R.Compiler.Compile(R, p_OutputLog);
                }
                if (Solved == 0)
                {  }
            }

            p_OutputLog.SectionEnd();
        }



        public static QualifiedName CompileQualifiedName(Parsers.SyntaxTreeNode Node)
        {
            System.Collections.Generic.List<string> Path = new System.Collections.Generic.List<string>();
            foreach (Parsers.SyntaxTreeNode Child in Node.Children)
                Path.Add(((Parsers.SymbolTokenText)Child.Symbol).ValueText);
            return new QualifiedName(Path);
        }
        public static SymbolAccess CompileSymbolAccess(Parsers.SyntaxTreeNode Node)
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
            Parser.Lexer_Hime_Kernel_FileCentralDogma UnitLexer = new Parser.Lexer_Hime_Kernel_FileCentralDogma(Data);
            Parser.Parser_Hime_Kernel_FileCentralDogma UnitParser = new Parser.Parser_Hime_Kernel_FileCentralDogma(UnitLexer);
            Parsers.SyntaxTreeNode UnitRoot = null;
            try { UnitRoot = UnitParser.Analyse(); }
            catch (System.Exception e) { }

            foreach (Hime.Kernel.Parsers.LexerTextError Error in UnitLexer.Errors)
            {
                p_OutputLog.EntryBegin("Error");
                p_OutputLog.EntryAddData("(" + Error.Line.ToString() + ", " + Error.Column.ToString() + ")");
                p_OutputLog.EntryAddData(Error.ToString());
                p_OutputLog.EntryEnd();
                IsError = true;
            }
            foreach (Hime.Kernel.Parsers.IParserError Error in UnitParser.Errors)
            {
                p_OutputLog.EntryBegin("Error");
                if (Error is Hime.Kernel.Parsers.ParserErrorUnexpectedToken)
                {
                    Hime.Kernel.Parsers.ParserErrorUnexpectedToken UnexpError = (Hime.Kernel.Parsers.ParserErrorUnexpectedToken)Error;
                    if (UnexpError.UnexpectedToken is Hime.Kernel.Parsers.SymbolTokenText)
                    {
                        Hime.Kernel.Parsers.SymbolTokenText Token = (Hime.Kernel.Parsers.SymbolTokenText)UnexpError.UnexpectedToken;
                        p_OutputLog.EntryAddData("Line " + Token.Line.ToString());
                    }
                    else
                    {
                        p_OutputLog.EntryAddData("End of file");
                    }
                }
                else
                {
                    p_OutputLog.EntryAddData("File");
                }
                p_OutputLog.EntryAddData(Error.Message);
                p_OutputLog.EntryEnd();
                IsError = true;
            }

            if (UnitRoot == null)
            {
                p_OutputLog.EntryBegin("Error"); p_OutputLog.EntryAddData("Compiler"); p_OutputLog.EntryAddData("The parser encountered an unrecoverable error."); p_OutputLog.EntryEnd();
                IsError = true;
            }
            else
            {
                UnitRoot = UnitRoot.ApplyActions();
                p_IntermediateRoot.AppendChild(UnitRoot);
            }
            return (!IsError);
        }


        private void Compile_file(Parsers.SyntaxTreeNode Node)
        {
            foreach (Parsers.SyntaxTreeNode Child in Node.Children)
            {
                if (Child.Symbol.Name == "Namespace")
                    Compile_namespace(Child, p_OutputRootNamespace);
                else
                {
                    IResourceCompiler Compiler = p_PluginRegister.GetCompilerFor(Child.Symbol.Name);
                    if (Compiler == null)
                        throw new NoResourceCompilerFoundException("Missing compiler for resource " + Child.Symbol.Name);
                    Compiler.CreateResource(p_OutputRootNamespace, Child, p_IntermediateResources, p_OutputLog);
                }
            }
        }
        private void Compile_namespace(Parsers.SyntaxTreeNode Node, Namespace CurrentNamespace)
        {
            QualifiedName Name = CompileQualifiedName(Node.Children[0]);
            CurrentNamespace = CurrentNamespace.AddSubNamespace(Name);
            foreach (Parsers.SyntaxTreeNode Child in Node.Children[1].Children)
            {
                if (Child.Symbol.Name == "Namespace")
                    Compile_namespace(Child, CurrentNamespace);
                else
                {
                    IResourceCompiler Compiler = p_PluginRegister.GetCompilerFor(Child.Symbol.Name);
                    if (Compiler == null)
                        throw new NoResourceCompilerFoundException("Missing compiler for resource " + Child.Symbol.Name);
                    Compiler.CreateResource(CurrentNamespace, Child, p_IntermediateResources, p_OutputLog);
                }
            }
        }
    }
}