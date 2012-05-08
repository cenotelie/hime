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
    /// Represents an action symbol in a shared packed parse forest
    /// </summary>
    public sealed class SymbolAction : Symbol
    {
        /// <summary>
        /// Represents the method to call for the execution of the symbol
        /// </summary>
        /// <param name="subroot">The syntax tree node on which the action is executed</param>
        public delegate void Callback(SyntaxTreeNode subroot);

        /// <summary>
        /// Gets the callback for the execution of the symbol
        /// </summary>
        public Callback Action { get; private set; }

        /// <summary>
        /// Initializes a new instance of the SymbolAction class with the given symbol type name and callback
        /// </summary>
        /// <param name="name">Symbol's type name</param>
        /// <param name="callback">Callback for the action's execution</param>
        public SymbolAction(string name, Callback callback) : base(0, name)
        {
            this.Action = callback;
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
            SymbolAction other = obj as SymbolAction;
            if (other == null) return false;
            return (this.Name == other.Name);
        }
    }
}