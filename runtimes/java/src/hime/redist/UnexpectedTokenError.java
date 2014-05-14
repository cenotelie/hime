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

package hime.redist;

import java.util.List;

public class UnexpectedTokenError extends ParseError {
    private Symbol unexpected;
    private List<Symbol> expected;

    public Symbol getUnexpectedToken() { return unexpected; }

    public List<Symbol> getExpectedTerminals() { return expected; }

    public UnexpectedTokenError(Symbol token, TextPosition position, List<Symbol> expected) {
        super(ParseErrorType.UnexpectedToken, position);
        this.unexpected = token;
        this.expected = expected;
        StringBuilder builder = new StringBuilder("Unexpected token \"");
        builder.append(token.getValue());
        builder.append("\"; expected {");
        for (int i=0; i!=expected.size(); i++) {
            if (i != 0)
                builder.append(", ");
            builder.append(expected.get(i).getName());
        }
        builder.append(" }");
        this.message += builder.toString();
    }
}
