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
using System.IO;
using Hime.Redist.Parsers;

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
		private Grammars.LR.Graph graph;
		/// <summary>
		/// The terminals matched by the associated lexer
		/// </summary>
		private ROList<Grammars.Terminal> terminals;
		/// <summary>
		/// The existing contexts in the parser
		/// </summary>
		private ROList<Grammars.Variable> contexts;
		/// <summary>
		/// The variables to be exported
		/// </summary>
		private List<Grammars.Variable> variables;
		/// <summary>
		/// The virtual symbols to be exported
		/// </summary>
		private List<Grammars.Virtual> virtuals;
		/// <summary>
		/// The action symbols to be exported
		/// </summary>
		private List<Grammars.Action> actions;
		/// <summary>
		/// The grammar rules
		/// </summary>
		private List<Grammars.Rule> rules;

		/// <summary>
		/// Initializes this parser generator
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		public ParserLRkDataGenerator(Unit unit)
		{
			this.graph = unit.Graph;
			this.terminals = unit.Expected;
			this.contexts = unit.Contexts;
			this.variables = new List<Grammars.Variable>(unit.Grammar.Variables);
			this.virtuals = new List<Grammars.Virtual>(unit.Grammar.Virtuals);
			this.actions = new List<Grammars.Action>(unit.Grammar.Actions);
			this.rules = new List<Grammars.Rule>(unit.Grammar.Rules);
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

			foreach (Grammars.Terminal terminal in terminals)
				writer.Write((ushort)terminal.ID);
			foreach (Grammars.Variable variable in variables)
				writer.Write((ushort)variable.ID);

			foreach (Grammars.LR.State state in graph.States)
				GenerateDataContexts(writer, state);

			foreach (Grammars.LR.State state in graph.States)
				GenerateDataLRTable(writer, state);

			foreach (Grammars.Rule rule in rules)
				GenerateDataProduction(writer, rule);

			writer.Close();
		}

		/// <summary>
		/// Generates the parser's binary data for the contexts
		/// </summary>
		/// <param name="writer">The output writer</param>
		/// <param name="state">The LR state</param>
		private void GenerateDataContexts(BinaryWriter writer, Grammars.LR.State state)
		{
			// retrieve the contexts
			List<ushort> result = new List<ushort>();
			foreach (Grammars.LR.Item item in state.Items)
			{
				if (item.DotPosition == 0)
				{
					Grammars.Variable head = item.BaseRule.Head;
					int index = contexts.IndexOf(head);
					if (index != -1)
						result.Add((ushort) index);
				}
			}
			
			// output
			writer.Write((ushort) result.Count);
			for (int i = 0; i != result.Count; i++)
				writer.Write(result[i]);
		}

		/// <summary>
		/// Generates the parser's binary data for the provided LR state
		/// </summary>
		/// <param name="writer">The output writer</param>
		/// <param name="state">The LR state</param>
		private void GenerateDataLRTable(BinaryWriter writer, Grammars.LR.State state)
		{
			Dictionary<Grammars.Terminal, Grammars.Rule> reductions = new Dictionary<Grammars.Terminal, Grammars.Rule>();
			foreach (Grammars.LR.StateActionReduce reduction in state.Reductions)
				reductions.Add(reduction.Lookahead, reduction.ToReduceRule);
			// write action on epsilon
			if (reductions.ContainsKey(Grammars.Epsilon.Instance) || reductions.ContainsKey(Grammars.NullTerminal.Instance))
				writer.Write((ushort)LRActionCode.Accept);
			else
				writer.Write((ushort)LRActionCode.None);
			writer.Write((ushort)LRActionCode.None);
			// write actions for terminals
			for (int i = 1; i != terminals.Count; i++)
			{
				Grammars.Terminal t = terminals[i];
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
				else if (reductions.ContainsKey(Grammars.NullTerminal.Instance))
				{
					writer.Write((ushort)LRActionCode.Reduce);
					writer.Write((ushort)rules.IndexOf(reductions[Grammars.NullTerminal.Instance]));
				}
				else
				{
					writer.Write((ushort)LRActionCode.None);
					writer.Write((ushort)LRActionCode.None);
				}
			}
			// write actions for variables
			foreach (Grammars.Variable variable in variables)
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
		private void GenerateDataProduction(BinaryWriter writer, Grammars.Rule rule)
		{
			writer.Write((ushort)variables.IndexOf(rule.Head));
			if (rule.IsGenerated)
				writer.Write((byte)Hime.Redist.TreeAction.Replace);
			else
				writer.Write((byte)Hime.Redist.TreeAction.None);
			writer.Write((byte)rule.Body.Choices[0].Length);
			byte length = 0;
			foreach (Grammars.RuleBodyElement elem in rule.Body)
			{
				if (elem.Symbol is Grammars.Virtual || elem.Symbol is Grammars.Action)
					length += 2;
				else
					length += 1;
			}
			writer.Write(length);
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
				else
				{
					if (elem.Action == Hime.Redist.TreeAction.Drop)
						writer.Write((ushort)LROpCodeValues.PopStackDrop);
					else if (elem.Action == Hime.Redist.TreeAction.Promote)
						writer.Write((ushort)LROpCodeValues.PopStackPromote);
					else
						writer.Write((ushort)LROpCodeValues.PopStackNoAction);
				}
			}
		}
	}
}
