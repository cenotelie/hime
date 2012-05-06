/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Parsers
{
    class TerminalDollar : Terminal
    {
        private static TerminalDollar instance;
        private static readonly object _lock = new object();
        private TerminalDollar() : base(2, "$", 0) { }

        public static TerminalDollar Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new TerminalDollar();
                    return instance;
                }
            }
        }

        public override string ToString() { return "$"; }
    }
}