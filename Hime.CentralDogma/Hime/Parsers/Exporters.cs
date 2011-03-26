using System.Collections.Generic;

namespace Hime.Parsers.Exporters
{
    class TextLexerExporter
    {
        private List<Terminal> symbols;
        private List<int> indices;
        private Automata.DFA finalDFA;
        private Terminal separator;
        private string _namespace;
        private System.IO.StreamWriter stream;
        private string name;

        public TextLexerExporter(System.IO.StreamWriter Stream, string Namespace, string Name, Automata.DFA DFA, Terminal Separator)
        {
            stream = Stream;
            finalDFA = DFA;
            _namespace = Namespace;
            name = Name;
            separator = Separator;
            symbols = new List<Terminal>();
            indices = new List<int>();
            foreach (Automata.DFAState State in finalDFA.States)
            {
                if (State.Final != null)
                {
                    if (symbols.Contains(State.Final))
                    {
                        indices.Add(symbols.IndexOf(State.Final));
                    }
                    else
                    {
                        indices.Add(symbols.Count);
                        symbols.Add(State.Final);
                    }
                }
                else
                {
                    indices.Add(-1);
                }
            }
        }

        public void Export()
        {
            stream.WriteLine("    class " + name + "_Lexer : Hime.Redist.Parsers.LexerText");
            stream.WriteLine("    {");
            Export_SymbolIDs();
            Export_SymbolNames();
            foreach (Automata.DFAState State in finalDFA.States)
                Export_Transition_State(State);
            Export_Transitions();
            Export_Finals();
            Export_Setup();
            Export_Clone();
            Export_Constructor();
            stream.WriteLine("    }");
        }

        private void Export_Constructor()
        {
            stream.WriteLine("        public " + name + "_Lexer(string input) : base(new System.IO.StringReader(input)) {}");
            stream.WriteLine("        public " + name + "_Lexer(System.IO.TextReader input) : base(input) {}");
            stream.WriteLine("        public " + name + "_Lexer(" + name + "_Lexer original) : base(original) {}");
        }
        private void Export_Clone()
        {
            stream.WriteLine("        public override Hime.Redist.Parsers.ILexer Clone() {");
            stream.WriteLine("            return new " + name + "_Lexer(this);");
            stream.WriteLine("        }");
        }
        private void Export_Setup()
        {
            stream.WriteLine("        protected override void setup() {");
            stream.WriteLine("            symbolsSID = staticSymbolsSID;");
            stream.WriteLine("            symbolsName = staticSymbolsName;");
            stream.WriteLine("            symbolsSubGrammars = new Dictionary<ushort, MatchSubGrammar>();");
            stream.WriteLine("            transitions = staticTransitions;");
            stream.WriteLine("            finals = staticFinals;");
            if (separator != null)
                stream.WriteLine("            separatorID = 0x" + separator.SID.ToString("X") + ";");
            stream.WriteLine("        }");
        }
        private void Export_SymbolIDs()
        {
            stream.Write("        private static ushort[] staticSymbolsSID = { ");
            bool first = true;
            foreach (Terminal terminal in symbols)
            {
                if (!first) stream.Write(", ");
                stream.Write("0x" + terminal.SID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        private void Export_SymbolNames()
        {
            stream.Write("        private static string[] staticSymbolsName = { ");
            bool first = true;
            foreach (Terminal terminal in symbols)
            {
                if (!first) stream.Write(", ");
                stream.Write("\"" + terminal.LocalName.Replace("\"", "\\\"") + "\"");
                first = false;
            }
            stream.WriteLine(" };");
        }
        private void Export_Transition_State(Automata.DFAState State)
        {
            stream.Write("        private static ushort[][] staticTransitions" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Automata.TerminalNFACharSpan Span in State.Transitions.Keys)
            {
                string begin = System.Convert.ToUInt16(Span.Begin).ToString("X");
                string end = System.Convert.ToUInt16(Span.End).ToString("X");
                string next = State.Transitions[Span].ID.ToString("X");
                if (!first) stream.Write(", ");
                stream.Write("new ushort[3] { 0x" + begin + ", 0x" + end + ", 0x" + next + " }");
                first = false;
            }
            stream.WriteLine(" };");
        }
        private void Export_Transitions()
        {
            stream.Write("        private static ushort[][][] staticTransitions = { ");
            bool first = true;
            foreach (Automata.DFAState State in finalDFA.States)
            {
                if (!first) stream.Write(", ");
                stream.Write("staticTransitions" + State.ID.ToString("X"));
                first = false;
            }
            stream.WriteLine(" };");
        }
        private void Export_Finals()
        {
            stream.Write("        private static int[] staticFinals = { ");
            bool first = true;
            foreach (int index in indices)
            {
                if (!first) stream.Write(", ");
                stream.Write(index);
                first = false;
            }
            stream.WriteLine(" };");
        }
    }

    class BinaryLexerExporter
    {
    }
}
