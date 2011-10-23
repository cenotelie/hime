/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;
using System.Collections.Generic;
using System.IO;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
    class ParserDataLR0 : ParserDataLR
    {
		internal protected override string GetBaseClassName 
		{
			get { return "LR0TextParser"; }
		}
		
        public ParserDataLR0(Reporter reporter, CFGrammar gram, Graph graph) : base(reporter, gram, graph) { }

        public override void Export(StreamWriter stream, string className, AccessModifier modifier, string lexerClassName, IList<Terminal> expected, bool exportDebug)
        {
			base.Export(stream, className, modifier, lexerClassName, expected, exportDebug);

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

		override protected void ExportRules(StreamWriter stream)
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
        
        protected void Export_State_Shifts(StreamWriter stream, State state)
        {
            TerminalSet expected = new TerminalSet();
            foreach (GrammarSymbol Symbol in state.Children.Keys)
            {
                if (Symbol is Terminal)
                    expected.Add((Terminal)Symbol);
            }
            bool first = true;
            stream.WriteLine("new LR0State(");
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
            stream.WriteLine("})");
        }
        protected void Export_State_Reduction(StreamWriter stream, State state)
        {
            bool first = true;
            stream.WriteLine("new LR0State(");
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
            // Write reductions
            stream.Write("               staticRules[" + this.IndexOfRule(state.Reductions[0].ToReduceRule) + "]");
            stream.WriteLine(")");
        }

        protected void Export_States(StreamWriter stream)
        {
            stream.WriteLine("        private static LR0State[] staticStates = {");
            bool first = true;
            foreach (State state in graph.States)
            {
                stream.Write("            ");
                if (!first) stream.Write(", ");
                if (state.Reductions.Count == 0)
                    Export_State_Shifts(stream, state);
                else
                    Export_State_Reduction(stream, state);
                first = false;
            }
            stream.WriteLine("        };");
        }
    }
}
