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
package hime.redist.parsers;

import hime.redist.*;

import java.util.ArrayList;
import java.util.List;

/**
 * Represents a base class for AST implementations
 */
abstract class ASTImpl implements AST {
    /**
     * Represents a node in this AST
     */
    public static class Node {
        /**
         * The node's symbol reference
         */
        public int symbol;
        /**
         * The number of children
         */
        public int count;
        /**
         * The index of the first child
         */
        public int first;

        /**
         * Initializes this node
         *
         * @param symbol The node's symbol
         */
        public Node(int symbol) {
            this.symbol = symbol;
            this.count = 0;
            this.first = -1;
        }
    }

    /**
     * The table of tokens
     */
    protected TokenizedText tableTokens;
    /**
     * The table of variables
     */
    protected List<Symbol> tableVariables;
    /**
     * The table of virtuals
     */
    protected List<Symbol> tableVirtuals;
    /**
     * The nodes' labels
     */
    protected List<Node> nodes;
    /**
     * The index of the tree's root node
     */
    protected int root;


    /**
     * Initializes this AST
     *
     * @param text      The table of tokens
     * @param variables The table of variables
     * @param virtuals  The table of virtuals
     */
    public ASTImpl(TokenizedText text, List<Symbol> variables, List<Symbol> virtuals) {
        this.tableTokens = text;
        this.tableVariables = variables;
        this.tableVirtuals = virtuals;
        this.nodes = new ArrayList<Node>();
        this.root = -1;
    }

    /**
     * Gets the root node of this tree
     *
     * @return the root node of this tree
     */
    public ASTNode getRoot() {
        return new ASTNode(this, this.root);
    }

    /**
     * Gets the symbol of the given node
     *
     * @param node A node
     * @return The node's symbol
     */
    public Symbol getSymbol(int node) {
        return getSymbolFor(nodes.get(node).symbol);
    }

    /**
     * Gets the number of children of the given node
     *
     * @param node A node
     * @return The node's numer of children
     */
    public int getChildrenCount(int node) {
        return nodes.get(node).count;
    }

    /**
     * Gets the position in the input text of the given node
     *
     * @param node A node
     * @return The position in the text
     */
    public TextPosition getPosition(int node) {
        int ref = nodes.get(node).symbol;
        if (SymbolRef.getType(ref) == SymbolType.TOKEN)
            return tableTokens.getPositionOf(SymbolRef.getIndex(ref));
        return new TextPosition(0, 0);
    }

    /**
     * Gets the symbol corresponding to the given symbol reference
     *
     * @param symRef A symbol reference
     * @return The corresponding symbol
     */
    public Symbol getSymbolFor(int symRef) {
        switch (SymbolRef.getType(symRef)) {
            case SymbolType.TOKEN:
                return tableTokens.at(SymbolRef.getIndex(symRef));
            case SymbolType.VARIABLE:
                return tableVariables.get(SymbolRef.getIndex(symRef));
            case SymbolType.VIRTUAL:
                return tableVirtuals.get(SymbolRef.getIndex(symRef));
        }
        // This cannot happen
        return null;
    }

    /**
     * Stores some children nodes in this AST
     *
     * @param nodes The nodes to store
     * @param index The starting index of the nodes in the data to store
     * @param count The number of nodes to store
     * @return The index of the first inserted node in this tree
     */
    public int Store(Node[] nodes, int index, int count) {
        int result = this.nodes.size();
        for (int i = index; i != index + count; i++)
            this.nodes.add(nodes[i]);
        return result;
    }
}
