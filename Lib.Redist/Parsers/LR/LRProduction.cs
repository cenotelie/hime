using System.IO;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a rule's production in a LR parser
    /// </summary>
    /// <remarks>
    /// The binary representation of a LR Production is as follow:
    /// --- header
    /// uint16: head's index
    /// uint8: 1=replace, 0=nothing
    /// uint8: reduction length
    /// uint8: bytecode length in bytes
    /// --- production's bytecode
    /// See LRBytecode
    /// </remarks>
    class LRProduction
    {
        private int head;
        private LRTreeAction headAction;
        private int reducLength;
        private LRBytecode bytecode;

        /// <summary>
        /// Index of the rule's head in the parser's array of variables
        /// </summary>
        public int Head { get { return head; } }
        /// <summary>
        /// Action of the rule's head (replace or not)
        /// </summary>
        public LRTreeAction HeadAction { get { return headAction; } }
        /// <summary>
        /// Size of the rule's body by ony counting terminals and variables
        /// </summary>
        public int ReductionLength { get { return reducLength; } }
        /// <summary>
        /// Bytecode for the rule's production
        /// </summary>
        public LRBytecode Bytecode { get { return bytecode; } }

        /// <summary>
        /// Loads a new instance of the LRProduction class from a binary representation
        /// </summary>
        /// <param name="reader">The binary reader to read from</param>
        public LRProduction(BinaryReader reader)
        {
            this.head = reader.ReadUInt16();
            this.headAction = (LRTreeAction)reader.ReadByte();
            this.reducLength = reader.ReadByte();
            this.bytecode = new LRBytecode(reader.ReadByte());
            reader.Read(bytecode.Raw, 0, this.bytecode.Raw.Length);
        }
    }
}
