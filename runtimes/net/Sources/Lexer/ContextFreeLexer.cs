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
						tokens.Add(1, inputIndex, 0);
						return;
					}
					else
					{
						// matched something
						int tIndex = automaton.GetState(match.state).GetTerminal(0).Index;
						if (symTerminals[tIndex].ID != separatorID)
							tokens.Add(tIndex, inputIndex, match.length);
						inputIndex += match.length;
					}
				}
				else
				{
					inputIndex += match.length;
				}
			}
		}
	}
}