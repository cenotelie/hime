using System;

namespace Hime.Redist.Parsers
{
    class LRkASTBuilder
    {
        private const int handleSize = 1024;
        private const int estimationBias = 5;

        private class SubTree
        {
            private Pool pool;
            private ParseTree.Cell[] items;
            private LRTreeAction[] actions;

            public Symbols.Symbol Head
            {
                get { return items[0].symbol; }
                set { items[0].symbol = value; }
            }
            public int ChildrenCount
            {
                get { return items[0].count; }
                set { items[0].count = value; }
            }
            public LRTreeAction Action
            {
                get { return actions[0]; }
                set { actions[0] = value; }
            }
            public ParseTree.Cell this[int index]
            {
                get { return items[index]; }
                set { items[index] = value; }
            }

            public LRTreeAction GetAction(int index) { return actions[index]; }

            public SubTree(Pool pool, int capacity)
            {
                this.pool = pool;
                this.items = new ParseTree.Cell[capacity];
                this.actions = new LRTreeAction[capacity];
            }

            public int GetSize()
            {
                int size = 1;
                for (int i = 0; i != items[0].count; i++)
                    size += items[size].count + 1;
                return size;
            }

            public void CopyTo(SubTree destination, int index)
            {
                if (this.items[0].count == 0)
                {
                    destination.items[index] = this.items[0];
                    destination.actions[index] = this.actions[0];
                }
                else
                {
                    Array.Copy(this.items, 0, destination.items, index, this.items[0].count + 1);
                    Array.Copy(this.actions, 0, destination.actions, index, this.items[0].count + 1);
                }
            }

            public void CopyChildrenTo(SubTree destination, int index)
            {
                if (this.items[0].count == 0)
                    return;
                int size = GetSize() - 1;
                Array.Copy(this.items, 1, destination.items, index, size);
                Array.Copy(this.actions, 1, destination.actions, index, size);
            }

            public void CommitTo(int index, ParseTree tree)
            {
                if (this.items[index].count != 0)
                    this.items[index].first = tree.Store(this.items, index + 1, this.items[index].count);
            }

            public void SetAt(int index, Symbols.Symbol symbol, LRTreeAction action)
            {
                this.items[index].symbol = symbol;
                this.items[index].count = 0;
                this.actions[index] = action;
            }

            public void Move(int from, int to)
            {
                this.items[to] = this.items[from];
            }

            public void MoveRange(int from, int to, int length)
            {
                if (length != 0)
                {
                    Array.Copy(items, from, items, to, length);
                    Array.Copy(actions, from, actions, to, length);
                }
            }

            public void Free()
            {
                if (pool != null)
                    pool.Free(this);
            }
        }

        private class Pool
        {
            private int itemsCapacity;
            private SubTree[] free;
            private int nextFree;
            private int allocated;

            public Pool(int itemsCapacity, int size)
            {
                this.itemsCapacity = itemsCapacity;
                this.free = new SubTree[size];
                this.nextFree = -1;
                this.allocated = 0;
            }

            public SubTree Acquire()
            {
                if (nextFree == -1)
                {
                    // Create new one
                    SubTree result = new SubTree(this, itemsCapacity);
                    allocated++;
                    return result;
                }
                else
                {
                    return free[nextFree--];
                }
            }

            public void Free(SubTree subTree)
            {
                nextFree++;
                if (nextFree == free.Length)
                {
                    SubTree[] temp = new SubTree[allocated];
                    Array.Copy(free, temp, free.Length);
                    free = temp;
                }
                free[nextFree] = subTree;
            }
        }

        // Sub-tree pools
        private Pool poolSingle;
        private Pool pool128;
        private Pool pool1024;
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

        internal LRkASTBuilder(int stackSize)
        {
            this.poolSingle = new Pool(1, 512);
            this.pool128 = new Pool(128, 128);
            this.pool1024 = new Pool(1024, 16);
            this.stack = new SubTree[stackSize];
            this.handle = new int[handleSize];
            this.tree = new ParseTree();
        }

        public void StackPush(Symbols.Symbol symbol)
        {
            SubTree single = poolSingle.Acquire();
            single.Head = symbol;
            single.Action = LRTreeAction.None;
            stack[stackNext++] = single;
        }

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

        public void ReductionPop(LRTreeAction action)
        {
            SubTree sub = stack[stackNext + popCount];
            if (sub.Action == LRTreeAction.Replace)
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
            else if (action == LRTreeAction.Drop)
            {
                sub.Free();
            }
            else
            {
                if (action != LRTreeAction.None)
                    sub.Action = action;
                // copy the complete sub-tree to the cache
                sub.CopyTo(cache, cacheNext);
                handle[handleNext++] = cacheNext;
                cacheNext += sub.ChildrenCount + 1;
                sub.Free();
            }
            popCount++;
        }

        public void ReductionVirtual(Symbols.Virtual symbol, LRTreeAction action)
        {
            if (action == LRTreeAction.Drop)
                return; // why would you do this?
            cache.SetAt(cacheNext, symbol, action);
            handle[handleNext++] = cacheNext++;
        }

        public void ReductionSemantic(SemanticAction callback)
        {
            cache.SetAt(cacheNext, new Symbols.Action(callback), LRTreeAction.Semantic);
            handle[handleNext++] = cacheNext++;
        }

        public void Reduce(Symbols.Variable var, LRTreeAction action)
        {
            // Build the subtree
            if (action == LRTreeAction.Replace)
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
            cache.Action = LRTreeAction.Replace;
        }

        private void ReduceNormal(Symbols.Variable var)
        {
            // write the sub-tree root
            cache.Head = var;
            cache.Action = LRTreeAction.None;
            // promotion data
            bool promotion = false;
            int insertion = 1;
            for (int i = 0; i != handleNext; i++)
            {
                switch (cache.GetAction(handle[i]))
                {
                    case LRTreeAction.Promote:
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
                    case LRTreeAction.Semantic:
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
            SubTree sub = stack[stackNext - 2];
            // Commit the children
            sub.CommitTo(0, tree);
            // Commit the root
            tree.StoreRoot(sub[0]);
            return tree;
        }
    }
}
