using System.Collections.Generic;

namespace Hime.Parsers
{
    public enum RuleDefinitionPartAction
    {
        Nothing,
        Promote,
        Drop
    }



    public sealed class RuleDefinitionPart
    {
        private Symbol p_Symbol;
        private RuleDefinitionPartAction p_Action;

        public Symbol Symbol { get { return p_Symbol; } }
        public RuleDefinitionPartAction Action
        {
            get { return p_Action; }
            set { p_Action = value; }
        }

        public RuleDefinitionPart(Symbol Sym, RuleDefinitionPartAction Action)
        {
            p_Symbol = Sym;
            p_Action = Action;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Symbol");
            Node.Attributes.Append(Doc.CreateAttribute("Action"));
            Node.Attributes.Append(Doc.CreateAttribute("SymbolType"));
            Node.Attributes.Append(Doc.CreateAttribute("SymbolID"));
            Node.Attributes.Append(Doc.CreateAttribute("SymbolName"));
            Node.Attributes.Append(Doc.CreateAttribute("SymbolValue"));
            Node.Attributes.Append(Doc.CreateAttribute("ParserIndex"));

            Node.Attributes["Action"].Value = p_Action.ToString();
            Node.Attributes["SymbolID"].Value = p_Symbol.SID.ToString("X");
            Node.Attributes["SymbolName"].Value = p_Symbol.LocalName;
            Node.Attributes["SymbolValue"].Value = p_Symbol.ToString();

            if (p_Symbol is Terminal)
                Node.Attributes["SymbolType"].Value = "Terminal";
            else if (p_Symbol is Variable)
                Node.Attributes["SymbolType"].Value = "Variable";
            else if (p_Symbol is Virtual)
                Node.Attributes["SymbolType"].Value = "Virtual";
            else if (p_Symbol is Action)
                Node.Attributes["SymbolType"].Value = "Action";
            return Node;
        }

        public static bool operator ==(RuleDefinitionPart Left, RuleDefinitionPart Right)
        {
            if (Left.p_Symbol != Right.p_Symbol)
                return false;
            return (Left.p_Action == Right.p_Action);
        }
        public static bool operator !=(RuleDefinitionPart Left, RuleDefinitionPart Right)
        {
            if (Left.p_Symbol != Right.p_Symbol)
                return true;
            return (Left.p_Action != Right.p_Action);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RuleDefinitionPart))
                return false;
            RuleDefinitionPart right = (RuleDefinitionPart)obj;
            return (this == right);
        }
        public override int GetHashCode() { return base.GetHashCode(); }
        public override string ToString()
        {
            string s = p_Symbol.ToString();
            if (p_Action == RuleDefinitionPartAction.Promote)
                return (s + "^");
            else if (p_Action == RuleDefinitionPartAction.Drop)
                return (s + "!");
            else
                return s;
        }
    }




    public abstract class RuleDefinition
    {
        protected List<RuleDefinitionPart> p_Parts;

        public int Length { get { return p_Parts.Count; } }
        public List<RuleDefinitionPart> Parts { get { return p_Parts; } }


        public RuleDefinition() { p_Parts = new List<RuleDefinitionPart>(); }
        public RuleDefinition(ICollection<RuleDefinitionPart> Parts)
        {
            p_Parts = new List<RuleDefinitionPart>();
            p_Parts.AddRange(Parts);
        }
        public RuleDefinition(Symbol UniqueSymbol)
        {
            p_Parts = new List<RuleDefinitionPart>();
            p_Parts.Add(new RuleDefinitionPart(UniqueSymbol, RuleDefinitionPartAction.Nothing));
        }

        public abstract Symbol GetSymbolAtIndex(int Index);

        

        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            foreach (RuleDefinitionPart Part in p_Parts)
            {
                Builder.Append(" ");
                Builder.Append(Part.ToString());
            }
            return Builder.ToString();
        }
    }
}
