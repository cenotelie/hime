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
	/// Handler for lexical errors
	/// </summary>
	/// <param name="error">The new error</param>
	public delegate void AddLexicalError(ParseError error);

	/// <summary>
	/// Represents a base lexer
	/// </summary>
	public abstract class BaseLexer
	{
		/// <summary>
		/// This lexer's automaton
		/// </summary>
		protected readonly Automaton automaton;
		/// <summary>
		/// The terminals matched by this lexer
		/// </summary>
		protected readonly ROList<Symbol> symTerminals;
		/// <summary>
		/// Symbol ID of the SEPARATOR terminal
		/// </summary>
		protected readonly int separatorID;
		/// <summary>
		/// The input text
		/// </summary>
		internal readonly BaseText text;
		/// <summary>
		/// The token repository
		/// </summary>
		internal readonly TokenRepository tokens;

		/// <summary>
		/// Gets the terminals matched by this lexer
		/// </summary>
		public ROList<Symbol> Terminals { get { return symTerminals; } }

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
			symTerminals = new ROList<Symbol>(new List<Symbol>(terminals));
			separatorID = separator;
			text = new PrefetchedText(input);
			tokens = new TokenRepository(symTerminals, text);
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
			symTerminals = new ROList<Symbol>(new List<Symbol>(terminals));
			separatorID = separator;
			text = new StreamingText(input);
			tokens = new TokenRepository(symTerminals, text);
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <returns>The next token in the input</returns>
		public Token GetNextToken()
		{
			return tokens[GetNextToken(null).Index];
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

		/// <summary>
		/// Gets the possible next tokens in the input
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The possible next tokens in the input</returns>
		internal abstract Buffer<TokenKernel> GetNextTokens(IContextProvider contexts);
	}
}
