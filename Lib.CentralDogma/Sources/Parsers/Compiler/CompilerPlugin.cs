using System;
using System.Collections.Generic;
using System.Text;
using Hime.Redist.Parsers;
using Hime.Utils.Reporting;

namespace Hime.Parsers
{
    interface CompilerPlugin
    {
        GrammarLoader GetLoader(CSTNode node, Reporter log);
    }
}
