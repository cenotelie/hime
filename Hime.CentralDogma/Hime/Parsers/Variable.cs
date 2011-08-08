/*
 * Author: Charles Hymans
 * Date: 07/08/2011
 * Time: 15:05
 * 
 */
using System;
using System.Xml;
using System.Collections.Generic;
using Hime.Parsers.ContextFree;

namespace Hime.Parsers
{
	public class Variable : Symbol
    {
        internal protected List<CFRule> Rules { get; private set; }
        internal protected TerminalSet Firsts { get; private set; }
        internal protected TerminalSet Followers { get; private set; }

        
        public Variable(Grammar Parent, ushort SID, string Name) : base(Parent, SID, Name) 
        { 
            this.Rules = new List<CFRule>();
            this.Firsts = new TerminalSet();
            this.Followers = new TerminalSet();
        }

        public void AddRule(CFRule Rule)
        {
            if (this.Rules.Contains(Rule))
                return;
            int ID = this.Rules.Count;
            Rule.ID = ID;
            this.Rules.Add(Rule);
        }

        public bool ComputeFirsts()
        {
            bool mod = false;
            foreach (CFRule Rule in this.Rules)
            {
                TerminalSet RuleFirsts = Rule.Definition.Firsts;
                if (RuleFirsts != null)
                    if (this.Firsts.AddRange(RuleFirsts))
                        mod = true;
                if (Rule.Definition.ComputeFirsts())
                    mod = true;
            }
            return mod;
        }

        public void ComputeFollowers_Step1()
        {
            foreach (CFRule Rule in this.Rules)
                Rule.Definition.ComputeFollowers_Step1();
        }

        public bool ComputeFollowers_Step23()
        {
            bool mod = false;
            foreach (CFRule Rule in this.Rules)
                if (Rule.Definition.ComputeFollowers_Step23(this))
                    mod = true;
            return mod;
        }

        public override XmlNode GetXMLNode(XmlDocument Doc)
        {
            XmlNode Node = Doc.CreateElement("SymbolVariable");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["SID"].Value = SID.ToString("X");
            Node.Attributes["Name"].Value = localName;
            foreach (CFRule Rule in this.Rules)
            {
                Node.AppendChild(Rule.GetXMLNode(Doc));
            }
            return Node;
        }
    }
}
