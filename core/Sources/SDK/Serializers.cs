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
using System.Text;
using Hime.Redist;
using Hime.CentralDogma.Automata;

namespace Hime.CentralDogma.SDK
{
	/// <summary>
	/// Common serializers for debugging purposes
	/// </summary>
	public static class Serializers
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

		/// <summary>
		/// Exports the given AST tree
		/// </summary>
		/// <param name="root">Root of the tree to export</param>
		/// <param name="file">File to export to</param>
		public static void Export(ASTNode root, string file)
		{
			System.IO.StreamWriter writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
			ExportNode(writer, "", root);
			writer.Close();
		}

		/// <summary>
		/// Exports the given AST node with the given serializer
		/// </summary>
		/// <param name="writer">The writer to export with</param>
		/// <param name="tab">The current indentation</param>
		/// <param name="node">The node to serialize</param>
		private static void ExportNode(System.IO.StreamWriter writer, string tab, ASTNode node)
		{
			writer.WriteLine(tab + node.ToString());
			foreach (Hime.Redist.ASTNode child in node.Children)
				ExportNode(writer, tab + "\t", child);
		}

		/// <summary>
		/// Exports the given AST tree to a DOT graph in the specified file
		/// </summary>
		/// <param name="root">Root of the tree to export</param>
		/// <param name="file">DOT file to export to</param>
		public static void ExportDOT(ASTNode root, string file)
		{
			DOTSerializer serializer = new DOTSerializer("CST", file);
			ExportNode(serializer, null, 0, root);
			serializer.Close();
		}

		/// <summary>
		/// Exports the given AST node with the given serializer
		/// </summary>
		/// <param name="serializer">The DOT serializer</param>
		/// <param name="parent">The parent node ID</param>
		/// <param name="nextID">The next available ID for the generated DOT data</param>
		/// <param name="node">The node to serialize</param>
		/// <returns>The next available ID for the generate DOT data</returns>
		private static int ExportNode(DOTSerializer serializer, string parent, int nextID, ASTNode node)
		{
			string name = "node" + nextID;
			string label = node.Symbol.ToString();
			serializer.WriteNode(name, label, DOTNodeShape.circle);
			if (parent != null)
				serializer.WriteEdge(parent, name, string.Empty);
			int result = nextID + 1;
			foreach (ASTNode child in node.Children)
				result = ExportNode(serializer, name, result, child);
			return result;
		}


		/// <summary>
		/// Exports the given DFA to a DOT graph in the specified file
		/// </summary>
		/// <param name="dfa">The DFA to export</param>
		/// <param name="file">DOT file to export to</param>
		public static void ExportDOT(DFA dfa, string file)
		{
			DOTSerializer serializer = new DOTSerializer("DFA", file);
			foreach (DFAState state in dfa.States)
			{
				if (state.IsFinal)
				{
					ROList<FinalItem> items = state.Items;
					StringBuilder builder = new StringBuilder(state.ID.ToString());
					builder.Append(" : ");
					for (int i = 0; i != items.Count; i++)
					{
						if (i != 0)
							builder.Append(", ");
						builder.Append(items[i].ToString());
					}
					serializer.WriteNode("state" + state.ID.ToString(), builder.ToString(), DOTNodeShape.doubleoctagon);
				}
				else
				{
					serializer.WriteNode("state" + state.ID.ToString(), state.ID.ToString());
				}
			}
			foreach (DFAState state in dfa.States)
				foreach (CharSpan value in state.Transitions)
					serializer.WriteEdge("state" + state.ID.ToString(), "state" + state.GetChildBy(value).ID.ToString(), value.ToString());
			serializer.Close();
		}

		/// <summary>
		/// Exports the given NFA to a DOT graph in the specified file
		/// </summary>
		/// <param name="nfa">The NFA to export</param>
		/// <param name="file">DOT file to export to</param>
		public static void ExportDOT(NFA nfa, string file)
		{
			DOTSerializer serializer = new DOTSerializer("DFA", file);
			for (int i=0; i!=nfa.States.Count; i++)
			{
				NFAState state = nfa.States[i];
				if (state.IsFinal)
				{
					ROList<FinalItem> items = state.Items;
					StringBuilder builder = new StringBuilder(i.ToString());
					builder.Append(" : ");
					for (int j = 0; i != items.Count; i++)
					{
						if (i != 0)
							builder.Append(", ");
						builder.Append(items[j].ToString());
					}
					serializer.WriteNode("state" + i.ToString(), builder.ToString(), DOTNodeShape.doubleoctagon);
				}
				else
				{
					serializer.WriteNode("state" + i.ToString(), i.ToString());
				}
			}
			for (int i=0; i!=nfa.States.Count; i++)
			{
				NFAState state = nfa.States[i];
				foreach (NFATransition transition in state.Transitions)
				{
					int to = nfa.States.IndexOf(transition.Next);
					serializer.WriteEdge("state" + i.ToString(), "state" + to.ToString(), transition.Span.ToString());
				}
			}
			serializer.Close();
		}

		/// <summary>
		/// Exports the given LR automaton to a DOT graph in the specified file
		/// </summary>
		/// <param name="automaton">The LR automaton to export</param>
		/// <param name="file">DOT file to export to</param>
		public static void ExportDOT(LRAutomaton automaton, string file)
		{
			DOTSerializer serializer = new DOTSerializer("LR", file);
			foreach (LRState state in automaton.States)
			{
				string[] items = new string[state.Reductions.Count];
				for (int i=0; i!=state.Reductions.Count; i++)
					items[i] = state.Reductions[i].ToString();
				string label = state.IsAccept ? state.ID.ToString() + "=Accept" : state.ID.ToString();
				serializer.WriteStructure("state" + state.ID.ToString(), label, items);
			}
			foreach (LRState state in automaton.States)
				foreach (LRTransition transition in state.Transitions)
					serializer.WriteEdge("state" + state.ID.ToString(), "state" + transition.Target.ID.ToString(), transition.Label.ToString());
			serializer.Close();
		}

		/// <summary>
		/// Exports the given LR automaton to a DOT graph in the specified file
		/// </summary>
		/// <param name="graph">The LR automaton to export</param>
		/// <param name="file">DOT file to export to</param>
		public static void ExportDOT(Grammars.LR.Graph graph, string file)
		{
			DOTSerializer serializer = new DOTSerializer("LR", file);
			foreach (Grammars.LR.State state in graph.States)
			{
				List<string> items = new List<string>();
				foreach (Grammars.LR.Item item in state.Items)
				{
					if (item.Action == Hime.Redist.Parsers.LRActionCode.Reduce)
						items.Add(item.ToString());
				}
				serializer.WriteStructure("state" + state.ID.ToString(), state.ID.ToString(), items.ToArray());
			}
			foreach (Grammars.LR.State state in graph.States)
				foreach (Grammars.Symbol symbol in state.Transitions)
					serializer.WriteEdge("state" + state.ID.ToString(), "state" + state.GetChildBy(symbol).ID.ToString(), symbol.ToString());
			serializer.Close();
		}
	}
}
