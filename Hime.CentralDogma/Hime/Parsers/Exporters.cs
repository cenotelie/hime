namespace Hime.Parsers
{
    public interface LexerExporter
    {
        void Export(string Name, Grammar Grammar, System.IO.StreamWriter Stream);
    }
    public interface ParserExporter
    {
        void Export(string Name, Grammar Grammar, ParseMethod Method, System.IO.StreamWriter Stream);
    }



    public class DefaultTextLexerExporter : LexerExporter
    {
        private System.Collections.Generic.List<Terminal> p_Symbols;
        private System.Collections.Generic.List<int> p_Indices;

        private void Prepare(Grammar Grammar)
        {
            p_Symbols = new System.Collections.Generic.List<Terminal>();
            p_Indices = new System.Collections.Generic.List<int>();
            foreach (Automata.DFAState State in Grammar.FinalDFA.States)
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

        public void Export(string Name, Grammar Grammar, System.IO.StreamWriter Stream)
        {
            Prepare(Grammar);
            Stream.WriteLine("    public class " + Name + " : Hime.Redist.Parsers.LexerText");
            Stream.WriteLine("    {");
            Export_SymbolIDs(Grammar, Stream);
            Export_SymbolNames(Grammar, Stream);
            foreach (Automata.DFAState State in Grammar.FinalDFA.States)
                Export_Transition_State(State, Stream);
            Export_Transitions(Grammar, Stream);
            Export_Finals(Stream);

            string separator = Grammar.GetOption("Separator");
            if (separator != null)
            {
                Terminal terminal = Grammar.GetTerminal(separator);
                if (terminal != null)
                    Stream.WriteLine("        private static ushort p_StaticSeparator = 0x" + terminal.SID.ToString("X") + ";");
            }

            Export_Setup(Grammar, Stream);
            Export_Clone(Name, Stream);
            Export_Constructor(Name, Stream);
            Stream.WriteLine("    }");
        }


        private void Export_Constructor(string Name, System.IO.StreamWriter Stream)
        {
            Stream.WriteLine("        public " + Name + "(string input) : base(input) {}");
            Stream.WriteLine("        public " + Name + "(string input, int position, int line, System.Collections.Generic.List<Hime.Redist.Parsers.LexerTextError> errors) : base(input, position, line, errors) {}");
        }
        private void Export_Clone(string Name, System.IO.StreamWriter Stream)
        {
            Stream.WriteLine("        public override Hime.Redist.Parsers.ILexer Clone() {");
            Stream.WriteLine("            return new " + Name + "(p_Input, p_CurrentPosition, p_Line, p_Errors);");
            Stream.WriteLine("        }");
        }
        private void Export_Setup(Grammar Grammar, System.IO.StreamWriter Stream)
        {
            Stream.WriteLine("        protected override void setup() {");
            Stream.WriteLine("            p_SymbolsSID = p_StaticSymbolsSID;");
            Stream.WriteLine("            p_SymbolsName = p_StaticSymbolsName;");
            Stream.WriteLine("            p_SymbolsSubGrammars = new System.Collections.Generic.Dictionary<ushort, MatchSubGrammar>();");
            Stream.WriteLine("            p_Transitions = p_StaticTransitions;");
            Stream.WriteLine("            p_Finals = p_StaticFinals;");

            string separator = Grammar.GetOption("Separator");
            if (separator != null)
            {
                Terminal terminal = Grammar.GetTerminal(separator);
                if (terminal != null)
                    Stream.WriteLine("            p_SeparatorID = 0x" + terminal.SID.ToString("X") + ";");
            }
            Stream.WriteLine("        }");
        }
        private void Export_SymbolIDs(Grammar Grammar, System.IO.StreamWriter Stream)
        {
            Stream.Write("        private static ushort[] p_StaticSymbolsSID = { ");
            bool first = true;
            foreach (Terminal terminal in p_Symbols)
            {
                if (!first) Stream.Write(", ");
                Stream.Write("0x" + terminal.SID.ToString("X"));
                first = false;
            }
            Stream.WriteLine(" };");
        }
        private void Export_SymbolNames(Grammar Grammar, System.IO.StreamWriter Stream)
        {
            Stream.Write("        private static string[] p_StaticSymbolsName = { ");
            bool first = true;
            foreach (Terminal terminal in p_Symbols)
            {
                if (!first) Stream.Write(", ");
                Stream.Write("\"" + terminal.LocalName.Replace("\"", "\\\"") + "\"");
                first = false;
            }
            Stream.WriteLine(" };");
        }
        private void Export_Transition_State(Automata.DFAState State, System.IO.StreamWriter Stream)
        {
            Stream.Write("        private static ushort[][] p_StaticTransitions" + State.ID.ToString("X") + " = { ");
            bool first = true;
            foreach (Automata.TerminalNFACharSpan Span in State.Transitions.Keys)
            {
                string begin = System.Convert.ToUInt16(Span.Begin).ToString("X");
                string end = System.Convert.ToUInt16(Span.End).ToString("X");
                string next = State.Transitions[Span].ID.ToString("X");
                if (!first) Stream.Write(", ");
                Stream.Write("new ushort[3] { 0x" + begin + ", 0x" + end + ", 0x" + next + " }");
                first = false;
            }
            Stream.WriteLine(" };");
        }
        private void Export_Transitions(Grammar Grammar, System.IO.StreamWriter Stream)
        {
            Stream.Write("        private static ushort[][][] p_StaticTransitions = { ");
            bool first = true;
            foreach (Automata.DFAState State in Grammar.FinalDFA.States)
            {
                if (!first) Stream.Write(", ");
                Stream.Write("p_StaticTransitions" + State.ID.ToString("X"));
                first = false;
            }
            Stream.WriteLine(" };");
        }
        private void Export_Finals(System.IO.StreamWriter Stream)
        {
            Stream.Write("        private static int[] p_StaticFinals = { ");
            bool first = true;
            foreach (int index in p_Indices)
            {
                if (!first) Stream.Write(", ");
                Stream.Write(index);
                first = false;
            }
            Stream.WriteLine(" };");
        }
    }
}