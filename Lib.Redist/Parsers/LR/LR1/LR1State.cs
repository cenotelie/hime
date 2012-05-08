/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
	// TODO: Compare with LR0State and see if they can not be all the same in the end
	/// <summary>
	/// State for LR1 automata.
	/// </summary>
    public sealed class LR1State : LRState
    {
        /// <summary>
        /// Dictionary associating a reduction to terminal ID
        /// </summary>
        public Dictionary<ushort, LRReduction> reducsOnTerminal;

        /// <summary>
        /// Initializes a new instance of the LR1State class with its inner data
        /// </summary>
        /// <param name="items">State's items</param>
        /// <param name="expected">Expected terminals for this state</param>
        /// <param name="st_keys">Terminal IDs for shift actions</param>
        /// <param name="st_val">Next state IDs for shift actions on terminals</param>
        /// <param name="sv_keys">Variable IDs for shift actions</param>
        /// <param name="sv_val">Next state IDs for shift actions on variables</param>
        /// <param name="rt">Reductions that can occur in this state</param>
        public LR1State(string[] items, SymbolTerminal[] expected, ushort[] st_keys, ushort[] st_val, ushort[] sv_keys, ushort[] sv_val, LRReduction[] rt)
            : base(items, expected, st_keys, st_val, sv_keys, sv_val)
        {
            reducsOnTerminal = new Dictionary<ushort, LRReduction>();
            for (int i = 0; i != rt.Length; i++)
                reducsOnTerminal.Add(rt[i].lookahead, rt[i]);
        }

        /// <summary>
        /// Determines whether a reduction can occur for the given terminal ID
        /// </summary>
        /// <param name="sid">The terminal ID</param>
        /// <returns>True if a reduction is found, false otherwise</returns>
        public bool HasReductionOnTerminal(ushort sid) { return reducsOnTerminal.ContainsKey(sid); }
        
        /// <summary>
        /// Get the reduction for the given terminal ID
        /// </summary>
        /// <param name="sid">The terminal ID</param>
        /// <returns>The reduction for the given terminal ID</returns>
        public LRReduction GetReductionOnTerminal(ushort sid) { return reducsOnTerminal[sid]; }
    }
}