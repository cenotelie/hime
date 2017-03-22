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
using Hime.Redist.Parsers;
using Hime.Redist.Utils;

namespace Hime.SDK.Output
{
	/// <summary>
	/// Represents a generator for RNGLR parser data for the .Net platform
	/// </summary>
	public class ParserRNGLRDataGenerator : Generator
	{
		/// <summary>
		/// The grammar to generate a parser for
		/// </summary>
		private readonly Grammars.Grammar grammar;
		/// <summary>
		/// LR graph for the parser
		/// </summary>
		private readonly Grammars.LR.Graph graph;
		/// <summary>
		/// The terminals matched by the associated lexer
		/// </summary>
		private readonly ROList<Grammars.Terminal> terminals;
		/// <summary>
		/// The variables to be exported
		/// </summary>
		private readonly List<Grammars.Variable> variables;
		/// <summary>
		/// The virtual symbols to be exported
		/// </summary>
		private readonly List<Grammars.Virtual> virtuals;
		/// <summary>
		/// The action symbols to be exported
		/// </summary>
		private readonly List<Grammars.Action> actions;

		/// <summary>
		/// Initializes this parser generator
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		public ParserRNGLRDataGenerator(Unit unit)
		{
			grammar = unit.Grammar;
			graph = unit.Graph;
			terminals = unit.Expected;
			variables = new List<Grammars.Variable>(unit.Grammar.Variables);
			virtuals = new List<Grammars.Virtual>(unit.Grammar.Virtuals);
			actions = new List<Grammars.Action>(unit.Grammar.Actions);
		}

