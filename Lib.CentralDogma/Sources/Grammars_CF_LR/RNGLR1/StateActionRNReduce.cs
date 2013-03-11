/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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
