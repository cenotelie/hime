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

using System;
using System.Xml;
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree
{
    class CFVariable : Variable
    {
        protected List<CFRule> cfRules;

        public IList<CFRule> CFRules { get { return cfRules; } }
        public TerminalSet Firsts { get; private set; }
        public TerminalSet Followers { get; private set; }

        public CFVariable(ushort sid, string name)
            : base(sid, name)
        {
            this.cfRules = new List<CFRule>();
            this.Firsts = new TerminalSet();
            this.Followers = new TerminalSet();
        }

        protected override void OnRuleAdd(Rule rule) { cfRules.Add(rule as CFRule); }

        public bool ComputeFirsts()
        {
            bool mod = false;
            foreach (CFRule rule in cfRules)
            {
                TerminalSet rulefirsts = rule.CFBody.Firsts;
                if (rulefirsts != null)
                    if (this.Firsts.AddRange(rulefirsts))
                        mod = true;
                if (rule.CFBody.ComputeFirsts())
                    mod = true;
            }
            return mod;
        }

        public void ComputeFollowers_Step1()
        {
            foreach (CFRule rule in cfRules)
                rule.CFBody.ComputeFollowers_Step1();
        }

        public bool ComputeFollowers_Step23()
        {
            bool mod = false;
            foreach (CFRule rule in cfRules)
                if (rule.CFBody.ComputeFollowers_Step23(this))
                    mod = true;
            return mod;
        }
    }
}
