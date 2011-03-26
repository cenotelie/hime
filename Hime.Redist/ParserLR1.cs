using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public abstract class BaseLR1Parser : IParser
    {
        protected delegate void Production(BaseLR1Parser parser, List<SyntaxTreeNode> nodes);

        // Parser automata data
        protected Production[] rules;
        protected ushort[] rulesHeadID;
        protected string[] rulesHeadName;
        protected ushort[] rulesParserLength;
        protected ushort[][] stateExpectedIDs;
        protected string[][] stateExpectedNames;
        protected string[][] stateItems;
        protected ushort[][][] stateShiftsOnTerminal;
        protected ushort[][][] stateShiftsOnVariable;
        protected ushort[][][] stateReducsOnTerminal;
        protected int errorSimulationLength;

        // Parser state data
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
            errors = new List<ParserError>();
            lexer = input;
            nodes = new List<SyntaxTreeNode>();
            stack = new Stack<ushort>();
            currentState = 0x0;
            nextToken = null;
        }

        protected ushort Analyse_GetNextByShiftOnTerminal(ushort state, ushort sid)
        {
            for (int i = 0; i != stateShiftsOnTerminal[state].Length; i++)
            {
                if (stateShiftsOnTerminal[state][i][0] == sid)
                    return stateShiftsOnTerminal[state][i][1];
            }
            return 0xFFFF;
        }
        protected ushort Analyse_GetNextByShiftOnVariable(ushort state, ushort sid)
        {
            for (int i = 0; i != stateShiftsOnVariable[state].Length; i++)
            {
                if (stateShiftsOnVariable[state][i][0] == sid)
                    return stateShiftsOnVariable[state][i][1];
            }
            return 0xFFFF;
        }
        protected ushort Analyse_GetProductionOnTerminal(ushort state, ushort sid)
        {
            for (int i = 0; i != stateReducsOnTerminal[state].Length; i++)
            {
                if (stateReducsOnTerminal[state][i][0] == sid)
                    return stateReducsOnTerminal[state][i][1];
            }
            return 0xFFFF;
        }

        protected void Analyse_HandleUnexpectedToken()
        {
            errors.Add(new ParserErrorUnexpectedToken(nextToken, stateExpectedNames[currentState]));

            if (errors.Count >= 100)
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
            for (int i = 0; i != stateExpectedIDs[currentState].Length; i++)
            {
                LexerText TestLexer = (LexerText)lexer.Clone();
                List<ushort> TempStack = new List<ushort>(stack);
                TempStack.Reverse();
                Stack<ushort> TestStack = new Stack<ushort>(TempStack);
                List<SymbolToken> Inserted = new List<SymbolToken>();
                Inserted.Add(new SymbolTokenText(stateExpectedNames[currentState][i], stateExpectedIDs[currentState][i], string.Empty, lexer.CurrentLine));
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
            for (int i = 0; i != stateExpectedIDs[currentState].Length; i++)
            {
                LexerText TestLexer = (LexerText)lexer.Clone();
                List<ushort> TempStack = new List<ushort>(stack);
                TempStack.Reverse();
                Stack<ushort> TestStack = new Stack<ushort>(TempStack);
                List<SymbolToken> Inserted = new List<SymbolToken>();
                Inserted.Add(new SymbolTokenText(stateExpectedNames[currentState][i], stateExpectedIDs[currentState][i], string.Empty, lexer.CurrentLine));
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
                ushort NextState = Analyse_GetNextByShiftOnTerminal(CurrentState, NextToken.SymbolID);
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
                ushort ReductionIndex = Analyse_GetProductionOnTerminal(CurrentState, NextToken.SymbolID);
                if (ReductionIndex != 0xFFFF)
                {
                    Production Reduce = rules[ReductionIndex];
                    ushort HeadID = rulesHeadID[ReductionIndex];
                    for (ushort j = 0; j != rulesParserLength[ReductionIndex]; j++)
                        stack.Pop();
                    // If next symbol is ε (after $) : return
                    if (NextToken.SymbolID == 0x1)
                        return true;
                    // Shift to next state on the reduce variable
                    NextState = Analyse_GetNextByShiftOnVariable(stack.Peek(), HeadID);
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
                ushort NextState = Analyse_GetNextByShiftOnTerminal(currentState, token.SymbolID);
                if (NextState != 0xFFFF)
                {
                    nodes.Add(new SyntaxTreeNode(token));
                    currentState = NextState;
                    stack.Push(currentState);
                    return true;
                }
                ushort ReductionIndex = Analyse_GetProductionOnTerminal(currentState, token.SymbolID);
                if (ReductionIndex != 0xFFFF)
                {
                    Production Reduce = rules[ReductionIndex];
                    ushort HeadID = rulesHeadID[ReductionIndex];
                    Reduce(this, nodes);
                    for (ushort j = 0; j != rulesParserLength[ReductionIndex]; j++)
                        stack.Pop();
                    // Shift to next state on the reduce variable
                    NextState = Analyse_GetNextByShiftOnVariable(stack.Peek(), HeadID);
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
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(stateExpectedIDs[state]); }
    }
}
