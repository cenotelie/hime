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
    public sealed class SymbolAction : SymbolVirtual
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
        public SymbolAction(string name, Callback callback) : base(name)
        {
            this.Action = callback;
        }
    }
}