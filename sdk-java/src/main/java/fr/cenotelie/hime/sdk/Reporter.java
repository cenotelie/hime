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

import fr.cenotelie.hime.redist.Text;
import fr.cenotelie.hime.redist.TextContext;
import fr.cenotelie.hime.redist.TextPosition;

/**
 * Represents a logger producing a compilation report
 *
 * @author Laurent Wouters
 */
public class Reporter {
    /**
     * The resulting report
     */
    private final Report report;

    /**
     * Gets the current report
     *
     * @return The current report
     */
    public Report getResult() {
        return report;
    }

    /**
     * Initializes the reporter
     */
    public Reporter() {
        this.report = new Report();
    }

    /**
     * Adds a new info entry to the log
     *
     * @param message The info message
     */
    public void info(Object message) {
        report.addInfo(message);
        System.out.println("[INFO] " + message.toString());
    }

    /**
     * Adds a new info entry to the log
     *
     * @param message  The info message
     * @param input    The input raising the message
     * @param position The position within the input
     */
    public void info(Object message, Text input, TextPosition position) {
        info(message);
        outputContext(input, position);
    }

    /**
     * Adds a new warning entry to the log
     *
     * @param message The warning message
     */
    public void warn(Object message) {
        report.addWarning(message);
        System.out.println("[WARNING] " + message.toString());
    }

    /**
     * Adds a new warning entry to the log
     *
     * @param message  The warning message
     * @param input    The input raising the message
     * @param position The position within the input
     */
    public void warn(Object message, Text input, TextPosition position) {
        warn(message);
        outputContext(input, position);
    }

    /**
     * Adds a new warning entry to the log
     *
     * @param message The warning message
     */
    public void error(Object message) {
        report.addError(message);
        System.out.println("[ERROR] " + message.toString());
    }

    /**
     * Adds a new error entry to the log
     *
     * @param message  The error message
     * @param input    The input raising the message
     * @param position The position within the input
     */
    public void error(Object message, Text input, TextPosition position) {
        error(message);
        outputContext(input, position);
    }

    /**
     * Outputs the context of a message in the console
     *
     * @param input    The input raising the message
     * @param position The position within the input
     */
    private void outputContext(Text input, TextPosition position) {
        TextContext context = input.getContext(position);
        System.out.print('\t');
        System.out.println(context.getContent());
        System.out.print('\t');
        System.out.println(context.getPointer());
    }
}
