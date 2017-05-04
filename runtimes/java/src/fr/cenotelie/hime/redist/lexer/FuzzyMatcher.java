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

package fr.cenotelie.hime.redist.lexer;

import fr.cenotelie.hime.redist.IncorrectEncodingSequence;
import fr.cenotelie.hime.redist.ParseErrorType;
import fr.cenotelie.hime.redist.UnexpectedCharError;
import fr.cenotelie.hime.redist.UnexpectedEndOfInput;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * A fuzzy DFA matcher
 * This matcher uses the Levenshtein distance to match the input ahead against the current DFA automaton.
 * The matcher favors solutions that are the closest to the original input.
 * When multiple solutions are at the same Levenshtein distance to the input, the longest one is preferred.
 *
 * @author Laurent Wouters
 */
class FuzzyMatcher {
    /**
     * This lexer's automaton
     */
    private final Automaton automaton;
    /**
     * Terminal index of the SEPARATOR terminal
     */
    private final int separator;
    /**
     * The input text
     */
    private final BaseText text;
    /**
     * Delegate for raising errors
     */
    private LexicalErrorHandler errors;
    /**
     * The maximum Levenshtein distance between the input and the DFA
     */
    private final int maxDistance;
    /**
     * The index in the input from wich the error was raised
     */
    private final int originIndex;
    /**
     * The current heads
     */
    private List<int[]> heads;
    /**
     * Buffer of DFA states for the computation of character insertions
     */
    private int[] insertions;
    /**
     * The current number of insertions in the buffer
     */
    private int insertionsCount;
    /**
     * The current matching head, if any
     */
    private int[] matchHead;
    /**
     * The current matching length
     */
    private int matchLength;
    /**
     * Buffer for automaton state data
     */
    private AutomatonState stateData;

    /**
     * Initializes this matcher
     *
     * @param automaton   This lexer's automaton
     * @param separator   Terminal index of the SEPARATOR terminal
     * @param text        The input text
     * @param errors      Delegate for raising errors
     * @param maxDistance The maximum Levenshtein distance between the input and the DFA
     * @param index       The index in the input from wich the error was raised
     */
    public FuzzyMatcher(Automaton automaton, int separator, BaseText text, LexicalErrorHandler errors, int maxDistance, int index) {
        this.automaton = automaton;
        this.separator = separator;
        this.text = text;
        this.errors = errors;
        this.maxDistance = maxDistance;
        this.originIndex = index;
    }

    /**
     * Runs this matcher
     *
     * @return The solution
     */
    public TokenMatch run() {
        heads = new ArrayList<>();
        insertions = new int[16];
        insertionsCount = 0;
        matchHead = newHead(0);
        matchLength = 0;
        stateData = new AutomatonState();
        int offset = 0;
        boolean atEnd = text.isEnd(originIndex + offset);
        char current = atEnd ? '\0' : text.getValue(originIndex + offset);
        if (atEnd)
            inspectAtEnd(matchHead, offset);
        else
            inspect(matchHead, offset, current);
        while (!heads.isEmpty()) {
            offset++;
            atEnd = text.isEnd(originIndex + offset);
            current = atEnd ? '\0' : text.getValue(originIndex + offset);
            List<int[]> temp = new ArrayList<>(heads);
            heads.clear();
            for (int[] head : temp) {
                if (atEnd)
                    inspectAtEnd(head, offset);
                else
                    inspect(head, offset, current);
            }
        }
        return matchLength == 0 ? onFailure() : onSuccess();
    }

    /**
     * Constructs the solution when succeeded to fix the error
     *
     * @return The constructed solution
     */
    private TokenMatch onSuccess() {
        int lastErrorIndex = -1;
        for (int i = 0; i != getHeadDistance(matchHead); i++) {
            int errorIndex = originIndex + getHeadError(matchHead, i);
            if (errorIndex != lastErrorIndex)
                onError(errorIndex);
            lastErrorIndex = errorIndex;
        }
        return new TokenMatch(getHeadState(matchHead), matchLength);
    }

