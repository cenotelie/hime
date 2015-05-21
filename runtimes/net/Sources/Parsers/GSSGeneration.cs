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

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a generation in a Graph-Structured Stack
	/// </summary>
	/// <remarks>
	/// Because GSS nodes and edges are always created sequentially,
	/// a generation basically describes a span in a buffer of GSS nodes or edges
	/// </remarks>
	struct GSSGeneration
	{
		/// <summary>
		/// The start index of this generation in the list of nodes
		/// </summary>
		private readonly int start;
		/// <summary>
		/// The number of nodes in this generation
		/// </summary>
		private int count;

		/// <summary>
		/// Gets the start index of this generation in the list of nodes
		/// </summary>
		public int Start { get { return start; } }

		/// <summary>
		/// Gets or sets the number of nodes in this generation
		/// </summary>
		public int Count
		{
			get { return count; }
			set { count = value; }
		}

		/// <summary>
		/// Initializes this generation
		/// </summary>
		/// <param name="start">The start index of this generation in the list of nodes</param>
		public GSSGeneration(int start)
		{
			this.start = start;
			count = 0;
		}
	}
}