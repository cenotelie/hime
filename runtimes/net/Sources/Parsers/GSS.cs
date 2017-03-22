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

using System;
using System.IO;
using System.Collections.Generic;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents Graph-Structured Stacks for GLR parsers
	/// </summary>
	class GSS
	{
		/// <summary>
		/// The initial size of the paths buffer in this GSS
		/// </summary>
		private const int INIT_PATHS_COUNT = 64;

		/// <summary>
		/// The label (GLR state) on the GSS node for the given index
		/// </summary>
		private readonly BigList<int> nodeLabels;
		/// <summary>
		/// The generations of nodes in this GSS
		/// </summary>
		private readonly BigList<GSSGeneration> nodeGenerations;

		/// <summary>
		/// The edges in this GSS
		/// </summary>
		private readonly BigList<GSSEdge> edges;
		/// <summary>
		/// The generations for the edges
		/// </summary>
		private readonly BigList<GSSGeneration> edgeGenerations;

		/// <summary>
		/// Index of the current generation
		/// </summary>
		private int generation;

		/// <summary>
		/// A single reusable GSS paths for returning 0-length GSS paths
		/// </summary>
		private readonly GSSPath path0;
		/// <summary>
		/// The single reusable buffer for returning 0-length GSS paths
		/// </summary>
		private readonly GSSPath[] paths0;
		/// <summary>
		/// A buffer of GSS paths
		/// </summary>
		private GSSPath[] paths;

		/// <summary>
		/// Initializes the GSS
		/// </summary>
		public GSS()
		{
			nodeLabels = new BigList<int>();
			nodeGenerations = new BigList<GSSGeneration>();
			edges = new BigList<GSSEdge>();
			edgeGenerations = new BigList<GSSGeneration>();
			generation = -1;
			path0 = new GSSPath();
			paths0 = new [] { path0 };
			paths = new GSSPath[INIT_PATHS_COUNT];
		}

		/// <summary>
		/// Gets the data of the current generation
		/// </summary>
		/// <returns>The generation's data</returns>
		public GSSGeneration GetGeneration()
		{
			return nodeGenerations[generation];
		}

		/// <summary>
		/// Gets the data of the specified generation of nodes
		/// </summary>
		/// <param name="generation">A generation</param>
		/// <returns>The generation's data</returns>
		public GSSGeneration GetGeneration(int generation)
		{
			return nodeGenerations[generation];
		}

		/// <summary>
		/// Gets the GLR state represented by the specified node
		/// </summary>
		/// <param name="node">A node</param>
		/// <returns>The GLR state represented by the node</returns>
		public int GetRepresentedState(int node)
		{
			return nodeLabels[node];
		}

		/// <summary>
		/// Finds in the given generation a node representing the given GLR state
		/// </summary>
		/// <param name="generation">A generation</param>
		/// <param name="state">A GLR state</param>
		/// <returns>The node representing the GLR state, or -1 if it is not found</returns>
		public int FindNode(int generation, int state)
		{
			GSSGeneration data = nodeGenerations[generation];
			for (int i = data.Start; i != data.Start + data.Count; i++)
				if (nodeLabels[i] == state)
					return i;
			return -1;
		}

		/// <summary>
		/// Determines whether this instance has the required edge
		/// </summary>
		/// <param name="generation">The generation of the edge's start node</param>
		/// <param name="from">The edge's start node</param>
		/// <param name="to">The edge's target node</param>
		/// <returns><c>true</c> if this instance has the required edge; otherwise, <c>false</c></returns>
		public bool HasEdge(int generation, int from, int to)
		{
			GSSGeneration data = edgeGenerations[generation];
			for (int i = data.Start; i != data.Start + data.Count; i++)
			{
				GSSEdge edge = edges[i];
				if (edge.From == from && edge.To == to)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Opens a new generation in this GSS
		/// </summary>
		/// <returns>The index of the new generation</returns>
		public int CreateGeneration()
		{
			nodeGenerations.Add(new GSSGeneration(nodeLabels.Size));
			edgeGenerations.Add(new GSSGeneration(edges.Size));
			generation++;
			return generation;
		}

		/// <summary>
		/// Creates a new node in the GSS
		/// </summary>
		/// <param name="state">The GLR state represented by the node</param>
		/// <returns>The node's identifier</returns>
		public int CreateNode(int state)
		{
			int node = nodeLabels.Add(state);
			GSSGeneration data = nodeGenerations[generation];
			data.Count++;
			nodeGenerations[generation] = data;
			return node;
		}

		/// <summary>
		/// Creates a new edge in the GSS
		/// </summary>
		/// <param name="from">The edge's starting node</param>
		/// <param name="to">The edge's target node</param>
		/// <param name="label">The edge's label</param>
		public void CreateEdge(int from, int to, int label)
		{
			edges.Add(new GSSEdge(from, to, label));
			GSSGeneration data = edgeGenerations[generation];
			data.Count++;
			edgeGenerations[generation] = data;
		}

		/// <summary>
		/// Setups a reusable GSS path with the given length
		/// </summary>
		/// <param name="index">The index in the buffer of reusable paths</param>
		/// <param name="last">The last GLR state in the path</param>
		/// <param name="length">The path's length</param>
		private void SetupPath(int index, int last, int length)
		{
			if (index >= paths.Length)
				Array.Resize(ref paths, paths.Length + INIT_PATHS_COUNT);
			if (paths[index] == null)
				paths[index] = new GSSPath(length);
			else
				paths[index].Ensure(length);
			paths[index].Last = last;
			paths[index].Generation = GetGenerationOf(last);
		}

		/// <summary>
		/// Retrieve the generation of the given node in this GSS
		/// </summary>
		/// <param name="node">A node's index</param>
		/// <returns>The index of the generation containing the node</returns>
		private int GetGenerationOf(int node)
		{
			for (int i = generation; i != -1; i--)
			{
				GSSGeneration gen = nodeGenerations[i];
				if (node >= gen.Start && node < gen.Start + gen.Count)
					return i;
			}
			// should node happen
			return -1;
		}

		/// <summary>
		/// Gets all paths in the GSS starting at the given node and with the given length
		/// </summary>
		/// <param name="from">The starting node</param>
		/// <param name="length">The length of the requested paths</param>
		/// <param name="count">The number of paths</param>
		/// <returns>A collection of paths in this GSS</returns>
		public GSSPath[] GetPaths(int from, int length, out int count)
		{
			if (length == 0)
			{
				// use the common 0-length GSS path to avoid new memory allocation
				path0.Last = from;
				count = 1;
				return paths0;
			}

			// Initializes the first path
			SetupPath(0, from, length);

			// The number of paths in the list
			int total = 1;
			// For the remaining hops
			for (int i = 0; i != length; i++)
			{
				int m = 0;          // Insertion index for the compaction process
				int next = total;   // Insertion index for new paths
				for (int p = 0; p != total; p++)
				{
					int last = paths[p].Last;
					int genIndex = paths[p].Generation;
					// Look for new additional paths from last
					GSSGeneration gen = edgeGenerations[genIndex];
					int firstEdgeTarget = -1;
					int firstEdgeLabel = -1;
					for (int e = gen.Start; e != gen.Start + gen.Count; e++)
					{
						GSSEdge edge = edges[e];
						if (edge.From == last)
						{
							if (firstEdgeTarget == -1)
							{
								// This is the first edge
								firstEdgeTarget = edge.To;
								firstEdgeLabel = edge.Label;
							}
							else
							{
								// Not the first edge
								// Clone and extend the new path
								SetupPath(next, edge.To, length);
								paths[next].CopyLabelsFrom(paths[p], i);
								paths[next][i] = edge.Label;
								// Go to next insert
								next++;
							}
						}
					}
					// Check whether there was at least one edge
					if (firstEdgeTarget != -1)
					{
						// Continue the current path
						if (m != p)
						{
							GSSPath t = paths[m];
							paths[m] = paths[p];
							paths[p] = t;
						}
						paths[m].Last = firstEdgeTarget;
						paths[m].Generation = GetGenerationOf(firstEdgeTarget);
						paths[m][i] = firstEdgeLabel;
						// goto next
						m++;
					}
				}
				if (m != total)
				{
					// if some previous paths have been removed
					// => compact the list if needed
					for (int p = total; p != next; p++)
					{
						GSSPath t = paths[m];
						paths[m] = paths[p];
						paths[p] = t;
						m++;
					}
					// m is now the exact number of paths
					total = m;
				}
				else if (next != total)
				{
					// no path has been removed, but some have been added
					// => next is the exact number of paths
					total = next;
				}
			}

			count = total;
			return paths;
		}

		/// <summary>
		/// Prints this stack onto the console output
		/// </summary>
		public void Print()
		{
			PrintTo(Console.Out);
		}

		/// <summary>
		/// Prints this stack into the specified file
		/// </summary>
		/// <param name="file">The file to print to</param>
		public void PrintTo(string file)
		{
			TextWriter writer = new StreamWriter(file, false, System.Text.Encoding.UTF8);
			PrintTo(writer);
			writer.Close();
		}

		/// <summary>
		/// Prints this stack with the specified writer
		/// </summary>
		/// <param name="writer">A text writer</param>
		public void PrintTo(TextWriter writer)
		{
			// list of all nodes having at least one child
			List<int> linked = new List<int>();

			for (int i = generation; i != -1; i--)
			{
				writer.WriteLine("--- generation {0} ---", i);
				// Retrieve the edges in this generation
				Dictionary<int, List<int>> myedges = new Dictionary<int, List<int>>();
				GSSGeneration cedges = edgeGenerations[i];
				for (int j = 0; j != cedges.Count; j++)
				{
					GSSEdge edge = edges[cedges.Start + j];
					if (!myedges.ContainsKey(edge.From))
						myedges.Add(edge.From, new List<int>());
					myedges[edge.From].Add(edge.To);
					if (!linked.Contains(edge.To))
						linked.Add(edge.To);
				}
				// Retrieve the nodes in this generation and reverse their order
				GSSGeneration cnodes = nodeGenerations[i];
				List<int> mynodes = new List<int>();
				for (int j = 0; j != cnodes.Count; j++)
					mynodes.Add(cnodes.Start + j);
				mynodes.Reverse();
				// print this generation
				foreach (int node in mynodes)
				{
					string mark = linked.Contains(node) ? "node" : "head";
					if (myedges.ContainsKey(node))
					{
						foreach (int to in myedges[node])
						{
							int gen = GetGenerationOf(to);
							if (gen == i)
								writer.WriteLine("\t{0} {1} to {2}", mark, nodeLabels[node], nodeLabels[to]);
							else
								writer.WriteLine("\t{0} {1} to {2} in gen {3}", mark, nodeLabels[node], nodeLabels[to], gen);
						}
					}
					else
					{
						writer.WriteLine("\t{0} {1}", mark, nodeLabels[node]);
					}
				}
			}
		}
	}
}