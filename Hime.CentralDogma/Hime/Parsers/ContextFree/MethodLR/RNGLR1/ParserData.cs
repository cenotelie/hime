using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ParserDataRNGLR1 : ParserDataLR
    {
        protected System.IO.StreamWriter stream;
        protected List<CFVariable> nullableVars;
        protected List<CFRuleDefinition> nullableChoices;

        public ParserDataRNGLR1(ParserGenerator generator, CFGrammar gram, Graph graph)
            : base(generator, gram, graph)
        {
            nullableVars = new List<CFVariable>();
            nullableChoices = new List<CFRuleDefinition>();
        }

        protected void DetermineNullables()
        {
            nullableChoices.Add(new CFRuleDefinition());
            foreach (CFVariable var in grammar.Variables)
            {
                if (var.Firsts.Contains(TerminalEpsilon.Instance))
                    nullableVars.Add(var);
                foreach (CFRule rule in var.Rules)
                {
                    int length = rule.Definition.GetChoiceAtIndex(0).Length;
                    for (int i = 0; i != length; i++)
                    {
                        CFRuleDefinition choice = rule.Definition.GetChoiceAtIndex(i);
                        if (choice.Firsts.Contains(TerminalEpsilon.Instance))
                            nullableChoices.Add(choice);
                    }
                }
            }
        }

        public override bool Export(GrammarBuildOptions Options)
        {
            DetermineNullables();
            stream = Options.ParserWriter;
            stream.WriteLine("    class " + grammar.LocalName + "_Parser : Hime.Redist.Parsers.BaseRNGLR1Parser");
            stream.WriteLine("    {");

            Export_NullVars();
            Export_NullChoices();
            foreach (CFRule rule in grammar.Rules)
                Export_Production(rule);
            Export_Rules();
            Export_States();
            Export_NullBuilders();
            Export_Actions();
            Export_Setup();
            Export_StaticConstructor();
            Export_Constructor();
            stream.WriteLine("    }");
            return true;
        }
        protected void Export_StaticConstructor()
        {
            stream.WriteLine("        static " + grammar.LocalName + "_Parser()");
            stream.WriteLine("        {");
            stream.WriteLine("            BuildNullables();");
            stream.WriteLine("        }");
        }
        protected void Export_Setup()
        {
            stream.WriteLine("        protected override void setup()");
            stream.WriteLine("        {");
            stream.WriteLine("            nullVarsSPPF = staticNullVarsSPPF;");
            stream.WriteLine("            nullChoicesSPPF = staticNullChoicesSPPF;");
            stream.WriteLine("            rules = staticRules;");
            stream.WriteLine("            states = staticStates;");
            stream.WriteLine("            axiomID = 0x" + grammar.GetVariable(grammar.GetOption("Axiom")).SID.ToString("X") + ";");
            stream.WriteLine("            axiomNullSPPF = 0x" + grammar.GetVariable(grammar.GetOption("Axiom")).SID.ToString("X") + ";");
            stream.WriteLine("            axiomPrimeID = 0x" + grammar.GetVariable("_Axiom_").SID.ToString("X") + ";");
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

            stream.WriteLine("        private static void Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X") + " (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)");
            stream.WriteLine("        {");
            if (Rule.ReplaceOnProduction)
                stream.WriteLine("            root.Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace;");
            stream.WriteLine("            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);");
            int i = 0;
            foreach (RuleDefinitionPart Part in Rule.Definition.Parts)
            {
                if (Part.Symbol is Action)
                {
                    Action action = (Action)Part.Symbol;
                    stream.WriteLine("            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolAction(\"" + action.LocalName + "\", ((" + grammar.LocalName + "_Parser)parser).actions." + action.LocalName + "), 0));");
                }
                else if (Part.Symbol is Virtual)
                    stream.WriteLine("            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolVirtual(\"" + ((Virtual)Part.Symbol).LocalName + "\"), 0, Hime.Redist.Parsers.SyntaxTreeNodeAction." + Part.Action.ToString() + "));");
                else if (Part.Symbol is Terminal || Part.Symbol is Variable)
                {
                    if (Part.Action != RuleDefinitionPartAction.Nothing)
                        stream.WriteLine("            nodes[" + i.ToString() + "].Action = Hime.Redist.Parsers.SyntaxTreeNodeAction." + Part.Action.ToString() + ";");
                    stream.WriteLine("            family.AddChild(nodes[" + i.ToString() + "]);");
                    i++;
                }
            }
            stream.WriteLine("            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);");
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
                stream.WriteLine("new Rule(" + production + ", " + head + ")");
                first = false;
            }
            stream.WriteLine("        };");
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

            // Write shifts on terminal
            stream.Write("               new ushort[" + ShitTerminalCount + "] {");
            first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("0x" + Symbol.SID.ToString("x"));
                first = false;
            }
            stream.WriteLine("},");
            stream.Write("               new ushort[" + ShitTerminalCount + "] {");
            first = true;
            foreach (Symbol Symbol in State.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("0x" + State.Children[Symbol].ID.ToString("X"));
                first = false;
            }
            stream.WriteLine("},");

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
            stream.WriteLine("},");
            // Write reductions
            stream.Write("               new Reduction[" + State.Reductions.Count + "] {");
            first = true;
            foreach (StateActionRNReduce Reduction in State.Reductions)
            {
                if (!first) stream.Write(", ");
                if (Reduction.ReduceLength == 0)
                {
                    int index = 0;
                    index = nullableVars.IndexOf(Reduction.ToReduceRule.Variable);
                    stream.Write("new Reduction(0x" + Reduction.OnSymbol.SID.ToString("x") + ", staticRules[0x" + grammar.Rules.IndexOf(Reduction.ToReduceRule).ToString("X") + "], 0x" + Reduction.ReduceLength.ToString("X") + ", staticNullVarsSPPF[0x" + index.ToString("X") + "])");
                }
                else
                {
                    int index = 0;
                    if (Reduction.ToReduceRule.Definition.GetChoiceAtIndex(0).Length != Reduction.ReduceLength)
                    {
                        CFRuleDefinition def = Reduction.ToReduceRule.Definition.GetChoiceAtIndex(Reduction.ReduceLength);
                        index = nullableChoices.IndexOf(def);
                    }
                    stream.Write("new Reduction(0x" + Reduction.OnSymbol.SID.ToString("x") + ", staticRules[0x" + grammar.Rules.IndexOf(Reduction.ToReduceRule).ToString("X") + "], 0x" + Reduction.ReduceLength.ToString("X") + ", staticNullChoicesSPPF[0x" + index.ToString("X") + "])");
                }
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
        
        protected void Export_NullVars()
        {
            stream.Write("        private static Hime.Redist.Parsers.SPPFNode[] staticNullVarsSPPF = { ");
            bool first = true;
            foreach (CFVariable var in nullableVars)
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
                if (!first) stream.Write(", ");
                stream.Write("new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolVariable(0x" + var.SID.ToString("X") + ", \"" + var.LocalName + "\"), 0" + action + ")");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_NullChoices()
        {
            stream.Write("        private static Hime.Redist.Parsers.SPPFNode[] staticNullChoicesSPPF = { ");
            bool first = true;
            foreach (CFRuleDefinition definition in nullableChoices)
            {
                if (!first) stream.Write(", ");
                stream.Write("new Hime.Redist.Parsers.SPPFNode(null, 0, Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace)");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_NullBuilders()
        {
            stream.WriteLine("        private static void BuildNullables() { ");
            stream.WriteLine("            List<Hime.Redist.Parsers.SPPFNode> temp = new List<Hime.Redist.Parsers.SPPFNode>();");
            for (int i=0; i!=nullableChoices.Count; i++)
            {
                CFRuleDefinition definition = nullableChoices[i];
                foreach (RuleDefinitionPart part in definition.Parts)
                    stream.WriteLine("            temp.Add(staticNullVarsSPPF[" + nullableVars.IndexOf((CFVariable)part.Symbol) + "]);");
                stream.WriteLine("            staticNullChoicesSPPF[" + i + "].AddFamily(temp);");
                stream.WriteLine("            temp.Clear();");
            }
            for (int i = 0; i != nullableVars.Count; i++)
            {
                CFVariable var = nullableVars[i];
                foreach (CFRule rule in var.Rules)
                {
                    CFRuleDefinition definition = rule.Definition.GetChoiceAtIndex(0);
                    if (definition.Firsts.Contains(TerminalEpsilon.Instance))
                    {
                        foreach (RuleDefinitionPart part in definition.Parts)
                            stream.WriteLine("            temp.Add(staticNullVarsSPPF[" + nullableVars.IndexOf((CFVariable)part.Symbol) + "]);");
                        stream.WriteLine("            staticNullVarsSPPF[" + i + "].AddFamily(temp);");
                        stream.WriteLine("            temp.Clear();");
                    }
                }
            }
            stream.WriteLine("        }");
        }
    }
}
