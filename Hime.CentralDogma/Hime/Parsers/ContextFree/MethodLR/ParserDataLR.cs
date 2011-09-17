/*
 * Author: Charles Hymans
 * Date: 06/08/2011
 * Time: 23:03
 * 
 */
using System.Xml;
using System.Collections.Generic;
using System.IO;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
    public abstract class ParserDataLR : ParserData
    {
        protected Reporter reporter;
        protected CFGrammar grammar;
        protected Graph graph;
        protected List<Terminal> terminals;
        protected List<Variable> variables;
        protected List<Rule> rules;
        protected string terminalsAccessor;
        protected bool debug;

        internal protected ICollection<Rule> GrammarRules { get { return rules; } }
        internal protected string IndexOfRule(Rule rule) { return "0x" + rules.IndexOf(rule).ToString("X"); }
        internal protected ICollection<Action> GrammarActions { get { return grammar.Actions; } }
        internal protected string GetVariable(string name) { return "0x" + grammar.GetVariable(name).SID.ToString("X"); }
        internal protected string GetOption(string name) { return this.grammar.GetOption(name); }

        public ParserDataLR(Reporter reporter, CFGrammar gram, Graph graph)
        {
            this.reporter = reporter;
            this.grammar = gram;
            this.graph = graph;
            this.variables = new List<Variable>(gram.Variables);
            this.rules = new List<Rule>(this.grammar.Rules);
        }

        public abstract void Export(StreamWriter stream, string className, AccessModifier modifier, string lexerClassName, IList<Terminal> expected, bool exportDebug);
        
        public List<string> SerializeVisuals(string directory, bool exportVisuals, string dotBin)
        {
            List<string> files = new List<string>();
            SerializeGraphVisual(directory, exportVisuals, dotBin, files);
            SerializeSpecifics(directory, exportVisuals, dotBin, files);
            return files;
        }

        public XmlNode SerializeXML(XmlDocument document)
        {
            XmlNode nodegraph = document.CreateElement("LRGraph");
            foreach (State state in graph.States)
                nodegraph.AppendChild(GetXMLDataState(document, state));
            return nodegraph;
        }

        private XmlNode GetXMLDataState(XmlDocument document, State state)
        {
            XmlNode root = document.CreateElement("ItemSet");
            root.Attributes.Append(document.CreateAttribute("SetID"));
            root.Attributes["SetID"].Value = state.ID.ToString("X");
            foreach (Item item in state.Items)
                root.AppendChild(item.GetXMLNode(document, state));
            return root;
        }


        protected void ExportVariables(StreamWriter stream)
        {
            stream.WriteLine("        public static readonly SymbolVariable[] variables = {");
            bool first = true;
            foreach (Variable var in variables)
            {
                if (!first) stream.WriteLine(", ");
                stream.Write("            ");
                stream.Write("new SymbolVariable(0x" + var.SID.ToString("X") + ", \"" + var.LocalName + "\")");
                first = false;
            }
            stream.WriteLine(" };");
        }

        protected void ExportConstructor(StreamWriter stream, string className, string lexerClassName)
        {
            string argument = "";
            string body = "";
            if (this.GrammarActions.GetEnumerator().MoveNext())
            {
                stream.WriteLine("        private Actions actions;");
                argument = ", Actions actions";
                body = "this.actions = actions;";
            }
            stream.WriteLine("        public " + className + "(" + lexerClassName + " lexer" + argument + ") : base (lexer) { " + body + " }");
        }

        protected void SerializeGraphVisual(string directory, bool exportVisuals, string dotBin, List<string> results)
        {
            Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("Parser", directory + "\\GraphParser.dot");
            foreach (State set in graph.States)
                serializer.WriteNode(set.ID.ToString("X"), set.ID.ToString("X"), "Set_" + set.ID.ToString("X") + ".html");
            foreach (State set in graph.States)
                foreach (GrammarSymbol symbol in set.Children.Keys)
                    serializer.WriteEdge(set.ID.ToString("X"), set.Children[symbol].ID.ToString("X"), symbol.ToString().Replace("\"", "\\\""));
            serializer.Close();
            results.Add(directory + "\\GraphParser.dot");

            if (exportVisuals)
            {
                Kernel.Graphs.DOTLayoutManager layout = new Kernel.Graphs.DOTExternalLayoutManager(dotBin);
                layout.Render(directory + "\\GraphParser.dot", directory + "\\GraphParser.svg");
                results.Add(directory + "\\GraphParser.svg");
            }
        }

        protected virtual void SerializeSpecifics(string directory, bool exportVisuals, string dotBin, List<string> results) { }
    }
}
