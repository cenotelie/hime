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

import fr.cenotelie.hime.redist.AST;

/**
 * Represents a sub-tree in an AST
 * A sub-tree is composed of a root with its children.
 * The children may also have children.
 * The maximum depth of a sub-tree is 2 (root, children and children's children), in which case the root is always a replaceable node.
 * The internal representation of a sub-tree is based on arrays.
 * The organization is that a node's children are immediately following it in the array.
 * For example, the tree A(B(CD)E(FG)) is represented as [ABCDEFG].
 *
 * @author Laurent wouters
 */
class SubTree implements Cloneable {
    /**
     * The pool containing this object
     */
    private final Pool<SubTree> pool;
    /**
     * The nodes in this buffer
     */
    private final AST.Node[] nodes;
    /**
     * The tree actions for the nodes
     */
    private final byte[] actions;

    /**
     * Gets the label of the node at the given index
     *
     * @param index The index within the buffer
     * @return The label in the buffer
     */
    public int getLabelAt(int index) {
        return nodes[index].label;
    }

    /**
     * Gets the tree action applied onto the node at the given index
     *
     * @param index The index within the buffer
     * @return The tree action in the buffer
     */
    public byte getActionAt(int index) {
        return actions[index];
    }

    /**
     * Sets the tree action applied onto the node at the given index
     *
     * @param index  The index within the buffer
     * @param action The tree action to apply
     */
    public void setActionAt(int index, byte action) {
        actions[index] = action;
    }

    /**
     * Gets the number of children of the node at the given index
     *
     * @param index The index within the buffer
     * @return The number of children
     */
    public int getChildrenCountAt(int index) {
        return nodes[index].count;
    }

    /**
     * Sets the number of children of the node at the given index
     *
     * @param index The index within the buffer
     * @param count The number of children
     */
    public void setChildrenCountAt(int index, int count) {
        nodes[index].count = count;
    }

    /**
     * Gets the total number of nodes in this sub-tree
     *
     * @return The total number of nodes in this sub-tree
     */
    public int size() {
        if (actions[0] != LROpCode.TREE_ACTION_REPLACE_BY_CHILDREN) {
            return nodes[0].count + 1;
        }
        int size = 1;
        for (int i = 0; i != nodes[0].count; i++)
            size += nodes[size].count + 1;
        return size;
    }

    /**
     * Instantiates a new sub-tree attached to the given pool, with the given capacity
     *
     * @param pool     The pool to which this sub-tree is attached
     * @param capacity The capacity of the internal buffer of this sub-tree
     */
    public SubTree(Pool<SubTree> pool, int capacity) {
        this.pool = pool;
        this.nodes = new AST.Node[capacity];
        this.actions = new byte[capacity];
    }

    public SubTree clone() {
        SubTree result = (pool != null) ? pool.acquire() : new SubTree(null, nodes.length);
        int size = size();
        System.arraycopy(this.nodes, 0, result.nodes, 0, size);
        System.arraycopy(this.actions, 0, result.actions, 0, size);
        return result;
    }

    /**
     * Initializes the root of this sub-tree
     *
     * @param symbol The root's symbol
     * @param action The tree action applied on the root
     */
    public void setupRoot(int symbol, byte action) {
        nodes[0] = new AST.Node(symbol);
        actions[0] = action;
    }

    /**
     * Copy the content of this sub-tree to the given sub-tree's buffer beginning at the given index
     * This methods only applies in the case of a depth 1 sub-tree (only a root and its children).
     * The results of this method in the case of a depth 2 sub-tree is undetermined.
     *
     * @param destination The sub-tree to copy to
     * @param index       The starting index in the destination's buffer
     */
    public void copyTo(SubTree destination, int index) {
        if (this.nodes[0].count == 0) {
            destination.nodes[index] = this.nodes[0];
            destination.actions[index] = this.actions[0];
        } else {
            int size = this.nodes[0].count + 1;
            System.arraycopy(this.nodes, 0, destination.nodes, index, size);
            System.arraycopy(this.actions, 0, destination.actions, index, size);
        }
    }

    /**
     * Copy the root's children of this sub-tree to the given sub-tree's buffer beginning at the given index
     * This methods only applies in the case of a depth 1 sub-tree (only a root and its children).
     * The results of this method in the case of a depth 2 sub-tree is undetermined.
     *
     * @param destination The sub-tree to copy to
     * @param index       The starting index in the destination's buffer
     */
    public void copyChildrenTo(SubTree destination, int index) {
        if (this.nodes[0].count == 0)
            return;
        int size = size() - 1;
        System.arraycopy(this.nodes, 1, destination.nodes, index, size);
        System.arraycopy(this.actions, 1, destination.actions, index, size);
    }

    /**
     * Commits the children of a sub-tree in this buffer to the final ast
     * If the index is 0, the root's children are commited, assuming this is a depth-1 sub-tree.
     * If not, the children of the child at the given index are commited.
     *
     * @param index The starting index of the sub-tree
     * @param ast   The ast to commit to
     */
    public void commitChildrenOf(int index, AST ast) {
        if (nodes[index].count != 0)
            nodes[index].first = ast.store(nodes, index + 1, nodes[index].count);
    }

    /**
     * Commits this buffer to the final ast
     *
     * @param ast The ast to commit to
     */
    public void commit(AST ast) {
        commitChildrenOf(0, ast);
        ast.storeRoot(nodes[0]);
    }

    /**
     * Sets the content of the i-th item
     *
     * @param index  The index of the item to set
     * @param symbol The symbol
     * @param action The tree action
     */
    public void setAt(int index, int symbol, byte action) {
        nodes[index] = new AST.Node(symbol);
        actions[index] = action;
    }

    /**
     * Moves an item within the buffer
     *
     * @param from The index of the item to move
     * @param to   The destination index for the item
     */
    public void move(int from, int to) {
        this.nodes[to] = this.nodes[from];
    }

    /**
     * Moves a range of items within the buffer
     *
     * @param from   The starting index of the items to move
     * @param to     The destination index for the items
     * @param length The number of items to move
     */
    public void moveRange(int from, int to, int length) {
        if (length != 0) {
            System.arraycopy(nodes, from, nodes, to, length);
            System.arraycopy(actions, from, actions, to, length);
        }
    }

    /**
     * Releases this sub-tree to the pool
     */
    public void free() {
        if (pool != null)
            pool.putBack(this);
    }
}
