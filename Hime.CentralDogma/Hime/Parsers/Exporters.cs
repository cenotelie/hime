namespace Hime.Parsers.Exporters
{
    class TextLexerExporter
    {
        private System.Collections.Generic.List<Terminal> p_Symbols;
        private System.Collections.Generic.List<int> p_Indices;
        private Automata.DFA p_FinalDFA;
        private Terminal p_Separator;
        private string p_Namespace;
        private System.IO.StreamWriter p_Stream;
        private string p_Name;

        public TextLexerExporter(System.IO.StreamWriter Stream, string Namespace, string Name, Automata.DFA DFA, Terminal Separator)
        {
            p_Stream = Stream;
            p_FinalDFA = DFA;
            p_Namespace = Namespace;
            p_Name = Name;
            p_Separator = Separator;
            p_Symbols = new System.Collections.Generic.List<Terminal>();
            p_Indices = new System.Collections.Generic.List<int>();
            foreach (Automata.DFAState State in p_FinalDFA.States)
            {
                if (State.Final != null)
                {
                    if (p_Symbols.Contains(State.Final))
                    {
                        p_Indices.Add(p_Symbols.IndexOf(State.Final));
                    }
                    else
                    {
                        p_Indices.Add(p_Symbols.Count);
                        p_Symbols.Add(State.Final);
                    }
                }
                else
                {
                    p_Indices.Add(-1);
                }
            }
        }

        public void Export()
        {
            p_Stream.WriteLine("    class " + p_Name + "_Lexer : Hime.Redist.Parsers.LexerText");
            p_Stream.WriteLine("    {");
            Export_SymbolIDs();
            Export_SymbolNames();
            foreach (Automata.DFAState State in p_FinalDFA.States)
                Export_Transition_State(State);
            Export_Transitions();
            Export_Finals();
            Export_Setup();
            Export_Clone();
            Export_Constructor();
            p_Stream.WriteLine("    }");
        }

        private void Export_Constructor()
        {
            p_Stream.WriteLine("        public " + p_Name + "_Lexer(string input) : base(input) {}");
            p_Stream.WriteLine("        public " + p_Name + "_Lexer(string input, int position, int line, System.Collections.Generic.List<Hime.Redist.Parsers.LexerTextError> errors) : base(input, position, line, errors) {}");
        }
        private void Export_Clone()
        {
            p_Stream.WriteLine("        public override Hime.Redist.Parsers.ILexer Clone() {");
            p_Stream.WriteLine("            return new " + p_Name + "_Lexer(p_Input, p_CurrentPosition, p_Line, p_Errors);");
            p_Stream.WriteLine("        }");
        }
        private void Export_Setup()
        {
            p_Stream.WriteLine("        protected override void setup() {");
            p_Stream.WriteLine("            p_SymbolsSID = p_StaticSymbolsSID;");
            p_Stream.WriteLine("            p_SymbolsName = p_StaticSymbolsName;");
            p_Stream.WriteLine("            p_SymbolsSubGrammars = new System.Collections.Generic.Dictionary<ushort, MatchSubGrammar>();");
            p_Stream.WriteLine("            p_Transitions = p_StaticTransitions;");
            p_Stream.WriteLine("            p_Finals = p_StaticFinals;");
            if (p_Separator != null)
                p_Stream.WriteLine("            p_SeparatorID = 0x" + p_Separator.SID.ToString("X") + ";");
            p_Stream.WriteLine("        }");
        }
        private void Export_SymbolIDs()
        {
            p_Stream.Write("        private static ushort[] p_StaticSymbolsSID = { ");
            bool first = true;
            foreach (Terminal terminal in p_Symbols)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("0x" + terminal.SID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        private void Export_SymbolNames()
        {
            p_Stream.Write("        private static string[] p_StaticSymbolsName = { ");
            bool first = true;
            foreach (Terminal terminal in p_Symbols)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("\"" + terminal.LocalName.Replace("\"", "\\\"") + "\"");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        private void Export_Transition_State(Automata.DFAState State)
        {
            p_Stream.Write("        private static ushort[][] p_StaticTransitions" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Automata.TerminalNFACharSpan Span in State.Transitions.Keys)
            {
                string begin = System.Convert.ToUInt16(Span.Begin).ToString("X");
                string end = System.Convert.ToUInt16(Span.End).ToString("X");
                string next = State.Transitions[Span].ID.ToString("X");
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("new ushort[3] { 0x" + begin + ", 0x" + end + ", 0x" + next + " }");
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        private void Export_Transitions()
        {
            p_Stream.Write("        private static ushort[][][] p_StaticTransitions = { ");
            bool first = true;
            foreach (Automata.DFAState State in p_FinalDFA.States)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write("p_StaticTransitions" + State.ID.ToString("X"));
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
        private void Export_Finals()
        {
            p_Stream.Write("        private static int[] p_StaticFinals = { ");
            bool first = true;
            foreach (int index in p_Indices)
            {
                if (!first) p_Stream.Write(", ");
                p_Stream.Write(index);
                first = false;
            }
            p_Stream.WriteLine(" };");
        }
    }

    class BinaryLexerExporter
    {
    }
}