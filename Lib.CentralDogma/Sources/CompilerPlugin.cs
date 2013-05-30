using Hime.Redist;

namespace Hime.CentralDogma
{
    interface CompilerPlugin
    {
        Grammars.GrammarLoader GetLoader(string resName, ASTNode node, Reporting.Reporter log);
    }
}
