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

import fr.cenotelie.hime.redist.utils.BinaryInput;

/**
 * Represents the contexts opening by transitions from a state
 *
 * @author Laurent Wouters
 */
public class LRContexts {
    /**
     * The contexts
     */
    private final char[] content;

    /**
     * Gets the number of contexts
     *
     * @return The number of contexts
     */
    public int size() {
        return content != null ? content.length : 0;
    }

    /**
     * Loads the contexts from the specified input
     *
     * @param input An input
     */
    public LRContexts(BinaryInput input) {
        int count = input.readChar();
        if (count > 0) {
            this.content = new char[count * 2];
            for (int i = 0; i != count * 2; i++) {
                this.content[i] = input.readChar();
            }
        } else {
            this.content = null;
        }
    }

    /**
     * Gets whether the specified context opens by a transition using the specified terminal ID
     *
     * @param terminalID The identifier of a terminal
     * @param context    A context
     * @return <code>true</code> if the specified context is opened
     */
    public boolean opens(int terminalID, int context) {
        if (content == null)
            return false;
        int index = 0;
        while (index != content.length && content[index] != terminalID)
            index += 2;
        if (index == content.length)
            // not found
            return false;
        while (index != content.length && content[index] == terminalID) {
            if (content[index + 1] == context)
                return true;
            index += 2;
        }
        return false;
    }
}
