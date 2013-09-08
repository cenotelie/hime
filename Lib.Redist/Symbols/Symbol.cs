/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections.Generic;

namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a grammar's symbol
    /// </summary>
    public abstract class Symbol
    {
        /// <summary>
        /// Gets the symbol's unique type ID
        /// </summary>
        public int SymbolID { get; private set; }
        /// <summary>
        /// Gets the symbol's type name
        /// </summary>
        public string Name { get; private set; }
		
        /// <summary>
        /// Initializes a new instance of the Symbol class with the given ID and name
        /// </summary>
        /// <param name="sid">Symbol's unique identifier</param>
        /// <param name="name">Symbol's name</param>
		protected Symbol(int sid, string name)
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
        /// Determines whether the specified object is equal to the current symbol
        /// </summary>
        /// <param name="obj">The symbol to compare with the current symbol</param>
        /// <returns>True if the specified symbol is equal to the current symbol; false otherwise</returns>
        public override bool Equals(object obj)
        {
            Symbol symbol = obj as Symbol;
            if (symbol == null) return false;
            return this.SymbolID == symbol.SymbolID;
        }
    }
}
