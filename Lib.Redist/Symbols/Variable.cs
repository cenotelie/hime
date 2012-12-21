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
    /// Represents a variable in a grammar
    /// </summary>
    public sealed class Variable : Symbol
    {
        /// <summary>
        /// Initializes a new instance of the Variable class with the given ID and name
        /// </summary>
        /// <param name="sid">Symbol's unique identifier</param>
        /// <param name="name">Symbol's name</param>
        public Variable(ushort sid, string name) : base(sid, name) { }
    }
}