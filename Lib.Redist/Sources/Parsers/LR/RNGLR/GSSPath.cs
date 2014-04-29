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
using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a path in a GSS
	/// </summary>
	class GSSPath
	{
		/// <summary>
		/// The pool containing this object
		/// </summary>
		private Pool<GSSPath> pool;
		/// <summary>
		/// The last GSS node in this path
		/// </summary>
		private GSSNode last;
		/// <summary>
		/// The labels on this GSS path
		/// </summary>
		private int[] labels;

		/// <summary>
		/// Gets or sets the final target of this path
		/// </summary>
		public GSSNode Last
		{
			get { return last; }
			set { last = value; }
		}

		/// <summary>
		/// Gets or sets the i-th label of the edges traversed by this path
		/// </summary>
		public int this[int index]
		{
			get { return labels[index]; }
			set { labels[index] = value; }
		}

		/// <summary>
		/// Initializes this path
		/// </summary>
		/// <param name="pool">The parent pool</param>
		/// <param name="capacity">The maximal length of this path</param>
		public GSSPath(Pool<GSSPath> pool, int capacity)
		{
			this.pool = pool;
			this.last = null;
			this.labels = new int[capacity];
		}

		/// <summary>
		/// Initializes this path
		/// </summary>
		/// <param name="capacity">The maximal length of this path</param>
		public GSSPath(int capacity)
		{
			this.pool = null;
			this.last = null;
			this.labels = new int[capacity];
		}

		/// <summary>
		/// Initializes this path as a 0-length path
		/// </summary>
		public GSSPath()
		{
			this.pool = null;
			this.last = null;
			this.labels = null;
		}

		/// <summary>
		/// Copy the content of another path to this one
		/// </summary>
		/// <param name="path">The path to copy</param>
		/// <param name="length">The path's length</param>
		public void CopyLabelsFrom(GSSPath path, int length)
		{
			Array.Copy(path.labels, this.labels, length);
		}

		/// <summary>
		/// Frees this path and returns it ot its pool
		/// </summary>
		public void Free()
		{
			if (pool != null)
				pool.Return(this);
		}
	}
}
