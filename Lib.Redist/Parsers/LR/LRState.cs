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
    /// Represents a base state in a LR(k) parser automaton
    /// </summary>
    public abstract class LRState
    {
        /// <summary>
        /// Array of the string representations of items of this state
        /// </summary>
        /// <remarks>
        /// This attribute is filled only if the debug option has been selected at generation time
        /// </remarks>
        public string[] items;
        /// <summary>
        /// Array of the the expected terminals at this state
        /// This is used for error recovery.
        /// </summary>
        public SymbolTerminal[] expecteds;
        /// <summary>
        /// Dictionary associating the ID of the next state given a terminal ID
        /// </summary>
        public Dictionary<ushort, ushort> shiftsOnTerminal;
        /// <summary>
        /// Dictionary associating the ID of the next state given a variable ID
        /// </summary>
        public Dictionary<ushort, ushort> shiftsOnVariable;

        /// <summary>
        /// Initializes a new instance of the LRState class with its inner data
        /// </summary>
        /// <param name="items">State's items</param>
        /// <param name="expected">Expected terminals for this state</param>
        /// <param name="st_keys">Terminal IDs for shift actions</param>
        /// <param name="st_val">Next state IDs for shift actions on terminals</param>
        /// <param name="sv_keys">Variable IDs for shift actions</param>
        /// <param name="sv_val">Next state IDs for shift actions on variables</param>
        public LRState(string[] items, SymbolTerminal[] expected, ushort[] st_keys, ushort[] st_val, ushort[] sv_keys, ushort[] sv_val)
        {
            this.items = items;
            this.expecteds = expected;
            this.shiftsOnTerminal = new Dictionary<ushort, ushort>();
            this.shiftsOnVariable = new Dictionary<ushort, ushort>();
            for (int i = 0; i != st_keys.Length; i++)
                this.shiftsOnTerminal.Add(st_keys[i], st_val[i]);
            for (int i = 0; i != sv_keys.Length; i++)
                this.shiftsOnVariable.Add(sv_keys[i], sv_val[i]);
        }
        /// <summary>
        /// Initializes a new instance of the LRState class with its inner data
        /// </summary>
        /// <param name="items">State's items</param>
        public LRState(string[] items)
        {
            this.items = items;
            this.expecteds = null;
            this.shiftsOnTerminal = null;
            this.shiftsOnVariable = null;
        }

        /// <summary>
        /// Gets the next state ID by a shift action on the given terminal ID
        /// </summary>
        /// <param name="sid">The terminal ID</param>
        /// <returns>The next state's ID by a shift action on the given terminal ID, or 0xFFFF if no suitable shift action is found</returns>
        public ushort GetNextByShiftOnTerminal(ushort sid)
        {
            if (!shiftsOnTerminal.ContainsKey(sid))
                return 0xFFFF;
            return shiftsOnTerminal[sid];
        }

        /// <summary>
        /// Gets the next state ID by a shift action on the given variable ID
        /// </summary>
        /// <param name="sid">The variable ID</param>
        /// <returns>The next state's ID by a shift action on the given variable ID, or 0xFFFF if no suitable shift action is found</returns>
        public ushort GetNextByShiftOnVariable(ushort sid)
        {
            if (!shiftsOnVariable.ContainsKey(sid))
                return 0xFFFF;
            return shiftsOnVariable[sid];
        }
    }
}