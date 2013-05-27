using System;

namespace Hime.Redist.Parsers
{
    class LRkASTBuilder
    {
        private const int bufferSize = 2048;

        private struct StackElem
        {
            public Symbols.Symbol symbol;
            public int index;
        }

        // stack of semantic objects
        private StackElem[] stack;
        private int stackNext;
        // Buffer of sub-trees not yet commited to the final AST
        private ParseTree.Cell[] subs;
        private LRTreeAction[] subsActions;
        private int subsNext;
        // Cache of sub-trees that are currently in use in a reduction
        private ParseTree.Cell[] cache;
        private LRTreeAction[] cacheActions;
        private int cacheNext;
        // Reduction info
        private int[] handle;
        private int handleNext;
        private int popCount;
        // Final AST
        private ParseTree tree;

        internal LRkASTBuilder(int stackSize)
        {
            this.stack = new StackElem[stackSize];
            this.subs = new ParseTree.Cell[bufferSize];
            this.subsActions = new LRTreeAction[bufferSize];
            this.cache = new ParseTree.Cell[bufferSize];
            this.cacheActions = new LRTreeAction[bufferSize];
            this.handle = new int[bufferSize];
            this.tree = new ParseTree();
        }

        public void StackPush(Symbols.Symbol symbol)
        {
            stack[stackNext].symbol = symbol;
            stack[stackNext].index = -1;
            stackNext++;
        }

        public void ReductionPrepare(int length)
        {
            stackNext -= length;
            cacheNext = 0;
            handleNext = 0;
            popCount = 0;
        }

        public void ReductionPop(LRTreeAction action)
        {
            if (stack[stackNext + popCount].index == -1)
            {
                // This is an individual symbol (token)
                if (action != LRTreeAction.Drop)
                {
                    // If it should be kept
                    EnsureCache(cacheNext + 1);
                    cache[cacheNext].symbol = stack[stackNext + popCount].symbol;
                    cache[cacheNext].count = 0;
                    cacheActions[cacheNext] = action;
                    handle[handleNext] = cacheNext;
                    cacheNext++;
                    handleNext++;
                }
            }
            else
            {
                // This is a sub-tree
                int current = stack[stackNext + popCount].index;
                ParseTree.Cell sub = subs[current];
                if (subsActions[current] == LRTreeAction.Replace)
                {
                    // Replace this node by its children
                    current++;
                    for (int i = 0; i != sub.count; i++)
                    {
                        handle[handleNext++] = cacheNext;
                        current += MoveToCache(current);
                    }
                    subsNext--; // This is to account for the replaced node
                }
                else if (action == LRTreeAction.Drop)
                {
                    // Drop now
                    subsNext -= sub.count + 1;
                }
                else
                {
                    EnsureCache(cacheNext + 1);
                    handle[handleNext] = cacheNext;
                    MoveToCache(current);
                    cacheActions[handle[handleNext]] = (action != LRTreeAction.None ? action : subsActions[current]);
                    handleNext++;
                }
            }
            popCount++;
        }

        public void ReductionVirtual(Symbols.Virtual symbol, LRTreeAction action)
        {
            if (action == LRTreeAction.Drop)
                return; // why would you do this?
            EnsureCache(cacheNext + 1);
            cache[cacheNext].symbol = symbol;
            cache[cacheNext].count = 0;
            cacheActions[cacheNext] = action;
            handle[handleNext] = cacheNext;
            cacheNext++;
            handleNext++;
        }

        public void ReductionSemantic(SemanticAction callback)
        {
            EnsureCache(cacheNext + 1);
            cache[cacheNext].symbol = new Symbols.Action(callback);
            cache[cacheNext].count = 0;
            cacheActions[cacheNext] = LRTreeAction.Semantic;
            handle[handleNext] = cacheNext;
            cacheNext++;
            handleNext++;
        }

        public void Reduce(Symbols.Variable var, LRTreeAction action)
        {
            // Put the variable on the stack
            stack[stackNext].symbol = var;
            stack[stackNext].index = subsNext;
            stackNext++;
            // Build the subtree
            if (action == LRTreeAction.Replace)
                ReduceReplaceable(var);
            else
                ReduceNormal(var);
        }

        private void ReduceReplaceable(Symbols.Variable var)
        {
            EnsureSubs(subsNext + 1);
            subs[subsNext].symbol = var;
            subs[subsNext].count = handleNext;
            subsActions[subsNext] = LRTreeAction.Replace;
            subsNext++;
            if (handleNext != 0)
            {
                EnsureSubs(subsNext + cacheNext);
                Array.Copy(cache, 0, subs, subsNext, cacheNext);
                Array.Copy(cacheActions, 0, subsActions, subsNext, cacheNext);
                subsNext += cacheNext;
            }
        }

