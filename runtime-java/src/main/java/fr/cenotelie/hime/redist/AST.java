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

package fr.cenotelie.hime.redist;

import fr.cenotelie.hime.redist.utils.BigList;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * Represents a base class for AST implementations
 *
 * @author Laurent Wouters
 */
public class AST {
    /**
     * Represents a node in this AST
     */
    public static final class Node implements Cloneable {
        /**
         * The node's label
         */
        public final int label;
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
         * @param label The node's label
         */
        public Node(int label) {
            this.label = label;
            this.count = 0;
            this.first = -1;
        }

        /**
         * Initializes this node
         *
         * @param label The node's label
         * @param count The number of children
         * @param first The index of the first child
         */
        public Node(int label, int count, int first) {
            this.label = label;
            this.count = count;
            this.first = first;
        }

        @Override
        public Node clone() {
            Node temp = new Node(label);
            temp.count = this.count;
            temp.first = this.first;
            return temp;
        }
    }

    /**
     * The table of tokens
     */
    private final TokenRepository tableTokens;
    /**
     * The table of variables
     */
    private final List<Symbol> tableVariables;
    /**
     * The table of virtuals
     */
    private final List<Symbol> tableVirtuals;
    /**
     * The nodes' labels
     */
    final BigList<Node> nodes;
    /**
     * The index of the tree's root node
     */
    int root;

