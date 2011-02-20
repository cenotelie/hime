namespace Hime.Parsers.CF.LR
{
    public abstract class LRParserData : ParserData
    {
        protected ParserGenerator p_Generator;
        protected CFGrammar p_Grammar;
        protected Graph p_Graph;

        public CFGrammar Grammar { get { return p_Grammar; } }
        public Graph Graph { get { return p_Graph; } }
        public ParserGenerator Generator { get { return p_Generator; } }

        public LRParserData(ParserGenerator generator, CFGrammar gram, Graph graph)
        {
            p_Grammar = gram;
            p_Graph = graph;
            p_Generator = generator;
        }

        public abstract bool Export(GrammarBuildOptions Options);

        public System.Xml.XmlNode GetData(System.Xml.XmlDocument Document)
        {
            System.Xml.XmlNode graph = Document.CreateElement("LRGraph");
            foreach (ItemSet set in p_Graph.Sets)
                graph.AppendChild(GetData_Set(Document, set));
            return graph;
        }
        protected System.Xml.XmlNode GetData_Set(System.Xml.XmlDocument Document, ItemSet Set)
        {
            System.Xml.XmlNode root = Document.CreateElement("ItemSet");
            root.Attributes.Append(Document.CreateAttribute("SetID"));
            root.Attributes["SetID"].Value = Set.ID.ToString("X");
            foreach (Item item in Set.Items)
                root.AppendChild(GetData_Set_Item(Document, Set, item));
            return root;
        }
        protected System.Xml.XmlNode GetData_Set_Item(System.Xml.XmlDocument Document, ItemSet Set, Item Item)
        {
            System.Xml.XmlNode root = Document.CreateElement("Item");
            root.Attributes.Append(Document.CreateAttribute("HeadName"));
            root.Attributes.Append(Document.CreateAttribute("HeadSID"));
            root.Attributes.Append(Document.CreateAttribute("Conflict"));
            root.Attributes["HeadName"].Value = Item.BaseRule.Variable.LocalName;
            root.Attributes["HeadSID"].Value = Item.BaseRule.Variable.SID.ToString("X");
            root.Attributes["Conflict"].Value = GetData_Set_Item_Conflictuous(Set, Item).ToString();

            System.Xml.XmlNode action = Document.CreateElement("Action");
            action.Attributes.Append(Document.CreateAttribute("Type"));
            action.Attributes["Type"].Value = Item.Action.ToString();
            if (Item.Action == ItemAction.Shift)
                action.InnerText = Set.Children[Item.NextSymbol].ID.ToString("X");
            else
                action.InnerText = Item.BaseRule.ID.ToString("X");
            root.AppendChild(action);

            System.Xml.XmlNode symbols = Document.CreateElement("Symbols");
            int i = 0;
            foreach (RuleDefinitionPart Part in Item.BaseRule.Definition.GetChoiceAtIndex(0).Parts)
            {
                if (i == Item.DotPosition)
                    symbols.AppendChild(Document.CreateElement("Dot"));
                symbols.AppendChild(Part.GetXMLNode(Document));
                i++;
            }
            if (i == Item.DotPosition)
                symbols.AppendChild(Document.CreateElement("Dot"));
            root.AppendChild(symbols);

            //System.Xml.XmlNode lookaheads = Document.CreateElement("Lookaheads");
            return root;
        }
        protected ConflictType GetData_Set_Item_Conflictuous(ItemSet Set, Item Item)
        {
            foreach (Conflict conflict in Set.Conflicts)
                if (conflict.ContainsItem(Item))
                    return conflict.ConflictType;
            return ConflictType.None;
        }
    }
}