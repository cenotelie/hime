/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System;
using System.Collections.Generic;
using Hime.Redist.Utils;

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
		protected readonly LRkAutomaton automaton;
		/// <summary>
		/// The parser's stack
		/// </summary>
		private int[] stack;
		/// <summary>
		/// The identifiers of the items on the stack
		/// </summary>
		private int[] stackIDs;
		/// <summary>
		/// Index of the stack's head
		/// </summary>
		private int head;
		/// <summary>
		/// The AST builder
		/// </summary>
		private readonly LRkASTBuilder builder;

		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="automaton">The parser's automaton</param>
		/// <param name="variables">The parser's variables</param>
		/// <param name="virtuals">The parser's virtuals</param>
		/// <param name="actions">The parser's actions</param>
		/// <param name="lexer">The input lexer</param>
		protected LRkParser(LRkAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, Lexer.BaseLexer lexer)
			: base(variables, virtuals, actions, lexer)
		{
			this.automaton = automaton;
			stack = new int[INIT_STACK_SIZE];
			stackIDs = new int[INIT_STACK_SIZE];
			head = 0;
			builder = new LRkASTBuilder(lexer.tokens, symVariables, symVirtuals);
		}

		/// <summary>
		/// Gets the priority of the specified context required by the specified terminal
		/// The priority is a positive integer. The lesser the value the higher the priority.
		/// A value of -1 represents the unavailability of the required context.
		/// </summary>
		/// <param name="context">A context</param>
		/// <param name="onTerminalID">The identifier of the terminal requiring the context</param>
		/// <returns>The context priority, or -1 if the context is unavailable</returns>
		public int GetContextPriority(int context, int onTerminalID)
		{
			// the default context is always active
			if (context == Lexer.Automaton.DEFAULT_CONTEXT)
				return int.MaxValue;
			if (lexer.tokens.Size == 0)
			{
				// this is the first token, does it open the context?
				return automaton.GetContexts(0).Opens(onTerminalID, context) ? 0 : -1;
			}
			// retrieve the action for this terminal
			LRAction action = automaton.GetAction(stack[head], onTerminalID);
			// if the terminal is unexpected, do not validate
			if (action.Code == LRActionCode.None)
				return -1;
			// does the context opens with the terminal?
			if (action.Code == LRActionCode.Shift && automaton.GetContexts(stack[head]).Opens(onTerminalID, context))
				return 0;
			LRProduction production = (action.Code == LRActionCode.Reduce) ? automaton.GetProduction(action.Data) : null;
			// look into the stack for the opening of the context
			for (int i = head - 1; i != -1; i--)
			{
				if (automaton.GetContexts(stack[i]).Opens(stackIDs[i + 1], context))
				{
					// the context opens here
					// but is it closed by the reduction (if any)?
					if (production == null || i < head - production.ReductionLength)
						// no, we are still in the context
						return head - i;
				}
			}
			// at this point, the requested context is not yet open or is closed by a reduction
			// now, if the action is something else than a reduction (shift, accept or error), the context can never be produced
			// for the context to open, a new state must be pushed onto the stack
			// this means that the provided terminal must trigger a chain of at least one reduction
			if (action.Code != LRActionCode.Reduce)
				return -1;
			// there is at least one reduction, simulate
			int[] myStack = new int[stack.Length];
			Array.Copy(stack, myStack, head + 1);
			int myHead = head;
			while (action.Code == LRActionCode.Reduce)
			{
				// execute the reduction
				production = automaton.GetProduction(action.Data);
				myHead -= production.ReductionLength;
				// this must be a shift
				action = automaton.GetAction(myStack[myHead], symVariables[production.Head].ID);
				myHead++;
				if (myHead == myStack.Length)
					Array.Resize(ref myStack, myStack.Length + INIT_STACK_SIZE);
				myStack[myHead] = action.Data;
				// now, get the new action for the terminal
				action = automaton.GetAction(action.Data, onTerminalID);
			}
			// is this a shift action that opens the context?
			return ((action.Code == LRActionCode.Shift && automaton.GetContexts(myStack[myHead]).Opens(onTerminalID, context)) ? 0 : -1);
		}

		/// <summary>
		/// Raises an error on an unexepcted token
		/// </summary>
		/// <param name="kernel">The unexpected token's kernel</param>
		/// <returns>The next token kernel in the case the error is recovered</returns>
		private Lexer.TokenKernel OnUnexpectedToken(Lexer.TokenKernel kernel)
		{
			LRExpected expectedOnHead = automaton.GetExpected(stack[head], lexer.Terminals);
			// the terminals for shifts are always expected
			List<Symbol> expected = new List<Symbol>(expectedOnHead.Shifts);
			// check the terminals for reductions
			foreach (Symbol terminal in expectedOnHead.Reductions)
				if (CheckIsExpected(terminal))
					expected.Add(terminal);
			// register the error
			allErrors.Add(new UnexpectedTokenError(lexer.tokens[kernel.Index], new ROList<Symbol>(expected)));
			// TODO: try to recover, or not
			return new Lexer.TokenKernel(Symbol.SID_NOTHING, -1);
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
			LRAction action = automaton.GetAction(myStack[myHead], terminal.ID);
			while (action.Code != LRActionCode.None)
			{
				if (action.Code == LRActionCode.Shift)
					// yep, the terminal was expected
					return true;
				if (action.Code == LRActionCode.Reduce)
				{
					// execute the reduction
					LRProduction production = automaton.GetProduction(action.Data);
					myHead -= production.ReductionLength;
					// this must be a shift
					action = automaton.GetAction(myStack[myHead], symVariables[production.Head].ID);
					myHead++;
					if (myHead == myStack.Length)
						Array.Resize(ref myStack, myStack.Length + INIT_STACK_SIZE);
					myStack[myHead] = action.Data;
					// now, get the new action for the terminal
					action = automaton.GetAction(action.Data, terminal.ID);
				}
			}
			// nope, that was a pathological case in a LALR graph
			return false;
		}

		/// <summary>
		/// Parses the input and returns the result
		/// </summary>
		/// <returns>A ParseResult object containing the data about the result</returns>
		public override ParseResult Parse()
		{
			Lexer.TokenKernel nextKernel = lexer.GetNextToken(this);
			while (true)
			{
				LRActionCode action = ParseOnToken(nextKernel);
				if (action == LRActionCode.Shift)
				{
					nextKernel = lexer.GetNextToken(this);
					continue;
				}
				if (action == LRActionCode.Accept)
					return new ParseResult(new ROList<ParseError>(allErrors), lexer.Input, builder.GetTree());
				nextKernel = OnUnexpectedToken(nextKernel);
				if (nextKernel.TerminalID == Symbol.SID_NOTHING || allErrors.Count >= MAX_ERROR_COUNT)
					return new ParseResult(new ROList<ParseError>(allErrors), lexer.Input);
			}
		}

		/// <summary>
		/// Parses on the specified token kernel
		/// </summary>
		/// <param name="kernel">The token kernel to parse on</param>
		/// <returns>The LR action that was used</returns>
		private LRActionCode ParseOnToken(Lexer.TokenKernel kernel)
		{
			while (true)
			{
				LRAction action = automaton.GetAction(stack[head], kernel.TerminalID);
				if (action.Code == LRActionCode.Shift)
				{
					head++;
					if (head == stack.Length)
					{
						Array.Resize(ref stack, stack.Length + INIT_STACK_SIZE);
						Array.Resize(ref stackIDs, stackIDs.Length + INIT_STACK_SIZE);
					}
					stack[head] = action.Data;
					stackIDs[head] = kernel.TerminalID;
					builder.StackPushToken(kernel.Index);
					return action.Code;
				}
				if (action.Code == LRActionCode.Reduce)
				{
					LRProduction production = automaton.GetProduction(action.Data);
					head -= production.ReductionLength;
					Reduce(production);
					action = automaton.GetAction(stack[head], symVariables[production.Head].ID);
					head++;
					if (head == stack.Length)
					{
						Array.Resize(ref stack, stack.Length + INIT_STACK_SIZE);
						Array.Resize(ref stackIDs, stackIDs.Length + INIT_STACK_SIZE);
					}
					stack[head] = action.Data;
					stackIDs[head] = symVariables[production.Head].ID;
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
			Symbol variable = symVariables[production.Head];
			builder.ReductionPrepare(production.Head, production.ReductionLength, production.HeadAction);
			for (int i = 0; i != production.BytecodeLength; i++)
			{
				LROpCode op = production[i];
				switch (op.Base)
				{
				case LROpCodeBase.SemanticAction:
					{
						SemanticAction action = symActions[production[i + 1].DataValue];
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
