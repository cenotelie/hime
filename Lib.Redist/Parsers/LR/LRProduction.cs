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

    public sealed class LRProduction
    {
        public const byte HeadReplace = 1;
        public const byte HeadKeep = 0;

        public const ushort PopNoAction = 0;
        public const ushort PopDrop = 2;
        public const ushort PopPromote = 3;
        public const ushort Virtual = 4;
        public const ushort VirtualNoAction = Virtual + PopNoAction;
        public const ushort VirtualDrop = Virtual + PopDrop;
        public const ushort VirtualPromote = Virtual + PopPromote;
        public const ushort SemanticAction = 8;
        public const ushort NullVariable = 16;
        public const ushort NullVariableNoAction = NullVariable;
        public const ushort NullVariableDrop = NullVariable + PopDrop;
        public const ushort NullVariablePromote = NullVariable + PopPromote;

        private ushort head;
        private byte headAction;
        private byte reducLength;
        private Utils.BlobUShort bytecode;

        public ushort Head { get { return head; } }
        public byte HeadAction { get { return headAction; } }
        public byte ReductionLength { get { return reducLength; } }
        internal Utils.BlobUShort Bytecode { get { return bytecode; } }

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
