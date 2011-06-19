using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    abstract class ParserDataLR : ParserData
    {
        protected ParserGenerator generator;
        protected CFGrammar grammar;
        protected Graph graph;

        public CFGrammar Grammar { get { return grammar; } }
        public Graph Graph { get { return graph; } }
        public ParserGenerator Generator { get { return generator; } }

        public ParserDataLR(ParserGenerator generator, CFGrammar gram, Graph graph)
        {
            this.grammar = gram;
            this.graph = graph;
            this.generator = generator;
        }

        public abstract bool Export(GrammarBuildOptions Options);

        public System.Xml.XmlNode SerializeXML(System.Xml.XmlDocument Document)
        {
            System.Xml.XmlNode nodegraph = Document.CreateElement("LRGraph");
            foreach (State set in graph.Sets)
                nodegraph.AppendChild(GetXMLData_Set(Document, set));
            return nodegraph;
        }
        protected System.Xml.XmlNode GetXMLData_Set(System.Xml.XmlDocument Document, State Set)
        {
            System.Xml.XmlNode root = Document.CreateElement("ItemSet");
            root.Attributes.Append(Document.CreateAttribute("SetID"));
            root.Attributes["SetID"].Value = Set.ID.ToString("X");
            foreach (Item item in Set.Items)
                root.AppendChild(GetXMLData_Item(Document, Set, item));
            return root;
        }
        protected System.Xml.XmlNode GetXMLData_Item(System.Xml.XmlDocument Document, State Set, Item Item)
        {
            System.Xml.XmlNode root = Document.CreateElement("Item");
            root.Attributes.Append(Document.CreateAttribute("HeadName"));
            root.Attributes.Append(Document.CreateAttribute("HeadSID"));
            root.Attributes.Append(Document.CreateAttribute("Conflict"));
            root.Attributes["HeadName"].Value = Item.BaseRule.Variable.LocalName;
            root.Attributes["HeadSID"].Value = Item.BaseRule.Variable.SID.ToString("X");
            root.Attributes["Conflict"].Value = GetXMLData_ConflictType(Set, Item).ToString();

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

            System.Xml.XmlNode lookaheads = Document.CreateElement("Lookaheads");
            foreach (Terminal terminal in Item.Lookaheads)
            {
                System.Xml.XmlNode lookahead = terminal.GetXMLNode(Document);
                lookaheads.AppendChild(lookahead);
            }
            root.AppendChild(lookaheads);
            return root;
        }
        protected ConflictType GetXMLData_ConflictType(State Set, Item Item)
        {
            foreach (Conflict conflict in Set.Conflicts)
                if (conflict.ContainsItem(Item))
                    return conflict.ConflictType;
            return ConflictType.None;
        }

        public virtual List<string> SerializeVisuals(string directory, bool doVisualLayout)
        {
            Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("Parser", directory + "\\GraphParser.dot");
            foreach (State set in graph.Sets)
                serializer.WriteNode(set.ID.ToString("X"), set.ID.ToString("X"), "Set_" + set.ID.ToString("X") + ".html");
            foreach (State set in graph.Sets)
                foreach (Symbol symbol in set.Children.Keys)
                    serializer.WriteEdge(set.ID.ToString("X"), set.Children[symbol].ID.ToString("X"), symbol.LocalName);
            serializer.Close();
            List<string> files = new List<string>();
            files.Add(directory + "\\GraphParser.dot");
            if (doVisualLayout)
            {
                Kernel.Graphs.DOTLayoutManager layout = new Kernel.Graphs.DOTExternalLayoutManager();
                layout.Render(directory + "\\GraphParser.dot", directory + "\\GraphParser.svg");
                files.Add(directory + "\\GraphParser.svg");
            }
            return files;
        }
    }
}
