/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.Automata
{
    class NFAState
    {
        private List<NFATransition> transitions;
        private Terminal final;
        private int mark;

        public List<NFATransition> Transitions { get { return transitions; } }
        public Terminal Final
        {
            get { return final; }
            set { final = value; }
        }
        public int Mark
        {
            get { return mark; }
            set { mark = value; }
        }

        public NFAState()
        {
            transitions = new List<NFATransition>();
            final = null;
            mark = 0;
        }

        public void AddTransition(CharSpan value, NFAState next) { transitions.Add(new NFATransition(value, next)); }
        public void ClearTransitions() { transitions.Clear(); }
    }
}