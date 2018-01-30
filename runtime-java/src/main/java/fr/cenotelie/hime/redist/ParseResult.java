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

import java.util.Collections;
import java.util.List;

/**
 * Represents the output of a parser
 *
 * @author Laurent Wouters
 */
public class ParseResult {
    /**
     * The list of errors
     */
    private final List<ParseError> errors;
    /**
     * The parsed text
     */
    private final Text text;
    /**
     * The produced AST
     */
    private AST ast;

    /**
     * Initializes this result as a failure
     *
     * @param errors The list of errors
     * @param text   The parsed text
     */
    public ParseResult(List<ParseError> errors, Text text) {
        this.errors = Collections.unmodifiableList(errors);
        this.text = text;
    }

    /**
     * Initializes this result as a failure
     *
     * @param errors The list of errors
     * @param text   The parsed text
     * @param ast    The produced AST
     */
    public ParseResult(List<ParseError> errors, Text text, AST ast) {
        this.errors = Collections.unmodifiableList(errors);
        this.text = text;
        this.ast = ast;
    }

    /**
     * Gets whether the parser was successful
     *
     * @return true if the parser was successful
     */
    public boolean isSuccess() {
        return (ast != null);
    }

    /**
     * Gets a list of the parsing errors
     *
     * @return The list of errors
     */
    public List<ParseError> getErrors() {
        return errors;
    }

    /**
     * Gets the text that has been parsed
     *
     * @return The parsed text
     */
    public Text getInput() {
        return text;
    }

    /**
     * Gets the root of the produced parse tree
     *
     * @return The root of the produced parse tree
     */
    public ASTNode getRoot() {
        if (ast == null)
            return null;
        return ast.getRoot();
    }

    /**
     * Gets the token (if any) that contains the specified index in the input text
     *
     * @param index An index within the input text
     * @return The token, if any
     */
    public Token findTokenAt(int index) {
        if (ast == null)
            return null;
        return ast.findTokenAt(index);
    }

    /**
     * Gets the token (if any) that contains the specified position in the input text
     *
     * @param position A position within the input text
     * @return The token, if any
     */
    public Token findTokenAt(TextPosition position) {
        if (ast == null)
            return null;
        int index = text.getLineIndex(position.getLine()) + position.getColumn() - 1;
        return ast.findTokenAt(index);
    }

    /**
     * Gets the AST node (if any) that has the specified token as label
     *
     * @param token The token to look for
     * @return The AST node, if any
     */
    public ASTNode findNodeFor(Token token) {
        if (ast == null)
            return null;
        return ast.findNodeFor(token);
    }

    /**
     * Gets the AST node (if any) that has a token label that contains the specified index in the input text
     *
     * @param index An index within the input text
     * @return The AST node, if any
     */
    public ASTNode findNodeAt(int index) {
        if (ast == null)
            return null;
        return ast.findNodeFor(findTokenAt(index));
    }

    /**
     * Gets the AST node (if any) that has a token label that contains the specified position in the input text
     *
     * @param position A position within the input text
     * @return The AST node, if any
     */
    public ASTNode findNodeAt(TextPosition position) {
        if (ast == null)
            return null;
        return ast.findNodeFor(findTokenAt(position));
    }
}
