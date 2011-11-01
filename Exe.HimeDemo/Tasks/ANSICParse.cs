/*
 * Author: Charles Hymans
 * */
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
    class ANSICParse : IExecutable
    {
        public void Execute()
        {
            Analyser.ANSI_C_Lexer lexer = new Analyser.ANSI_C_Lexer(new StreamReader("test.c"));
            Analyser.ANSI_C_Parser parser = new Analyser.ANSI_C_Parser(lexer);
            SyntaxTreeNode root = parser.Analyse();

            foreach (ParserError error in parser.Errors)
			{
                Console.WriteLine(error.ToString());
			}
			
            if (root != null)
            {
				// TODO: what is this LangTest namespace, why is it different?
				using (LangTest.WinTreeView window = new LangTest.WinTreeView(root))
				{
                	window.ShowDialog();
				}
            }
        }
    }
}
