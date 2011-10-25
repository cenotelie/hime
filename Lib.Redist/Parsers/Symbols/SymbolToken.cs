/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a piece of data matched by a lexer
    /// </summary>
    public abstract class SymbolToken : Symbol
    {
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public abstract object Value { get; }
		
        /// <summary>
        /// Initializes a new instance of the SymbolToken class with a given name and id
        /// </summary>
        /// <param name="sid">The unique ID of the symbol</param>
        /// <param name="name">The name of the symbol</param>
        public SymbolToken(ushort sid, string name): base(sid, name)
        {
        }
    }
}