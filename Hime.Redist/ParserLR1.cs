using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base for LR(1) parsers
    /// </summary>
    public abstract class BaseLR1Parser : IParser
    {
        /// <summary>
        /// Callback for rule productions
        /// </summary>
        /// <param name="parser">The reducing parser</param>
        protected delegate SyntaxTreeNode Production(BaseLR1Parser parser);

        /// <summary>
        /// Represents a LR(1) parser rule
        /// </summary>
        protected struct Rule
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
        /// Represents a rule reduction in a LR(1) parser
        /// </summary>
        protected struct Reduction
        {
            /// <summary>
            /// ID of the lookahead on which the reduction is triggered
            /// </summary>
            public ushort Lookahead;
            /// <summary>
            /// Rule for the reduction
            /// </summary>
            public Rule ToReduce;
            /// <summary>
            /// Initializes a new instance of the Reduction structure with the given lookahead and rule
            /// </summary>
            /// <param name="lookahead">The reduction's lookahead ID</param>
            /// <param name="rule">The rule for the reduction</param>
            public Reduction(ushort lookahead, Rule rule)
            {
                Lookahead = lookahead;
                ToReduce = rule;
            }
        }

        /// <summary>
        /// Represents a state of the LR(1) parser automaton
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
            /// Dictionary associating a reduction to terminal ID
            /// </summary>
            public Dictionary<ushort, Reduction> ReducsOnTerminal;
            /// <summary>
            /// Initializes a new instance of the State structure with its inner data
            /// </summary>
            /// <param name="items">State's items</param>
            /// <param name="expected">Expected terminals for this state</param>
            /// <param name="st_keys">Terminal IDs for shift actions</param>
            /// <param name="st_val">Next state IDs for shift actions on terminals</param>
            /// <param name="sv_keys">Variable IDs for shift actions</param>
            /// <param name="sv_val">Next state IDs for shift actions on variables</param>
            /// <param name="rt">Reductions that can occur in this state</param>
            public State(string[] items, SymbolTerminal[] expected, ushort[] st_keys, ushort[] st_val, ushort[] sv_keys, ushort[] sv_val, Reduction[] rt)
            {
                Items = items;
                Expected = expected;
                ShiftsOnTerminal = new Dictionary<ushort, ushort>();
                ShiftsOnVariable = new Dictionary<ushort, ushort>();
                ReducsOnTerminal = new Dictionary<ushort, Reduction>();
                for (int i = 0; i != st_keys.Length; i++)
                    ShiftsOnTerminal.Add(st_keys[i], st_val[i]);
                for (int i = 0; i != sv_keys.Length; i++)
                    ShiftsOnVariable.Add(sv_keys[i], sv_val[i]);
                for (int i = 0; i != rt.Length; i++)
                    ReducsOnTerminal.Add(rt[i].Lookahead, rt[i]);
            }
            /// <summary>
            /// Gets a list of the expected terminal IDs
            /// </summary>
            /// <returns>The list of expected terminal IDs</returns>
            public ushort[] GetExpectedIDs()
            {
                ushort[] results = new ushort[Expected.Length];
                for (int i = 0; i != Expected.Length; i++)
                    results[i] = Expected[i].SymbolID;
                return results;
            }
            /// <summary>
            /// Gets a list of the expected terminal names
            /// </summary>
            /// <returns>The list of expected terminal names</returns>
            public string[] GetExpectedNames()
            {
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
            /// Determines whether a reduction can occur for the given terminal ID
            /// </summary>
            /// <param name="sid">The terminal ID</param>
            /// <returns>True if a reduction is found, false otherwise</returns>
            public bool HasReductionOnTerminal(ushort sid) { return ReducsOnTerminal.ContainsKey(sid); }
            /// <summary>
            /// Get the reduction for the given terminal ID
            /// </summary>
            /// <param name="sid">The terminal ID</param>
            /// <returns>The reduction for the given terminal ID</returns>
            public Reduction GetReductionOnTerminal(ushort sid) { return ReducsOnTerminal[sid]; }
        }


        // Parser automata data
        /// <summary>
        /// Rules of the LR(1) parser
        /// </summary>
        protected Rule[] rules;
        /// <summary>
        /// States of the LR(1) parser automaton
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
        public BaseLR1Parser(ILexer input)
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

        protected void Analyse_HandleUnexpectedToken()
        {
            errors.Add(new ParserErrorUnexpectedToken(nextToken, states[currentState].GetExpectedNames()));
            if (errors.Count >= maxErrorCount)
                throw new ParserException("Too much errors, parsing stopped.");

            if (Analyse_HandleUnexpectedToken_SimpleRecovery()) return;
            throw new ParserException("Unrecoverable error encountered");
        }
        protected bool Analyse_HandleUnexpectedToken_SimpleRecovery()
        {
            if (Analyse_HandleUnexpectedToken_SimpleRecovery_RemoveUnexpected()) return true;
            if (Analyse_HandleUnexpectedToken_SimpleRecovery_InsertExpected()) return true;
            if (Analyse_HandleUnexpectedToken_SimpleRecovery_ReplaceUnexpectedByExpected()) return true;
            return false;
        }
        protected bool Analyse_HandleUnexpectedToken_SimpleRecovery_RemoveUnexpected()
        {
            ILexer TestLexer = lexer.Clone();
            List<ushort> TempStack = new List<ushort>(stack);
            TempStack.Reverse();
            Stack<ushort> TestStack = new Stack<ushort>(TempStack);
            if (Analyse_Simulate(TestStack, TestLexer))
            {
                nextToken = GetNextToken(lexer, currentState);
                return true;
            }
            return false;
        }
        protected bool Analyse_HandleUnexpectedToken_SimpleRecovery_InsertExpected()
        {
            for (int i = 0; i != states[currentState].Expected.Length; i++)
            {
                LexerText TestLexer = (LexerText)lexer.Clone();
                List<ushort> TempStack = new List<ushort>(stack);
                TempStack.Reverse();
                Stack<ushort> TestStack = new Stack<ushort>(TempStack);
                List<SymbolToken> Inserted = new List<SymbolToken>();
                Inserted.Add(new SymbolTokenText(states[currentState].Expected[i].Name, states[currentState].Expected[i].SymbolID, string.Empty, lexer.CurrentLine));
                Inserted.Add(nextToken);
                if (Analyse_Simulate(TestStack, TestLexer, Inserted))
                {
                    Analyse_RunForToken(Inserted[0]);
                    Analyse_RunForToken(Inserted[1]);
                    nextToken = GetNextToken(lexer, currentState);
                    return true;
                }
            }
            return false;
        }
        protected bool Analyse_HandleUnexpectedToken_SimpleRecovery_ReplaceUnexpectedByExpected()
        {
            for (int i = 0; i != states[currentState].Expected.Length; i++)
            {
                LexerText TestLexer = (LexerText)lexer.Clone();
                List<ushort> TempStack = new List<ushort>(stack);
                TempStack.Reverse();
                Stack<ushort> TestStack = new Stack<ushort>(TempStack);
                List<SymbolToken> Inserted = new List<SymbolToken>();
                Inserted.Add(new SymbolTokenText(states[currentState].Expected[i].Name, states[currentState].Expected[i].SymbolID, string.Empty, lexer.CurrentLine));
                if (Analyse_Simulate(TestStack, TestLexer, Inserted))
                {
                    Analyse_RunForToken(Inserted[0]);
                    nextToken = GetNextToken(lexer, currentState);
                    return true;
                }
            }
            return false;
        }

        protected bool Analyse_Simulate(Stack<ushort> stack, ILexer lexer, List<SymbolToken> inserted)
        {
            int InsertedIndex = 0;
            ushort CurrentState = stack.Peek();
            SymbolToken NextToken = null;
            if (inserted.Count != 0)
            {
                NextToken = inserted[0];
                InsertedIndex++;
            }
            else
                NextToken = GetNextToken(lexer, CurrentState);

            for (int i = 0; i != errorSimulationLength + inserted.Count; i++)
            {
                ushort NextState = states[CurrentState].GetNextByShiftOnTerminal(NextToken.SymbolID);
                if (NextState != 0xFFFF)
                {
                    CurrentState = NextState;
                    stack.Push(CurrentState);
                    if (InsertedIndex != inserted.Count)
                    {
                        NextToken = inserted[InsertedIndex];
                        InsertedIndex++;
                    }
                    else
                        NextToken = GetNextToken(lexer, CurrentState);
                    continue;
                }
                if (states[CurrentState].HasReductionOnTerminal(NextToken.SymbolID))
                {
                    Reduction reduction = states[CurrentState].GetReductionOnTerminal(NextToken.SymbolID);
                    Production Reduce = reduction.ToReduce.OnReduction;
                    ushort HeadID = reduction.ToReduce.Head.SymbolID;
                    for (ushort j = 0; j != reduction.ToReduce.Length; j++)
                        stack.Pop();
                    // If next symbol is e (after $) : return
                    if (NextToken.SymbolID == 0x1)
                        return true;
                    // Shift to next state on the reduce variable
                    NextState = states[stack.Peek()].GetNextByShiftOnVariable(HeadID);
                    // Handle error here : no transition for symbol HeadID
                    if (NextState == 0xFFFF)
                        return false;
                    CurrentState = NextState;
                    stack.Push(CurrentState);
                    continue;
                }
                // Handle error here : no action for symbol NextToken.SymbolID
                return false;
            }
            return true;
        }
        protected bool Analyse_Simulate(Stack<ushort> stack, ILexer lexer)
        {
            return Analyse_Simulate(stack, lexer, new List<SymbolToken>());
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
                if (states[currentState].HasReductionOnTerminal(token.SymbolID))
                {
                    Reduction reduction = states[currentState].GetReductionOnTerminal(token.SymbolID);
                    Production Reduce = reduction.ToReduce.OnReduction;
                    ushort HeadID = reduction.ToReduce.Head.SymbolID;
                    nodes.AddLast(Reduce(this));
                    for (ushort j = 0; j != reduction.ToReduce.Length; j++)
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
                    Analyse_HandleUnexpectedToken();
            }
        }
    }



    /// <summary>
    /// Base class for text-based LR(1) parsers
    /// </summary>
    public abstract class LR1TextParser : BaseLR1Parser
    {
        protected LR1TextParser(LexerText lexer) : base(lexer) { }
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <param name="lexer">Base lexer for reading tokens</param>
        /// <param name="state">Parser's current state</param>
        /// <returns>The next token in the input</returns>
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(); }
    }

    /// <summary>
    /// Base class for binary-based LR(1) parsers
    /// </summary>
    public abstract class LR1BinaryParser : BaseLR1Parser
    {
        protected LR1BinaryParser(LexerBinary lexer) : base(lexer) { }
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <param name="lexer">Base lexer for reading tokens</param>
        /// <param name="state">Parser's current state</param>
        /// <returns>The next token in the input</returns>
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(states[state].GetExpectedIDs()); }
    }
}
