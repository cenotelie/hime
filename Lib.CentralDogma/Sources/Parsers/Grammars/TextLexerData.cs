/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using Hime.Utils.Reporting;

namespace Hime.Parsers
{
    class TextLexerData : LexerData
    {
        private Automata.DFA dfa;
        private List<Terminal> terminals;
        private Terminal separator;

        public IList<Terminal> Expected { get { return terminals; } }

        internal TextLexerData(Automata.DFA dfa, Terminal separator)
        {
            this.dfa = dfa;
            this.terminals = new List<Terminal>();
            this.separator = separator;
            this.terminals.Add(TerminalEpsilon.Instance);
            this.terminals.Add(TerminalDollar.Instance);
            foreach (Automata.DFAState state in dfa.States)
            {
                if (state.Final != null)
                    if (!terminals.Contains(state.Final))
                        terminals.Add(state.Final);
            }
        }

        public void ExportCode(StreamWriter codeStream, string className, AccessModifier modifier, string resource)
        {
            codeStream.WriteLine("    " + modifier.ToString().ToLower() + " class " + className + " : TextLexer");
            codeStream.WriteLine("    {");
            ExportStatics(codeStream, className, resource);
            ExportConstructor(codeStream, className);
            codeStream.WriteLine("    }");
        }
        public void ExportData(BinaryWriter dataStream)
        {
            ExportStates(dataStream);
        }

        private void ExportConstructor(StreamWriter stream, string className)
        {
            string sep = "FFFF";
            if (separator != null) sep = separator.SID.ToString("X");
            stream.WriteLine("        public " + className + "(string input) : base(automaton, terminals, 0x" + sep + ", new System.IO.StringReader(input)) {}");
            stream.WriteLine("        public " + className + "(System.IO.TextReader input) : base(automaton, terminals, 0x" + sep + ", input) {}");
        }
        private void ExportStatics(StreamWriter stream, string className, string resource)
        {
            stream.WriteLine("        private static readonly TextLexerAutomaton automaton = TextLexerAutomaton.FindAutomaton(typeof(" + className + ").Assembly, \"" + resource + ".lexer\");");
            stream.WriteLine("        public static readonly SymbolTerminal[] terminals = {");
            bool first = true;
            foreach (Terminal terminal in terminals)
            {
                if (!first) stream.WriteLine(",");
                stream.Write("            ");
                stream.Write("new SymbolTerminal(0x" + terminal.SID.ToString("X") + ", \"" + terminal.ToString().Replace("\"", "\\\"") + "\")");
                first = false;
            }
            stream.WriteLine(" };");
        }
        private void ExportState(BinaryWriter dataStream, Automata.DFAState state)
        {
            ushort[] cache = new ushort[256];
            for (int i = 0; i != 256; i++)
                cache[i] = 0xFFFF;

            foreach (Automata.CharSpan span in state.Transitions.Keys)
            {
                if (span.Begin <= 255)
                {
                    int end = (span.End < 255 ? span.End : 255);
                    for (int i = span.Begin; i != end + 1; i++)
                        cache[i] = (ushort)state.Transitions[span].ID;
                }
            }

            if (state.Final != null)
                dataStream.Write((ushort)terminals.IndexOf(state.Final));
            else
                dataStream.Write((ushort)0xFFFF);
            dataStream.Write((ushort)state.TransitionsCount);

            for (int i = 0; i != 256; i++)
                dataStream.Write(cache[i]);

            List<Automata.CharSpan> keys = new List<Automata.CharSpan>(state.Transitions.Keys);
            keys.Sort(new Comparison<Automata.CharSpan>(Automata.CharSpan.CompareReverse));
            foreach (Automata.CharSpan span in keys)
            {
                dataStream.Write(System.Convert.ToUInt16(span.Begin));
                dataStream.Write(System.Convert.ToUInt16(span.End));
                dataStream.Write((ushort)state.Transitions[span].ID);
            }
        }
        private void ExportStates(BinaryWriter dataStream)
        {
            dataStream.Write(dfa.States.Count);
            int offset = 0;
            foreach (Automata.DFAState state in dfa.States)
            {
                dataStream.Write(offset);
                offset += state.TransitionsCount * 3 + 258;
            }
            foreach (Automata.DFAState state in dfa.States)
                ExportState(dataStream, state);
        }
    }
}
