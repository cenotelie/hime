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
		/// The parser's automaton
		/// </summary>
		protected LRkAutomaton automaton;
		/// <summary>
		/// The AST builder
		/// </summary>
		internal LRkASTBuilder builder;

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
		/// Raises an error on an unexpected token
		/// </summary>
		/// <param name="state">The current LR state</param>
		/// <param name="token">The unexpected token</param>
		/// <returns>The next token in the case the error is recovered</returns>
		protected Token OnUnexpectedToken(int state, Token token)
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
		protected void Reduce(LRProduction production)
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
