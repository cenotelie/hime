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
using System.Reflection;
using Hime.Redist;

namespace Hime.CentralDogma.SDK
{
	/// <summary>
	/// Represents a marker for a matched terminal in a lexer DFA
	/// </summary>
	public class MatchedTerminal : Automata.FinalItem
	{
		/// <summary>
		/// The terminal represented by this marker
		/// </summary>
		private Symbol terminal;

		/// <summary>
		/// Gets the terminal repesented by this marker
		/// </summary>
		public Symbol Terminal { get { return terminal; } }

		/// <summary>
		/// Gets the priority of this marker
		/// </summary>
		public int Priority { get { return 0; } }

		/// <summary>
		/// Initializes this marker
		/// </summary>
		/// <param name="t">The matched terminal</param>
		public MatchedTerminal(Symbol t)
		{
			this.terminal = t;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.SDK.MatchedTerminal"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.SDK.MatchedTerminal"/>.
		/// </returns>
		public override string ToString()
		{
			return terminal.ToString();
		}
	}

	/// <summary>
	/// Utilities to decompile a lexer produced by Central Dogma
	/// </summary>
	public class LexerReflection
	{
		/// <summary>
		/// List of the terminals that can be matched
		/// </summary>
		private IList<Symbol> terminals;

		/// <summary>
		/// DFA of the lexer
		/// </summary>
		private Automata.DFA dfa;


		/// <summary>
		/// Gets the terminals that can be matched by this lexer
		/// </summary>
		public ROList<Symbol> Terminals { get { return new ROList<Symbol>(terminals); } }

		/// <summary>
		/// Gets the lexer's dfa
		/// </summary>
		public Automata.DFA DFA { get { return dfa; } }

		/// <summary>
		/// Initializes this lexer reflection
		/// </summary>
		/// <param name="lexerType">The lexer's type</param>
		public LexerReflection(System.Type lexerType)
		{
			string input = "";
			ConstructorInfo ctor = lexerType.GetConstructor(new System.Type[] { typeof(string) });
			Hime.Redist.Lexer.ILexer lexer = ctor.Invoke(new object[] { input }) as Hime.Redist.Lexer.ILexer;

			this.terminals = lexer.Terminals;
			this.dfa = new Automata.DFA();

			string[] resources = lexerType.Assembly.GetManifestResourceNames();
			Stream stream = null;
			foreach (string existing in resources)
			{
				if (existing.EndsWith(lexerType.Name + ".bin"))
				{
					stream = lexerType.Assembly.GetManifestResourceStream(existing);
					break;
				}
			}

			BinaryReader reader = new BinaryReader(stream);
			Hime.Redist.Lexer.Automaton automaton = new Hime.Redist.Lexer.Automaton(reader);
			reader.Close();

			for (int i=0; i!=automaton.StatesCount; i++)
				dfa.CreateState();
			for (int i=0; i!=automaton.StatesCount; i++)
			{
				Automata.DFAState current = this.dfa.States[i];
				int offset = automaton.GetOffsetOf(i);

				int terminal = automaton.GetStateRecognizedTerminal(offset);
				if (terminal != 0xFFFF)
					current.AddItem(new MatchedTerminal(this.terminals[terminal]));

				for (int j=0; j!=256; j++)
				{
					int next = automaton.GetStateCachedTransition(offset, j);
					char c = System.Convert.ToChar(j);
					if (next != 0xFFFF)
						current.AddTransition(new CharSpan(c, c), this.dfa.States[next]);
				}

				int nNonCached = automaton.GetStateBulkTransitionsCount(offset);
				for (int j=0; j!=nNonCached; j++)
				{
					int begin = 0;
					int end = 0;
					int next = automaton.GetStateBulkTransition(offset, j, out begin, out end);
					current.AddTransition(new CharSpan(System.Convert.ToChar(begin), System.Convert.ToChar(end)), this.dfa.States[next]);
				}
				current.RepackTransitions();
			}
		}
	}
}