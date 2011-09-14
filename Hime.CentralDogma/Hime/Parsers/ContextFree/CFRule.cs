/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    public sealed class CFRule
    {
        public const string arrow = "→";

        private Variable variable;
        private CFRuleDefinition definition;
        private bool replaceOnProduction;
        private int iD;
        private int watermark;

        public Variable Variable { get { return variable; } }
        public CFRuleDefinition Definition { get { return definition; } }
        public bool ReplaceOnProduction { get { return replaceOnProduction; } }
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public int Watermark { get { return watermark; } }


        public CFRule(Variable variable, CFRuleDefinition definition, bool replaceOnProduction)
        {
            this.variable = variable;
            this.definition = definition;
            this.replaceOnProduction = replaceOnProduction;
        }
        public CFRule(Variable variable, CFRuleDefinition definition, bool replaceOnProduction, int watermark)
        {
            this.variable = variable;
            this.definition = definition;
            this.replaceOnProduction = replaceOnProduction;
            this.watermark = watermark;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("Rule");
            node.Attributes.Append(document.CreateAttribute("HeadName"));
            node.Attributes.Append(document.CreateAttribute("HeadSID"));
            node.Attributes.Append(document.CreateAttribute("RuleID"));
            node.Attributes.Append(document.CreateAttribute("Replace"));
            node.Attributes["HeadName"].Value = variable.LocalName;
            node.Attributes["HeadSID"].Value = variable.SID.ToString("X");
            node.Attributes["RuleID"].Value = iD.ToString("X");
            node.Attributes["Replace"].Value = replaceOnProduction.ToString();
            node.AppendChild(definition.GetXMLNode(document));
            return node;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            CFRule rule = obj as CFRule;
            if (this.variable.SID != rule.variable.SID)
                return false;
            if (this.replaceOnProduction != rule.replaceOnProduction)
                return false;
            return this.definition.Equals(rule.definition);
        }
        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(variable.LocalName);
            builder.Append(" ");
            builder.Append(arrow);
            builder.Append(definition.ToString());
            return builder.ToString();
        }

        internal class Comparer : IEqualityComparer<CFRule>
        {
            public bool Equals(CFRule x, CFRule y)
            {
                if (x.variable.SID != y.variable.SID) return false;
                return (x.iD == y.iD);
            }
            public int GetHashCode(CFRule obj) { return ((obj.variable.SID << 16) + obj.iD); }
            private Comparer() { }
            private static Comparer instance = new Comparer();
            public static Comparer Instance { get { return instance; } }
        }
    }
}
