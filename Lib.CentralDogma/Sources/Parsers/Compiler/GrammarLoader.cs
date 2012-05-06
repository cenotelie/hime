/*
 * Author: Charles Hymans
 * Date: 21/07/2011
 * Time: 22:24
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Hime.Parsers
{
	interface GrammarLoader
    {
        string Name { get; }
        Grammar Grammar { get; }
        bool IsSolved { get; }

        int Resolve(Dictionary<string, GrammarLoader> loaders);
    }
}
