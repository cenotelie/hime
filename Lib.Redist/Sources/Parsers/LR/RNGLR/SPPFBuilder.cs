using System;

namespace Hime.Redist.Parsers
{
    class SPPFBuilder
    {
        private const int handleSize = 1024;
        private const int estimationBias = 5;

        // Sub-tree SPPFPools
        private SPPFPool SPPFPoolSingle;
        private SPPFPool SPPFPool128;
        private SPPFPool SPPFPool1024;
        // Stack of semantic objects
        private SPPFSubTree[] stack;
        private int stackNext;
        // Cache for the reductions
        private SPPFSubTree cache;
        private int cacheNext;
        private int popCount;
        // Reduction data
        private int[] handle;
        private int handleNext;
        // Final AST
        private ParseTree tree;

        internal SPPFBuilder(int stackSize)
        {
            this.SPPFPoolSingle = new SPPFPool(1, 512);
            this.SPPFPool128 = new SPPFPool(128, 128);
            this.SPPFPool1024 = new SPPFPool(1024, 16);
            this.stack = new SPPFSubTree[stackSize];
            this.handle = new int[handleSize];
            this.tree = new ParseTree();
        }

        public void StackPush(Symbols.Symbol symbol)
        {
            SPPFSubTree single = SPPFPoolSingle.Acquire();
            single.Head = symbol;
            single.Action = TreeAction.None;
            stack[stackNext++] = single;
        }

        public void ReductionPrepare(int length)
        {
            stackNext -= length;
            int estimation = estimationBias;
            for (int i = 0; i != length; i++)
                estimation += stack[stackNext + i].GetSize();
            if (estimation <= 128)
                cache = SPPFPool128.Acquire();
            else if (estimation <= 1024)
                cache = SPPFPool1024.Acquire();
            else
                cache = new SPPFSubTree(null, estimation);
            cacheNext = 1;
            handleNext = 0;
            popCount = 0;
        }

        public void ReductionPop(TreeAction action)
        {
            SPPFSubTree sub = stack[stackNext + popCount];
            if (sub.Action == TreeAction.Replace)
            {
                // copy the children to the cache
                sub.CopyChildrenTo(cache, cacheNext);
                // setup the handle
                int index = 1;
                for (int i = 0; i != sub.ChildrenCount; i++)
                {
                    int size = sub[index].count + 1;
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

        public void ReductionVirtual(Symbols.Virtual symbol, TreeAction action)
        {
            if (action == TreeAction.Drop)
                return; // why would you do this?
            cache.SetAt(cacheNext, symbol, action);
            handle[handleNext++] = cacheNext++;
        }

        public void ReductionSemantic(SemanticAction callback)
        {
            cache.SetAt(cacheNext, new Symbols.Action(callback), TreeAction.Semantic);
            handle[handleNext++] = cacheNext++;
        }

        public void Reduce(Symbols.Variable var, TreeAction action)
        {
            // Build the SPPFSubTree
            if (action == TreeAction.Replace)
                ReduceReplaceable(var);
            else
                ReduceNormal(var);
            // Put it on the stack
            stack[stackNext++] = cache;
        }

        private void ReduceReplaceable(Symbols.Variable var)
        {
            cache.Head = var;
            cache.ChildrenCount = handleNext;
            cache.Action = TreeAction.Replace;
        }

        private void ReduceNormal(Symbols.Variable var)
        {
            // write the sub-tree root
            cache.Head = var;
            cache.Action = TreeAction.None;
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

        public ParseTree GetTree()
        {
            // Get the axiom's sub tree
            SPPFSubTree sub = stack[stackNext - 2];
            // Commit the children
            sub.CommitTo(0, tree);
            // Commit the root
            tree.StoreRoot(sub[0]);
            return tree;
        }
    }
}
