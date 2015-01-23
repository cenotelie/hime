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
using System.Text;
using Hime.Redist.Utils;

namespace Hime.SDK.Output
{
	/// <summary>
	/// Represent an output unit, i.e. a grammar with its compilation parameters
	/// </summary>
	public class Unit
	{
		/// <summary>
		/// The represented grammar
		/// </summary>
		private Grammars.Grammar grammar;
		/// <summary>
		/// The parsing method to use
		/// </summary>
		private ParsingMethod method;
		/// <summary>
		/// The namespace for the generated code
		/// </summary>
		private string nmspace;
		/// <summary>
		/// The access modifier for the generated code
		/// </summary>
		private Modifier modifier;
		/// <summary>
		/// The DFA to emit in a lexer
		/// </summary>
		private Automata.DFA dfa;
		/// <summary>
		/// The seperator terminal
		/// </summary>
		private Grammars.Terminal separator;
		/// <summary>
		/// The terminals matched by the DFA and expected by the parser
		/// </summary>
		private List<Grammars.Terminal> expected;
		/// <summary>
		/// The available contexts for the lexer
		/// </summary>
		private List<Grammars.Variable> contexts;
		/// <summary>
		/// The LR graph to emit in a parser
		/// </summary>
		private Grammars.LR.Graph graph;

		/// <summary>
		/// Gets the unit's name
		/// </summary>
		public string Name { get { return grammar.Name; } }
		/// <summary>
		/// Gets the represented grammar
		/// </summary>
		public Grammars.Grammar Grammar { get { return grammar; } }
		/// <summary>
		/// Gets the associated parsing method to generate code against
		/// </summary>
		public ParsingMethod Method { get { return method; } }
		/// <summary>
		/// Gets the namespace to use for the generated code
		/// </summary>
		public string Namespace { get { return nmspace == null ? grammar.Name : nmspace; } }
		/// <summary>
		/// Gets the visibility modifier to use for the generated code
		/// </summary>
		public Modifier Modifier { get { return modifier; } }
		/// <summary>
		/// Gets the DFA for this unit
		/// </summary>
		public Automata.DFA DFA { get { return dfa; } }
		/// <summary>
		/// Gets the separator terminal for the associated lexer
		/// </summary>
		public Grammars.Terminal Separator { get { return separator; } }
		/// <summary>
		/// Gets the expected terminals produced by the associated lexer
		/// </summary>
		public ROList<Grammars.Terminal> Expected { get { return new ROList<Grammars.Terminal>(expected); } }
		/// <summary>
		/// Gets the contexts supported by the associated lexer
		/// </summary>
		public ROList<Grammars.Variable> Contexts { get { return new ROList<Grammars.Variable>(contexts); } }
		/// <summary>
		/// Gets the LR graph for the associated parser
		/// </summary>
		public Grammars.LR.Graph Graph { get { return graph; } }

		/// <summary>
		/// Initializes this unit
		/// </summary>
		/// <param name="grammar">The represented grammar</param>
		/// <param name="method">The parsing method to use</param>
		/// <param name="nmspace">The namespace for the artifacts</param>
		/// <param name="modifier">The modifier for the artifacts</param>
		public Unit(Grammars.Grammar grammar, ParsingMethod method, string nmspace, Modifier modifier)
		{
			this.grammar = grammar;
			this.method = method;
			this.nmspace = nmspace;
			this.modifier = modifier;
		}

