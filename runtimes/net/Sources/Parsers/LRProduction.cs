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
	/// uint8: bytecode length in number of op-code
	/// --- production's bytecode
	/// See LRBytecode
	/// </remarks>
	public class LRProduction
	{
		/// <summary>
		/// Index of the rule's head in the parser's array of variables
		/// </summary>
		private readonly int head;
		/// <summary>
		/// Action of the rule's head (replace or not)
		/// </summary>
		private readonly TreeAction headAction;
		/// <summary>
		/// Size of the rule's body by only counting terminals and variables
		/// </summary>
		private readonly int reducLength;
		/// <summary>
		/// Bytecode for the rule's production
		/// </summary>
		private readonly LROpCode[] bytecode;

		/// <summary>
		/// Gets the index of the rule's head in the parser's array of variables
		/// </summary>
		public int Head { get { return head; } }

		/// <summary>
		/// Gets the action of the rule's head (replace or not)
		/// </summary>
		public TreeAction HeadAction { get { return headAction; } }

		/// <summary>
		/// Gets the size of the rule's body by only counting terminals and variables
		/// </summary>
		public int ReductionLength { get { return reducLength; } }

		/// <summary>
		/// Gets the length of the bytecode
		/// </summary>
		public int BytecodeLength { get { return bytecode.Length; } }

		/// <summary>
		/// Gets the op-code at the specified index in the bytecode
		/// </summary>
		/// <param name="index">Index in the bytecode</param>
		public LROpCode this[int index] { get { return bytecode[index]; } }

		/// <summary>
		/// Loads a new instance of the LRProduction class from a binary representation
		/// </summary>
		/// <param name="reader">The binary reader to read from</param>
		public LRProduction(BinaryReader reader)
		{
			head = reader.ReadUInt16();
			headAction = (TreeAction)reader.ReadByte();
			reducLength = reader.ReadByte();
			bytecode = new LROpCode[reader.ReadByte()];
			for (int i = 0; i != bytecode.Length; i++)
				bytecode[i] = new LROpCode(reader);
		}
	}
}
