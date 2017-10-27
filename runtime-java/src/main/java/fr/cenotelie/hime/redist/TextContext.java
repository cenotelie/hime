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

/**
 * Represents the context description of a position in a piece of text.
 * A context two pieces of text, the line content and the pointer.
 * For example, given the piece of text:
 * "public Struct Context"
 * A context pointing to the second word will look like:
 * content = "public Struct Context"
 * pointer = "       ^"
 *
 * @author Laurent Wouters
 */
public class TextContext {
    /**
     * The text content being represented
     */
    private final String content;
    /**
     * The pointer textual representation
     */
    private final String pointer;

    /**
     * Gets the text content being represented
     *
     * @return The text content being represented
     */
    public String getContent() {
        return content;
    }

    /**
     * Gets the pointer textual representation
     *
     * @return The pointer textual representation
     */
    public String getPointer() {
        return pointer;
    }

    /**
     * Initializes this empty context
     */
    public TextContext() {
        this.content = null;
        this.pointer = null;
    }

    /**
     * Initializes this context
     *
     * @param content The text being begin represented
     * @param pointer The pointer textual representation
     */
    public TextContext(String content, String pointer) {
        this.content = content;
        this.pointer = pointer;
    }
}
