/*
 * Author: Laurent Wouters
 * Date: 01/12/2012
 * Time: 10:15
 * 
 */
using System.IO;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents the RNGLR parsing table and productions
    /// </summary>
    public sealed class RNGLRAutomaton
    {
        /* Binary data of a GLR parser
         * uint16: number of columns
         * uint16: number of states
         * uint16: number of actions
         * uint16: number of productions
         * uint16: number of null productions
         * 
         * -- parse table columns
         * uint16: sid of the column
         * 
         * -- parse table
         * uint16: number of actions
         * uint16: offset into the action table
         * 
         * -- action table
         * Each reduction is of the form:
         * uint16: =1
         * uint16: index of the production
         * Each shift is of the form:
         * uint16: =2
         * uint16: new state
         * 
         * -- productions table
         * -- null production table
         * indices of the null productions
         */

        private ushort ncols;
        private Utils.BlobUShort columnsID;
        private Utils.SIDHashMap<ushort> columns;
        private Utils.BlobUShort table;
        private Utils.BlobUShort actions;
        private LRProduction[] productions;
        private Utils.BlobUShort nullables;

        internal Utils.BlobUShort Nullables { get { return nullables; } }

        private RNGLRAutomaton(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            this.ncols = reader.ReadUInt16();
            ushort nstates = reader.ReadUInt16();
            ushort nactions = reader.ReadUInt16();
            ushort nprod = reader.ReadUInt16();
            ushort nnprod = reader.ReadUInt16();
            this.columnsID = new Utils.BlobUShort(ncols * 2);
            reader.Read(columnsID.Raw, 0, ncols * 2);
            this.columns = new Utils.SIDHashMap<ushort>();
            for (ushort i = 0; i != ncols; i++)
                this.columns.Add(columnsID[i], i);
            this.actions = new Utils.BlobUShort(ncols * nstates * 4);
            reader.Read(this.actions.Raw, 0, this.actions.RawSize);
            this.table = new Utils.BlobUShort(nactions * 4);
            reader.Read(this.table.Raw, 0, this.table.RawSize);
            this.productions = new LRProduction[nprod];
            for (int i = 0; i != nprod; i++)
                this.productions[i] = new LRProduction(reader);
            this.nullables = new Utils.BlobUShort(nnprod * 2);
            reader.Read(this.nullables.Raw, 0, this.nullables.RawSize);
            reader.Close();
        }

        /// <summary>
        /// Loads an automaton from a resource
        /// </summary>
        /// <param name="assembly">The assembly containing the automaton definition</param>
        /// <param name="resource">The resource's name</param>
        /// <returns>The automaton</returns>
        public static RNGLRAutomaton FindAutomaton(System.Type type, string resource)
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
        public ushort GetActionsCount(ushort state, ushort sid)
        {
            return actions[(state * ncols + columns[sid]) * 2];
        }

        /// <summary>
        /// Gets the nth GLR action for the given state and sid
        /// </summary>
        /// <param name="state">An automaton's state</param>
        /// <param name="sid">A symbol ID</param>
        /// <param name="index">The action index</param>
        /// <param name="action">The action's value</param>
        /// <returns>The action's data</returns>
        public ushort GetAction(ushort state, ushort sid, int index, out ushort action)
        {
            int offset = actions[(state * ncols + columns[sid]) * 2 + 1] + index * 2;
            action = table[offset];
            return table[offset + 1];
        }

        /// <summary>
        /// Gets the production at the given index
        /// </summary>
        /// <param name="index">Production's index</param>
        /// <returns>The production a the given index</returns>
        public LRProduction GetProduction(ushort index) { return productions[index]; }

        /// <summary>
        /// Determine whether the given state is the accepting state
        /// </summary>
        /// <param name="state">The DFA state</param>
        /// <returns>True if the state is the accepting state, false otherwise</returns>
        public bool IsAcceptingState(ushort state)
        {
            int offset = (state * ncols) * 2;
            if (actions[offset] == 0)
                return false;
            return (table[actions[offset + 1]] == LRkAutomaton.ActionAccept);
        }

        /// <summary>
        /// Gets a list of the expected terminal indices
        /// </summary>
        /// <param name="state">The DFA state</param>
        /// <param name="terminalCount">The maximal number of terminals</param>
        /// <returns>The expected terminal indices</returns>
        public List<int> GetExpected(ushort state, int terminalCount)
        {
            List<int> result = new List<int>();
            int offset = ncols * state * 2;
            for (int i = 0; i != terminalCount; i++)
            {
                if (table[offset] != 0)
                    result.Add(i);
                offset += 2;
            }
            return result;
        }
    }
}
