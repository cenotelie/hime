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

using System.IO;
using System.Reflection;

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents the automaton of a lexer
	/// </summary>
	/// <remarks>
	/// Binary data structure of lexers:
	/// uint32: number of entries in the states index table
	/// -- states offset table
	/// each entry is of the form:
	/// uint32: offset of the state from the beginning of the states table in number of uint16
	/// -- states table
	/// See AutomatonState
	/// </remarks>
	public class Automaton
	{
		/// <summary>
		/// Identifier of inexistant state in an automaton
		/// </summary>
		public const int DEAD_STATE = 0xFFFF;
		/// <summary>
		/// Identifier of the default context
		/// </summary>
		public const int DEFAULT_CONTEXT = 0;

		/// <summary>
		/// Table of indices in the states table
		/// </summary>
		private readonly uint[] table;
		/// <summary>
		/// Lexer's DFA table of states
		/// </summary>
		private readonly ushort[] states;
		/// <summary>
		/// The number of states in this automaton
		/// </summary>
		private readonly int statesCount;

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
			for (int i = 0; i != statesCount; i++)
				table[i] = reader.ReadUInt32();
			long rest = (reader.BaseStream.Length - table.Length * 4 - 4) / 2;
			states = new ushort[rest];
			for (int i = 0; i != rest; i++)
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
#if NETSTANDARD1_0
			Assembly assembly = type.GetTypeInfo().Assembly;
#else
			Assembly assembly = type.Assembly;
#endif
			string[] resources = assembly.GetManifestResourceNames();
			foreach (string existing in resources)
			{
				if (existing.EndsWith(resource))
				{
					BinaryReader reader = new BinaryReader(assembly.GetManifestResourceStream(existing));
					Automaton result = new Automaton(reader);
#if NETSTANDARD1_0
					reader.Dispose();
#else
					reader.Close();
#endif
					return result;
				}
			}
			throw new IOException(string.Format("The resource {0} cannot be found in the assembly {1}", resource, assembly.GetName().Name));
		}

		/// <summary>
		/// Get the data of the specified state
		/// </summary>
		/// <param name="state">A state's index</param>
		/// <returns>The data of the specified state</returns>
		public AutomatonState GetState(int state)
		{
			return new AutomatonState(states, (int)table[state]);
		}
	}
}
