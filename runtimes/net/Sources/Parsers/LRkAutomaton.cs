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
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents the LR(k) parsing table and productions
	/// </summary>
	/// <remarks>
	/// Binary data of a LR(k) parser
	/// --- header
	/// uint16: number of columns
	/// uint16: number of states
	/// uint16: number of productions
	/// --- parse table columns
	/// uint16: sid of the column
	/// --- parse table
	/// See LRActions
	/// --- productions table
	/// See LRProduction
	/// </remarks>
	public class LRkAutomaton
	{
		/// <summary>
		/// The number of columns in the LR table
		/// </summary>
		private readonly ushort ncols;
		/// <summary>
		/// The number of states in the LR table
		/// </summary>
		private readonly ushort nstates;
		/// <summary>
		/// Map of symbol ID to column index in the LR table
		/// </summary>
		private readonly ColumnMap columns;
		/// <summary>
		/// The contexts information
		/// </summary>
		private readonly LRContexts[] contexts;
		/// <summary>
		/// The LR table
		/// </summary>
		private readonly LRAction[] table;
		/// <summary>
		/// The table of LR productions
		/// </summary>
		private readonly LRProduction[] productions;

		/// <summary>
		/// Gets the number of states in this automaton
		/// </summary>
		public int StatesCount { get { return nstates; } }

		/// <summary>
		/// Initializes a new automaton from the given binary stream
		/// </summary>
		/// <param name="reader">The binary stream to load from</param>
		public LRkAutomaton(BinaryReader reader)
		{
			ncols = reader.ReadUInt16();
			nstates = reader.ReadUInt16();
			int nprod = reader.ReadUInt16();
			columns = new ColumnMap();
			for (int i = 0; i != ncols; i++)
				columns.Add(reader.ReadUInt16(), i);
			contexts = new LRContexts[nstates];
			for (int i = 0; i != nstates; i++)
				contexts[i] = new LRContexts(reader);
			table = new LRAction[nstates * ncols];
			for (int i = 0; i != nstates * ncols; i++)
				table[i] = new LRAction(reader);
			productions = new LRProduction[nprod];
			for (int i = 0; i != nprod; i++)
				productions[i] = new LRProduction(reader);
		}

		/// <summary>
		/// Loads an automaton from a resource
		/// </summary>
		/// <param name="type">The lexer's type</param>
		/// <param name="resource">The name of the resource containing the lexer</param>
		/// <returns>The automaton</returns>
		public static LRkAutomaton Find(System.Type type, string resource)
		{
			System.Reflection.Assembly assembly = type.Assembly;
			string[] resources = assembly.GetManifestResourceNames();
			foreach (string existing in resources)
			{
				if (existing.EndsWith(resource))
				{
					BinaryReader reader = new BinaryReader(assembly.GetManifestResourceStream(existing));
					LRkAutomaton automaton = new LRkAutomaton(reader);
					reader.Close();
					return automaton;
				}
			}
			throw new IOException(string.Format("The resource {0} cannot be found in the assembly {1}", resource, assembly.GetName().Name));
		}

		/// <summary>
		/// Gets the contexts opened by the specified state
		/// </summary>
		/// <param name="state">State in the LR(k) automaton</param>
		/// <returns>The opened contexts</returns>
		public LRContexts GetContexts(int state)
		{
			return contexts[state];
		}

		/// <summary>
		/// Gets the LR(k) action for the given state and sid
		/// </summary>
		/// <param name="state">State in the LR(k) automaton</param>
		/// <param name="sid">Symbol's ID</param>
		/// <returns>The LR(k) action for the state and sid</returns>
		public LRAction GetAction(int state, int sid)
		{
			return table[state * ncols + columns[sid]];
		}

		/// <summary>columns
		/// Gets the production at the given index
		/// </summary>
		/// <param name="index">Production's index</param>
		/// <returns>The production a the given index</returns>
		public LRProduction GetProduction(int index)
		{
			return productions[index];
		}

		/// <summary>
		/// Gets the expected terminals for the specified state
		/// </summary>
		/// <param name="state">The DFA state</param>
		/// <param name="terminals">The possible terminals</param>
		/// <returns>The expected terminals</returns>
		public LRExpected GetExpected(int state, ROList<Symbol> terminals)
		{
			LRExpected result = new LRExpected();
			int offset = ncols * state;
			for (int i = 0; i != terminals.Count; i++)
			{
				LRAction action = table[offset];
				if (action.Code == LRActionCode.Shift)
					result.Shifts.Add(terminals[i]);
				else if (action.Code == LRActionCode.Reduce)
					result.Reductions.Add(terminals[i]);
				offset++;
			}
			return result;
		}
	}
}
