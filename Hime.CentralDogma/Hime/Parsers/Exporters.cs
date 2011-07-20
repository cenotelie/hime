using System.Collections.Generic;

namespace Hime.Parsers.Exporters
{
    class TextLexerExporter
    {
        private Automata.DFA finalDFA;
        private List<Terminal> terminals;
        private Terminal separator;
        private string _namespace;
        private System.IO.StreamWriter stream;
        private string name;

        public TextLexerExporter(System.IO.StreamWriter Stream, string Namespace, string Name, Automata.DFA DFA, Terminal Separator)
        {
            stream = Stream;
            finalDFA = DFA;
            terminals = new List<Terminal>();
            _namespace = Namespace;
            name = Name;
            separator = Separator;
            terminals.Add(TerminalEpsilon.Instance);
            terminals.Add(TerminalDollar.Instance);
            foreach (Automata.DFAState state in finalDFA.States)
            {
                if (state.Final != null)
                    if (!terminals.Contains(state.Final))
                        terminals.Add(state.Final);
            }
        }

        public List<Terminal> Export()
        {
            stream.WriteLine("    class " + name + "_Lexer : LexerText");
            stream.WriteLine("    {");
            Export_Terminals();
            Export_States();
            Export_Setup();
            Export_Clone();
            Export_Constructor();
            stream.WriteLine("    }");
            return terminals;
        }

        private void Export_Constructor()
        {
            stream.WriteLine("        public " + name + "_Lexer(string input) : base(new System.IO.StringReader(input)) {}");
            stream.WriteLine("        public " + name + "_Lexer(System.IO.TextReader input) : base(input) {}");
            stream.WriteLine("        public " + name + "_Lexer(" + name + "_Lexer original) : base(original) {}");
        }
        private void Export_Clone()
        {
            stream.WriteLine("        public override ILexer Clone() {");
            stream.WriteLine("            return new " + name + "_Lexer(this);");
            stream.WriteLine("        }");
        }
        private void Export_Setup()
        {
            stream.WriteLine("        protected override void setup() {");
            stream.WriteLine("            states = staticStates;");
            stream.WriteLine("            subGrammars = new Dictionary<ushort, MatchSubGrammar>();");
            if (separator != null)
                stream.WriteLine("            separatorID = 0x" + separator.SID.ToString("X") + ";");
            stream.WriteLine("        }");
        }
        protected void Export_Terminals()
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
        private void Export_State(Automata.DFAState state)
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
                foreach (Automata.TerminalNFACharSpan span in state.Transitions.Keys)
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
        private void Export_States()
        {
            stream.WriteLine("        private static LexerDFAState[] staticStates = { ");
            bool first = true;
            foreach (Automata.DFAState State in finalDFA.States)
            {
                if (!first) stream.WriteLine(",");
                Export_State(State);
                first = false;
            }
            stream.WriteLine(" };");
        }
    }

    class BinaryLexerExporter
    {
    }
}
