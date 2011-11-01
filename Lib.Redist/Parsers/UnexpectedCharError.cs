/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an unexpected character error in the input stream of a lexer
    /// </summary>
    internal class UnexpectedCharError : ParserError
    {
        /// <summary>
        /// Initializes a new instance of the LexerTextErrorDiscardedChar class for the given character at the given line and column number
        /// </summary>
        /// <param name="discarded">The errorneous character</param>
        /// <param name="line">The line number of the character</param>
        /// <param name="column">The column number of the character</param>
        public UnexpectedCharError(char discarded, int line, int column) : base(line, column)
        {
			// TODO: discarded should really be an encapsulated character with a little more semantics and a ToString method
            this.Message += "character '" + discarded.ToString() + "' (0x" + Convert.ToInt32(discarded).ToString("X") + ")";
        }
    }
}