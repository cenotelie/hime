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
	class SPPF : LRkASTBuilder
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
		/// Represents a generation of node in the current history
		/// </summary>
		private class HistoryPart
		{
			/// <summary>
			/// The GSS labels in this part
			/// </summary>
			public GSSLabel[] data;
			/// <summary>
			/// The index of the represented GSS generation
			/// </summary>
			public int generation;
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
		/// The GSS path being reduced
		/// </summary>
		private GSSPath path;
		/// <summary>
		/// The first label on the complete GSS path
		/// </summary>
		private GSSLabel first;
		/// <summary>
		/// The reduction length
		/// </summary>
		private int length;

		/// <summary>
		/// Gets the epsilon GSS label
		/// </summary>
		public GSSLabel Epsilon { get { return new GSSLabel(); } }


		/// <summary>
		/// Initializes this SPPF
		/// </summary>
		public SPPF(int stackSize, TokenizedText text, SymbolDictionary variables, SymbolDictionary virtuals)
			: base(stackSize, text, variables, virtuals)
		{
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
		/// Prepares for the forthcoming reduction operations
		/// </summary>
		/// <param name="varIndex">The reduced variable index</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		/// <param name="first">The first label</param>
		/// <param name="path">The path being reduced</param>
		/// <param name="length">The reduction length</param>
		public void ReductionPrepare(int varIndex, TreeAction action, GSSLabel first, GSSPath path, int length)
		{
			this.path = path;
			this.first = first;
			this.length = length;

			int estimation = estimationBias;
			if (length > 0)
			{
				for (int i = 0; i != length - 1; i++)
					estimation += path[i].Tree.GetSize();
				estimation += first.Tree.GetSize();
			}

			cache = GetSubTree(estimation);
			cache.SetupRoot(new SymbolRef(SymbolType.Variable, varIndex), action);
			cacheNext = 1;
			handleNext = 0;
			popCount = 0;
		}

		/// <summary>
		/// During a redution, pops the top symbol from the stack and gives it a tree action
		/// </summary>
		/// <param name="action">The tree action to apply to the symbol</param>
		public new void ReductionPop(TreeAction action)
		{
			SubTree sub = null;
			if (popCount < length - 1)
				sub = path[length - 2 - popCount].Tree;
			else
				sub = first.Tree;

			ReductionAddSub(sub, action);
			sub.Free();
			popCount++;
		}

		/// <summary>
		/// During a reduction, inserts the sub-tree of a nullable variable
		/// </summary>
		/// <param name="nullable">The sub-tree of a nullable variable</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		public void ReductionAddNullable(SubTree nullable, TreeAction action)
		{
			ReductionAddSub(nullable, action);
		}

		/// <summary>
		/// Finalizes the reduction operation
		/// </summary>
		/// <param name="generation">The generation to reduce from</param>
		/// <param name="varIndex">The reduced variable index</param>
		/// <returns>The produced sub-tree</returns>
		public GSSLabel Reduce(int generation, int varIndex)
		{
			if (cache.GetActionAt(0) == TreeAction.Replace)
			{
				cache.SetChildrenCountAt(0, handleNext);
			}
			else
			{
				ReduceTree();
			}

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
			GSSLabel result = new GSSLabel(cache, new SymbolRef(SymbolType.Variable, varIndex));
			hp.data[hp.next++] = result;
			return result;
		}

		/// <summary>
		/// Finalizes the parse tree and returns it
		/// </summary>
		/// <param name="root">The root's sub-tree</param>
		/// <returns>The final parse tree</returns>
		public AST GetTree(SubTree root)
		{
			// Commit the remaining sub-tree
			root.Commit(result);
			return result;
		}
	}
}