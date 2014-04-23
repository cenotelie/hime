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
	/// Represents the builder of Parse Trees for LR(k) parsers
	/// </summary>
    class LRkASTBuilder
    {
		private class SubTreeFactory : Factory<SubTree>
		{
			private int capacity;
			public SubTreeFactory(int capacity) { this.capacity = capacity; }
			public SubTree CreateNew(Pool<SubTree> pool) { return new SubTree(pool, capacity); }
		}
    	
        private const int handleSize = 1024;
        private const int estimationBias = 5;

		// Sub-tree pools
        private Pool<SubTree> poolSingle;
        private Pool<SubTree> pool128;
        private Pool<SubTree> pool1024;
        // Stack of semantic objects
        private SubTree[] stack;
        private int stackNext;
        // Cache for the reductions
        private SubTree cache;
        private int cacheNext;
        private int popCount;
        // Reduction data
        private int[] handle;
        private int handleNext;
        // The AST being built
        private SimpleAST result;

        /// <summary>
        /// Initializes the builder with the given stack size
        /// </summary>
        /// <param name="stackSize">The maximal size of the stack</param>
        public LRkASTBuilder(int stackSize, TokenizedText text, SymbolDictionary variables, SymbolDictionary virtuals)
        {
			this.poolSingle = new Pool<SubTree>(new SubTreeFactory(1), 512);
            this.pool128 = new Pool<SubTree>(new SubTreeFactory(128), 128);
            this.pool1024 = new Pool<SubTree>(new SubTreeFactory(1024), 16);
            this.stack = new SubTree[stackSize];
            this.stackNext = 0;


			this.handle = new int[handleSize];
			this.result = new SimpleAST(text, variables, virtuals);
        }

        /// <summary>
        /// Push a token onto the stack
        /// </summary>
        /// <param name="index">The token's index in the parsed text</param>
        public void StackPushToken(int index)
        {
			SubTree single = poolSingle.Acquire();
			single.SetupRoot(new SymbolRef(SymbolType.Token, index), TreeAction.None);
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
            int estimation = estimationBias;
            for (int i = 0; i != length; i++)
                estimation += stack[stackNext + i].GetSize();
			cache = GetSubTree(estimation);
			cache.SetupRoot(new SymbolRef(SymbolType.Variable, varIndex), action);
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
        /// During a redution, pops the top symbol from the stack and gives it a tree action
        /// </summary>
        /// <param name="action">The tree action to apply to the symbol</param>
        public void ReductionPop(TreeAction action)
        {
			SubTree sub = stack[stackNext + popCount];
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
                sub.Free();
            }
            else if (action == TreeAction.Drop)
            {
                sub.Free();
            }
            else
            {
                if (action != TreeAction.None)
					sub.SetActionAt(0, action);
                // copy the complete sub-tree to the cache
                sub.CopyTo(cache, cacheNext);
                handle[handleNext++] = cacheNext;
                cacheNext += sub.GetChildrenCountAt(0) + 1;
                sub.Free();
            }
            popCount++;
        }

        /// <summary>
        /// During a reduction, inserts a virtual symbol
        /// </summary>
        /// <param name="index">The virtual symbol's index</param>
        /// <param name="action">The tree action applied onto the symbol</param>
        public void ReductionVirtual(int index, TreeAction action)
        {
            if (action == TreeAction.Drop)
                return; // why would you do this?
			cache.SetAt(cacheNext, new SymbolRef(SymbolType.Virtual, index), action);
            handle[handleNext++] = cacheNext++;
        }

        /// <summary>
        /// During a reduction, inserts a semantic action
        /// </summary>
        /// <param name="action">The semantic action</param>
        public void ReductionSemantic(UserAction action)
        {

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
