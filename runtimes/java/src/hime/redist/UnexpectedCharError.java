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

public class UnexpectedCharError extends ParseError {
    private String unexpected;

    public String getUnexpected() { return unexpected; }

    public UnexpectedCharError(String unexpected, TextPosition position) {
        super(ParseErrorType.UnexpectedChar, position);
        StringBuilder builder = new StringBuilder("Unexpected character '");
        builder.append(unexpected);
        builder.append("' (U+");
        if (unexpected.length() == 1)
        {
            builder.append(Integer.toHexString(unexpected.charAt(0)));
        }
        else
        {
            int lead = unexpected.charAt(0);
            int trail = unexpected.charAt(1);
            int cp = ((trail - 0xDC00) | ((lead - 0xD800) << 10)) + 0x10000;
            builder.append(Integer.toHexString(cp));
        }
        builder.append(")");
        this.message += builder.toString();
    }
}
