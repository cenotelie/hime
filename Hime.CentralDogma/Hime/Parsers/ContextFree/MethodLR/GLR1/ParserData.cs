using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class GLR1ParserData : LRParserData
    {
        //protected System.IO.StreamWriter p_Stream;

        public GLR1ParserData(ParserGenerator generator, CFGrammar gram, Graph graph) : base(generator, gram, graph) { }

        public override bool Export(GrammarBuildOptions Options) { return false; }
    }
}
