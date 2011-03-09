namespace LangTest
{
    public class Program
    {        
        static void Parse()
        {
            Interpreter interpreter = new Interpreter();
            Analyser.MathExp_Lexer Lex = new Analyser.MathExp_Lexer("5 + 6");
            Analyser.MathExp_Parser Parser = new Analyser.MathExp_Parser(interpreter, Lex);
            Hime.Redist.Parsers.SyntaxTreeNode Root = Parser.Analyse();
            System.Console.Write(interpreter.Value);
            
            foreach (Hime.Redist.Parsers.LexerError LexerError in Lex.Errors) System.Console.WriteLine(LexerError.ToString());
            foreach (Hime.Redist.Parsers.ParserError ParserError in Parser.Errors) System.Console.WriteLine(ParserError.ToString());
            if (Root != null)
            {
                Root = Root.ApplyActions();
                LangTest.WinTreeView Win = new LangTest.WinTreeView(Root);
                Win.ShowDialog();
            }
        }

        static void Main(string[] args)
        {
            //Hime.Parsers.CompilationTask Task = Hime.Parsers.CompilationTask.Create(new string[] { "Languages\\MathExp.gram" }, "MathExp", Hime.Parsers.ParsingMethod.RNGLR1, "Analyser", null, "MathExp.cs", true, true);
            //Task.Execute();
            Parse();
        }

        class Interpreter : Analyser.MathExp_Parser.Actions
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
