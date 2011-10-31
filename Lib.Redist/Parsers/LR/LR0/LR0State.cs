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
	/// State for LR0 parsers.
	/// </summary>
	// TODO: think about it, but maybe could use the same state for any kind of parsers. In the end an automaton is an automaton?
    public class LR0State : LRState
    {
        /// <summary>
        /// Rule to reduce on this state
        /// </summary>
        public LRRule reduction;

        /// <summary>
        /// Initializes a new instance of the LR0State class with its inner data
        /// </summary>
        /// <param name="items">State's items</param>
        /// <param name="expected">Expected terminals for this state</param>
        /// <param name="st_keys">Terminal IDs for shift actions</param>
        /// <param name="st_val">Next state IDs for shift actions on terminals</param>
        /// <param name="sv_keys">Variable IDs for shift actions</param>
        /// <param name="sv_val">Next state IDs for shift actions on variables</param>
        public LR0State(string[] items, SymbolTerminal[] expected, ushort[] st_keys, ushort[] st_val, ushort[] sv_keys, ushort[] sv_val) :base(items, expected, st_keys, st_val, sv_keys, sv_val)
        {
            this.reduction = null;
        }
        /// <summary>
        /// Initializes a new instance of the State structure with its inner data
        /// </summary>
        /// <param name="items">State's items</param>
        /// <param name="reduction">The rule to reduce at this state</param>
        public LR0State(string[] items, LRRule reduction) : base(items)
        {
            this.reduction = reduction;
        }

        /// <summary>
        /// Determines whether a reduction can occur
        /// </summary>
        /// <returns>True if a reduction is found, false otherwise</returns>
        public bool HasReduction() { return (reduction != null); }
    }
}