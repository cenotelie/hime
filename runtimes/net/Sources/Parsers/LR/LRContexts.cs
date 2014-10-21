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
	/// Represents the contexts of a LR state
	/// </summary>
	public struct LRContexts
	{
		/// <summary>
		/// The contexts
		/// </summary>
		private ushort[] content;
		
		/// <summary>
		/// Gets the number of contexts
		/// </summary>
		public int Count { get { return content.Length; } }
		
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
			this.content = new ushort[count];
			for (int i = 0; i != count; i++)
				this.content[i] = input.ReadUInt16();
		}
		
		/// <summary>
		/// Gets whether the specified context is in this collection
		/// </summary>
		/// <param name="context">A context</param>
		/// <returns><c>true</c> if the specified context is in this collection</returns>
		public bool Contains(int context)
		{
			for (int i = 0; i != content.Length; i++)
				if (content[i] == context)
					return true;
			return false;
		}
	}
}