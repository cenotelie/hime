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
        // Final AST
        private ParseTree tree;

        /// <summary>
        /// Initializes the builder with the given stack size
        /// </summary>
        /// <param name="stackSize">The maximal size of the stack</param>
        public LRkASTBuilder(int stackSize)
        {
            this.poolSingle = new Pool<SubTree>(new SubTreeFactory(1), 512);
            this.pool128 = new Pool<SubTree>(new SubTreeFactory(128), 128);
            this.pool1024 = new Pool<SubTree>(new SubTreeFactory(1024), 16);
            this.stack = new SubTree[stackSize];
            this.handle = new int[handleSize];
            this.tree = new ParseTree();
        }

        /// <summary>
        /// Push a symbol onto the stack
        /// </summary>
        /// <param name="symbol">The symbol to push onto the stack</param>
        public void StackPush(Symbols.Symbol symbol)
        {
            SubTree single = poolSingle.Acquire();
            single.Initialize(symbol, 0, TreeAction.None);
            stack[stackNext++] = single;
        }

        /// <summary>
        /// Prepares for the forthcoming reduction operations
        /// </summary>
        /// <param name="length">The length of the reduction</param>
        public void ReductionPrepare(int length)
        {
            stackNext -= length;
            int estimation = estimationBias;
            for (int i = 0; i != length; i++)
                estimation += stack[stackNext + i].GetSize();
            if (estimation <= 128)
                cache = pool128.Acquire();
            else if (estimation <= 1024)
                cache = pool1024.Acquire();
            else
                cache = new SubTree(null, estimation);
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
            SubTree sub = stack[stackNext + popCount];
            if (sub.Action == TreeAction.Replace)
            {
                // copy the children to the cache
                sub.CopyChildrenTo(cache, cacheNext);
                // setup the handle
                int index = 1;
                for (int i = 0; i != sub.ChildrenCount; i++)
                {
                    int size = sub.GetItem(index).count + 1;
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
                    sub.Action = action;
                // copy the complete sub-tree to the cache
                sub.CopyTo(cache, cacheNext);
                handle[handleNext++] = cacheNext;
                cacheNext += sub.ChildrenCount + 1;
                sub.Free();
            }
            popCount++;
        }

        /// <summary>
        /// During a reduction, inserts a virtual symbol
        /// </summary>
        /// <param name="symbol">The virtual symbol</param>
        /// <param name="action">The tree action applied onto the symbol</param>
        public void ReductionVirtual(Symbols.Virtual symbol, TreeAction action)
        {
            if (action == TreeAction.Drop)
                return; // why would you do this?
            cache.SetAt(cacheNext, symbol, action);
            handle[handleNext++] = cacheNext++;
        }

        /// <summary>
        /// During a reduction, inserts a semantic action
        /// </summary>
        /// <param name="callback">The semantic action</param>
        public void ReductionSemantic(SemanticAction callback)
        {
            cache.SetAt(cacheNext, new Symbols.Action(callback), TreeAction.Semantic);
            handle[handleNext++] = cacheNext++;
        }

        /// <summary>
        /// Finalizes the reduction opration
        /// </summary>
        /// <param name="var">The reduced variable</param>
        /// <param name="action">The tree action applied onto the variable</param>
        public void Reduce(Symbols.Variable var, TreeAction action)
        {
            // Build the subtree
            if (action == TreeAction.Replace)
                ReduceReplaceable(var);
            else
                ReduceNormal(var);
            // Put it on the stack
            stack[stackNext++] = cache;
        }

        private void ReduceReplaceable(Symbols.Variable var)
        {
            cache.Initialize(var, handleNext, TreeAction.Replace);
        }

        private void ReduceNormal(Symbols.Variable var)
        {
            // write the sub-tree root
            cache.Initialize(var, 0, TreeAction.None);

            // promotion data
            bool promotion = false;
            int insertion = 1;
            for (int i = 0; i != handleNext; i++)
            {
                switch (cache.GetAction(handle[i]))
                {
                    case TreeAction.Promote:
                        if (promotion)
                        {
                            // This is not the first promotion
                            // Commit the previously promoted node's children
                            cache.ChildrenCount = insertion - 1;
                            cache.CommitTo(0, tree);
                            // Reput the previously promoted node in the cache
                            cache.Move(0, 1);
                            insertion = 2;
                        }
                        promotion = true;    
                        // Save the new promoted node
                        cache.Move(handle[i], 0);
                        // Repack the children on the left if any
                        cache.MoveRange(handle[i] + 1, insertion, cache.ChildrenCount);
                        insertion += cache.ChildrenCount;
                        break;
                    case TreeAction.Semantic:
                        // TODO: something !
                        break;
                    default:
                        // Commit the children if any
                        cache.CommitTo(handle[i], tree);
                        // Repack the sub-root on the left
                        if (insertion != handle[i])
                            cache.Move(handle[i], insertion);
                        insertion++;
                        break;
                }
            }
            // finalize the sub-tree data
            cache.ChildrenCount = insertion - 1;
        }

        /// <summary>
        /// Finalizes the parse tree and returns it
        /// </summary>
        /// <returns>The final parse tree</returns>
        public ParseTree GetTree()
        {
            // Get the axiom's sub tree
            SubTree sub = stack[stackNext - 2];
            // Commit the children
            sub.CommitTo(0, tree);
            // Commit the root
            tree.StoreRoot(sub.GetItem(0));
            return tree;
        }
    }
}
