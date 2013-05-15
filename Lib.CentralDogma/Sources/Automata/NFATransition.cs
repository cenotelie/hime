namespace Hime.CentralDogma.Automata
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