using Hime.Redist.Parsers;
using Hime.Redist.AST;

namespace Hime.CentralDogma
{
    interface CompilerPlugin
    {
        Grammars.GrammarLoader GetLoader(string resName, ASTNode node, Reporting.Reporter log);
    }
}
