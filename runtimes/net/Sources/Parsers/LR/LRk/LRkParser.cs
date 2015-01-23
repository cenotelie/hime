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
using System;
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
		/// Initial size of the stack
		/// </summary>
		protected internal const int INIT_STACK_SIZE = 128;

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
			this.builder = new LRkASTBuilder(lexer.Output, parserVariables, parserVirtuals);
		}

		/// <summary>
		/// Gets whether the specified context is in effect
		/// </summary>
		/// <param name="context">A context</param>
		/// <returns><c>true</c> if the specified context is in effect</returns>
		public bool IsWithin(int context)
		{

		/// <summary>
		/// Raises an error on an unexepcted token
		/// </summary>
		/// <param name="token">The unexpected token</param>
		/// <returns>The next token in the case the error is recovered</returns>
		private Token OnUnexpectedToken(Token token)
			LRExpected expectedOnHead = parserAutomaton.GetExpected(stack[head], lexer.Terminals);
			// the terminals for shifts are always expected
			List<Symbol> expected = new List<Symbol>(expectedOnHead.Shifts);
			// check the terminals for reductions
			foreach (Symbol terminal in expectedOnHead.Reductions)
				if (CheckIsExpected(terminal))
					expected.Add(terminal);
			// register the error
			allErrors.Add(new UnexpectedTokenError(lexer.Output[token.Index], lexer.Output.GetPositionOf(token.Index), expected));
			// try to recover, or not
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
			if (context == Lexer.Automaton.DEFAULT_CONTEXT)
				return true;
			for (int i = head; i != -1; i--)
				if (automaton.GetContexts(stack[i]).Contains(context))
					return true;
			return false;
		}

		/// <summary>
		/// Checks whether the specified terminal is indeed expected for a reduction
		/// </summary>
		/// <param name="terminal">The terminal to check</param>
		/// <returns><code>true</code> if the terminal is really expected</returns>
		/// <remarks>
		/// This check is required because in the case of a base LALR graph,
		/// some terminals expected for reduction in the automaton are coming from other paths.
		/// </remarks>
		private bool CheckIsExpected(Symbol terminal)
		{
			// copy the stack to use for the simulation
			int[] myStack = new int[stack.Length];
			Array.Copy(stack, myStack, head + 1);
			int myHead = head;
			// get the action for the stack's head
			LRAction action = parserAutomaton.GetAction(myStack[myHead], terminal.ID);
			while (action.Code != LRActionCode.None)
			{
				if (action.Code == LRActionCode.Shift)
					// yep, the terminal was expected
					return true;
				if (action.Code == LRActionCode.Reduce)
				{
					// execute the reduction
					LRProduction production = parserAutomaton.GetProduction(action.Data);
					myHead -= production.ReductionLength;
					// this must be a shift
					action = parserAutomaton.GetAction(myStack[myHead], parserVariables[production.Head].ID);
					myHead++;
					if (myHead == myStack.Length)
						Array.Resize(ref myStack, myStack.Length + INIT_STACK_SIZE);
					myStack[myHead] = action.Data;
					// now, get the new action for the terminal
					action = parserAutomaton.GetAction(action.Data, terminal.ID);
				}
			}
			// nope, that was a pathological case in a LALR graph
			return false;
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
			this.stack = new int[INIT_STACK_SIZE];
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
					head++;
					if (head == stack.Length)
						Array.Resize(ref stack, stack.Length + INIT_STACK_SIZE);
					stack[head] = action.Data;
					builder.StackPushToken(token.Index);
					return action.Code;
				}
				else if (action.Code == LRActionCode.Reduce)
				{
					LRProduction production = automaton.GetProduction(action.Data);
					head -= production.ReductionLength;
					Reduce(production);
					action = automaton.GetAction(stack[head], parserVariables[production.Head].ID);
					head++;
					if (head == stack.Length)
						Array.Resize(ref stack, stack.Length + INIT_STACK_SIZE);
					stack[head] = action.Data;
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
