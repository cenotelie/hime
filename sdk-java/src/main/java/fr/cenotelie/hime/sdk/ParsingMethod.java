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

/**
 * Represents a parsing method
 *
 * @author Laurent Wouters
 */
public enum ParsingMethod {
    /**
     * The LR(0) parsing method
     */
    LR0,
    /**
     * The LR(1) parsing method
     */
    LR1,
    /**
     * The LALR(1) parsing method
     */
    LALR1,
    /**
     * The RNGLR parsing method based on a LR(1) graph
     */
    RNGLR1,
    /**
     * The RNGLR parsing method based on a LALR(1) graph
     */
    RNGLALR1
}
