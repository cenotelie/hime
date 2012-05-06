/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Parsers.Automata
{
    class NFATransition
    {
        public CharSpan span;
        public NFAState next;
        public NFATransition(CharSpan span, NFAState next)
        {
            this.span = span;
            this.next = next;
        }
    }
}