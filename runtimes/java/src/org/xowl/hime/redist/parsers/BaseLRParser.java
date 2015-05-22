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
package org.xowl.hime.redist.parsers;

import org.xowl.hime.redist.ParseError;
import org.xowl.hime.redist.SemanticAction;
import org.xowl.hime.redist.Symbol;
import org.xowl.hime.redist.lexer.ILexer;
import org.xowl.hime.redist.lexer.LexicalErrorHandler;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

/**
 * Represents a base LR parser
 */
public abstract class BaseLRParser implements IParser {
    /**
     * Maximum number of errors
     */
    protected static final int MAX_ERROR_COUNT = 100;

    /**
     * Determines whether the parser will try to recover from errors
     */
    protected boolean recover = true;
    /**
     * Value indicating whether this parser is in debug mode
     */
    protected boolean debug = false;
    /**
     * Parser's variables
     */
    protected List<Symbol> parserVariables;
    /**
     * Parser's virtuals
     */
    protected List<Symbol> parserVirtuals;
    /**
     * Parser's actions
     */
    protected SemanticAction[] parserActions;
    /**
     * List of the encountered syntaxic errors
     */
    protected List<ParseError> allErrors;
    /**
     * Lexer associated to this parser
     */
    protected ILexer lexer;


    /**
     * Gets the variable symbols used by this parser
     *
     * @return The variable symbols used by this parser
     */
    public List<Symbol> getVariables() {
        return parserVariables;
    }

    /**
     * Gets the virtual symbols used by this parser
     *
     * @return The virtual symbols used by this parser
     */
    public List<Symbol> getVirtuals() {
        return parserVirtuals;
    }

    /**
     * Gets whether the parser should try to recover from errors
     *
     * @return Whether the parser should try to recover from errors
     */
    public boolean getRecover() {
        return recover;
    }

    /**
     * Sets whether the parser should try to recover from errors
     *
     * @param recover Whether the parser should try to recover from errors
     */
    public void setRecover(boolean recover) {
        this.recover = recover;
    }

    /**
     * Gets whether this parser is in debug mode
     *
     * @return Whether this parser is in debug mode
     */
    public boolean isDebugMode() {
        return debug;
    }

    /**
     * Sets whether this parser is in debug mode
     *
     * @param mode Whether this parser is in debug mode
     */
    public void setDebugMode(boolean mode) {
        this.debug = mode;
    }

    /**
     * Initializes a new instance of the LRkParser class with the given lexer
     *
     * @param variables The parser's variable
     * @param virtuals  The parser's virtuals
     * @param actions   The parser's actions
     * @param lexer     The input lexer
     */
    protected BaseLRParser(Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, ILexer lexer) {
        this.parserVariables = Collections.unmodifiableList(Arrays.asList(variables));
        this.parserVirtuals = Collections.unmodifiableList(Arrays.asList(virtuals));
        this.parserActions = actions;
        this.recover = true;
        this.allErrors = new ArrayList<ParseError>();
        this.lexer = lexer;
        this.lexer.setErrorHandler(new LexicalErrorHandler() {
            @Override
            public void handle(ParseError error) {
                allErrors.add(error);
            }
        });
    }
}
