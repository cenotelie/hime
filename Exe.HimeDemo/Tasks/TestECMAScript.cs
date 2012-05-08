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
    internal class TestECMAScript : ParsingTask
    {
        internal TestECMAScript()
		{
            Hime.Demo.Generated.ECMA.ECMAScriptLexer lexer = new Hime.Demo.Generated.ECMA.ECMAScriptLexer("");
            this.parser = new Hime.Demo.Generated.ECMA.ECMAScriptParser(lexer);
		}
    }
}
