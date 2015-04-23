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
			head = 0;
			builder = new LRkASTBuilder(lexer.tokens, symVariables, symVirtuals);
		}

		/// <summary>
		/// Gets whether a terminal is acceptable
		/// </summary>
		/// <param name="context">The terminal's context</param>
		/// <param name="terminalIndex">The terminal's index</param>
		/// <returns><code>true</code> if the terminal is acceptable</returns>
		public bool IsAcceptable(int context, int terminalIndex)
		{
			// check that there is an action for this terminal
			LRAction action = automaton.GetAction(stack[head], lexer.Terminals[terminalIndex].ID);
			if (action.Code == LRActionCode.None)
				return false;
			// check that the parser is in the right context
			if (context == Lexer.Automaton.DEFAULT_CONTEXT)
				return true;
			for (int i = head; i != -1; i--)
				if (automaton.GetContexts(stack[i]).Contains(context))
					return true;
			return false;
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
						Array.Resize(ref stack, stack.Length + INIT_STACK_SIZE);
					stack[head] = action.Data;
					builder.StackPushToken(kernel.Index);
					return action.Code;
				}
				else if (action.Code == LRActionCode.Reduce)
				{
					LRProduction production = automaton.GetProduction(action.Data);
					head -= production.ReductionLength;
					Reduce(production);
					action = automaton.GetAction(stack[head], symVariables[production.Head].ID);
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
