using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ParserDataLRA : ParserDataLR
    {
        private Dictionary<State, SubMachine> submachines;
        private System.IO.StreamWriter stream;

        public ParserDataLRA(ParserGenerator generator, CFGrammar gram, Graph graph, Dictionary<State, SubMachine> submachines)
            : base(generator, gram, graph)
        {
            this.submachines = submachines;
        }

        public override bool Export(GrammarBuildOptions Options)
        {
            stream = Options.ParserWriter;
            stream.Write("    class " + grammar.LocalName + "_Parser : ");
            stream.WriteLine("Hime.Redist.Parsers.BaseLRAParser");
            stream.WriteLine("    {");
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
                    stream.WriteLine("            void " + name + "(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);");
                stream.WriteLine("        }");
            }
        }
        protected void Export_Production(CFRule Rule)
        {
            string ParserLength = Rule.Definition.GetChoiceAtIndex(0).Length.ToString();

            stream.WriteLine("        private static void Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X") + " (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)");
            stream.WriteLine("        {");
            if (ParserLength != "0")
            {
                stream.WriteLine("            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - " + ParserLength + ", " + ParserLength + ");");
                stream.WriteLine("            nodes.RemoveRange(nodes.Count - " + ParserLength + ", " + ParserLength + ");");
            }
            stream.Write("            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x" + Rule.Variable.SID.ToString("X") + ", \"" + Rule.Variable.LocalName + "\")");
            if (Rule.ReplaceOnProduction)
                stream.WriteLine(", Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);");
            else
                stream.WriteLine(");");

            int i = 0;
            foreach (RuleDefinitionPart Part in Rule.Definition.Parts)
            {
                if (Part.Symbol is Action)
                {
                    Action action = (Action)Part.Symbol;
                    stream.WriteLine("            ((" + grammar.LocalName + "_Parser)parser).actions." + action.LocalName + "(SubRoot);");
                }
                else if (Part.Symbol is Virtual)
                    stream.WriteLine("            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual(\"" + Part.Symbol.LocalName + "\"), Hime.Redist.Parsers.SyntaxTreeNodeAction." + Part.Action.ToString() + "));");
                else if (Part.Symbol is Terminal || Part.Symbol is Variable)
                {
                    stream.Write("            SubRoot.AppendChild(Definition[" + i.ToString() + "]");
                    if (Part.Action != RuleDefinitionPartAction.Nothing)
                        stream.WriteLine(", Hime.Redist.Parsers.SyntaxTreeNodeAction." + Part.Action.ToString() + ");");
                    else
                        stream.WriteLine(");");
                    i++;
                }
            }
            stream.WriteLine("            nodes.Add(SubRoot);");
            stream.WriteLine("        }");
        }
        protected void Export_Rules()
        {
            stream.WriteLine("        private static Rule[] staticRules = {");
            bool first = true;
            foreach (CFRule Rule in grammar.Rules)
            {
                stream.Write("           ");
                if (!first) stream.Write(", ");
                string production = "Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X");
                string head = "new Hime.Redist.Parsers.SymbolVariable(0x" + Rule.Variable.SID.ToString("X") + ", \"" + Rule.Variable.LocalName + "\")";
                stream.WriteLine("new Rule(" + production + ", " + head + ", " + Rule.Definition.GetChoiceAtIndex(0).Length + ")");
                first = false;
            }
            stream.WriteLine("        };");
        }


        protected void Export_DeciderState(SubState state)
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
                SubState child = state.Transitions[t];
                if (!first) stream.Write(", ");
                stream.Write("0x" + child.ID.ToString("X"));
                first = false;
            }
            stream.Write("}");

            // write shift decision
            if (state.ShiftDecision != null)
                stream.Write(", 0x" + state.ShiftDecision.ID.ToString("X"));
            else
                stream.Write(", 0xFFFF");
            // Write rule decision
            if (state.RuleDecision != null)
                stream.Write(", staticRules[0x" + grammar.Rules.IndexOf(state.RuleDecision).ToString("X") + "]");
            else
                stream.Write(", new Rule()");
            stream.WriteLine(")");
        }
        
        protected void Export_State(State State)
        {
            TerminalSet Terminals = State.Reductions.ExpectedTerminals;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (Symbol is Terminal)
                    Terminals.Add((Terminal)Symbol);
            }
            stream.WriteLine("new State(");
            // Write items
            stream.Write("               new string[" + State.Items.Count + "] {");
            bool first = true;
            foreach (Item item in State.Items)
            {
                if (!first) stream.Write(", ");
                stream.Write("\"" + item.ToString(true) + "\"");
                first = false;
            }
            stream.WriteLine("},");
            // Write terminals
            stream.Write("               new Terminal[" + Terminals.Count + "] {");
            first = true;
            foreach (Terminal terminal in Terminals)
            {
                if (!first) stream.Write(", ");
                stream.Write("new Terminal(\"" + terminal.LocalName + "\", 0x" + terminal.SID.ToString("X") + ")");
                first = false;
            }
            stream.WriteLine("},");

            int ShitTerminalCount = 0;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                ShitTerminalCount++;
            }
            // write submachine
            stream.WriteLine("               new DeciderState[" + submachines[State].States.Count + "] {");
            first = true;
            foreach (SubState sub in submachines[State].States)
            {
                stream.Write("                   ");
                if (!first) stream.Write(", ");
                Export_DeciderState(sub);
                first = false;
            }
            stream.WriteLine("               },");
            // Write shifts on variable
            stream.Write("               new ushort[" + (State.Children.Count - ShitTerminalCount).ToString() + "] {");
            first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Variable))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("0x" + Symbol.SID.ToString("x"));
                first = false;
            }
            stream.WriteLine("},");
            stream.Write("               new ushort[" + (State.Children.Count - ShitTerminalCount).ToString() + "] {");
            first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Variable))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("0x" + State.Children[Symbol].ID.ToString("X"));
                first = false;
            }
            stream.WriteLine("})");
        }
        protected void Export_States()
        {
            stream.WriteLine("        private static State[] staticStates = {");
            bool first = true;
            foreach (State State in graph.Sets)
            {
                stream.Write("            ");
                if (!first) stream.Write(", ");
                Export_State(State);
                first = false;
            }
            stream.WriteLine("        };");
        }
    }
}
