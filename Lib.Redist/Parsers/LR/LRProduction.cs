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
    */

    public sealed class LRProduction
    {
        public const ushort PopNoAction = (ushort)CSTAction.Nothing;
        public const ushort PopDrop = (ushort)CSTAction.Drop;
        public const ushort PopPromote = (ushort)CSTAction.Promote;
        public const ushort Virtual = 4;
        public const ushort VirtualNoAction = Virtual + (ushort)CSTAction.Nothing;
        public const ushort VirtualDrop = Virtual + (ushort)CSTAction.Drop;
        public const ushort VirtualPromote = Virtual + (ushort)CSTAction.Promote;
        public const ushort SemanticAction = 8;

        private ushort head;
        private byte headAction;
        private byte reducLength;
        private byte bytecodeLength;
        private Utils.BlobUShort bytecode;

        public ushort Head { get { return head; } }
        public byte HeadAction { get { return headAction; } }
        public byte ReductionLength { get { return reducLength; } }
        public ushort[] Bytecode { get { return bytecode.Data; } }
        public int BytecodeLength { get { return bytecodeLength; } }

        public LRProduction(BinaryReader stream)
        {
            this.head = stream.ReadUInt16();
            this.headAction = stream.ReadByte();
            this.reducLength = stream.ReadByte();
            this.bytecodeLength = stream.ReadByte();
            byte[] buffer = new byte[this.bytecodeLength];
            stream.Read(buffer, 0, this.bytecodeLength);
            this.bytecode = new Utils.BlobUShort(buffer);
            this.bytecodeLength = (byte)(this.bytecodeLength >> 1);
        }
    }
}
