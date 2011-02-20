namespace Hime.Parsers.CF.LR
{
    public class RNGLR1ParserData : LRParserData
    {
        protected System.IO.StreamWriter p_Stream;
        protected System.Collections.Generic.List<CFVariable> p_NullableVars;
        protected System.Collections.Generic.List<CFRuleDefinition> p_NullableChoices;

        public RNGLR1ParserData(ParserGenerator generator, CFGrammar gram, Graph graph)
            : base(generator, gram, graph)
        {
            p_NullableVars = new System.Collections.Generic.List<CFVariable>();
            p_NullableChoices = new System.Collections.Generic.List<CFRuleDefinition>();
        }

        protected void DetermineNullables()
        {
            p_NullableChoices.Add(new CFRuleDefinition());
            foreach (CFVariable var in p_Grammar.Variables)
            {
                if (var.Firsts.Contains(TerminalEpsilon.Instance))
                    p_NullableVars.Add(var);
                foreach (CFRule rule in var.Rules)
                {
                    int length = rule.Definition.GetChoiceAtIndex(0).Length;
                    for (int i = 0; i != length; i++)
                    {
                        CFRuleDefinition choice = rule.Definition.GetChoiceAtIndex(i);
                        if (choice.Firsts.Contains(TerminalEpsilon.Instance))
                            p_NullableChoices.Add(choice);
                    }
                }
            }
        }

        public override bool Export(GrammarBuildOptions Options)
        {
            DetermineNullables();
            p_Stream = Options.ParserWriter;
            p_Stream.WriteLine("    public class " + p_Grammar.LocalName + "_Parser : Hime.Redist.Parsers.BaseRNGLR1Parser");
            p_Stream.WriteLine("    {");

            Export_NullVars();
            Export_NullChoices();
            foreach (CFRule rule in p_Grammar.Rules)
                Export_Production(rule);
            Export_Rules();
            Export_States();
            Export_NullBuilders();
            Export_Actions();
            Export_Setup();
            Export_StaticConstructor();
            Export_Constructor();
            p_Stream.WriteLine("    }");
            return true;
        }
        protected void Export_StaticConstructor()
        {
            p_Stream.WriteLine("        static " + p_Grammar.LocalName + "_Parser()");
            p_Stream.WriteLine("        {");
            p_Stream.WriteLine("            BuildNullables();");
            p_Stream.WriteLine("        }");
        }
        protected void Export_Setup()
        {
            p_Stream.WriteLine("        protected override void setup()");
            p_Stream.WriteLine("        {");
            p_Stream.WriteLine("            p_NullVarsSPPF = p_StaticNullVarsSPPF;");
            p_Stream.WriteLine("            p_NullChoicesSPPF = p_StaticNullChoicesSPPF;");
            p_Stream.WriteLine("            p_Rules = p_StaticRules;");
            p_Stream.WriteLine("            p_States = p_StaticStates;");
            p_Stream.WriteLine("            p_AxiomID = 0x" + p_Grammar.GetVariable(p_Grammar.GetOption("Axiom")).SID.ToString("X") + ";");
            p_Stream.WriteLine("            p_AxiomNullSPPF = 0x" + p_Grammar.GetVariable(p_Grammar.GetOption("Axiom")).SID.ToString("X") + ";");
            p_Stream.WriteLine("            p_AxiomPrimeID = 0x" + p_Grammar.GetVariable("_Axiom_").SID.ToString("X") + ";");
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
                p_Stream.WriteLine("        void " + name + "(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);");
            p_Stream.WriteLine("        }");
        }
        protected void Export_Production(CFRule Rule)
        {
            string ParserLength = Rule.Definition.GetChoiceAtIndex(0).Length.ToString();

            p_Stream.WriteLine("        private static void Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X") + " (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)");
            p_Stream.WriteLine("        {");
            if (Rule.ReplaceOnProduction)
                p_Stream.WriteLine("            root.Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace;");
            p_Stream.WriteLine("            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);");
            int i = 0;
            foreach (RuleDefinitionPart Part in Rule.Definition.Parts)
            {
                if (Part.Symbol is Action)
                {
                    Action action = (Action)Part.Symbol;
                    p_Stream.WriteLine("            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolAction(\"" + action.LocalName + "\", ((" + p_Grammar.LocalName + "_Parser)parser).p_Actions." + action.LocalName + "), 0));");
                }
                else if (Part.Symbol is Virtual)
                    p_Stream.WriteLine("            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolVirtual(\"" + ((Virtual)Part.Symbol).LocalName + "\"), 0));");
                else if (Part.Symbol is Terminal || Part.Symbol is Variable)
                {
                    if (Part.Action != RuleDefinitionPartAction.Nothing)
                        p_Stream.WriteLine("            nodes[" + i.ToString() + "].Action = Hime.Redist.Parsers.SyntaxTreeNodeAction." + Part.Action.ToString() + ";");
                    p_Stream.WriteLine("            family.AddChild(nodes[" + i.ToString() + "]);");
                    i++;
                }
            }
            p_Stream.WriteLine("            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);");
            p_Stream.WriteLine("        }");
        }
        protected void Export_Rules()
        {
            p_Stream.WriteLine("        private static Rule[] p_StaticRules = {");
            bool first = true;
            foreach (CFRule Rule in p_Grammar.Rules)
            {
                p_Stream.Write("           ");
                if (!first) p_Stream.Write(", ");
                string production = "Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X");
                string head = "new Hime.Redist.Parsers.SymbolVariable(0x" + Rule.Variable.SID.ToString("X") + ", \"" + Rule.Variable.LocalName + "\")";
                p_Stream.WriteLine("new Rule(" + production + ", " + head + ")");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_State(ItemSet State)
        {
            TerminalSet Terminals = State.Reductions.ExpectedTerminals;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (Symbol is Terminal)
                    Terminals.Add((Terminal)Symbol);
            }
            p_Stream.WriteLine("new State(");
            // Write items
            p_Stream.Write("               new string[" + State.Items.Count + "] {");
            bool first = true;
            foreach (Item item in State.Items)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("\"" + item.ToString(true) + "\"");
                first = false;
            }
            p_Stream.WriteLine("},");
            // Write terminals
            p_Stream.Write("               new Terminal[" + Terminals.Count + "] {");
            first = true;
            foreach (Terminal terminal in Terminals)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("new Terminal(\"" + terminal.LocalName + "\", 0x" + terminal.SID.ToString("X") + ")");
                first = false;
            }
            p_Stream.WriteLine("},");
            // Write shifts on terminal
            p_Stream.Write("               new System.Collections.Generic.Dictionary<ushort, ushort>() {");
            first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("{ 0x" + Symbol.SID.ToString("x") + ", 0x" + State.Children[Symbol].ID.ToString("X") + " }");
                first = false;
            }
            p_Stream.WriteLine("},");
            // Write shifts on variable
            p_Stream.Write("               new System.Collections.Generic.Dictionary<ushort, ushort>() {");
            first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Variable))
                    continue;
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("{ 0x" + Symbol.SID.ToString("x") + ", 0x" + State.Children[Symbol].ID.ToString("X") + " }");
                first = false;
            }
            p_Stream.WriteLine("},");
            // Write reductions
            p_Stream.Write("               new System.Collections.Generic.List<Reduction>() {");
            first = true;
            foreach (ItemSetActionRNReduce Reduction in State.Reductions.Reductions)
            {
                if (!first) p_Stream.Write(", ");
                int index = 0;
                if (Reduction.ToReduceRule.Definition.GetChoiceAtIndex(0).Length != Reduction.ReduceLength)
                {
                    CFRuleDefinition def = Reduction.ToReduceRule.Definition.GetChoiceAtIndex(Reduction.ReduceLength);
                    index = p_NullableChoices.IndexOf(def);
                }
                p_Stream.Write("new Reduction(0x" + Reduction.OnSymbol.SID.ToString("x") + ", p_StaticRules[0x" + p_Grammar.Rules.IndexOf(Reduction.ToReduceRule).ToString("X") + "], 0x" + Reduction.ReduceLength.ToString("X") + ", p_StaticNullChoicesSPPF[0x" + index.ToString("X") + "])");
                first = false;
            }
            p_Stream.WriteLine("})");
        }
        protected void Export_States()
        {
            p_Stream.WriteLine("        private static State[] p_StaticStates = {");
            bool first = true;
            foreach (ItemSet State in p_Graph.Sets)
            {
                p_Stream.Write("            ");
                if (!first) p_Stream.Write(", ");
                Export_State(State);
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        
        protected void Export_NullVars()
        {
            p_Stream.Write("        private static Hime.Redist.Parsers.SPPFNode[] p_StaticNullVarsSPPF = { ");
            bool first = true;
            foreach (CFVariable var in p_NullableVars)
            {
                string action = ", Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace";
                foreach (CFRule rule in var.Rules)
                {
                    if (!rule.ReplaceOnProduction)
                    {
                        action = "";
                        break;
                    }
                }
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolVariable(0x" + var.SID.ToString("X") + ", \"" + var.LocalName + "\"), 0" + action + ")");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_NullChoices()
        {
            p_Stream.Write("        private static Hime.Redist.Parsers.SPPFNode[] p_StaticNullChoicesSPPF = { ");
            bool first = true;
            foreach (CFRuleDefinition definition in p_NullableChoices)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("new Hime.Redist.Parsers.SPPFNode(null, 0, Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace)");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        protected void Export_NullBuilders()
        {
            p_Stream.WriteLine("        private static void BuildNullables() { ");
            p_Stream.WriteLine("            System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> temp = new System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode>();");
            for (int i=0; i!=p_NullableChoices.Count; i++)
            {
                CFRuleDefinition definition = p_NullableChoices[i];
                foreach (RuleDefinitionPart part in definition.Parts)
                    p_Stream.WriteLine("            temp.Add(p_StaticNullVarsSPPF[" + p_NullableVars.IndexOf((CFVariable)part.Symbol) + "]);");
                p_Stream.WriteLine("            p_StaticNullChoicesSPPF[" + i + "].AddFamily(temp);");
                p_Stream.WriteLine("            temp.Clear();");
            }
            for (int i = 0; i != p_NullableVars.Count; i++)
            {
                CFVariable var = p_NullableVars[i];
                foreach (CFRule rule in var.Rules)
                {
                    CFRuleDefinition definition = rule.Definition.GetChoiceAtIndex(0);
                    if (definition.Firsts.Contains(TerminalEpsilon.Instance))
                    {
                        foreach (RuleDefinitionPart part in definition.Parts)
                            p_Stream.WriteLine("            temp.Add(p_StaticNullVarsSPPF[" + p_NullableVars.IndexOf((CFVariable)part.Symbol) + "]);");
                        p_Stream.WriteLine("            p_StaticNullVarsSPPF[" + i + "].AddFamily(temp);");
                        p_Stream.WriteLine("            temp.Clear();");
                    }
                }
            }
            p_Stream.WriteLine("        }");
        }
    }
}