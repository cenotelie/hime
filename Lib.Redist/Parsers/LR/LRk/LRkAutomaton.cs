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
    public sealed class LRkAutomaton
    {
        private ushort ncols;
        private Utils.BlobUShort columnsID;
        private Utils.SIDHashMap<int> columns;
        private LRActions table;
        private LRProduction[] productions;

        private LRkAutomaton(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            this.ncols = reader.ReadUInt16();
            int nstates = reader.ReadUInt16();
            int nprod = reader.ReadUInt16();
            this.columnsID = new Utils.BlobUShort(ncols);
            reader.Read(columnsID.Raw, 0, columnsID.Raw.Length);
            this.columns = new Utils.SIDHashMap<int>();
            for (int i = 0; i != ncols; i++)
                this.columns.Add(columnsID[i], i);
            this.table = new LRActions(nstates * ncols);
            reader.Read(this.table.Raw, 0, this.table.Raw.Length);
            this.productions = new LRProduction[nprod];
            for (int i = 0; i != nprod; i++)
                this.productions[i] = new LRProduction(reader);
            reader.Close();
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
                if (existing.EndsWith(resource))
                    return new LRkAutomaton(assembly.GetManifestResourceStream(existing));
            return null;
        }

        /// <summary>
        /// Gets the LR(k) action for the given state and sid
        /// </summary>
        /// <param name="state">State in the LR(k) automaton</param>
        /// <param name="sid">Symbol's ID</param>
        /// <returns>The LR(k) action for the state and sid</returns>
        internal LRAction GetAction(int state, int sid)
        {
            return table[state * ncols + columns[sid]];
        }

        /// <summary>
        /// Gets the production at the given index
        /// </summary>
        /// <param name="index">Production's index</param>
        /// <returns>The production a the given index</returns>
        internal LRProduction GetProduction(int index) { return productions[index]; }

        /// <summary>
        /// Gets a collection of the expected terminal indices
        /// </summary>
        /// <param name="state">The DFA state</param>
        /// <param name="terminalCount">The maximal number of terminals</param>
        /// <returns>The expected terminal indices</returns>
        internal ICollection<int> GetExpected(int state, int terminalCount)
        {
            List<int> result = new List<int>();
            int offset = ncols * state;
            for (int i = 0; i != terminalCount; i++)
            {
                if (table[offset].Code != LRActionCode.None)
                    result.Add(i);
                offset++;
            }
            return result;
        }
    }
}
