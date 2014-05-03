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

namespace Hime.CentralDogma.Output
{
	/// <summary>
	/// Represents a generator of data and code for a LR(k) parser
	/// </summary>
	public class ParserGeneratorLRk : ParserGenerator
	{
		/// <summary>
		/// Initializes this parser generator
		/// </summary>
		/// <param name="gram">The grammar to generate a parser for</param>
		/// <param name="graph">The LR graph to use</param>
		/// <param name="expected">The terminals matched by the associated lexer</param>
		public ParserGeneratorLRk(Grammars.Grammar gram, Grammars.LR.Graph graph, ROList<Grammars.Terminal> expected)
			 : base(gram, graph, expected)
		{
		}

		/// <summary>
		/// Gets the name of the base parser class for the code generation
		/// </summary>
		protected override string BaseClassName { get { return "LRkParser"; } }

		/// <summary>
		/// Generates the code for the automaton
		/// </summary>
		/// <param name="stream">The output stream</param>
		/// <param name="name">The parser's name</param>
		/// <param name="resource">The name of the associated binary data resource</param>
		protected override void GenerateCodeAutomaton(StreamWriter stream, string name, string resource)
		{
			stream.WriteLine("\t\tprivate static readonly LRkAutomaton automaton = LRkAutomaton.Find(typeof(" + name + "Parser), \"" + resource + "\");");
		}

		/// <summary>
		/// Generates the parser's binary data
		/// </summary>
		/// <param name="stream">The output stream</param>
		public override void GenerateData(BinaryWriter stream)
		{
			stream.Write((ushort)(terminals.Count + variables.Count));  // Nb of columns
			stream.Write((ushort)graph.States.Count);                   // Nb or rows
			stream.Write((ushort)rules.Count);                          // Nb or rules

			foreach (Grammars.Terminal terminal in terminals)
				stream.Write((ushort)terminal.ID);
			foreach (Grammars.Variable variable in variables)
				stream.Write((ushort)variable.ID);

			foreach (Grammars.LR.State state in graph.States)
				GenerateDataLRTable(stream, state);

			foreach (Grammars.Rule rule in rules)
				GenerateDataProduction(stream, rule);
		}

		/// <summary>
		/// Generates the parser's binary data for the provided LR state
		/// </summary>
		/// <param name="stream">The output stream</param>
		/// <param name="state">The LR state</param>
		private void GenerateDataLRTable(BinaryWriter stream, Grammars.LR.State state)
		{
			Dictionary<Grammars.Terminal, Grammars.Rule> reductions = new Dictionary<Grammars.Terminal, Grammars.Rule>();
			foreach (Grammars.LR.StateActionReduce reduction in state.Reductions)
				reductions.Add(reduction.Lookahead, reduction.ToReduceRule);
			// write action on epsilon
			if (reductions.ContainsKey(Grammars.Epsilon.Instance) || reductions.ContainsKey(Grammars.NullTerminal.Instance))
				stream.Write((ushort)LRActionCode.Accept);
			else
				stream.Write((ushort)LRActionCode.None);
			stream.Write((ushort)LRActionCode.None);
			// write actions for terminals
			for (int i = 1; i != terminals.Count; i++)
			{
				Grammars.Terminal t = terminals[i];
				if (state.Children.ContainsKey(t))
				{
					stream.Write((ushort)LRActionCode.Shift);
					stream.Write((ushort)state.Children[t].ID);
				}
				else if (reductions.ContainsKey(t))
				{
					stream.Write((ushort)LRActionCode.Reduce);
					stream.Write((ushort)rules.IndexOf(reductions[t]));
				}
				else if (reductions.ContainsKey(Grammars.NullTerminal.Instance))
				{
					stream.Write((ushort)LRActionCode.Reduce);
					stream.Write((ushort)rules.IndexOf(reductions[Grammars.NullTerminal.Instance]));
				}
				else
				{
					stream.Write((ushort)LRActionCode.None);
					stream.Write((ushort)LRActionCode.None);
				}
			}
			// write actions for variables
			foreach (Grammars.Variable var in variables)
			{
				if (state.Children.ContainsKey(var))
				{
					stream.Write((ushort)LRActionCode.Shift);
					stream.Write((ushort)state.Children[var].ID);
				}
				else
				{
					stream.Write((ushort)LRActionCode.None);
					stream.Write((ushort)LRActionCode.None);
				}
			}
		}

		/// <summary>
		/// Generates the parser's binary representation of a rule production
		/// </summary>
		/// <param name="stream">The output stream</param>
		/// <param name="rule">A grammar rule</param>
		private void GenerateDataProduction(BinaryWriter stream, Grammars.Rule rule)
		{
			stream.Write((ushort)variables.IndexOf(rule.Head));
			if (rule.IsGenerated)
				stream.Write((byte)Hime.Redist.TreeAction.Replace);
			else
				stream.Write((byte)Hime.Redist.TreeAction.None);
			stream.Write((byte)rule.Body.Choices[0].Length);
			byte length = 0;
			foreach (Grammars.RuleBodyElement elem in rule.Body)
			{
				if (elem.Symbol is Grammars.Virtual || elem.Symbol is Grammars.Action)
					length += 2;
				else
					length += 1;
			}
			stream.Write(length);
			foreach (Grammars.RuleBodyElement elem in rule.Body)
			{
				if (elem.Symbol is Grammars.Virtual)
				{
					if (elem.Action == Hime.Redist.TreeAction.Drop)
						stream.Write((ushort)LROpCodeValues.AddVirtualDrop);
					else if (elem.Action == Hime.Redist.TreeAction.Promote)
						stream.Write((ushort)LROpCodeValues.AddVirtualPromote);
					else
						stream.Write((ushort)LROpCodeValues.AddVirtualNoAction);
					stream.Write((ushort)virtuals.IndexOf(elem.Symbol as Grammars.Virtual));
				}
				else if (elem.Symbol is Grammars.Action)
				{
					stream.Write((ushort)LROpCodeValues.SemanticAction);
					stream.Write((ushort)actions.IndexOf(elem.Symbol as Grammars.Action));
				}
				else
				{
					if (elem.Action == Hime.Redist.TreeAction.Drop)
						stream.Write((ushort)LROpCodeValues.PopStackDrop);
					else if (elem.Action == Hime.Redist.TreeAction.Promote)
						stream.Write((ushort)LROpCodeValues.PopStackPromote);
					else
						stream.Write((ushort)LROpCodeValues.PopStackNoAction);
				}
			}
		}
	}
}
