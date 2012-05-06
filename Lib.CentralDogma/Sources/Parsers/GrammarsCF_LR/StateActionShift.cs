/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class StateActionShift : StateAction
    {
        private GrammarSymbol symbol;
        private State childSet;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public GrammarSymbol OnSymbol { get { return symbol; } }
        public State ChildSet { get { return childSet; } }

        public StateActionShift(GrammarSymbol Lookahead, State ChildSet)
        {
            symbol = Lookahead;
            childSet = ChildSet;
        }
    }
}