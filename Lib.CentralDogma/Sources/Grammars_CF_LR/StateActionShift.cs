using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class StateActionShift : StateAction
    {
        private Symbol symbol;
        private State childSet;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public Symbol OnSymbol { get { return symbol; } }
        public State ChildSet { get { return childSet; } }

        public StateActionShift(Symbol Lookahead, State ChildSet)
        {
            symbol = Lookahead;
            childSet = ChildSet;
        }
    }
}