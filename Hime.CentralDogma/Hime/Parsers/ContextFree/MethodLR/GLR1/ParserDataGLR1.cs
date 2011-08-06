using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ParserDataGLR1 : ParserDataLR
    {
        //protected System.IO.StreamWriter stream;

        public ParserDataGLR1(ParserGenerator generator, CFGrammar gram, Graph graph) : base(generator, gram, graph) { }

        public override bool Export(IList<Terminal> expected, CompilationTask options) { return false; }
    }
}
