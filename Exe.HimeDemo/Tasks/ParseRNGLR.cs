using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Demo.Tasks
{
    class ParseRNGLR : IExecutable
    {
        public void Execute()
        {
            Generated.Test2Lexer lexer = new Generated.Test2Lexer("aa");
            Generated.Test2Parser parser = new Generated.Test2Parser(lexer);

            Redist.AST.SPPFNode root = parser.ParseSPPF();
            foreach (Redist.Parsers.ParserError error in parser.Errors)
                System.Console.WriteLine(error);
            Export(root, "graph.dot");
        }

        private void Show(Redist.AST.CSTNode root)
        {
            WinTreeView win = new WinTreeView(root);
            win.ShowDialog();
        }

        private void Export(Redist.AST.SPPFNode root, string file)
        {
            Hime.CentralDogma.ASTExporter.ExportDOT(root, file);
        }
    }
}
