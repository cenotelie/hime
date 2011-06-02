using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ParserDataLRA : ParserDataLR1
    {
        private Dictionary<State, Dictionary<Terminal, DeciderGraph>> deciders;
        //protected System.IO.StreamWriter stream;

        public ParserDataLRA(ParserGenerator generator, CFGrammar gram, Graph graph, Dictionary<State, Dictionary<Terminal, DeciderGraph>> deciders) : base(generator, gram, graph)
        {
            this.deciders = deciders;
        }

        public override bool Export(GrammarBuildOptions Options) { return false; }
    }
}
