/*
 * Author: Charles Hymans
 * Date: 06/08/2011
 * Time: 23:03
 * 
 */
using System.Xml;
using System.Xml.Xsl;
using System.Collections.Generic;
using System.IO;
using Hime.Utils.Reporting;
using Hime.Utils.Resources;
using Hime.Utils.Documentation;
using Hime.Utils.Graphs;

namespace Hime.Parsers.ContextFree.LR
{
    abstract class ParserDataLR : ParserData
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
        internal protected CFVariable GetVariable(string name) { return grammar.GetVariable(name) as CFVariable; }
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
            ExportAdditionalStaticElements(stream, className);
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
                stream.Write("new SymbolVariable(0x" + var.SID.ToString("X") + ", \"" + var.Name + "\")");
                first = false;
            }
            stream.WriteLine(" };");
        }
		
		protected abstract void ExportRules(StreamWriter stream);
		protected abstract void ExportStates(StreamWriter stream);
		protected abstract void ExportActions(StreamWriter stream);
		protected abstract void ExportSetup(StreamWriter stream);
		
		// TODO: try to get rid of this method
        protected virtual void ExportAdditionalStaticElements(StreamWriter stream, string className) { }
        // TODO: try to get rid of this method
		protected virtual void ExportAdditionalElements(StreamWriter stream, string className) { }
			
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
                    stream.WriteLine("            parser.actions." + action.Name + "(root);");
                }
                else if (part.Symbol is Virtual)
                    stream.WriteLine("            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual(\"" + part.Symbol.Name + "\"), SyntaxTreeNodeAction." + part.Action.ToString() + "));");
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
		
		// TODO: this method could be factored more (look at the similar code)
        public void Document(string directory, bool exportVisuals, string dotBin)
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
            Directory.CreateDirectory(directory);
            string dirRes = Path.Combine(directory, "resources");
            Directory.CreateDirectory(dirRes);

            using (ResourceAccessor accessor = new ResourceAccessor())
			{
                accessor.Export("Transforms.Hime.css", Path.Combine(dirRes, "Hime.css"));
                accessor.Export("Transforms.Hime.js", Path.Combine(dirRes, "Hime.js"));
                accessor.Export("Visuals.button_plus.gif", Path.Combine(dirRes, "button_plus.gif"));
                accessor.Export("Visuals.button_minus.gif", Path.Combine(dirRes, "button_minus.gif"));
                accessor.Export("Visuals.Hime.Logo.png", Path.Combine(dirRes, "Hime.Logo.png"));
                accessor.Export("Visuals.Hime.GoTo.png", Path.Combine(dirRes, "Hime.GoTo.png"));
                accessor.Export("Visuals.Hime.Info.png", Path.Combine(dirRes, "Hime.Info.png"));
                accessor.Export("Visuals.Hime.Warning.png", Path.Combine(dirRes, "Hime.Warning.png"));
                accessor.Export("Visuals.Hime.Error.png", Path.Combine(dirRes, "Hime.Error.png"));
                accessor.Export("Visuals.Hime.Shift.png", Path.Combine(dirRes, "Hime.Shift.png"));
                accessor.Export("Visuals.Hime.Reduce.png", Path.Combine(dirRes, "Hime.Reduce.png"));
                accessor.Export("Visuals.Hime.None.png", Path.Combine(dirRes, "Hime.None.png"));
                accessor.Export("Visuals.Hime.ShiftReduce.png", Path.Combine(dirRes, "Hime.ShiftReduce.png"));
                accessor.Export("Visuals.Hime.ReduceReduce.png", Path.Combine(dirRes, "Hime.ReduceReduce.png"));

                XmlDocument document = new XmlDocument();
                XmlNode nodeGraph = this.graph.Serialize(document);
                document.AppendChild(nodeGraph);
                document.Save(Path.Combine(directory, "data.xml"));
                accessor.AddCheckoutFile(Path.Combine(directory, "data.xml"));

                // generate index
                accessor.CheckOut("Transforms.Doc.Index.xslt", Path.Combine(directory, "Index.xslt"));
                XslCompiledTransform transform = new XslCompiledTransform();
                transform.Load(Path.Combine(directory, "Index.xslt"));
                transform.Transform(Path.Combine(directory, "data.xml"), Path.Combine(directory, "index.html"));
                
                // generate sets
                accessor.CheckOut("Transforms.Doc.ParserData.xslt", Path.Combine(directory, "ParserData.xslt"));
                transform = new XslCompiledTransform();
                transform.Load(Path.Combine(directory, "ParserData.xslt"));
                foreach (XmlNode child in nodeGraph.ChildNodes)
                {
                    string temp = Path.Combine(directory, "set_" + child.Attributes["SetID"].Value);
                    XmlDocument tempXML = new XmlDocument();
                    tempXML.AppendChild(tempXML.ImportNode(child, true));
                    tempXML.Save(temp + ".xml");
                    accessor.AddCheckoutFile(temp + ".xml");
                    transform.Transform(temp + ".xml", temp + ".html");
                }

                // generate grammar
            	document = new XmlDocument();
            	document.AppendChild(SerializeGrammar(document));
                document.Save(Path.Combine(directory, "data.xml"));
            	accessor.CheckOut("Transforms.Doc.Grammar.xslt", Path.Combine(directory, "Grammar.xslt"));
                transform = new XslCompiledTransform();
                transform.Load(Path.Combine(directory, "Grammar.xslt"));
                transform.Transform(Path.Combine(directory, "data.xml"), Path.Combine(directory, "grammar.html"));

            	// export parser data
            	List<string> files = SerializeVisuals(directory, exportVisuals, dotBin);
			}
        }

        protected List<string> SerializeVisuals(string directory, bool exportVisuals, string dotBin)
        {
            List<string> files = new List<string>();
            SerializeGraphVisual(directory, exportVisuals, dotBin, files);
            SerializeSpecifics(directory, exportVisuals, dotBin, files);
            return files;
        }

        protected void SerializeGraphVisual(string directory, bool exportVisuals, string dotBin, List<string> results)
        {
            DOTSerializer serializer = new DOTSerializer("Parser", Path.Combine(directory, "GraphParser.dot"));
			graph.SerializeVisual(serializer);
            serializer.Close();
            results.Add(Path.Combine(directory, "GraphParser.dot"));

            if (exportVisuals)
            {
                DOTLayoutManager layout = new DOTExternalLayoutManager(dotBin);
                layout.Render(Path.Combine(directory, "GraphParser.dot"), Path.Combine(directory, "GraphParser.svg"));
                results.Add(Path.Combine(directory, "GraphParser.svg"));
            }
        }

        protected virtual void SerializeSpecifics(string directory, bool exportVisuals, string dotBin, List<string> results) { }

        protected XmlNode SerializeGrammar(XmlDocument document)
        {
            XmlNode root = document.CreateElement("CFGrammar");
            root.Attributes.Append(document.CreateAttribute("Name"));
            root.Attributes["Name"].Value = grammar.Name;
            foreach (Variable var in grammar.Variables)
                root.AppendChild(var.GetXMLNodeWithRules(document));
            return root;
        }
    }
}
