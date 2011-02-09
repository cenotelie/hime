namespace Hime.Parsers.CF
{
    /// <summary>
    /// Represents a variable in a context-free grammar
    /// </summary>
    public class CFVariable : Variable
    {
        protected System.Collections.Generic.List<CFRule> p_Rules;
        protected TerminalSet p_Firsts;
        protected TerminalSet p_Followers;

        public System.Collections.Generic.List<CFRule> Rules { get { return p_Rules; } }
        public TerminalSet Firsts { get { return p_Firsts; } }
        public TerminalSet Followers { get { return p_Followers; } }

        public CFVariable(Grammar Parent, ushort SID, string Name) : base(Parent, SID, Name)
        {
            p_Rules = new System.Collections.Generic.List<CFRule>();
            p_Firsts = new TerminalSet();
            p_Followers = new TerminalSet();
        }

        public void AddRule(CFRule Rule)
        {
            foreach (CFRule R in p_Rules)
                if (R == Rule)
                    return;
            int ID = p_Rules.Count;
            Rule.ID = ID;
            p_Rules.Add(Rule);
        }

        public bool ComputeFirsts()
        {
            bool mod = false;
            foreach (CFRule Rule in p_Rules)
            {
                TerminalSet RuleFirsts = Rule.Definition.Firsts;
                if (RuleFirsts != null)
                    if (p_Firsts.AddRange(RuleFirsts))
                        mod = true;
                if (Rule.Definition.ComputeFirsts())
                    mod = true;
            }
            return mod;
        }

        public void ComputeFollowers_Step1()
        {
            foreach (CFRule Rule in p_Rules)
                Rule.Definition.ComputeFollowers_Step1();
        }

        public bool ComputeFollowers_Step23()
        {
            bool mod = false;
            foreach (CFRule Rule in p_Rules)
                if (Rule.Definition.ComputeFollowers_Step23(this))
                    mod = true;
            return mod;
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolVariable");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["SID"].Value = p_SID.ToString("X");
            Node.Attributes["Name"].Value = p_LocalName;
            foreach (CFRule Rule in p_Rules)
                Node.AppendChild(Rule.GetXMLNode(Doc));
            return Node;
        }
    }
}