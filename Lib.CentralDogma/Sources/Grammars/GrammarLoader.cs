/*
 * Author: Charles Hymans
 * Date: 21/07/2011
 * Time: 22:24
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars
{
	interface GrammarLoader
    {
        string Name { get; }
        Grammar Grammar { get; }
        bool IsSolved { get; }

        void Load(Dictionary<string, GrammarLoader> loaders);
    }
}
