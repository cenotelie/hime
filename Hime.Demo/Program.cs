using Hime.Parsers;

namespace Hime.Demo
{
    public class Program
    {        
        static void Parse_MathExp()
        {
            Interpreter interpreter = new Interpreter();
            Analyser.MathExp_Lexer lexer = new Analyser.MathExp_Lexer("5 + 6");
            Analyser.MathExp_Parser parser = new Analyser.MathExp_Parser(lexer, interpreter);
            Hime.Redist.Parsers.SyntaxTreeNode root = parser.Analyse();
            System.Console.Write(interpreter.Value);

            foreach (Hime.Redist.Parsers.LexerError error in lexer.Errors) System.Console.WriteLine(error.ToString());
            foreach (Hime.Redist.Parsers.ParserError error in parser.Errors) System.Console.WriteLine(error.ToString());
            if (root != null)
            {
                LangTest.WinTreeView window = new LangTest.WinTreeView(root);
                window.ShowDialog();
            }
        }

        static void Parse_Test()
        {
            /*System.IO.StreamWriter stream = new System.IO.StreamWriter("Test2.txt");
            stream.Write("((x.x)x.x).x");
            for (int i = 0; i != 100000; i++)
                stream.Write("|((x.x)x.x).x");
            stream.Close();*/

            Analyser.Test2_Lexer lexer = new Analyser.Test2_Lexer(new System.IO.StreamReader("Test2.txt"));
            //Analyser.Test2_Lexer lexer = new Analyser.Test2_Lexer("((x.x)x.x).x");
            Analyser.Test2_Parser parser = new Analyser.Test2_Parser(lexer);
            Hime.Redist.Parsers.SyntaxTreeNode root = parser.Analyse();
            
            /*foreach (Hime.Redist.Parsers.LexerError error in lexer.Errors) System.Console.WriteLine(error.ToString());
            foreach (Hime.Redist.Parsers.ParserError error in parser.Errors) System.Console.WriteLine(error.ToString());
            if (root != null)
            {
                LangTest.WinTreeView window = new LangTest.WinTreeView(root);
                window.ShowDialog();
            }*/
        }

        static void Parse_ANSI_C()
        {
            Analyser.ANSI_C_Lexer lexer = new Analyser.ANSI_C_Lexer(new System.IO.StreamReader("test.c"));
            Analyser.ANSI_C_Parser parser = new Analyser.ANSI_C_Parser(lexer);
            Hime.Redist.Parsers.SyntaxTreeNode root = parser.Analyse();

            foreach (Hime.Redist.Parsers.LexerError error in lexer.Errors) System.Console.WriteLine(error.ToString());
            foreach (Hime.Redist.Parsers.ParserError error in parser.Errors) System.Console.WriteLine(error.ToString());
            if (root != null)
            {
                LangTest.WinTreeView window = new LangTest.WinTreeView(root);
                window.ShowDialog();
            }
        }

        // TODO: remove all static methods!!!
        static void Compile()
        {
        	CompilationTask task = new CompilationTask();
            task.Namespace = "Analyser";
            task.ExportLog = true;
            task.ExportDoc = false;
            task.ExportVisuals = false;
            task.InputFiles.Add("Languages\\Test.gram");
            task.ParserFile = "Test.cs";
            task.Method = EParsingMethod.LRStar;
            task.DOTBinary = "C:\\Program Files\\Graphviz 2.28\\bin\\dot.exe";
            task.Execute();
        }

        static void Main(string[] args)
        {
            //Hime.Kernel.KernelDaemon.GenerateNextStep("D:\\Data\\VisualStudioProjects");
            Compile();
            //Parse_ANSI_C();
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
