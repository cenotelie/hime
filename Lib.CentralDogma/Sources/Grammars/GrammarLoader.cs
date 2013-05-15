using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars
{
	interface GrammarLoader
    {
        Grammar Grammar { get; }
        bool IsSolved { get; }
        void Load(Dictionary<string, GrammarLoader> loaders);
    }
}
