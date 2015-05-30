/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters
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
package org.xowl.hime.redist;

import org.xowl.hime.redist.utils.IntBigList;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * Represents an AST using a graph structure
 *
 * @author Laurent Wouters
 */
public class ASTGraph extends AST {
    /**
     * The adjacency table
     */
    private final IntBigList adjacency;

    /**
     * Initializes this AST
     *
     * @param text      The table of tokens
     * @param variables The table of variables
     * @param virtuals  The table of virtuals
     */
    public ASTGraph(TokenRepository text, List<Symbol> variables, List<Symbol> virtuals) {
        super(text, variables, virtuals);
        this.adjacency = new IntBigList();
    }

    @Override
    public List<ASTNode> getChildren(int parent) {
        List<ASTNode> result = new ArrayList<ASTNode>();
        Node node = nodes.get(parent);
        for (int i = 0; i != node.count; i++)
            result.add(new ASTNode(this, adjacency.get(node.first + i)));
        return Collections.unmodifiableList(result);
    }

    /**
     * Stores the specified symbol in this AST as a new node
     *
     * @param symbol The symbol to store
     * @return The index of the new node
     */
    public int store(int symbol) {
        return nodes.add(new Node(symbol));
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
        Node copy = nodes.get(result).clone();
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
        Node temp = nodes.get(node);
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
        Node temp = nodes.get(node);
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
