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
		/// <summary>
		/// The simulator for LR(k) parsers used for error recovery
		/// </summary>
		private class Simulator : LRkSimulator
		{
			/// <summary>
			/// Initializes a new simulator based on the given LR(k) parser
			/// </summary>
			/// <param name="parser">The base LR(k) parser</param>
			public Simulator(LRkParser parser)
			{
				this.parserAutomaton = parser.parserAutomaton;
				this.parserVariables = parser.parserVariables;
				this.input = parser.input;
				this.stack = new int[MAX_STACK_SIZE];
				this.head = parser.head;
				System.Array.Copy(parser.stack, this.stack, parser.head + 1);
			}
		}

		/// <summary>
		/// The parser's automaton
		/// </summary>
		private LRkAutomaton parserAutomaton;

		/// <summary>
		/// The parser's input as a stream of tokens
		/// </summary>
		private RewindableTokenStream input;

		/// <summary>
		/// The AST builder
		/// </summary>
		private LRkASTBuilder builder;

		/// <summary>
		/// The parser's stack
		/// </summary>
		private int[] stack;

		/// <summary>
		/// Index of the stack's head
		/// </summary>
		private int head;

		/// <summary>
		/// Initializes a new instance of the LRkParser class with the given lexer
		/// </summary>
		/// <param name="automaton">The parser's automaton</param>
		/// <param name="variables">The parser's variables</param>
		/// <param name="virtuals">The parser's virtuals</param>
		/// <param name="actions">The parser's actions</param>
		/// <param name="lexer">The input lexer</param>
		protected LRkParser(LRkAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, Lexer.ILexer lexer)
            : base(variables, virtuals, actions, lexer)
		{
			this.parserAutomaton = automaton;
			this.input = new RewindableTokenStream(lexer);
			this.builder = new LRkASTBuilder(MAX_STACK_SIZE, lexer.Output, parserVariables, parserVirtuals);
		}

		/// <summary>
		/// Raises an error on an unexepcted token
		/// </summary>
		/// <param name="token">The unexpected token</param>
		/// <returns>The next token in the case the error is recovered</returns>
		private Token OnUnexpectedToken(Token token)
		{
			ICollection<int> expectedIDs = parserAutomaton.GetExpected(stack[head], lexer.Terminals.Count);
			List<Symbol> expected = new List<Symbol>();
			foreach (int index in expectedIDs)
				expected.Add(lexer.Terminals[index]);
			allErrors.Add(new UnexpectedTokenError(lexer.Output[token.Index], lexer.Output.GetPositionOf(token.Index), expected));
			if (!recover)
				return new Token(0, 0);
			if (TryDrop1Unexpected())
				return input.GetNextToken();
			if (TryDrop2Unexpected())
				return input.GetNextToken();
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
			this.stack = new int[MAX_STACK_SIZE];
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
				if (nextToken.SymbolID == 0 || allErrors.Count >= MAX_ERROR_COUNT)
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
