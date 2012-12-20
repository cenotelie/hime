using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.CentralDogma.Grammars
{
    interface ParserGenerator
    {
        ParserData Build(Grammar grammar);
    }
}
