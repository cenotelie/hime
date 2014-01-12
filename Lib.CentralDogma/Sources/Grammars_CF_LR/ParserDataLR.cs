/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using Hime.Redist;
using Hime.Redist.Parsers;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    abstract class ParserDataLR : ParserData
    {
        protected Reporting.Reporter reporter;
        protected CFGrammar grammar;
        protected Graph graph;
        protected List<Terminal> terminals;
        protected List<Variable> variables;
        protected List<Virtual> virtuals;
        protected List<Action> actions;
        protected List<Rule> rules;
        protected string terminalsAccessor;

        abstract protected string BaseClassName { get; }

        public ParserDataLR(Reporting.Reporter reporter, CFGrammar gram, Graph graph)
        {
            this.reporter = reporter;
            this.grammar = gram;
            this.graph = graph;
            this.variables = new List<Variable>(gram.Variables);
            this.virtuals = new List<Virtual>(gram.Virtuals);
            this.actions = new List<Action>(gram.Actions);
            this.rules = new List<Rule>(this.grammar.Rules);
        }

        public abstract void ExportData(BinaryWriter stream);

        protected void ExportDataProduction(BinaryWriter stream, Rule rule)
        {
            stream.Write((ushort)variables.IndexOf(rule.Head));
            if (rule.ReplaceOnProduction) stream.Write((byte)TreeAction.Replace);
            else stream.Write((byte)TreeAction.None);
            stream.Write((byte)(rule as CFRule).CFBody.GetChoiceAt(0).Length);
            byte length = 0;
            foreach (RuleBodyElement elem in rule.Body.Parts)
            {
                if (elem.Symbol is Virtual || elem.Symbol is Action)
                    length += 2;
                else
                    length += 1;
            }
            stream.Write(length);
            foreach (RuleBodyElement elem in rule.Body.Parts)
            {
                if (elem.Symbol is Virtual)
                {
                    if (elem.Action == RuleBodyElementAction.Drop) stream.Write((ushort)LROpCodeValues.VirtualDrop);
                    else if (elem.Action == RuleBodyElementAction.Promote) stream.Write((ushort)LROpCodeValues.VirtualPromote);
                    else stream.Write((ushort)LROpCodeValues.VirtualNoAction);
                    stream.Write((ushort)virtuals.IndexOf(elem.Symbol as Virtual));
                }
                else if (elem.Symbol is Action)
                {
                    stream.Write((ushort)LROpCodeValues.SemanticAction);
                    stream.Write((ushort)actions.IndexOf(elem.Symbol as Action));
                }
                else
                {
                    if (elem.Action == RuleBodyElementAction.Drop) stream.Write((ushort)LROpCodeValues.PopDrop);
                    else if (elem.Action == RuleBodyElementAction.Promote) stream.Write((ushort)LROpCodeValues.PopPromote);
                    else stream.Write((ushort)LROpCodeValues.PopNoAction);
                }
            }
        }

        // TODO: think about it, but shouldn't stream be a field of the class? or create a new class?
        public void ExportCode(StreamWriter stream, string name, AccessModifier modifier, string resource, IList<Terminal> expected)
        {
            this.terminals = new List<Terminal>(expected);

            stream.WriteLine("    " + modifier.ToString().ToLower() + " class " + name + "Parser : " + this.BaseClassName);
            stream.WriteLine("    {");
            ExportAutomaton(stream, name, resource);
            ExportVariables(stream);
            ExportVirtuals(stream);
            ExportActions(stream);
            ExportConstructor(stream, name);
            stream.WriteLine("    }");
        }

        protected abstract void ExportAutomaton(StreamWriter stream, string name, string resource);

        protected void ExportVariables(StreamWriter stream)
        {
            stream.WriteLine("        private static readonly Symbol[] variables = {");
            bool first = true;
            foreach (Variable var in variables)
            {
                if (!first) stream.WriteLine(", ");
                stream.Write("            ");
                stream.Write("new Symbol(0x" + var.SID.ToString("X") + ", \"" + var.Name + "\")");
                first = false;
            }
            stream.WriteLine(" };");
        }

        protected void ExportVirtuals(StreamWriter stream)
        {
            stream.WriteLine("        private static readonly Symbol[] virtuals = {");
            bool first = true;
            foreach (Virtual v in virtuals)
            {
                if (!first) stream.WriteLine(", ");
                stream.Write("            ");
                stream.Write("new Symbol(0, \"" + v.Name + "\")");
                first = false;
            }
            stream.WriteLine(" };");
        }

        protected void ExportActions(StreamWriter stream)
        {
            if (actions.Count == 0)
                return;
            stream.WriteLine("        public sealed class Actions");
            stream.WriteLine("        {");
            stream.WriteLine("            private void DoNothing(Variable head, Symbol[] body, int length) { }");
            stream.WriteLine("            private SemanticAction nullAction;");
            stream.WriteLine("            public SemanticAction NullAction { get { return nullAction; } }");
            stream.WriteLine("            private SemanticAction[] raw;");
            stream.WriteLine("            internal SemanticAction[] RawActions { get { return raw; } }");
            stream.WriteLine("            public Actions()");
            stream.WriteLine("            {");
            stream.WriteLine("                nullAction = new SemanticAction(DoNothing);");
            stream.WriteLine("                raw = new SemanticAction[" + actions.Count + "];");
            for (int i = 0; i != actions.Count; i++)
                stream.WriteLine("                raw[" + i + "] = nullAction;");
            stream.WriteLine("            }");
            for (int i = 0; i != actions.Count; i++)
            {
                stream.WriteLine("            public SemanticAction " + actions[i].Name);
                stream.WriteLine("            {");
                stream.WriteLine("                get { return raw[" + i + "]; }");
                stream.WriteLine("                set { raw[" + i + "] = value; }");
                stream.WriteLine("            }");
            }
            stream.WriteLine("        }");
        }

        protected virtual void ExportConstructor(StreamWriter stream, string name)
        {
            if (actions.Count == 0)
            {
                stream.WriteLine("        public " + name + "Parser(" + name + "Lexer lexer) : base (automaton, variables, virtuals, null, lexer) { }");
            }
            else
            {
                stream.WriteLine("        public " + name + "Parser(" + name + "Lexer lexer) : base (automaton, variables, virtuals, (new Actions()).RawActions, lexer) { }");
                stream.WriteLine("        public " + name + "Parser(" + name + "Lexer lexer, Actions actions) : base (automaton, variables, virtuals, actions.RawActions, lexer) { }");
            }
        }
        
		// TODO: this method could be factored more (look at the similar code)
        public void Document(string directory)
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
            	List<string> files = SerializeVisuals(directory);
			}
        }

        protected List<string> SerializeVisuals(string directory)
        {
            List<string> files = new List<string>();
            SerializeGraphVisual(directory, files);
            SerializeSpecifics(directory, files);
            return files;
        }

        protected void SerializeGraphVisual(string directory, List<string> results)
        {
            Documentation.DOTSerializer serializer = new Documentation.DOTSerializer("Parser", Path.Combine(directory, "GraphParser.dot"));
			graph.SerializeVisual(serializer);
            serializer.Close();
            results.Add(Path.Combine(directory, "GraphParser.dot"));
        }

        protected virtual void SerializeSpecifics(string directory, List<string> results) { }

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
