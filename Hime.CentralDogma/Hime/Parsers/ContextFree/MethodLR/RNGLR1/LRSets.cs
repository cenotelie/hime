using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetActionRNReduce : ItemSetActionReduce
    {
        protected int p_ReduceLength;

        public int ReduceLength { get { return p_ReduceLength; } }

        public ItemSetActionRNReduce(Terminal Lookahead, CFRule ToReduce, int ReduceLength) : base(Lookahead, ToReduce) { p_ReduceLength = ReduceLength; }
    }
}
