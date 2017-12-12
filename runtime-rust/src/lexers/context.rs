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

//! Module for the management of lexers' context

use std::usize;

/// Identifier of the default context
pub const DEFAULT_CONTEXT: u16 = 0;

/// Defines the context provider as a function that gets
/// the priority of the specified context required by the specified terminal.
/// The priority is an unsigned integer. The lesser the value the higher the priority.
/// The absence of value represents the unavailability of the required context.
pub type ContextProvider = fn(u16, u32) -> Option<usize>;

/// Gets the priority of the specified context required by the specified terminal
/// The priority is an unsigned integer. The lesser the value the higher the priority.
/// The absence of value represents the unavailability of the required context.
pub fn default_context_provider(context: u16, _terminal_id: u32) -> Option<usize> {
    if context == DEFAULT_CONTEXT { Some(usize::MAX) } else { Some(0) }
}