/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters and others
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
 ******************************************************************************/
package org.xowl.hime.redist.parsers;

import org.xowl.hime.redist.*;

import java.util.Arrays;
import java.util.List;

/**
 * Represents a structure that helps build a Shared Packed Parse Forest (SPPF)
 * A SPPF is a compact representation of multiple variants of an AST at once.
 * GLR algorithms originally builds the complete SPPF.
 * However we only need to build one of the variant, i.e. an AST for the user.
 *
 * @author Laurent Wouters
 */
class SPPFBuilder implements SemanticBody {
    /**
     * The initial size of the reduction handle
     */
    private static final int INIT_HANDLE_SIZE = 1024;
    /**
     * The initial size of the history buffer
     */
    private static final int INIT_HISTORY_SIZE = 8;
    /**
     * The initial size of the history parts' buffers
     */
    private static final int INIT_HISTORY_PART_SIZE = 64;

    /**
     * Gets the EPSILON GSS label
     */
    public static final GSSLabel EPSILON = new GSSLabel(null);

    /**
     * Represents a generation of GSS edges in the current history
     * The history is used to quickly find pre-existing matching GSS edges
     */
    private static class HistoryPart {
        /**
         * The GSS labels in this part
         */
        public GSSLabel[] data;
        /**
         * The index of the represented GSS generation
         */
        public int generation;
        /**
         * The next available slot in the data
         */
        public int next;

        /**
         * Initializes a new instance
         */
        public HistoryPart() {
            this.generation = 0;
            this.data = new GSSLabel[INIT_HISTORY_PART_SIZE];
            this.next = 0;
        }
    }

    /**
     * The pool of sub-tree with a capacity of 128 nodes
     */
    private final Pool<SubTree> pool8;
    /**
     * The pool of sub-tree with a capacity of 128 nodes
     */
    private final Pool<SubTree> pool128;
    /**
     * The pool of sub-tree with a capacity of 1024 nodes
     */
    private final Pool<SubTree> pool1024;
    /**
     * The pool of history parts
     */
    private final Pool<HistoryPart> poolHPs;
    /**
     * The history
     */
    private HistoryPart[] history;
    /**
     * The next available slot for a history part
     */
    private int nextHP;

    /**
     * The adjacency cache for the reduction
     */
    private int[] cacheChildren;
    /**
     * The actions cache for the reduction
     */
    private byte[] cacheActions;
    /**
     * The new available slot in the current cache
     */
    private int cacheNext;
    /**
     * The reduction handle represented as the indices of the sub-trees in the cache
     */
    private int[] handle;
    /**
     * The index of the next available slot in the handle
     */
    private int handleNext;
    /**
     * The stack of semantic objects for the reduction
     */
    private final GSSLabel[] stack;
    /**
     * The number of items popped from the stack
     */
    private int popCount;

    /**
     * The AST being built
     */
    private final ASTGraph result;

