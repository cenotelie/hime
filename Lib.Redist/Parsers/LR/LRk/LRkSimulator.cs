/*
 * Author: Laurent Wouters
 * Date: 26/10/2012
 * Time: 10:15
 * 
 */
using System.IO;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base simulator for all LR(k) parsers
    /// </summary>
    public abstract class LRkSimulator
    {
        /// <summary>
        /// Parser's variables
        /// </summary>
        protected Utils.SymbolDictionary<SymbolVariable> parserVariables;
        /// <summary>
        /// LR(k) parsing table and productions
        /// </summary>
        protected LRkAutomaton parserAutomaton;
        /// <summary>
        /// Parser's input encapsulating the lexer
        /// </summary>
        protected RewindableTokenStream input;
        /// <summary>
        /// Parser's stack
        /// </summary>
        protected ushort[] stack;
        /// <summary>
        /// Current stack's head
        /// </summary>
        protected int head;

        /// <summary>
        /// Tests the given input against the parser
        /// </summary>
        /// <param name="length">Length to test</param>
        /// <param name="inserted">Token to insert, or null if none should be inserted</param>
        /// <param name="advance">Returns the number of token used from the input</param>
        /// <returns>True of the parser matches the input, false otherwise</returns>
        public bool TestForLength(int length, SymbolToken inserted, out int advance)
        {
            int remaining = length;
            SymbolToken nextToken = (inserted != null) ? inserted : input.GetNextToken();
            advance = (inserted == null) ? 1 : 0;
            while (true)
            {
                int action = RecognizeOnToken(nextToken);
                if (action == LRkAutomaton.ActionShift)
                {
                    remaining--;
                    if (remaining == 0) return true;
                    nextToken = input.GetNextToken();
                    advance++;
                    continue;
                }
                if (action == LRkAutomaton.ActionAccept)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        private int RecognizeOnToken(SymbolToken token)
        {
            while (true)
            {
                ushort action = 0;
                ushort data = parserAutomaton.GetAction(stack[head], token.SymbolID, out action);
                if (action == LRkAutomaton.ActionShift)
                {
                    stack[++head] = data;
                    return action;
                }
                else if (action == LRkAutomaton.ActionReduce)
                {
                    LRProduction production = parserAutomaton.GetProduction(data);
                    SymbolVariable var = parserVariables[production.Head];
                    head -= production.ReductionLength;
                    data = parserAutomaton.GetAction(stack[head], var.SymbolID, out action);
                    stack[++head] = data;
                    continue;
                }
                return action;
            }
        }
    }
}
