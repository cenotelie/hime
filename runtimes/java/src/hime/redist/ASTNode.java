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

import java.util.Enumeration;

public class ASTNode {
    private AST tree;
    private int index;

    public Symbol getSymbol() {
        return tree.getSymbol(index);
    }

    public TextPosition getPosition() {
        return tree.getPosition(index);
    }

    public Enumeration<ASTNode> getChildren() {
        return tree.getChildren(index);
    }

    public ASTNode(AST tree, int index) {
        this.tree = tree;
        this.index = index;
    }

    @Override
    public String toString() {
        return tree.getSymbol(index).toString();
    }
}
