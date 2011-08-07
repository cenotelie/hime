/*
 * Author: Charles Hymans
 * Date: 06/08/2011
 * Time: 23:03
 * 
 */
using System.Xml;
using System.Collections.Generic;
using System.IO;
using Hime.Parsers.CF;
using Hime.Parsers.CF.LR;

namespace Hime.Parsers
{
    public abstract class ParserData
    {
    	protected StreamWriter stream;
        protected ParserGenerator generator;
        protected Graph graph;
        protected List<Terminal> terminals;
        protected List<Variable> variables;
        protected bool debug;

        internal protected CFGrammar Grammar { get; private set; }
        public Graph Graph { get { return graph; } }
        public ParserGenerator Generator { get { return generator; } }

        private string GrammarName { get { return this.Grammar.LocalName; } }
        internal protected string LexerName { get { return this.GrammarName + "_Lexer"; } }
        internal protected string ParserName { get { return this.GrammarName + "_Parser"; } }

        internal protected ICollection<CFRule> GrammarRules { get { return this.Grammar.Rules; } }
        
        internal protected string IndexOfRule(CFRule rule)
        {
        	return "0x" + this.Grammar.Rules.IndexOf(rule).ToString("X");
        }
        
        internal protected ICollection<Action> GrammarActions { get { return this.Grammar.Actions; } }
        
        internal protected ICollection<Variable> GrammarVariables { get { return this.Grammar.Variables; } }
        
        internal protected string GetVariable(string name)
        {
        	return "0x" + this.Grammar.GetVariable(name).SID.ToString("X");
        }
        
        internal protected string GetOption(string name)
        {
        	return this.GetVariable(this.Grammar.GetOption(name));
        }
        
        public ParserData(ParserGenerator generator, CFGrammar gram, Graph graph)
        {
            this.Grammar = gram;
            this.graph = graph;
            this.generator = generator;
            this.variables = new List<Variable>(gram.Variables);
        }

        public virtual bool Export(IList<Terminal> expected, CompilationTask options)
        {
        	ExportConstructor();
            stream.WriteLine("    }");
            return true;
        }

        public XmlNode SerializeXML(XmlDocument Document)
        {
            XmlNode nodegraph = Document.CreateElement("LRGraph");
            foreach (State set in graph.States)
            {
                nodegraph.AppendChild(GetXMLData_Set(Document, set));
            }
            return nodegraph;
        }
        
        private XmlNode GetXMLData_Set(XmlDocument document, State set)
        {
            XmlNode root = document.CreateElement("ItemSet");
            root.Attributes.Append(document.CreateAttribute("SetID"));
            root.Attributes["SetID"].Value = set.ID.ToString("X");
            foreach (Item item in set.Items)
            {
                root.AppendChild(item.GetXMLNode(document, set));
            }
            return root;
        }
        

        protected void Export_Variables(StreamWriter stream)
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

        public virtual List<string> SerializeVisuals(string directory, CompilationTask options)
        {
            Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("Parser", directory + "\\GraphParser.dot");
            foreach (State set in graph.States)
                serializer.WriteNode(set.ID.ToString("X"), set.ID.ToString("X"), "Set_" + set.ID.ToString("X") + ".html");
            foreach (State set in graph.States)
                foreach (Symbol symbol in set.Children.Keys)
                    serializer.WriteEdge(set.ID.ToString("X"), set.Children[symbol].ID.ToString("X"), symbol.ToString().Replace("\"", "\\\""));
            serializer.Close();
            List<string> files = new List<string>();
            files.Add(directory + "\\GraphParser.dot");
            if (options.ExportVisuals)
            {
                Kernel.Graphs.DOTLayoutManager layout = new Kernel.Graphs.DOTExternalLayoutManager(options.DOTBinary);
                layout.Render(directory + "\\GraphParser.dot", directory + "\\GraphParser.svg");
                files.Add(directory + "\\GraphParser.svg");
            }
            return files;
        }

		protected void ExportConstructor()
        {
          	string argument = "";
        	string body = "";
            if (this.GrammarActions.GetEnumerator().MoveNext())
            {
                stream.WriteLine("        private Actions actions;");
                argument = ", Actions actions";
                body = "this.actions = actions;";
            }
            stream.WriteLine("        public " + this.ParserName + "(" + this.LexerName + " lexer" + argument + ") : base (lexer) { " + body + " }");
        }
    }
}
