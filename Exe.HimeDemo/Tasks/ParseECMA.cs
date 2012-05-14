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
    internal class ParseECMA : Parse
    {
        internal ParseECMA()
		{
            string file = "value-of.js";
            System.IO.DirectoryInfo current = new System.IO.DirectoryInfo(Environment.CurrentDirectory);
            string dir = System.IO.Path.Combine(current.Parent.Parent.FullName, "JSData");
            dir = System.IO.Path.Combine(dir, file);
            Hime.Demo.Generated.ECMA.ECMAScriptLexer lexer = new Hime.Demo.Generated.ECMA.ECMAScriptLexer(new System.IO.StreamReader(dir));
            this.parser = new Hime.Demo.Generated.ECMA.ECMAScriptParser(lexer);
		}
    }
}