    /**
     * Reports on the lexical error at the specified index
     *
     * @param index The index in the input where the error occurs
     */
    private void onError(int index) {
        ParseErrorType errorType = ParseErrorType.UnexpectedChar;
        boolean atEnd = text.isEnd(index);
        String value = "";
        if (atEnd) {
            // the end of input was not expected
            // there is necessarily some input before because an empty input would have matched the $
            char c = text.getValue(index - 1);
            if (c >= 0xD800 && c <= 0xDBFF) {
                // a trailing UTF-16 high surrogate
                index--;
                errorType = ParseErrorType.IncorrectUTF16NoLowSurrogate;
            } else
                errorType = ParseErrorType.UnexpectedEndOfInput;
        } else {
            char c = text.getValue(index);
            if (c >= 0xD800 && c <= 0xDBFF && !text.isEnd(index + 1)) {
                // a UTF-16 high surrogate
                // if next next character is a low surrogate, also get it
                char c2 = text.getValue(index + 1);
                if (c2 >= 0xDC00 && c2 <= 0xDFFF)
                    value = new String(new char[]{c, c2});
                else
                    errorType = ParseErrorType.IncorrectUTF16NoLowSurrogate;
            } else if (c >= 0xDC00 && c <= 0xDFFF && index > 0) {
                // a UTF-16 low surrogate
                // if the previous character is a high surrogate, also get it
                char c2 = text.getValue(index - 1);
                if (c2 >= 0xD800 && c2 <= 0xDBFF) {
                    index--;
                    value = new String(new char[]{c, c2});
                } else
                    errorType = ParseErrorType.IncorrectUTF16NoHighSurrogate;
            }
            if (value.length() == 0)
                value = Character.toString(c);
        }
        switch (errorType) {
            case UnexpectedEndOfInput:
                errors.handle(new UnexpectedEndOfInput(text.getPositionAt(index)));
                break;
            case UnexpectedChar:
                errors.handle(new UnexpectedCharError(value, text.getPositionAt(index)));
                break;
            case IncorrectUTF16NoHighSurrogate:
            case IncorrectUTF16NoLowSurrogate:
                errors.handle(new IncorrectEncodingSequence(text.getPositionAt(index), text.getValue(index), errorType));
                break;
            default:
                break;
        }
    }

    /**
     * Constructs the solution when failed to fix the error
     *
     * @return The solution
     */
    private TokenMatch onFailure() {
        errors.handle(new UnexpectedCharError(Character.toString(text.getValue(originIndex)), text.getPositionAt(originIndex)));
        return new TokenMatch(1);
    }

    /**
     * Pushes a new head onto the the queue
     *
     * @param previous The previous head
     * @param state    The associated DFA state
     */
    private void pushHead(int[] previous, int state) {
        pushHead(previous, state, -1, getHeadDistance(previous));
    }

    /**
     * Pushes a new head onto the the queue
     *
     * @param previous The previous head
     * @param state    The associated DFA state
     * @param offset   The offset of the error from the original index
     */
    private void pushHead(int[] previous, int state, int offset) {
        pushHead(previous, state, offset, getHeadDistance(previous) + 1);
    }

    /**
     * Pushes a new head onto the the queue
     *
     * @param previous The previous head
     * @param state    The associated DFA state
     * @param offset   The offset of the error from the original index
     * @param distance The distance to reach
     */
    private void pushHead(int[] previous, int state, int offset, int distance) {
        for (int i = heads.size() - 1; i != -1; i--)
            if (getHeadState(heads.get(i)) == state && getHeadDistance(heads.get(i)) <= distance)
                return;
        if (offset == -1)
            heads.add(newHead(previous, state));
        else
            heads.add(newHead(previous, state, offset, distance));
    }

