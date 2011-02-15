namespace LangTest
{
    public class Program
    {
        static void Compile()
        {
            Hime.Kernel.Reporting.Reporter Reporter = new Hime.Kernel.Reporting.Reporter(typeof(Program));
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputFile("Languages\\LALR1-ambiguous.gram");
            compiler.Compile(root, Reporter);
            Hime.Parsers.Grammar grammar = (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("AmbiguousLALR1"));
            Hime.Parsers.GrammarBuildOptions Options = new Hime.Parsers.GrammarBuildOptions(Reporter, "Analyzer", new Hime.Parsers.CF.LR.MethodRNGLR1(), "LALR1-ambiguous.cs");
            grammar.Build(Options);
            Options.Close();
            Reporter.ExportHTML("LALR1-ambiguous.html", "Grammar Log");
            System.Diagnostics.Process.Start("LALR1-ambiguous.html");
        }

        static void Parse()
        {
            Analyzer.AmbiguousLALR1_Lexer Lex = new Analyzer.AmbiguousLALR1_Lexer("(a)b");
            Analyzer.AmbiguousLALR1_Parser Parser = new Analyzer.AmbiguousLALR1_Parser(null, Lex);

            Hime.Redist.Parsers.SyntaxTreeNode Root = Parser.Analyse();
            if (Root != null)
            {
                Root = Root.ApplyActions();
                LangTest.WinTreeView Win = new LangTest.WinTreeView(Root);
                Win.ShowDialog();
            }
        }

        static void Main(string[] args)
        {
            //Compile();
            Parse();
        }

        class Interpreter : Analyzer.MathExp_Parser.Actions
        {
            private System.Collections.Generic.Stack<float> p_Stack;

            public float Value { get { return p_Stack.Peek(); } }

            public Interpreter() { p_Stack = new System.Collections.Generic.Stack<float>(); }

            public void OnNumber(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                Hime.Redist.Parsers.SyntaxTreeNode node = SubRoot.Children[0];
                Hime.Redist.Parsers.SymbolTokenText token = (Hime.Redist.Parsers.SymbolTokenText)node.Symbol;
                float value = System.Convert.ToSingle(token.ValueText);
                p_Stack.Push(value);
            }

            public void OnMult(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                float right = p_Stack.Pop();
                float left = p_Stack.Pop();
                p_Stack.Push(left * right);
            }

            public void OnDiv(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                float right = p_Stack.Pop();
                float left = p_Stack.Pop();
                p_Stack.Push(left / right);
            }

            public void OnPlus(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                float right = p_Stack.Pop();
                float left = p_Stack.Pop();
                p_Stack.Push(left + right);
            }

            public void OnMinus(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                float right = p_Stack.Pop();
                float left = p_Stack.Pop();
                p_Stack.Push(left - right);
            }
        }
    }
}
