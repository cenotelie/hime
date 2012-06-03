/*
 * Author: Laurent Wouters
 * Date: 02/06/2012
 * Time: 10:15
 * 
 */
using System.IO;

namespace Hime.Redist.Parsers.LR
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

    public sealed class LRkProduction
    {
        private ushort head;
        private byte headAction;
        private byte reducLength;
        private Utils.BlobUShort bytecode;

        public ushort Head { get { return head; } }
        public byte HeadAction { get { return headAction; } }
        public byte ReductionLength { get { return reducLength; } }
        public ushort[] Bytecode { get { return bytecode.data; } }

        public LRkProduction(BinaryReader stream)
        {
            this.head = stream.ReadUInt16();
            this.headAction = stream.ReadByte();
            this.reducLength = stream.ReadByte();
            byte length = stream.ReadByte();
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            this.bytecode = new Utils.BlobUShort(buffer);
        }
    }
}
