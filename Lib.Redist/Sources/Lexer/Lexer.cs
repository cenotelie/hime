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
using System.IO;

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Handler for lexical errors
	/// </summary>
	/// <param name="error">The new error</param>
	internal delegate void AddLexicalError(Error error);


	/// <summary>
	/// Represents a lexer for a text stream
	/// </summary>
	public abstract class Lexer
	{
		/// <summary>
		/// Symbol ID of the Epsilon terminal
		/// </summary>
		public const int sidEpsilon = 1;

		/// <summary>
		/// Symbol ID of the Dollar terminal
		/// </summary>
		public const int sidDollar = 2;

		/// <summary>
		/// This lexer's automaton
		/// </summary>
		private Automaton lexAutomaton;

		/// <summary>
		/// The terminals matched by this lexer
		/// </summary>
		private SymbolDictionary terminals;

		/// <summary>
		/// Symbol ID of the SEPARATOR terminal
		/// </summary>
		private int lexSeparator;

		/// <summary>
		/// This lexer's input
		/// </summary>
		private RewindableReader input;

		/// <summary>
		/// The tokenized text
		/// </summary>
		private TokenizedContent text;

		/// <summary>
		/// Flags whether the input's end has been reached and the Dollar token emited
		/// </summary>
		private bool isDollatEmited;

		/// <summary>
		/// The current index in the input
		/// </summary>
		private int index;

		/// <summary>
		/// Gets the terminals matched by this lexer
		/// </summary>
		public SymbolDictionary Terminals { get { return terminals; } }

		/// <summary>
		/// Gets the lexer's output as a tokenized text
		/// </summary>
		public TokenizedText Output { get { return text; } }

		/// <summary>
		/// Events for lexical errors
		/// </summary>
		internal event AddLexicalError OnError;

		/// <summary>
		/// Initializes a new instance of the TextLexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected Lexer(Automaton automaton, Symbol[] terminals, int separator, TextReader input)
		{
			this.lexAutomaton = automaton;
			this.terminals = new SymbolDictionary(terminals);
			this.lexSeparator = separator;
			this.text = new TokenizedContent(this.terminals);
			this.input = new RewindableReader(input, text);
			this.isDollatEmited = false;
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <returns>The next token in the input</returns>
		public Token GetNextToken()
		{
			if (isDollatEmited)
				return new Token(sidEpsilon, 0);

			while (true)
			{
				Token token = GetNextToken_DFA();
				if (token.SymbolID == sidEpsilon)
				{
					// Failed to match anything
					RewindableReader.Single s = input.ReadOne();
					OnError(new UnexpectedCharError(s.Value, text.GetPositionAt(index)));
					index++;
				}
				else if (token.SymbolID == sidDollar)
				{
					// At the end of the input
					isDollatEmited = true;
					return token;
				}
				else if (token.SymbolID != lexSeparator)
				{
					return token;
				}
			}
		}

		/// <summary>
		/// Gets the next token by running this lexer's DFA
		/// </summary>
		/// <returns>The next token in the input</returns>
		private Token GetNextToken_DFA()
		{
			int matchedIndex = 0;           // Terminal's index of the last match
			int matchedLength = 0;          // Length of the last match
			int length = 0;                 // Current number of character
			int state = 0;                  // Current state in the DFA

			TextBuffer buffer = input.Read();
			if (buffer.IsEmpty)
				return new Token(sidDollar, 0); // At the end of input
			int i = buffer.Start;

			while (state != 0xFFFF)
			{
				int offset = lexAutomaton.GetOffsetOf(state);
				// Is this state a matching state ?
				int terminal = lexAutomaton.GetStateRecognizedTerminal(offset);
				if (terminal != 0xFFFF)
				{
					matchedIndex = terminal;
					matchedLength = length;
				}
				// No further transition => exit
				if (lexAutomaton.IsStateDeadEnd(offset))
					break;
				// At the end of the buffer
				if (i == buffer.End)
				{
					buffer = input.Read();
					if (buffer.IsEmpty)
						break;
					i = buffer.Start;
				}
				char current = buffer[i++];
				length++;
				// Try to find a transition from this state with the read character
				if (current <= 255)
					state = lexAutomaton.GetStateCachedTransition(offset, current);
				else
					state = lexAutomaton.GetStateBulkTransition(offset, current);
			}
			input.GoTo(i - (length - matchedLength));

			if (matchedLength == 0)
				return new Token(sidEpsilon, 0);

			Token result = text.OnToken(matchedIndex, index, matchedLength);
			index += matchedLength;
			return result;
		}
	}
}
