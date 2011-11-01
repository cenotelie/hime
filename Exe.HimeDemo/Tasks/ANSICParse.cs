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
    internal class ANSICParse : ParsingTask
    {
		internal ANSICParse()
		{
            Analyser.ANSI_C_Lexer lexer = new Analyser.ANSI_C_Lexer(new StreamReader("test.c"));
            this.parser = new Analyser.ANSI_C_Parser(lexer);
		}
    }
}
