/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;
using System.Collections.Generic;

namespace Hime.Parsers
{
    class Action : GrammarSymbol
    {
        public Action(string name) : base(0, name) { }

        public override string ToString() { return "{" + Name + "}"; }
    }
}
