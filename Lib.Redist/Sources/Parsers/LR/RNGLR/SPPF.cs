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
	/// Represents a Shared Packed Parse Forest
	/// </summary>
	class SPPF
	{
		/// <summary>
		/// The initial size of the history buffer
		/// </summary>
		private const int initHistorySize = 8;
		/// <summary>
		/// The initial size of the history parts' buffers
		/// </summary>
		private const int initHistoryPartSize = 64;
		/// <summary>
		/// The bias for estimating the size of the reduced sub-tree
		/// </summary>
		private const int estimationBias = 5;

		/// <summary>
		/// Represents a generation of node in the current history
		/// </summary>
		private class HistoryPart
		{
			/// <summary>
			/// The index of the represented GSS generation
			/// </summary>
			public int generation;
			/// <summary>
			/// The GSS labels in this part
			/// </summary>
			public GSSLabel[] data;
			/// <summary>
			/// The next available slot in the data
			/// </summary>
			public int next;
			/// <summary>
			/// Initializes a new instance
			/// </summary>
			public HistoryPart()
			{
				this.generation = 0;
				this.data = new GSSLabel[initHistoryPartSize];
				this.next = 0;
			}
		}

		/// <summary>
		/// Represents a factory of history parts
		/// </summary>
		private class HistoryPartFactory : Factory<HistoryPart>
		{
			/// <summary>
			///  Creates a new object
			/// </summary>
			/// <param name="pool">The enclosing pool</param>
			/// <returns>The created object</returns>
			public HistoryPart CreateNew(Pool<HistoryPart> pool)
			{
				return new HistoryPart();
			}
		}


		/// <summary>
		/// The pool of single node sub-trees
		/// </summary>
		private Pool<SubTree> poolSingle;
		/// <summary>
		/// The pool of sub-tree with a capacity of 128 nodes
		/// </summary>
		private Pool<SubTree> pool128;
		/// <summary>
		/// The pool of sub-tree with a capacity of 1024 nodes
		/// </summary>
		private Pool<SubTree> pool1024;
		/// <summary>
		/// The pool of history parts
		/// </summary>
		private Pool<HistoryPart> poolHPs;

		/// <summary>
		/// The history
		/// </summary>
		private HistoryPart[] history;
		/// <summary>
		/// The next available slot for a history part
		/// </summary>
		private int nextHP;

		/// <summary>
		/// Gets the epsilon GSS label
		/// </summary>
		public GSSLabel Epsilon { get { return new GSSLabel(); } }

		/// <summary>
		/// Initializes this SPPF
		/// </summary>
		public SPPF()
		{
			this.poolSingle = new Pool<SubTree>(new SubTreeFactory(1), 512);
			this.pool128 = new Pool<SubTree>(new SubTreeFactory(128), 128);
			this.pool1024 = new Pool<SubTree>(new SubTreeFactory(1024), 16);
			this.poolHPs = new Pool<HistoryPart>(new HistoryPartFactory(), initHistorySize);
			this.history = new HistoryPart[initHistorySize];
			this.nextHP = 0;
		}

		/// <summary>
		/// Gets a sub-tree for a single node
		/// </summary>
		/// <returns>A sub-tree</returns>
		public SubTree GetSingleNode()
		{
			return poolSingle.Acquire();
		}

		/// <summary>
		/// Gets a pooled sub-tree with the given maximal size
		/// </summary>
		/// <param name="size">The size of the sub-tree</param>
		/// <returns>A sub-tree of the required size</returns>
		private SubTree GetSubTree(int size)
		{
			if (size <= 128)
				return pool128.Acquire();
			else if (size <= 1024)
				return pool1024.Acquire();
			else
				return new SubTree(null, size);
		}

		/// <summary>
		/// Gets the history part for the given GSS generation
		/// </summary>
		/// <param name="generation">The index of a GSS generation</param>
		/// <returns>The corresponding history part, or <c>null</c></returns>
		private HistoryPart GetHistoryPart(int generation)
		{
			for (int i = 0; i != nextHP; i++)
				if (history[i].generation == generation)
					return history[i];
			return null;
		}

		/// <summary>
		/// Clears the current history
		/// </summary>
		public void ClearHistory()
		{
			for (int i = 0; i != nextHP; i++)
				poolHPs.Return(history[i]);
			nextHP = 0;
		}

		/// <summary>
		/// Gets the GSS label already in history for the given GSS generation and symbol
		/// </summary>
		/// <param name="generation">The index of a GSS generation</param>
		/// <param name="symbol">A symbol to look for</param>
		/// <returns>The existing GSS label, or the epsilon label</returns>
		public GSSLabel GetLabelFor(int generation, SymbolRef symbol)
		{
			HistoryPart hp = GetHistoryPart(generation);
			if (hp == null)
				return new GSSLabel();
			for (int i = 0; i != hp.next; i++)
				if (hp.data[i].Original == symbol)
					return hp.data[i];
			return new GSSLabel();
		}

		/// <summary>
		/// Creates a new label in the history
		/// </summary>
		/// <param name="generation">The index of a GSS generation</param>
		/// <param name="symbol">The sub-tree root symbol</param>
		/// <param name="action">The action applied on the sub-tree root</param>
		/// <param name="length">The size of the children</param>
		public GSSLabel NewLabelFor(int generation, SymbolRef symbol, TreeAction action, int length)
		{
			HistoryPart hp = GetHistoryPart(generation);
			if (hp == null)
			{
				hp = poolHPs.Acquire();
				hp.generation = generation;
				hp.next = 0;
				if (history.Length == nextHP)
				{
					HistoryPart[] temp = new HistoryPart[history.Length + initHistorySize];
					Array.Copy(history, temp, history.Length);
					history = temp;
				}
				history[nextHP++] = hp;
			}
			SubTree st = GetSubTree(length);
			st.SetupRoot(symbol, action);
			GSSLabel result = new GSSLabel(st);
			hp.data[hp.next++] = result;
			return result;
		}
	}
}