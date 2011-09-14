using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Demo.Tasks
{
    class ANSICParse : IExecutable
    {
        public void Execute()
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
    }
}