		/// <summary>
		/// Generates the parser's binary data
		/// </summary>
		/// <param name="file">The file to output to</param>
		public void Generate(string file)
		{
			// complete list of rules, including new ones for the right-nullable parts
			List<KeyValuePair<Grammars.Rule, int>> rules = new List<KeyValuePair<Grammars.Rule, int>>();
			// index of the nullable rule for the variable with the same index
			List<int> nullables = new List<int>();

			foreach (Grammars.Variable variable in grammar.Variables)
			{
				// temporary list for this variable
				List<KeyValuePair<Grammars.Rule, int>> temp = new List<KeyValuePair<Grammars.Rule, int>>();
				foreach (Grammars.Rule rule in variable.Rules)
				{
					// Add normal rule
					temp.Add(new KeyValuePair<Grammars.Rule, int>(rule, rule.Body.Choices[0].Length));
					// Look for right-nullable choices
					for (int i = 1; i < rule.Body.Choices[0].Length; i++)
						if (rule.Body.Choices[i].Firsts.Contains(Grammars.Epsilon.Instance))
							temp.Add(new KeyValuePair<Grammars.Rule, int>(rule, i));
				}
				int nullIndex = 0xFFFF;
				// nullable variable?
				if (variable.Firsts.Contains(Grammars.Epsilon.Instance))
				{

					// look for a nullable rule
					for (int i = 0; i != temp.Count; i++)
					{
						if (temp[i].Value == 0)
						{
							// Found a 0-length reduction rule => perfect
							nullIndex = rules.Count + i;
							break;
						}
					}
					if (nullIndex == 0xFFFF)
					{
						// no 0-length reduction rule => create a new 0-length reduction with a nullable rule
						for (int i = 0; i != temp.Count; i++)
						{
							Grammars.Rule rule = temp[i].Key;
							if (rule.Body.Choices[0].Firsts.Contains(Grammars.Epsilon.Instance))
							{
								temp.Add(new KeyValuePair<Grammars.Rule, int>(rule, 0));
								nullIndex = rules.Count + temp.Count - 1;
								break;
							}
						}
					}

				}
				// commit
				nullables.Add(nullIndex);
				rules.AddRange(temp);
			}

			uint total = 0;
			List<uint> offsets = new List<uint>(); // for each state, the offset in the action table
			List<ushort> counts = new List<ushort>(); // for each state, the number of actions
			foreach (Grammars.LR.State state in graph.States)
				total = GenerateDataOffsetTable(offsets, counts, total, state);

			BinaryWriter writer = new BinaryWriter(new FileStream(file, FileMode.Create));

			writer.Write((ushort)variables.IndexOf(grammar.GetVariable(grammar.GetOption(Grammars.Grammar.OPTION_AXIOM))));
			writer.Write((ushort)(terminals.Count + variables.Count));  // Nb of columns
			writer.Write((ushort)graph.States.Count);                   // Nb or rows
			writer.Write(total);                   						// Nb of actions
			writer.Write((ushort)rules.Count);                          // Nb of rules
			writer.Write((ushort)nullables.Count);                      // Nb of nullables

			foreach (Grammars.Terminal terminal in terminals)
				writer.Write((ushort)terminal.ID);
			foreach (Grammars.Variable variable in variables)
				writer.Write((ushort)variable.ID);

			foreach (Grammars.LR.State state in graph.States)
			{
				int count = 0;
				foreach (Grammars.Symbol symbol in state.Transitions)
				{
					Grammars.Terminal terminal = symbol as Grammars.Terminal;
					if (terminal != null)
						count += state.GetContextsOpenedBy(terminal).Count;
				}
				writer.Write((ushort)count);
				foreach (Grammars.Symbol symbol in state.Transitions)
				{
					Grammars.Terminal terminal = symbol as Grammars.Terminal;
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

			for (int i = 0; i != offsets.Count; i++)
			{
				writer.Write(counts[i]);
				writer.Write(offsets[i]);
			}

			foreach (Grammars.LR.State state in graph.States)
				GenerateDataActionTable(writer, rules, state);

			foreach (KeyValuePair<Grammars.Rule, int> pair in rules)
				GenerateDataProduction(writer, pair.Key, pair.Value);

			foreach (int index in nullables)
				writer.Write((ushort)index);

			writer.Close();
		}

		/// <summary>
		/// Builds the offset table for the RNGLR actions
		/// </summary>
		/// <param name="offsets">The offset table</param>
		/// <param name="counts">The cout table</param>
		/// <param name="total">The total length at this point</param>
		/// <param name="state">The state to inspect</param>
		/// <returns>The total length of the table so far</returns>
		private uint GenerateDataOffsetTable(List<uint> offsets, List<ushort> counts, uint total, Grammars.LR.State state)
		{
			Dictionary<Grammars.Terminal, int> reductionCounters = new Dictionary<Grammars.Terminal, int>();
			foreach (Grammars.LR.StateActionReduce reduce in state.Reductions)
			{
				if (reductionCounters.ContainsKey(reduce.Lookahead))
					reductionCounters[reduce.Lookahead] = reductionCounters[reduce.Lookahead] + 1;
				else
					reductionCounters.Add(reduce.Lookahead, 1);
			}
			foreach (Grammars.Terminal terminal in terminals)
			{
				int count = state.HasTransition(terminal) ? 1 : 0;
				if (reductionCounters.ContainsKey(terminal))
					count += reductionCounters[terminal];
				offsets.Add(total);
				counts.Add((ushort)count);
				total += (uint)count;
			}
			foreach (Grammars.Variable variable in variables)
			{
				int count = state.HasTransition(variable) ? 1 : 0;
				offsets.Add(total);
				counts.Add((ushort)count);
				total += (uint)count;
			}
			return total;
		}

		/// <summary>
		/// Generates the action table
		/// </summary>
		/// <param name="writer">The output writer</param>
		/// <param name="rules">The rules</param>
		/// <param name="state">The state to inspect</param>
		private void GenerateDataActionTable(BinaryWriter writer, List<KeyValuePair<Grammars.Rule, int>> rules, Grammars.LR.State state)
		{
			Dictionary<Grammars.Terminal, List<Grammars.LR.StateActionRNReduce>> reductions = new Dictionary<Grammars.Terminal, List<Grammars.LR.StateActionRNReduce>>();
			foreach (Grammars.LR.StateActionReduce reduce in state.Reductions)
			{
				if (!reductions.ContainsKey(reduce.Lookahead))
					reductions.Add(reduce.Lookahead, new List<Grammars.LR.StateActionRNReduce>());
				reductions[reduce.Lookahead].Add(reduce as Grammars.LR.StateActionRNReduce);
			}
			if (reductions.ContainsKey(Grammars.Epsilon.Instance))
			{
				// There can be only one reduction on epsilon
				writer.Write((ushort)LRActionCode.Accept);
				writer.Write((ushort)LRActionCode.None);
			}
			for (int i = 1; i != terminals.Count; i++)
			{
				Grammars.Terminal terminal = terminals[i];
				if (state.HasTransition(terminal))
				{
					writer.Write((ushort)LRActionCode.Shift);
					writer.Write((ushort)state.GetChildBy(terminal).ID);
				}
				if (reductions.ContainsKey(terminal))
				{
					foreach (Grammars.LR.StateActionRNReduce reduce in reductions[terminal])
					{
						writer.Write((ushort)LRActionCode.Reduce);
						writer.Write((ushort)rules.IndexOf(new KeyValuePair<Grammars.Rule, int>(reduce.ToReduceRule, reduce.ReduceLength)));
					}
				}
			}
			foreach (Grammars.Variable variable in variables)
			{
				if (state.HasTransition(variable))
				{
					writer.Write((ushort)LRActionCode.Shift);
					writer.Write((ushort)state.GetChildBy(variable).ID);
				}
			}
		}

		/// <summary>
		/// Generates the parser's binary representation of a rule production
		/// </summary>
		/// <param name="writer">The output writer</param>
		/// <param name="rule">A grammar rule</param>
		/// <param name="length">The reduction's length</param>
		private void GenerateDataProduction(BinaryWriter writer, Grammars.Rule rule, int length)
		{
			writer.Write((ushort)variables.IndexOf(rule.Head));
			if (rule.IsGenerated)
				writer.Write((byte)Hime.Redist.TreeAction.Replace);
			else
				writer.Write((byte)Hime.Redist.TreeAction.None);
			writer.Write((byte)length);
			byte bcl = 0;
			int pop = 0;
			foreach (Grammars.RuleBodyElement elem in rule.Body)
			{
				if (elem.Symbol is Grammars.Virtual || elem.Symbol is Grammars.Action)
					bcl += 2;
				else if (pop >= length)
					bcl += 2;
				else
				{
					bcl += 1;
					pop++;
				}
			}
			writer.Write(bcl);
			pop = 0;
			foreach (Grammars.RuleBodyElement elem in rule.Body)
			{
				if (elem.Symbol is Grammars.Virtual)
				{
					if (elem.Action == Hime.Redist.TreeAction.Drop)
						writer.Write((ushort)LROpCodeValues.AddVirtualDrop);
					else if (elem.Action == Hime.Redist.TreeAction.Promote)
						writer.Write((ushort)LROpCodeValues.AddVirtualPromote);
					else
						writer.Write((ushort)LROpCodeValues.AddVirtualNoAction);
					writer.Write((ushort)virtuals.IndexOf(elem.Symbol as Grammars.Virtual));
				}
				else if (elem.Symbol is Grammars.Action)
				{
					writer.Write((ushort)LROpCodeValues.SemanticAction);
					writer.Write((ushort)actions.IndexOf(elem.Symbol as Grammars.Action));
				}
				else if (pop >= length)
				{
					// Here the symbol must be a variable
					ushort index = (ushort)variables.IndexOf(elem.Symbol as Grammars.Variable);
					if (elem.Action == Hime.Redist.TreeAction.Drop)
						writer.Write((ushort)LROpCodeValues.AddNullVariableDrop);
					else if (elem.Action == Hime.Redist.TreeAction.Promote)
						writer.Write((ushort)LROpCodeValues.AddNullVariablePromote);
					else
						writer.Write((ushort)LROpCodeValues.AddNullVariableNoAction);
					writer.Write(index);
				}
				else
				{
					if (elem.Action == Hime.Redist.TreeAction.Drop)
						writer.Write((ushort)LROpCodeValues.PopStackDrop);
					else if (elem.Action == Hime.Redist.TreeAction.Promote)
						writer.Write((ushort)LROpCodeValues.PopStackPromote);
					else
						writer.Write((ushort)LROpCodeValues.PopStackNoAction);
					pop++;
				}
			}
		}
	}
}
