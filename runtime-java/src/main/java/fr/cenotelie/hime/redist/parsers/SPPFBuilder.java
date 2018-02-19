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
package fr.cenotelie.hime.redist.parsers;

import fr.cenotelie.hime.redist.*;

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
     * Represents a generation of GSS edges in the current history
     * The history is used to quickly find pre-existing matching GSS edges
     */
    private static class HistoryPart {
        /**
         * The GSS labels in this part
         */
        public int[] data;
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
            this.data = new int[INIT_HISTORY_PART_SIZE];
            this.next = 0;
        }
    }

    /**
     * A factory of history parts
     */
    private static class HistoryPartFactory implements Factory<HistoryPart> {
        @Override
        public HistoryPart createNew() {
            return new HistoryPart();
        }
    }

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
     * The SPPF being built
     */
    private SPPF sppf;
    /**
     * The adjacency cache for the reduction
     */
    private long[] cacheChildren;
    /**
     * The new available slot in the current cache
     */
    private int cacheNext;
    /**
     * The reduction handle represented as the indices of the sub-trees in the cache
     */
    private int[] handleIndices;
    /**
     * The actions cache for the reduction
     */
    private byte[] handleActions;
    /**
     * The index of the next available slot in the handle
     */
    private int handleNext;
    /**
     * The stack of semantic objects for the reduction
     */
    private final int[] stack;
    /**
     * The number of items popped from the stack
     */
    private int popCount;

    /**
     * The AST being built
     */
    private final AST result;

    /**
     * Gets the symbol at the i-th index
     *
     * @param index Index of the symbol
     * @return The symbol at the given index
     */
    public SemanticElement at(int index) {
        long reference = cacheChildren[handleIndices[index]];
        SPPFNode sppfNode = sppf.getNode(SPPF.refNodeId(reference));
        int label = ((SPPFNodeNormal) sppfNode).getVersion(SPPF.refVersion(reference)).getLabel();
        return result.getSemanticElementForLabel(label);
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
        this.poolHPs = new Pool<>(new HistoryPartFactory(), INIT_HISTORY_SIZE, HistoryPart.class);
        this.history = new HistoryPart[INIT_HISTORY_SIZE];
        this.nextHP = 0;
        this.sppf = new SPPF();
        this.cacheChildren = new long[INIT_HANDLE_SIZE];
        this.handleIndices = new int[INIT_HANDLE_SIZE];
        this.handleActions = new byte[INIT_HANDLE_SIZE];
        this.stack = new int[INIT_HANDLE_SIZE];
        this.result = new AST(tokens, variables, virtuals);
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
     * Gets the symbol on the specified GSS edge label
     *
     * @param label The label of a GSS edge
     * @return The symbol on the edge
     */
    public Symbol getSymbolOn(int label) {
        return result.getSymbolFor(sppf.getNode(label).getOriginalSymbol());
    }

    /**
     * Gets the GSS label already in history for the given GSS generation and symbol
     *
     * @param generation The index of a GSS generation
     * @param symbol     A symbol to look for
     * @return The existing GSS label, or the EPSILON label
     */
    public int getLabelFor(int generation, int symbol) {
        HistoryPart hp = getHistoryPart(generation);
        if (hp == null)
            return SPPF.EPSILON;
        for (int i = 0; i != hp.next; i++) {
            if (sppf.getNode(hp.data[i]).getOriginalSymbol() == symbol)
                return hp.data[i];
        }
        return SPPF.EPSILON;
    }

    /**
     * Creates a single node in the result SPPF an returns it
     *
     * @param symbol The symbol as the node's label
     * @return The created node's index in the SPPF
     */
    public int getSingleNode(int symbol) {
        return sppf.newNode(symbol);
    }

    /**
     * Prepares for the forthcoming reduction operations
     *
     * @param first  The first label
     * @param path   The path being reduced
     * @param length The reduction length
     */
    public void reductionPrepare(int first, GSSPath path, int length) {
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
     * @param gssLabel The label to add to the cache
     * @param action   The tree action to apply
     */
    private void addToCache(int gssLabel, byte action) {
        if (action == LROpCode.TREE_ACTION_DROP)
            return;
        SPPFNode node = sppf.getNode(gssLabel);
        if (node.isReplaceable()) {
            SPPFNodeReplaceable replaceable = (SPPFNodeReplaceable) node;
            // this is replaceable sub-tree
            for (int i = 0; i != replaceable.getChildrenCount(); i++) {
                long reference = replaceable.getChildren()[i];
                SPPFNode child = sppf.getNode(SPPF.refNodeId(reference));
                byte childAction = replaceable.getActions()[i];
                addToCache((SPPFNodeNormal) child, childAction);
            }
        } else {
            // this is a simple reference to an existing SPPF node
            addToCache((SPPFNodeNormal) node, action);
        }
    }

    /**
     * Adds the specified SPPF node to the cache
     *
     * @param node   The node to add to the cache
     * @param action The tree action to apply onto the node
     */
    private void addToCache(SPPFNodeNormal node, byte action) {
        SPPFNodeVersion version = node.getDefaultVersion();
        while (cacheNext + version.getChildrenCount() >= cacheChildren.length) {
            // the current cache is not big enough, build a bigger one
            cacheChildren = Arrays.copyOf(cacheChildren, cacheChildren.length + INIT_HANDLE_SIZE);
        }
        // add the node in the cache
        cacheChildren[cacheNext] = SPPF.reference(node.getIdentifier(), 0);
        // setup the handle to point to the root
        if (handleNext == handleIndices.length) {
            handleIndices = Arrays.copyOf(handleIndices, handleIndices.length + INIT_HANDLE_SIZE);
            handleActions = Arrays.copyOf(handleActions, handleActions.length + INIT_HANDLE_SIZE);
        }
        handleIndices[handleNext] = cacheNext;
        handleActions[handleNext] = action;
        // copy the children
        if (version.getChildrenCount() > 0)
            System.arraycopy(version.getChildren(), 0, cacheChildren, cacheNext + 1, version.getChildrenCount());
        handleNext++;
        cacheNext += version.getChildrenCount() + 1;
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
        int nodeId = sppf.newNode(TableElemRef.encode(TableElemRef.TABLE_VIRTUAL, index));
        if (cacheNext + 1 >= cacheChildren.length) {
            // the current cache is not big enough, build a bigger one
            cacheChildren = Arrays.copyOf(cacheChildren, cacheChildren.length + INIT_HANDLE_SIZE);
        }
        // add the node in the cache
        cacheChildren[cacheNext] = SPPF.reference(nodeId, 0);
        // setup the handle to point to the root
        if (handleNext == handleIndices.length) {
            handleIndices = Arrays.copyOf(handleIndices, handleIndices.length + INIT_HANDLE_SIZE);
            handleActions = Arrays.copyOf(handleActions, handleActions.length + INIT_HANDLE_SIZE);
        }
        handleIndices[handleNext] = cacheNext;
        handleActions[handleNext] = action;
        handleNext++;
        cacheNext++;
    }

    /**
     * During a reduction, inserts the sub-tree of a nullable variable
     *
     * @param nullable The sub-tree of a nullable variable
     * @param action   The tree action applied onto the symbol
     */
    public void reductionAddNullable(int nullable, byte action) {
        addToCache(nullable, action);
    }

    /**
     * Finalizes the reduction operation
     *
     * @param generation The generation to reduce from
     * @param varIndex   The reduced variable index
     * @param headAction The tree action applied in the rule's head
     * @return The identifier of the produced SPPF node
     */
    public int reduce(int generation, int varIndex, byte headAction) {
        int label = headAction == LROpCode.TREE_ACTION_REPLACE_BY_CHILDREN ? reduceReplaceable(varIndex) : reduceNormal(varIndex, headAction);
        addToHistory(generation, label);
        return label;
    }

    /**
     * Executes the reduction as a normal reduction
     *
     * @param varIndex   The reduced variable index
     * @param headAction The tree action applied in the rule's head
     * @return The identifier of the produced SPPF node
     */
    private int reduceNormal(int varIndex, byte headAction) {
        int promotedSymbol = -1;
        long promotedReference = -1;

        int insertion = 0;
        for (int i = 0; i != handleNext; i++) {
            switch (handleActions[i]) {
                case LROpCode.TREE_ACTION_PROMOTE:
                    if (promotedReference != -1) {
                        // not the first promotion
                        // create a new version for the promoted node
                        SPPFNodeNormal oldPromotedNode = (SPPFNodeNormal) sppf.getNode(SPPF.refNodeId(promotedReference));
                        long oldPromotedRef = oldPromotedNode.newVersion(promotedSymbol, cacheChildren, insertion);
                        // register the previously promoted reference into the cache
                        cacheChildren[0] = oldPromotedRef;
                        insertion = 1;
                    }
                    // save the new promoted node
                    promotedReference = cacheChildren[handleIndices[i]];
                    SPPFNodeNormal promotedNode = (SPPFNodeNormal) sppf.getNode(SPPF.refNodeId(promotedReference));
                    SPPFNodeVersion promotedVersion = promotedNode.getVersion(SPPF.refVersion(promotedReference));
                    promotedSymbol = promotedVersion.getLabel();
                    // repack the children on the left if any
                    System.arraycopy(cacheChildren, handleIndices[i] + 1, cacheChildren, insertion, promotedVersion.getChildrenCount());
                    insertion += promotedVersion.getChildrenCount();
                    break;
                default:
                    // Repack the sub-root on the left
                    if (insertion != handleIndices[i])
                        cacheChildren[insertion] = cacheChildren[handleIndices[i]];
                    insertion++;
                    break;
            }
        }
        int originalLabel = TableElemRef.encode(TableElemRef.TABLE_VARIABLE, varIndex);
        int currentLabel = originalLabel;
        if (promotedReference != -1)
            // a promotion occurred
            currentLabel = promotedSymbol;
        else if (headAction == LROpCode.TREE_ACTION_REPLACE_BY_EPSILON)
            // this variable must be replaced in the final AST
            currentLabel = TableElemRef.encode(TableElemRef.TABLE_NONE, 0);
        return sppf.newNode(originalLabel, currentLabel, cacheChildren, insertion);
    }

    /**
     * Executes the reduction as the reduction of a replaceable variable
     *
     * @param varIndex The reduced variable index
     * @return TThe identifier of the produced SPPF node
     */
    private int reduceReplaceable(int varIndex) {
        int insertion = 0;
        for (int i = 0; i != handleNext; i++) {
            if (insertion != handleIndices[i])
                cacheChildren[insertion] = cacheChildren[handleIndices[i]];
            insertion++;
        }
        int originalLabel = TableElemRef.encode(TableElemRef.TABLE_VARIABLE, varIndex);
        return sppf.newReplaceableNode(originalLabel, cacheChildren, handleActions, handleNext);
    }

    /**
     * Adds the specified GSS label to the current history
     *
     * @param generation The current generation
     * @param label      The label identifier of the SPPF node to use as a GSS label
     */
    private void addToHistory(int generation, int label) {
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
     * @param root The identifier of the SPPF node that serves as root
     * @return The final parse tree
     */
    public AST getTree(int root) {
        AST.Node astRoot = buildFinalAST(SPPF.reference(root, 0));
        result.storeRoot(astRoot);
        return result;
    }

    /**
     * Builds the final AST for the specified SPPF node reference
     *
     * @param reference A reference to an SPPF node in a specific version
     * @return The AST node for the SPPF reference
     */
    public AST.Node buildFinalAST(long reference) {
        SPPFNode sppfNode = sppf.getNode(SPPF.refNodeId(reference));
        SPPFNodeVersion version = ((SPPFNodeNormal) sppfNode).getVersion(SPPF.refVersion(reference));

        if (version.getChildrenCount() == 0)
            return new AST.Node(version.getLabel());

        AST.Node[] buffer = new AST.Node[version.getChildrenCount()];
        for (int i = 0; i != version.getChildrenCount(); i++)
            buffer[i] = buildFinalAST(version.getChildren()[i]);
        int first = result.store(buffer, 0, version.getChildrenCount());
        return new AST.Node(version.getLabel(), version.getChildrenCount(), first);
    }
}
