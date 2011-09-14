namespace Hime.Parsers
{
    public sealed class TerminalDummy : Terminal
    {
        private static TerminalDummy instance;
        private static readonly object _lock = new object();
        private TerminalDummy() : base(null, 0xFFFF, "#", -1) { }

        public static TerminalDummy Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new TerminalDummy();
                    return instance;
                }
            }
        }

        public override string ToString() { return "#"; }
    }
}