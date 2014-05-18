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
using System.Runtime.InteropServices;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents the RNGLR parsing table and productions
	/// </summary>
	/// <remarks>
	/// Binary data of a RNGLR parser
	/// --- header
	/// uint16: index of the axiom's variable
	/// uint16: number of columns
	/// uint16: number of states
	/// uint32: number of actions
	/// uint16: number of productions
	/// uint16: number of null productions
	/// --- parse table columns
	/// uint16: sid of the column
	/// --- parse table
	/// See RNGLRTable
	/// --- action table
	/// See LRActions
	/// --- productions table
	/// See LRProduction
	/// --- null production table
	/// indices of the null productions
	/// </remarks>
	public class RNGLRAutomaton
	{
		/// <summary>
		/// Represents a cell in a RNGLR parse table
		/// </summary>
		[StructLayout(LayoutKind.Explicit, Size = 6)]
		private struct Cell
		{
			/// <summary>
			/// The number of actions in this cell
			/// </summary>
			[FieldOffset(0)]
			private ushort count;
			/// <summary>
			/// Index of the cell's data
			/// </summary>
			[FieldOffset(2)]
			private uint index;
			/// <summary>
			/// Gets the number of actions in the cell
			/// </summary>
			public int ActionsCount { get { return count; } }
			/// <summary>
			/// Gets the index of the first action in the Actions table
			/// </summary>
			public int ActionsIndex { get { return (int)index; } }
		}

		/// <summary>
		/// Index of the axiom variable
		/// </summary>
		private int axiom;
		/// <summary>
		/// The number of columns in the LR table
		/// </summary>
		private ushort ncols;
		/// <summary>
		/// The number of states in the automaton
		/// </summary>
		private int nstates;
		/// <summary>
		/// Map of symbol ID to column index in the LR table
		/// </summary>
		private ColumnMap columns;
		/// <summary>
		/// The RNGLR table
		/// </summary>
		private Utils.Blob<Cell> table;
		/// <summary>
		/// The action table
		/// </summary>
		private Utils.Blob<LRAction> actions;
		/// <summary>
		/// The table of LR productions
		/// </summary>
		private LRProduction[] productions;
		/// <summary>
		/// The table of nullable variables
		/// </summary>
		private Utils.Blob<ushort> nullables;

		/// <summary>
		/// Gets the index of the axiom
		/// </summary>
		public int Axiom { get { return axiom; } }
		/// <summary>
		/// Gets the number of states in the RNGLR table
		/// </summary>
		public int StatesCount { get { return nstates; } }

		/// <summary>
		/// Initializes a new automaton from the given binary stream
		/// </summary>
		/// <param name="reader">The binary stream to load from</param>
		public RNGLRAutomaton(BinaryReader reader)
		{
			this.axiom = reader.ReadUInt16();
			this.ncols = reader.ReadUInt16();
			this.nstates = reader.ReadUInt16();
			int nactions = (int)reader.ReadUInt32();
			int nprod = reader.ReadUInt16();
			int nnprod = reader.ReadUInt16();
			Utils.Blob<ushort> columnsID = new Utils.Blob<ushort>(ncols, 2);
			columnsID.LoadFrom(reader);
			this.columns = new ColumnMap();
			for (int i = 0; i != ncols; i++)
				this.columns.Add(columnsID[i], i);
			this.table = new Utils.Blob<Cell>(nstates * ncols, 6);
			this.table.LoadFrom(reader);
			this.actions = new Utils.Blob<LRAction>(nactions, 4);
			this.actions.LoadFrom(reader);
			this.productions = new LRProduction[nprod];
			for (int i = 0; i != nprod; i++)
				this.productions[i] = new LRProduction(reader);
			this.nullables = new Utils.Blob<ushort>(nnprod, 2);
			this.nullables.LoadFrom(reader);
		}

		/// <summary>
		/// Loads an automaton from a resource
		/// </summary>
		/// <param name="type">The lexer's type</param>
		/// <param name="resource">The name of the resource containing the lexer</param>
		/// <returns>The automaton</returns>
		public static RNGLRAutomaton Find(System.Type type, string resource)
		{
			System.Reflection.Assembly assembly = type.Assembly;
			string[] resources = assembly.GetManifestResourceNames();
			foreach (string existing in resources)
			{
				if (existing.EndsWith(resource))
				{
					BinaryReader reader = new BinaryReader(assembly.GetManifestResourceStream(existing));
					RNGLRAutomaton automaton = new RNGLRAutomaton(reader);
					reader.Close();
					return automaton;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the number of GLR actions for the given state and sid
		/// </summary>
		/// <param name="state">An automaton's state</param>
		/// <param name="sid">A symbol ID</param>
		/// <returns>The number of GLR actions</returns>
		public int GetActionsCount(int state, int sid)
		{
			return table[state * ncols + columns[sid]].ActionsCount;
		}

		/// <summary>
		/// Gets the i-th GLR action for the given state and sid
		/// </summary>
		/// <param name="state">An automaton's state</param>
		/// <param name="sid">A symbol ID</param>
		/// <param name="index">The action index</param>
		/// <returns>The GLR action</returns>
		public LRAction GetAction(int state, int sid, int index)
		{
			return actions[table[state * ncols + columns[sid]].ActionsIndex + index];
		}

		/// <summary>
		/// Gets the production at the given index
		/// </summary>
		/// <param name="index">Production's index</param>
		/// <returns>The production a the given index</returns>
		public LRProduction GetProduction(int index)
		{
			return productions[index];
		}

		/// <summary>
		/// Gets the production for the nullable variable with the given index
		/// </summary>
		/// <param name="index">Index of a nullable variable</param>
		/// <returns>The production, or <c>null</c> if the variable is not nullable</returns>
		public LRProduction GetNullableProduction(int index)
		{
			int temp = nullables[index];
			if (temp == 0xFFFF)
				return null;
			return productions[temp];
		}

		/// <summary>
		/// Determine whether the given state is the accepting state
		/// </summary>
		/// <param name="state">An automaton's state</param>
		/// <returns>True if the state is the accepting state, false otherwise</returns>
		public bool IsAcceptingState(int state)
		{
			if (table[state * ncols].ActionsCount != 1)
				return false;
			return (actions[table[state * ncols].ActionsIndex].Code == LRActionCode.Accept);
		}

		/// <summary>
		/// Gets a collection of the expected terminal indices
		/// </summary>
		/// <param name="state">The DFA state</param>
		/// <param name="terminalCount">The maximal number of terminals</param>
		/// <returns>The expected terminal indices</returns>
		public ICollection<int> GetExpected(int state, int terminalCount)
		{
			List<int> result = new List<int>();
			for (int i = 0; i != terminalCount; i++)
			{
				Cell cell = table[state * ncols + i];
				for (int j=0; j!=cell.ActionsCount; j++)
				{
					if (actions[cell.ActionsIndex + j].Code == LRActionCode.Shift)
					{
						result.Add(i);
						break;
					}
				}
			}
			return result;
		}
	}
}
