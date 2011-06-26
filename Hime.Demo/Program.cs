namespace Hime.Demo
{
    public class Program
    {        
        static void Parse_MathExp()
        {
            Interpreter interpreter = new Interpreter();
            Analyser.MathExp_Lexer Lex = new Analyser.MathExp_Lexer("5 + 6");
            Analyser.MathExp_Parser Parser = new Analyser.MathExp_Parser(Lex, interpreter);
            Hime.Redist.Parsers.SyntaxTreeNode Root = Parser.Analyse();
            System.Console.Write(interpreter.Value);
            
            foreach (Hime.Redist.Parsers.LexerError LexerError in Lex.Errors) System.Console.WriteLine(LexerError.ToString());
            foreach (Hime.Redist.Parsers.ParserError ParserError in Parser.Errors) System.Console.WriteLine(ParserError.ToString());
            if (Root != null)
            {
                LangTest.WinTreeView Win = new LangTest.WinTreeView(Root);
                Win.ShowDialog();
            }
        }

        static void Parse_Test()
        {
            Analyser.Test2_Lexer Lex = new Analyser.Test2_Lexer("(x.x).x.x");
            Analyser.Test2_Parser Parser = new Analyser.Test2_Parser(Lex);
            Hime.Redist.Parsers.SyntaxTreeNode Root = Parser.Analyse();
            
            foreach (Hime.Redist.Parsers.LexerError LexerError in Lex.Errors) System.Console.WriteLine(LexerError.ToString());
            foreach (Hime.Redist.Parsers.ParserError ParserError in Parser.Errors) System.Console.WriteLine(ParserError.ToString());
            if (Root != null)
            {
                LangTest.WinTreeView Win = new LangTest.WinTreeView(Root);
                Win.ShowDialog();
            }
        }
        
        static void Main(string[] args)
        {
            Hime.Parsers.CompilationTask task = new Hime.Parsers.CompilationTask();
            task.Namespace = "Analyser";
            task.ExportLog = true;
            task.ExportDoc = true;
            //task.ExportVisuals = true;
            task.InputFiles.Add("Languages\\Earth.CIL.CSharp.gram");
            task.ParserFile = "Earth.CIL.CSharp.cs";
            task.Method = Parsers.ParsingMethod.LRStar;
            task.DOTBinary = "C:\\Program Files\\Graphviz 2.28\\bin\\dot.exe";
            task.Execute();

            //Parse_Test();
            //Parse_MathExp();
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
