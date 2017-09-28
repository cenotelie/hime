/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System.IO;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represent an op-code for a LR production
	/// An op-code can be either an instruction or raw data
	/// </summary>
	public struct LROpCode
	{
		/// <summary>
		/// Bit mask for the tree action part of an instruction
		/// </summary>
		private const int MASK_TREE_ACTION = 0x0003;
		/// <summary>
		/// Bit mask for the base part of an instruction
		/// </summary>
		private const int MASK_BASE = 0xFFFC;

		/// <summary>
		/// The op-code value
		/// </summary>
		private readonly int code;

		/// <summary>
		/// Gets the value of the data interpretation of this op-code
		/// </summary>
		public int DataValue { get { return code; } }

		/// <summary>
		/// Gets the tree action included in this code
		/// </summary>
		public TreeAction TreeAction { get { return (TreeAction)(code & MASK_TREE_ACTION); } }

		/// <summary>
		/// Gets the base instruction in this code
		/// </summary>
		public LROpCodeBase Base { get { return (LROpCodeBase)(code & MASK_BASE); } }

		/// <summary>
		/// Loads this op-code from the specified input
		/// </summary>
		/// <param name="input">An input</param>
		public LROpCode(BinaryReader input)
		{
			code = input.ReadUInt16();
		}
	}
}