		/// <summary>
		/// Prepare this unit for artifact generation
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if the operation succeeded</returns>
		public bool Prepare(Reporter reporter)
		{
			reporter.Info("Preparing grammar " + grammar.Name + " ...");
			string message = grammar.Prepare();
			if (message != null)
			{
				reporter.Error(message);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Builds the expected terminals and the contexts for the lexer
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if the operation succeeded</returns>
		private bool BuildExpected(Reporter reporter)
		{
			bool errors = false;
			expected = new List<Grammars.Terminal>();
			expected.Add(Grammars.Epsilon.Instance);
			expected.Add(Grammars.Dollar.Instance);
			contexts = new List<Grammars.Variable>();
			contexts.Add(null); // the default context
			foreach (Automata.DFAState state in dfa.States)
			{
				List<Grammars.Variable> foundContexts = new List<Grammars.Variable>();
				foreach (Automata.FinalItem item in state.Items)
				{
					Grammars.Terminal terminal = item as Grammars.Terminal;
					if (expected.Contains(terminal))
						break;
					Grammars.Variable context = terminal.Context == null ? null : grammar.GetVariable(terminal.Context);
					if (context == null && terminal.Context != null)
					{
						reporter.Error(string.Format("Terminal {0} is in unknown context {1} (not the name of a variable)", terminal, terminal.Context));
						errors = true;
					}
					if (!foundContexts.Contains(context))
					{
						// this is the first time this context is found in the current DFA state
						// this is the terminal with the most priority for this context
						expected.Add(terminal);
						foundContexts.Add(context);
						if (!contexts.Contains(context))
							contexts.Add(context);
					}
				}
			}
			return !errors;
		}

		/// <summary>
		/// Builds the separator terminal information
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if the operation succeeded</returns>
		private bool BuildSeparator(Reporter reporter)
		{
			string name = grammar.GetOption(Grammars.Grammar.optionSeparator);
			if (name == null)
			{
				// no separator defined ... this is ok
				separator = null;
				return true;
			}
			separator = grammar.GetTerminalByName(name);
			if (separator == null)
			{
				// but the separator could not be found ...
				reporter.Error(string.Format("Terminal {0} specified as the separator is undefined", name));
				return false;
			}
			// warn if the separator is context-sensitive
			if (separator.Context != null)
				reporter.Warn(string.Format("Terminal {0} specified as the separator has non-default context {1}", name, separator.Context));
			// if the separator is among the expected terminals, then everything is find
			if (expected.Contains(separator))
				return true;
			// the separator will not be produced by the lexer, try to investigate why
			List<Grammars.Terminal> supercedings = new List<Grammars.Terminal>();
			foreach (Automata.DFAState state in dfa.States)
			{
				if (state.Items.Contains(separator))
				{
					foreach (Grammars.Terminal item in state.Items)
					{
						if (item == separator)
							break;
						if (item.Context != separator.Context)
							continue;
						if (!supercedings.Contains(item))
							supercedings.Add(item);
					}
				}
			}
			if (supercedings.Count == 0)
			{
				reporter.Error(string.Format("Terminal {0} specified as the separator cannot be matched", separator));
			}
			else
			{
				foreach (Automata.FinalItem superceding in supercedings)
					reporter.Error(string.Format("Terminal {0} specified as the separator cannot be matched, it is superceded by {1}", separator, superceding));
			}
			return false;
		}

		/// <summary>
		/// Builds the lexer's data
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		public bool BuildLexerData(Reporter reporter)
		{
			reporter.Info("Preparing " + grammar.Name + " lexer's data ...");
			// build the lexer's dfa
			dfa = grammar.BuildDFA();
			// check well-formedness
			foreach (Automata.FinalItem item in dfa.Entry.Items)
				reporter.Error(string.Format("Terminal {0} can be an empty string, this is forbidden", item.ToString()));
			if (dfa.Entry.IsFinal)
				return false;
			// build the expected terminals
			if (!BuildExpected(reporter))
				return false;
			// build the separator
			return BuildSeparator(reporter);
		}

		/// <summary>
		/// Builds the parser's data
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		public bool BuildParserData(Reporter reporter)
		{
			// build the LR graph
			reporter.Info("Preparing " + grammar.Name + " parser's data ...");
			Grammars.LR.Builder builder = new Grammars.LR.Builder(grammar);
			graph = builder.Build(method);
			// report the conflicts
			if (method == ParsingMethod.RNGLR1 || method == ParsingMethod.RNGLALR1)
			{
				// for RNGLR method we only warn of existing conflicts
				if (builder.Conflicts.Count > 0)
					reporter.Warn(string.Format("Found {0} conflict(s), use debug output mode for the details", builder.Conflicts.Count));
			}
			else
			{
				// for LR(k) methods, conflicts prevent the generation of the automaton
				if (builder.Conflicts.Count > 0)
				{
					foreach (Grammars.LR.Conflict conflict in builder.Conflicts)
						reporter.Error(conflict);
					return false;
				}
			}
			return true;
		}
	}
}