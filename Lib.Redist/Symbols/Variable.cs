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
    /// Represents a variable in an abstract syntax tree or a grammar
    /// </summary>
    public sealed class Variable : Symbol
    {
        /// <summary>
        /// Initializes a new instance of the SymbolVariable class with the given id and name
        /// </summary>
        /// <param name="sid">Symbol's unique type identifier</param>
        /// <param name="name">Symbol's type name</param>
        public Variable(ushort sid, string name) : base(sid, name) { }
    }
}