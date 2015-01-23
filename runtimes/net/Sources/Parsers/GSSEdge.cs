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
	/// Represents an edge in a Graph-Structured Stack
	/// </summary>
	struct GSSEdge
	{
		/// <summary>
		/// The index of the node from which this edge starts
		/// </summary>
		private int from;
		/// <summary>
		/// The index of the node to which this edge arrives to
		/// </summary>
		private int to;
		/// <summary>
		/// Gets the index of the node from which this edge starts
		/// </summary>
		public int From { get { return from; } }
		/// <summary>
		/// Gets the index of the node to which this edge arrives to
		/// </summary>
		public int To { get { return to; } }
		/// <summary>
		/// Initializes this edge
		/// </summary>
		/// <param name="from">Index of the node from which this edge starts</param>
		/// <param name="to">Index of the node to which this edge arrives to</param>
		public GSSEdge(int from, int to)
		{
			this.from = from;
			this.to = to;
		}
	}
}