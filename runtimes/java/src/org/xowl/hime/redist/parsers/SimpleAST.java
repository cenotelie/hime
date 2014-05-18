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

import java.util.ArrayList;
import java.util.List;

/**
 * Represents a simple AST with a tree structure
 * <p/>
 * The nodes are stored in sequential arrays where the children of a node are an inner sequence.
 * The linkage is represented by each node storing its number of children and the index of its first child.
 */
class SimpleAST extends ASTImpl {

    /**
     * Initializes this AST
     *
     * @param text      The table of tokens
     * @param variables The table of variables
     * @param virtuals  The table of virtuals
     */
    public SimpleAST(TokenizedText text, List<Symbol> variables, List<Symbol> virtuals) {
        super(text, variables, virtuals);
    }

    /**
     * Gets the i-th child of the given node
     *
     * @param parent A node
     * @param i      The child's number
     * @return The i-th child
     */
    public ASTNode getChild(int parent, int i) {
        return new ASTNode(this, nodes.get(parent).first + i);
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
            result.add(new ASTNode(this, node.first + i));
        return result;
    }

    /**
     * Stores the root of this tree
     *
     * @param node The root
     */
    public void StoreRoot(Node node) {
        this.root = this.nodes.size();
        this.nodes.add(node);
    }
}
