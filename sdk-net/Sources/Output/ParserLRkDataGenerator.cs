/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System.Collections.Generic;
using System.IO;
using Hime.Redist;
using Hime.Redist.Parsers;
using Hime.Redist.Utils;
using Hime.SDK.Grammars;
using Hime.SDK.Grammars.LR;

namespace Hime.SDK.Output
{
	/// <summary>
	/// Represents a generator for LR(k) parser data for the .Net platform
	/// </summary>
	public class ParserLRkDataGenerator : Generator
	{
		/// <summary>
		/// LR graph for the parser
		/// </summary>
		private readonly Graph graph;
		/// <summary>
		/// The terminals matched by the associated lexer
		/// </summary>
		private readonly ROList<Terminal> terminals;
		/// <summary>
		/// The variables to be exported
		/// </summary>
		private readonly List<Variable> variables;
		/// <summary>
		/// The virtual symbols to be exported
		/// </summary>
		private readonly List<Virtual> virtuals;
		/// <summary>
		/// The action symbols to be exported
		/// </summary>
		private readonly List<Action> actions;
		/// <summary>
		/// The grammar rules
		/// </summary>
		private readonly List<Rule> rules;

		/// <summary>
		/// Initializes this parser generator
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		public ParserLRkDataGenerator(Unit unit)
		{
			graph = unit.Graph;
			terminals = unit.Expected;
			variables = new List<Variable>(unit.Grammar.Variables);
			virtuals = new List<Virtual>(unit.Grammar.Virtuals);
			actions = new List<Action>(unit.Grammar.Actions);
			rules = new List<Rule>(unit.Grammar.Rules);
			variables.Sort(new Grammars.Symbol.IdComparer<Variable>());
			virtuals.Sort(new Grammars.Symbol.IdComparer<Virtual>());
			actions.Sort(new Grammars.Symbol.IdComparer<Action>());
		}

		/// <summary>
		/// Generates the parser's binary data
		/// </summary>
		/// <param name="file">The file to output to</param>
		public void Generate(string file)
		{
			BinaryWriter writer = new BinaryWriter(new FileStream(file, FileMode.Create));

			writer.Write((ushort)(terminals.Count + variables.Count));  // Nb of columns
			writer.Write((ushort)graph.States.Count);                   // Nb or rows
			writer.Write((ushort)rules.Count);                          // Nb or rules

			foreach (Terminal terminal in terminals)
				writer.Write((ushort)terminal.ID);
			foreach (Variable variable in variables)
				writer.Write((ushort)variable.ID);

			foreach (State state in graph.States)
			{
				int count = 0;
				foreach (Hime.SDK.Grammars.Symbol symbol in state.Transitions)
				{
					Terminal terminal = symbol as Terminal;
					if (terminal != null)
						count += state.GetContextsOpenedBy(terminal).Count;
				}
				writer.Write((ushort)count);
				foreach (Hime.SDK.Grammars.Symbol symbol in state.Transitions)
				{
					Terminal terminal = symbol as Terminal;
					if (terminal != null)
					{
						foreach (int context in state.GetContextsOpenedBy(terminal))
						{
							writer.Write((ushort)terminal.ID);
							writer.Write((ushort)context);
						}
					}
				}
			}

			foreach (State state in graph.States)
				GenerateDataLRTable(writer, state);

			foreach (Rule rule in rules)
				GenerateDataProduction(writer, rule);

			writer.Close();
		}

		/// <summary>
		/// Generates the parser's binary data for the provided LR state
		/// </summary>
		/// <param name="writer">The output writer</param>
		/// <param name="state">The LR state</param>
		private void GenerateDataLRTable(BinaryWriter writer, State state)
		{
			Dictionary<Terminal, Rule> reductions = new Dictionary<Terminal, Rule>();
			foreach (StateActionReduce reduction in state.Reductions)
				reductions.Add(reduction.Lookahead, reduction.ToReduceRule);
			// write action on epsilon
			if (reductions.ContainsKey(Epsilon.Instance) || reductions.ContainsKey(NullTerminal.Instance))
				writer.Write((ushort)LRActionCode.Accept);
			else
				writer.Write((ushort)LRActionCode.None);
			writer.Write((ushort)LRActionCode.None);
			// write actions for terminals
			for (int i = 1; i != terminals.Count; i++)
			{
				Terminal t = terminals[i];
				if (state.HasTransition(t))
				{
					writer.Write((ushort)LRActionCode.Shift);
					writer.Write((ushort)state.GetChildBy(t).ID);
				}
				else if (reductions.ContainsKey(t))
				{
					writer.Write((ushort)LRActionCode.Reduce);
					writer.Write((ushort)rules.IndexOf(reductions[t]));
				}
				else if (reductions.ContainsKey(NullTerminal.Instance))
				{
					writer.Write((ushort)LRActionCode.Reduce);
					writer.Write((ushort)rules.IndexOf(reductions[NullTerminal.Instance]));
				}
				else
				{
					writer.Write((ushort)LRActionCode.None);
					writer.Write((ushort)LRActionCode.None);
				}
			}
			// write actions for variables
			foreach (Variable variable in variables)
			{
				if (state.HasTransition(variable))
				{
					writer.Write((ushort)LRActionCode.Shift);
					writer.Write((ushort)state.GetChildBy(variable).ID);
				}
				else
				{
					writer.Write((ushort)LRActionCode.None);
					writer.Write((ushort)LRActionCode.None);
				}
			}
		}

		/// <summary>
		/// Generates the parser's binary representation of a rule production
		/// </summary>
		/// <param name="writer">The output writer</param>
		/// <param name="rule">A grammar rule</param>
		private void GenerateDataProduction(BinaryWriter writer, Rule rule)
		{
			writer.Write((ushort)variables.IndexOf(rule.Head));
			writer.Write((byte)rule.HeadAction);
			writer.Write((byte)rule.Body.Choices[0].Length);
			byte length = 0;
			foreach (RuleBodyElement elem in rule.Body)
			{
				if (elem.Symbol is Virtual || elem.Symbol is Action)
					length += 2;
				else
					length += 1;
			}
			writer.Write(length);
			foreach (RuleBodyElement elem in rule.Body)
			{
				if (elem.Symbol is Virtual)
				{
					if (elem.Action == TreeAction.Drop)
						writer.Write((ushort)LROpCodeValues.AddVirtualDrop);
					else if (elem.Action == TreeAction.Promote)
						writer.Write((ushort)LROpCodeValues.AddVirtualPromote);
					else
						writer.Write((ushort)LROpCodeValues.AddVirtualNoAction);
					writer.Write((ushort)virtuals.IndexOf(elem.Symbol as Virtual));
				}
				else if (elem.Symbol is Action)
				{
					writer.Write((ushort)LROpCodeValues.SemanticAction);
					writer.Write((ushort)actions.IndexOf(elem.Symbol as Action));
				}
				else
				{
					if (elem.Action == TreeAction.Drop)
						writer.Write((ushort)LROpCodeValues.PopStackDrop);
					else if (elem.Action == TreeAction.Promote)
						writer.Write((ushort)LROpCodeValues.PopStackPromote);
					else
						writer.Write((ushort)LROpCodeValues.PopStackNoAction);
				}
			}
		}
	}
}
