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
import java.util.Collections;
import java.util.List;

/**
 * Represents a compilation report
 *
 * @author Laurent Wouters
 */
public class Report {
    /**
     * The list of info messages in this report
     */
    private final List<Object> infos;
    /**
     * The list of warnings in this report
     */
    private final List<Object> warnings;
    /**
     * The list of errors in this report
     */
    private final List<Object> errors;

    /**
     * Gets the list of info messages in this report
     *
     * @return The list of info messages in this report
     */
    public List<Object> getInfos() {
        return Collections.unmodifiableList(infos);
    }

    /**
     * Gets the list of warnings in this report
     *
     * @return The list of warnings in this report
     */
    public List<Object> getWarnings() {
        return Collections.unmodifiableList(warnings);
    }

    /**
     * Gets the list of errors in this report
     *
     * @return The list of errors in this report
     */
    public List<Object> getErrors() {
        return Collections.unmodifiableList(errors);
    }

    /**
     * Initializes a new report
     */
    public Report() {
        this.infos = new ArrayList<>();
        this.warnings = new ArrayList<>();
        this.errors = new ArrayList<>();
    }

    /**
     * Adds a new info entry
     *
     * @param message The info message
     */
    public void addInfo(Object message) {
        infos.add(message);
    }

    /**
     * Adds a new warning entry
     *
     * @param message The warning message
     */
    public void addWarning(Object message) {
        infos.add(message);
    }

    /**
     * Adds a new error entry
     *
     * @param message The error message
     */
    public void addError(Object message) {
        infos.add(message);
    }
}
