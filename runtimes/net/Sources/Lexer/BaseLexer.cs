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
using System.Collections.Generic;
using System.IO;
using Hime.Redist.Utils;

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents a base lexer
	/// </summary>
	public abstract class BaseLexer : ILexer
	{
		/// <summary>
		/// Represents a match in the input
		/// </summary>
		protected struct Match
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
		/// This lexer's automaton
		/// </summary>
		protected Automaton automaton;
		/// <summary>
		/// The terminals matched by this lexer
		/// </summary>
		protected List<Symbol> terminals;
		/// <summary>
		/// Symbol ID of the SEPARATOR terminal
		/// </summary>
		protected int separatorID;
		/// <summary>
		/// The input text
		/// </summary>
		internal BaseText text;
		/// <summary>
		/// The token repository
		/// </summary>
		internal TokenRepository tokens;

		/// <summary>
		/// Gets the terminals matched by this lexer
		/// </summary>
		public ROList<Symbol> Terminals { get { return new ROList<Symbol>(terminals); } }

		/// <summary>
		/// Gets the lexer's input text
		/// </summary>
		public Text Input { get { return text; } }

		/// <summary>
		/// Gets the lexer's output stream of tokens
		/// </summary>
		public IEnumerable<Token> Output { get { return tokens; } }

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
		protected BaseLexer(Automaton automaton, Symbol[] terminals, int separator, string input)
		{
			this.automaton = automaton;
			this.terminals = new List<Symbol>(terminals);
			this.separatorID = separator;
			this.text = new PrefetchedText(input);
			this.tokens = new TokenRepository(new ROList<Symbol>(this.terminals), this.text);
		}

		/// <summary>
		/// Initializes a new instance of the Lexer class with the given input
		/// </summary>
		/// <param name="automaton">DFA automaton for this lexer</param>
		/// <param name="terminals">Terminals recognized by this lexer</param>
		/// <param name="separator">SID of the separator token</param>
		/// <param name="input">Input to this lexer</param>
		protected BaseLexer(Automaton automaton, Symbol[] terminals, int separator, TextReader input)
		{
			this.automaton = automaton;
			this.terminals = new List<Symbol>(terminals);
			this.separatorID = separator;
			this.text = new StreamingText(input);
			this.tokens = new TokenRepository(new ROList<Symbol>(this.terminals), this.text);
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <returns>The next token in the input</returns>
		public Token GetNextToken()
		{
			TokenKernel kernel = GetNextToken(null);
			return tokens[kernel.Index];
		}

		/// <summary>
		/// Raises the specified error
		/// </summary>
		/// <param name="error">An error raised by this lexer</param>
		protected void RaiseError(ParseError error)
		{
			OnError(error);
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The next token in the input</returns>
		internal abstract TokenKernel GetNextToken(IContextProvider contexts);
	}
}
