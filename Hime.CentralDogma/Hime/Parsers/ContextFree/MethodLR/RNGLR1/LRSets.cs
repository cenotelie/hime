/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class StateActionRNReduce : StateActionReduce
    {
        protected int reduceLength;

        public int ReduceLength { get { return reduceLength; } }

        public StateActionRNReduce(Terminal Lookahead, CFRule ToReduce, int ReduceLength) : base(Lookahead, ToReduce) { reduceLength = ReduceLength; }
    }
}
