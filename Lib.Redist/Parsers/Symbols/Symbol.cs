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
    /// Represents a symbol in an abstract syntax tree
    /// </summary>
    public abstract class Symbol
    {
        /// <summary>
        /// Gets the symbol's unique type ID
        /// </summary>
        public ushort SymbolID { get; private set; }
        /// <summary>
        /// Gets the symbol's type name
        /// </summary>
        public string Name { get; private set; }
		
        /// <summary>
        /// Initializes a new instance of the Symbol class with the given type ID and name
        /// </summary>
        /// <param name="sid">Symbol's unique type identifier</param>
        /// <param name="name">Symbol's type name</param>
		public Symbol(ushort sid, string name)
        {
            this.SymbolID = sid;
            this.Name = name;
        }
		
        /// <summary>
        /// Returns the name of the symbol
        /// </summary>
        /// <returns>The name of the symbol</returns>
        public override string ToString() 
		{ 
			return this.Name; 
		}
		
        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns>A hash code for the current symbol</returns>
        public override int GetHashCode() 
		{ 
			return this.SymbolID.GetHashCode(); 
		}
		
        /// <summary>
        /// Determines whether the specified symbol is equal to the current symbol
        /// </summary>
        /// <param name="obj">The symbol to compare with the current symbol</param>
        /// <returns>true if the specified symbol is equal to the current symbol; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            Symbol symbol = obj as Symbol;
            if (symbol == null) return false;
            return this.SymbolID == symbol.SymbolID;
        }
    }
}
