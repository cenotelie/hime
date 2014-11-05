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
using System.Collections.Generic;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a structure that helps build a Shared Packed Parse Forest (SPPF)
	/// </summary>
	/// <remarks>
	/// A SPPF is a compact representation of multiple variants of an AST at once.
	/// GLR algorithms originally builds the complete SPPF.
	/// However we only need to build one of the variant, i.e. an AST for the user.
	/// </remarks>
	class SPPFBuilder : SemanticBody
	{
		/// <summary>
		/// The initial size of the reduction handle
		/// </summary>
		private const int INIT_HANDLE_SIZE = 1024;
		/// <summary>
		/// The initial size of the history buffer
		/// </summary>
		private const int INIT_HISTORY_SIZE = 8;
		/// <summary>
		/// The initial size of the history parts' buffers
		/// </summary>
		private const int INIT_HISTORY_PART_SIZE = 64;

		/// <summary>
		/// Represents a generation of GSS edges in the current history
		/// The history is used to quickly find pre-existing matching GSS edges
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
				this.data = new GSSLabel[INIT_HISTORY_PART_SIZE];
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
		/// The pool of sub-tree with a capacity of 8 nodes
		/// </summary>
		private Pool<SubTree> pool8;
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
		/// The adjacency cache for the reduction
		/// </summary>
		private int[] cacheChildren;
		/// <summary>
		/// The actions cache for the reduction
		/// </summary>
		private TreeAction[] cacheActions;
		/// <summary>
		/// The new available slot in the current cache
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
		/// The stack of semantic objects for the reduction
		/// </summary>
		private GSSLabel[] stack;
		/// <summary>
		/// The number of items popped from the stack
		/// </summary>
		private int popCount;

		/// <summary>
		/// The AST being built
		/// </summary>
		private GraphAST result;

		#region Implementation of SemanticBody
		/// <summary>
		/// Gets the symbol at the i-th index
		/// </summary>
		/// <param name="index">Index of the symbol</param>
		/// <returns>The symbol at the given index</returns>
		public Symbol this[int index] { get { return  result.GetSymbol(cacheChildren[handle[index]]); } }

		/// <summary>
		/// Gets the length of this body
		/// </summary>
		public int Length { get { return handleNext; } }
		#endregion

		/// <summary>
		/// Gets the epsilon GSS label
		/// </summary>
		public GSSLabel Epsilon { get { return new GSSLabel(null); } }

		/// <summary>
		/// Initializes this SPPF
		/// </summary>
		/// <param name="text">The tokenined text</param>
		/// <param name="variables">The table of parser variables</param>
		/// <param name="virtuals">The table of parser virtuals</param>
		public SPPFBuilder(TokenizedText text, IList<Symbol> variables, IList<Symbol> virtuals)
		{
			this.pool8 = new Pool<SubTree>(new SubTreeFactory(8), 1024);
			this.pool128 = new Pool<SubTree>(new SubTreeFactory(128), 128);
			this.pool1024 = new Pool<SubTree>(new SubTreeFactory(1024), 16);
			this.poolHPs = new Pool<HistoryPart>(new HistoryPartFactory(), INIT_HISTORY_SIZE);
			this.history = new HistoryPart[INIT_HISTORY_SIZE];
			this.nextHP = 0;
			this.cacheChildren = new int[INIT_HANDLE_SIZE];
			this.cacheActions = new TreeAction[INIT_HANDLE_SIZE];
			this.handle = new int[INIT_HANDLE_SIZE];
			this.stack = new GSSLabel[INIT_HANDLE_SIZE];
			this.result = new GraphAST(text, variables, virtuals);
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
				return Epsilon;
			for (int i = 0; i != hp.next; i++)
			{
				if (hp.data[i].IsReplaceable)
				{
					if (hp.data[i].ReplaceableTree.GetLabelAt(0) == symbol)
						return hp.data[i];
				}
				else
				{
					if (hp.data[i].Original == symbol)
						return hp.data[i];
				}
			}
			return Epsilon;
		}

		/// <summary>
		/// Creates a single node in the result SPPF an returns it
		/// </summary>
		/// <param name="symbol">The symbol as the node's label</param>
		/// <returns>The created node's index in the SPPF</returns>
		public int GetSingleNode(SymbolRef symbol)
		{
			return result.Store(symbol);
		}

		/// <summary>
		/// Gets a pooled sub-tree with the given maximal size
		/// </summary>
		/// <param name="size">The size of the sub-tree</param>
		private SubTree GetSubTree(int size)
		{
			if (size <= 8)
				return pool8.Acquire();
			if (size <= 128)
				return pool128.Acquire();
			if (size <= 1024)
				return pool1024.Acquire();
			return new SubTree(null, size);
		}

		/// <summary>
		/// Prepares for the forthcoming reduction operations
		/// </summary>
		/// <param name="first">The first label</param>
		/// <param name="path">The path being reduced</param>
		/// <param name="length">The reduction length</param>
		public void ReductionPrepare(GSSLabel first, GSSPath path, int length)
		{
			// build the stack
			if (length > 0)
			{
				for (int i=0; i<length-1; i++)
					stack[i] = path[length - 2 - i];
				stack[length - 1] = first;
			}
			// initialize the reduction data
			this.cacheNext = 0;
			this.handleNext = 0;
			this.popCount = 0;
		}

		/// <summary>
		/// During a reduction, pops the top symbol from the stack and gives it a tree action
		/// </summary>
		/// <param name="action">The tree action to apply to the symbol</param>
		public void ReductionPop(TreeAction action)
		{
			AddToCache(stack[popCount++], action);
		}

		/// <summary>
		/// Adds the specified GSS label to the reduction cache with the given tree action
		/// </summary>
		/// <param name="label">The label to add to the cache</param>
		/// <param name="action">The tree action to apply</param>
		private void AddToCache(GSSLabel label, TreeAction action)
		{
			if (action == TreeAction.Drop)
				return;
			if (label.IsReplaceable)
			{
				// this is replaceable sub-tree
				SubTree sub = label.ReplaceableTree;
				for (int i = 0; i != sub.GetChildrenCountAt(0); i++)
					AddToCache(sub.GetLabelAt(i + 1).Index, sub.GetActionAt(i + 1));
			}
			else
			{
				// this is a simple reference to an existing SPPF node
				AddToCache(label.NodeIndex, action);
			}
		}

		/// <summary>
		/// Adds the specified SPPF node to the cache
		/// </summary>
		/// <param name="node">The node to add to the cache</param>
		/// <param name="action">The tree action to apply onto the node</param>
		private void AddToCache(int node, TreeAction action)
		{
			int count = result.GetChildrenCount(node);
			if (cacheNext + count >= cacheChildren.Length)
			{
				// the current cache is not big enough, build a bigger one
				int[] t1 = new int[cacheChildren.Length + INIT_HANDLE_SIZE];
				TreeAction[] t2 = new TreeAction[cacheActions.Length + INIT_HANDLE_SIZE];
				Array.Copy(cacheChildren, t1, cacheChildren.Length);
				Array.Copy(cacheActions, t2, cacheActions.Length);
				cacheChildren = t1;
				cacheActions = t2;
			}
			// add the node in the cache
			cacheChildren[cacheNext] = node;
			cacheActions[cacheNext] = action;
			// setup the hanle to point to the root
			handle[handleNext++] = cacheNext;
			// copy the root's children
			result.GetAdjacency(node, cacheChildren, cacheNext + 1);
			cacheNext += count + 1;
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
			AddToCache(result.Store(new SymbolRef(SymbolType.Virtual, index)), action);
		}

		/// <summary>
		/// During a reduction, inserts the sub-tree of a nullable variable
		/// </summary>
		/// <param name="nullable">The sub-tree of a nullable variable</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		public void ReductionAddNullable(GSSLabel nullable, TreeAction action)
		{
			if (action == TreeAction.Drop)
				return;
			if (nullable.IsReplaceable)
			{
				AddToCache(new GSSLabel(nullable.ReplaceableTree.Clone()), action);
			}
			else
			{
				AddToCache(result.CopyNode(nullable.NodeIndex), action);
			}
		}

		/// <summary>
		/// Finalizes the reduction operation
		/// </summary>
		/// <param name="generation">The generation to reduce from</param>
		/// <param name="varIndex">The reduced variable index</param>
		/// <param name="replaceable">Whether the sub-tree to build must have a replaceable root or not</param>
		/// <returns>The produced sub-tree</returns>
		public GSSLabel Reduce(int generation, int varIndex, bool replaceable)
		{
			GSSLabel label = Epsilon;
			if (replaceable)
				label = ReduceReplaceable(varIndex);
			else
				label = ReduceNormal(varIndex);
			AddToHistory(generation, label);
			return label;
		}

		/// <summary>
		/// Executes the reduction as a normal reduction
		/// </summary>
		/// <param name="varIndex">The reduced variable index</param>
		/// <returns>The produced sub-tree</returns>
		private GSSLabel ReduceNormal(int varIndex)
		{
			int root = -1;
			int insertion = 0;
			for (int i = 0; i != handleNext; i++)
			{
				switch (cacheActions[handle[i]])
				{
					case TreeAction.Promote:
						if (root != -1)
						{
							// not the first promotion
							// store the adjacency data for the previously promoted node
							int index = result.Store(cacheChildren, insertion);
							result.SetAdjacency(root, index, insertion);
							// put the previously promoted node in the cache
							cacheChildren[0] = root;
							insertion = 1;
						}
						// save the new promoted node
						root = cacheChildren[handle[i]];
                        // repack the children on the left if any
						int nb = result.GetChildrenCount(root);
						Array.Copy(cacheChildren, handle[i] + 1, cacheChildren, insertion, nb);
						Array.Copy(cacheActions, handle[i] + 1, cacheActions, insertion, nb);
						insertion += nb;
						break;
					default:
                        // Repack the sub-root on the left
						if (insertion != handle[i])
							cacheChildren[insertion] = cacheChildren[handle[i]];
						insertion++;
						break;
				}
			}
			if (root == -1)
			{
				// no promotion, create the node for the root
				root = result.Store(new SymbolRef(SymbolType.Variable, varIndex));
			}
			// setup the adjacency for the new root
			result.SetAdjacency(root, result.Store(cacheChildren, insertion), insertion);
			// create the GSS label
			GSSLabel label = new GSSLabel(new SymbolRef(SymbolType.Variable, varIndex), root);
			return label;
		}

		/// <summary>
		/// Executes the reduction as the reduction of a replaceable variable
		/// </summary>
		/// <param name="varIndex">The reduced variable index</param>
		/// <returns>The produced sub-tree</returns>
		private GSSLabel ReduceReplaceable(int varIndex)
		{
			SubTree tree = GetSubTree(handleNext + 1);
			tree.SetupRoot(new SymbolRef(SymbolType.Variable, varIndex), TreeAction.Replace);
			tree.SetChildrenCountAt(0, handleNext);
			for (int i = 0; i != handleNext; i++)
			{
				int node = cacheChildren[handle[i]];
				TreeAction action = cacheActions[handle[i]];
				tree.SetAt(i + 1, new SymbolRef(SymbolType.None, node), action);
			}
			GSSLabel label = new GSSLabel(tree);
			return label;
		}

		/// <summary>
		/// Adds the specified GSS label to the current history
		/// </summary>
		/// <param name="generation">The current generation</param>
		/// <param name="label">The label to register</param>
		private void AddToHistory(int generation, GSSLabel label)
		{
			HistoryPart hp = GetHistoryPart(generation);
			if (hp == null)
			{
				hp = poolHPs.Acquire();
				hp.generation = generation;
				hp.next = 0;
				if (history.Length == nextHP)
					Array.Resize(ref history, history.Length + INIT_HISTORY_SIZE);
				history[nextHP++] = hp;
			}
			if (hp.next == hp.data.Length)
				Array.Resize(ref hp.data, hp.data.Length + INIT_HISTORY_PART_SIZE);
			hp.data[hp.next++] = label;
		}

		/// <summary>
		/// Finalizes the parse tree and returns it
		/// </summary>
		/// <param name="root">The root's sub-tree</param>
		/// <returns>The final parse tree</returns>
		public AST GetTree(GSSLabel root)
		{
			result.SetRoot(root.NodeIndex);
			return result;
		}
	}
}