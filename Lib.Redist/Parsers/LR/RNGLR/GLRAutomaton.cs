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
    /// Represents the GLR parsing table and productions
    /// </summary>
    public sealed class GLRAutomaton
    {
        /* Binary data of a GLR parser
         * uint16: number of columns
         * uint16: number of rows
         * uint16: number of actions
         * uint16: number of productions
         * 
         * -- parse table columns
         * uint16: sid of the column
         * 
         * -- parse table
         * uint16:  =0 => no action
         *          else => offset into the action table
         * 
         * -- action table
         * uint16: number of actions
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
        private Utils.BlobUShort tableParse;
        private Utils.BlobUShort tableActions;
        private LRProduction[] productions;

        private GLRAutomaton(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            this.ncols = reader.ReadUInt16();
            ushort nrows = reader.ReadUInt16();
            ushort nactions = reader.ReadUInt16();
            ushort nprod = reader.ReadUInt16();

            byte[] cb = new byte[ncols * 2];
            stream.Read(cb, 0, ncols * 2);
            this.columnsID = new Utils.BlobUShort(cb);
            this.columns = new Utils.SIDHashMap<ushort>();
            for (ushort i = 0; i != ncols; i++)
                this.columns.Add(columnsID[i], i);

            byte[] btp = new byte[ncols * nrows * 2];
            stream.Read(btp, 0, btp.Length);
            this.tableParse = new Utils.BlobUShort(btp);

            byte[] bta = new byte[nactions];
            stream.Read(bta, 0, bta.Length);
            this.tableActions = new Utils.BlobUShort(bta);

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
        public static GLRAutomaton FindAutomaton(System.Type type, string resource)
        {
            System.Reflection.Assembly assembly = type.Assembly;
            string[] resources = assembly.GetManifestResourceNames();
            foreach (string existing in resources)
                if (existing.EndsWith(resource))
                    return new GLRAutomaton(assembly.GetManifestResourceStream(existing));
            return null;
        }

        /// <summary>
        /// Gets the number of GLR actions for the given state and sid
        /// </summary>
        /// <param name="state">An automaton's state</param>
        /// <param name="sid">A symbol ID</param>
        /// <returns>The number of GLR actions</returns>
        public ushort GetActionCount(ushort state, ushort sid)
        {
            int action = tableParse[ncols * state + columns[sid]];
            if (action == 0xFFFF)
                return 0;
            return tableActions[action];
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
            int offset = tableParse[ncols * state + columns[sid]] + index * 2 + 1;
            action = tableActions[offset];
            return tableActions[offset + 1];
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
            int action = tableParse[ncols * state];
            if (action == 0xFFFF)
                return false;
            int count = tableActions[action];
            for (int i = 0; i != count; i++)
                if (tableActions[action + i * 2 + 1] == LRkAutomaton.ActionAccept)
                    return true;
            return false;
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
            int offset = ncols * state;
            for (int i = 0; i != terminalCount; i++)
            {
                int action = tableParse[offset + i];
                if (action != 0xFFFF)
                    result.Add(i);
            }
            return result;
        }
    }
}
