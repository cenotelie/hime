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
	public abstract class LRkParser : BaseLRParser, Lexer.IContextProvider
	{
		/// <summary>
		/// The parser's automaton
		/// </summary>
		protected LRkAutomaton automaton;
		/// <summary>
		/// The parser's stack
		/// </summary>
		private int[] stack;
		/// <summary>
		/// Index of the stack's head
		/// </summary>
		private int head;
		/// <summary>
		/// The AST builder
		/// </summary>
		private LRkASTBuilder builder;

		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="automaton">The parser's automaton</param>
		/// <param name="variables">The parser's variables</param>
		/// <param name="virtuals">The parser's virtuals</param>
		/// <param name="actions">The parser's actions</param>
		/// <param name="lexer">The input lexer</param>
		protected LRkParser(LRkAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, Lexer.ILexer lexer)
            : base(variables, virtuals, actions, lexer)
		{
			this.automaton = automaton;
			this.builder = new LRkASTBuilder(MAX_STACK_SIZE, lexer.Output, parserVariables, parserVirtuals);
		}

		/// <summary>
		/// Gets whether the specified context is in effect
		/// </summary>
		/// <param name="context">A context</param>
		/// <returns><c>true</c> if the specified context is in effect</returns>
		public bool IsWithin(int context)
		{
			if (context == Lexer.Automaton.DEFAULT_CONTEXT)
				return true;
			for (int i = head; i != -1; i--)
				if (automaton.GetContexts(stack[i]).Contains(context))
					return true;
			return false;
		}

		/// <summary>
		/// Parses the input and returns the result
		/// </summary>
		/// <returns>A ParseResult object containing the data about the result</returns>
		public override ParseResult Parse()
		{
			this.stack = new int[MAX_STACK_SIZE];
			this.head = 0;
			Token nextToken = lexer.GetNextToken(this);
			while (true)
			{
				LRActionCode action = ParseOnToken(nextToken);
				if (action == LRActionCode.Shift)
				{
					nextToken = lexer.GetNextToken(this);
					continue;
				}
				if (action == LRActionCode.Accept)
					return new ParseResult(allErrors, lexer.Output, builder.GetTree());
				nextToken = OnUnexpectedToken(stack[head], nextToken);
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
				LRAction action = automaton.GetAction(stack[head], token.SymbolID);
				if (action.Code == LRActionCode.Shift)
				{
					stack[++head] = action.Data;
					builder.StackPushToken(token.Index);
					return action.Code;
				}
				else if (action.Code == LRActionCode.Reduce)
				{
					LRProduction production = automaton.GetProduction(action.Data);
					head -= production.ReductionLength;
					Reduce(production);
					action = automaton.GetAction(stack[head], parserVariables[production.Head].ID);
					stack[++head] = action.Data;
					continue;
				}
				return action.Code;
			}
		}

		/// <summary>
		/// Raises an error on an unexpected token
		/// </summary>
		/// <param name="state">The current LR state</param>
		/// <param name="token">The unexpected token</param>
		/// <returns>The next token in the case the error is recovered</returns>
		private Token OnUnexpectedToken(int state, Token token)
		{
			ICollection<int> expectedIndices = automaton.GetExpected(state, lexer.Terminals.Count);
			List<Symbol> expected = new List<Symbol>();
			foreach (int index in expectedIndices)
				expected.Add(lexer.Terminals[index]);
			allErrors.Add(new UnexpectedTokenError(lexer.Output[token.Index], lexer.Output.GetPositionOf(token.Index), expected));
			if (!recover)
				return new Token(Symbol.SID_NOTHING, 0);
			// TODO: try to recover from the error
			return new Token(Symbol.SID_NOTHING, 0);
		}

		/// <summary>
		/// Executes the given LR reduction
		/// </summary>
		/// <param name="production">A LR reduction</param>
		private void Reduce(LRProduction production)
		{
			Symbol variable = parserVariables[production.Head];
			builder.ReductionPrepare(production.Head, production.ReductionLength, production.HeadAction);
			for (int i = 0; i != production.BytecodeLength; i++)
			{
				LROpCode op = production[i];
				switch (op.Base)
				{
					case LROpCodeBase.SemanticAction:
						{
							SemanticAction action = parserActions[production[i + 1].DataValue];
							i++;
							action.Invoke(variable, builder);
							break;
						}
					case LROpCodeBase.AddVirtual:
						{
							int index = production[i + 1].DataValue;
							builder.ReductionAddVirtual(index, op.TreeAction);
							i++;
							break;
						}
					default:
						builder.ReductionPop(op.TreeAction);
						break;
				}
			}
			builder.Reduce();
		}
	}
}
