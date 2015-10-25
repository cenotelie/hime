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
		/// The default maximum Levenshtein distance to go to for the recovery of a matching failure
		/// </summary>
		protected const int DEFAULT_RECOVERY_MATCHING_DISTANCE = 3;

		/// <summary>
		/// The default context provider
		/// </summary>
		private class DefaultContextProvider : IContextProvider
		{
			public int GetContextPriority(int context, int onTerminalID)
			{
				return context == Automaton.DEFAULT_CONTEXT ? int.MaxValue : 0;
			}
		}

		/// <summary>
		/// The default context provider
		/// </summary>
		private static readonly DefaultContextProvider DEFAULT_CONTEXT_PROVIDER = new DefaultContextProvider();

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
		/// Gets or sets the maximum Levenshtein distance to go to for the recovery of a matching failure.
		/// A distance of 0 indicates no recovery.
		/// </summary>
		public int RecoveryDistance
		{
			get;
			set;
		}

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
			RecoveryDistance = DEFAULT_RECOVERY_MATCHING_DISTANCE;
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
			RecoveryDistance = DEFAULT_RECOVERY_MATCHING_DISTANCE;
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <returns>The next token in the input</returns>
		public Token GetNextToken()
		{
			return tokens[GetNextToken(DEFAULT_CONTEXT_PROVIDER).Index];
		}

		/// <summary>
		/// Runs the lexer's DFA to match a terminal in the input ahead
		/// </summary>
		/// <param name="index">The current start index in the input text</param>
		/// <returns>The matching DFA state and length</returns>
		internal TokenMatch RunDFA(int index)
		{
			if (text.IsEnd(index))
			{
				// At the end of input
				// The only terminal matched at state index 0 is $
				return new TokenMatch(0, 0);
			}

			TokenMatch result = new TokenMatch(0);
			int state = 0;
			int i = index;

			while (state != Automaton.DEAD_STATE)
			{
				AutomatonState stateData = automaton.GetState(state);
				// Is this state a matching state ?
				if (stateData.TerminalsCount != 0)
					result = new TokenMatch(state, i - index);
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

		/// <summary>
		/// When an error was encountered, runs the lexer's DFA to match a terminal in the input ahead
		/// </summary>
		/// <param name="originIndex">The current start index in the input text</param>
		/// <returns>The matching DFA state and length</returns>
		internal TokenMatch RunDFAOnError(int originIndex)
		{
			if (RecoveryDistance <= 0)
			{
				OnError(new UnexpectedCharError(text.GetValue(originIndex).ToString(), text.GetPositionAt(originIndex)));
				return new TokenMatch(1);
			}
			else
			{
				int index = -1;
				// index of the separator terminal, if any
				for (int i = 0; i != symTerminals.Count; i++)
				{
					if (symTerminals[i].ID == separatorID)
					{
						index = i;
						break;
					}
				}
				FuzzyMatcher handler = new FuzzyMatcher(automaton, index, text, OnError, RecoveryDistance, originIndex);
				return handler.Run();
			}
		}

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <param name="contexts">The current applicable contexts</param>
		/// <returns>The next token in the input</returns>
		internal abstract TokenKernel GetNextToken(IContextProvider contexts);
	}
}
