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
    /// Represents a synthetic symbol in an abstract syntax tree
    /// </summary>
    public sealed class SymbolVirtual : Symbol
    {
        /// <summary>
        /// Initializes a new instance of the SymbolVirtual class with a name
        /// </summary>
        /// <param name="name">The name of the virtual symbol</param>
        public SymbolVirtual(string name)
        {
            this.name = name;
            this.sid = 0;
        }
		
        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns>A hash code for the current symbol</returns>
        public override int GetHashCode() { return this.name.GetHashCode(); }
		
        /// <summary>
        /// Determines whether the specified symbol is equal to the current symbol
        /// </summary>
        /// <param name="obj">The symbol to compare with the current symbol</param>
        /// <returns>true if the specified symbol is equal to the current symbol; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            SymbolVirtual other = obj as SymbolVirtual;
            if (other == null) return false;
            return (this.name == other.name);
        }
    }
}