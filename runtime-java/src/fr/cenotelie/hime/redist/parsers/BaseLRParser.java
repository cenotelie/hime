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
package fr.cenotelie.hime.redist.parsers;

import fr.cenotelie.hime.redist.ParseError;
import fr.cenotelie.hime.redist.ParseResult;
import fr.cenotelie.hime.redist.SemanticAction;
import fr.cenotelie.hime.redist.Symbol;
import fr.cenotelie.hime.redist.lexer.BaseLexer;
import fr.cenotelie.hime.redist.lexer.LexicalErrorHandler;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

/**
 * Represents a base LR parser
 *
 * @author Laurent Wouters
 */
public abstract class BaseLRParser {
    /**
     * Maximum number of errors
     */
    static final int MAX_ERROR_COUNT = 100;
    /**
     * The default value of the recover mode
     */
    private static final boolean DEFAULT_MODE_RECOVER = true;
    /**
     * The default value of the debug mode
     */
    private static final boolean DEFAULT_MODE_DEBUG = false;

    /**
     * Determines whether the parser will try to recover from errors
     */
    private boolean modeRecover;
    /**
     * Value indicating whether this parser is in debug mode
     */
    private boolean modeDebug;
    /**
     * Parser's variables
     */
    final List<Symbol> symVariables;
    /**
     * Parser's virtuals
     */
    final List<Symbol> symVirtuals;
    /**
     * Parser's actions
     */
    final List<SemanticAction> symActions;
    /**
     * List of the encountered syntaxic errors
     */
    final List<ParseError> allErrors;
    /**
     * Lexer associated to this parser
     */
    final BaseLexer lexer;


    /**
     * Gets the variable symbols used by this parser
     *
     * @return The variable symbols used by this parser
     */
    public List<Symbol> getVariables() {
        return symVariables;
    }

    /**
     * Gets the virtual symbols used by this parser
     *
     * @return The virtual symbols used by this parser
     */
    public List<Symbol> getVirtuals() {
        return symVirtuals;
    }

    /**
     * Gets whether the parser should try to recover from errors
     *
     * @return Whether the parser should try to recover from errors
     */
    public boolean getModeRecoverErrors() {
        return modeRecover;
    }

    /**
     * Sets whether the parser should try to recover from errors
     *
     * @param recover Whether the parser should try to recover from errors
     */
    public void setModeRecoverErrors(boolean recover) {
        this.modeRecover = recover;
    }

    /**
     * Gets whether this parser is in debug mode
     *
     * @return Whether this parser is in debug mode
     */
    public boolean isModeDebug() {
        return modeDebug;
    }

    /**
     * Sets whether this parser is in debug mode
     *
     * @param mode Whether this parser is in debug mode
     */
    public void setModeDebug(boolean mode) {
        this.modeDebug = mode;
    }

    /**
     * Initializes a new instance of the LRkParser class with the given lexer
     *
     * @param variables The parser's variable
     * @param virtuals  The parser's virtuals
     * @param actions   The parser's actions
     * @param lexer     The input lexer
     */
    protected BaseLRParser(Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, BaseLexer lexer) {
        this.modeRecover = DEFAULT_MODE_RECOVER;
        this.modeDebug = DEFAULT_MODE_DEBUG;
        this.symVariables = Collections.unmodifiableList(Arrays.asList(variables));
        this.symVirtuals = Collections.unmodifiableList(Arrays.asList(virtuals));
        this.symActions = Collections.unmodifiableList(actions != null ? Arrays.asList(actions) : new ArrayList<SemanticAction>());
        this.allErrors = new ArrayList<>();
        this.lexer = lexer;
        this.lexer.setErrorHandler(new LexicalErrorHandler() {
            @Override
            public void handle(ParseError error) {
                allErrors.add(error);
            }
        });
    }

    /**
     * Parses the input and returns the result
     *
     * @return A ParseResult object containing the data about the result
     */
    public abstract ParseResult parse();
}
