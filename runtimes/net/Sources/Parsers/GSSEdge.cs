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
		private readonly int from;
		/// <summary>
		/// The index of the node to which this edge arrives to
		/// </summary>
		private readonly int to;
		/// <summary>
		/// The identifier of the SPPF node that serve as label for this edge
		/// </summary>
		private readonly int label;

		/// <summary>
		/// Gets the index of the node from which this edge starts
		/// </summary>
		public int From { get { return from; } }

		/// <summary>
		/// Gets the index of the node to which this edge arrives to
		/// </summary>
		public int To { get { return to; } }

		/// <summary>
		/// Gets the identifier of the SPPF node that serve as label for this edge
		/// </summary>
		public int Label { get { return label; } }

		/// <summary>
		/// Initializes this edge
		/// </summary>
		/// <param name="from">Index of the node from which this edge starts</param>
		/// <param name="to">Index of the node to which this edge arrives to</param>
		/// <param name="label">The identifier of the SPPF node that serve as label for this edge</param>
		public GSSEdge(int from, int to, int label)
		{
			this.from = from;
			this.to = to;
			this.label = label;
		}
	}
}