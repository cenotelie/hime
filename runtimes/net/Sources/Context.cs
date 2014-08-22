/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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
**********************************************************************/
using System;

namespace Hime.Redist
{
	/// <summary>
	/// Represents the context description of a position in a piece of text.
	/// A context two pieces of text, the line content and the pointer.
	/// For example, given the piece of text:
	/// "public Struct Context"
	/// A context pointing to the second word will look like:
	/// content = "public Struct Context"
	/// pointer = "       ^"
	/// </summary>
	public struct Context
	{
		/// <summary>
		/// The text content being represented
		/// </summary>
		private string content;
		/// <summary>
		/// The pointer textual representation
		/// </summary>
		private string pointer;

		/// <summary>
		/// Gets the text content being represented
		/// </summary>
		/// <value>The text content being represented</value>
		public string Content { get { return content; } }

		/// <summary>
		/// Gets the pointer textual representation
		/// </summary>
		/// <value>The pointer textual representation</value>
		public string Pointer { get { return pointer; } }

		/// <summary>
		/// Initializes this context
		/// </summary>
		/// <param name="content">The text being begin represented</param>
		/// <param name="pointer">The pointer textual representation</param>
		public Context(string content, string pointer)
		{
			this.content = content;
			this.pointer = pointer;
		}
	}
}