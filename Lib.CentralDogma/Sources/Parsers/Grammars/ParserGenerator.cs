using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Parsers
{
    interface ParserGenerator
    {
        ParserData Build(Grammar grammar);
    }
}
