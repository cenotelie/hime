namespace Hime.Parsers.CF.LR
{
    class LR1ParserData : LRParserData
    {
        protected System.IO.StreamWriter p_Stream;

        public LR1ParserData(ParserGenerator generator, CFGrammar gram, Graph graph) : base(generator, gram, graph) { }


        public override bool Export(GrammarBuildOptions Options)
        {
            p_Stream = Options.ParserWriter;
            p_Stream.Write("    class " + p_Grammar.LocalName + "_Parser : ");
            if (p_Grammar is CFGrammarText)
                p_Stream.WriteLine("Hime.Redist.Parsers.LR1TextParser");
            else
                p_Stream.WriteLine("Hime.Redist.Parsers.LR1BinaryParser");
            p_Stream.WriteLine("    {");
            foreach (CFRule rule in p_Grammar.Rules)
                Export_Production(rule);
            Export_Rules();
            Export_RulesHeadID();
            Export_RulesName();
            Export_RulesParserLength();
            foreach (ItemSet set in p_Graph.Sets)
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
            p_Stream.WriteLine("    }");
            return true;
        }
        protected void Export_Setup()
        {
            p_Stream.WriteLine("        protected override void setup()");
            p_Stream.WriteLine("        {");
            p_Stream.WriteLine("            p_Rules = p_StaticRules;");
            p_Stream.WriteLine("            p_RulesHeadID = p_StaticRulesHeadID;");
            p_Stream.WriteLine("            p_RulesHeadName = p_StaticRulesHeadName;");
            p_Stream.WriteLine("            p_RulesParserLength = p_StaticRulesParserLength;");

            p_Stream.WriteLine("            p_StateExpectedIDs = p_StaticStateExpectedIDs;");
            p_Stream.WriteLine("            p_StateExpectedNames = p_StaticStateExpectedNames;");
            p_Stream.WriteLine("            p_StateItems = p_StaticStateItems;");
            p_Stream.WriteLine("            p_StateShiftsOnTerminal = p_StaticStateShiftsOnTerminal;");
            p_Stream.WriteLine("            p_StateShiftsOnVariable = p_StaticStateShiftsOnVariable;");
            p_Stream.WriteLine("            p_StateReducsOnTerminal = p_StaticStateReducsOnTerminal;");
            p_Stream.WriteLine("            p_ErrorSimulationLength = 3;");
            p_Stream.WriteLine("        }");
        }
        protected void Export_Constructor()
        {
            p_Stream.WriteLine("        private Actions p_Actions;");
            p_Stream.WriteLine("        public " + p_Grammar.LocalName + "_Parser(Actions actions, " + p_Grammar.LocalName + "_Lexer lexer) : base (lexer) { p_Actions = actions; }");
        }
        protected void Export_Actions()
        {
            System.Collections.Generic.List<string> Names = new System.Collections.Generic.List<string>();
            foreach (Action action in p_Grammar.Actions)
                if (!Names.Contains(action.LocalName))
                    Names.Add(action.LocalName);

            p_Stream.WriteLine("        public interface Actions");
            p_Stream.WriteLine("        {");
            foreach (string name in Names)
                p_Stream.WriteLine("            void " + name + "(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);");
            p_Stream.WriteLine("        }");
        }
        protected void Export_Production(CFRule Rule)
        {
            string ParserLength = Rule.Definition.GetChoiceAtIndex(0).Length.ToString();

            p_Stream.WriteLine("        private static void Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X") + " (Hime.Redist.Parsers.BaseLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)");
            p_Stream.WriteLine("        {");
            if (ParserLength != "0")
            {
                p_Stream.WriteLine("            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - " + ParserLength + ", " + ParserLength + ");");
                p_Stream.WriteLine("            nodes.RemoveRange(nodes.Count - " + ParserLength + ", " + ParserLength + ");");
            }
            p_Stream.Write("            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x" + Rule.Variable.SID.ToString("X") + ", \"" + Rule.Variable.LocalName + "\")");
            if (Rule.ReplaceOnProduction)
                p_Stream.WriteLine(", Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);");
            else
                p_Stream.WriteLine(");");

            int i = 0;
            foreach (RuleDefinitionPart Part in Rule.Definition.Parts)
            {
                if (Part.Symbol is Action)
                {
                    Action action = (Action)Part.Symbol;
                    p_Stream.WriteLine("            ((" + p_Grammar.LocalName + "_Parser)parser).p_Actions." + action.LocalName + "(SubRoot);");
                }
                else if (Part.Symbol is Virtual)
                    p_Stream.WriteLine("            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual(\"" + Part.Symbol.LocalName + "\"), Hime.Redist.Parsers.SyntaxTreeNodeAction." + Part.Action.ToString() + "));");
                else if (Part.Symbol is Terminal || Part.Symbol is Variable)
                {
                    p_Stream.Write("            SubRoot.AppendChild(Definition[" + i.ToString() + "]");
                    if (Part.Action != RuleDefinitionPartAction.Nothing)
                        p_Stream.WriteLine(", Hime.Redist.Parsers.SyntaxTreeNodeAction." + Part.Action.ToString() + ");");
                    else
                        p_Stream.WriteLine(");");
                    i++;
                }
            }
            p_Stream.WriteLine("            nodes.Add(SubRoot);");
            p_Stream.WriteLine("        }");
        }
        protected void Export_Rules()
        {
            p_Stream.Write("        private static Production[] p_StaticRules = { ");
            bool first = true;
            foreach (CFRule Rule in p_Grammar.Rules)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_RulesHeadID()
        {
            p_Stream.Write("        private static ushort[] p_StaticRulesHeadID = { ");
            bool first = true;
            foreach (CFRule Rule in p_Grammar.Rules)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("0x" + Rule.Variable.SID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_RulesName()
        {
            p_Stream.Write("        private static string[] p_StaticRulesHeadName = { ");
            bool first = true;
            foreach (CFRule Rule in p_Grammar.Rules)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("\"" + Rule.Variable.LocalName + "\"");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_RulesParserLength()
        {
            p_Stream.Write("        private static ushort[] p_StaticRulesParserLength = { ");
            bool first = true;
            foreach (CFRule Rule in p_Grammar.Rules)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("0x" + Rule.Definition.GetChoiceAtIndex(0).Length.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_State_Expected(ItemSet State)
        {
            TerminalSet Terminals = State.Reductions.ExpectedTerminals;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (Symbol is Terminal)
                    Terminals.Add((Terminal)Symbol);
            }

            p_Stream.Write("        private static ushort[] p_StateExpectedIDs_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Terminal Terminal in Terminals)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("0x" + Terminal.SID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");

            p_Stream.Write("        private static string[] p_StateExpectedNames_" + State.ID.ToString("X") + " = { ");
            first = true;
            foreach (Terminal Terminal in Terminals)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("\"" + Terminal.LocalName + "\"");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_State_Items(ItemSet State)
        {
            p_Stream.Write("        private static string[] p_StateItems_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Item item in State.Items)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("\"" + item.ToString() + "\"");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_State_ShiftOnTerminals(ItemSet State)
        {
            p_Stream.Write("        private static ushort[][] p_StateShiftsOnTerminal_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("new ushort[2] { 0x" + Symbol.SID.ToString("x") + ", 0x" + State.Children[Symbol].ID.ToString("X") + " }");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_State_ShiftOnVariables(ItemSet State)
        {
            p_Stream.Write("        private static ushort[][] p_StateShiftsOnVariable_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Variable))
                    continue;
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("new ushort[2] { 0x" + Symbol.SID.ToString("x") + ", 0x" + State.Children[Symbol].ID.ToString("X") + " }");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_State_ReducsOnTerminal(ItemSet State)
        {
            p_Stream.Write("        private static ushort[][] p_StateReducsOnTerminal_" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (ItemSetActionReduce Reduction in State.Reductions.Reductions)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("new ushort[2] { 0x" + Reduction.OnSymbol.SID.ToString("x") + ", 0x" + p_Grammar.Rules.IndexOf(Reduction.ToReduceRule).ToString("X") + " }");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_StateExpectedIDs()
        {
            p_Stream.Write("        private static ushort[][] p_StaticStateExpectedIDs = { ");
            bool first = true;
            foreach (ItemSet State in p_Graph.Sets)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("p_StateExpectedIDs_" + State.ID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_StateExpectedNames()
        {
            p_Stream.Write("        private static string[][] p_StaticStateExpectedNames = { ");
            bool first = true;
            foreach (ItemSet State in p_Graph.Sets)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("p_StateExpectedNames_" + State.ID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_StateItems()
        {
            p_Stream.Write("        private static string[][] p_StaticStateItems = { ");
            bool first = true;
            foreach (ItemSet State in p_Graph.Sets)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("p_StateItems_" + State.ID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_StateShiftsOnTerminal()
        {
            p_Stream.Write("        private static ushort[][][] p_StaticStateShiftsOnTerminal = { ");
            bool first = true;
            foreach (ItemSet State in p_Graph.Sets)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("p_StateShiftsOnTerminal_" + State.ID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_StateShiftsOnVariable()
        {
            p_Stream.Write("        private static ushort[][][] p_StaticStateShiftsOnVariable = { ");
            bool first = true;
            foreach (ItemSet State in p_Graph.Sets)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("p_StateShiftsOnVariable_" + State.ID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_StateReducsOnTerminal()
        {
            p_Stream.Write("        private static ushort[][][] p_StaticStateReducsOnTerminal = { ");
            bool first = true;
            foreach (ItemSet State in p_Graph.Sets)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("p_StateReducsOnTerminal_" + State.ID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
    }
}