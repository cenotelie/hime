namespace Hime.Parsers.CF.LR
{
    public abstract class LRParserData : ParserData
    {
        protected ParserGenerator p_Generator;
        protected CFGrammar p_Grammar;
        protected Graph p_Graph;

        public CFGrammar Grammar { get { return p_Grammar; } }
        public Graph Graph { get { return p_Graph; } }
        public ParserGenerator Generator { get { return p_Generator; } }

        public LRParserData(ParserGenerator generator, CFGrammar gram, Graph graph)
        {
            p_Grammar = gram;
            p_Graph = graph;
            p_Generator = generator;
        }

        public abstract bool Export(GrammarBuildOptions Options);
    }
}