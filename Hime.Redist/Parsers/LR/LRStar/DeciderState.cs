using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public struct DeciderState
    {
        public Dictionary<ushort, ushort> transitions;
        public ushort shift;
        public LRRule reduction;
        public DeciderState(ushort[] t_keys, ushort[] t_val, ushort shift, LRRule reduction)
        {
            this.transitions = new Dictionary<ushort, ushort>();
            for (int i = 0; i != t_keys.Length; i++)
                this.transitions.Add(t_keys[i], t_val[i]);
            this.shift = shift;
            this.reduction = reduction;
        }
    }
}