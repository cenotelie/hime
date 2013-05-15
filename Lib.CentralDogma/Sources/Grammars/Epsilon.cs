namespace Hime.CentralDogma.Grammars
{
    class Epsilon : Terminal
    {
        private static Epsilon instance;
        private static readonly object _lock = new object();
        private Epsilon() : base(1, "ε", 0) { }

        public static Epsilon Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new Epsilon();
                    return instance;
                }
            }
        }

        public override string ToString() { return "ε"; }
    }
}