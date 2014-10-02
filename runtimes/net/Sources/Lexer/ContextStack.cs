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
	/// <remarks>
	/// A context stack contains blocks of various size containing the contexts, e.g.
	/// [1 2 3] [4 5]
	/// is a stack with two blocks.
	/// The first block of size 3 contains the contexts 1, 2 and 3.
	/// The second block of size 2 contains the contexts 4 and 5.
	/// </remarks>
	public class ContextStack
	{
		private struct StackItem
		{
			/// <summary>
			/// The context represented by this item
			/// </summary>
			public ushort context;
			/// <summary>
			/// The size of the block containing this item
			/// </summary>
			public ushort blockSize;
		}

		/// <summary>
		/// Initial size of the stack
		/// </summary>
		private const int INIT_STACK_SIZE = 1024;

		/// <summary>
		/// The underlying items
		/// </summary>
		private StackItem[] stack;
		/// <summary>
		/// Index of the stack's top item
		/// </summary>
		private int top;

		/// <summary>
		/// Initializes this stack as empty
		/// </summary>
		public ContextStack()
		{
			this.stack = new StackItem[INIT_STACK_SIZE];
			this.top = -1;
		}

		/// <summary>
		/// Pushes a new block of contexts with the specified size onto this stack
		/// </summary>
		/// <param name="size">The size of the block to push</param>
		public void PushBlock(int size)
		{
			if (top + size >= stack.Length)
			{
				StackItem[] temp = new StackItem[stack.Length + INIT_STACK_SIZE];
				System.Array.Copy(stack, temp, stack.Length);
				stack = temp;
			}
			top += size;
			stack[top].blockSize = (ushort)size;
		}

		/// <summary>
		/// Setups the value within the last block
		/// </summary>
		/// <param name="index">The index of the item to set within the current top block</param>
		/// <param name="context">The value to set</param>
		public void SetupBlockItem(int index, int context)
		{
			stack[top - stack[top].blockSize + 1 + index].context = (ushort)context;
		}

		/// <summary>
		/// Pushes a new empty block onto this stack
		/// </summary>
		public void PushEmptyBlock()
		{
			if (top + 1 >= stack.Length)
			{
				StackItem[] temp = new StackItem[stack.Length + INIT_STACK_SIZE];
				System.Array.Copy(stack, temp, stack.Length);
				stack = temp;
			}
			top++;
			stack[top].blockSize = 1;
			stack[top].context = Automaton.DEFAULT_CONTEXT;
		}

		/// <summary>
		/// Pops the specified number of blocks from this stack
		/// </summary>
		/// <param name="count">The number of blocks to pop</param>
		public void PopBlocks(int count)
		{
			while (count > 0)
			{
				top -= stack[top].blockSize;
				count--;
			}
		}

		/// <summary>
		/// Determines whether the specified context is in the stack
		/// </summary>
		/// <param name="context">A context</param>
		/// <returns><c>true</c> if the specified context is in the stack</returns>
		public bool Contains(int context)
		{
			// the default context is always the bottom element of the stack and therefore is always in the stack
			if (context == Automaton.DEFAULT_CONTEXT)
				return true;
			for (int i = top; i != -1; i--)
			{
				if (stack[i].context == context)
					return true;
			}
			return false;
		}
	}
}
