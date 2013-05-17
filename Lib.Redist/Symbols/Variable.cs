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
        public Variable(int sid, string name) : base(sid, name) { }
    }
}