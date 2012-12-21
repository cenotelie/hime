using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Demo.Tasks
{
    class ParseRNGLR : IExecutable
    {
        public void Execute()
        {
            Generated.Test1Lexer lexer = new Generated.Test1Lexer("a.b");
            Generated.Test1Parser parser = new Generated.Test1Parser(lexer);

            Redist.AST.CSTNode root = parser.Parse();
            foreach (Redist.Parsers.ParserError error in parser.Errors)
                System.Console.WriteLine(error);
            WinTreeView win = new WinTreeView(root);
            win.ShowDialog();
        }
    }
}
