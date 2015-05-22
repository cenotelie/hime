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

import org.xowl.hime.redist.Symbol;

import java.util.ArrayList;
import java.util.Collection;

/**
 * Container for the expected terminals for a LR state
 */
public class LRExpected {
    /**
     * The terminals expected for shift actions
     */
    private Collection<Symbol> shifts;
    /**
     * The terminals expected for reduction actions
     */
    private Collection<Symbol> reductions;

    /**
     * Gets the terminals expected for shift actions
     *
     * @return The terminals expected for shift actions
     */
    public Collection<Symbol> getShifts() {
        return shifts;
    }

    /**
     * Gets the terminals expected for a reduction actions
     *
     * @return The terminals expected for reduction actions
     */
    public Collection<Symbol> getReductions() {
        return reductions;
    }

    /**
     * Initializes this container
     */
    public LRExpected() {
        this.shifts = new ArrayList<Symbol>();
        this.reductions = new ArrayList<Symbol>();
    }

    /**
     * Adds the specified terminal as expected on a shift action
     * If the terminal is terminal is already added to the reduction collection it is removed from it.
     *
     * @param terminal The terminal
     */
    public void addUniqueShift(Symbol terminal) {
        reductions.remove(terminal);
        if (!shifts.contains(terminal))
            shifts.add(terminal);
    }

    /**
     * Adds the specified terminal as expected on a reduction action
     * If the terminal is in the shift collection, nothing happens.
     *
     * @param terminal The terminal
     */
    public void addUniqueReduction(Symbol terminal) {
        if (!shifts.contains(terminal) && !reductions.contains(terminal))
            reductions.add(terminal);
    }
}