    /**
     * Inspects a head while at the end of the input
     *
     * @param head   The head to inspect
     * @param offset The current offset from the original index
     */
    private void inspectAtEnd(int[] head, int offset) {
        automaton.retrieveState(getHeadState(head), stateData);
        // is it a matching state
        if (stateData.getTerminalCount() != 0 && stateData.getTerminal() != separator)
            onMatchingHead(head, offset);
        if (getHeadDistance(head) >= maxDistance || stateData.isDeadEnd())
            // cannot stray further
            return;
        // lookup the transitions
        exploreTransitions(head, offset, true);
        exploreInsertions(head, offset, true, '\0');
    }


    /**
     * Inspects a head with a specified character ahead
     *
     * @param head    The head to inspect
     * @param offset  The current offset from the original index
     * @param current The leading character in the input
     */
    private void inspect(int[] head, int offset, char current) {
        automaton.retrieveState(getHeadState(head), stateData);
        // is it a matching state
        if (stateData.getTerminalCount() != 0 && stateData.getTerminal() != separator)
            onMatchingHead(head, offset);
        if (getHeadDistance(head) >= maxDistance || stateData.isDeadEnd())
            // cannot stray further
            return;
        // could be a straight match
        int target = stateData.getTargetBy(current);
        if (target != Automaton.DEAD_STATE)
            // it is!
            pushHead(head, target);
        // could try a drop
        pushHead(head, getHeadState(head), offset);
        // lookup the transitions
        exploreTransitions(head, offset, false);
        exploreInsertions(head, offset, false, current);
    }

    /**
     * Explores a state transition
     *
     * @param head   The head to inspect
     * @param offset The current offset from the original index
     * @param atEnd  Whether the current index is at the end of the input
     */
    private void exploreTransitions(int[] head, int offset, boolean atEnd) {
        for (int i = 0; i != 256; i++) {
            int target = stateData.getTargetBy(i);
            if (target == Automaton.DEAD_STATE)
                continue;
            exploreTransitionToTarget(head, target, offset, atEnd);
        }
        for (int i = 0; i != stateData.getBulkTransitionCount(); i++) {
            exploreTransitionToTarget(head, stateData.getBulkTransition(i), offset, atEnd);
        }
    }

    /**
     * Explores a state transition
     *
     * @param head   The head to inspect
     * @param target The target DFA state
     * @param offset The current offset from the original index
     * @param atEnd  Whether the current index is at the end of the input
     */
    private void exploreTransitionToTarget(int[] head, int target, int offset, boolean atEnd) {
        if (!atEnd) {
            // try replace
            pushHead(head, target, offset);
        }
        // try to insert
        boolean found = false;
        for (int i = insertionsCount - 1; i != -1; i--) {
            if (insertions[i] == target) {
                found = true;
                break;
            }
        }
        if (!found) {
            if (insertionsCount == insertions.length)
                insertions = Arrays.copyOf(insertions, insertions.length * 2);
            insertions[insertionsCount++] = target;
        }
    }

    /**
     * Explores the current insertions
     *
     * @param head    The head to inspect
     * @param offset  The current offset from the original index
     * @param atEnd   Whether the current index is at the end of the input
     * @param current The leading character in the input
     */
    private void exploreInsertions(int[] head, int offset, boolean atEnd, char current) {
        // setup the first round
        int distance = getHeadDistance(head) + 1;
        int end = insertionsCount;
        int start = 0;
        // while there are insertions to examine in a round
        while (start != insertionsCount) {
            for (int i = start; i != end; i++) {
                // examine insertion i
                exploreInsertion(head, offset, atEnd, current, insertions[i], distance);
            }
            // prepare next round
            distance++;
            start = end;
            end = insertionsCount;
        }
        // reset the insertions data
        insertionsCount = 0;
    }

