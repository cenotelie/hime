/*
 * Author: Charles Hymans
 * */
using System;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
	class Parse : IExecutable
	{
		protected IParser parser;
				
        public void Execute()
        {
			SyntaxTreeNode root = parser.Analyse();

            foreach (ParserError error in parser.Errors)
			{
                Console.WriteLine(error.ToString());
			}
			
            if (root == null) return;
            using (LangTest.WinTreeView window = new LangTest.WinTreeView(root))
			{
                window.ShowDialog();
            }
        }
	}
}

