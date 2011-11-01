/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an unexpected token error in a parser
    /// </summary>
    public class UnexpectedTokenError : ParserError
    {
		/// <summary>
		/// Initializes a new instance of the ParserErrorUnexpectedToken class with a token and an array of expected names.
		/// </summary>
		/// <param name='token'>
		/// The unexpected token.
		/// </param>
		/// <param name='expected'>
		/// The array of expected tokens' names.
		/// </param>
		/// <param name='line'>
		/// Line.
		/// </param>
		/// <param name='column'>
		/// Column.
		/// </param>
        internal UnexpectedTokenError(SymbolToken token, string[] expected, int line, int column) : base(line, column)
        {
            StringBuilder Builder = new StringBuilder();
			Builder.Append("token \"");
            Builder.Append(token.Value.ToString());
            Builder.Append("\"; expected: { ");
            for (int i = 0; i != expected.Length; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(expected[i]);
            }
            Builder.Append(" }");
            this.Message += Builder.ToString();
        }
    }
}