    /**
     * Explores an insertion
     *
     * @param head     The head to inspect
     * @param offset   The current offset from the original index
     * @param atEnd    Whether the current index is at the end of the input
     * @param current  The leading character in the input
     * @param state    The DFA state for the insertion
     * @param distance The distance associated to this insertion
     */
    private void exploreInsertion(int[] head, int offset, boolean atEnd, char current, int state, int distance) {
        automaton.retrieveState(state, stateData);
        if (stateData.getTerminalCount() != 0 && stateData.getTerminal() != separator)
            onMatchingInsertion(head, offset, state, distance);
        if (!atEnd) {
            int target = stateData.getTargetBy(current);
            if (target != Automaton.DEAD_STATE)
                pushHead(head, target, offset, distance);
        }
        if (distance >= maxDistance)
            return;
        // continue insertion
        exploreTransitions(head, offset, true);
    }

    /**
     * When a matching head is encountered
     *
     * @param head   The matching head
     * @param offset The current offset from the original index
     */
    private void onMatchingHead(int[] head, int offset) {
        int clCurrent = getHeadComparableLegnth(matchHead, matchLength);
        int clCandidate = getHeadComparableLegnth(head, offset);
        if (matchLength == 0 || clCandidate > clCurrent) {
            matchHead = head;
            matchLength = offset;
        }
    }

    /**
     * When a matching insertion is encountered
     *
     * @param previous The previous head
     * @param offset   The current offset from the original index
     * @param target   The DFA state for the insertion
     * @param distance The distance associated to this insertion
     */
    private void onMatchingInsertion(int[] previous, int offset, int target, int distance) {
        int d = distance - getHeadDistance(previous);
        int clCurrent = getHeadComparableLegnth(matchHead, matchLength);
        int clCandidate = getHeadComparableLegnth(previous, offset - d);
        if (matchLength == 0 || clCandidate > clCurrent) {
            matchHead = newHead(previous, target, offset, distance);
            matchLength = offset;
        }
    }

    /**
     * Gets the associated DFA state
     *
     * @param data A DFA stack head
     * @return The associated DFA state
     */
    private static int getHeadState(int[] data) {
        return data[0];
    }

    /**
     * Gets the Levenshtein distance of this head form the input
     *
     * @param data A DFA stack head
     * @return The Levenshtein distance of this head form the input
     */
    private static int getHeadDistance(int[] data) {
        return data.length - 1;
    }

    /**
     * Gets the offset in the input of the i-th lexical error on this head
     *
     * @param data A DFA stack head
     * @param i    Index of the error
     * @return The offset of the i-th error in the input
     */
    private static int getHeadError(int[] data, int i) {
        return data[i + 1];
    }

    /**
     * Computes the comparable length of the specified match
     *
     * @param data   The matching head
     * @param length The matching length in the input
     * @return The comparable length
     */
    private static int getHeadComparableLegnth(int[] data, int length) {
        return length - getHeadDistance(data);
    }

    /**
     * Initializes a new head with a state and a 0 distance
     *
     * @param state The associated DFA state
     * @return The DFA stack head
     */
    public static int[] newHead(int state) {
        return new int[]{state};
    }

    /**
     * Initializes a new head from a previous one
     *
     * @param previous The previous head
     * @param state    The associated DFA state
     * @return The DFA stack head
     */
    public static int[] newHead(int[] previous, int state) {
        int[] result = Arrays.copyOf(previous, previous.length);
        result[0] = state;
        return result;
    }

    /**
     * Initializes a new head from a previous one
     *
     * @param previous The previous head
     * @param state    The associated DFA state
     * @param offset   The offset of the error from the original index
     * @param distance The distance to reach
     * @return The DFA stack head
     */
    public static int[] newHead(int[] previous, int state, int offset, int distance) {
        if (distance < previous.length)
            throw new IllegalArgumentException("The distance for the new head must be at least one more than the distance of the previous head");
        int[] result = Arrays.copyOf(previous, distance + 1);
        result[0] = state;
        for (int i = previous.length; i != result.length; i++)
            result[i] = offset;
        return result;
    }
}
