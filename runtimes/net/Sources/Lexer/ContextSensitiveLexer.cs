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
				Match match = RunDFA(contexts);
				if (match.length != 0)
				{
					// matched something !
					int terminalID = symTerminals[match.terminal].ID;
					if (terminalID == separatorID)
					{
						inputIndex += match.length;
						continue;
					}
					TokenKernel token = new TokenKernel(terminalID, tokens.Add(match.terminal, inputIndex, match.length));
					inputIndex += match.length;
					return token;
				}
				if (match.terminal == 1)
				{
					// This is the dollar terminal, at the end of the input
					isDollarEmitted = true;
					return new TokenKernel(Symbol.SID_DOLLAR, tokens.Add(1, inputIndex, 0));
				}
				// Failed to match anything
				TextPosition position = text.GetPositionAt(inputIndex);
				string unexpected;
				int c = text.GetValue(inputIndex);
				if (c >= 0xD800 && c <= 0xDFFF)
				{
					// this is a surrogate encoding point
					unexpected = text.GetValue(inputIndex, 2);
					inputIndex += 2;
				}
				else
				{
					unexpected = text.GetValue(inputIndex).ToString();
					inputIndex++;
				}
				RaiseError(new UnexpectedCharError(unexpected, position));
			}
		}

		/// <summary>
		/// Runs the lexer's DFA to match a terminal in the input ahead
		/// </summary>
		/// <param name="provider">The provider of contextual information</param>
		/// <returns>The matched terminal and length</returns>
		private Match RunDFA(IContextProvider provider)
		{
			if (text.IsEnd(inputIndex))
			{
				// At the end of input
				return new Match(1, 0); // 1 is always the index of the $ terminal
			}

			AutomatonState stateData;
			int matchState = 0;
			int matchLength = 0;
			int state = 0;
			int i = inputIndex;

			while (state != Automaton.DEAD_STATE)
			{
				stateData = automaton.GetState(state);
				// Is this state a matching state ?
				if (stateData.TerminalsCount != 0)
				{
					matchState = state;
					matchLength = i - inputIndex;
				}
				// No further transition => exit
				if (stateData.IsDeadEnd)
					break;
				// At the end of the buffer
				if (text.IsEnd(i))
					break;
				char current = text.GetValue(i);
				i++;
				// Try to find a transition from this state with the read character
				state = stateData.GetTargetBy(current);
			}

			if (matchLength == 0)
				// no match
				return new Match(-1, 0);

			stateData = automaton.GetState(matchState);
			Match contextual = new Match();
			for (int j = 0; j != stateData.TerminalsCount; j++)
			{
				MatchedTerminal mt = stateData.GetTerminal(j);
				int id = symTerminals[mt.Index].ID;
				if (id == separatorID)
					return new Match(mt.Index, matchLength);
				if (provider.IsExpected(id))
				{
					if (provider.IsInContext(mt.Context, id))
						// perfect match
						return new Match(mt.Index, matchLength);
					// not in context, do not match
				}
				else if (provider.IsInContext(mt.Context, id))
				{
					// in the right context, but not expected
					if (contextual.length == 0)
						// this is the first, register it
						contextual = new Match(mt.Index, matchLength);
				}
			}
			// at this point we do not have a perfect match
			// return the match with the highest priority that is possible in the contexts
			return contextual;
		}
	}
}