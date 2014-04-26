/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

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
        protected SymbolDictionary parserVariables;
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
        public bool TestForLength(int length, Token inserted, out int advance)
        {
            int remaining = length;
            Token nextToken = (inserted.SymbolID != 0) ? inserted : input.GetNextToken();
            advance = (inserted.SymbolID != 0) ? 1 : 0;
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

        private LRActionCode RecognizeOnToken(Token token)
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
                    Symbol var = parserVariables[production.Head];
                    head -= production.ReductionLength;
                    action = parserAutomaton.GetAction(stack[head], var.ID);
                    stack[++head] = action.Data;
                    continue;
                }
                return action.Code;
            }
        }
    }
}
