/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
    class ParserDataLR1 : ParserDataLR
    {
        public ParserDataLR1(Reporter reporter, CFGrammar gram, Graph graph) : base(reporter, gram, graph) { }
		
		// TODO: this method should be factored with other Export in ParserDatas...
        public override void Export(StreamWriter stream, string className, AccessModifier modifier, string lexerClassName, IList<Terminal> expected, bool exportDebug)
        {
			base.Export(stream, className, modifier, lexerClassName, expected, exportDebug);
			
            stream.Write("    " + modifier.ToString().ToLower() + " class " + className + " : ");
            // TODO: not nice!!! because based on type, should rather call a method on grammar
            if (grammar is CFGrammarText)
                stream.WriteLine("LR1TextParser");
            else
                stream.WriteLine("LR1BinaryParser");
            stream.WriteLine("    {");

			ExportVariables(stream);
            foreach (CFRule rule in this.GrammarRules) ExportProduction(stream, rule, className);
            Export_Rules(stream);
            Export_States(stream);
            Export_Actions(stream);
            Export_Setup(stream);
            ExportConstructor(stream, className, lexerClassName);
            stream.WriteLine("    }");
        }
        protected void Export_Setup(StreamWriter stream)
        {
            stream.WriteLine("        protected override void setup()");
            stream.WriteLine("        {");
            stream.WriteLine("            rules = staticRules;");
            stream.WriteLine("            states = staticStates;");
            stream.WriteLine("            errorSimulationLength = 3;");
            stream.WriteLine("        }");
        }

        protected void Export_Actions(StreamWriter stream)
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

		protected void Export_Rules(StreamWriter stream)
        {
            stream.WriteLine("        private static LRRule[] staticRules = {");
            bool first = true;
            foreach (CFRule Rule in this.GrammarRules)
            {
                stream.Write("           ");
                if (!first) stream.Write(", ");
                string production = "Production_" + Rule.Head.SID.ToString("X") + "_" + Rule.ID.ToString("X");
                string head = "variables[" + this.variables.IndexOf(Rule.Head) + "]";
                stream.WriteLine("new LRRule(" + production + ", " + head + ", " + Rule.CFBody.GetChoiceAt(0).Length + ")");
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
            stream.WriteLine("new LR1State(");
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
            stream.Write("               new LRReduction[" + state.Reductions.Count + "] {");
            first = true;
            foreach (StateActionReduce Reduction in state.Reductions)
            {
                if (!first) stream.Write(", ");
                stream.Write("new LRReduction(0x" + Reduction.OnSymbol.SID.ToString("x") + ", staticRules[" + this.IndexOfRule(Reduction.ToReduceRule) + "])");
                first = false;
            }
            stream.WriteLine("})");
        }
        protected void Export_States(StreamWriter stream)
        {
            stream.WriteLine("        private static LR1State[] staticStates = {");
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
    }
}