    /**
     * Initializes this AST
     *
     * @param tokens    The table of tokens
     * @param variables The table of variables
     * @param virtuals  The table of virtuals
     */
    public AST(TokenRepository tokens, List<Symbol> variables, List<Symbol> virtuals) {
        this.tableTokens = tokens;
        this.tableVariables = variables;
        this.tableVirtuals = virtuals;
        this.nodes = new BigList<>(Node.class, Node[].class);
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
     * Gets the number of children of the given node
     *
     * @param node A node
     * @return The node's number of children
     */
    public int getChildrenCount(int node) {
        return nodes.get(node).count;
    }

    /**
     * Gets the children of the given node
     *
     * @param parent A node
     * @return The children
     */
    public List<ASTNode> getChildren(int parent) {
        List<ASTNode> result = new ArrayList<>();
        Node node = nodes.get(parent);
        for (int i = 0; i != node.count; i++) {
            result.add(new ASTNode(this, node.first + i));
        }
        return Collections.unmodifiableList(result);
    }

    /**
     * Gets the type of symbol for the given node
     *
     * @param node A node
     * @return The type of symbol for the node
     */
    public SymbolType getSymbolType(int node) {
        switch (TableElemRef.getType(nodes.get(node).label)) {
            case TableElemRef.TABLE_TOKEN:
                return SymbolType.Terminal;
            case TableElemRef.TABLE_VARIABLE:
                return SymbolType.Variable;
            case TableElemRef.TABLE_VIRTUAL:
                return SymbolType.Virtual;
        }
        // This cannot happen
        return null;
    }

    /**
     * Gets the position in the input text of the given node
     *
     * @param node A node
     * @return The position in the text
     */
    public TextPosition getPosition(int node) {
        int reference = nodes.get(node).label;
        if (TableElemRef.getType(reference) == TableElemRef.TABLE_TOKEN)
            return tableTokens.getPosition(TableElemRef.getIndex(reference));
        return new TextPosition(0, 0);
    }

    /**
     * Gets the span in the input text of the given node
     *
     * @param node A node
     * @return The span in the text
     */
    public TextSpan getSpan(int node) {
        int reference = nodes.get(node).label;
        if (TableElemRef.getType(reference) == TableElemRef.TABLE_TOKEN)
            return tableTokens.getSpan(TableElemRef.getIndex(reference));
        return new TextSpan(0, 0);
    }

    /**
     * Gets the context in the input of the given node
     *
     * @param node A node
     * @return The context
     */
    public TextContext getContext(int node) {
        int reference = nodes.get(node).label;
        if (TableElemRef.getType(reference) == TableElemRef.TABLE_TOKEN)
            return tableTokens.getContext(TableElemRef.getIndex(reference));
        return new TextContext();
    }

    /**
     * Gets the symbol of the given node
     *
     * @param node A node
     * @return The node's symbol
     */
    public Symbol getSymbol(int node) {
        return getSymbolFor(nodes.get(node).label);
    }

    /**
     * Gets the value of the given node
     *
     * @param node A node
     * @return The associated value
     */
    public String getValue(int node) {
        int reference = nodes.get(node).label;
        if (TableElemRef.getType(reference) == TableElemRef.TABLE_TOKEN)
            return tableTokens.getValue(TableElemRef.getIndex(reference));
        return null;
    }

    /**
     * Gets the symbol corresponding to the specified label
     *
     * @param label A node label
     * @return The corresponding symbol
     */
    public Symbol getSymbolFor(int label) {
        switch (TableElemRef.getType(label)) {
            case TableElemRef.TABLE_TOKEN:
                return tableTokens.getSymbol(TableElemRef.getIndex(label));
            case TableElemRef.TABLE_VARIABLE:
                return tableVariables.get(TableElemRef.getIndex(label));
            case TableElemRef.TABLE_VIRTUAL:
                return tableVirtuals.get(TableElemRef.getIndex(label));
            case TableElemRef.TABLE_NONE:
                return tableTokens.getTerminals().get(0);  // terminal epsilon
        }
        // This cannot happen
        return null;
    }

    /**
     * Gets the semantic element corresponding to the specified node
     *
     * @param node A node
     * @return The corresponding semantic element
     */
    public SemanticElement getSemanticElementForNode(int node) {
        return getSemanticElementForLabel(nodes.get(node).label);
    }

    /**
     * Gets the semantic element corresponding to the specified label
     *
     * @param label The label of an AST node
     * @return The corresponding semantic element
     */
    public SemanticElement getSemanticElementForLabel(int label) {
        switch (TableElemRef.getType(label)) {
            case TableElemRef.TABLE_TOKEN:
                return tableTokens.at(TableElemRef.getIndex(label));
            case TableElemRef.TABLE_VARIABLE:
                return new ASTLabel(tableVariables.get(TableElemRef.getIndex(label)), SymbolType.Variable);
            case TableElemRef.TABLE_VIRTUAL:
                return new ASTLabel(tableVirtuals.get(TableElemRef.getIndex(label)), SymbolType.Virtual);
            case TableElemRef.TABLE_NONE:
                return new ASTLabel(tableTokens.getTerminals().get(0), SymbolType.Terminal);
        }
        // This cannot happen
        return null;
    }

    /**
     * Gets the token (if any) that contains the specified index in the input text
     *
     * @param index An index within the input text
     * @return The token, if any
     */
    public Token findTokenAt(int index) {
        return tableTokens.findTokenAt(index);
    }

    /**
     * Gets the AST node (if any) that has the specified token as label
     *
     * @param token The token to look for
     * @return The AST node, if any
     */
    public ASTNode findNodeFor(Token token) {
        if (token == null)
            return null;
        for (int i = 0; i != nodes.size(); i++) {
            Node node = nodes.get(i);
            if (TableElemRef.getType(node.label) == TableElemRef.TABLE_TOKEN && TableElemRef.getIndex(node.label) == token.index)
                return new ASTNode(this, i);
        }
        return null;
    }

    /**
     * Gets the parent of the specified node, if any
     *
     * @param node A node
     * @return The parent node, if any
     */
    public ASTNode findParentOf(int node) {
        if (root == node)
            return null;
        for (int i = 0; i != nodes.size(); i++) {
            Node candidate = nodes.get(i);
            if (candidate.count > 0 && node >= candidate.first && node < candidate.first + candidate.count)
                return new ASTNode(this, i);
        }
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
    public int store(Node[] nodes, int index, int count) {
        int result = this.nodes.size();
        for (int i = index; i != index + count; i++)
            this.nodes.add(nodes[i]);
        return result;
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
