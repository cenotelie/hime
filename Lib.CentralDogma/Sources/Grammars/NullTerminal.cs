namespace Hime.CentralDogma.Grammars
{
    class NullTerminal : Terminal
    {
        private static NullTerminal instance;
        private static readonly object _lock = new object();
        private NullTerminal() : base(0, string.Empty, 0) { }

        public static NullTerminal Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new NullTerminal();
                    return instance;
                }
            }
        }

        public override string ToString() { return string.Empty; }
    }
}