using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public abstract class BaseLR1Parser : IParser
    {
        protected delegate void Production(BaseLR1Parser parser, List<SyntaxTreeNode> nodes);
        protected struct Rule
        {
            public Production OnReduction;
            public SymbolVariable Head;
            public ushort Length;
            public Rule(Production prod, SymbolVariable head, ushort length)
            {
                OnReduction = prod;
                Head = head;
                Length = length;
            }
        }
        protected struct Reduction
        {
            public ushort Lookahead;
            public Rule ToReduce;
            public Reduction(ushort lookahead, Rule rule)
            {
                Lookahead = lookahead;
                ToReduce = rule;
            }
        }
        protected struct State
        {
            public string[] Items;
            public SymbolTerminal[] Expected;
            public Dictionary<ushort, ushort> ShiftsOnTerminal;
            public Dictionary<ushort, ushort> ShiftsOnVariable;
            public Dictionary<ushort, Reduction> ReducsOnTerminal;
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
            public ushort[] GetExpectedIDs()
            {
                ushort[] results = new ushort[Expected.Length];
                for (int i = 0; i != Expected.Length; i++)
                    results[i] = Expected[i].SymbolID;
                return results;
            }
            public string[] GetExpectedNames()
            {
                string[] results = new string[Expected.Length];
                for (int i = 0; i != Expected.Length; i++)
                    results[i] = Expected[i].Name;
                return results;
            }
            public ushort GetNextByShiftOnTerminal(ushort sid)
            {
                if (!ShiftsOnTerminal.ContainsKey(sid))
                    return 0xFFFF;
                return ShiftsOnTerminal[sid];
            }
            public ushort GetNextByShiftOnVariable(ushort sid)
            {
                if (!ShiftsOnVariable.ContainsKey(sid))
                    return 0xFFFF;
                return ShiftsOnVariable[sid];
            }
            public bool HasReductionOnTerminal(ushort sid) { return ReducsOnTerminal.ContainsKey(sid); }
            public Reduction GetReductionOnTerminal(ushort sid) { return ReducsOnTerminal[sid]; }
        }


        // Parser automata data
        protected Rule[] rules;
        protected State[] states;
        protected int errorSimulationLength;

        // Parser state data
        protected int maxErrorCount;
        protected List<ParserError> errors;
        protected ILexer lexer;
        protected List<SyntaxTreeNode> nodes;
        protected Stack<ushort> stack;
        protected SymbolToken nextToken;
        protected ushort currentState;

        public System.Collections.ObjectModel.ReadOnlyCollection<ParserError> Errors { get { return new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(errors); } }

        protected abstract void setup();
        protected abstract SymbolToken GetNextToken(ILexer lexer, ushort state);

        public BaseLR1Parser(ILexer input)
        {
            setup();
            maxErrorCount = 100;
            errors = new List<ParserError>();
            lexer = input;
            nodes = new List<SyntaxTreeNode>();
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
                    nodes.Add(new SyntaxTreeNode(token));
                    currentState = NextState;
                    stack.Push(currentState);
                    return true;
                }
                if (states[currentState].HasReductionOnTerminal(token.SymbolID))
                {
                    Reduction reduction = states[currentState].GetReductionOnTerminal(token.SymbolID);
                    Production Reduce = reduction.ToReduce.OnReduction;
                    ushort HeadID = reduction.ToReduce.Head.SymbolID;
                    Reduce(this, nodes);
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
                    return nodes[0].ApplyActions();
                else
                    Analyse_HandleUnexpectedToken();
            }
        }
    }




    public abstract class LR1TextParser : BaseLR1Parser
    {
        protected LR1TextParser(LexerText lexer) : base(lexer) { }
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(); }
    }

    public abstract class LR1BinaryParser : BaseLR1Parser
    {
        protected LR1BinaryParser(LexerBinary lexer) : base(lexer) { }
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(states[state].GetExpectedIDs()); }
    }
}
