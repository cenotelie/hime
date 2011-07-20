using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an action symbol in a shared packed parse forest
    /// </summary>
    public sealed class SymbolAction : Symbol
    {
        /// <summary>
        /// Represents the method to call for executing the action
        /// </summary>
        /// <param name="subroot">The syntax tree node on which the action is executed</param>
        public delegate void Callback(SyntaxTreeNode subroot);

        private Callback callback;

        /// <summary>
        /// Gets the callback represented by this symbol
        /// </summary>
        public Callback Action { get { return callback; } }

        /// <summary>
        /// Initializes a new instance of the SymbolAction class with a name and a callback
        /// </summary>
        /// <param name="name">The name of the action symbol</param>
        /// <param name="callback">The callback for the action</param>
        public SymbolAction(string name, Callback callback)
        {
            this.name = name;
            this.sid = 0;
            this.callback = callback;
        }

        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns>A hash code for the current symbol</returns>
        public override int GetHashCode() { return base.GetHashCode(); }
        /// <summary>
        /// Determines whether the specified symbol is equal to the current symbol
        /// </summary>
        /// <param name="obj">The symbol to compare with the current symbol</param>
        /// <returns>true if the specified symbol is equal to the current symbol; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            SymbolAction other = obj as SymbolAction;
            if (other == null)
                return false;
            return (this.name == other.name);
        }
    }
}