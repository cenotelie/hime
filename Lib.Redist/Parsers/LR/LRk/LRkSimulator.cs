using System.IO;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base simulator for all LR(k) parsers
    /// </summary>
    abstract class LRkSimulator
    {
        /// <summary>
        /// Parser's variables
        /// </summary>
        protected Utils.SymbolDictionary<Symbols.Variable> parserVariables;
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
        protected int[] stack;
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
        public bool TestForLength(int length, Symbols.Token inserted, out int advance)
        {
            int remaining = length;
            Symbols.Token nextToken = (inserted != null) ? inserted : input.GetNextToken();
            advance = (inserted == null) ? 1 : 0;
            while (true)
            {
                LRActionCode action = RecognizeOnToken(nextToken);
                if (action == LRActionCode.Shift)
                {
                    remaining--;
                    if (remaining == 0) return true;
                    nextToken = input.GetNextToken();
                    advance++;
                    continue;
                }
                if (action == LRActionCode.Accept)
                    return true;
                return false;
            }
        }

        private LRActionCode RecognizeOnToken(Symbols.Token token)
        {
            while (true)
            {
                LRAction action = parserAutomaton.GetAction(stack[head], token.SymbolID);
                if (action.Code == LRActionCode.Shift)
                {
                    stack[++head] = action.Data;
                    return action.Code;
                }
                else if (action.Code == LRActionCode.Reduce)
                {
                    LRProduction production = parserAutomaton.GetProduction(action.Data);
                    Symbols.Variable var = parserVariables[production.Head];
                    head -= production.ReductionLength;
                    action = parserAutomaton.GetAction(stack[head], var.SymbolID);
                    stack[++head] = action.Data;
                    continue;
                }
                return action.Code;
            }
        }
    }
}
