/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree
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

        public sealed class Comparer : IEqualityComparer<CFRule>
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
