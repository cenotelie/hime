namespace Hime.Redist.Parsers
{
    public class ParserException : System.Exception
    {
        public ParserException() : base() { }
        public ParserException(string message) : base(message) { }
        public ParserException(string message, System.Exception innerException) : base(message, innerException) { }
        protected ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public interface ParserError
    {
        string Message { get; }
    }
    public class ParserErrorUnexpectedToken : ParserError
    {
        private SymbolToken p_Token;
        private System.Collections.ObjectModel.Collection<string> p_Expected;
        private System.Collections.ObjectModel.ReadOnlyCollection<string> p_ReadOnlyExpected;
        private string p_Message;

        public SymbolToken UnexpectedToken { get { return p_Token; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<string> ExpectedTokens { get { return p_ReadOnlyExpected; } }
        public string Message { get { return p_Message; } }

        public ParserErrorUnexpectedToken(SymbolToken Token, string[] Expected)
        {
            p_Token = Token;
            p_Expected = new System.Collections.ObjectModel.Collection<string>(Expected);
            p_ReadOnlyExpected = new System.Collections.ObjectModel.ReadOnlyCollection<string>(p_Expected);
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unexpected token ");
            Builder.Append(p_Token.Value.ToString());
            Builder.Append(", expected : { ");
            for (int i = 0; i != p_Expected.Count; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(p_Expected[i]);
            }
            Builder.Append(" }.");
            p_Message = Builder.ToString();
        }
        public override string ToString() { return "Parser Error : unexpected token"; }
    }



    public interface IParser
    {
        System.Collections.Generic.List<ParserError> Errors { get; }
        SyntaxTreeNode Analyse();
    }

    public abstract class BaseLR1Parser
    {
        protected delegate void Production(BaseLR1Parser parser, SyntaxTreeNodeCollection nodes);
        
        // Parser automata data
        protected Production[] p_Rules;
        protected ushort[] p_RulesHeadID;
        protected string[] p_RulesHeadName;
        protected ushort[] p_RulesParserLength;
        protected ushort[][] p_StateExpectedIDs;
        protected string[][] p_StateExpectedNames;
        protected string[][] p_StateItems;
        protected ushort[][][] p_StateShiftsOnTerminal;
        protected ushort[][][] p_StateShiftsOnVariable;
        protected ushort[][][] p_StateReducsOnTerminal;
        protected int p_ErrorSimulationLength;
        
        // Parser state data
        protected System.Collections.Generic.List<ParserError> p_Errors;
        protected ILexer p_Lexer;
        protected SyntaxTreeNodeCollection p_Nodes;
        protected System.Collections.Generic.Stack<ushort> p_Stack;
        protected SymbolToken p_NextToken;
        protected ushort p_CurrentState;

        public System.Collections.Generic.List<ParserError> Errors { get { return p_Errors; } }

        protected abstract void setup();
        protected abstract SymbolToken GetNextToken(ILexer lexer, ushort state);

        public BaseLR1Parser(ILexer input)
        {
            setup();
            p_Errors = new System.Collections.Generic.List<ParserError>();
            p_Lexer = input;
            p_Nodes = new SyntaxTreeNodeCollection();
            p_Stack = new System.Collections.Generic.Stack<ushort>();
            p_CurrentState = 0x0;
            p_NextToken = null;
        }

        protected ushort Analyse_GetNextByShiftOnTerminal(ushort state, ushort sid)
        {
            for (int i = 0; i != p_StateShiftsOnTerminal[state].Length; i++)
            {
                if (p_StateShiftsOnTerminal[state][i][0] == sid)
                    return p_StateShiftsOnTerminal[state][i][1];
            }
            return 0xFFFF;
        }
        protected ushort Analyse_GetNextByShiftOnVariable(ushort state, ushort sid)
        {
            for (int i = 0; i != p_StateShiftsOnVariable[state].Length; i++)
            {
                if (p_StateShiftsOnVariable[state][i][0] == sid)
                    return p_StateShiftsOnVariable[state][i][1];
            }
            return 0xFFFF;
        }
        protected ushort Analyse_GetProductionOnTerminal(ushort state, ushort sid)
        {
            for (int i = 0; i != p_StateReducsOnTerminal[state].Length; i++)
            {
                if (p_StateReducsOnTerminal[state][i][0] == sid)
                    return p_StateReducsOnTerminal[state][i][1];
            }
            return 0xFFFF;
        }

        protected void Analyse_HandleUnexpectedToken()
        {
            p_Errors.Add(new ParserErrorUnexpectedToken(p_NextToken, p_StateExpectedNames[p_CurrentState]));

            if (p_Errors.Count >= 100)
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
            ILexer TestLexer = p_Lexer.Clone();
            System.Collections.Generic.List<ushort> TempStack = new System.Collections.Generic.List<ushort>(p_Stack);
            TempStack.Reverse();
            System.Collections.Generic.Stack<ushort> TestStack = new System.Collections.Generic.Stack<ushort>(TempStack);
            if (Analyse_Simulate(TestStack, TestLexer))
            {
                p_NextToken = GetNextToken(p_Lexer, p_CurrentState);
                return true;
            }
            return false;
        }
        protected bool Analyse_HandleUnexpectedToken_SimpleRecovery_InsertExpected()
        {
            for (int i = 0; i != p_StateExpectedIDs[p_CurrentState].Length; i++)
            {
                LexerText TestLexer = (LexerText)p_Lexer.Clone();
                System.Collections.Generic.List<ushort> TempStack = new System.Collections.Generic.List<ushort>(p_Stack);
                TempStack.Reverse();
                System.Collections.Generic.Stack<ushort> TestStack = new System.Collections.Generic.Stack<ushort>(TempStack);
                System.Collections.Generic.List<SymbolToken> Inserted = new System.Collections.Generic.List<SymbolToken>();
                Inserted.Add(new SymbolTokenText(p_StateExpectedNames[p_CurrentState][i], p_StateExpectedIDs[p_CurrentState][i], string.Empty, p_Lexer.CurrentLine));
                Inserted.Add(p_NextToken);
                if (Analyse_Simulate(TestStack, TestLexer, Inserted))
                {
                    Analyse_RunForToken(Inserted[0]);
                    Analyse_RunForToken(Inserted[1]);
                    p_NextToken = GetNextToken(p_Lexer, p_CurrentState);
                    return true;
                }
            }
            return false;
        }
        protected bool Analyse_HandleUnexpectedToken_SimpleRecovery_ReplaceUnexpectedByExpected()
        {
            for (int i = 0; i != p_StateExpectedIDs[p_CurrentState].Length; i++)
            {
                LexerText TestLexer = (LexerText)p_Lexer.Clone();
                System.Collections.Generic.List<ushort> TempStack = new System.Collections.Generic.List<ushort>(p_Stack);
                TempStack.Reverse();
                System.Collections.Generic.Stack<ushort> TestStack = new System.Collections.Generic.Stack<ushort>(TempStack);
                System.Collections.Generic.List<SymbolToken> Inserted = new System.Collections.Generic.List<SymbolToken>();
                Inserted.Add(new SymbolTokenText(p_StateExpectedNames[p_CurrentState][i], p_StateExpectedIDs[p_CurrentState][i], string.Empty, p_Lexer.CurrentLine));
                if (Analyse_Simulate(TestStack, TestLexer, Inserted))
                {
                    Analyse_RunForToken(Inserted[0]);
                    p_NextToken = GetNextToken(p_Lexer, p_CurrentState);
                    return true;
                }
            }
            return false;
        }

        protected bool Analyse_Simulate(System.Collections.Generic.Stack<ushort> stack, ILexer lexer, System.Collections.Generic.List<SymbolToken> inserted)
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

            for (int i = 0; i != p_ErrorSimulationLength + inserted.Count; i++)
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
                    Production Reduce = p_Rules[ReductionIndex];
                    ushort HeadID = p_RulesHeadID[ReductionIndex];
                    for (ushort j = 0; j != p_RulesParserLength[ReductionIndex]; j++)
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
        protected bool Analyse_Simulate(System.Collections.Generic.Stack<ushort> stack, ILexer lexer)
        {
            return Analyse_Simulate(stack, lexer, new System.Collections.Generic.List<SymbolToken>());
        }

        protected bool Analyse_RunForToken(SymbolToken token)
        {
            while (true)
            {
                ushort NextState = Analyse_GetNextByShiftOnTerminal(p_CurrentState, token.SymbolID);
                if (NextState != 0xFFFF)
                {
                    p_Nodes.Add(new SyntaxTreeNode(token));
                    p_CurrentState = NextState;
                    p_Stack.Push(p_CurrentState);
                    return true;
                }
                ushort ReductionIndex = Analyse_GetProductionOnTerminal(p_CurrentState, token.SymbolID);
                if (ReductionIndex != 0xFFFF)
                {
                    Production Reduce = p_Rules[ReductionIndex];
                    ushort HeadID = p_RulesHeadID[ReductionIndex];
                    Reduce(this, p_Nodes);
                    for (ushort j = 0; j != p_RulesParserLength[ReductionIndex]; j++)
                        p_Stack.Pop();
                    // Shift to next state on the reduce variable
                    NextState = Analyse_GetNextByShiftOnVariable(p_Stack.Peek(), HeadID);
                    if (NextState == 0xFFFF)
                        return false;
                    p_CurrentState = NextState;
                    p_Stack.Push(p_CurrentState);
                    continue;
                }
                return false;
            }
        }

        public SyntaxTreeNode Analyse()
        {
            p_Stack.Push(p_CurrentState);
            p_NextToken = GetNextToken(p_Lexer, p_CurrentState);

            while (true)
            {
                if (Analyse_RunForToken(p_NextToken))
                {
                    p_NextToken = GetNextToken(p_Lexer, p_CurrentState);
                    continue;
                }
                else if (p_NextToken.SymbolID == 0x0001)
                    return p_Nodes[0];
                else
                    Analyse_HandleUnexpectedToken();
            }
        }
    }




    public abstract class LR1TextParser : BaseLR1Parser
    {
        protected LR1TextParser(LexerText lexer) : base (lexer) { }
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(); }
    }

    public abstract class LR1BinaryParser : BaseLR1Parser
    {
        protected LR1BinaryParser(LexerBinary lexer) : base(lexer) { }
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(p_StateExpectedIDs[state]); }
    }
}