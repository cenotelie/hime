using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base for LR(0) parsers
    /// </summary>
    public abstract class BaseLR0Parser : IParser
    {
        /// <summary>
        /// Callback for rule productions
        /// </summary>
        /// <param name="parser">The reducing parser</param>
        protected delegate SyntaxTreeNode Production(BaseLR0Parser parser);

        /// <summary>
        /// Represents a LR(0) parser rule
        /// </summary>
        protected class Rule
        {
            /// <summary>
            /// Callback to invoke when reducing this rule
            /// </summary>
            public Production OnReduction;
            /// <summary>
            /// The rule's head variable
            /// </summary>
            public SymbolVariable Head;
            /// <summary>
            /// The rule's length
            /// </summary>
            public ushort Length;
            /// <summary>
            /// Initializes a new instance of the Rule structure with the given callback, variable and length
            /// </summary>
            /// <param name="prod">The callback for reductions</param>
            /// <param name="head">The head variable</param>
            /// <param name="length">The rule's length</param>
            public Rule(Production prod, SymbolVariable head, ushort length)
            {
                OnReduction = prod;
                Head = head;
                Length = length;
            }
        }

        /// <summary>
        /// Represents a state of the LR(0) parser automaton
        /// </summary>
        protected struct State
        {
            /// <summary>
            /// Array of the string representations of items of this state
            /// </summary>
            /// <remarks>
            /// This attribute is filled only if the debug option has been selected at generation time.
            /// </remarks>
            public string[] Items;
            /// <summary>
            /// Array of the the expected terminals at this state.
            /// This is used for error recovery.
            /// </summary>
            public SymbolTerminal[] Expected;
            /// <summary>
            /// Dictionary associating the ID of the next state given a terminal ID
            /// </summary>
            public Dictionary<ushort, ushort> ShiftsOnTerminal;
            /// <summary>
            /// Dictionary associating the ID of the next state given a variable ID
            /// </summary>
            public Dictionary<ushort, ushort> ShiftsOnVariable;
            /// <summary>
            /// Rule to reduce on this state
            /// </summary>
            public Rule Reduction;
            /// <summary>
            /// Initializes a new instance of the State structure with its inner data
            /// </summary>
            /// <param name="items">State's items</param>
            /// <param name="expected">Expected terminals for this state</param>
            /// <param name="st_keys">Terminal IDs for shift actions</param>
            /// <param name="st_val">Next state IDs for shift actions on terminals</param>
            /// <param name="sv_keys">Variable IDs for shift actions</param>
            /// <param name="sv_val">Next state IDs for shift actions on variables</param>
            public State(string[] items, SymbolTerminal[] expected, ushort[] st_keys, ushort[] st_val, ushort[] sv_keys, ushort[] sv_val)
            {
                Items = items;
                Expected = expected;
                ShiftsOnTerminal = new Dictionary<ushort, ushort>();
                ShiftsOnVariable = new Dictionary<ushort, ushort>();
                Reduction = null;
                for (int i = 0; i != st_keys.Length; i++)
                    ShiftsOnTerminal.Add(st_keys[i], st_val[i]);
                for (int i = 0; i != sv_keys.Length; i++)
                    ShiftsOnVariable.Add(sv_keys[i], sv_val[i]);
            }
            /// <summary>
            /// Initializes a new instance of the State structure with its inner data
            /// </summary>
            /// <param name="items">State's items</param>
            /// <param name="reduction">The rule to reduce at this state</param>
            public State(string[] items, Rule reduction)
            {
                Items = items;
                Expected = null;
                ShiftsOnTerminal = null;
                ShiftsOnVariable = null;
                Reduction = reduction;
            }


            /// <summary>
            /// Gets a list of the expected terminal IDs
            /// </summary>
            /// <returns>The list of expected terminal IDs, or null if this is a reduction state</returns>
            public ushort[] GetExpectedIDs()
            {
                if (Expected == null) return null;
                ushort[] results = new ushort[Expected.Length];
                for (int i = 0; i != Expected.Length; i++)
                    results[i] = Expected[i].SymbolID;
                return results;
            }
            /// <summary>
            /// Gets a list of the expected terminal names
            /// </summary>
            /// <returns>The list of expected terminal names, or null if this is a reduction state</returns>
            public string[] GetExpectedNames()
            {
                if (Expected == null) return null;
                string[] results = new string[Expected.Length];
                for (int i = 0; i != Expected.Length; i++)
                    results[i] = Expected[i].Name;
                return results;
            }
            /// <summary>
            /// Gets the next state ID by a shift action on the given terminal ID
            /// </summary>
            /// <param name="sid">The terminal ID</param>
            /// <returns>The next state's ID by a shift action on the given terminal ID, or 0xFFFF if no suitable shift action is found</returns>
            public ushort GetNextByShiftOnTerminal(ushort sid)
            {
                if (!ShiftsOnTerminal.ContainsKey(sid))
                    return 0xFFFF;
                return ShiftsOnTerminal[sid];
            }
            /// <summary>
            /// Gets the next state ID by a shift action on the given variable ID
            /// </summary>
            /// <param name="sid">The variable ID</param>
            /// <returns>The next state's ID by a shift action on the given variable ID, or 0xFFFF if no suitable shift action is found</returns>
            public ushort GetNextByShiftOnVariable(ushort sid)
            {
                if (!ShiftsOnVariable.ContainsKey(sid))
                    return 0xFFFF;
                return ShiftsOnVariable[sid];
            }
            /// <summary>
            /// Determines whether a reduction can occur
            /// </summary>
            /// <returns>True if a reduction is found, false otherwise</returns>
            public bool HasReduction() { return (Reduction != null); }
        }


        // Parser automata data
        /// <summary>
        /// Rules of the LR(0) parser
        /// </summary>
        protected Rule[] rules;
        /// <summary>
        /// States of the LR(0) parser automaton
        /// </summary>
        protected State[] states;
        /// <summary>
        /// Number of tokens to correctly match during an error recovery procedure
        /// </summary>
        protected int errorSimulationLength;

        // Parser state data
        /// <summary>
        /// Maximum number of errors before determining the parser definitely fails
        /// </summary>
        protected int maxErrorCount;
        /// <summary>
        /// List of the encountered syntaxic errors
        /// </summary>
        protected List<ParserError> errors;
        /// <summary>
        /// Read-only list of the errors
        /// </summary>
        protected System.Collections.ObjectModel.ReadOnlyCollection<ParserError> readonlyErrors;
        /// <summary>
        /// Lexer associated to this parser
        /// </summary>
        protected ILexer lexer;
        /// <summary>
        /// Buffer for nodes of the AST being constructed
        /// </summary>
        protected LinkedList<SyntaxTreeNode> nodes;
        /// <summary>
        /// Parser's stack
        /// </summary>
        protected Stack<ushort> stack;
        /// <summary>
        /// The next token in the input
        /// </summary>
        protected SymbolToken nextToken;
        /// <summary>
        /// ID of the parser's current state
        /// </summary>
        protected ushort currentState;

        /// <summary>
        /// Gets a read-only collection of syntaxic errors encountered by the parser
        /// </summary>
        public ICollection<ParserError> Errors { get { return readonlyErrors; } }

        /// <summary>
        /// Initialization method to be overriden
        /// </summary>
        protected abstract void setup();
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <param name="lexer">Base lexer for reading tokens</param>
        /// <param name="state">Parser's current state</param>
        /// <returns>The next token in the input</returns>
        protected abstract SymbolToken GetNextToken(ILexer lexer, ushort state);

        /// <summary>
        /// Initializes a new instance of the BaseLR1Parser class with the given lexer
        /// </summary>
        /// <param name="input"></param>
        public BaseLR0Parser(ILexer input)
        {
            setup();
            maxErrorCount = 100;
            errors = new List<ParserError>();
            readonlyErrors = new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(errors);
            lexer = input;
            nodes = new LinkedList<SyntaxTreeNode>();
            stack = new Stack<ushort>();
            currentState = 0x0;
            nextToken = null;
        }

        protected bool Analyse_RunForToken(SymbolToken token)
        {
            while (true)
            {
                ushort NextState = states[currentState].GetNextByShiftOnTerminal(token.SymbolID);
                if (NextState != 0xFFFF)
                {
                    nodes.AddLast(new SyntaxTreeNode(token));
                    currentState = NextState;
                    stack.Push(currentState);
                    return true;
                }
                if (states[currentState].HasReduction())
                {
                    Rule rule = states[currentState].Reduction;
                    Production Reduce = rule.OnReduction;
                    ushort HeadID = rule.Head.SymbolID;
                    nodes.AddLast(Reduce(this));
                    for (ushort j = 0; j != rule.Length; j++)
                        stack.Pop();
                    // Shift to next state on the reduce variable
                    NextState = states[stack.Peek()].GetNextByShiftOnVariable(HeadID);
                    if (NextState == 0xFFFF)
                        return false;
                    currentState = NextState;
                    stack.Push(currentState);
                    continue;
                }
                return false;
            }
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public SyntaxTreeNode Analyse()
        {
            stack.Push(currentState);
            nextToken = GetNextToken(lexer, currentState);

            while (true)
            {
                if (Analyse_RunForToken(nextToken))
                {
                    nextToken = GetNextToken(lexer, currentState);
                    continue;
                }
                else if (nextToken.SymbolID == 0x0001)
                    return nodes.First.Value.ApplyActions();
                else
                {
                    errors.Add(new ParserErrorUnexpectedToken(nextToken, states[currentState].GetExpectedNames()));
                    return null;
                }
            }
        }
    }



    /// <summary>
    /// Base class for text-based LR(0) parsers
    /// </summary>
    public abstract class LR0TextParser : BaseLR0Parser
    {
        protected LR0TextParser(LexerText lexer) : base(lexer) { }
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <param name="lexer">Base lexer for reading tokens</param>
        /// <param name="state">Parser's current state</param>
        /// <returns>The next token in the input</returns>
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(); }
    }
}
