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
	/// Represents a LR action in a LR parse table
	/// </summary>
	public struct LRAction
	{
		/// <summary>
		/// The LR action code
		/// </summary>
		private readonly LRActionCode code;
		/// <summary>
		/// The data associated with the action
		/// </summary>
		private readonly int data;

		/// <summary>
		/// Gets the action code
		/// </summary>
		public LRActionCode Code { get { return code; } }

		/// <summary>
		/// Gets the data associated with the action
		/// </summary>
		/// <remarks>
		/// If the code is Reduce, it is the index of the LRProduction
		/// If the code is Shift, it is the index of the next state
		/// </remarks>
		public int Data { get { return data; } }

		/// <summary>
		/// Loads this LR Action from the specified input
		/// </summary>
		/// <param name="input">An input</param>
		public LRAction(BinaryReader input)
		{
			this.code = (LRActionCode)(input.ReadUInt16());
			this.data = input.ReadUInt16();
		}
	}
}
