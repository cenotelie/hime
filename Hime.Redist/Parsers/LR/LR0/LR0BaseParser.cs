using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base for LR(0) parsers
    /// </summary>
    public abstract class LR0BaseParser : LRParser
    {
        /// <summary>
        /// States of the LR(0) parser automaton
        /// </summary>
        protected LR0State[] states;

        /// <summary>
        /// Gets the automaton's state with the given id
        /// </summary>
        /// <param name="id">State's id</param>
        /// <returns>The automaton's state which has the given id, or null if no state with the given id is found</returns>
        protected override LRState GetState(int id) { return states[id]; }

        /// <summary>
        /// Acts when an unexpected token is encountered
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>The new next token if the error is resolved, null otherwise</returns>
        protected override SymbolToken OnUnexpectedToken(SymbolToken nextToken) { return null; }

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        protected override bool RunForToken(SymbolToken token)
        {
            while (true)
            {
                if (states[state].HasReduction())
                {
                    LRRule rule = states[state].reduction;
                    Production Reduce = rule.OnReduction;
                    ushort HeadID = rule.Head.SymbolID;
                    nodes.AddLast(Reduce(this));
                    for (ushort j = 0; j != rule.Length; j++)
                        stack.Pop();
                    // Shift to next state on the reduce variable
                    ushort nextState = states[stack.Peek()].GetNextByShiftOnVariable(HeadID);
                    if (nextState == 0xFFFF)
                        return false;
                    state = nextState;
                    stack.Push(state);
                    continue;
                }
                else
                {
                    ushort nextState = states[state].GetNextByShiftOnTerminal(token.SymbolID);
                    if (nextState != 0xFFFF)
                    {
                        nodes.AddLast(new SyntaxTreeNode(token));
                        state = nextState;
                        stack.Push(state);
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the LR0BaseParser class with the given lexer
        /// </summary>
        /// <param name="input">Input lexer</param>
        public LR0BaseParser(ILexer input) :base(input) { }
    }
}
