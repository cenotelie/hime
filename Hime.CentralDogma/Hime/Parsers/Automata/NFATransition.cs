namespace Hime.Parsers.Automata
{
    public sealed class NFATransition
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