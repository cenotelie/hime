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
using Hime.Redist.Lexer;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents the stack of LR(k) parser
	/// </summary>
	class LRkStack : IContextProvider
	{
		/// <summary>
		/// The initial size of the stack
		/// </summary>
		private const int INIT_STACK_SIZE = 1024;

		/// <summary>
		/// The blocks in this stack
		/// </summary>
		private LRkStackBlock[] blocks;
		/// <summary>
		/// The lexical contexts in this stack
		/// </summary>
		private ushort[] contexts;
		/// <summary>
		/// The index of the top block
		/// </summary>
		private int headBlock;
		/// <summary>
		/// The index of the top context
		/// </summary>
		private int headContext;

		/// <summary>
		/// Gets the LR(k) state on this stack's head
		/// </summary>
		public int HeadState { get { return blocks[headBlock].State; } }

		/// <summary>
		/// Initializes this stack
		/// </summary>
		public LRkStack()
		{
			this.blocks = new LRkStackBlock[INIT_STACK_SIZE];
			this.contexts = new ushort[INIT_STACK_SIZE];
			this.headBlock = -1;
			this.headContext = -1;
		}

		/// <summary>
		/// Gets whether the specified context is in effect
		/// </summary>
		/// <param name="context">A context</param>
		/// <returns><c>true</c> if the specified context is in effect</returns>
		public bool IsWithin(int context)
		{
			if (context == Lexer.Automaton.DEFAULT_CONTEXT)
				return true;
			for (int i = headContext; i != -1; i--)
				if (contexts[i] == context)
					return true;
			return false;
		}

		/// <summary>
		/// Pushes a new block onto this stack
		/// </summary>
		/// <param name="state">The LR(k) state associated to the new block</param>
		/// <param name="size">The block's size</param>
		public void PushBlock(int state, int size)
		{
			if (headBlock + 1 >= blocks.Length)
			{
				LRkStackBlock[] temp = new LRkStackBlock[blocks.Length + INIT_STACK_SIZE];
				Array.Copy(blocks, temp, blocks.Length);
				blocks = temp;
			}
			headBlock++;
			blocks[headBlock].Setup((ushort)state, (ushort)size);

			if (headContext + size >= contexts.Length)
			{
				ushort[] temp = new ushort[contexts.Length + INIT_STACK_SIZE];
				Array.Copy(contexts, temp, contexts.Length);
				contexts = temp;
			}
			headContext += size;
		}

		/// <summary>
		/// Sets a context information in the top block of this stack
		/// </summary>
		/// <param name="index">The index within the top block</param>
		/// <param name="context">The context to set</param>
		public void SetContext(int index, int context)
		{
			contexts[headContext - blocks[headBlock].Size + 1 + index] = (ushort)context;
		}

		/// <summary>
		/// Pops the specified amount of blocks from this stack
		/// </summary>
		/// <param name="count">The number of blocks to pop</param>
		public void PopBlocks(int count)
		{
			while (count > 0)
			{
				headContext -= blocks[headBlock].Size;
				headBlock--;
				count--;
			}
		}
	}
}
