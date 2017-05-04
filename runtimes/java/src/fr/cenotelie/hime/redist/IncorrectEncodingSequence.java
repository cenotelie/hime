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

package fr.cenotelie.hime.redist;

/**
 * Represents an incorrect encoding sequence error in the input of a lexer
 *
 * @author Laurent Wouters
 */
public class IncorrectEncodingSequence extends ParseError {
    /**
     * The incorrect sequence
     */
    private final char sequence;
    /**
     * The precise error type
     */
    private final ParseErrorType type;

    @Override
    public ParseErrorType getType() {
        return type;
    }

    @Override
    public int getLength() {
        return 1;
    }

    @Override
    public String getMessage() {
        return buildMessage();
    }

    /**
     * Initializes this error
     *
     * @param position  Error's position in the input
     * @param sequence  The incorrect sequence
     * @param errorType The precise error type
     */
    public IncorrectEncodingSequence(TextPosition position, char sequence, ParseErrorType errorType) {
        super(position);
        this.sequence = sequence;
        this.type = errorType;
    }

    /**
     * Builds the message for this error
     *
     * @return The message for this error
     */
    private String buildMessage() {
        StringBuilder builder = new StringBuilder("Incorrect encoding sequence: [");
        switch (type) {
            case IncorrectUTF16NoHighSurrogate:
                builder.append("<missing> ");
                builder.append("0x");
                builder.append(Integer.toHexString((int) sequence));
                break;
            case IncorrectUTF16NoLowSurrogate:
                builder.append("0x");
                builder.append(Integer.toHexString((int) sequence));
                builder.append(" <missing>");
                break;
            default:
                break;
        }
        builder.append("]");
        return builder.toString();
    }
}
