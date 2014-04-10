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
using Hime.Redist;

namespace Hime.CentralDogma.SDK
{
	/// <summary>
	/// Represents a marker for a matched terminal in a lexer DFA
	/// </summary>
	public class MatchedTerminal : Automata.FinalItem
	{
		private Symbol terminal;

		/// <summary>
		/// Gets the terminal repesented by this marker
		/// </summary>
		/// <value>
		public Symbol Terminal { get { return terminal; } }

		/// <summary>
    	/// Gets the priority of this marker
    	/// </summary>
		public int Priority { get { return 0; } }

		/// <summary>
		/// Initializes this marker
		/// </summary>
		/// <param name="t">The matched terminal</param>
		public MatchedTerminal(Symbol t)
		{
			this.terminal = t;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.SDK.MatchedTerminal"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.SDK.MatchedTerminal"/>.
		/// </returns>
		public override string ToString()
		{
			return terminal.ToString();
		}
	}

	/// <summary>
	/// Utilities to decompile a lexer produced by Central Dogma
	/// </summary>
	public class LexerReflection
	{
		/// <summary>
		/// List of the terminals that can be matched
		/// </summary>
		private List<Symbol> terminals;

		/// <summary>
		/// DFA of the lexer
		/// </summary>
		private Automata.DFA dfa;


		/// <summary>
		/// Gets the terminals that can be matched by this lexer
		/// </summary>
		public List<Symbol> Terminals { get { return terminals; } }

		/// <summary>
		/// Gets the lexer's dfa
		/// </summary>
		public Automata.DFA DFA { get { return dfa; } }

		/// <summary>
		/// Initializes this lexer reflection
		/// </summary>
		/// <param name="assembly">The assembly containing the compiled lexer</param>
		/// <param name="lexerType">The type of the lexer</param>
		public LexerReflection(Assembly assembly, System.Type lexerType)
		{
			string input = "";
			ConstructorInfo ctor = lexerType.GetConstructor(new System.Type[] { typeof(string) });
			Hime.Redist.Lexer.Lexer lexer = ctor.Invoke(new object[] { input }) as Hime.Redist.Lexer.Lexer;

			this.terminals = new List<Symbol>(lexer.Terminals.Values);
			this.dfa = new Automata.DFA ();

			string[] resources = assembly.GetManifestResourceNames();
			Stream stream = null;
			foreach (string existing in resources)
			{
				if (existing.EndsWith(lexerType.Name + ".bin"))
				{
					stream = assembly.GetManifestResourceStream(existing);
					break;
				}
			}
			BinaryReader reader = new BinaryReader(stream);
			int count = reader.ReadInt32();
			Hime.Redist.Utils.BlobUInt table = new Hime.Redist.Utils.BlobUInt(count);
			reader.Read(table.Raw, 0, table.Raw.Length);
			Hime.Redist.Utils.BlobUShort data = new Hime.Redist.Utils.BlobUShort((int)((stream.Length - table.Raw.Length - 4) / 2));
			reader.Read(data.Raw, 0, data.Raw.Length);
			reader.Close();

			for (int i=0; i!=count; i++)
				this.dfa.States.Add(new Automata.DFAState ());
			for (int i=0; i!=count; i++)
			{
				Automata.DFAState current = this.dfa.States[i];
				current.ID = i;
				int offset = (int)table[i];
				ushort tIndex = data[offset];
				ushort nNonCached = data[offset + 2];
				if (tIndex != 0xFFFF)
					current.AddFinal(new MatchedTerminal(this.terminals[(int)tIndex]));
				for (int j=0; j!=256; j++)
				{
					ushort next = data[offset + 3 + j];
					char c = System.Convert.ToChar(j);
					if (next != 0xFFFF)
						current.AddTransition(new Automata.CharSpan(c, c), this.dfa.States[next]);
				}
				for (int j=0; j!=nNonCached; j++)
				{
					ushort begin = data[offset + 3 + 256 + (j * 3)];
					ushort end = data[offset + 3 + 256 + (j * 3) + 1];
					ushort next = data[offset + 3 + 256 + (j * 3) + 2];
					current.AddTransition(new Automata.CharSpan(System.Convert.ToChar(begin), System.Convert.ToChar(end)), this.dfa.States[next]);
				}
				current.RepackTransitions();
			}
		}

		/// <summary>
		/// Exports the lexer's DFA in the DOT format into the given file
		/// </summary>
		/// <param name="file">The file to export to</param>
		public void ExportDFA(string file)
		{
			Documentation.DOTSerializer serializer = new Documentation.DOTSerializer("dfa", file);
			this.dfa.SerializeGraph(serializer);
			serializer.Close();
		}
	}
}