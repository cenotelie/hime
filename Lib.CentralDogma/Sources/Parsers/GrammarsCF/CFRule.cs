/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    class CFRule : Rule
    {
        public CFVariable CFHead { get { return head as CFVariable; } }
        public CFRuleBody CFBody { get { return body as CFRuleBody; } }

        public CFRule(CFVariable head, CFRuleBody body, bool replaceOnProduction)
            : base(head, body, replaceOnProduction)
        { }
        public CFRule(CFVariable head, CFRuleBody body, bool replaceOnProduction, int watermark)
            : base(head, body, replaceOnProduction, watermark)
        { }

        public class Comparer : IEqualityComparer<CFRule>
        {
            public bool Equals(CFRule x, CFRule y)
            {
                if (x.head.SID != y.head.SID) return false;
                return (x.id == y.id);
            }
            public int GetHashCode(CFRule obj) { return ((obj.head.SID << 16) + obj.id); }
            private Comparer() { }
            private static Comparer instance = new Comparer();
            public static Comparer Instance { get { return instance; } }
        }
    }
}
