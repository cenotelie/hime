/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters and others
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

import java.util.Collections;
import java.util.List;

/**
 * Represents the output of a parser
 *
 * @author Laurent Wouters
 */
public final class ParseResult {
    /**
     * The list of errors
     */
    private List<ParseError> errors;
    /**
     * The parsed text
     */
    private Text text;
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
}
