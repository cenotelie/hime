using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    public interface ITokenStream
    {
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        SymbolToken GetNextToken();
    }
}
