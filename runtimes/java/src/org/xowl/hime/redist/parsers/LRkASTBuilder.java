/**********************************************************************
 * Copyright (c) 2014 Laurent Wouters and others
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
package org.xowl.hime.redist.parsers;

import org.xowl.hime.redist.AST;
import org.xowl.hime.redist.SemanticBody;
import org.xowl.hime.redist.Symbol;
import org.xowl.hime.redist.TokenizedText;

import java.util.List;

/**
 * Represents the builder of Parse Trees for LR(k) parsers
 */
class LRkASTBuilder implements SemanticBody {
    /**
     * The maximum size of the reduction handle
     */
    protected static final int handleSize = 1024;
    /**
     * The bias for estimating the size of the reduced sub-tree
     */
    protected static final int estimationBias = 5;

    /**
     * The pool of single node sub-trees
     */
    private Pool<SubTree> poolSingle;
    /**
     * The pool of sub-tree with a capacity of 128 nodes
     */
    private Pool<SubTree> pool128;
    /**
     * The pool of sub-tree with a capacity of 1024 nodes
     */
    private Pool<SubTree> pool1024;
    /**
     * The stack of semantic objects
     */
    private SubTree[] stack;
    /**
     * Index of the available cell on top of the stack's head
     */
    private int stackNext;
    /**
     * The sub-tree build-up cache
     */
    private SubTree cache;
    /**
     * The new available node in the current cache
     */
    private int cacheNext;
    /**
     * The number of items popped from the stack
     */
    private int popCount;
    /**
     * The reduction handle represented as the indices of the sub-trees in the cache
     */
    private int[] handle;
    /**
     * The index of the next available slot in the handle
     */
    private int handleNext;
    /**
     * The AST being built
     */
    private SimpleAST result;

    /**
     * Gets the symbol at the i-th index
     *
     * @param index Index of the symbol
     * @return The symbol at the given index
     */
    public Symbol at(int index) {
        return result.getSymbolFor(cache.getLabelAt(handle[index]));
    }

    /**
     * Gets the length of this body
     *
     * @return The length of this body
     */
    public int length() {
        return handleNext;
    }

    /**
     * Initializes the builder with the given stack size
     *
     * @param stackSize The maximal size of the stack
     * @param text      The tokenined text
     * @param variables The table of parser variables
     * @param virtuals  The table of parser virtuals
     */
    public LRkASTBuilder(int stackSize, TokenizedText text, List<Symbol> variables, List<Symbol> virtuals) {
        this.poolSingle = new Pool<SubTree>(new Factory<SubTree>() {
            @Override
            public SubTree createNew(Pool<SubTree> pool) {
                return new SubTree(poolSingle, 1);
            }
        }, 512, SubTree.class);
        this.pool128 = new Pool<SubTree>(new Factory<SubTree>() {
            @Override
            public SubTree createNew(Pool<SubTree> pool) {
                return new SubTree(pool128, 128);
            }
        }, 128, SubTree.class);
        this.pool1024 = new Pool<SubTree>(new Factory<SubTree>() {
            @Override
            public SubTree createNew(Pool<SubTree> pool) {
                return new SubTree(pool1024, 1024);
            }
        }, 16, SubTree.class);
        this.stack = new SubTree[stackSize];
        this.stackNext = 0;
        this.handle = new int[handleSize];
        this.result = new SimpleAST(text, variables, virtuals);
    }

    /**
     * Push a token onto the stack
     *
     * @param index The token's index in the parsed text
     */
    public void stackPushToken(int index) {
        SubTree single = poolSingle.acquire();
        single.setupRoot(SymbolRef.encode(SymbolType.TOKEN, index), LROpCode.TREE_ACTION_NONE);
        stack[stackNext++] = single;
    }

    /**
     * Prepares for the forthcoming reduction operations
     *
     * @param varIndex The reduced variable index
     * @param length   The length of the reduction
     * @param action   The tree action applied onto the symbol
     */
    public void reductionPrepare(int varIndex, int length, byte action) {
        stackNext -= length;
        int estimation = estimationBias;
        for (int i = 0; i != length; i++)
            estimation += stack[stackNext + i].size();
        cache = getSubTree(estimation);
        cache.setupRoot(SymbolRef.encode(SymbolType.VARIABLE, varIndex), action);
        cacheNext = 1;
        handleNext = 0;
        popCount = 0;
    }