        private void ReduceNormal(Symbols.Variable var)
        {
            // write the sub-tree root
            int top = subsNext;
            EnsureSubs(subsNext + 1);
            subs[subsNext].symbol = var;
            subsActions[subsNext] = LRTreeAction.None;
            subsNext++;
            // promotion data
            ParseTree.Cell promoted = new ParseTree.Cell();
            bool promotion = false;
            int insertion = 0;
            for (int i = 0; i != handleNext; i++)
            {
                switch (cacheActions[handle[i]])
                {
                    case LRTreeAction.Promote:
                        if (promotion)
                        {
                            // This is not the first promotion
                            promoted.count = insertion;
                            // Commit the previously promoted node's children
                            if (insertion != 0)
                                promoted.first = tree.Store(cache, 0, insertion);
                            // Reput the previously promoted node in the cache
                            cache[0] = promoted;
                            cacheActions[0] = LRTreeAction.None;
                            insertion = 1;
                        }
                        promotion = true;    
                        // Save the new promoted node
                        promoted = cache[handle[i]];
                        // Repack the children on the left if any
                        if (promoted.count != 0)
                        {
                            Array.Copy(cache, handle[i] + 1, cache, insertion, promoted.count);
                            Array.Copy(cacheActions, handle[i] + 1, cacheActions, insertion, promoted.count);
                            insertion += promoted.count;
                        }
                        break;
                    case LRTreeAction.Semantic:
                        // TODO: something !
                        break;
                    default:
                        // Commit the children if any
                        if (cache[handle[i]].count != 0)
                            cache[handle[i]].first = tree.Store(cache, handle[i] + 1, cache[handle[i]].count);
                        // Repack the sub-root on the left
                        if (insertion != handle[i])
                        {
                            cache[insertion] = cache[handle[i]];
                            cacheActions[insertion] = cacheActions[handle[i]];
                        }
                        insertion++;
                        break;
                }
            }
            // finalize the sub-tree data
            subs[top].count = insertion;
            if (promotion)
                subs[top].symbol = promoted.symbol;
            // write the children back in the sub-trees, if any
            if (insertion != 0)
            {
                EnsureSubs(subsNext + insertion);
                Array.Copy(cache, 0, subs, subsNext, insertion);
                Array.Copy(cacheActions, 0, subsActions, subsNext, insertion);
                subsNext += insertion;
            }
        }

        private int MoveToCache(int index)
        {
            if (subs[index].count == 0)
            {
                EnsureCache(cacheNext + 1);
                cache[cacheNext] = subs[index];
                cacheActions[cacheNext] = subsActions[index];
                cacheNext++;
                subsNext--;
                return 1;
            }
            else
            {
                int length = subs[index].count + 1;
                EnsureCache(cacheNext + length);
                Array.Copy(subs, index, cache, cacheNext, length);
                Array.Copy(subsActions, index, cacheActions, cacheNext, length);
                cacheNext += length;
                subsNext -= length;
                return length;
            }
        }

        private void EnsureSubs(int size)
        {
            if (subs.Length >= size)
                return;
            int s = subs.Length + bufferSize;
            while (s < size)
                s += bufferSize;
            ParseTree.Cell[] t1 = new ParseTree.Cell[s];
            LRTreeAction[] t2 = new LRTreeAction[s];
            Array.Copy(subs, t1, subs.Length);
            Array.Copy(subsActions, t2, subs.Length);
            subs = t1;
            subsActions = t2;
        }

        private void EnsureCache(int size)
        {
            if (cache.Length >= size)
                return;
            int s = cache.Length + bufferSize;
            while (s < size)
                s += bufferSize;
            ParseTree.Cell[] t1 = new ParseTree.Cell[s];
            LRTreeAction[] t2 = new LRTreeAction[s];
            Array.Copy(cache, t1, cache.Length);
            Array.Copy(cacheActions, t2, cache.Length);
            cache = t1;
            cacheActions = t2;
        }

        public ParseTree GetTree()
        {
            int index = stack[stackNext - 2].index;
            ParseTree.Cell rootCell = subs[index];
            // commit the children if any
            if (rootCell.count != 0)
                rootCell.first = tree.Store(subs, index + 1, rootCell.count);
            // commit the root
            tree.StoreRoot(rootCell);
            return tree;
        }
    }
}
