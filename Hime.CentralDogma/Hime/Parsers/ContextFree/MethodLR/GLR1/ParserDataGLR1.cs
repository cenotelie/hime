/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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
