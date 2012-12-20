/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class ParserDataRNGLR1 : ParserDataLR
    {
		protected override string GetBaseClassName { get { return "BaseRNGLR1Parser"; } }

        protected List<Variable> nullableVars;
        protected List<CFRuleBody> nullableChoices;

        public ParserDataRNGLR1(Reporting.Reporter reporter, CFGrammar gram, Graph graph)
            : base(reporter, gram, graph)
        {
            nullableVars = new List<Variable>();
            nullableChoices = new List<CFRuleBody>();
        }

        public override void ExportData(BinaryWriter stream)
        {
            throw new NotImplementedException();
        }

        private void DetermineNullables()
        {
            nullableChoices.Add(new CFRuleBody());
            foreach (CFVariable var in variables)
            {
                if (var.Firsts.Contains(Epsilon.Instance))
                    nullableVars.Add(var);
                foreach (CFRule rule in var.Rules)
                {
                    int length = rule.CFBody.GetChoiceAt(0).Length;
                    for (int i = 0; i != length; i++)
                    {
                        CFRuleBody choice = rule.CFBody.GetChoiceAt(i);
                        if (choice.Firsts.Contains(Epsilon.Instance))
                            nullableChoices.Add(choice);
                    }
                }
            }
        }

        protected override void ExportAutomaton(StreamWriter stream, string name, string className)
        {
        }

        /*protected override void ExportAdditionalStaticElements(StreamWriter stream, string className)
        {
            DetermineNullables();
            Export_NullVars(stream);
            Export_NullChoices(stream);
            Export_NullBuilders(stream);
            Export_StaticConstructor(stream, className);
        }
		
        protected void Export_StaticConstructor(StreamWriter stream, string className)
        {
            stream.WriteLine("        static " + className + "()");
            stream.WriteLine("        {");
            stream.WriteLine("            BuildNullables();");
            stream.WriteLine("        }");
        }
		
        protected override void ExportSetup(StreamWriter stream)
        {
            string axiomID = GetVariable(GetOption("Axiom")).SID.ToString("X");
            string _axiom_ID = GetVariable("_Axiom_").SID.ToString("X");
            int index = nullableVars.IndexOf(GetVariable(GetOption("Axiom")));
            if (index < 0) index = 0;

            stream.WriteLine("        protected override void setup()");
            stream.WriteLine("        {");
            stream.WriteLine("            nullVarsSPPF = staticNullVarsSPPF;");
            stream.WriteLine("            nullChoicesSPPF = staticNullChoicesSPPF;");
            stream.WriteLine("            rules = staticRules;");
            stream.WriteLine("            states = staticStates;");
            stream.WriteLine("            axiomID = 0x" + axiomID + ";");
            stream.WriteLine("            axiomNullSPPF = 0x" + index.ToString("X") + ";");
            stream.WriteLine("            axiomPrimeID = 0x" + _axiom_ID + ";");
            stream.WriteLine("        }");
        }

        protected override void ExportActions(StreamWriter stream)
        {
            List<string> Names = new List<string>();
            foreach (Action action in this.GrammarActions)
                if (!Names.Contains(action.Name))
                    Names.Add(action.Name);

            if (Names.Count != 0)
            {
                stream.WriteLine("        public interface Actions");
                stream.WriteLine("        {");
                foreach (string name in Names)
                    stream.WriteLine("            void " + name + "(SyntaxTreeNode SubRoot);");
                stream.WriteLine("        }");
            }
        }
        override protected void ExportProduction(StreamWriter stream, CFRule Rule, string className)
        {
            stream.WriteLine("        private static void Production_" + Rule.Head.SID.ToString("X") + "_" + Rule.ID.ToString("X") + " (BaseRNGLR1Parser parser, SPPFNode root, List<SPPFNode> nodes)");
            stream.WriteLine("        {");
            if (Rule.ReplaceOnProduction)
                stream.WriteLine("            root.Action = SyntaxTreeNodeAction.Replace;");
            stream.WriteLine("            SPPFNodeFamily family = new SPPFNodeFamily(root);");
            int i = 0;
            foreach (RuleBodyElement Part in Rule.CFBody.Parts)
            {
                if (Part.Symbol is Action)
                {
                    Action action = (Action)Part.Symbol;
                    stream.WriteLine("            family.AddChild(new SPPFNode(new SymbolAction(\"" + action.Name + "\", ((" + className + ")parser).actions." + action.Name + "), 0));");
                }
                else if (Part.Symbol is Virtual)
                    stream.WriteLine("            family.AddChild(new SPPFNode(new Virtual(\"" + ((Virtual)Part.Symbol).Name + "\"), 0, SyntaxTreeNodeAction." + Part.Action.ToString() + "));");
                else if (Part.Symbol is Terminal || Part.Symbol is Variable)
                {
                    if (Part.Action != RuleBodyElementAction.Nothing)
                        stream.WriteLine("            nodes[" + i.ToString() + "].Action = SyntaxTreeNodeAction." + Part.Action.ToString() + ";");
                    stream.WriteLine("            family.AddChild(nodes[" + i.ToString() + "]);");
                    i++;
                }
            }
            stream.WriteLine("            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);");
            stream.WriteLine("        }");
        }
		
        override protected void ExportRules(StreamWriter stream)
        {
            stream.WriteLine("        private static Rule[] staticRules = {");
            bool first = true;
            foreach (CFRule Rule in this.GrammarRules)
            {
                stream.Write("           ");
                if (!first) stream.Write(", ");
                string production = "Production_" + Rule.Head.SID.ToString("X") + "_" + Rule.ID.ToString("X");
                string head = "variables[" + this.variables.IndexOf(Rule.Head) + "]";
                stream.WriteLine("new Rule(" + production + ", " + head + ")");
                first = false;
            }
            stream.WriteLine("        };");
        }
        protected void Export_State(StreamWriter stream, State state)
        {
            TerminalSet expected = state.Reductions.ExpectedTerminals;
            foreach (GrammarSymbol Symbol in state.Children.Keys)
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
            stream.Write("               new Terminal[" + expected.Count + "] {");
            first = true;
            foreach (Terminal terminal in expected)
            {
                int index = terminals.IndexOf(terminal);
                if (index == -1)
                    reporter.Error("Grammar", "In state " + state.ID.ToString("X") + " expected terminal " + terminal.ToString() + " cannot be produced by the lexer. Check the regular expressions.");
                if (!first) stream.Write(", ");
                stream.Write(terminalsAccessor + "[" + index + "]");
                first = false;
            }
            stream.WriteLine("},");

            int ShitTerminalCount = 0;
            foreach (GrammarSymbol Symbol in state.Children.Keys)
            {
                if (!(Symbol is Terminal))
                    continue;
                ShitTerminalCount++;
            }

            // Write shifts on terminal
            stream.Write("               new ushort[" + ShitTerminalCount + "] {");
            first = true;
            foreach (GrammarSymbol Symbol in state.Children.Keys)
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
            foreach (GrammarSymbol Symbol in state.Children.Keys)
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
            foreach (GrammarSymbol Symbol in state.Children.Keys)
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
            foreach (GrammarSymbol Symbol in state.Children.Keys)
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
				string tableName;
                int index = 0;
				int reductionLength = Reduction.ReduceLength;
				CFRule reductionRule = Reduction.ToReduceRule;
                if (reductionLength == 0)
                {
                    index = nullableVars.IndexOf(reductionRule.Head);
					tableName = "staticNullVarsSPPF";
                }
                else
                {
                    if (reductionRule.CFBody.GetChoiceAt(0).Length != reductionLength)
                    {
                        CFRuleBody def = reductionRule.CFBody.GetChoiceAt(reductionLength);
                        index = nullableChoices.IndexOf(def);
                    }
					tableName = "staticNullChoicesSPPF";
                }
				// TODO: think about it but many violations of the Demeter rule!!!
                stream.Write("new Reduction(0x" + Reduction.OnSymbol.SID.ToString("x") + ", staticRules[" + this.IndexOfRule(reductionRule) + "], 0x" + reductionLength.ToString("X") + ", " + tableName + "[0x" + index.ToString("X") + "])");
                first = false;
            }
            stream.WriteLine("})");
        }
		// TODO: should factor this method with other
        protected override void ExportStates(StreamWriter stream)
        {
            stream.WriteLine("        private static State[] staticStates = {");
            bool first = true;
            foreach (State State in graph.States)
            {
                stream.Write("            ");
                if (!first) stream.Write(", ");
                Export_State(stream, State);
                first = false;
            }
            stream.WriteLine("        };");
        }

        protected void Export_NullVars(StreamWriter stream)
        {
            stream.Write("        private static SPPFNode[] staticNullVarsSPPF = { ");
            bool first = true;
            foreach (Variable var in nullableVars)
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
        protected void Export_NullChoices(StreamWriter stream)
        {
            stream.Write("        private static SPPFNode[] staticNullChoicesSPPF = { ");
            bool first = true;
            foreach (CFRuleBody definition in nullableChoices)
            {
                if (!first) stream.Write(", ");
                stream.Write("new SPPFNode(null, 0, SyntaxTreeNodeAction.Replace)");
                first = false;
            }
            stream.WriteLine(" };");
        }
        protected void Export_NullBuilders(StreamWriter stream)
        {
            stream.WriteLine("        private static void BuildNullables() { ");
            stream.WriteLine("            List<SPPFNode> temp = new List<SPPFNode>();");
            for (int i=0; i!=nullableChoices.Count; i++)
            {
                CFRuleBody definition = nullableChoices[i];
                foreach (RuleBodyElement part in definition.Parts)
                    stream.WriteLine("            temp.Add(staticNullVarsSPPF[" + nullableVars.IndexOf((Variable)part.Symbol) + "]);");
                stream.WriteLine("            staticNullChoicesSPPF[" + i + "].AddFamily(temp);");
                stream.WriteLine("            temp.Clear();");
            }
            for (int i = 0; i != nullableVars.Count; i++)
            {
                Variable var = nullableVars[i];
                foreach (CFRule rule in var.Rules)
                {
                    CFRuleBody definition = rule.CFBody.GetChoiceAt(0);
                    if (definition.Firsts.Contains(TerminalEpsilon.Instance))
                    {
                        foreach (RuleBodyElement part in definition.Parts)
                            stream.WriteLine("            temp.Add(staticNullVarsSPPF[" + nullableVars.IndexOf((Variable)part.Symbol) + "]);");
                        stream.WriteLine("            staticNullVarsSPPF[" + i + "].AddFamily(temp);");
                        stream.WriteLine("            temp.Clear();");
                    }
                }
            }
            stream.WriteLine("        }");
        }*/
    }
}
