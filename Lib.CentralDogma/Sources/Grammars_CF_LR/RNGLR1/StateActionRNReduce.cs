using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class StateActionRNReduce : StateActionReduce
    {
        internal int ReduceLength { get; private set; }

        public StateActionRNReduce(Terminal Lookahead, CFRule ToReduce, int ReduceLength) : base(Lookahead, ToReduce) 
		{ 
			this.ReduceLength = ReduceLength; 
		}
    }
}
