using Hime.Redist.Parsers;

namespace Hime.CentralDogma.Grammars.ContextFree
{
    class CFPlugin : CompilerPlugin
    {
        public GrammarLoader GetLoader(string resName, ASTNode node, Reporting.Reporter log)
        {
            return new CFGrammarLoader(resName, node, log);
        }
    }
}
