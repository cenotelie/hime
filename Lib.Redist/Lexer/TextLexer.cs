/*
 * Author: Laurent Wouters
 * Date: 02/06/2012
 * Time: 10:15
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Represents a lexer for a text stream
    /// </summary>
    public abstract class TextLexer : ILexer
    {
        private const int readerSize = 1024;        // Size of the encapsulated stream's buffer for reading
        private const int maxRewind = 128;          // Maximum number of character that can be rewound
        private const int initBufferSize = 128;     // Initial size of the buffer storing tokens' values

        // General data
        private TextLexerAutomaton lexAutomaton;                        // The automaton
        private Utils.SymbolDictionary<Symbols.Terminal> lexTerminals;  // The dictionary of symbols
        private ushort lexSeparator;                                    // Symbol ID of the SEPARATOR terminal
        // Runtime data
        private RewindableTextReader input;     // Lexer's input
        private int currentLine;                // Current line number in the input
        private int currentColumn;              // Current column in the input
        private bool isDollatEmited;            // Flags whether the input's end has been reached and the Dollar token emited
        private char[] buffer;                  // Buffer storing the tokens' values while matching
        private int bufferSize;                 // Size of the buffer

        /// <summary>
        /// Gets the terminals matched by this lexer
        /// </summary>
        public Utils.SymbolDictionary<Symbols.Terminal> Terminals { get { return lexTerminals; } }
        /// <summary>
        /// Gets the current line number
        /// </summary>
        public int CurrentLine { get { return currentLine; } }
        /// <summary>
        /// Gets the current column number
        /// </summary>
        public int CurrentColumn { get { return currentColumn; } }

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
        protected TextLexer(TextLexerAutomaton automaton, Symbols.Terminal[] terminals, ushort separator, System.IO.TextReader input)
        {
            this.lexAutomaton = automaton;
            this.lexTerminals = new Utils.SymbolDictionary<Symbols.Terminal>(terminals);
            this.lexSeparator = separator;
            this.input = new RewindableTextReader(input, readerSize, maxRewind);
            this.currentLine = 1;
            this.currentColumn = 1;
            this.isDollatEmited = false;
            this.bufferSize = initBufferSize;
            this.buffer = new char[initBufferSize];
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
                        OnError(new Parsers.UnexpectedCharError(c, currentLine, currentColumn));
                        AdvanceStats(c.ToString());
                    }
                }
                else if (token.SymbolID != lexSeparator)
                    return token;
            }
        }

        private void AdvanceStats(string text)
        {
            foreach (char c in text)
            {
                if (c == '\n')
                {
                    currentLine++;
                    currentColumn = 1;
                }
                else
                    currentColumn++;
            }
        }

        private Symbols.TextToken GetNextToken_DFA()
        {
            int matchedIndex = 0;           // Terminal's index of the last match
            int matchedLength = 0;          // Length of the last match
            int matchedNewColumn = 0;       // Current column at the end of the last match
            int matchedNewLine = 0;         // Current line at the end of the last match
            int readCount = 0;              // Number of read characters
            int readColumn = currentColumn; // Current column while reading
            int readLine = currentLine;     // Current line while reading
            bool flagCR = false;            // flag indicating whether the last read character was a Carriage Return
            ushort state = 0;               // Current state in the DFA

            while (state != 0xFFFF)
            {
                int offset = lexAutomaton.GetOffset(state);
                // Is this state a matching state ?
                ushort terminal = lexAutomaton.GetTerminal(offset);
                if (terminal != 0xFFFF)
                {
                    matchedIndex = terminal;
                    matchedLength = readCount;
                    matchedNewColumn = readColumn;
                    matchedNewLine = readLine;
                }
                // No further transition => exit
                if (lexAutomaton.HasNoTransition(offset))
                    break;
                // Read the next character and store it
                bool endOfInput = false;
                char current = input.Read(out endOfInput);
                if (endOfInput)
                    break;
                if (readCount == bufferSize)
                {
                    char[] temp = new char[bufferSize * 2];
                    System.Array.Copy(buffer, temp, bufferSize);
                    buffer = temp;
                    bufferSize *= 2;
                }
                buffer[readCount] = current;
                readCount++;
                // Advance stats (current line and column)
                switch ((int)current)
                {
                    case 0x0D:
                        flagCR = true;
                        readLine++;
                        readColumn = 0;
                        break;
                    case 0x0A:
                        if (!flagCR)
                        {
                            readLine++;
                            readColumn = 0;
                        }
                        else
                            readColumn++;
                        flagCR = false;
                        break;
                    case 0x0B:
                    case 0x0C:
                    case 0x85:
                    case 0x2028:
                    case 0x2029:
                        flagCR = false;
                        readLine++;
                        readColumn = 0;
                        break;
                    default:
                        flagCR = false;
                        readColumn++;
                        break;
                }
                // Try to find a transition from this state with the read character
                if (current <= 255)
                    state = lexAutomaton.GetCachedTransition(offset + current + 2);
                else
                    state = lexAutomaton.GetFallbackTransition(offset, current);
            }
            input.Rewind(readCount - matchedLength);
            if (matchedLength == 0)
                return null;
            Symbols.Terminal matched = lexTerminals[matchedIndex];
            Symbols.TextToken token = new Symbols.TextToken(matched.SymbolID, matched.Name, new string(buffer, 0, matchedLength), currentLine, currentColumn);
            currentLine = matchedNewLine;
            currentColumn = matchedNewColumn;
            return token;
        }
    }
}
