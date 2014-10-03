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

package org.xowl.hime.redist.parsers;

import org.xowl.hime.redist.lexer.Automaton;
import org.xowl.hime.redist.lexer.IContextProvider;

import java.util.Arrays;

/**
 * Represents the stack of LR(k) parser
 */
class LRkStack implements IContextProvider {
    /**
     * The initial size of the stack
     */
    private static final int INIT_STACK_SIZE = 1024;

    /**
     * Stack of LR states
     */
    private int[] stackStates;
    /**
     * Stack of blocks
     */
    private int[] stackSizes;
    /**
     * The lexical contexts in this stack
     */
    private int[] contexts;
    /**
     * The index of the top block
     */
    private int headBlock;
    /**
     * The index of the top context
     */
    private int headContext;

    /**
     * Gets the LR(k) state on this stack's head
     *
     * @return The LR(k) state on this stack's head
     */
    public int getHeadState() {
        return stackStates[headBlock];
    }

    /**
     * Initializes this stack
     */
    public LRkStack() {
        this.stackStates = new int[INIT_STACK_SIZE];
        this.stackSizes = new int[INIT_STACK_SIZE];
        this.contexts = new int[INIT_STACK_SIZE];
        this.headBlock = -1;
        this.headContext = -1;
    }

    /**
     * Gets whether the specified context is in effect
     *
     * @param context A context
     * @return <code>true</code>  if the specified context is in effect
     */
    public boolean isWithin(int context) {
        if (context == Automaton.DEFAULT_CONTEXT)
            return true;
        for (int i = headContext; i != -1; i--) {
            if (contexts[i] == context)
                return true;
        }
        return false;
    }

    /**
     * Pushes a new block onto this stack
     *
     * @param state The LR(k) state associated to the new block
     * @param size  The block's size
     */
    public void pushBlock(int state, int size) {
        if (headBlock + 1 >= stackStates.length) {
            stackStates = Arrays.copyOf(stackStates, stackStates.length + INIT_STACK_SIZE);
            stackSizes = Arrays.copyOf(stackSizes, stackSizes.length + INIT_STACK_SIZE);
        }
        headBlock++;
        stackStates[headBlock] = state;
        stackSizes[headBlock] = size;

        if (headContext + size >= contexts.length) {
            contexts = Arrays.copyOf(contexts, contexts.length + INIT_STACK_SIZE);
        }
        headContext += size;
    }

    /**
     * Sets a context information in the top block of this stack
     *
     * @param index   The index within the top block
     * @param context The context to set
     */
    public void setContext(int index, int context) {
        contexts[headContext - stackSizes[headBlock] + 1 + index] = context;
    }

    /**
     * Pops the specified amount of blocks from this stack
     *
     * @param count The number of blocks to pop
     */
    public void popBlocks(int count) {
        while (count > 0) {
            headContext -= stackSizes[headBlock];
            headBlock--;
            count--;
        }
    }
}
