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
	public class ContextFreeLexer : BaseLexer
	{
		/// <summary>
		/// Index of the next token
		/// </summary>
		private int tokenIndex;

		/// <summary>
		/// Initializes a new instance of the Lexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected ContextFreeLexer(Automaton automaton, Symbol[] terminals, int separator, string input)
			: base(automaton, terminals, separator, input)
		{
			tokenIndex = -1;
		}

		/// <summary>
		/// Initializes a new instance of the Lexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected ContextFreeLexer(Automaton automaton, Symbol[] terminals, int separator, TextReader input)
			: base(automaton, terminals, separator, input)
		{
			tokenIndex = -1;
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The next token in the input</returns>
		internal override TokenKernel GetNextToken(IContextProvider contexts)
		{
			if (tokens.Size == 0)
			{
				// this is the first call to this method, prefetch the tokens
				FindTokens();
				tokenIndex = 0;
			}
			// no more tokens? return epsilon
			if (tokenIndex >= tokens.Size)
				return new TokenKernel(Symbol.SID_EPSILON, 0);
			TokenKernel result = new TokenKernel(tokens.GetSymbol(tokenIndex).ID, tokenIndex);
			tokenIndex++;
			return result;
		}

		/// <summary>
		/// Finds all the tokens in the lexer's input
		/// </summary>
		private void FindTokens()
		{
			int inputIndex = 0;
			while (true)
			{
				Match match = RunDFA(inputIndex);
				if (match.length != 0)
				{
					// matched something
					if (symTerminals[match.terminal].ID != separatorID)
						tokens.Add(match.terminal, inputIndex, match.length);
					inputIndex += match.length;
					continue;
				}
				if (match.terminal == 0)
				{
					// This is the EPSILON terminal, failed to match anything
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
					continue;
				}
				// This is the dollar terminal, at the end of the input
				tokens.Add(match.terminal, inputIndex, match.length);
				return;
			}
		}

		/// <summary>
		/// Runs the lexer's DFA to match a terminal in the input ahead
		/// </summary>
		/// <param name="inputIndex">The current start index in the input text</param>
		/// <returns>The matched terminal and length</returns>
		private Match RunDFA(int inputIndex)
		{
			if (text.IsEnd(inputIndex))
			{
				// At the end of input
				return new Match(1, 0); // 1 is always the index of the $ terminal
			}

			Match result = new Match();
			int state = 0;
			int i = inputIndex;

			while (state != Automaton.DEAD_STATE)
			{
				AutomatonState stateData = automaton.GetState(state);
				// Is this state a matching state ?
				if (stateData.TerminalsCount != 0)
					result = new Match(stateData.GetTerminal(0).Index, i - inputIndex);
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
			return result;
		}
	}
}