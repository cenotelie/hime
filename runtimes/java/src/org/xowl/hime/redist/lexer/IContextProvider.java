/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters
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

package org.xowl.hime.redist.lexer;

/**
 * Represents an entity providing information about the current contexts
 *
 * @author Laurent Wouters
 */
public interface IContextProvider {
    /**
     * Gets the priority of the specified context required by the specified terminal
     * The priority is a positive integer. The lesser the value the higher the priority.
     * A negative value represents the unavailability of the required context.
     *
     * @param context      A context
     * @param onTerminalID The identifier of the terminal requiring the context
     * @return The context priority, or a negative value if the context is unavailable
     */
    int getContextPriority(int context, int onTerminalID);
}
