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
	class SPPF : SemanticBody
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
		/// The maximum size of the reduction handle
		/// </summary>
		private const int handleSize = 1024;
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
		/// The number of items popped from the stack
		/// </summary>
		private int popCount;
		/// <summary>
		/// The sub-tree build-up cache
		/// </summary>
		private SubTree cache;
		/// <summary>
		/// The new available node in the current cache
		/// </summary>
		private int cacheNext;
		/// <summary>
		/// The reduction handle represented as the indices of the sub-trees in the cache
		/// </summary>
		private int[] handle;
		/// <summary>
		/// The index of the next available slot in the handle
		/// </summary>
		private int handleNext;

		/// <summary>
		/// The AST being built
		/// </summary>
		private SimpleAST result;

		/// <summary>
		/// Gets the epsilon GSS label
		/// </summary>
		public GSSLabel Epsilon { get { return new GSSLabel(); } }

		#region Implementation of SemanticBody
		/// <summary>
		/// Gets the symbol at the i-th index
		/// </summary>
		/// <param name="index">Index of the symbol</param>
		/// <returns>The symbol at the given index</returns>
		public Symbol this[int index] { get { return result.GetSymbolFor(cache.GetLabelAt(handle[index])); } }

		/// <summary>
		/// Gets the length of this body
		/// </summary>
		public int Length { get { return handleNext; } }
		#endregion

		/// <summary>
		/// Initializes this SPPF
		/// </summary>
		public SPPF(int stackSize, TokenizedText text, SymbolDictionary variables, SymbolDictionary virtuals)
		{
			this.poolSingle = new Pool<SubTree>(new SubTreeFactory(1), 512);
			this.pool128 = new Pool<SubTree>(new SubTreeFactory(128), 128);
			this.pool1024 = new Pool<SubTree>(new SubTreeFactory(1024), 16);
			this.poolHPs = new Pool<HistoryPart>(new HistoryPartFactory(), initHistorySize);
			this.history = new HistoryPart[initHistorySize];
			this.nextHP = 0;
			this.handle = new int[handleSize];
			this.result = new SimpleAST(text, variables, virtuals);
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
		public void ReductionPop(TreeAction action)
		{
			SubTree sub = null;
			if (popCount < length - 1)
				sub = path[length - 1 - popCount].Tree;
			else
				sub = first.Tree;

			ReductionInsertSub(sub, action);
			sub.Free();
			popCount++;
		}

		/// <summary>
		/// During a reduction, insert the given sub-tree
		/// </summary>
		/// <param name="sub">The sub-tree</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		private void ReductionInsertSub(SubTree sub, TreeAction action)
		{
			if (sub.GetActionAt(0) == TreeAction.Replace)
			{
				// copy the children to the cache
				sub.CopyChildrenTo(cache, cacheNext);
				// setup the handle
				int index = 1;
				for (int i = 0; i != sub.GetChildrenCountAt(0); i++)
				{
					int size = sub.GetChildrenCountAt(index) + 1;
					handle[handleNext++] = cacheNext;
					cacheNext += size;
					index += size;
				}
			}
			else if (action == TreeAction.Drop)
			{
			}
			else
			{
				if (action != TreeAction.None)
					sub.SetActionAt(0, action);
				// copy the complete sub-tree to the cache
				sub.CopyTo(cache, cacheNext);
				handle[handleNext++] = cacheNext;
				cacheNext += sub.GetChildrenCountAt(0) + 1;
			}
		}

		/// <summary>
		/// During a reduction, inserts a virtual symbol
		/// </summary>
		/// <param name="index">The virtual symbol's index</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		public void ReductionAddVirtual(int index, TreeAction action)
		{
			if (action == TreeAction.Drop)
				return; // why would you do this?
			cache.SetAt(cacheNext, new SymbolRef(SymbolType.Virtual, index), action);
			handle[handleNext++] = cacheNext++;
		}

		/// <summary>
		/// During a reduction, inserts the sub-tree of a nullable variable
		/// </summary>
		/// <param name="nullable">The sub-tree of a nullable variable</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		public void ReductionAddNullable(SubTree nullable, TreeAction action)
		{
			ReductionInsertSub(nullable, action);
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
		/// Applies the promotion tree actions to the cache and commits to the final AST
		/// </summary>
		private void ReduceTree()
		{
			// promotion data
			bool promotion = false;
			int insertion = 1;
			for (int i = 0; i != handleNext; i++)
			{
				switch (cache.GetActionAt(handle[i]))
				{
					case TreeAction.Promote:
						if (promotion)
						{
							// This is not the first promotion
							// Commit the previously promoted node's children
							cache.SetChildrenCountAt(0, insertion - 1);
							cache.CommitChildrenOf(0, result);
							// Reput the previously promoted node in the cache
							cache.Move(0, 1);
							insertion = 2;
						}
						promotion = true;
                        // Save the new promoted node
						cache.Move(handle[i], 0);
                        // Repack the children on the left if any
						int nb = cache.GetChildrenCountAt(0);
						cache.MoveRange(handle[i] + 1, insertion, nb);
						insertion += nb;
						break;
					default:
                        // Commit the children if any
						cache.CommitChildrenOf(handle[i], result);
                        // Repack the sub-root on the left
						if (insertion != handle[i])
							cache.Move(handle[i], insertion);
						insertion++;
						break;
				}
			}
			// finalize the sub-tree data
			cache.SetChildrenCountAt(0, insertion - 1);
		}
	}
}