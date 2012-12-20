using Hime.Redist.Parsers;
using Hime.Redist.AST;

namespace Hime.CentralDogma
{
    interface CompilerPlugin
    {
        Grammars.GrammarLoader GetLoader(CSTNode node, Reporting.Reporter log);
    }
}
