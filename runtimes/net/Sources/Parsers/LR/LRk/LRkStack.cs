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
		/// The index of the top block
		/// </summary>
		private int headBlock;

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
			this.headBlock = -1;
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
			for (int i = headBlock; i != -1; i--)
				if (blocks[i].Contexts.Contains(context))
					return true;
			return false;
		}

		/// <summary>
		/// Pushes a new block onto this stack
		/// </summary>
		/// <param name="state">The LR(k) state associated to the new block</param>
		/// <param name="contexts">The contexts opened at this state</param>
		public void Push(int state, LRContexts contexts)
		{
			if (headBlock + 1 >= blocks.Length)
			{
				LRkStackBlock[] temp = new LRkStackBlock[blocks.Length + INIT_STACK_SIZE];
				Array.Copy(blocks, temp, blocks.Length);
				blocks = temp;
			}
			headBlock++;
			blocks[headBlock].Setup(state, contexts);
		}

		/// <summary>
		/// Pops the specified amount of blocks from this stack
		/// </summary>
		/// <param name="count">The number of blocks to pop</param>
		public void Pop(int count)
		{
			headBlock -= count;
		}
	}
}
