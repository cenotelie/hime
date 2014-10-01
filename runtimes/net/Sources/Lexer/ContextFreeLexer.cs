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
	public abstract class ContextFreeLexer : BaseLexer
	{
		/// <summary>
		/// Index of the next token
		/// </summary>
		protected int tokenIndex = -1;

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
			this.tokenIndex = -1;
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
			this.tokenIndex = -1;
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <returns>The next token in the input</returns>
		/// <remarks>This forces the use of the default context</remarks>
		public override Token GetNextToken()
		{
			if (tokenIndex == -1)
			{
				// this is the first call to this method, prefetch the tokens
				FindTokens();
				tokenIndex = 0;
			}
			// no more tokens? return epsilon
			if (tokenIndex >= text.TokenCount)
				return new Token(Symbol.SID_EPSILON, 0);
			return text.GetTokenAt(tokenIndex++);
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The next token in the input</returns>
		public override Token GetNextToken(ContextStack contexts)
		{
			return GetNextToken();
		}

		/// <summary>
		/// Finds all the tokens in the lexer's input
		/// </summary>
		private void FindTokens()
		{
			while (true)
			{
				Match match = RunDFA();
				if (match.length != 0)
				{
					// matched something
					if (recognizedTerminals[match.terminal].ID != separatorID)
						text.AddToken(match.terminal, inputIndex, match.length);
					inputIndex += match.length;
					continue;
				}
				if (match.terminal == Symbol.SID_NOTHING)
				{
					// This is an error
					TextPosition position = text.GetPositionAt(inputIndex);
					string unexpected = null;
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
				text.AddToken(match.terminal, inputIndex, match.length);
				return;
			}
		}

		/// <summary>
		/// Runs the lexer's DFA to match a terminal in the input ahead
		/// </summary>
		/// <returns>The matched terminal and length</returns>
		private Match RunDFA()
		{
			if (text.IsEnd(inputIndex))
			{
				// At the end of input
				return new Match(Symbol.SID_DOLLAR);
			}

			Match result = new Match();
			int state = 0;
			int i = inputIndex;

			while (state != Automaton.DEAD_STATE)
			{
				State stateData = automaton.GetState(state);
				// Is this state a matching state ?
				if (stateData.TerminalsCount != 0)
				{
					result.terminal = stateData.GetTerminal(0).Index;
					result.length = (i - inputIndex);
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
			return result;
		}
	}
}