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
            Hime.Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor(typeof(Hime.Kernel.Resources.ResourceAccessor).Assembly, "Resources");
            string content = accessor.GetAllTextFor("Transforms.Hime.js");
            Analyser.ECMAScriptLexer lexer = new Analyser.ECMAScriptLexer(content);
            this.parser = new Analyser.ECMAScriptParser(lexer);
		}
    }
}
