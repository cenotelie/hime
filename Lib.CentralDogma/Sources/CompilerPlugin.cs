using Hime.Redist.Parsers;
using Hime.Redist.AST;

namespace Hime.CentralDogma
{
    interface CompilerPlugin
    {
        Grammars.GrammarLoader GetLoader(ASTNode node, Reporting.Reporter log);
    }
}
