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

package org.xowl.hime.redist.lexer;

import java.util.Arrays;

/**
 * Represents a stack of lexing contexts
 * <p/>
 * A context stack contains blocks of various size containing the contexts, e.g.
 * [1 2 3] [4 5]
 * is a stack with two blocks.
 * The first block of size 3 contains the contexts 1, 2 and 3.
 * The second block of size 2 contains the contexts 4 and 5.
 */
public class ContextStack {
    /**
     * Initial size of the stack
     */
    private static final int INIT_STACK_SIZE = 1024;

    /**
     * The underlying items
     */
    private int[] stackContexts;
    /**
     * The underlying blocks in this stack
     */
    private int[] stackBlocks;
    /**
     * Index of the stack's top item
     */
    private int top;

    /**
     * Initializes this stack
     */
    public ContextStack() {
        this.stackContexts = new int[INIT_STACK_SIZE];
        this.stackBlocks = new int[INIT_STACK_SIZE];
        this.top = 0;
    }

    /**
     * Pushes a new block of contexts with the specified size onto this stack
     *
     * @param size The size of the block to push
     */
    public void pushBlock(int size) {
        if (top + size >= stackContexts.length) {
            stackContexts = Arrays.copyOf(stackContexts, stackContexts.length + INIT_STACK_SIZE);
            stackBlocks = Arrays.copyOf(stackBlocks, stackBlocks.length + INIT_STACK_SIZE);
        }
        top += size;
        stackBlocks[top] = size;
    }

    /**
     * Setups the value within the last block
     *
     * @param index   The index of the item to set within the current top block
     * @param context The value to set
     */
    public void setupBlockItem(int index, int context) {
        stackContexts[top - stackBlocks[top] + 1 + index] = context;
    }

    /**
     * Pushes a new empty block onto this stack
     */
    public void pushEmptyBlock() {
        if (top + 1 >= stackContexts.length) {
            stackContexts = Arrays.copyOf(stackContexts, stackContexts.length + INIT_STACK_SIZE);
            stackBlocks = Arrays.copyOf(stackBlocks, stackBlocks.length + INIT_STACK_SIZE);
        }
        top++;
        stackBlocks[top] = 1;
        stackContexts[top] = Automaton.DEFAULT_CONTEXT;
    }

    /**
     * Pops the specified number of blocks from this stack
     *
     * @param count The number of blocks to pop
     */
    public void popBlocks(int count) {
        while (count > 0) {
            top -= stackBlocks[top];
            count--;
        }
    }

    /**
     * Determines whether the specified context is in the stack
     *
     * @param context A context
     * @return <code>true</code> if the specified context is in the stack
     */
    public boolean contains(int context) {
        // the default context is always the bottom element of the stack and therefore is always in the stack
        if (context == Automaton.DEFAULT_CONTEXT)
            return true;
        for (int i = top; i != -1; i--) {
            if (stackContexts[i] == context)
                return true;
        }
        return false;
    }
}
