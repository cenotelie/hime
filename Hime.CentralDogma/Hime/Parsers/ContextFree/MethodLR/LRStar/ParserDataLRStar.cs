using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ParserDataLRStar : ParserData
    {
        private Kernel.Reporting.Reporter reporter;
        private Dictionary<State, DeciderLRStar> deciders;
        private System.IO.StreamWriter stream;
        private string terminalsAccessor;

        public ParserDataLRStar(ParserGenerator generator, CFGrammar gram, Graph graph, Dictionary<State, DeciderLRStar> deciders)
            : base(generator, gram, graph)
        {
            this.deciders = deciders;
        }

        public override bool Export(IList<Terminal> expected, CompilationTask options)
        {
            reporter = options.Reporter;
            terminals = new List<Terminal>(expected);
            debug = options.ExportDebug;
            terminalsAccessor = grammar.LocalName + "_Lexer.terminals";
            stream = options.ParserWriter;
            stream.Write("    " + options.GeneratedCodeModifier.ToString().ToLower() + " class " + grammar.LocalName + "_Parser : ");
            stream.WriteLine("LRStarBaseParser");
            stream.WriteLine("    {");
            Export_Variables(stream);
            foreach (CFRule rule in grammar.Rules)
                Export_Production(rule);
            Export_Rules();
            Export_States();
            Export_Actions();
            Export_Setup();
            Export_Constructor();
            stream.WriteLine("    }");
            return true;
        }
        protected void Export_Setup()
        {
            stream.WriteLine("        protected override void setup()");
            stream.WriteLine("        {");
            stream.WriteLine("            rules = staticRules;");
            stream.WriteLine("            states = staticStates;");
            stream.WriteLine("            errorSimulationLength = 3;");
            stream.WriteLine("        }");
        }
        protected void Export_Constructor()
        {
            if (grammar.Actions.GetEnumerator().MoveNext())
            {
                stream.WriteLine("        private Actions actions;");
                stream.WriteLine("        public " + grammar.LocalName + "_Parser(" + grammar.LocalName + "_Lexer lexer, Actions actions) : base (lexer) { this.actions = actions; }");
            }
            else
            {
                stream.WriteLine("        public " + grammar.LocalName + "_Parser(" + grammar.LocalName + "_Lexer lexer) : base (lexer) {}");
            }
        }
        protected void Export_Actions()
        {
            List<string> Names = new List<string>();
            foreach (Action action in grammar.Actions)
                if (!Names.Contains(action.LocalName))
                    Names.Add(action.LocalName);

            if (Names.Count != 0)
            {
                stream.WriteLine("        public interface Actions");
                stream.WriteLine("        {");
                foreach (string name in Names)
                    stream.WriteLine("            void " + name + "(SyntaxTreeNode SubRoot);");
                stream.WriteLine("        }");
            }
        }
        protected void Export_Production(CFRule rule)
        {
            int length = rule.Definition.GetChoiceAtIndex(0).Length;
            stream.WriteLine("        private static SyntaxTreeNode Production_" + rule.Variable.SID.ToString("X") + "_" + rule.ID.ToString("X") + " (LRParser baseParser)");
            stream.WriteLine("        {");
            stream.WriteLine("            " + grammar.LocalName + "_Parser parser = baseParser as " + grammar.LocalName + "_Parser;");
            if (length != 0)
            {
                stream.WriteLine("            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;");
                stream.WriteLine("            LinkedListNode<SyntaxTreeNode> temp = null;");
                for (int i = 1; i != length; i++)
                    stream.WriteLine("            current = current.Previous;");
            }
            stream.Write("            SyntaxTreeNode root = new SyntaxTreeNode(variables[" + this.variables.IndexOf(rule.Variable) + "]");
            if (rule.ReplaceOnProduction)
                stream.WriteLine(", SyntaxTreeNodeAction.Replace);");
            else
                stream.WriteLine(");");

            foreach (RuleDefinitionPart part in rule.Definition.Parts)
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
                    if (part.Action != RuleDefinitionPartAction.Drop)
                    {
                        stream.Write("            root.AppendChild(current.Value");
                        if (part.Action != RuleDefinitionPartAction.Nothing)
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
        protected void Export_Rules()
        {
            stream.WriteLine("        private static LRRule[] staticRules = {");
            bool first = true;
            foreach (CFRule Rule in grammar.Rules)
            {
                stream.Write("           ");
                if (!first) stream.Write(", ");
                string production = "Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X");
                string head = "variables[" + this.variables.IndexOf(Rule.Variable) + "]";
                stream.WriteLine("new LRRule(" + production + ", " + head + ", " + Rule.Definition.GetChoiceAtIndex(0).Length + ")");
                first = false;
            }
            stream.WriteLine("        };");
        }


        protected void Export_DeciderState(DeciderLRStar decider, DeciderStateLRStar state)
        {
            stream.WriteLine("new DeciderState(");
            // write transitions
            stream.Write("                   new ushort[" + state.Transitions.Count + "] {");
            bool first = true;
            foreach (Terminal t in state.Transitions.Keys)
            {
                if (!first) stream.Write(", ");
                stream.Write("0x" + t.SID.ToString("X"));
                first = false;
            }
            stream.WriteLine("},");
            stream.Write("                   new ushort[" + state.Transitions.Count + "] {");
            first = true;
            foreach (Terminal t in state.Transitions.Keys)
            {
                DeciderStateLRStar child = state.Transitions[t];
                if (!first) stream.Write(", ");
                stream.Write("0x" + child.ID.ToString("X"));
                first = false;
            }
            stream.Write("}");

            // write shift decision
            if (state.Decision != -1)
            {
                Item item = decider.GetItem(state.Decision);
                if (item.Action == ItemAction.Shift)
                    stream.Write(", 0x" + decider.LRState.Children[item.NextSymbol].ID.ToString("X") + ", null");
                else
                    stream.Write(", 0xFFFF, staticRules[0x" + grammar.Rules.IndexOf(item.BaseRule).ToString("X") + "]");
            }
            else
                stream.Write(", 0xFFFF, null");
            stream.WriteLine(")");
        }
        
        protected void Export_State(State state)
        {
            TerminalSet expected = state.Reductions.ExpectedTerminals;
            foreach (Symbol Symbol in state.Children.Keys)
            {
                if (Symbol is Terminal)
                    expected.Add((Terminal)Symbol);
            }
            bool first = true;
            stream.WriteLine("new LRStarState(");
            // Write items
            if (debug)
            {
                stream.Write("               new string[" + state.Items.Count + "] {");
                first = true;
                foreach (Item item in state.Items)
                {
                    if (!first) stream.Write(", ");
                    stream.Write("\"" + item.ToString(true) + "\"");
                    first = false;
                }
                stream.WriteLine("},");
            }
            else
            {
                stream.WriteLine("               null,");
            }
            // Write terminals
            stream.Write("               new SymbolTerminal[" + expected.Count + "] {");
            first = true;
            foreach (Terminal terminal in expected)
            {
                int index = terminals.IndexOf(terminal);
                if (index == -1)
                    reporter.Error("Grammar", "In state " + state.ID.ToString("X") + " expected terminal " + terminal.ToString() + " cannot be produced by the parser. Check the regular expressions.");
                if (!first) stream.Write(", ");
                stream.Write(terminalsAccessor + "[" + index + "]");
                first = false;
            }
            stream.WriteLine("},");

            int ShitTerminalCount = 0;
            foreach (Symbol Symbol in state.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                ShitTerminalCount++;
            }
            // write submachine
            stream.WriteLine("               new DeciderState[" + deciders[state].States.Count + "] {");
            first = true;
            foreach (DeciderStateLRStar sub in deciders[state].States)
            {
                stream.Write("                   ");
                if (!first) stream.Write(", ");
                Export_DeciderState(deciders[state], sub);
                first = false;
            }
            stream.WriteLine("               },");
            // Write shifts on variable
            stream.Write("               new ushort[" + (state.Children.Count - ShitTerminalCount).ToString() + "] {");
            first = true;
            foreach (Symbol Symbol in state.Children.Keys)
            {
                if (!(Symbol is Variable))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("0x" + Symbol.SID.ToString("x"));
                first = false;
            }
            stream.WriteLine("},");
            stream.Write("               new ushort[" + (state.Children.Count - ShitTerminalCount).ToString() + "] {");
            first = true;
            foreach (Symbol Symbol in state.Children.Keys)
            {
                if (!(Symbol is Variable))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("0x" + state.Children[Symbol].ID.ToString("X"));
                first = false;
            }
            stream.WriteLine("})");
        }
        protected void Export_States()
        {
            stream.WriteLine("        private static LRStarState[] staticStates = {");
            bool first = true;
            foreach (State State in graph.States)
            {
                stream.Write("            ");
                if (!first) stream.Write(", ");
                Export_State(State);
                first = false;
            }
            stream.WriteLine("        };");
        }


        public override List<string> SerializeVisuals(string directory, CompilationTask options)
        {
            List<string> files = base.SerializeVisuals(directory, options);
            foreach (State state in deciders.Keys)
            {
                Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("State_" + state.ID.ToString(), directory + "\\Set_" + state.ID.ToString("X") + ".dot");
                Serialize_Deciders(deciders[state], serializer);
                serializer.Close();
                files.Add(directory + "\\Set_" + state.ID.ToString("X") + ".dot");
                if (options.ExportVisuals)
                {
                    Kernel.Graphs.DOTLayoutManager layout = new Kernel.Graphs.DOTExternalLayoutManager(options.DOTBinary);
                    layout.Render(directory + "\\Set_" + state.ID.ToString("X") + ".dot", directory + "\\Set_" + state.ID.ToString("X") + ".svg");
                    files.Add(directory + "\\Set_" + state.ID.ToString("X") + ".svg");
                }
            }
            return files;
        }

        private void Serialize_Deciders(DeciderLRStar machine, Kernel.Graphs.DOTSerializer serializer)
        {
            foreach (DeciderStateLRStar state in machine.States)
            {
                string id = state.ID.ToString();
                string label = id;
                if (state.Decision != -1)
                {
                    Item item = machine.GetItem(state.Decision);
                    if (item.Action == ItemAction.Shift)
                        label = "SHIFT: " + machine.LRState.Children[item.NextSymbol].ID.ToString("X");
                    else
                        label = item.BaseRule.ToString();
                }
                serializer.WriteNode(id, label);
            }
            foreach (DeciderStateLRStar state in machine.States)
                foreach (Terminal t in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[t].ID.ToString(), t.ToString().Replace("\"", "\\\""));
        }
    }
}
