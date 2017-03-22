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
package org.xowl.hime.redist;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * Represents a simple AST with a tree structure
 * The nodes are stored in sequential arrays where the children of a node are an inner sequence.
 * The linkage is represented by each node storing its number of children and the index of its first child.
 *
 * @author Laurent Wouters
 */
public class ASTSimpleTree extends AST {
    /**
     * Initializes this AST
     *
     * @param tokens    The table of tokens
     * @param variables The table of variables
     * @param virtuals  The table of virtuals
     */
    public ASTSimpleTree(TokenRepository tokens, List<Symbol> variables, List<Symbol> virtuals) {
        super(tokens, variables, virtuals);
    }

    @Override
    public List<ASTNode> getChildren(int parent) {
        List<ASTNode> result = new ArrayList<>();
        Node node = nodes.get(parent);
        for (int i = 0; i != node.count; i++) {
            result.add(new ASTNode(this, node.first + i));
        }
        return Collections.unmodifiableList(result);
    }

    /**
     * Stores the root of this tree
     *
     * @param node The root
     */
    public void storeRoot(Node node) {
        root = nodes.add(node);
    }
}
