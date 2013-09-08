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
    public sealed class RNGLRAutomaton
    {
        private int axiom;
        private int ncols;
        private int nstates;
        private Utils.BlobUShort columnsID;
        private ColumnMap columns;
        private RNGLRTable table;
        private LRActions actions;
        private LRProduction[] productions;
        private Utils.BlobUShort nullables;

        internal int Axiom { get { return axiom; } }
        internal int StatesCount { get { return nstates; } }
        internal Utils.BlobUShort Nullables { get { return nullables; } }

        private RNGLRAutomaton(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            this.axiom = reader.ReadUInt16();
            this.ncols = reader.ReadUInt16();
            this.nstates = reader.ReadUInt16();
            int nactions = (int)reader.ReadUInt32();
            int nprod = reader.ReadUInt16();
            int nnprod = reader.ReadUInt16();
            this.columnsID = new Utils.BlobUShort(ncols);
            reader.Read(columnsID.Raw, 0, columnsID.Raw.Length);
            this.columns = new ColumnMap();
            for (int i = 0; i != ncols; i++)
                this.columns.Add(columnsID[i], i);
            this.table = new RNGLRTable(nstates, ncols);
            reader.Read(this.table.Raw, 0, this.table.Raw.Length);
            this.actions = new LRActions(nactions);
            reader.Read(this.actions.Raw, 0, this.actions.Raw.Length);
            this.productions = new LRProduction[nprod];
            for (int i = 0; i != nprod; i++)
                this.productions[i] = new LRProduction(reader);
            this.nullables = new Utils.BlobUShort(nnprod);
            reader.Read(this.nullables.Raw, 0, this.nullables.Raw.Length);
            reader.Close();
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
                if (existing.EndsWith(resource))
                    return new RNGLRAutomaton(assembly.GetManifestResourceStream(existing));
            return null;
        }

        /// <summary>
        /// Gets the number of GLR actions for the given state and sid
        /// </summary>
        /// <param name="state">An automaton's state</param>
        /// <param name="sid">A symbol ID</param>
        /// <returns>The number of GLR actions</returns>
        internal int GetActionsCount(int state, int sid)
        {
            return table[state, columns[sid]].ActionsCount;
        }

        /// <summary>
        /// Gets the nth GLR action for the given state and sid
        /// </summary>
        /// <param name="state">An automaton's state</param>
        /// <param name="sid">A symbol ID</param>
        /// <param name="index">The action index</param>
        /// <returns>The GLR action</returns>
        internal LRAction GetAction(int state, int sid, int index)
        {
            return actions[table[state, columns[sid]].ActionsIndex + index];
        }

        /// <summary>
        /// Gets the production at the given index
        /// </summary>
        /// <param name="index">Production's index</param>
        /// <returns>The production a the given index</returns>
        internal LRProduction GetProduction(int index) { return productions[index]; }

        /// <summary>
        /// Determine whether the given state is the accepting state
        /// </summary>
        /// <param name="state">The DFA state</param>
        /// <returns>True if the state is the accepting state, false otherwise</returns>
        internal bool IsAcceptingState(int state)
        {
            if (table[state, 0].ActionsCount != 1)
                return false;
            return (actions[table[state, 0].ActionsIndex].Code == LRActionCode.Accept);
        }

        /// <summary>
        /// Gets a collection of the expected terminal indices
        /// </summary>
        /// <param name="state">The DFA state</param>
        /// <param name="terminalCount">The maximal number of terminals</param>
        /// <returns>The expected terminal indices</returns>
        internal ICollection<int> GetExpected(int state, int terminalCount)
        {
            List<int> result = new List<int>();
            for (ushort i = 0; i != terminalCount; i++)
            {
                if (table[state, i].ActionsCount != 0)
                    result.Add(i);
            }
            return result;
        }
    }
}
