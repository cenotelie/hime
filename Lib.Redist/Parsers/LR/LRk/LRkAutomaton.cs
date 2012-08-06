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
        /* Binary data of a LR(k) parser
         * uint16: number of columns
         * uint16: number of rows
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
        private Utils.BlobUShort table;
        private Utils.BlobUShort columnsID;
        private Utils.SIDHashMap<ushort> columns;
        private LRProduction[] productions;

        private LRkAutomaton(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            this.ncols = reader.ReadUInt16();
            ushort nrows = reader.ReadUInt16();
            ushort nprod = reader.ReadUInt16();
            
            byte[] cb = new byte[ncols * 2];
            stream.Read(cb, 0, ncols * 2);
            this.columnsID = new Utils.BlobUShort(cb);
            this.columns = new Utils.SIDHashMap<ushort>();
            for (ushort i = 0; i != ncols; i++)
                this.columns.Add(columnsID.data[i], i);
            
            byte[] buffer = new byte[ncols * nrows * 4];
            stream.Read(buffer, 0, buffer.Length);
            this.table = new Utils.BlobUShort(buffer);
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
        /// <returns></returns>
        public static LRkAutomaton FindAutomaton(System.Type type)
        {
            System.Reflection.Assembly assembly = type.Assembly;
            System.IO.Stream stream = assembly.GetManifestResourceStream(type.Name);
            if (stream != null)
                return new LRkAutomaton(stream);
            string[] resources = assembly.GetManifestResourceNames();
            foreach (string existing in resources)
                if (existing.EndsWith(type.Name))
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
            action = table.data[offset];
            return table.data[offset + 1];
        }

        /// <summary>
        /// Gets the production at the given index
        /// </summary>
        /// <param name="index">Production's index</param>
        /// <returns>The production a the given index</returns>
        public LRProduction GetProduction(ushort index) { return productions[index]; }

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
                int action = table.data[offset];
                if (action != 0)
                    result.Add(i);
                offset += 2;
            }
            return result;
        }
    }
}
