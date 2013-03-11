namespace Hime.CentralDogma.Automata
{
    class DummyItem : FinalItem
    {
        private static DummyItem instance;
        private static readonly object _lock = new object();
        private DummyItem() { }

        public static FinalItem Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new DummyItem();
                    return instance;
                }
            }
        }

        public int Priority { get { return -1; } }

        public override string ToString() { return "#"; }
    }
}
