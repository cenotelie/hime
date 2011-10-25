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
        internal protected Reporter reporter;
        internal protected CFGrammar grammar;
        internal protected Graph graph;
        internal protected List<Terminal> terminals;
        internal protected List<Variable> variables;
        internal protected List<Rule> rules;
        internal protected string terminalsAccessor;
        internal protected bool debug;

        internal protected ICollection<Rule> GrammarRules { get { return rules; } }
        internal protected string IndexOfRule(Rule rule) 
		{ 
			return "0x" + rules.IndexOf(rule).ToString("X"); 
		}
		
        internal protected ICollection<Action> GrammarActions { get { return grammar.Actions; } }
        internal protected string GetVariable(string name) { return "0x" + grammar.GetVariable(name).SID.ToString("X"); }
        internal protected string GetOption(string name) { return this.grammar.GetOption(name); }
        
		internal abstract protected string GetBaseClassName { get; }
		
        public ParserDataLR(Reporter reporter, CFGrammar gram, Graph graph)
        {
            this.reporter = reporter;
            this.grammar = gram;
            this.graph = graph;
            this.variables = new List<Variable>(gram.Variables);
            this.rules = new List<Rule>(this.grammar.Rules);
        }

		// TODO: think about it, but shouldn't stream be a field of the class? or create a new class?
        public void Export(StreamWriter stream, string className, AccessModifier modifier, string lexerClassName, IList<Terminal> expected, bool exportDebug)
		{
	        this.terminals = new List<Terminal>(expected);
            this.debug = exportDebug;
            this.terminalsAccessor = lexerClassName + ".terminals";

			stream.WriteLine("    " + modifier.ToString().ToLower() + " class " + className + " : " + this.GetBaseClassName);
            stream.WriteLine("    {");
            ExportVariables(stream);
            foreach (CFRule rule in this.GrammarRules) ExportProduction(stream, rule, className);
            ExportRules(stream);
			ExportStates(stream);
            ExportActions(stream);
            ExportSetup(stream);
            ExportConstructor(stream, className, lexerClassName);
			// TODO: try to get rid of this method
			ExportAdditionalElements(stream, className);
						
			stream.WriteLine("    }");
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
		
		protected abstract void ExportRules(StreamWriter stream);
		protected abstract void ExportStates(StreamWriter stream);
		protected abstract void ExportActions(StreamWriter stream);
		protected abstract void ExportSetup(StreamWriter stream);
		
		// TODO: try to get rid of this method
		protected virtual void ExportAdditionalElements(StreamWriter stream, string className)
		{
		}
			
        private void ExportConstructor(StreamWriter stream, string className, string lexerClassName)
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
		
		protected virtual void ExportProduction(StreamWriter stream, CFRule rule, string className)
		{
            int length = rule.CFBody.GetChoiceAt(0).Length;
            stream.WriteLine("        private static SyntaxTreeNode Production_" + rule.Head.SID.ToString("X") + "_" + rule.ID.ToString("X") + " (LRParser baseParser)");
            stream.WriteLine("        {");
            if (length != 0)
            {
				stream.WriteLine("            " + className + " parser = baseParser as " + className + ";");
                stream.WriteLine("            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;");
                stream.WriteLine("            LinkedListNode<SyntaxTreeNode> temp = null;");
                for (int i = 1; i != length; i++)
                    stream.WriteLine("            current = current.Previous;");
            }
            stream.Write("            SyntaxTreeNode root = new SyntaxTreeNode(variables[" + this.variables.IndexOf(rule.Head) + "]");
            if (rule.ReplaceOnProduction)
                stream.WriteLine(", SyntaxTreeNodeAction.Replace);");
            else
                stream.WriteLine(");");

		    foreach (RuleBodyElement part in rule.CFBody.Parts)
            {
                if (part.Symbol is Action)
                {
                    Action action = (Action)part.Symbol;
                    stream.WriteLine("            parser.actions." + action.LocalName + "(root);");
                }
                else if (part.Symbol is Virtual)
                    stream.WriteLine("            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual(\"" + part.Symbol.LocalName + "\"), SyntaxTreeNodeAction." + part.Action.ToString() + "));");
                else if (part.Symbol is Terminal || part.Symbol is Variable)
                {
                    if (part.Action != RuleBodyElementAction.Drop)
                    {
                        stream.Write("            root.AppendChild(current.Value");
                        if (part.Action != RuleBodyElementAction.Nothing)
                            stream.WriteLine(", SyntaxTreeNodeAction." + part.Action.ToString() + ");");
                        else
                            stream.WriteLine(");");
                    }
                    stream.WriteLine("            temp = current.Next;");
                    stream.WriteLine("            parser.nodes.Remove(current);");
                    stream.WriteLine("            current = temp;");
                }
            }

            stream.WriteLine("            return root;");
            stream.WriteLine("        }");
		}
		
        public void Document(string file, bool exportVisuals, string dotBin)
        {
            string directory = file + "_temp";
            System.IO.Directory.CreateDirectory(directory);

            Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor();
            Kernel.Documentation.MHTMLCompiler compiler = new Kernel.Documentation.MHTMLCompiler();
            compiler.Title = "Documentation " + grammar.LocalName;
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/html", "utf-8", "index.html", accessor.GetStreamFor("Transforms.Doc.Index.html")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/html", "utf-8", "GraphParser.html", accessor.GetStreamFor("Transforms.Doc.Parser.html")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/css", "utf-8", "hime_data/Hime.css", accessor.GetStreamFor("Transforms.Hime.css")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/javascript", "utf-8", "hime_data/Hime.js", accessor.GetStreamFor("Transforms.Hime.js")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/gif", "hime_data/button_plus.gif", accessor.GetStreamFor("Visuals.button_plus.gif")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/gif", "hime_data/button_minus.gif", accessor.GetStreamFor("Visuals.button_minus.gif")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Logo.png", accessor.GetStreamFor("Visuals.Hime.Logo.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.GoTo.png", accessor.GetStreamFor("Visuals.Hime.GoTo.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Info.png", accessor.GetStreamFor("Visuals.Hime.Info.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Warning.png", accessor.GetStreamFor("Visuals.Hime.Warning.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Error.png", accessor.GetStreamFor("Visuals.Hime.Error.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Shift.png", accessor.GetStreamFor("Visuals.Hime.Shift.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Reduce.png", accessor.GetStreamFor("Visuals.Hime.Reduce.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.None.png", accessor.GetStreamFor("Visuals.Hime.None.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.ShiftReduce.png", accessor.GetStreamFor("Visuals.Hime.ShiftReduce.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.ReduceReduce.png", accessor.GetStreamFor("Visuals.Hime.ReduceReduce.png")));

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.AppendChild(SerializeGrammar(doc));
            doc.Save(directory + "\\data.xml");
            accessor.AddCheckoutFile(directory + "\\data.xml");

            // generate header
            accessor.CheckOut("Transforms.Doc.Header.xslt", directory + "\\Header.xslt");
            System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Header.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\header.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "header.html", directory + "\\header.html"));
            accessor.AddCheckoutFile(directory + "\\header.html");
            // generate grammar
            accessor.CheckOut("Transforms.Doc.Grammar.xslt", directory + "\\Grammar.xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Grammar.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\grammar.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "grammar.html", directory + "\\grammar.html"));
            accessor.AddCheckoutFile(directory + "\\grammar.html");

            doc = new System.Xml.XmlDocument();
            List<System.Xml.XmlNode> nodes = new List<System.Xml.XmlNode>();
            System.Xml.XmlNode nodeGraph = SerializeGraph(doc);
            foreach (System.Xml.XmlNode child in nodeGraph.ChildNodes)
                nodes.Add(child);

            // generate sets
            string tfile = GetTransformation(exportVisuals);
            accessor.CheckOut("Transforms.Doc." + tfile + ".xslt", directory + "\\" + tfile + ".xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\" + tfile + ".xslt");
            foreach (System.Xml.XmlNode child in nodes)
            {
                string temp = directory + "\\Set_" + child.Attributes["SetID"].Value;
                while (doc.HasChildNodes)
                    doc.RemoveChild(doc.FirstChild);
                doc.AppendChild(child);
                doc.Save(temp + ".xml");
                accessor.AddCheckoutFile(temp + ".xml");
                transform.Transform(temp + ".xml", temp + ".html");
                compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "Set_" + child.Attributes["SetID"].Value + ".html", temp + ".html"));
                accessor.AddCheckoutFile(temp + ".html");
            }

            while (doc.HasChildNodes)
                doc.RemoveChild(doc.FirstChild);
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            doc.AppendChild(nodeGraph);
            foreach (System.Xml.XmlNode child in nodes)
                nodeGraph.AppendChild(child);
            doc.Save(directory + "\\data.xml");
            // generate menu
            accessor.CheckOut("Transforms.Doc.Menu.xslt", directory + "\\Menu.xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Menu.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\menu.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "menu.html", directory + "\\menu.html"));
            accessor.AddCheckoutFile(directory + "\\menu.html");

            // export parser data
            List<string> files = SerializeVisuals(directory, exportVisuals, dotBin);
            foreach (string vfile in files)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(vfile);
                if (vfile.EndsWith(".svg"))
                    compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileImage("image/svg+xml", info.Name, vfile));
                else
                    compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/plain", "utf-8", info.Name, vfile));
                accessor.AddCheckoutFile(vfile);
            }
            compiler.CompileTo(file);
            accessor.Close();
            System.IO.Directory.Delete(directory, true);
        }

        protected virtual string GetTransformation(bool exportVisuals) { return "ParserData_LR1"; }

        protected List<string> SerializeVisuals(string directory, bool exportVisuals, string dotBin)
        {
            List<string> files = new List<string>();
            SerializeGraphVisual(directory, exportVisuals, dotBin, files);
            SerializeSpecifics(directory, exportVisuals, dotBin, files);
            return files;
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

        protected XmlNode SerializeGraph(XmlDocument document)
        {
            XmlNode nodegraph = document.CreateElement("LRGraph");
            foreach (State state in graph.States)
                nodegraph.AppendChild(SerializeGraphState(document, state));
            return nodegraph;
        }

        protected XmlNode SerializeGraphState(XmlDocument document, State state)
        {
            XmlNode root = document.CreateElement("ItemSet");
            root.Attributes.Append(document.CreateAttribute("SetID"));
            root.Attributes["SetID"].Value = state.ID.ToString("X");
            foreach (Item item in state.Items)
                root.AppendChild(item.GetXMLNode(document, state));
            return root;
        }

        protected XmlNode SerializeGrammar(XmlDocument document)
        {
            XmlNode root = document.CreateElement("CFGrammar");
            root.Attributes.Append(document.CreateAttribute("Name"));
            root.Attributes["Name"].Value = grammar.LocalName;
            foreach (Variable var in grammar.Variables)
                root.AppendChild(var.GetXMLNode(document));
            return root;
        }
    }
}
