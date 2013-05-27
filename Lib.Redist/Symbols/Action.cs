namespace Hime.Redist.Symbols
{
    class Action : Symbol
    {
        private Parsers.SemanticAction callback;

        public Parsers.SemanticAction Callback { get { return callback; } }

        public Action(Parsers.SemanticAction callback) : base(-1, string.Empty) { this.callback = callback; }
    }
}