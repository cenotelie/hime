using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ParserDataRNGLR1 : ParserData
    {
        protected Kernel.Reporting.Reporter reporter;
        protected string terminalsAccessor;
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
            foreach (CFVariable var in this.GrammarVariables)
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

        public override bool Export(IList<Terminal> expected, CompilationTask options)
        {
            reporter = options.Reporter;
            terminals = new List<Terminal>(expected);
            debug = options.ExportDebug;
            terminalsAccessor = this.GrammarName + "_Lexer.terminals";
            DetermineNullables();
            stream = options.ParserWriter;
            stream.WriteLine("    " + options.GeneratedCodeModifier.ToString().ToLower() + " class " + this.GrammarName + "_Parser : BaseRNGLR1Parser");
            stream.WriteLine("    {");

            Export_Variables(stream);
            Export_NullVars();
            Export_NullChoices();
            foreach (CFRule rule in this.GrammarRules)
                Export_Production(rule);
            Export_Rules();
            Export_States();
            Export_NullBuilders();
            Export_Actions();
            Export_Setup();
            Export_StaticConstructor();
            return base.Export(expected, options);
        }
        protected void Export_StaticConstructor()
        {
            stream.WriteLine("        static " + this.GrammarName + "_Parser()");
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
            stream.WriteLine("            axiomID = " + this.GetOption("Axiom") + ";");
            stream.WriteLine("            axiomNullSPPF = " + this.GetOption("Axiom") + ";");
            stream.WriteLine("            axiomPrimeID = " + this.GetVariable("_Axiom_") + ";");
            stream.WriteLine("        }");
        }
        
        protected void Export_Actions()
        {
            List<string> Names = new List<string>();
            foreach (Action action in this.GrammarActions)
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
        protected void Export_Production(CFRule Rule)
        {
            string ParserLength = Rule.Definition.GetChoiceAtIndex(0).Length.ToString();

            stream.WriteLine("        private static void Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X") + " (BaseRNGLR1Parser parser, SPPFNode root, List<SPPFNode> nodes)");
            stream.WriteLine("        {");
            if (Rule.ReplaceOnProduction)
                stream.WriteLine("            root.Action = SyntaxTreeNodeAction.Replace;");
            stream.WriteLine("            SPPFNodeFamily family = new SPPFNodeFamily(root);");
            int i = 0;
            foreach (RuleDefinitionPart Part in Rule.Definition.Parts)
            {
                if (Part.Symbol is Action)
                {
                    Action action = (Action)Part.Symbol;
                    stream.WriteLine("            family.AddChild(new SPPFNode(new SymbolAction(\"" + action.LocalName + "\", ((" + this.GrammarName + "_Parser)parser).actions." + action.LocalName + "), 0));");
                }
                else if (Part.Symbol is Virtual)
                    stream.WriteLine("            family.AddChild(new SPPFNode(new SymbolVirtual(\"" + ((Virtual)Part.Symbol).LocalName + "\"), 0, SyntaxTreeNodeAction." + Part.Action.ToString() + "));");
                else if (Part.Symbol is Terminal || Part.Symbol is Variable)
                {
                    if (Part.Action != RuleDefinitionPartAction.Nothing)
                        stream.WriteLine("            nodes[" + i.ToString() + "].Action = SyntaxTreeNodeAction." + Part.Action.ToString() + ";");
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
            foreach (CFRule Rule in this.GrammarRules)
            {
                stream.Write("           ");
                if (!first) stream.Write(", ");
                string production = "Production_" + Rule.Variable.SID.ToString("X") + "_" + Rule.ID.ToString("X");
                string head = "variables[" + this.variables.IndexOf(Rule.Variable) + "]";
                stream.WriteLine("new Rule(" + production + ", " + head + ")");
                first = false;
            }
            stream.WriteLine("        };");
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
            stream.WriteLine("new State(");
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

            // Write shifts on terminal
            stream.Write("               new ushort[" + ShitTerminalCount + "] {");
            first = true;
            foreach (Symbol Symbol in state.Children.Keys)
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
            foreach (Symbol Symbol in state.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                if (!first) stream.Write(", ");
                stream.Write("0x" + state.Children[Symbol].ID.ToString("X"));
                first = false;
            }
            stream.WriteLine("},");

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
            stream.WriteLine("},");
            // Write reductions
            stream.Write("               new Reduction[" + state.Reductions.Count + "] {");
            first = true;
            foreach (StateActionRNReduce Reduction in state.Reductions)
            {
                if (!first) stream.Write(", ");
                if (Reduction.ReduceLength == 0)
                {
                    int index = 0;
                    index = nullableVars.IndexOf(Reduction.ToReduceRule.Variable);
                    stream.Write("new Reduction(0x" + Reduction.OnSymbol.SID.ToString("x") + ", staticRules[" + this.IndexOfRule(Reduction.ToReduceRule) + "], 0x" + Reduction.ReduceLength.ToString("X") + ", staticNullVarsSPPF[0x" + index.ToString("X") + "])");
                }
                else
                {
                    int index = 0;
                    if (Reduction.ToReduceRule.Definition.GetChoiceAtIndex(0).Length != Reduction.ReduceLength)
                    {
                        CFRuleDefinition def = Reduction.ToReduceRule.Definition.GetChoiceAtIndex(Reduction.ReduceLength);
                        index = nullableChoices.IndexOf(def);
                    }
                    stream.Write("new Reduction(0x" + Reduction.OnSymbol.SID.ToString("x") + ", staticRules[" + this.IndexOfRule(Reduction.ToReduceRule) + "], 0x" + Reduction.ReduceLength.ToString("X") + ", staticNullChoicesSPPF[0x" + index.ToString("X") + "])");
                }
                first = false;
            }
            stream.WriteLine("})");
        }
        protected void Export_States()
        {
            stream.WriteLine("        private static State[] staticStates = {");
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
        
        protected void Export_NullVars()
        {
            stream.Write("        private static SPPFNode[] staticNullVarsSPPF = { ");
            bool first = true;
            foreach (CFVariable var in nullableVars)
            {
                string action = ", SyntaxTreeNodeAction.Replace";
                foreach (CFRule rule in var.Rules)
                {
                    if (!rule.ReplaceOnProduction)
                    {
                        action = "";
                        break;
                    }
                }
                if (!first) stream.Write(", ");
                stream.Write("new SPPFNode(variables[" + variables.IndexOf(var) + "], 0" + action + ")");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_NullChoices()
        {
            stream.Write("        private static SPPFNode[] staticNullChoicesSPPF = { ");
            bool first = true;
            foreach (CFRuleDefinition definition in nullableChoices)
            {
                if (!first) stream.Write(", ");
                stream.Write("new SPPFNode(null, 0, SyntaxTreeNodeAction.Replace)");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_NullBuilders()
        {
            stream.WriteLine("        private static void BuildNullables() { ");
            stream.WriteLine("            List<SPPFNode> temp = new List<SPPFNode>();");
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
