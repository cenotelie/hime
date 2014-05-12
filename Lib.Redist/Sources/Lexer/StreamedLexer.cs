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
	/// Represents a lexer for a text stream
	/// </summary>
	public abstract class StreamedLexer : ILexer
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
		/// This lexer's input
		/// </summary>
		private RewindableReader input;
		/// <summary>
		/// The tokenized text
		/// </summary>
		private StreamedText text;
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
		protected StreamedLexer(Automaton automaton, Symbol[] terminals, int separator, TextReader input)
		{
			this.lexAutomaton = automaton;
			this.terminals = new ReadOnlyCollection<Symbol>(new List<Symbol>(terminals));
			this.lexSeparator = separator;
			this.text = new StreamedText(this.terminals);
			this.input = new RewindableReader(input, text);
			this.isDollatEmited = false;
		}

		/// <summary>
		/// Initializes a new instance of the Lexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected StreamedLexer(Automaton automaton, Symbol[] terminals, int separator, string input)
			: this(automaton, terminals, separator, new StringReader(input))
		{
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <returns>The next token in the input</returns>
		public Token GetNextToken()
		{
			if (isDollatEmited)
				return new Token(Symbol.sidEpsilon, 0);

			while (true)
			{
				int length = 0;
				int tIndex = RunDFA(out length);
				if (length == 0)
				{
					if (tIndex == 0)
					{
						// This is the epsilon terminal, failed to match anything
						TextPosition position = text.GetPositionAt(index);
						string unexpected = null;
						int c = input.ReadOne().Value;
						if (c >= 0xD800 && c <= 0xDFFF)
						{
							// this is a surrogate encoding point
							unexpected = new string(new char[] { System.Convert.ToChar(c), input.ReadOne().Value });
							index += 2;
						}
						else
						{
							unexpected = new string(System.Convert.ToChar(c), 1);
							index++;
						}
						OnError(new UnexpectedCharError(unexpected, position));
					}
					else if (tIndex == 1)
					{
						// This is the dollar terminal, at the end of the input
						isDollatEmited = true;
						return text.AddToken(tIndex, index, length);
					}
				}
				else
				{
					if (terminals[tIndex].ID != lexSeparator)
					{
						Token token = text.AddToken(tIndex, index, length);
						index += length;
						return token;
					}
					index += length;
				}
			}
		}

		/// <summary>
		/// Runs the lexer's DFA to match a terminal in the input ahead
		/// </summary>
		/// <param name="tokenLength">The length of the matched token</param>
		/// <returns>The index of the matched terminal</returns>
		private int RunDFA(out int tokenLength)
		{
			int matchedIndex = 0;           // Terminal's index of the last match
			int matchedLength = 0;          // Length of the last match
			int length = 0;                 // Current number of character
			int state = 0;                  // Current state in the DFA

			TextBuffer buffer = input.Read();
			if (buffer.IsEmpty)
			{
				// At the end of input
				tokenLength = 0;
				return 1;
			}
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
			// rewind if necessary
			input.GoTo(i - (length - matchedLength));
			tokenLength = matchedLength;
			return matchedIndex;
		}
	}
}
