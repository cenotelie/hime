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
using System.IO;

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Data structure for a text lexer automaton
	/// </summary>
	/// <remarks>
	/// Binary data structure of lexers:
	/// uint32: number of entries in the states index table
	/// -- states offset table
	/// each entry is of the form:
	/// uint32: offset of the state from the beginning of the states table in number of uint16
	///
	/// -- states table
	/// each entry is of the form:
	/// uint16: recognized terminal's index
	/// uint16: total number of transitions
	/// uint16: number of non-cached transitions
	/// -- cache: 256 entries
	/// uint16: next state's index for index of the entry
	/// -- transitions
	/// each transition is of the form:
	/// uint16: start of the range
	/// uint16: end of the range
	/// uint16: next state's index
	/// </remarks>
	public class Automaton
	{
		/// <summary>
		/// Table of indices in the states table
		/// </summary>
		private uint[] table;

		/// <summary>
		/// Lexer's DFA table of states
		/// </summary>
		private ushort[] states;

		/// <summary>
		/// The number of states in this automaton
		/// </summary>
		private int statesCount;

		/// <summary>
		/// Gets the number of states in this automaton
		/// </summary>
		public int StatesCount { get { return statesCount; } }

		/// <summary>
		/// Initializes a new automaton from the given binary stream
		/// </summary>
		/// <param name="reader">The binary stream to load from</param>
		/// <remarks>
		/// This methods reads the necessary data from the reader assuming the reader only contains this automaton.
		/// It will read from reader until the end of the underlying stream.
		/// </remarks>
		public Automaton(BinaryReader reader)
		{
			statesCount = reader.ReadInt32();
			table = new uint[statesCount];
			for (int i=0; i!=statesCount; i++)
				table[i] = reader.ReadUInt32();
			long rest = (reader.BaseStream.Length - table.Length * 4 - 4) / 2;
			states = new ushort[rest];
			for (int i=0; i!=rest; i++)
				states[i] = reader.ReadUInt16();
		}

		/// <summary>
		/// Loads an automaton from a resource
		/// </summary>
		/// <param name="type">The lexer's type</param>
		/// <param name="resource">The name of the resource containing the lexer</param>
		/// <returns>The automaton</returns>
		public static Automaton Find(System.Type type, string resource)
		{
			System.Reflection.Assembly assembly = type.Assembly;
			string[] resources = assembly.GetManifestResourceNames();
			foreach (string existing in resources)
			{
				if (existing.EndsWith(resource))
				{
					BinaryReader reader = new BinaryReader(assembly.GetManifestResourceStream(existing));
					Automaton result = new Automaton(reader);
					reader.Close();
					return result;
				}
			}
			throw new IOException(string.Format("The resource {0} cannot be found in the assembly {1}", resource, assembly.GetName().Name));
		}

		/// <summary>
		/// Get the offset of the given state in the table
		/// </summary>
		/// <param name="state">The DFA which offset shall be retrieved</param>
		/// <returns>The offset of the given DFA state</returns>
		public int GetOffsetOf(int state)
		{
			return (int)table[state];
		}

		/// <summary>
		/// Gets the recognized terminal index for the DFA at the given offset
		/// </summary>
		/// <param name="offset">The DFA state's offset</param>
		/// <returns>The index of the terminal recognized at this state, or 0xFFFF if none</returns>
		public int GetStateRecognizedTerminal(int offset)
		{
			return states[offset];
		}

		/// <summary>
		/// Checks whether the DFA state at the given offset does not have any transition
		/// </summary>
		/// <param name="offset">The DFA state's offset</param>
		/// <returns><c>true</c> if the state at the given offset has no transition</returns>
		public bool IsStateDeadEnd(int offset)
		{
			return (states[offset + 1] == 0);
		}

		/// <summary>
		/// Gets the number of non-cached transitions from the DFA state at the given offset
		/// </summary>
		/// <param name="offset">The DFA state's offset</param>
		/// <returns>The number of non-cached transitions</returns>
		public int GetStateBulkTransitionsCount(int offset)
		{
			return states[offset + 2];
		}

		/// <summary>
		/// Gets the transition from the DFA state at the given offset with the input value (max 255)
		/// </summary>
		/// <param name="offset">The DFA state's offset</param>
		/// <param name="value">The input value</param>
		/// <returns>The state obtained by the transition, or 0xFFFF if none is found</returns>
		public int GetStateCachedTransition(int offset, int value)
		{
			return states[offset + 3 + value];
		}

		/// <summary>
		/// Gets the transition from the DFA state at the given offset with the input value (min 256)
		/// </summary>
		/// <param name="offset">The DFA state's offset</param>
		/// <param name="value">The input value</param>
		/// <returns>The state obtained by the transition, or 0xFFFF if none is found</returns>
		public int GetStateBulkTransition(int offset, int value)
		{
			int count = states[offset + 2];
			offset += 259;
			for (int i = 0; i != count; i++)
			{
				if (value >= states[offset] && value <= states[offset + 1])
					return states[offset + 2];
				offset += 3;
			}
			return 0xFFFF;
		}

		/// <summary>
		/// Gets the transition i-th from the DFA state at the given offset
		/// </summary>
		/// <param name="offset">The DFA state's offset</param>
		/// <param name="index">The non-cached transition index</param>
		/// <param name="start">The starting value of the transition</param>
		/// <param name="end">The ending value of the transition</param>
		/// <returns>The state obtained by the transition</returns>
		public int GetStateBulkTransition(int offset, int index, out int start, out int end)
		{
			int real = offset + 3 + 256 + (index * 3);
			start = states[real];
			end = states[real + 1];
			return states[real + 2];
		}
	}
}
