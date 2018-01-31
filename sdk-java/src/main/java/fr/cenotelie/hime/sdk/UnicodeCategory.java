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

package fr.cenotelie.hime.sdk;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;

/**
 * Represents a Unicode category
 *
 * @author Laurent Wouters
 */
public class UnicodeCategory {
    /**
     * The category's name
     */
    private final String name;
    /**
     * The list of character spans contained in this category
     */
    private final Collection<UnicodeSpan> spans;

    /**
     * Gets the category's name
     *
     * @return The category's name
     */
    public String getName() {
        return name;
    }

    /**
     * Gets the list of character spans contained in this category
     *
     * @return The list of character spans contained in this category
     */
    public Collection<UnicodeSpan> getSpans() {
        return Collections.unmodifiableCollection(spans);
    }

    /**
     * Initializes a new (empty) category
     *
     * @param name The category's name
     */
    public UnicodeCategory(String name) {
        this.name = name;
        this.spans = new ArrayList<>();
    }

    /**
     * Adds a span to this category
     *
     * @param begin The span's beginning character
     * @param end   The span's ending character
     */
    public void addSpan(int begin, int end) {
        this.spans.add(new UnicodeSpan(begin, end));
    }

    /**
     * Aggregate the specified category into this one
     *
     * @param category The category to aggregate
     */
    public void aggregate(UnicodeCategory category) {
        this.spans.addAll(category.spans);
    }
}
