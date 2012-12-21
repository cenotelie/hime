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
    /// Represents a semantic action embedded within a Shared Packed Parser Forest
    /// </summary>
    public sealed class Action : Symbol
    {
        /// <summary>
        /// Gets the user callback corresponding to this action
        /// </summary>
        public Parsers.SemanticAction Callback { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Action class with the given callback
        /// </summary>
        /// <param name="callback">User callback for this action</param>
        public Action(Parsers.SemanticAction callback) : base(0, callback.Method.Name)
        {
            this.Callback = callback;
        }
		
        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns>A hash code for the current symbol</returns>
        public override int GetHashCode() 
		{ 
			return this.Name.GetHashCode(); 
		}
		
        /// <summary>
        /// Determines whether the specified symbol is equal to the current symbol
        /// </summary>
        /// <param name="obj">The symbol to compare with the current symbol</param>
        /// <returns>true if the specified symbol is equal to the current symbol; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            Action other = obj as Action;
            if (other == null) return false;
            return (this.Callback == other.Callback);
        }
    }
}