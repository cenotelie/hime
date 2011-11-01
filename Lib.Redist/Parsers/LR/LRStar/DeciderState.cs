/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public class DeciderState
    {
        private Dictionary<ushort, ushort> transitions;
        internal ushort Shift { get; private set; }
        public LRRule reduction;
        public DeciderState(ushort[] t_keys, ushort[] t_val, ushort shift, LRRule reduction)
        {
            this.transitions = new Dictionary<ushort, ushort>();
            for (int i = 0; i != t_keys.Length; i++)
                this.transitions.Add(t_keys[i], t_val[i]);
            this.Shift = shift;
            this.reduction = reduction;
        }
		
		internal bool ContainsKey(ushort token)
		{
			return this.transitions.ContainsKey(token);
		}
		
		internal ushort this[ushort index]
		{
			get { return this.transitions[index]; }
		}
    }
}