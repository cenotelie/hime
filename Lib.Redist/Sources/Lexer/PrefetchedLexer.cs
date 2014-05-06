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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents a lexer for a prefetched piece of text, i.e. the text is already in memory
	/// </summary>
	public abstract class PrefetchedLexer : ILexer
	{
		/// <summary>
		/// This lexer's automaton
		/// </summary>
		private Automaton lexAutomaton;
		/// <summary>
		/// The terminals matched by this lexer
		/// </summary>
		private IList<Symbol> terminals;
		/// <summary>
		/// Symbol ID of the SEPARATOR terminal
		/// </summary>
		private int lexSeparator;
		/// <summary>
		/// The lexer's full input
		/// </summary>
		private string input;
		/// <summary>
		/// The tokenized text
		/// </summary>
		private PrefetchedText text;
		/// <summary>
		/// The current index in the input
		/// </summary>
		private int inputIndex;
		/// <summary>
		/// The index of the next token
		/// </summary>
		private int tokenIndex;

		/// <summary>
		/// Gets the terminals matched by this lexer
		/// </summary>
		public IList<Symbol> Terminals { get { return terminals; } }
		/// <summary>
		/// Gets the lexer's output as a tokenized text
		/// </summary>
		public TokenizedText Output { get { return text; } }
		/// <summary>
		/// Events for lexical errors
		/// </summary>
		public event AddLexicalError OnError;

		/// <summary>
		/// Initializes a new instance of the Lexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected PrefetchedLexer(Automaton automaton, Symbol[] terminals, int separator, string input)
		{
			this.lexAutomaton = automaton;
			this.terminals = new ReadOnlyCollection<Symbol>(new List<Symbol>(terminals));
			this.lexSeparator = separator;
			this.input = input;
			this.text = new PrefetchedText(this.terminals, this.input);
			this.inputIndex = 0;
			this.tokenIndex = -1;
		}

		/// <summary>
		/// Initializes a new instance of the Lexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected PrefetchedLexer(Automaton automaton, Symbol[] terminals, int separator, TextReader input)
			: this (automaton, terminals, separator, input.ReadToEnd())
		{
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <returns>The next token in the input</returns>
		public Token GetNextToken()
		{
			if (tokenIndex == -1)
			{
				// this is the first call to this method, prefetch all the things!
				text.FindLines();
				FindTokens();
				tokenIndex = 0;
			}
			// no more tokens? return epsilon
			if (tokenIndex >= text.TokenCount)
				return new Token(Symbol.sidEpsilon, 0);
			return text.GetTokenAt(tokenIndex++);
		}

		/// <summary>
		/// Represents a match in the input
		/// </summary>
		private struct Match
		{
			/// <summary>
			/// Index of the matched terminal
			/// </summary>
			public int terminal;
			/// <summary>
			/// Length of the matched input
			/// </summary>
			public int length;
			/// <summary>
			/// Initializes a match
			/// </summary>
			/// <param name='terminal'>Index of the matched terminal</param>
			public Match(int terminal)
			{
				this.terminal = terminal;
				this.length = 0;
			}
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
					if (terminals[match.terminal].ID != lexSeparator)
						text.AddToken(match.terminal, inputIndex, match.length);
					inputIndex += match.length;
					continue;
				}
				if (match.terminal == 0)
				{
					// This is the epsilon terminal, failed to match anything
					OnError(new UnexpectedCharError(input[inputIndex], text.GetPositionAt(inputIndex)));
					inputIndex++;
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
			if (inputIndex == input.Length)
			{
				// At the end of input
				return new Match(1);
			}

			Match result = new Match();
			int state = 0;
			int i = inputIndex;

			while (state != 0xFFFF)
			{
				int offset = lexAutomaton.GetOffsetOf(state);
				// Is this state a matching state ?
				int terminal = lexAutomaton.GetStateRecognizedTerminal(offset);
				if (terminal != 0xFFFF)
				{
					result.terminal = terminal;
					result.length = (i - inputIndex);
				}
				// No further transition => exit
				if (lexAutomaton.IsStateDeadEnd(offset))
					break;
				// At the end of the buffer
				if (i == input.Length)
					break;
				char current = input[i++];
				// Try to find a transition from this state with the read character
				if (current <= 255)
					state = lexAutomaton.GetStateCachedTransition(offset, current);
				else
					state = lexAutomaton.GetStateBulkTransition(offset, current);
			}
			return result;
		}
	}
}