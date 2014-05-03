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

namespace Hime.CentralDogma.SDK
{
	/// <summary>
	/// Convenience class for the export of debug information
	/// </summary>
	public class DebugSerializer
	{
		/// <summary>
		/// Export the content of the given grammar to the specified file
		/// </summary>
		/// <param name="grammar">The grammar to export</param>
		/// <param name="file">File to export to</param>
		public static void Export(Grammars.Grammar grammar, string file)
		{
			System.IO.StreamWriter writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
			writer.WriteLine("Name: {0}", grammar.Name);
			writer.WriteLine("Options:");
			foreach (string option in grammar.Options)
				writer.WriteLine("\t{0} = \"{1}\"", option, grammar.GetOption(option));

			writer.WriteLine("Terminals:");
			List<Grammars.Terminal> terminals = new List<Grammars.Terminal>(grammar.Terminals);
			terminals.Sort(new System.Comparison<Grammars.Terminal>(CompareTerminal));
			foreach (Grammars.Terminal terminal in terminals)
				writer.WriteLine("\t{0} = {1}", terminal.Name, terminal.ToString());

			writer.WriteLine("Rules:");
			List<Grammars.Variable> variables = new List<Grammars.Variable>(grammar.Variables);
			variables.Sort(new System.Comparison<Grammars.Variable>(CompareVariable));
			foreach (Grammars.Variable variable in variables)
				foreach (Grammars.Rule rule in variable.Rules)
					writer.WriteLine("\t{0}", rule.ToString());
			writer.Close();
		}

		/// <summary>
		/// Compares two terminals by name
		/// </summary>
		/// <param name="t1">Terminal one</param>
		/// <param name="t2">Terminal two</param>
		/// <returns>The ordinal comparison</returns>
		private static int CompareTerminal(Grammars.Terminal t1, Grammars.Terminal t2)
		{
			return t1.Name.CompareTo(t2.Name);
		}

		/// <summary>
		/// Compares two variables by name
		/// </summary>
		/// <param name="t1">Variable one</param>
		/// <param name="t2">Variable two</param>
		/// <returns>The ordinal comparison</returns>
		private static int CompareVariable(Grammars.Variable t1, Grammars.Variable t2)
		{
			return t1.Name.CompareTo(t2.Name);
		}

		/// <summary>
		/// Export the content of the given LR graph to the specified file
		/// </summary>
		/// <param name="graph">The LR graph to export</param>
		/// <param name="file">File to export to</param>
		public static void Export(Grammars.LR.Graph graph, string file)
		{
			System.IO.StreamWriter writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
			foreach (Grammars.LR.State state in graph.States)
				ExportLRState(writer, state);
			writer.Close();
		}

		/// <summary>
		/// Export the content of the given LR state to the specified writer
		/// </summary>
		/// <param name="writer">The writer to export with</param>
		/// <param name="state">The LR state to export</param>
		private static void ExportLRState(System.IO.StreamWriter writer, Grammars.LR.State state)
		{
			writer.WriteLine();
			writer.WriteLine("State {0}:", state.ID.ToString());
			writer.WriteLine("\tTransitions:");
			foreach (Grammars.Symbol symbol in state.Transitions)
				writer.WriteLine("\t\tOn {0} shift to {1}", symbol.ToString(), state.GetChildBy(symbol).ID.ToString());
			writer.WriteLine("\tItems:");
			foreach (Grammars.LR.Item item in state.Items)
				writer.WriteLine("\t\t" + item.ToString(true));
			writer.WriteLine("\tConflicts:");
			foreach (Grammars.LR.Conflict conflict in state.Conflicts)
				ExportLRConflict(writer, conflict);
		}

		/// <summary>
		/// Export the content of the given LR conflict to the specified writer
		/// </summary>
		/// <param name="writer">The writer to export with</param>
		/// <param name="conflict">The LR conflict to export</param>
		private static void ExportLRConflict(System.IO.StreamWriter writer, Grammars.LR.Conflict conflict)
		{
			writer.WriteLine("\t\tConflict {0} on {1}:", conflict.ConflictType, conflict.ConflictSymbol);
			writer.WriteLine("\t\t\tItems:");
			foreach (Grammars.LR.Item item in conflict.Items)
				writer.WriteLine("\t\t\t\t" + item.ToString());
			writer.WriteLine("\t\t\tExamples:");
			foreach (Grammars.Phrase example in conflict.Examples)
				writer.WriteLine("\t\t\t\t" + example.ToString());
		}
	}
}