/*
 * Author: Laurent Wouters
 * Date: 02/06/2012
 * Time: 10:15
 * 
 */
using System.IO;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents the LR(k) parsing table and productions
    /// </summary>
    public sealed class LRkAutomaton
    {
        public const ushort ActionNone = 0;
        public const ushort ActionReduce = 1;
        public const ushort ActionShift = 2;
        public const ushort ActionAccept = 3;

        /* Binary data of a LR(k) parser
         * uint16: number of columns
         * uint16: number of states
         * uint16: number of productions
         * 
         * -- parse table columns
         * uint16: sid of the column
         * 
         * -- parse table
         * Each reduction is of the form:
         * uint16: =1
         * uint16: index of the production
         * Each shift is of the form:
         * uint16: =2
         * uint16: new state
         * 
         * -- productions table
         */

        private ushort ncols;
        private Utils.BlobUShort columnsID;
        private Utils.SIDHashMap<ushort> columns;
        private Utils.BlobUShort table;
        private LRProduction[] productions;

        private LRkAutomaton(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            this.ncols = reader.ReadUInt16();
            ushort nstates = reader.ReadUInt16();
            ushort nprod = reader.ReadUInt16();
            this.columnsID = new Utils.BlobUShort(ncols * 2);
            reader.Read(columnsID.Raw, 0, ncols * 2);
            this.columns = new Utils.SIDHashMap<ushort>();
            for (ushort i = 0; i != ncols; i++)
                this.columns.Add(columnsID[i], i);
            this.table = new Utils.BlobUShort(ncols * nstates * 4);
            reader.Read(this.table.Raw, 0, this.table.RawSize);
            this.productions = new LRProduction[nprod];
            for (int i = 0; i != nprod; i++)
                this.productions[i] = new LRProduction(reader);
            reader.Close();
        }

        /// <summary>
        /// Loads an automaton from a resource
        /// </summary>
        /// <param name="assembly">The assembly containing the automaton definition</param>
        /// <param name="resource">The resource's name</param>
        /// <returns>The automaton</returns>
        public static LRkAutomaton FindAutomaton(System.Type type, string resource)
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
        /// <param name="state">A automaton's state</param>
        /// <param name="sid">A symbol ID</param>
        /// <param name="action">The action as the given state for the given sid</param>
        /// <returns>The action's data</returns>
        public ushort GetAction(ushort state, ushort sid, out ushort action)
        {
            int offset = (ncols * state + columns[sid]) * 2;
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
            return (table[ncols * state * 2] == ActionAccept);
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
                if (table[offset] != ActionNone)
                    result.Add(i);
                offset += 2;
            }
            return result;
        }
    }
}
