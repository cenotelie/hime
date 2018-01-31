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

package fr.cenotelie.hime.sdk.automata;

import fr.cenotelie.hime.sdk.CharSpan;

import java.util.*;

/**
 * Represents a state in a Deterministic Finite Automaton
 *
 * @author Laurent Wouters
 */
public class DFAState {
    /**
     * The state's identifier
     */
    private final int id;
    /**
     * The transitions from this state
     */
    private final Map<CharSpan, DFAState> transitions;

    /**
     * Initializes this state
     *
     * @param id The state's identifier
     */
    public DFAState(int id) {
        this.id = id;
        this.transitions = new HashMap<>();
    }

    /**
     * Gets the state's identifier
     *
     * @return The state's identifier
     */
    public int getId() {
        return id;
    }

    /**
     * Gets the transitions from this state
     *
     * @return The transitions from this state
     */
    public Collection<CharSpan> getTransitions() {
        return transitions.keySet();
    }

    /**
     * Gets the children of this state, i.e. states that are reached by a single transition from this one
     *
     * @return The children of this state
     */
    public Collection<DFAState> getChildren() {
        return transitions.values();
    }

    /**
     * Gets the child state by the specified transition
     *
     * @param value The value on the transition
     * @return The child state
     */
    public DFAState getChildBy(CharSpan value) {
        return transitions.get(value);
    }

    /**
     * Determines whether this state has the specified transition
     *
     * @param value The value on the transition
     * @return Whether this state has the specified transition
     */
    public boolean hasTransition(CharSpan value) {
        return transitions.containsKey(value);
    }

    /**
     * Adds a transition from this state
     *
     * @param value The value on the transition
     * @param next  The next state by the transition
     */
    public void addTransition(CharSpan value, DFAState next) {
        transitions.put(value, next);
    }

    /**
     * Removes a transition from this state
     *
     * @param value The value on the transition
     */
    public void removeTransition(CharSpan value) {
        transitions.remove(value);
    }

    /**
     * Removes all the transitions from this state
     */
    public void clearTransitions() {
        transitions.clear();
    }

    /**
     * Repacks all the transitions from this state to remove overlaps between the transitions' values
     */
    public void repackTransitions() {
        Map<DFAState, List<CharSpan>> inverse = new HashMap<>();
        for (Map.Entry<CharSpan, DFAState> entry : transitions.entrySet()) {
            inverse.computeIfAbsent(entry.getValue(), k -> new ArrayList<>()).add(entry.getKey());
        }
        this.transitions.clear();
        for (Map.Entry<DFAState, List<CharSpan>> entry : inverse.entrySet()) {
            entry.getValue().sort(CharSpan::compareTo);
            for (int i = 0; i != entry.getValue().size(); i++) {
                CharSpan k1 = entry.getValue().get(i);
                for (int j = i + 1; j != entry.getValue().size(); j++) {
                    CharSpan k2 = entry.getValue().get(j);
                    if (k2.getSpanBegin() == k1.getSpanEnd() + 1) {
                        k1 = new CharSpan(k1.getSpanBegin(), k2.getSpanEnd());
                        entry.getValue().set(i, k1);
                        entry.getValue().remove(j);
                        j--;
                    }
                }
            }
            for (CharSpan key : entry.getValue())
                transitions.put(key, entry.getKey());
        }
    }
}