    /**
     * Gets the symbol at the i-th index
     *
     * @param index Index of the symbol
     * @return The symbol at the given index
     */
    public Symbol at(int index) {
        return result.getSymbol(cacheChildren[handle[index]]);
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
     * Initializes this SPPF
     *
     * @param tokens    The token table
     * @param variables The table of parser variables
     * @param virtuals  The table of parser virtuals
     */
    public SPPFBuilder(TokenRepository tokens, List<Symbol> variables, List<Symbol> virtuals) {
        this.pool8 = new Pool<SubTree>(new Factory<SubTree>() {
            @Override
            public SubTree createNew() {
                return new SubTree(pool8, 8);
            }
        }, 1024, SubTree.class);
        this.pool128 = new Pool<SubTree>(new Factory<SubTree>() {
            @Override
            public SubTree createNew() {
                return new SubTree(pool128, 128);
            }
        }, 128, SubTree.class);
        this.pool1024 = new Pool<SubTree>(new Factory<SubTree>() {
            @Override
            public SubTree createNew() {
                return new SubTree(pool1024, 1024);
            }
        }, 16, SubTree.class);
        this.poolHPs = new Pool<HistoryPart>(new Factory<HistoryPart>() {
            @Override
            public HistoryPart createNew() {
                return new HistoryPart();
            }
        }, INIT_HISTORY_SIZE, HistoryPart.class);
        this.history = new HistoryPart[INIT_HISTORY_SIZE];
        this.nextHP = 0;
        this.cacheChildren = new int[INIT_HANDLE_SIZE];
        this.cacheActions = new byte[INIT_HANDLE_SIZE];
        this.handle = new int[INIT_HANDLE_SIZE];
        this.stack = new GSSLabel[INIT_HANDLE_SIZE];
        this.result = new ASTGraph(tokens, variables, virtuals);
    }

    /**
     * Gets the history part for the given GSS generation
     *
     * @param generation The index of a GSS generation
     * @return The corresponding history part, or null
     */
    private HistoryPart getHistoryPart(int generation) {
        for (int i = 0; i != nextHP; i++)
            if (history[i].generation == generation)
                return history[i];
        return null;
    }

    /**
     * Clears the current history
     */
    public void clearHistory() {
        for (int i = 0; i != nextHP; i++)
            poolHPs.putBack(history[i]);
        nextHP = 0;
    }

    /**
     * Gets the GSS label already in history for the given GSS generation and symbol
     *
     * @param generation The index of a GSS generation
     * @param symbol     A symbol to look for
     * @return The existing GSS label, or the EPSILON label
     */
    public GSSLabel getLabelFor(int generation, int symbol) {
        HistoryPart hp = getHistoryPart(generation);
        if (hp == null)
            return EPSILON;
        for (int i = 0; i != hp.next; i++) {
            if (hp.data[i].isReplaceable()) {
                if (hp.data[i].getTree().getLabelAt(0) == symbol)
                    return hp.data[i];
            } else {
                if (hp.data[i].getOriginal() == symbol)
                    return hp.data[i];
            }
        }
        return EPSILON;
    }

    /**
     * Creates a single node in the result SPPF an returns it
     *
     * @param symbol The symbol as the node's label
     * @return The created node's index in the SPPF
     */
    public int getSingleNode(int symbol) {
        return result.store(symbol);
    }

    /**
     * Gets a pooled sub-tree with the given maximal size
     *
     * @param size The size of the sub-tree
     * @return A pooled sub-tree with the given maximal size
     */
    private SubTree getSubTree(int size) {
        if (size <= 8)
            return pool8.acquire();
        if (size <= 128)
            return pool128.acquire();
        if (size <= 1024)
            return pool1024.acquire();
        return new SubTree(null, size);
    }

    /**
     * Prepares for the forthcoming reduction operations
     *
     * @param first  The first label
     * @param path   The path being reduced
     * @param length The reduction length
     */
    public void reductionPrepare(GSSLabel first, GSSPath path, int length) {
        // build the stack
        if (length > 0) {
            for (int i = 0; i < length - 1; i++)
                stack[i] = path.get(length - 2 - i);
            stack[length - 1] = first;
        }
        // initialize the reduction data
        this.cacheNext = 0;
        this.handleNext = 0;
        this.popCount = 0;
    }

    /**
     * During a reduction, pops the top symbol from the stack and gives it a tree action
     *
     * @param action The tree action to apply to the symbol
     */
    public void reductionPop(byte action) {
        addToCache(stack[popCount++], action);
    }

    /**
     * Adds the specified GSS label to the reduction cache with the given tree action
     *
     * @param label  The label to add to the cache
     * @param action The tree action to apply
     */
    private void addToCache(GSSLabel label, byte action) {
        if (action == LROpCode.TREE_ACTION_DROP)
            return;
        if (label.isReplaceable()) {
            // this is replaceable sub-tree
            SubTree sub = label.getTree();
            for (int i = 0; i != sub.getChildrenCountAt(0); i++)
                addToCache(TableElemRef.getIndex(sub.getLabelAt(i + 1)), sub.getActionAt(i + 1));
        } else {
            // this is a simple reference to an existing SPPF node
            addToCache(label.getIndex(), action);
        }
    }

    /**
     * Adds the specified SPPF node to the cache
     *
     * @param node   The node to add to the cache
     * @param action The tree action to apply onto the node
     */
    private void addToCache(int node, byte action) {
        int count = result.getChildrenCount(node);
        if (cacheNext + count >= cacheChildren.length) {
            // the current cache is not big enough, build a bigger one
            cacheChildren = Arrays.copyOf(cacheChildren, cacheChildren.length + INIT_HANDLE_SIZE);
            cacheActions = Arrays.copyOf(cacheActions, cacheActions.length + INIT_HANDLE_SIZE);
        }
        // add the node in the cache
        cacheChildren[cacheNext] = node;
        cacheActions[cacheNext] = action;
        // setup the handle to point to the root
        if (handleNext == handle.length)
            handle = Arrays.copyOf(handle, handle.length + INIT_HANDLE_SIZE);
        handle[handleNext++] = cacheNext;
        // copy the root's children
        result.getAdjacency(node, cacheChildren, cacheNext + 1);
        cacheNext += count + 1;
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
        addToCache(result.store(TableElemRef.encode(TableElemRef.TABLE_VIRTUAL, index)), action);
    }

    /**
     * During a reduction, inserts the sub-tree of a nullable variable
     *
     * @param nullable The sub-tree of a nullable variable
     * @param action   The tree action applied onto the symbol
     */
    public void reductionAddNullable(GSSLabel nullable, byte action) {
        if (action == LROpCode.TREE_ACTION_DROP)
            return;
        if (nullable.isReplaceable()) {
            addToCache(new GSSLabel(nullable.getTree().clone()), action);
        } else {
            addToCache(result.copyNode(nullable.getIndex()), action);
        }
    }

    /**
     * Finalizes the reduction operation
     *
     * @param generation  The generation to reduce from
     * @param varIndex    The reduced variable index
     * @param replaceable Whether the sub-tree to build must have a replaceable root or not
     * @return The produced sub-tree
     */
    public GSSLabel reduce(int generation, int varIndex, boolean replaceable) {
        GSSLabel label = replaceable ? reduceReplaceable(varIndex) : reduceNormal(varIndex);
        addToHistory(generation, label);
        return label;
    }

    /**
     * Executes the reduction as a normal reduction
     *
     * @param varIndex The reduced variable index
     * @return The produced sub-tree
     */
    private GSSLabel reduceNormal(int varIndex) {
        int root = -1;
        int insertion = 0;
        for (int i = 0; i != handleNext; i++) {
            switch (cacheActions[handle[i]]) {
                case LROpCode.TREE_ACTION_PROMOTE:
                    if (root != -1) {
                        // not the first promotion
                        // store the adjacency data for the previously promoted node
                        int index = result.store(cacheChildren, insertion);
                        result.setAdjacency(root, index, insertion);
                        // put the previously promoted node in the cache
                        cacheChildren[0] = root;
                        insertion = 1;
                    }
                    // save the new promoted node
                    root = cacheChildren[handle[i]];
                    // repack the children on the left if any
                    int nb = result.getChildrenCount(root);
                    System.arraycopy(cacheChildren, handle[i] + 1, cacheChildren, insertion, nb);
                    System.arraycopy(cacheActions, handle[i] + 1, cacheActions, insertion, nb);
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
        if (root == -1) {
            // no promotion, create the node for the root
            root = result.store(TableElemRef.encode(TableElemRef.TABLE_VARIABLE, varIndex));
        }
        // setup the adjacency for the new root
        result.setAdjacency(root, result.store(cacheChildren, insertion), insertion);
        // create the GSS label
        return new GSSLabel(TableElemRef.encode(TableElemRef.TABLE_VARIABLE, varIndex), root);
    }

    /**
     * Executes the reduction as the reduction of a replaceable variable
     *
     * @param varIndex The reduced variable index
     * @return The produced sub-tree
     */
    private GSSLabel reduceReplaceable(int varIndex) {
        SubTree tree = getSubTree(handleNext + 1);
        tree.setupRoot(TableElemRef.encode(TableElemRef.TABLE_VARIABLE, varIndex), LROpCode.TREE_ACTION_REPLACE);
        tree.setChildrenCountAt(0, handleNext);
        for (int i = 0; i != handleNext; i++) {
            int node = cacheChildren[handle[i]];
            byte action = cacheActions[handle[i]];
            tree.setAt(i + 1, TableElemRef.encode(TableElemRef.TABLE_NONE, node), action);
        }
        return new GSSLabel(tree);
    }

    /**
     * Adds the specified GSS label to the current history
     *
     * @param generation The current generation
     * @param label      The label to register
     */
    private void addToHistory(int generation, GSSLabel label) {
        HistoryPart hp = getHistoryPart(generation);
        if (hp == null) {
            hp = poolHPs.acquire();
            hp.generation = generation;
            hp.next = 0;
            if (history.length == nextHP)
                history = Arrays.copyOf(history, history.length + INIT_HISTORY_SIZE);
            history[nextHP++] = hp;
        }
        if (hp.data.length == hp.next)
            hp.data = Arrays.copyOf(hp.data, hp.data.length + INIT_HISTORY_PART_SIZE);
        hp.data[hp.next++] = label;
    }

    /**
     * Finalizes the parse tree and returns it
     *
     * @param root The root's sub-tree
     * @return The final parse tree
     */
    public AST getTree(GSSLabel root) {
        result.setRoot(root.getIndex());
        return result;
    }
}
