using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetActionRNReduce : ItemSetActionReduce
    {
        protected int reduceLength;

        public int ReduceLength { get { return reduceLength; } }

        public ItemSetActionRNReduce(Terminal Lookahead, CFRule ToReduce, int ReduceLength) : base(Lookahead, ToReduce) { reduceLength = ReduceLength; }
    }
}
