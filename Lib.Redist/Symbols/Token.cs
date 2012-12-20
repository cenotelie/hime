/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a piece of data matched by a lexer
    /// </summary>
    public abstract class Token : Symbol
    {
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public abstract object Value { get; }
		
        /// <summary>
        /// Initializes a new instance of the SymbolToken class with the given type ID and name
        /// </summary>
        /// <param name="sid">Symbol's unique type identifier</param>
        /// <param name="name">Symbol's type name</param>
        public Token(ushort sid, string name): base(sid, name) { }
    }
}