using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ParserDataLRA : ParserDataLR
    {
        //protected System.IO.StreamWriter stream;

        public ParserDataLRA(ParserGenerator generator, CFGrammar gram, Graph graph) : base(generator, gram, graph) { }

        public override bool Export(GrammarBuildOptions Options) { return false; }
    }
}
