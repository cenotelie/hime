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
using Hime.Kernel.Reporting;
using Hime.Kernel.Resources;
using Hime.Kernel.Documentation;
using Hime.Kernel.Graphs;

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
        protected virtual void ExportAdditionalStaticElements(StreamWriter stream, string className) { }
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
		
		// TODO: this method could be factored more (look at the similar code)
        public void Document(string file, bool exportVisuals, string dotBin)
        {
            string directory = file + "_temp";
            System.IO.Directory.CreateDirectory(directory);

            using (ResourceAccessor accessor = new ResourceAccessor())
			{
	            MHTMLCompiler compiler = new MHTMLCompiler("Documentation " + grammar.LocalName);
                compiler.AddSource(new MHTMLSource("text/css", "hime_data/Hime.css", accessor.GetStreamFor("Transforms.Hime.css")));
            	compiler.AddSource(new MHTMLSource("text/javascript", "hime_data/Hime.js", accessor.GetStreamFor("Transforms.Hime.js")));
            	compiler.AddSource(new MHTMLSource("image/gif", "hime_data/button_plus.gif", accessor.GetStreamFor("Visuals.button_plus.gif")));
            	compiler.AddSource(new MHTMLSource("image/gif", "hime_data/button_minus.gif", accessor.GetStreamFor("Visuals.button_minus.gif")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.Logo.png", accessor.GetStreamFor("Visuals.Hime.Logo.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.GoTo.png", accessor.GetStreamFor("Visuals.Hime.GoTo.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.Info.png", accessor.GetStreamFor("Visuals.Hime.Info.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.Warning.png", accessor.GetStreamFor("Visuals.Hime.Warning.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.Error.png", accessor.GetStreamFor("Visuals.Hime.Error.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.Shift.png", accessor.GetStreamFor("Visuals.Hime.Shift.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.Reduce.png", accessor.GetStreamFor("Visuals.Hime.Reduce.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.None.png", accessor.GetStreamFor("Visuals.Hime.None.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.ShiftReduce.png", accessor.GetStreamFor("Visuals.Hime.ShiftReduce.png")));
            	compiler.AddSource(new MHTMLSource("image/png", "hime_data/Hime.ReduceReduce.png", accessor.GetStreamFor("Visuals.Hime.ReduceReduce.png")));

                XmlDocument document = new XmlDocument();
                XmlNode nodeGraph = this.graph.Serialize(document);
                document.AppendChild(nodeGraph);
                document.Save(directory + "\\data.xml");

                // generate index
                accessor.CheckOut("Transforms.Doc.Index.xslt", directory + "\\Index.xslt");
                XslCompiledTransform transform = new XslCompiledTransform();
                transform.Load(directory + "\\Index.xslt");
                transform.Transform(directory + "\\data.xml", directory + "\\index.html");
                compiler.AddSource(new MHTMLSource("text/html", "index.html", directory + "\\index.html"));
                accessor.AddCheckoutFile(directory + "\\index.html");

                // generate sets
                string tfile = GetTransformation(exportVisuals);
                accessor.CheckOut("Transforms.Doc." + tfile + ".xslt", directory + "\\" + tfile + ".xslt");
                transform = new XslCompiledTransform();
                transform.Load(directory + "\\" + tfile + ".xslt");
                foreach (XmlNode child in nodeGraph.ChildNodes)
                {
                    string temp = directory + "\\Set_" + child.Attributes["SetID"].Value;
                    XmlDocument tempXML = new XmlDocument();
                    tempXML.AppendChild(tempXML.ImportNode(child, true));
                    tempXML.Save(temp + ".xml");
                    accessor.AddCheckoutFile(temp + ".xml");
                    transform.Transform(temp + ".xml", temp + ".html");
                    compiler.AddSource(new MHTMLSource("text/html", "Set_" + child.Attributes["SetID"].Value + ".html", temp + ".html"));
                    accessor.AddCheckoutFile(temp + ".html");
                }

            	document = new XmlDocument();
            	document.AppendChild(SerializeGrammar(document));
            	document.Save(directory + "\\data.xml");
            	accessor.AddCheckoutFile(directory + "\\data.xml");

            	// generate grammar
            	accessor.CheckOut("Transforms.Doc.Grammar.xslt", directory + "\\Grammar.xslt");
                transform = new XslCompiledTransform();
          		transform.Load(directory + "\\Grammar.xslt");
         	   	transform.Transform(directory + "\\data.xml", directory + "\\grammar.html");
         	   	compiler.AddSource(new MHTMLSource("text/html", "grammar.html", directory + "\\grammar.html"));
            	accessor.AddCheckoutFile(directory + "\\grammar.html");

            	// export parser data
            	List<string> files = SerializeVisuals(directory, exportVisuals, dotBin);
            	foreach (string vfile in files)
            	{
                	FileInfo info = new FileInfo(vfile);
					string mime;
                	if (vfile.EndsWith(".svg")) mime = "image/svg+xml";
            	    else mime = "text/plain";
					MHTMLSource source = new MHTMLSource(mime, info.Name, vfile);
					compiler.AddSource(source);
    	            accessor.AddCheckoutFile(vfile);
	            }
            	compiler.CompileTo(file);
			}
            Directory.Delete(directory, true);
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
            DOTSerializer serializer = new DOTSerializer("Parser", Path.Combine(directory, "GraphParser.dot"));
			graph.SerializeVisual(serializer);
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

        protected XmlNode SerializeGrammar(XmlDocument document)
        {
            XmlNode root = document.CreateElement("CFGrammar");
            root.Attributes.Append(document.CreateAttribute("Name"));
            root.Attributes["Name"].Value = grammar.LocalName;
            foreach (Variable var in grammar.Variables)
                root.AppendChild(var.GetXMLNodeWithRules(document));
            return root;
        }
    }
}
