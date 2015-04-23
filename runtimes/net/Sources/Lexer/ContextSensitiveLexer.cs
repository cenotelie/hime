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
using Hime.Redist.Utils;

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
		/// The buffer for token kernels
		/// </summary>
		private Buffer<TokenKernel> buffer;
		/// <summary>
		/// The buffer of matches
		/// </summary>
		private Buffer<int> matches;

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
			buffer = new Buffer<TokenKernel>(5);
			matches = new Buffer<int>(5);
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
			buffer = new Buffer<TokenKernel>(5);
			matches = new Buffer<int>(5);
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The next token in the input</returns>
		internal override TokenKernel GetNextToken(IContextProvider contexts)
		{
			while (true)
			{
				int length = RunDFA(contexts);
				if (length != 0)
				{
					// matched something !
					int terminalIndex = matches[0];
					int terminalID = symTerminals[terminalIndex].ID;
					if (terminalID == separatorID)
						continue;
					TokenKernel token = new TokenKernel(terminalID, tokens.Add(terminalIndex, inputIndex, length));
					inputIndex += length;
					return token;
				}
				if (matches.Size > 0)
				{
					// This is the dollar terminal, at the end of the input
					return new TokenKernel(Symbol.SID_DOLLAR, tokens.Add(matches[0], inputIndex, 0));
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
				continue;
			}
		}

		/// <summary>
		/// Gets the possible next tokens in the input
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The possible next tokens in the input</returns>
		internal override Buffer<TokenKernel> GetNextTokens(IContextProvider contexts)
		{
			while (true)
			{
				int length = RunDFA(contexts);
				if (length != 0)
				{
					// matched something !
					buffer.Reset();
					for (int i = 0; i != matches.Size; i++)
					{
						int terminalIndex = matches[i];
						int terminalID = symTerminals[terminalIndex].ID;
						if (terminalID == separatorID)
							// filter out the separators
							continue;
						buffer.Add(new TokenKernel(terminalID, tokens.Add(terminalIndex, inputIndex, length)));
						inputIndex += length;
					}
					if (buffer.Size > 0)
						return buffer;
					continue;
				}
				if (matches.Size > 0)
				{
					// This is the dollar terminal, at the end of the input
					buffer.Reset();
					buffer.Add(new TokenKernel(Symbol.SID_DOLLAR, tokens.Add(matches[0], inputIndex, 0)));
					return buffer;
				}
				// Failed to match anything
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
		}

		/// <summary>
		/// Runs the lexer's DFA to match a terminal in the input ahead
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The length of the matches</returns>
		private int RunDFA(IContextProvider contexts)
		{
			matches.Reset();

			if (text.IsEnd(inputIndex))
			{
				// At the end of input
				matches.Add(1); // 1 is always the index of the $ terminal
				return 0; // length is 0
			}

			int state = 0;
			int i = inputIndex;
			int length = 0;

			while (state != Automaton.DEAD_STATE)
			{
				AutomatonState stateData = automaton.GetState(state);
				// Is this state a matching state ?
				bool firstMatch = true;
				for (int j = 0; i != stateData.TerminalsCount; j++)
				{
					MatchedTerminal mt = stateData.GetTerminal(j);
					if (contexts.IsAcceptable(mt.Context, mt.Index))
					{
						if (firstMatch)
						{
							// this is the first match in this state, the longest input
							matches.Reset();
							length = i - inputIndex;
							firstMatch = false;
						}
						matches.Add(mt.Index);
					}
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
			return length;
		}
	}
}