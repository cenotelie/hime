/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using Hime.Kernel.Reporting;

namespace Hime.Parsers
{
    public sealed class TextLexerData : LexerData
    {
        private Automata.DFA dfa;
        private List<Terminal> terminals;
        private Terminal separator;

        public IList<Terminal> Expected { get { return terminals; } }

        public TextLexerData(Automata.DFA dfa, Terminal separator)
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

        public void Export(StreamWriter stream, string className, AccessModifier modifier)
        {
            stream.WriteLine("    " + modifier.ToString().ToLower() + " class " + className + " : LexerText");
            stream.WriteLine("    {");
            ExportTerminals(stream);
            ExportStates(stream);
            ExportSetup(stream);
            ExportClone(stream, className);
            ExportConstructor(stream, className);
            stream.WriteLine("    }");
        }

        private void ExportConstructor(StreamWriter stream, string className)
        {
            stream.WriteLine("        public " + className + "(string input) : base(new System.IO.StringReader(input)) {}");
            stream.WriteLine("        public " + className + "(System.IO.TextReader input) : base(input) {}");
            stream.WriteLine("        public " + className + "(" + className + " original) : base(original) {}");
        }
        private void ExportClone(StreamWriter stream, string className)
        {
            stream.WriteLine("        public override ILexer Clone() {");
            stream.WriteLine("            return new " + className + "(this);");
            stream.WriteLine("        }");
        }
        private void ExportSetup(StreamWriter stream)
        {
            stream.WriteLine("        protected override void setup() {");
            stream.WriteLine("            states = staticStates;");
            stream.WriteLine("            subGrammars = new Dictionary<ushort, MatchSubGrammar>();");
            if (separator != null)
                stream.WriteLine("            separatorID = 0x" + separator.SID.ToString("X") + ";");
            stream.WriteLine("        }");
        }
        private void ExportTerminals(StreamWriter stream)
        {
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
        private void ExportState(StreamWriter stream, Automata.DFAState state)
        {
            stream.Write("            ");
            if (state.Transitions.Count == 0)
            {
                stream.Write("new LexerDFAState(new ushort[][] {}, ");
                if (state.Final == null)
                    stream.Write("null)");
                else
                    stream.Write("terminals[0x" + terminals.IndexOf(state.Final).ToString("X") + "])");
            }
            else
            {
                stream.WriteLine("new LexerDFAState(new ushort[][] {");
                bool first = true;
                foreach (Automata.CharSpan span in state.Transitions.Keys)
                {
                    string begin = System.Convert.ToUInt16(span.Begin).ToString("X");
                    string end = System.Convert.ToUInt16(span.End).ToString("X");
                    string next = state.Transitions[span].ID.ToString("X");
                    if (!first) stream.WriteLine(",");
                    stream.Write("                ");
                    stream.Write("new ushort[3] { 0x" + begin + ", 0x" + end + ", 0x" + next + " }");
                    first = false;
                }
                stream.WriteLine("},");
                if (state.Final == null)
                    stream.Write("                null)");
                else
                    stream.Write("                terminals[0x" + terminals.IndexOf(state.Final).ToString("X") + "])");
            }
        }
        private void ExportStates(StreamWriter stream)
        {
            stream.WriteLine("        private static LexerDFAState[] staticStates = { ");
            bool first = true;
            foreach (Automata.DFAState state in dfa.States)
            {
                if (!first) stream.WriteLine(",");
                ExportState(stream, state);
                first = false;
            }
            stream.WriteLine(" };");
        }
    }
}
