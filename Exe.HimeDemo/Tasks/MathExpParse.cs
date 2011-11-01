/*
 * Author: Charles Hymans
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
    internal class MathExpParse : ParsingTask
    {
		internal MathExpParse()
		{
            MathExpInterpreter interpreter = new MathExpInterpreter();
            Analyser.MathExp_Lexer lexer = new Analyser.MathExp_Lexer("5 + 6");
            this.parser = new Analyser.MathExp_Parser(lexer, interpreter);
        }
    }
}
