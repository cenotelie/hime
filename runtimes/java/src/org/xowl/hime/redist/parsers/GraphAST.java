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

import org.xowl.hime.redist.ASTNode;
import org.xowl.hime.redist.Symbol;
import org.xowl.hime.redist.TokenizedText;
import org.xowl.hime.redist.utils.IntBigList;

import java.util.ArrayList;
import java.util.List;

/**
 * Represents an AST using a graph structure
 */
class GraphAST extends ASTImpl {
    /**
     * The adjacency table
     */
    private IntBigList adjacency;

    /**
     * Initializes this AST
     *
     * @param text      The table of tokens
     * @param variables The table of variables
     * @param virtuals  The table of virtuals
     */
    public GraphAST(TokenizedText text, List<Symbol> variables, List<Symbol> virtuals) {
        super(text, variables, virtuals);
        this.adjacency = new IntBigList();
    }

    /**
     * Gets the i-th child of the given node
     *
     * @param parent A node
     * @param i      The child's number
     * @return The i-th child
     */
    public ASTNode getChild(int parent, int i) {
        return new ASTNode(this, adjacency.get(nodes.get(parent).first + i));
    }

    /**
     * Gets the children of the given node
     *
     * @param parent A node
     * @return The children
     */
    public List<ASTNode> getChildren(int parent) {
        List<ASTNode> result = new ArrayList<ASTNode>();
        Node node = nodes.get(parent);
        for (int i = 0; i != node.count; i++)
            result.add(new ASTNode(this, adjacency.get(node.first + i)));
        return result;
    }

    /**
     * Stores the specified symbol in this AST as a new node
     *
     * @param symbol The symbol to store
     * @return The index of the new node
     */
    public int store(int symbol) {
        return nodes.add(new SimpleAST.Node(symbol));
    }

    /**
     * Stores some adjacency data in this graph AST
     *
     * @param adjacents A buffer of adjacency data
     * @param count     The number of adjacents to store
     * @return The index of the data stored in this graph
     */
    public int store(int[] adjacents, int count) {
        return adjacency.add(adjacents, 0, count);
    }

    /**
     * Copies the provided node (and its adjacency data)
     *
     * @param node The node to copy
     * @return The index of the copy
     */
    public int copyNode(int node) {
        int result = nodes.add(nodes.get(node));
        SimpleAST.Node copy = nodes.get(result).clone();
        if (copy.count != 0) {
            copy.first = adjacency.duplicate(copy.first, copy.count);
            nodes.set(result, copy);
        }
        return result;
    }

    /**
     * Gets the adjacency data for the specified node
     *
     * @param node   The node to retrieve the adjacency data of
     * @param buffer The buffer to store the retrieved data in
     * @param index  The starting index in the provided buffer
     * @return The number of adjacents
     */
    public int getAdjacency(int node, int[] buffer, int index) {
        SimpleAST.Node temp = nodes.get(node);
        for (int i = 0; i != temp.count; i++)
            buffer[index + i] = adjacency.get(temp.first + i);
        return temp.count;
    }

    /**
     * Sets the adjacency data for the specified node
     *
     * @param node  The node to set the adjacency data of
     * @param first The index of the first adjacency item
     * @param count The number of adjacency items
     */
    public void setAdjacency(int node, int first, int count) {
        SimpleAST.Node temp = nodes.get(node);
        temp.first = first;
        temp.count = count;
    }

    /**
     * Sets the root of this AST
     *
     * @param node Index of the root node
     */
    public void setRoot(int node) {
        this.root = node;
    }
}
