/*
 * Author: Laurent WOUTERS
 * */
using System;
using System.Collections.Generic;
using Hime.Redist.Symbols;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
    class ParseGrammar : IExecutable
    {
        public void Execute()
        {
            Generated.FileCentralDogmaLexer lexer = new Generated.FileCentralDogmaLexer(new System.IO.StreamReader("Languages\\FileCentralDogma.gram"));
            Generated.FileCentralDogmaParser parser = new Generated.FileCentralDogmaParser(lexer);
            Redist.AST.CSTNode root = parser.Parse();
            foreach (ParserError error in parser.Errors)
                Console.WriteLine(error.ToString());
            if (root == null)
                return;
            WinTreeView win = new WinTreeView(root);
            win.ShowDialog();
        }
    }
}

