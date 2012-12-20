/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.CentralDogma.Grammars
{
    class Dollar : Terminal
    {
        private static Dollar instance;
        private static readonly object _lock = new object();
        private Dollar() : base(2, "$", 0) { }

        public static Dollar Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new Dollar();
                    return instance;
                }
            }
        }

        public override string ToString() { return "$"; }
    }
}