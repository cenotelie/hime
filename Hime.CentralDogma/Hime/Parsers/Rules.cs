using System.Collections.Generic;

namespace Hime.Parsers
{
    /// <summary>
    /// Define the possible action fot the associated symbol on rule production during the parsing process
    /// </summary>
    public enum RuleDefinitionPartAction
    {
        /// <summary>
        /// Do nothing
        /// </summary>
        Nothing,
        /// <summary>
        /// Promote the associated symbol as the new root for the rule production syntax sub-tree
        /// </summary>
        Promote,
        /// <summary>
        /// Remove the associated symbol from the syntax tree on rule production
        /// </summary>
        Drop
    }



    /// <summary>
    /// Represent a part of a rule definition associating a symbol to an action
    /// </summary>
    public sealed class RuleDefinitionPart
    {
        /// <summary>
        /// Symbol represented by the part
        /// </summary>
        private Symbol p_Symbol;
        /// <summary>
        /// Action taken against the symbol on rule production
        /// </summary>
        private RuleDefinitionPartAction p_Action;

        /// <summary>
        /// Get the symbol represented by the part
        /// </summary>
        public Symbol Symbol { get { return p_Symbol; } }
        /// <summary>
        /// Get or set the action taken on rule production against the associated symbol
        /// </summary>
        /// <value>The action taken on rule production</value>
        public RuleDefinitionPartAction Action
        {
            get { return p_Action; }
            set { p_Action = value; }
        }

        /// <summary>
        /// Constructs the part from the given symbol and action
        /// </summary>
        /// <param name="Sym">Symbol to represents</param>
        /// <param name="Action">Associated action taken on rule production</param>
        public RuleDefinitionPart(Symbol Sym, RuleDefinitionPartAction Action)
        {
            p_Symbol = Sym;
            p_Action = Action;
        }

        /// <summary>
        /// Create the XML node representing the rule part
        /// </summary>
        /// <param name="Doc">XML parent document</param>
        /// <returns>Returns the new node</returns>
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

        /// <summary>
        /// Determine if two definition parts are equal
        /// </summary>
        /// <param name="Left">Left operand part</param>
        /// <param name="Right">Right operand part</param>
        /// <returns>Returns true if the parts are equal, false otherwise</returns>
        public static bool operator ==(RuleDefinitionPart Left, RuleDefinitionPart Right)
        {
            if (Left.p_Symbol != Right.p_Symbol)
                return false;
            return (Left.p_Action == Right.p_Action);
        }
        /// <summary>
        /// Determine if two definition parts are different
        /// </summary>
        /// <param name="Left">Left operand part</param>
        /// <param name="Right">Right operand part</param>
        /// <returns>Returns true if the parts are different, false otherwise</returns>
        public static bool operator !=(RuleDefinitionPart Left, RuleDefinitionPart Right)
        {
            if (Left.p_Symbol != Right.p_Symbol)
                return true;
            return (Left.p_Action != Right.p_Action);
        }

        /// <summary>
        /// Override the Equals function to use the overloaded == operator
        /// </summary>
        /// <param name="obj">The tested object</param>
        /// <returns>Returns true if the tested object object is equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is RuleDefinitionPart))
                return false;
            RuleDefinitionPart right = (RuleDefinitionPart)obj;
            return (this == right);
        }
        /// <summary>
        /// Override the GetHashCode function (call the base function)
        /// </summary>
        /// <returns>Returns the returns of the base function</returns>
        public override int GetHashCode() { return base.GetHashCode(); }
        /// <summary>
        /// Get the symbol name eventually followed by the operator representing the action taken on rule production
        /// </summary>
        /// <returns>Returns the symbol name eventually concatenated with the action operator</returns>
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




    /// <summary>
    /// Represents a rule definiton
    /// </summary>
    public abstract class RuleDefinition
    {
        /// <summary>
        /// List of the parts composing the definition
        /// </summary>
        protected List<RuleDefinitionPart> p_Parts;

        /// <summary>
        /// Get the length of the definition
        /// </summary>
        /// <value>The length of the definition that may include virtual symbols if present</value>
        public int Length { get { return p_Parts.Count; } }
        /// <summary>
        /// Get a list of the parts composing the definition
        /// </summary>
        /// <value>List of the parts</value>
        public List<RuleDefinitionPart> Parts { get { return p_Parts; } }


        /// <summary>
        /// Constructs an empty definition
        /// </summary>
        public RuleDefinition() { p_Parts = new List<RuleDefinitionPart>(); }
        /// <summary>
        /// Constructs a definition representing the given part list
        /// </summary>
        /// <param name="Parts">The parts to compose the definition</param>
        public RuleDefinition(ICollection<RuleDefinitionPart> Parts)
        {
            p_Parts = new List<RuleDefinitionPart>();
            p_Parts.AddRange(Parts);
        }
        /// <summary>
        /// Constructs a definition containing a single symbol with no action
        /// </summary>
        /// <param name="UniqueSymbol">The unique symbol of the definition</param>
        public RuleDefinition(Symbol UniqueSymbol)
        {
            p_Parts = new List<RuleDefinitionPart>();
            p_Parts.Add(new RuleDefinitionPart(UniqueSymbol, RuleDefinitionPartAction.Nothing));
        }

        /// <summary>
        /// Get the symbol at the given index
        /// </summary>
        /// <param name="Index">Index of the symbol to retrieve</param>
        /// <returns>Returns the symbol of null if it cannot be retrieved</returns>
        /// <remarks>The returned symbol cannot be a virtual symbol, it is always a terminal or a variable</remarks>
        public abstract Symbol GetSymbolAtIndex(int Index);

        

        /// <summary>
        /// Override the ToString function
        /// </summary>
        /// <returns>Returns the concatenation of all the string representation of the parts</returns>
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
