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

namespace Hime.CentralDogma.Output
{
	/// <summary>
	/// Represents a generator of data and code for a lexer
	/// </summary>
	public class LexerGenerator
	{
		/// <summary>
		/// The terminals matched by the lexer
		/// </summary>
		private List<Grammars.Terminal> terminals;
		/// <summary>
		/// The lexer's DFA
		/// </summary>
		private Automata.DFA dfa;
		/// <summary>
		/// The terminal that acts as a separator
		/// </summary>
		private Grammars.Terminal separator;

		/// <summary>
		/// Gets a list of the expected terminals
		/// </summary>
		public ROList<Grammars.Terminal> Expected { get { return new ROList<Grammars.Terminal>(terminals); } }

		/// <summary>
		/// Initializes this generator
		/// </summary>
		/// <param name="dfa">The dfa to serialize</param>
		/// <param name="separator">The separator terminal</param>
		public LexerGenerator(Automata.DFA dfa, Grammars.Terminal separator)
		{
			this.dfa = dfa;
			this.terminals = new List<Grammars.Terminal>();
			this.separator = separator;
			this.terminals.Add(Grammars.Epsilon.Instance);
			this.terminals.Add(Grammars.Dollar.Instance);
			foreach (Automata.DFAState state in dfa.States)
			{
				if (state.TopItem != null)
				{
					if (!terminals.Contains(state.TopItem as Grammars.Terminal))
						terminals.Add(state.TopItem as Grammars.Terminal);
				}	
			}
		}

		/// <summary>
		/// Generates the lexer's code
		/// </summary>
		/// <param name="stream">The output stream</param>
		/// <param name="name">The lexer's name</param>
		/// <param name="modifier">The lexer's visibility modifier</param>
		/// <param name="resource">The name of the associated binary data resource</param>
		public void GenerateCode(StreamWriter stream, string name, Modifier modifier, string resource)
		{
			stream.WriteLine("\t/// <summary>");
			stream.WriteLine("\t/// Represents a lexer");
			stream.WriteLine("\t/// </summary>");
			stream.WriteLine("\t" + modifier.ToString().ToLower() + " class " + name + "Lexer : Lexer");
			stream.WriteLine("\t{");

			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// The automaton for this lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\tprivate static readonly Automaton automaton = Automaton.Find(typeof(" + name + "Lexer), \"" + resource + "\");");

			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Contains the constant IDs for the terminals for this lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\tpublic sealed class ID");
			stream.WriteLine("\t\t{");
			for (int i = 2; i != terminals.Count; i++)
			{
				Grammars.Terminal terminal = terminals[i];
				if (terminal.Name.StartsWith(Grammars.Grammar.prefixGeneratedTerminal))
					continue;
				stream.WriteLine("\t\t\t/// <summary>");
				stream.WriteLine("\t\t\t/// The unique identifier for terminal " + terminal.Name);
				stream.WriteLine("\t\t\t/// </summary>");
				stream.WriteLine("\t\t\tpublic const int {0} = 0x{1};", terminal.Name, terminal.ID.ToString("X4"));
			}
			stream.WriteLine("\t\t}");

			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// The collection of terminals matched by this lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <remarks>");
			stream.WriteLine("\t\t/// The terminals are in an order consistent with the automaton,");
			stream.WriteLine("\t\t/// so that terminal indices in the automaton can be used to retrieve the terminals in this table");
			stream.WriteLine("\t\t/// </remarks>");
			stream.WriteLine("\t\tprivate static readonly Symbol[] terminals = {");
			bool first = true;
			foreach (Grammars.Terminal terminal in terminals)
			{
				if (!first)
					stream.WriteLine(",");
				stream.Write("\t\t\t");
				stream.Write("new Symbol(0x" + terminal.ID.ToString("X4") + ", \"" + terminal.ToString().Replace("\"", "\\\"") + "\")");
				first = false;
			}
			stream.WriteLine(" };");

			string sep = "FFFF";
			if (separator != null)
				sep = separator.ID.ToString("X4");
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Initializes a new instance of the lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <param name=\"input\">The lexer's input</param>");
			stream.WriteLine("\t\tpublic " + name + "Lexer(string input) : base(automaton, terminals, 0x" + sep + ", new System.IO.StringReader(input)) {}");

			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Initializes a new instance of the lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <param name=\"input\">The lexer's input</param>");
			stream.WriteLine("\t\tpublic " + name + "Lexer(System.IO.TextReader input) : base(automaton, terminals, 0x" + sep + ", input) {}");

			stream.WriteLine("\t}");
		}

		/// <summary>
		/// Generates the lexer's binary data
		/// </summary>
		/// <param name="stream">The output stream</param>
		public void GenerateData(BinaryWriter stream)
		{
			stream.Write((uint)dfa.StatesCount);
			uint offset = 0;
			foreach (Automata.DFAState state in dfa.States)
			{
				stream.Write(offset);
				offset += 3 + 256;
				foreach (CharSpan key in state.Transitions.Keys)
					if (key.End >= 256)
						offset += 3;
			}
			foreach (Automata.DFAState state in dfa.States)
				GenerateDataFor(stream, state);
		}

		/// <summary>
		/// Generates the given state binary data
		/// </summary>
		/// <param name="stream">The output stream</param>
		/// <param name="state">The state to export</param>
		private void GenerateDataFor(BinaryWriter stream, Automata.DFAState state)
		{
			ushort[] cache = new ushort[256];
			for (int i = 0; i != 256; i++)
				cache[i] = 0xFFFF;
			ushort cached = 0;
			ushort slow = 0;
			foreach (CharSpan span in state.Transitions.Keys)
			{
				if (span.Begin <= 255)
				{
					cached++;
					int end = span.End;
					if (end >= 256)
					{
						end = 255;
						slow++;
					}
					for (int i = span.Begin; i <= end; i++)
						cache[i] = (ushort)state.Transitions[span].ID;
				}
				else
					slow++;
			}

			if (state.TopItem != null)
				stream.Write((ushort)terminals.IndexOf(state.TopItem as Grammars.Terminal));
			else
				stream.Write((ushort)0xFFFF);
			stream.Write((ushort)(slow + cached));
			stream.Write(slow);

			for (int i = 0; i != 256; i++)
				stream.Write(cache[i]);

			List<CharSpan> keys = new List<CharSpan>(state.Transitions.Keys);
			keys.Sort(new System.Comparison<CharSpan>(CharSpan.CompareReverse));
			foreach (CharSpan span in keys)
			{
				if (span.End <= 255)
					break; // the rest of the transitions are in the cache
				ushort begin = span.Begin;
				if (begin <= 255)
					begin = 256;
				stream.Write(begin);
				stream.Write(System.Convert.ToUInt16(span.End));
				stream.Write((ushort)state.Transitions[span].ID);
				slow--;
			}
		}
	}
}
