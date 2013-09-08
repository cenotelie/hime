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
    public sealed class Automaton
    {
        private Utils.BlobUInt table;
        private Utils.BlobUShort states;

        private Automaton(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int count = reader.ReadInt32();
            table = new Utils.BlobUInt(count);
            reader.Read(table.Raw, 0, table.Raw.Length);
            states = new Utils.BlobUShort((int)((stream.Length - table.Raw.Length - 4) / 2));
            reader.Read(states.Raw, 0, states.Raw.Length);
            reader.Close();
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
                if (existing.EndsWith(resource))
                    return new Automaton(assembly.GetManifestResourceStream(existing));
            return null;
        }

        /// <summary>
        /// Get the offset of the given state in the table
        /// </summary>
        /// <param name="state">The DFA which offset shall be retrieved</param>
        /// <returns>The offset of the given DFA state</returns>
        internal int GetOffset(int state) { return (int)table[state]; }

        /// <summary>
        /// Gets the recognized terminal index for the DFA at the given index
        /// </summary>
        /// <param name="offset">The DFA state's offset</param>
        /// <returns>The index of the terminal recognized at this state, or 0xFFFF if none</returns>
        internal int GetTerminalIndex(int offset) { return states[offset]; }

        /// <summary>
        /// Checks whether the DFA state at the given state has any transition
        /// </summary>
        /// <param name="offset">The DFA state's offset</param>
        /// <returns>True of the state at the given offset has no transition</returns>
        internal bool HasNoTransition(int offset) { return (states[offset + 1] == 0); }

        /// <summary>
        /// Gets the transition corresponding to the given state's index and input value
        /// </summary>
        /// <param name="offset">The DFA state's offset</param>
        /// <returns>The state obtained by the transition, or 0xFFFF if none is found</returns>
        internal int GetCachedTransition(int offset) { return states[offset]; }

        /// <summary>
        /// Gets the transition corresponding to the given state's index and input value
        /// </summary>
        /// <param name="offset">The DFA state's offset</param>
        /// <param name="value">The input value</param>
        /// <returns>The state obtained by the transition, or 0xFFFF if none is found</returns>
        internal int GetFallbackTransition(int offset, int value)
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
    }
}
