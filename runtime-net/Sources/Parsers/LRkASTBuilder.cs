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
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents the builder of Parse Trees for LR(k) parsers
	/// </summary>
	class LRkASTBuilder : SemanticBody
	{
		/// <summary>
		/// The initial size of the reduction handle
		/// </summary>
		private const int INIT_HANDLE_SIZE = 1024;
		/// <summary>
		/// The bias for estimating the size of the reduced sub-tree
		/// </summary>
		private const int ESTIMATION_BIAS = 5;

		/// <summary>
		/// The pool of single node sub-trees
		/// </summary>
		private readonly Pool<SubTree> poolSingle;
		/// <summary>
		/// The pool of sub-tree with a capacity of 128 nodes
		/// </summary>
		private readonly Pool<SubTree> pool128;
		/// <summary>
		/// The pool of sub-tree with a capacity of 1024 nodes
		/// </summary>
		private readonly Pool<SubTree> pool1024;
		/// <summary>
		/// The stack of semantic objects
		/// </summary>
		private SubTree[] stack;
		/// <summary>
		/// Index of the available cell on top of the stack's head
		/// </summary>
		private int stackNext;
		/// <summary>
		/// The sub-tree build-up cache
		/// </summary>
		private SubTree cache;
		/// <summary>
		/// The new available node in the current cache
		/// </summary>
		private int cacheNext;
		/// <summary>
		/// The number of items popped from the stack
		/// </summary>
		private int popCount;
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
		private readonly ASTSimpleTree result;

		#region Implementation of SemanticBody

		/// <summary>
		/// Gets the symbol at the i-th index
		/// </summary>
		/// <param name="index">Index of the symbol</param>
		/// <returns>The symbol at the given index</returns>
		public SemanticElement this[int index] { get { return result.GetSemanticElementForLabel(cache.GetLabelAt(handle[index])); } }

		/// <summary>
		/// Gets the length of this body
		/// </summary>
		public int Length { get { return handleNext; } }

		#endregion

		/// <summary>
		/// Initializes the builder with the given stack size
		/// </summary>
		/// <param name="tokens">The table of tokens</param>
		/// <param name="variables">The table of parser variables</param>
		/// <param name="virtuals">The table of parser virtuals</param>
		public LRkASTBuilder(TokenRepository tokens, ROList<Symbol> variables, ROList<Symbol> virtuals)
		{
			poolSingle = new Pool<SubTree>(new SubTreeFactory(1), 512);
			pool128 = new Pool<SubTree>(new SubTreeFactory(128), 128);
			pool1024 = new Pool<SubTree>(new SubTreeFactory(1024), 16);
			stack = new SubTree[LRkParser.INIT_STACK_SIZE];
			stackNext = 0;
			handle = new int[INIT_HANDLE_SIZE];
			result = new ASTSimpleTree(tokens, variables, virtuals);
		}

		/// <summary>
		/// Push a token onto the stack
		/// </summary>
		/// <param name="index">The token's index in the parsed text</param>
		public void StackPushToken(int index)
		{
			SubTree single = poolSingle.Acquire();
			single.SetupRoot(new TableElemRef(TableType.Token, index), TreeAction.None);
			if (stackNext == stack.Length)
				Array.Resize(ref stack, stack.Length + LRkParser.INIT_STACK_SIZE);
			stack[stackNext++] = single;
		}

		/// <summary>
		/// Prepares for the forthcoming reduction operations
		/// </summary>
		/// <param name="varIndex">The reduced variable index</param>
		/// <param name="length">The length of the reduction</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		public void ReductionPrepare(int varIndex, int length, TreeAction action)
		{
			stackNext -= length;
			int estimation = ESTIMATION_BIAS;
			for (int i = 0; i != length; i++)
				estimation += stack[stackNext + i].GetSize();
			cache = GetSubTree(estimation);
			cache.SetupRoot(new TableElemRef(TableType.Variable, varIndex), action);
			cacheNext = 1;
			handleNext = 0;
			popCount = 0;
		}

		/// <summary>
		/// Gets a pooled sub-tree with the given maximal size
		/// </summary>
		/// <param name="size">The size of the sub-tree</param>
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
		/// During a reduction, insert the given sub-tree
		/// </summary>
		/// <param name="sub">The sub-tree</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		private void ReductionAddSub(SubTree sub, TreeAction action)
		{
			if (sub.GetActionAt(0) == TreeAction.Replace)
			{
				int directChildrenCount = sub.GetChildrenCountAt(0);
				while (handleNext + directChildrenCount >= handle.Length)
					Array.Resize(ref handle, handle.Length + INIT_HANDLE_SIZE);
				// copy the children to the cache
				sub.CopyChildrenTo(cache, cacheNext);
				// setup the handle
				int index = 1;
				for (int i = 0; i != directChildrenCount; i++)
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
				if (handleNext == handle.Length)
					Array.Resize(ref handle, handle.Length + INIT_HANDLE_SIZE);
				sub.CopyTo(cache, cacheNext);
				handle[handleNext++] = cacheNext;
				cacheNext += sub.GetChildrenCountAt(0) + 1;
			}
		}

		/// <summary>
		/// During a redution, pops the top symbol from the stack and gives it a tree action
		/// </summary>
		/// <param name="action">The tree action to apply to the symbol</param>
		public void ReductionPop(TreeAction action)
		{
			SubTree sub = stack[stackNext + popCount];
			ReductionAddSub(sub, action);
			sub.Free();
			popCount++;
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
			cache.SetAt(cacheNext, new TableElemRef(TableType.Virtual, index), action);
			handle[handleNext++] = cacheNext++;
		}

		/// <summary>
		/// Finalizes the reduction operation
		/// </summary>
		public void Reduce()
		{
			if (cache.GetActionAt(0) == TreeAction.Replace)
			{
				cache.SetChildrenCountAt(0, handleNext);
			}
			else
			{
				ReduceTree();
			}
			// Put it on the stack
			if (stackNext == stack.Length)
				Array.Resize(ref stack, stack.Length + LRkParser.INIT_STACK_SIZE);
			stack[stackNext++] = cache;
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

		/// <summary>
		/// Finalizes the parse tree and returns it
		/// </summary>
		/// <returns>The final parse tree</returns>
		public AST GetTree()
		{
			// Get the axiom's sub tree
			SubTree sub = stack[stackNext - 2];
			// Commit the remaining sub-tree
			sub.Commit(result);
			return result;
		}
	}
}
