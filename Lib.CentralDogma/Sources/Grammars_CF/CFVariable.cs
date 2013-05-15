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
