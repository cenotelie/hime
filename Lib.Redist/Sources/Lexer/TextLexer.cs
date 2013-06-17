using System.Collections.Generic;

namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Represents a lexer for a text stream
    /// </summary>
    public abstract class TextLexer : ILexer
    {
        private const int maxRewind = 128;          // Maximum number of character that can be rewound

        // General data
        private Automaton lexAutomaton;                        // The automaton
        private SymbolDictionary<Symbols.Terminal> lexTerminals;  // The dictionary of symbols
        private int lexSeparator;                                       // Symbol ID of the SEPARATOR terminal
        // Runtime data
        private TextContent content;            // Container for all read text
        private RewindableTextReader input;     // Lexer's input
        private bool isDollatEmited;            // Flags whether the input's end has been reached and the Dollar token emited
        private int index;                      // The current index in the input

        /// <summary>
        /// Gets the terminals matched by this lexer
        /// </summary>
        public SymbolDictionary<Symbols.Terminal> Terminals { get { return lexTerminals; } }

        /// <summary>
        /// Gets the text content that served as input
        /// </summary>
        public TextContent Input { get { return content; } }
        
        /// <summary>
        /// Events for lexical errors
        /// </summary>
        internal event AddLexicalError OnError;

        /// <summary>
        /// Initializes a new instance of the TextLexer class with the given input
        /// </summary>
        /// <param name="automaton">DFA automaton for this lexer</param>
        /// <param name="terminals">Terminals recognized by this lexer</param>
        /// <param name="separator">SID of the separator token</param>
        /// <param name="input">Input to this lexer</param>
        protected TextLexer(Automaton automaton, Symbols.Terminal[] terminals, int separator, System.IO.TextReader input)
        {
            this.lexAutomaton = automaton;
            this.lexTerminals = new SymbolDictionary<Symbols.Terminal>(terminals);
            this.lexSeparator = separator;
            this.content = new TextContent();
            this.input = new RewindableTextReader(input, content, maxRewind);
            this.isDollatEmited = false;
        }

        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        public Symbols.Token GetNextToken()
        {
            if (isDollatEmited)
                return Symbols.Epsilon.Instance;
            while (true)
            {
                Symbols.TextToken token = GetNextToken_DFA();
                if (token == null)
                {
                    bool atend = false;
                    char c = input.Read(out atend);
                    if (atend)
                    {
                        isDollatEmited = true;
                        return Symbols.Dollar.Instance;
                    }
                    else
                    {
                        OnError(new UnexpectedCharError(c, content.GetPositionAt(index)));
                        index++;
                    }
                }
                else if (token.SymbolID != lexSeparator)
                    return token;
            }
        }

        private Symbols.TextToken GetNextToken_DFA()
        {
            int matchedIndex = 0;           // Terminal's index of the last match
            int matchedLength = 0;          // Length of the last match
            int length = 0;
            int state = 0;                  // Current state in the DFA

            while (state != 0xFFFF)
            {
                int offset = lexAutomaton.GetOffset(state);
                // Is this state a matching state ?
                int terminal = lexAutomaton.GetTerminalIndex(offset);
                if (terminal != 0xFFFF)
                {
                    matchedIndex = terminal;
                    matchedLength = length;
                }
                // No further transition => exit
                if (lexAutomaton.HasNoTransition(offset))
                    break;
                // Read the next character and store it
                bool endOfInput = false;
                char current = input.Read(out endOfInput);
                if (endOfInput)
                    break;
                length++;
                // Try to find a transition from this state with the read character
                if (current <= 255)
                    state = lexAutomaton.GetCachedTransition(offset + current + 3);
                else
                    state = lexAutomaton.GetFallbackTransition(offset, current);
            }
            input.Rewind(length - matchedLength);
            if (matchedLength == 0)
                return null;
            Symbols.Terminal matched = lexTerminals[matchedIndex];
            Symbols.TextToken token = new Symbols.TextToken(matched.SymbolID, matched.Name, content, index, matchedLength);
            index += matchedLength;
            return token;
        }
    }
}
