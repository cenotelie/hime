/*
 * Author: Laurent Wouters
 * Date: 25/01/2012
 * Time: 19:00
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Demo.Tasks
{
    internal class ParseC : Parse
    {
        internal ParseC()
		{
            string input = "int main() { int i = 1; return i; }";
            Hime.Demo.Generated.C.ANSI_CLexer lexer = new Hime.Demo.Generated.C.ANSI_CLexer(input);
            this.parser = new Hime.Demo.Generated.C.ANSI_CParser(lexer);
		}
    }
}
