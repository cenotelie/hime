/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public class LRStarState : LRState
    {
        internal DeciderState[] decider;
		
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