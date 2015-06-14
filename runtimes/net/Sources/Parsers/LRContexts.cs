/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
	/// Represents the contexts opening by transitions from a state
	/// </summary>
	public struct LRContexts
	{
		/// <summary>
		/// The contexts
		/// </summary>
		private readonly ushort[] content;

		/// <summary>
		/// Gets the number of contexts
		/// </summary>
		public int Count { get { return content == null ? 0 : content.Length; } }

		/// <summary>
		/// Gets the i-th context
		/// </summary>
		public int this[int index] { get { return content[index]; } }

		/// <summary>
		/// Loads the contexts from the specified input
		/// </summary>
		/// <param name="input">An input</param>
		public LRContexts(BinaryReader input)
		{
			int count = input.ReadUInt16();
			if (count > 0)
			{
				content = new ushort[count * 2];
				for (int i = 0; i != count * 2; i++)
					content[i] = input.ReadUInt16();
			}
			else
			{
				content = null;
			}
		}

		/// <summary>
		/// Gets whether the specified context opens by a transition using the specified terminal ID
		/// </summary>
		/// <param name="terminalID">The identifier of a terminal</param>
		/// <param name="context">A context</param>
		/// <returns><c>true</c> if the specified context is opened</returns>
		public bool Opens(int terminalID, int context)
		{
			if (content == null)
				return false;
			int index = 0;
			while (index != content.Length && content[index] != terminalID)
				index += 2;
			if (index == content.Length)
				// not found
				return false;
			while (index != content.Length && content[index] == terminalID)
			{
				if (content[index + 1] == context)
					return true;
				index += 2;
			}
			return false;
		}
	}
}