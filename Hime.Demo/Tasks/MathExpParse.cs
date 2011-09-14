using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Demo.Tasks
{
    class MathExpParse : IExecutable
    {
        public void Execute()
        {
            MathExpInterpreter interpreter = new MathExpInterpreter();
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
    }
}
