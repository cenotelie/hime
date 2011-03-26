using System.Collections.Generic;

namespace Hime.Parsers.CF
{
    public sealed class CFVariable : Variable
    {
        private List<CFRule> rules;
        private TerminalSet firsts;
        private TerminalSet followers;

        public List<CFRule> Rules { get { return rules; } }
        public TerminalSet Firsts { get { return firsts; } }
        public TerminalSet Followers { get { return followers; } }

        public CFVariable(Grammar Parent, ushort SID, string Name) : base(Parent, SID, Name)
        {
            rules = new List<CFRule>();
            firsts = new TerminalSet();
            followers = new TerminalSet();
        }

        public void AddRule(CFRule Rule)
        {
            foreach (CFRule R in rules)
                if (R == Rule)
                    return;
            int ID = rules.Count;
            Rule.ID = ID;
            rules.Add(Rule);
        }

        public bool ComputeFirsts()
        {
            bool mod = false;
            foreach (CFRule Rule in rules)
            {
                TerminalSet RuleFirsts = Rule.Definition.Firsts;
                if (RuleFirsts != null)
                    if (firsts.AddRange(RuleFirsts))
                        mod = true;
                if (Rule.Definition.ComputeFirsts())
                    mod = true;
            }
            return mod;
        }

        public void ComputeFollowers_Step1()
        {
            foreach (CFRule Rule in rules)
                Rule.Definition.ComputeFollowers_Step1();
        }

        public bool ComputeFollowers_Step23()
        {
            bool mod = false;
            foreach (CFRule Rule in rules)
                if (Rule.Definition.ComputeFollowers_Step23(this))
                    mod = true;
            return mod;
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolVariable");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["SID"].Value = sID.ToString("X");
            Node.Attributes["Name"].Value = localName;
            foreach (CFRule Rule in rules)
                Node.AppendChild(Rule.GetXMLNode(Doc));
            return Node;
        }
    }
}
