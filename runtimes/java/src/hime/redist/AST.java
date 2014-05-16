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

package hime.redist;

import java.util.List;

/**
 * Represents an Abstract Syntax Tree produced by a parser
 */
public interface AST {
    /**
     * Gets the root node of this tree
     *
     * @return the root node of this tree
     */
    ASTNode getRoot();

    /**
     * Gets the symbol of the given node
     *
     * @param node A node
     * @return The node's symbol
     */
    Symbol getSymbol(int node);

    /**
     * Gets the number of children of the given node
     *
     * @param node A node
     * @return The node's numer of children
     */
    int getChildrenCount(int node);

    /**
     * Gets the i-th child of the given node
     *
     * @param parent A node
     * @param i      The child's number
     * @return The i-th child
     */
    ASTNode getChild(int parent, int i);

    /**
     * Gets the children of the given node
     *
     * @param parent A node
     * @return The children
     */
    List<ASTNode> getChildren(int parent);

    /**
     * Gets the position in the input text of the given node
     *
     * @param node A node
     * @return The position in the text
     */
    TextPosition getPosition(int node);
}
