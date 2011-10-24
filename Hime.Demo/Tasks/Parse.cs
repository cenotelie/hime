using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Demo.Tasks
{
    class Parse : IExecutable
    {
        public void Execute()
        {
            /*System.IO.StreamWriter stream = new System.IO.StreamWriter("Test2.txt");
            stream.Write("((x.x)x.x).x");
            for (int i = 0; i != 100000; i++)
                stream.Write("|((x.x)x.x).x");
            stream.Close();*/

            Analyser.Test2_Lexer lexer = new Analyser.Test2_Lexer(new System.IO.StreamReader("Test2.txt"));
            //Analyser.Test2_Lexer lexer = new Analyser.Test2_Lexer("((x.x)x.x).x");
            Analyser.Test2_Parser parser = new Analyser.Test2_Parser(lexer);
            parser.Analyse();

            /*foreach (Hime.Redist.Parsers.LexerError error in lexer.Errors) System.Console.WriteLine(error.ToString());
            foreach (Hime.Redist.Parsers.ParserError error in parser.Errors) System.Console.WriteLine(error.ToString());
            if (root != null)
            {
                LangTest.WinTreeView window = new LangTest.WinTreeView(root);
                window.ShowDialog();
            }*/
        }
    }
}
