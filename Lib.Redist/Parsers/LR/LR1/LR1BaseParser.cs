/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base for LR(1) parsers
    /// </summary>
    public abstract class LR1BaseParser : LRParser
    {
        /// <summary>
        /// States of the LR(1) parser automaton
        /// </summary>
        protected LR1State[] states;

        /// <summary>
        /// Gets the automaton's state with the given id
        /// </summary>
        /// <param name="id">State's id</param>
        /// <returns>The automaton's state which has the given id, or null if no state with the given id is found</returns>
        protected override LRState GetState(int id) { return states[id]; }

        /// <summary>
        /// Initializes a new instance of the LR1BaseParser class with the given lexer
        /// </summary>
        /// <param name="input">Input lexer</param>
        public LR1BaseParser(ILexer input) : base(input) { }

		/// <summary>
        /// Acts when an unexpected token is encountered
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>The new next token if the error is resolved, null otherwise</returns>
        protected override SymbolToken OnUnexpectedToken(SymbolToken nextToken)
        {
            SymbolToken token = SimpleRecovery_RemoveUnexpected();
            if (token != null) return token;
            token = SimpleRecovery_InsertExpected(nextToken);
            if (token != null) return token;
            return SimpleRecovery_ReplaceUnexpectedByExpected();
        }

        private SymbolToken SimpleRecovery_RemoveUnexpected()
        {
            ILexer TestLexer = lexer.Clone();
            List<ushort> TempStack = new List<ushort>(stack);
            TempStack.Reverse();
            Stack<ushort> TestStack = new Stack<ushort>(TempStack);
            if (Simulate(TestStack, TestLexer))
            {
                return GetNextToken(lexer, state);
            }
            return null;
        }
		
        private SymbolToken SimpleRecovery_InsertExpected(SymbolToken nextToken)
        {
            for (int i = 0; i != states[state].expecteds.Length; i++)
            {
                LexerText TestLexer = (LexerText)lexer.Clone();
                List<ushort> TempStack = new List<ushort>(stack);
                TempStack.Reverse();
                Stack<ushort> TestStack = new Stack<ushort>(TempStack);
                List<SymbolToken> Inserted = new List<SymbolToken>();
                Inserted.Add(new SymbolTokenText(states[state].expecteds[i].SymbolID, states[state].expecteds[i].Name, string.Empty, lexer.CurrentLine));
                Inserted.Add(nextToken);
                if (Simulate(TestStack, TestLexer, Inserted))
                {
                    RunForToken(Inserted[0]);
                    RunForToken(Inserted[1]);
                    return GetNextToken(lexer, state);
                }
            }
            return null;
        }
		
        private SymbolToken SimpleRecovery_ReplaceUnexpectedByExpected()
        {
            for (int i = 0; i != states[state].expecteds.Length; i++)
            {
                LexerText TestLexer = (LexerText)lexer.Clone();
                List<ushort> TempStack = new List<ushort>(stack);
                TempStack.Reverse();
                Stack<ushort> TestStack = new Stack<ushort>(TempStack);
                List<SymbolToken> Inserted = new List<SymbolToken>();
                Inserted.Add(new SymbolTokenText(states[state].expecteds[i].SymbolID, states[state].expecteds[i].Name, string.Empty, lexer.CurrentLine));
                if (Simulate(TestStack, TestLexer, Inserted))
                {
                    RunForToken(Inserted[0]);
                    return GetNextToken(lexer, state);
                }
            }
            return null;
        }

        private bool Simulate(Stack<ushort> stack, ILexer lexer, List<SymbolToken> inserted)
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
                    LRReduction reduction = states[CurrentState].GetReductionOnTerminal(NextToken.SymbolID);
                    LRRule rule = reduction.toReduce;
                    ushort HeadID = rule.Head.SymbolID;
                    for (ushort j = 0; j != rule.Length; j++) stack.Pop();
                    // If next symbol is e (after $) : return
                    if (NextToken.SymbolID == 0x1) return true;
                    // Shift to next state on the reduce variable
                    NextState = states[stack.Peek()].GetNextByShiftOnVariable(HeadID);
                    // Handle error here : no transition for symbol HeadID
                    if (NextState == 0xFFFF) return false;
                    CurrentState = NextState;
                    stack.Push(CurrentState);
                    continue;
                }
                // Handle error here : no action for symbol NextToken.SymbolID
                return false;
            }
            return true;
        }
		
        private bool Simulate(Stack<ushort> stack, ILexer lexer)
        {
            return Simulate(stack, lexer, new List<SymbolToken>());
        }

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        protected override bool RunForToken(SymbolToken token)
        {
            while (true)
            {
                ushort NextState = states[state].GetNextByShiftOnTerminal(token.SymbolID);
                if (NextState != 0xFFFF)
                {
                    nodes.AddLast(new SyntaxTreeNode(token));
                    state = NextState;
                    stack.Push(state);
                    return true;
                }
                if (states[state].HasReductionOnTerminal(token.SymbolID))
                {
                    LRReduction reduction = states[state].GetReductionOnTerminal(token.SymbolID);
                    Production Reduce = reduction.toReduce.OnReduction;
                    ushort HeadID = reduction.toReduce.Head.SymbolID;
                    nodes.AddLast(Reduce(this));
                    for (ushort j = 0; j != reduction.toReduce.Length; j++)
                        stack.Pop();
                    // Shift to next state on the reduce variable
                    NextState = states[stack.Peek()].GetNextByShiftOnVariable(HeadID);
                    if (NextState == 0xFFFF)
                        return false;
                    state = NextState;
                    stack.Push(state);
                    continue;
                }
                return false;
            }
        }
    }
}
