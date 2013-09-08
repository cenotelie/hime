using System;
using System.Collections.Generic;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	class SPPFBuilder
	{
        private const int initHistorySize = 8;
        private const int initHistoryPartSize = 64;
        private const int handleSize = 1024;
        private const int estimationBias = 5;

        private class HistoryPart
        {

            private SPPFBuilder builder;
            private int generation;
            private SubSPPF[] data;
            private int index;

            public int Generation
            {
                get { return generation; }
                set { generation = value; }
            }

            public HistoryPart(SPPFBuilder builder)
            {
                this.builder = builder;
                this.data = new SubSPPF[initHistoryPartSize];
                this.index = 0;
            }

            public void Free()
            {
                index = 0;
                builder.poolHP.Return(this);
            }

            public SubSPPF Resolve(Symbols.Variable variable, int childrenCount)
            {
                for (int i = 0; i != index; i++)
                {
                    if (data[i].OriginalSID == variable.SymbolID)
                    {
                        builder.hitHistory = true;
                        return data[i];
                    }
                }
                if (index == data.Length)
                {
                    SubSPPF[] temp = new SubSPPF[data.Length + initHistoryPartSize];
                    Array.Copy(data, temp, data.Length);
                    data = temp;
                }
                SubSPPF result = builder.AcquireNode(variable, childrenCount);
                data[index++] = result;
                return result;
            }
        }

        private class HistoryPartFactory : Factory<HistoryPart>
        {
            private SPPFBuilder builder;
            public HistoryPartFactory(SPPFBuilder builder) { this.builder = builder; }
            public HistoryPart CreateNew(Pool<HistoryPart> pool) { return new HistoryPart(builder); }
        }

        private class SubTreeFactory : Factory<SubSPPF>
        {
            private int capacity;
            public SubTreeFactory(int capacity) { this.capacity = capacity; }
            public SubSPPF CreateNew(Pool<SubSPPF> pool) { return new SubSPPF(pool, capacity); }
        }

        // SPPF pools
        private Pool<SubSPPF> poolSingle;
        private Pool<SubSPPF> poolSPPF128;
        private Pool<SubSPPF> poolSPPF1024;
        // Constant SPPFs for nullable variables
        private SubSPPF epsilon;
        private Dictionary<int, SubSPPF> nullVars;
        // History data
        private Pool<HistoryPart> poolHP;
        private HistoryPart[] history;
        private int nextHP;
        private bool hitHistory;
        // Stack of semantic objects
        private GSSPath stack;
        private SubSPPF stackFirst;
        private int stackNext;
        // Cache for the reductions
        private SubSPPF cache;
        private int cacheNext;
        // Reduction data
        private int[] handle;
        private int handleNext;
        // Final AST
        private ParseTree tree;

        public SubSPPF Epsilon { get { return epsilon; } }
        public SubSPPF GetNullSPPF(int sid)
        {
            hitHistory = true;
            return nullVars[sid];
        }

        public SPPFBuilder()
        {
            this.poolSingle = new Pool<SubSPPF>(new SubTreeFactory(1), 512);
            this.poolSPPF128 = new Pool<SubSPPF>(new SubTreeFactory(128), 128);
            this.poolSPPF1024 = new Pool<SubSPPF>(new SubTreeFactory(1024), 16);
            this.epsilon = AcquireSingleNode(Symbols.Epsilon.Instance);
            this.nullVars = new Dictionary<int, SubSPPF>();
            this.poolHP = new Pool<HistoryPart>(new HistoryPartFactory(this), initHistorySize);
            this.history = new HistoryPart[initHistorySize];
            this.hitHistory = false;
            this.handle = new int[handleSize];
            this.tree = new ParseTree();
        }

        public void ClearHistory()
        {
            for (int i = 0; i != nextHP; i++)
                history[i].Free();
            nextHP = 0;
        }

        public SubSPPF ResolveSPPF(int generation, Symbols.Variable variable, int childrenCount)
        {
            hitHistory = false;
            for (int i = 0; i != nextHP; i++)
                if (history[i].Generation == generation)
                    return history[i].Resolve(variable, childrenCount);
            HistoryPart part = poolHP.Acquire();
            part.Generation = generation;
            history[nextHP++] = part;
            return part.Resolve(variable, childrenCount);
        }


        public SubSPPF AcquireSingleNode(Symbols.Symbol symbol)
        {
            SubSPPF sppf = poolSingle.Acquire();
            sppf.Initialize(symbol, 0, TreeAction.None);
            return sppf;
        }

        public SubSPPF AcquireNode(Symbols.Symbol symbol, int childrenCount)
        {
            return null;
        }


        /// <summary>
        /// Prepare the forthcoming reduction operations for nullable variables
        /// </summary>
        public void ReductionPrepare()
        {

        }

        /// <summary>
        /// Prepares for the forthcoming reduction operations
        /// </summary>
        /// <param name="path">The path in the GSS</param>
        /// <param name="first">The first SPPF node, or epsilon</param>
        /// <param name="length">The length of the reduction</param>
        public void ReductionPrepare(GSSPath path, SubSPPF first, int length)
        {
            int estimation = estimationBias;
            stack = path;
            stackFirst = first;
            if (length == 0)
            {
                stackNext = -1;
                cache = null;
            }
            else
            {
                stackNext = length - 2;
                estimation += first.GetSize();
                for (int i = 0; i != length - 2; i++)
                    estimation += stack[i].GetSize();
                if (estimation <= 128)
                    cache = poolSPPF128.Acquire();
                else if (estimation <= 1024)
                    cache = poolSPPF1024.Acquire();
                else
                    cache = new SubSPPF(null, estimation);
            }
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
            SubSPPF sub = null;
            if (stackNext < 0)
                sub = stackFirst;
            else
                sub = stack[stackNext--];
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
            { }
            else
            {
                if (action != TreeAction.None)
                    sub.Action = action;
                // copy the complete sub-tree to the cache
                sub.CopyTo(cache, cacheNext);
                handle[handleNext++] = cacheNext;
                cacheNext += sub.ChildrenCount + 1;
            }
        }

        /// <summary>
        /// During a reduction, inserts a virtual symbol
        /// </summary>
        /// <param name="symbol">The virtual symbol</param>
        /// <param name="action">The tree action applied onto the symbol</param>
        public void ReductionVirtual(Symbols.Virtual symbol, TreeAction action)
        {
            if (cache == null)
                return; // do not modify an existing nullable SPPF
            if (action == TreeAction.Drop)
                return; // why would you do this?
            cache.SetAt(cacheNext, symbol, action);
            handle[handleNext++] = cacheNext++;
        }

        /// <summary>
        /// During a reduction, inserts a nullable variable
        /// </summary>
        /// <param name="var">The nullable variable</param>
        /// <param name="action">The tree action applied onto the symbol</param>
        public void ReductionNullVariable(Symbols.Variable var, TreeAction action)
        {
            if (action == TreeAction.Drop)
                return;
            SubSPPF sub = nullVars[var.SymbolID];
            // copy the complete sub-tree to the cache
            sub.CopyTo(cache, cacheNext);
            if (action != TreeAction.None)
                sub.Action = action; // FIXME do NOT modify sub
            handle[handleNext++] = cacheNext;
            cacheNext += sub.ChildrenCount + 1;
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
        /// Finalizes the reduction operation on a nullable variable
        /// </summary>
        /// <param name="index">The index of the nullable variable</param>
        /// <param name="action">The tree action applied onto the variable</param>
        public void Reduce(int index, TreeAction action)
        {

        }

        /// <summary>
        /// Finalizes the reduction opration
        /// </summary>
        /// <param name="var">The reduced variable</param>
        /// <param name="action">The tree action applied onto the variable</param>
        public SubSPPF Reduce(Symbols.Variable var, TreeAction action)
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


        public ParseTree GetTree(SubSPPF root)
        {
            return tree;
        }
	}
}
