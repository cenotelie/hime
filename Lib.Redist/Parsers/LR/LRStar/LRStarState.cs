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
	/// States for LRStar automata.
	/// </summary>
    public sealed class LRStarState : LRState
    {
        internal DeciderState[] decider;
		
		// TODO: not nice: it is strange that items seem not to be used, idem of shiftsOnTerminal
		// TODO: Maybe LRState should not be like it is now... Think about it.
        public LRStarState(string[] items, SymbolTerminal[] expected, DeciderState[] decider, ushort[] sv_keys, ushort[] sv_val) : base(items)
        {
            this.expecteds = expected;
            this.shiftsOnTerminal = null;
            this.shiftsOnVariable = new Dictionary<ushort, ushort>();
            for (int i = 0; i != sv_keys.Length; i++)
                this.shiftsOnVariable.Add(sv_keys[i], sv_val[i]);
            this.decider = decider;
        }
    }
}