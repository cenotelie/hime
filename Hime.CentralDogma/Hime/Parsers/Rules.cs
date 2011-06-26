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
        private Symbol symbol;
        private RuleDefinitionPartAction action;

        public Symbol Symbol { get { return symbol; } }
        public RuleDefinitionPartAction Action
        {
            get { return action; }
            set { action = value; }
        }

        public RuleDefinitionPart(Symbol Sym, RuleDefinitionPartAction Action)
        {
            symbol = Sym;
            action = Action;
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

            Node.Attributes["Action"].Value = action.ToString();
            Node.Attributes["SymbolID"].Value = symbol.SID.ToString("X");
            Node.Attributes["SymbolName"].Value = symbol.LocalName;
            Node.Attributes["SymbolValue"].Value = symbol.ToString();

            if (symbol is Terminal)
                Node.Attributes["SymbolType"].Value = "Terminal";
            else if (symbol is Variable)
                Node.Attributes["SymbolType"].Value = "Variable";
            else if (symbol is Virtual)
                Node.Attributes["SymbolType"].Value = "Virtual";
            else if (symbol is Action)
                Node.Attributes["SymbolType"].Value = "Action";
            return Node;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            RuleDefinitionPart part = obj as RuleDefinitionPart;
            if (this.symbol.SID != part.symbol.SID)
                return false;
            return (this.action == part.action);
        }
        public override string ToString()
        {
            string s = symbol.ToString();
            if (action == RuleDefinitionPartAction.Promote)
                return (s + "^");
            else if (action == RuleDefinitionPartAction.Drop)
                return (s + "!");
            else
                return s;
        }
    }




    public abstract class RuleDefinition
    {
        protected List<RuleDefinitionPart> parts;

        public int Length { get { return parts.Count; } }
        public List<RuleDefinitionPart> Parts { get { return parts; } }


        public RuleDefinition() { parts = new List<RuleDefinitionPart>(); }
        public RuleDefinition(ICollection<RuleDefinitionPart> Parts)
        {
            parts = new List<RuleDefinitionPart>();
            parts.AddRange(Parts);
        }
        public RuleDefinition(Symbol UniqueSymbol)
        {
            parts = new List<RuleDefinitionPart>();
            parts.Add(new RuleDefinitionPart(UniqueSymbol, RuleDefinitionPartAction.Nothing));
        }

        public abstract Symbol GetSymbolAtIndex(int Index);


        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            RuleDefinition def = obj as RuleDefinition;
            if (this.parts.Count != def.parts.Count)
                return false;
            for (int i = 0; i != this.parts.Count; i++)
                if (!this.parts[i].Equals(def.parts[i]))
                    return false;
            return true;
        }
        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            foreach (RuleDefinitionPart Part in parts)
            {
                Builder.Append(" ");
                Builder.Append(Part.ToString());
            }
            return Builder.ToString();
        }
    }
}