    /**
     * Gets a pooled sub-tree with the given maximal size
     *
     * @param size The size of the sub-tree
     * @return A pooled sub-tree with the given maximal size
     */
    protected SubTree getSubTree(int size) {
        if (size <= 128)
            return pool128.acquire();
        else if (size <= 1024)
            return pool1024.acquire();
        else
            return new SubTree(null, size);
    }

    /**
     * During a reduction, insert the given sub-tree
     *
     * @param sub    The sub-tree
     * @param action The tree action applied onto the symbol
     */
    protected void reductionAddSub(SubTree sub, byte action) {
        if (sub.getActionAt(0) == LROpCode.TREE_ACTION_REPLACE) {
            // copy the children to the cache
            sub.copyChildrenTo(cache, cacheNext);
            // setup the handle
            int index = 1;
            for (int i = 0; i != sub.getChildrenCountAt(0); i++) {
                int size = sub.getChildrenCountAt(index) + 1;
                handle[handleNext++] = cacheNext;
                cacheNext += size;
                index += size;
            }
        } else if (action == LROpCode.TREE_ACTION_DROP) {
        } else {
            if (action != LROpCode.TREE_ACTION_NONE)
                sub.setActionAt(0, action);
            // copy the complete sub-tree to the cache
            sub.copyTo(cache, cacheNext);
            handle[handleNext++] = cacheNext;
            cacheNext += sub.getChildrenCountAt(0) + 1;
        }
    }

    /**
     * During a redution, pops the top symbol from the stack and gives it a tree action
     *
     * @param action The tree action to apply to the symbol
     */
    public void reductionPop(byte action) {
        SubTree sub = stack[stackNext + popCount];
        reductionAddSub(sub, action);
        sub.free();
        popCount++;
    }

    /**
     * During a reduction, inserts a virtual symbol
     *
     * @param index  The virtual symbol's index
     * @param action The tree action applied onto the symbol
     */
    public void reductionAddVirtual(int index, byte action) {
        if (action == LROpCode.TREE_ACTION_DROP)
            return; // why would you do this?
        cache.setAt(cacheNext, SymbolRef.encode(SymbolType.VIRTUAL, index), action);
        handle[handleNext++] = cacheNext++;
    }

    /**
     * Finalizes the reduction operation
     */
    public void reduce() {
        if (cache.getActionAt(0) == LROpCode.TREE_ACTION_REPLACE) {
            cache.setChildrenCountAt(0, handleNext);
        } else {
            ReduceTree();
        }
        // Put it on the stack
        stack[stackNext++] = cache;
    }

    /**
     * Applies the promotion tree actions to the cache and commits to the final AST
     */
    protected void ReduceTree() {
        // promotion data
        boolean promotion = false;
        int insertion = 1;
        for (int i = 0; i != handleNext; i++) {
            switch (cache.getActionAt(handle[i])) {
                case LROpCode.TREE_ACTION_PROMOTE:
                    if (promotion) {
                        // This is not the first promotion
                        // Commit the previously promoted node's children
                        cache.setChildrenCountAt(0, insertion - 1);
                        cache.commitChildrenOf(0, result);
                        // Reput the previously promoted node in the cache
                        cache.move(0, 1);
                        insertion = 2;
                    }
                    promotion = true;
                    // Save the new promoted node
                    cache.move(handle[i], 0);
                    // Repack the children on the left if any
                    int nb = cache.getChildrenCountAt(0);
                    cache.moveRange(handle[i] + 1, insertion, nb);
                    insertion += nb;
                    break;
                default:
                    // Commit the children if any
                    cache.commitChildrenOf(handle[i], result);
                    // Repack the sub-root on the left
                    if (insertion != handle[i])
                        cache.move(handle[i], insertion);
                    insertion++;
                    break;
            }
        }
        // finalize the sub-tree data
        cache.setChildrenCountAt(0, insertion - 1);
    }

    /**
     * Finalizes the parse tree and returns it
     *
     * @return The final parse tree
     */
    public AST GetTree() {
        // Get the axiom's sub tree
        SubTree sub = stack[stackNext - 2];
        // Commit the remaining sub-tree
        sub.commit(result);
        return result;
    }
}
