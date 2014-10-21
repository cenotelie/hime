/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
	/// Represents a base for all context-free LR(k) parsers
	/// </summary>
	public abstract class LRkContextSensitiveParser : LRkParser
	{
		/// <summary>
		/// The parser's stack
		/// </summary>
		private LRkStack stack;

		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="automaton">The parser's automaton</param>
		/// <param name="variables">The parser's variables</param>
		/// <param name="virtuals">The parser's virtuals</param>
		/// <param name="actions">The parser's actions</param>
		/// <param name="lexer">The input lexer</param>
		protected LRkContextSensitiveParser(LRkAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, Lexer.ILexer lexer)
			: base(automaton, variables, virtuals, actions, lexer)
		{
		}

		/// <summary>
		/// Parses the input and returns the result
		/// </summary>
		/// <returns>A ParseResult object containing the data about the result</returns>
		public override ParseResult Parse()
		{
			stack = new LRkStack();
			stack.Push(0, automaton.GetContexts(0));
			Token nextToken = lexer.GetNextToken(stack);
			while (true)
			{
				LRActionCode action = ParseOnToken(nextToken);
				if (action == LRActionCode.Shift)
				{
					nextToken = lexer.GetNextToken(stack);
					continue;
				}
				if (action == LRActionCode.Accept)
					return new ParseResult(allErrors, lexer.Output, builder.GetTree());
				nextToken = OnUnexpectedToken(stack.HeadState, nextToken);
				if (nextToken.SymbolID == Symbol.SID_NOTHING || allErrors.Count >= MAX_ERROR_COUNT)
					return new ParseResult(allErrors, lexer.Output);
			}
		}

		/// <summary>
		/// Parses the given token
		/// </summary>
		/// <param name="token">The token to parse</param>
		/// <returns>The LR action on the token</returns>
		private LRActionCode ParseOnToken(Token token)
		{
			while (true)
			{
				LRAction action = automaton.GetAction(stack.HeadState, token.SymbolID);
				if (action.Code == LRActionCode.Shift)
				{
					stack.Push(action.Data, automaton.GetContexts(action.Data));
					builder.StackPushToken(token.Index);
					return action.Code;
				}
				else if (action.Code == LRActionCode.Reduce)
				{
					LRProduction production = automaton.GetProduction(action.Data);
					stack.Pop(production.ReductionLength);
					Reduce(production);
					action = automaton.GetAction(stack.HeadState, parserVariables[production.Head].ID);
					stack.Push(action.Data, automaton.GetContexts(action.Data));
					continue;
				}
				return action.Code;
			}
		}
	}
}
