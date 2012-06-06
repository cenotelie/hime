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
        /// <param name="type">The parent lexer's type</param>
        /// <returns></returns>
        public static TextLexerAutomaton FindAutomaton(System.Type type)
        {
            System.Reflection.Assembly assembly = type.Assembly;
            System.IO.Stream stream = assembly.GetManifestResourceStream(type.Name);
            if (stream != null)
                return new TextLexerAutomaton(stream);
            string[] resources = assembly.GetManifestResourceNames();
            foreach (string existing in resources)
                if (existing.EndsWith(type.Name))
                    return new TextLexerAutomaton(assembly.GetManifestResourceStream(existing));
            return null;
        }

        /// <summary>
        /// Gets the index of the terminal recognized at the given state
        /// </summary>
        /// <param name="state">A DFA state</param>
        /// <returns>The index of the terminal recognized at the given state</returns>
        public ushort GetTerminal(ushort state) { return states.data[table.data[state]]; }

        /// <summary>
        /// Gets the transition for the given state and character value
        /// </summary>
        /// <param name="state">A DFA state</param>
        /// <param name="value">The current input value</param>
        /// <returns>The next DFA state</returns>
        public ushort GetTransition(ushort state, ushort value)
        {
            int offset = table.data[state];
            if (value <= 255)
                return states.data[offset + value + 2];
            int count = states.data[offset + 1];
            offset += 258;
            for (int i = 0; i != count; i++)
            {
                if (value >= states.data[offset] && value <= states.data[offset + 1])
                    return states.data[offset + 2];
                offset += 3;
            }
            return 0xFFFF;
        }
    }
}
