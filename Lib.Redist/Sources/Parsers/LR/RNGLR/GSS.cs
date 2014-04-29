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
	/// Represents Graph-Structured Stacks for RNGLR parsers
	/// </summary>
	class GSS
	{
		/// <summary>
		/// Factory for pooled GSS paths
		/// </summary>
		private class GSSPathFactory : Factory<GSSPath>
		{
			/// <summary>
			/// The capacity of the GSS paths produced by this factory
			/// </summary>
			private int capacity;
			/// <summary>
			/// Initializes a new instance of the factory
			/// </summary>
			/// <param name="capacity">The capacity of the GSS paths produced by this factory</param>
			public GSSPathFactory(int capacity)
			{
				this.capacity = capacity;
			}
			/// <summary>
			///  Creates a new object
			/// </summary>
			/// <param name="pool">The enclosing pool</param>
			/// <returns>The created object</returns>
			public GSSPath CreateNew(Pool<GSSPath> pool)
			{
				return new GSSPath(pool, capacity);
			}
		}

		/// <summary>
		/// Factory for pooled GSS nodes
		/// </summary>
		private class GSSNodeFactory : Factory<GSSNode>
		{
			/// <summary>
			///  Creates a new object
			/// </summary>
			/// <param name="pool">The enclosing pool</param>
			/// <returns>The created object</returns>
			public GSSNode CreateNew(Pool<GSSNode> pool)
			{
				return new GSSNode();
			}
		}

		/// <summary>
		/// The initial size of the paths buffer in this GSS
		/// </summary>
		private const int initPathsCount = 64;

		/// <summary>
		/// The pool of GSS paths with a capacity of 128
		/// </summary>
		private Pool<GSSPath> pathsPool128;
		/// <summary>
		/// The pool of GSS paths with a capacity of 1024
		/// </summary>
		private Pool<GSSPath> pathsPool1024;
		/// <summary>
		/// The pool of GSS nodes
		/// </summary>
		private Pool<GSSNode> nodesPool;
		/// <summary>
		/// A single reusable GSS paths for returning 0-length GSS paths
		/// </summary>
		private GSSPath path0;
		/// <summary>
		/// A buffer of GSS paths
		/// </summary>
		private GSSPath[] paths;
		/// <summary>
		/// The index of the next free item in the 'paths' buffer
		/// </summary>
		private int nextIndex;
		/// <summary>
		/// The number of states in the RNGLR automaton
		/// </summary>
		private int nbstates;

		/// <summary>
		/// Initializes the GSS
		/// </summary>
		/// <param name="nbstates">The number of states in the RNGLR automaton</param>
		public GSS(int nbstates)
		{
			this.nbstates = nbstates;
			this.nextIndex = 0;
			this.path0 = new GSSPath();
			this.pathsPool128 = new Pool<GSSPath>(new GSSPathFactory(128), 128);
			this.pathsPool1024 = new Pool<GSSPath>(new GSSPathFactory(1024), 128);
			this.nodesPool = new Pool<GSSNode>(new GSSNodeFactory(), 1024);
			this.paths = new GSSPath[initPathsCount];
		}

		/// <summary>
		/// Gets the next generation for this stack
		/// </summary>
		/// <returns>The next generation</returns>
		public GSSGeneration GetNextGen()
		{
			return new GSSGeneration(this, nextIndex++, nbstates);
		}

		/// <summary>
		/// Acquire a GSS node from a pool of reusable ones
		/// </summary>
		/// <returns>A usable GSS node</returns>
		public GSSNode AcquireNode()
		{
			return nodesPool.Acquire();
		}

		/// <summary>
		/// Returns the given GSS node to the common pool
		/// </summary>
		/// <param name="node">The GSS node to return</param>
		public void ReturnNode(GSSNode node)
		{
			nodesPool.Return(node);
		}

		/// <summary>
		/// Acquires a reusable GSS path with the given length
		/// </summary>
		/// <param name="last">The last RNGLR state in the path</param>
		/// <param name="length">The path's length</param>
		/// <returns>A reusable GSS path</returns>
		private GSSPath AcquirePath(GSSNode last, int length)
		{
			GSSPath p = null;
			if (length <= 128)
				p = pathsPool128.Acquire();
			else if (length <= 1024)
				p = pathsPool1024.Acquire();
			else
				p = new GSSPath(length);
			p.Last = last;
			return p;
		}

		/// <summary>
		/// Grows the 'paths' buffer
		/// </summary>
		private void GrowBuffer()
		{
			GSSPath[] temp = new GSSPath[paths.Length + initPathsCount];
			System.Array.Copy(paths, temp, paths.Length);
			paths = temp;
		}

		/// <summary>
		/// Gets all paths in the GSS starting at the given node and with the given length
		/// </summary>
		/// <param name="from">The starting node</param>
		/// <param name="length">The length of the requested paths</param>
		/// <returns>A collection of paths in this GSS</returns>
		public GSSPaths GetPaths(GSSNode from, int length)
		{
			if (length == 0)
			{
				// use the common 0-length GSS path to avoid new memory allocation
				path0.Last = from;
				paths[0] = path0;
				return new GSSPaths(1, paths);
			}

			// Initializes the first path
			paths[0] = AcquirePath(from, length);

			// The number of paths in the list
			int count = 1;
			// For the remaining hops
			for (int i = 0; i != length; i++)
			{
				int m = 0;          // Insertion index for the compaction process
				int next = count;   // Insertion index for new paths
				for (int p = 0; p != count; p++)
				{
					GSSNode last = paths[p].Last;
					// The path stops here
					if (last.EdgesCount == 0)
					{
						// Cleanup
						paths[p].Free();
						continue;
					}
					// Look for new additional paths
					for (int j = 1; j != last.EdgesCount; j++)
					{
						// Extend the list of paths if necessary
						if (next == paths.Length)
							GrowBuffer();
						// Clone and extend the new path
						paths[next] = AcquirePath(last.GetEdgeTarget(j), length);
						paths[next].CopyLabelsFrom(paths[p], i);
						paths[next][i] = last.GetEdgeLabel(j);
						// Go to next insert
						next++;
					}
					// Continue the current path
					paths[m] = paths[p];
					paths[m].Last = last.GetEdgeTarget(0);
					paths[m][i] = last.GetEdgeLabel(0);
					// goto next
					m++;
				}
				// If Some previous paths have been removed (m != count)
				//    and some have been added (next != cout)
				// => Compact the list
				if (m != count && next != count)
					for (int p = count; p != next; p++)
						paths[m++] = paths[p];
				// m is now the exact number of paths
				count = m;
			}

			return new GSSPaths(count, paths);
		}
	}
}