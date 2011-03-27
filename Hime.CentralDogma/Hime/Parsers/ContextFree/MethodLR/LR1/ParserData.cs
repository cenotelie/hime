using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ParserDataLR1 : ParserDataLR
    {
        protected System.IO.StreamWriter stream;

        public ParserDataLR1(ParserGenerator generator, CFGrammar gram, Graph graph) : base(generator, gram, graph) { }


        public override bool Export(GrammarBuildOptions Options)
        {
            stream = Options.ParserWriter;
            stream.Write("    class " + grammar.LocalName + "_Parser : ");
            if (grammar is CFGrammarText)
                stream.WriteLine("Hime.Redist.Parsers.LR1TextParser");
            else
                stream.WriteLine("Hime.Redist.Parsers.LR1BinaryParser");
            stream.WriteLine("    {");
            foreach (CFRule rule in grammar.Rules)
                Export_Production(rule);
            Export_Rules();
            Export_RulesHeadID();
            Export_RulesName();
            Export_RulesParserLength();
            foreach (State set in graph.Sets)
            {
                Export_State_Expected(set);
                Export_State_Items(set);
                Export_State_ShiftOnTerminals(set);
                Export_State_ShiftOnVariables(set);
                Export_State_ReducsOnTerminal(set);
            }
            Export_StateExpectedIDs();
            Export_StateExpectedNames();
            Export_StateItems();
            Export_StateShiftsOnTerminal();
            Export_StateShiftsOnVariable();
            Export_StateReducsOnTerminal();
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
            stream.WriteLine("            rulesHeadID = staticRulesHeadID;");
            stream.WriteLine("            rulesHeadName = staticRulesHeadName;");
            stream.WriteLine("            rulesParserLength = staticRulesParserLength;");

            stream.WriteLine("            stateExpectedIDs = staticStateExpectedIDs;");
            stream.WriteLine("            stateExpectedNames = staticStateExpectedNames;");
            stream.WriteLine("            stateItems = staticStateItems;");
            stream.WriteLine("            stateShiftsOnTerminal = staticStateShiftsOnTerminal;");
            stream.WriteLine("            stateShiftsOnVariable = staticStateShiftsOnVariable;");
            stream.WriteLine("            stateReducsOnTerminal = staticStateReducsOnTerminal;");
            stream.WriteLine("            errorSimulationLength = 3;");
            stream.WriteLine("        }");
        }
        protected void Export_Constructor()
        {
            if (grammar.Actions.GetEnumerator().MoveNext())
            {
                stream.WriteLine("        private Actions actions;");
                stream.WriteLine("        public " + grammar.LocalName + "_Parser(" + grammar.LocalName + "_Lexer lexer, Actions actions) : base (lexer) { actions = actions; }");
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

            stream.WriteLine("        private static void Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X") + " (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)");
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
            stream.Write("        private static Production[] staticRules = { ");
            bool first = true;
            foreach (CFRule Rule in grammar.Rules)
            {
                if (!first) stream.Write(", ");
                stream.Write("Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_RulesHeadID()
        {
            stream.Write("        private static ushort[] staticRulesHeadID = { ");
            bool first = true;
            foreach (CFRule Rule in grammar.Rules)
            {
                if (!first) stream.Write(", ");
                stream.Write("0x" + Rule.Variable.SID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_RulesName()
        {
            stream.Write("        private static string[] staticRulesHeadName = { ");
            bool first = true;
            foreach (CFRule Rule in grammar.Rules)
            {
                if (!first) stream.Write(", ");
                stream.Write("\"" + Rule.Variable.LocalName + "\"");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_RulesParserLength()
        {
            stream.Write("        private static ushort[] staticRulesParserLength = { ");
            bool first = true;
            foreach (CFRule Rule in grammar.Rules)
            {
                if (!first) stream.Write(", ");
                stream.Write("0x" + Rule.Definition.GetChoiceAtIndex(0).Length.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_State_Expected(State State)
        {
            TerminalSet Terminals = State.Reductions.ExpectedTerminals;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (Symbol is Terminal)
                    Terminals.Add((Terminal)Symbol);
            }

            stream.Write("        private static ushort[] stateExpectedIDs_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Terminal Terminal in Terminals)
            {
                if (!first) stream.Write(", ");
                stream.Write("0x" + Terminal.SID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");

            stream.Write("        private static string[] stateExpectedNames_" + State.ID.ToString("X") + " = { ");
            first = true;
            foreach (Terminal Terminal in Terminals)
            {
                if (!first) stream.Write(", ");
                stream.Write("\"" + Terminal.LocalName + "\"");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_State_Items(State State)
        {
            stream.Write("        private static string[] stateItems_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Item item in State.Items)
            {
                if (!first) stream.Write(", ");
                stream.Write("\"" + item.ToString() + "\"");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_State_ShiftOnTerminals(State State)
        {
            stream.Write("        private static ushort[][] stateShiftsOnTerminal_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("new ushort[2] { 0x" + Symbol.SID.ToString("x") + ", 0x" + State.Children[Symbol].ID.ToString("X") + " }");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_State_ShiftOnVariables(State State)
        {
            stream.Write("        private static ushort[][] stateShiftsOnVariable_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Variable))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("new ushort[2] { 0x" + Symbol.SID.ToString("x") + ", 0x" + State.Children[Symbol].ID.ToString("X") + " }");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_State_ReducsOnTerminal(State State)
        {
            stream.Write("        private static ushort[][] stateReducsOnTerminal_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (StateActionReduce Reduction in State.Reductions.Reductions)
            {
                if (!first) stream.Write(", ");
                stream.Write("new ushort[2] { 0x" + Reduction.OnSymbol.SID.ToString("x") + ", 0x" + grammar.Rules.IndexOf(Reduction.ToReduceRule).ToString("X") + " }");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_StateExpectedIDs()
        {
            stream.Write("        private static ushort[][] staticStateExpectedIDs = { ");
            bool first = true;
            foreach (State State in graph.Sets)
            {
                if (!first) stream.Write(", ");
                stream.Write("stateExpectedIDs_" + State.ID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_StateExpectedNames()
        {
            stream.Write("        private static string[][] staticStateExpectedNames = { ");
            bool first = true;
            foreach (State State in graph.Sets)
            {
                if (!first) stream.Write(", ");
                stream.Write("stateExpectedNames_" + State.ID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_StateItems()
        {
            stream.Write("        private static string[][] staticStateItems = { ");
            bool first = true;
            foreach (State State in graph.Sets)
            {
                if (!first) stream.Write(", ");
                stream.Write("stateItems_" + State.ID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_StateShiftsOnTerminal()
        {
            stream.Write("        private static ushort[][][] staticStateShiftsOnTerminal = { ");
            bool first = true;
            foreach (State State in graph.Sets)
            {
                if (!first) stream.Write(", ");
                stream.Write("stateShiftsOnTerminal_" + State.ID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_StateShiftsOnVariable()
        {
            stream.Write("        private static ushort[][][] staticStateShiftsOnVariable = { ");
            bool first = true;
            foreach (State State in graph.Sets)
            {
                if (!first) stream.Write(", ");
                stream.Write("stateShiftsOnVariable_" + State.ID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_StateReducsOnTerminal()
        {
            stream.Write("        private static ushort[][][] staticStateReducsOnTerminal = { ");
            bool first = true;
            foreach (State State in graph.Sets)
            {
                if (!first) stream.Write(", ");
                stream.Write("stateReducsOnTerminal_" + State.ID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
    }
}
