using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Parsers.ContextFree
{
    class CFPlugin : CompilerPlugin
    {
        public GrammarLoader GetLoader(Redist.Parsers.CSTNode node, Utils.Reporting.Reporter log)
        {
            return new CFGrammarLoader(node, log);
        }
    }
}
