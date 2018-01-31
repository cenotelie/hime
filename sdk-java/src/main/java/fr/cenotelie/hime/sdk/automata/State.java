/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

package fr.cenotelie.hime.sdk.automata;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.List;

/**
 * Represents a state in a Finite Automaton
 *
 * @author Laurent Wouters
 */
public class State {
    /**
     * List of the items on this state
     */
    private final List<FinalItem> items;

    /**
     * Gets the items on this state
     *
     * @return The items on this state
     */
    public List<FinalItem> getItems() {
        return Collections.unmodifiableList(items);
    }

    /**
     * Gets whether this state is final (i.e. it is marked with final items)
     *
     * @return Whether this state is final (i.e. it is marked with final items)
     */
    public boolean isFinal() {
        return !items.isEmpty();
    }

    /**
     * Initializes this state
     */
    public State() {
        this.items = new ArrayList<>();
    }

    /**
     * Adds a new item making this state a final state
     *
     * @param item The new item
     */
    public void addItem(FinalItem item) {
        if (!items.contains(item)) {
            items.add(item);
            items.sort((item1, item2) -> item2.getPriority() - item1.getPriority());
        }
    }

    /**
     * Adds new items making this state a final state
     *
     * @param items The new items
     */
    public void addItems(Collection<FinalItem> items) {
        boolean modified = false;
        for (FinalItem item : items) {
            if (!this.items.contains(item)) {
                modified = true;
                this.items.add(item);
            }
        }
        if (modified)
            this.items.sort((item1, item2) -> item2.getPriority() - item1.getPriority());
    }

    /**
     * Clears all markers for this states making it non-final
     */
    public void clearItems() {
        items.clear();
    }
}
