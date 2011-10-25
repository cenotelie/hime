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
    /// Represents a state of the DFA
    /// </summary>
    public struct LexerDFAState
    {
        /// <summary>
        /// State's transitions
        /// </summary>
        public ushort[][] transitions;
        /// <summary>
        /// Terminal recognized at this state, or null if none is recognized
        /// </summary>
        public SymbolTerminal terminal;
        /// <summary>
        /// Initializes a new instance of the State structure with transitions and a terminal
        /// </summary>
        /// <param name="transitions">The transitions for the state</param>
        /// <param name="terminal">The terminal recognized at this state</param>
        public LexerDFAState(ushort[][] transitions, SymbolTerminal terminal)
        {
            this.transitions = transitions;
            this.terminal = terminal;
        }
    }
}