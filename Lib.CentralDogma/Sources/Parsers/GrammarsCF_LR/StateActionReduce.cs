/*
 * Author: Charles Hymans
 * Date: 19/07/2011
 * Time: 21:11
 * 
 */
using System;

namespace Hime.Parsers.ContextFree.LR
{
    class StateActionReduce : StateAction
    {
        protected Terminal lookahead;
        protected CFRule toReduce;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public GrammarSymbol OnSymbol { get { return lookahead; } }
        public Terminal Lookahead { get { return lookahead; } }
        public CFRule ToReduceRule { get { return toReduce; } }

        public StateActionReduce(Terminal Lookahead, CFRule ToReduce)
        {
        	lookahead = Lookahead;
            toReduce = ToReduce;
        }
    }
}
