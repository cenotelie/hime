namespace Hime.CentralDogma.Grammars
{
    class Dummy : Terminal
    {
        private static Dummy instance;
        private static readonly object _lock = new object();
        private Dummy() : base(0xFFFF, "#", -1) { }

        public static Dummy Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new Dummy();
                    return instance;
                }
            }
        }

        public override string ToString() { return "#"; }
    }
}