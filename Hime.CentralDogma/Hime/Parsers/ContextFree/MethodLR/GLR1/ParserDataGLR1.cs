using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class ParserDataGLR1 : ParserData
    {
        //protected System.IO.StreamWriter stream;

        public ParserDataGLR1(ParserGenerator generator, CFGrammar gram, Graph graph) : base(generator, gram, graph) { }

        public override bool Export(IList<Terminal> expected, CompilationTask options) { return false; }
    }
}
