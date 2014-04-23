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
    /// Represents a base for all LR(k) parsers
    /// </summary>
    public abstract class LRkParser : BaseLRParser
    {
        private class Simulator : LRkSimulator
        {
            public Simulator(LRkParser parser)
            {
                this.parserAutomaton = parser.parserAutomaton;
                this.parserVariables = parser.parserVariables;
                this.input = parser.input;
                this.stack = new int[maxStackSize];
                this.head = parser.head;
                System.Array.Copy(parser.stack, this.stack, parser.head + 1);
            }
        }

        private LRkAutomaton parserAutomaton;
        private RewindableTokenStream input;
        private LRkASTBuilder builder;
        private int[] stack;
        private int head;

        /// <summary>
        /// Initializes a new instance of the LRkParser class with the given lexer
        /// </summary>
        /// <param name="automaton">The parser's automaton</param>
        /// <param name="variables">The parser's variables</param>
        /// <param name="virtuals">The parser's virtuals</param>
        /// <param name="actions">The parser's actions</param>
        /// <param name="lexer">The input lexer</param>
        protected LRkParser(LRkAutomaton automaton, Symbol[] variables, Symbol[] virtuals, UserAction[] actions, Lexer.Lexer lexer)
            : base(variables, virtuals, actions, lexer)
        {
            this.parserAutomaton = automaton;
            this.input = new RewindableTokenStream(lexer);
            this.builder = new LRkASTBuilder(maxStackSize, lexer.Output, parserVariables, parserVirtuals);
        }

        private Token OnUnexpectedToken(Token token)
        {
            ICollection<int> expectedIDs = parserAutomaton.GetExpected(stack[head], lexer.Terminals.Count);
            List<Symbol> expected = new List<Symbol>();
            foreach (int index in expectedIDs)
                expected.Add(lexer.Terminals[index]);
            allErrors.Add(new UnexpectedTokenError(token, expected, lexer.Output));
            if (!recover) return new Token(0, 0);
            if (TryDrop1Unexpected()) return input.GetNextToken();
            if (TryDrop2Unexpected()) return input.GetNextToken();
            foreach (Symbol terminal in expected)
            {
                Token dummy = new Token(terminal.ID, 0);
                if (TryInsertExpected(dummy))
                    return dummy;
            }
            return new Token(0, 0);
        }
        private bool TryDrop1Unexpected()
        {
            int used = 0;
            bool success = (new Simulator(this)).TestForLength(3, new Token(0, 0), out used);
            input.Rewind(used);
            return success;
        }
        private bool TryDrop2Unexpected()
        {
            input.GetNextToken();
            int used = 0;
            bool success = (new Simulator(this)).TestForLength(3, new Token(0, 0), out used);
            input.Rewind(used);
            if (!success)
                input.Rewind(1);
            return success;
        }
        private bool TryInsertExpected(Token terminal)
        {
            int used = 0;
            bool success = (new Simulator(this)).TestForLength(3, terminal, out used);
            input.Rewind(used);
            return success;
        }

        /// <summary>
        /// Parses the input and returns the result
        /// </summary>
        /// <returns>A ParseResult object containing the data about the result</returns>
        public override ParseResult Parse()
        {
            this.stack = new int[maxStackSize];
            Token nextToken = input.GetNextToken();
            while (true)
            {
                LRActionCode action = ParseOnToken(nextToken);
                if (action == LRActionCode.Shift)
                {
                    nextToken = input.GetNextToken();
                    continue;
                }
                if (action == LRActionCode.Accept)
                    return new ParseResult(allErrors, lexer.Output, builder.GetTree());
                nextToken = OnUnexpectedToken(nextToken);
                if (nextToken.SymbolID == 0 || allErrors.Count >= maxErrorCount)
                    return new ParseResult(allErrors, lexer.Output);
            }
        }

        private LRActionCode ParseOnToken(Token token)
        {
            while (true)
            {
                LRAction action = parserAutomaton.GetAction(stack[head], token.SymbolID);
                if (action.Code == LRActionCode.Shift)
                {
                    stack[++head] = action.Data;
                    builder.StackPushToken(token.Index);
                    return action.Code;
                }
                else if (action.Code == LRActionCode.Reduce)
                {
                    LRProduction production = parserAutomaton.GetProduction(action.Data);
                    head -= production.ReductionLength;
                    Reduce(production);
                    action = parserAutomaton.GetAction(stack[head], parserVariables[production.Head].ID);
                    stack[++head] = action.Data;
                    continue;
                }
                return action.Code;
            }
        }

        private void Reduce(LRProduction production)
        {
            Symbol variable = parserVariables[production.Head];
            builder.ReductionPrepare(production.Head, production.ReductionLength, production.HeadAction);
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                LROpCode op = production.Bytecode[i];
                if (op.IsSemAction)
                {
                    builder.ReductionSemantic(parserActions[production.Bytecode[i + 1].Value]);
                    i++;
                }
                else if (op.IsAddVirtual)
                {
                    int index = production.Bytecode[i + 1].Value;
                    Symbol virt = parserVirtuals[index];
                    builder.ReductionVirtual(index, op.TreeAction);
                    i++;
                }
                else
                {
                    builder.ReductionPop(op.TreeAction);
                }
            }
            builder.Reduce();
        }
    }
}
