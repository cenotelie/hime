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
	public class LexerDataGenerator : Generator
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
		/// Initializes this generator
		/// </summary>
		/// <param name="dfa">The dfa to serialize</param>
		public LexerDataGenerator(Automata.DFA dfa)
		{
			this.dfa = dfa;
			this.terminals = new List<Grammars.Terminal>();
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
		/// Generates the lexer's binary data
		/// </summary>
		/// <param name="file">The file to output to</param>
		public void Generate(string file)
		{
			BinaryWriter writer = new BinaryWriter(new FileStream(file, FileMode.Create));

			writer.Write((uint)dfa.StatesCount);
			uint offset = 0;
			foreach (Automata.DFAState state in dfa.States)
			{
				writer.Write(offset);
				offset += 3 + 256;
				foreach (CharSpan key in state.Transitions)
					if (key.End >= 256)
						offset += 3;
			}
			foreach (Automata.DFAState state in dfa.States)
				GenerateDataFor(writer, state);

			writer.Close();
		}

		/// <summary>
		/// Generates the given state binary data
		/// </summary>
		/// <param name="writer">The output writer</param>
		/// <param name="state">The state to export</param>
		private void GenerateDataFor(BinaryWriter writer, Automata.DFAState state)
		{
			ushort[] cache = new ushort[256];
			for (int i = 0; i != 256; i++)
				cache[i] = 0xFFFF;
			ushort cached = 0;
			ushort slow = 0;
			foreach (CharSpan span in state.Transitions)
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
						cache[i] = (ushort)state.GetChildBy(span).ID;
				}
				else
					slow++;
			}

			if (state.TopItem != null)
				writer.Write((ushort)terminals.IndexOf(state.TopItem as Grammars.Terminal));
			else
				writer.Write((ushort)0xFFFF);
			writer.Write((ushort)(slow + cached));
			writer.Write(slow);

			for (int i = 0; i != 256; i++)
				writer.Write(cache[i]);

			List<CharSpan> keys = new List<CharSpan>(state.Transitions);
			keys.Sort(new System.Comparison<CharSpan>(CharSpan.CompareReverse));
			foreach (CharSpan span in keys)
			{
				if (span.End <= 255)
					break; // the rest of the transitions are in the cache
				ushort begin = span.Begin;
				if (begin <= 255)
					begin = 256;
				writer.Write(begin);
				writer.Write(System.Convert.ToUInt16(span.End));
				writer.Write((ushort)state.GetChildBy(span).ID);
				slow--;
			}
		}
	}
}
