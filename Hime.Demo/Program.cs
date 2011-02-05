namespace LangTest
{
    public class Program
    {
        static void Compile()
        {
            Hime.Kernel.Logs.LogProxy LogProxy = new Hime.Kernel.Logs.LogProxy();
            Hime.Kernel.Logs.LogXML LogXML = new Hime.Kernel.Logs.LogXML();
            LogProxy.AddTarget(Hime.Kernel.Logs.LogConsole.Instance);
            LogProxy.AddTarget(LogXML);

            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputFile("Languages\\LALR1-ambiguous.gram");
            compiler.Compile(root, LogProxy);
            Hime.Generators.Parsers.Grammar grammar = (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("AmbiguousLALR1"));
            grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LALR1, "TestAnalyze.cs", LogProxy, false);
            grammar.GenerateGrammarInfo("Grammar.xml", LogProxy);

            LogXML.ExportHTML("LogTest.html", "Grammar Log");
            System.Diagnostics.Process.Start("LogTest.html");
        }

        static void Parse()
        {
            Analyzer.Lexer_Hime_Earth_CIL_GrammarCSharp Lex = new Analyzer.Lexer_Hime_Earth_CIL_GrammarCSharp("(x)");
            Analyzer.Parser_Hime_Earth_CIL_GrammarCSharp Parser = new Analyzer.Parser_Hime_Earth_CIL_GrammarCSharp(Lex);

            Hime.Kernel.Parsers.SyntaxTreeNode Root = Parser.Analyse();
            if (Root != null)
            {
                Root = Root.ApplyActions();
                LangTest.WinTreeView Win = new LangTest.WinTreeView(Root);
                Win.ShowDialog();
            }
        }

        static void Main(string[] args)
        {
            //Hime.Kernel.KernelDaemon.GenerateNextStep("C:\\Users\\Laurent\\Documents\\Visual Studio 2008\\Projects\\HimeSystems");
            Compile();
            //Parse();
        }
    }
}
