/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Parsers
{
    class TerminalDummy : Terminal
    {
        private static TerminalDummy instance;
        private static readonly object _lock = new object();
        private TerminalDummy() : base(0xFFFF, "#", -1) { }

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