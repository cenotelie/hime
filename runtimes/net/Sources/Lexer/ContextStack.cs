/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
using System.Collections.Generic;

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents a stack of lexing contexts
	/// </summary>
	public class ContextStack
	{
		/// <summary>
		/// The underlying stack
		/// </summary>
		private int[] stack;
		/// <summary>
		/// Index of the stack's top item
		/// </summary>
		private int top;

		/// <summary>
		/// Initializes this stack
		/// </summary>
		/// <param name="size">The maximum size of this stack</param>
		public ContextStack(int size)
		{
			this.stack = new int[size];
			this.top = 0;
		}

		/// <summary>
		/// Pushes the specified context onto the stack
		/// </summary>
		/// <param name="context">The context to push</param>
		public void Push(int context)
		{
			stack[top++] = context;
		}

		/// <summary>
		/// Pops the top-most contexts from the stack
		/// </summary>
		/// <param name="count">The number of contexts to pop</param>
		public void Pop(int count)
		{
			top -= count;
		}

		/// <summary>
		/// Determines whether the specified context is in the stack
		/// </summary>
		/// <param name="context">A context</param>
		/// <returns><c>true</c> if the specified context is in the stack</returns>
		public bool Contains(int context)
		{
			// the default context (0) is always the bottom element of the stack and therefore is always in the stack
			if (context == 0)
				return true;
			for (int i = top; i != -1; i--)
			{
				if (stack[i] == context)
					return true;
			}
			return false;
		}
	}
}
