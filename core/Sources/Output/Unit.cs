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

namespace Hime.CentralDogma.Output
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
		/// Builds and get the DFA for this unit
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		public bool BuildDFA(Reporter reporter)
		{
			reporter.Info("Preparing " + grammar.Name + " lexer's data ...");
			// build the lexer's dfa
			dfa = grammar.BuildDFA();
			
			// build the expected
			expected = new List<Grammars.Terminal>();
			expected.Add(Grammars.Epsilon.Instance);
			expected.Add(Grammars.Dollar.Instance);
			foreach (Automata.DFAState state in dfa.States)
			{
				if (state.TopItem != null)
				{
					if (!expected.Contains(state.TopItem as Grammars.Terminal))
						expected.Add(state.TopItem as Grammars.Terminal);
				}	
			}

			// perform diagnostics
			bool result = true;
			result &= CheckDFAWellformedness(reporter);
			result &= CheckSeparatorAxiom(reporter);
			result &= CheckRulesAgainstExpectedTerminals(reporter);
			return result;
		}

		/// <summary>
		/// Checks the wellformed-ness of the DFA
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if everything is OK</returns>
		private bool CheckDFAWellformedness(Reporter reporter)
		{
			foreach (Automata.FinalItem item in dfa.Entry.Items)
				reporter.Error(string.Format("Terminal {0} can be an empty string, this is forbidden", item.ToString()));
			return (dfa.Entry.TopItem == null);
		}

		/// <summary>
		/// Checks the wellformed-ness of the Separator axiom against the DFA
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if everything is OK</returns>
		private bool CheckSeparatorAxiom(Reporter reporter)
		{
			string name = grammar.GetOption(Grammars.Grammar.optionSeparator);
			separator = name != null ? grammar.GetTerminalByName(name) : null;
			if (name == null)
				return true;
			// a separator is defined
			if (separator == null)
			{
				// but could not be found ...
				reporter.Error(string.Format("Terminal {0} specified as the separator is undefined", name));
				return false;
			}
			// look for the separator in the dfa
			bool found = false;
			Automata.FinalItem superceding = null;
			foreach (Automata.DFAState state in dfa.States)
			{
				if (state.TopItem == separator)
				{
					found = true;
					break;
				}
				else if (state.Items.Contains(separator))
				{
					superceding = state.TopItem;
				}
			}
			if (!found)
			{
				if (superceding != null)
					reporter.Error(string.Format("Terminal {0} defined as the separator cannot be matched, it is superceded by {1}", separator, superceding));
				else
					reporter.Error(string.Format("Terminal {0} defined as the separator cannot be matched", separator));
			}
			return found;
		}

		/// <summary>
		/// Checks the wellformed-ness of the syntactic rules against the DFA
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if everything is OK</returns>
		private bool CheckRulesAgainstExpectedTerminals(Reporter reporter)
		{
			List<Grammars.Terminal> inError = new List<Grammars.Terminal>();
			foreach (Grammars.Rule rule in grammar.Rules)
			{
				Grammars.RuleChoice choice = rule.Body.Choices[0];
				for (int i = 0; i != choice.Length; i++)
				{
					Grammars.RuleBodyElement element = choice[i];
					if (element.Symbol is Grammars.Terminal)
					{
						Grammars.Terminal terminal = element.Symbol as Grammars.Terminal;
						if (!expected.Contains(terminal) && !inError.Contains(terminal))
						{
							List<Grammars.Terminal> supercedings = GetSupercedingsOf(dfa, terminal);
							StringBuilder builder = new StringBuilder("Terminal ");
							builder.Append(terminal.Value);
							builder.Append(" is used in a syntactic rule but is not produced by the lexer, it is superceded by { ");
							for (int j = 0; j != supercedings.Count; j++)
							{
								if (j != 0)
									builder.Append(", ");
								builder.Append(supercedings[j].Value);
							}
							builder.Append(" }");
							reporter.Error(builder.ToString());
							inError.Add(terminal);
						}
					}
				}
			}
			return (inError.Count == 0);
		}

		/// <summary>
		/// Gets all the superceding terminals of the specified one in a DFA
		/// </summary>
		/// <param name="dfa">A DFA</param>
		/// <param name="terminal">A terminal</param>
		/// <returns>All terminals that supercedes the specified one at least once</returns>
		private List<Grammars.Terminal> GetSupercedingsOf(Automata.DFA dfa, Grammars.Terminal terminal)
		{
			List<Grammars.Terminal> result = new List<Grammars.Terminal>();
			foreach (Automata.DFAState state in dfa.States)
			{
				if (state.Items.Contains(terminal) && state.TopItem != terminal)
				{
					Grammars.Terminal superceding = state.TopItem as Grammars.Terminal;
					if (!result.Contains(superceding))
						result.Add(superceding);
				}
			}
			return result;
		}

		/// <summary>
		/// Builds and gets the graph.
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		public bool BuildGraph(Reporter reporter)
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