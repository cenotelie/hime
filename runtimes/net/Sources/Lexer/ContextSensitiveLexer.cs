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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents a context-free lexer (lexing rules do not depend on the context)
	/// </summary>
	public class ContextSensitiveLexer : BaseLexer
	{
		/// <summary>
		/// The current index in the input
		/// </summary>
		private int inputIndex;
		/// <summary>
		/// Whether the end-of-input dollar marker has already been emitted
		/// </summary>
		private bool isDollarEmitted;

		/// <summary>
		/// Initializes a new instance of the Lexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected ContextSensitiveLexer(Automaton automaton, Symbol[] terminals, int separator, string input)
			: base(automaton, terminals, separator, input)
		{
			inputIndex = 0;
		}

		/// <summary>
		/// Initializes a new instance of the Lexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected ContextSensitiveLexer(Automaton automaton, Symbol[] terminals, int separator, TextReader input)
			: base(automaton, terminals, separator, input)
		{
			inputIndex = 0;
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The next token in the input</returns>
		internal override TokenKernel GetNextToken(IContextProvider contexts)
		{
			if (isDollarEmitted)
				return new TokenKernel(Symbol.SID_EPSILON, -1);

			while (true)
			{
				TokenMatch match = RunDFA(inputIndex);
				if (!match.IsSuccess)
				{
					// failed to match, retry with error handling
					match = RunDFAOnError(inputIndex);
				}
				if (match.IsSuccess)
				{
					if (match.state == 0)
					{
						// this is the dollar terminal, at the end of the input
						// the index of the $ symbol is always 1
						isDollarEmitted = true;
						return new TokenKernel(Symbol.SID_DOLLAR, tokens.Add(1, inputIndex, 0));
					}
					else
					{
						// matched something
						int tIndex = GetTerminalFor(match.state, contexts);
						int tID = symTerminals[tIndex].ID;
						if (tID == separatorID)
						{
							inputIndex += match.length;
							continue;
						}
						else
						{
							TokenKernel token = new TokenKernel(tID, tokens.Add(tIndex, inputIndex, match.length));
							inputIndex += match.length;
							return token;
						}
					}
				}
				else
				{
					inputIndex += match.length;
				}
			}
		}

		/// <summary>
		/// Gets the index of the terminal with the highest priority that is possible in the contexts
		/// </summary>
		/// <param name="state">The DFA state</param>
		/// <param name="provider">The current applicable contexts</param>
		/// <returns>The index of the terminal</returns>
		private int GetTerminalFor(int state, IContextProvider provider)
		{
			AutomatonState stateData = automaton.GetState(state);
			MatchedTerminal mt = stateData.GetTerminal(0);
			int id = symTerminals[mt.Index].ID;
			int currentResult = mt.Index;
			if (id == separatorID)
				// the separator trumps all
				return currentResult;
			int currentPriority = provider.GetContextPriority(mt.Context, id);
			for (int i = 1; i != stateData.TerminalsCount; i++)
			{
				mt = stateData.GetTerminal(i);
				id = symTerminals[mt.Index].ID;
				if (id == separatorID)
					// the separator trumps all
					return mt.Index;
				int priority = provider.GetContextPriority(mt.Context, id);
				if (currentPriority < 0 || (priority >= 0 && priority < currentPriority))
				{
					currentResult = mt.Index;
					currentPriority = priority;
				}
			}
			return currentResult;
		}
	}
}