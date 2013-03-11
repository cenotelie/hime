/*
 * Author: Laurent Wouters
 * Date: 02/06/2012
 * Time: 10:15
 * 
 */
using System.IO;

namespace Hime.Redist.Parsers
{
    /* 
    * uint16: head's index
    * uint8: 1=replace, 0=nothing
    * uint8: reduction length
    * uint8: bytecode length
    * List of elements of the form:
    * -- ast pop
    * uint16: 0=no action, 2=drop, 3=promote
    * -- add virtual
    * uint16: 4=no action, 6=drop, 7=promote
    * uint16: virtual's index
    * -- semantic action
    * uint16: 8
    * uint16: action's index
    * --- null variable
    * uint16: 16 + action
    * uint16: variable's index
    */

    /// <summary>
    /// Represents a rule's production in a LR parser
    /// </summary>
    public sealed class LRProduction
    {
        /// <summary>
        /// In the CST, the root node will be replaced by its children
        /// </summary>
        public const byte HeadReplace = 1;
        /// <summary>
        /// In the CST, keep the root node
        /// </summary>
        public const byte HeadKeep = 0;

        private ushort head;
        private byte headAction;
        private byte reducLength;
        private Utils.BlobUShort bytecode;

        /// <summary>
        /// Index of the rule's head in the parser's array of variables
        /// </summary>
        public ushort Head { get { return head; } }
        /// <summary>
        /// Action of the rule's head (replace or not)
        /// </summary>
        public byte HeadAction { get { return headAction; } }
        /// <summary>
        /// Size of the rule's body by ony counting terminals and variables
        /// </summary>
        public byte ReductionLength { get { return reducLength; } }
        /// <summary>
        /// Bytecode for the rule's production
        /// </summary>
        internal Utils.BlobUShort Bytecode { get { return bytecode; } }

        /// <summary>
        /// Loads a new instance of the LRProduction class from a binary representation
        /// </summary>
        /// <param name="reader">The binary reader to read from</param>
        public LRProduction(BinaryReader reader)
        {
            this.head = reader.ReadUInt16();
            this.headAction = reader.ReadByte();
            this.reducLength = reader.ReadByte();
            this.bytecode = new Utils.BlobUShort(reader.ReadByte());
            reader.Read(bytecode.Raw, 0, this.bytecode.RawSize);
        }
    }
}
