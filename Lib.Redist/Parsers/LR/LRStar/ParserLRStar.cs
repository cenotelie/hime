/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public abstract class LRStarBaseParser : LRParser
    {
        protected LRStarState[] states;
        protected BufferedTokenReader reader;

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
        /// Gets the next token in the input
        /// </summary>
        /// <param name="lexer">Base lexer for reading tokens</param>
        /// <param name="state">Parser's current state</param>
        /// <returns>The next token in the input</returns>
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(); }

        public LRStarBaseParser(ILexer input) : base(input) { reader = new BufferedTokenReader(input); }

        protected ushort RunDecider(ushort first, out LRRule reduction)
        {
            int depth = 0;
            reduction = null;
            LRStarState lrState = states[state];
            DeciderState ds = lrState.decider[0];
            ushort token = first;
            if (!ds.transitions.ContainsKey(token)) // Unexpected token !
                return 0xFFFF;
            ds = lrState.decider[ds.transitions[token]];
            while (true)
            {
                if (ds.shift != 0xFFFF)
                {
                    reader.Rewind(depth);
                    return ds.shift;
                }
                if (ds.reduction.Head != null)
                {
                    reduction = ds.reduction;
                    reader.Rewind(depth);
                    return 0xFFFF;
                }
                // go to new state
                token = reader.Read().SymbolID;
                depth++;
                if (!ds.transitions.ContainsKey(token))
                {
                    // Unexpected token !
                    reader.Rewind(depth);
                    return 0xFFFF;
                }
                ds = lrState.decider[ds.transitions[token]];
            }
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
                LRRule reduction = null;
                ushort nextState = RunDecider(token.SymbolID, out reduction);
                if (nextState != 0xFFFF)
                {
                    nodes.AddLast(new SyntaxTreeNode(token));
                    state = nextState;
                    stack.Push(state);
                    return true;
                }
                if (reduction != null)
                {
                    Production Reduce = reduction.OnReduction;
                    ushort HeadID = reduction.Head.SymbolID;
                    nodes.AddLast(Reduce(this));
                    for (ushort j = 0; j != reduction.Length; j++)
                        stack.Pop();
                    // Shift to next state on the reduce variable
                    nextState = states[stack.Peek()].GetNextByShiftOnVariable(HeadID);
                    if (nextState == 0xFFFF)
                        return false;
                    state = nextState;
                    stack.Push(state);
                    continue;
                }
                return false;
            }
        }
    }
}
