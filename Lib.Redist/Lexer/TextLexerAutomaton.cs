/*
 * Author: Laurent Wouters
 * Date: 02/06/2012
 * Time: 10:15
 * 
 */
using System.IO;

namespace Hime.Redist.Parsers
{
    /* Binary data structure of lexers:
     * uint32: number of entries in the states index table
     * 
     * -- states index table
     * each entry is of the form:
     * uint32: offset of the state from the beginning of the states table in number of uint16
     * 
     * -- states table
     * each entry is of the form:
     * uint16: recognized terminal's index
     * uint16: number of transitions
     * -- cache: 256 entries
     * uint16: next state's index for index of the entry
     * -- transitions
     * each transition is of the form:
     * uint16: start of the range
     * uint16: end of the range
     * uint16: next state's index
     */

    /// <summary>
    /// Data structure for a text lexer automaton
    /// </summary>
    public sealed class TextLexerAutomaton
    {
        private Utils.BlobInt table;
        private Utils.BlobUShort states;

        private TextLexerAutomaton(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int count = reader.ReadInt32();
            byte[] bt = new byte[count * 4];
            reader.Read(bt, 0, bt.Length);
            byte[] bd = new byte[stream.Length - bt.Length - 4];
            reader.Read(bd, 0, bd.Length);
            reader.Close();
            table = new Utils.BlobInt(bt);
            states = new Utils.BlobUShort(bd);
        }

        /// <summary>
        /// Loads an automaton from a resource
        /// </summary>
        /// <param name="assembly">The assembly containing the automaton definition</param>
        /// <param name="resource">The resource's name</param>
        /// <returns>The automaton</returns>
        public static TextLexerAutomaton FindAutomaton(System.Type type, string resource)
        {
            System.Reflection.Assembly assembly = type.Assembly;
            string[] resources = assembly.GetManifestResourceNames();
            foreach (string existing in resources)
                if (existing.EndsWith(resource))
                    return new TextLexerAutomaton(assembly.GetManifestResourceStream(existing));
            return null;
        }

        /// <summary>
        /// Get the offset of the given state in the table
        /// </summary>
        /// <param name="state">The DFA which offset shall be retrieved</param>
        /// <returns>The offset of the given DFA state</returns>
        public int GetOffset(ushort state) { return table[state]; }

        /// <summary>
        /// Gets the recognized terminal index for the DFA at the given index
        /// </summary>
        /// <param name="offset">The DFA state's offset</param>
        /// <returns>The index of the terminal recognized at this state, or 0xFFFF if none</returns>
        public ushort GetTerminal(int offset) { return states[offset]; }

        /// <summary>
        /// Checks whether the DFA state at the given state has any transition
        /// </summary>
        /// <param name="offset">The DFA state's offset</param>
        /// <returns>True of the state at the given offset has no transition</returns>
        public bool HasNoTransition(int offset) { return (states[offset + 1] == 0); }

        /// <summary>
        /// Gets the transition corresponding to the given state's index and input value
        /// </summary>
        /// <param name="offset">The DFA state's offset</param>
        /// <returns>The state obtained by the transition, or 0xFFFF if none is found</returns>
        public ushort GetCachedTransition(int offset) { return states[offset]; }

        /// <summary>
        /// Gets the transition corresponding to the given state's index and input value
        /// </summary>
        /// <param name="offset">The DFA state's offset</param>
        /// <param name="value">The input value</param>
        /// <returns>The state obtained by the transition, or 0xFFFF if none is found</returns>
        public ushort GetFallbackTransition(int offset, ushort value)
        {
            int count = states[offset + 1];
            offset += 258;
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
