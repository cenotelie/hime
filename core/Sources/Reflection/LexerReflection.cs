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
using Hime.Redist.Lexer;
using Hime.Redist.Utils;
using Hime.SDK.Grammars;

namespace Hime.SDK.Reflection
{
	/// <summary>
	/// Utilities to decompile a lexer produced by Central Dogma
	/// </summary>
	public class LexerReflection
	{
		/// <summary>
		/// List of the terminals that can be matched
		/// </summary>
		private List<Terminal> terminals;
		/// <summary>
		/// DFA of the lexer
		/// </summary>
		private Automata.DFA dfa;

		/// <summary>
		/// Gets the terminals that can be matched by this lexer
		/// </summary>
		public ROList<Terminal> Terminals { get { return new ROList<Terminal>(terminals); } }

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
			ConstructorInfo ctor = lexerType.GetConstructor(new [] { typeof(string) });
			BaseLexer lexer = ctor.Invoke(new object[] { "" }) as BaseLexer;

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
			Automaton automaton = new Automaton(reader);
			reader.Close();

			LoadTerminals(lexer);
			LoadDFA(automaton);
		}

		/// <summary>
		/// Loads the terminals from the specified lexers
		/// </summary>
		/// <param name="lexer">The lexer to investigate</param>
		private void LoadTerminals(BaseLexer lexer)
		{
			terminals = new List<Terminal>();
			terminals.Add(Epsilon.Instance);
			terminals.Add(Dollar.Instance);
			ROList<Hime.Redist.Symbol> spec = lexer.Terminals;
			for (int i = 2; i != spec.Count; i++)
			{
				Hime.Redist.Symbol symbol = spec[i];
				terminals.Add(new Terminal(symbol.ID, symbol.Name, "", null, null));
			}
		}

		/// <summary>
		/// Loads the specified DFA automaton
		/// </summary>
		/// <param name="automaton">An automaton</param>
		private void LoadDFA(Automaton automaton)
		{
			dfa = new Automata.DFA();
			for (int i = 0; i != automaton.StatesCount; i++)
				dfa.CreateState();
			for (int i = 0; i != automaton.StatesCount; i++)
			{
				Automata.DFAState current = dfa.States[i];
				AutomatonState stateData = automaton.GetState(i);
				// retrieve the matched terminals
				for (int j = 0; j != stateData.TerminalsCount; j++)
				{
					MatchedTerminal mt = stateData.GetTerminal(j);
					Terminal terminal = terminals[mt.Index];
					current.AddItem(terminal);
					if (mt.Context != 0 && terminal.Context == null)
						terminals[mt.Context].Context = mt.Context.ToString();
				}
				// retrieve the transitions
				for (int j = 0; j != 256; j++)
				{
					int next = stateData.GetCachedTransition(j);
					char c = System.Convert.ToChar(j);
					if (next != Automaton.DEAD_STATE)
						current.AddTransition(new CharSpan(c, c), dfa.States[next]);
				}

				for (int j = 0; j != stateData.BulkTransitionsCount; j++)
				{
					AutomatonTransition transition = stateData.GetBulkTransition(j);
					current.AddTransition(new CharSpan(System.Convert.ToChar(transition.Start), System.Convert.ToChar(transition.End)), dfa.States[transition.Target]);
				}
				current.RepackTransitions();
			}
		}
	}
}