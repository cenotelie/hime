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

using System.Collections.Generic;

namespace Hime.Redist.Symbols
{
    /// <summary>
    /// Represents a piece of text matched by a lexer
    /// </summary>
    public sealed class TextToken : Token
    {
        private string value;
        private int line;
        private int column;

        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override string Value { get { return value; } }
        /// <summary>
        /// Gets the line number where the text begins
        /// </summary>
        public int Line { get { return line; } }
        /// <summary>
        /// Gets the column number where the text begins
        /// </summary>
        public int Column { get { return column; } }

        /// <summary>
        /// Initializes a new instance of the TextToken class with the given name, id, value and line number
        /// </summary>
        /// <param name="sid">ID of the terminal matched by this Token</param>
        /// <param name="name">Name of the terminal matched by this Token</param>
        /// <param name="value">Token's value</param>
        /// <param name="line">Token's line number</param>
        /// <param name="column">Token's starting column in the line</param>
        public TextToken(int sid, string name, string value, int line, int column)
            : base(sid, name)
        {
            this.value = value;
            this.line = line;
            this.column = column;
        }
    }
}