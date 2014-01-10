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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hime.Redist
{
    /// <summary>
    /// Represents the output of a parser
    /// </summary>
    public sealed class ParseResult
    {
        /// <summary>
        /// Represents the content of the text read by a lexer
        /// </summary>
        /// <remarks>
        /// All line numbers and column numbers are 1-based.
        /// Indices in the content are 0-based.
        /// </remarks>
        public interface Text
        {
            /// <summary>
            /// Gets the number of lines
            /// </summary>
            int LineCount { get; }

            /// <summary>
            /// Gets the size in number of characters
            /// </summary>
            int Size { get; }

            /// <summary>
            /// Gets the substring beginning at the given index with the given length
            /// </summary>
            /// <param name="index">Index of the substring from the start</param>
            /// <param name="length">Length of the substring</param>
            /// <returns>The substring</returns>
            string GetValue(int index, int length);

            /// <summary>
            /// Gets the starting index of the i-th line
            /// </summary>
            /// <param name="line">The line number</param>
            /// <returns>The starting index of the line</returns>
            /// <remarks>The line numbering is 1-based</remarks>
            int GetLineIndex(int line);

            /// <summary>
            /// Gets the length of the i-th line
            /// </summary>
            /// <param name="line">The line number</param>
            /// <returns>The length of the line</returns>
            /// <remarks>The line numbering is 1-based</remarks>
            int GetLineLength(int line);

            /// <summary>
            /// Gets the string content of the i-th line
            /// </summary>
            /// <param name="line">The line number</param>
            /// <returns>The string content of the line</returns>
            /// <remarks>The line numbering is 1-based</remarks>
            string GetLineContent(int line);

            /// <summary>
            /// Gets the line number at the given index
            /// </summary>
            /// <param name="index">Index from the start</param>
            /// <returns>The line number at the index</returns>
            int GetLineAt(int index);

            /// <summary>
            /// Gets the column number at the given index
            /// </summary>
            /// <param name="index">Index from the start</param>
            /// <returns>The column number at the index</returns>
            int GetColumnAt(int index);

            /// <summary>
            /// Gets the position at the given index
            /// </summary>
            /// <param name="index">Index from the start</param>
            /// <returns>The position (line and column) at the index</returns>
            TextPosition GetPositionAt(int index);
        }

        private IList<Error> errors;
        private Text text;
        private ParseTree ast;

        /// <summary>
        /// Initializes this result as a failure
        /// </summary>
        /// <param name="errors">The list of errors</param>
        /// <param name="text">The parsed text</param>
        internal ParseResult(List<Error> errors, Text text)
        {
            this.errors = new ReadOnlyCollection<Error>(errors);
            this.text = text;
        }

        /// <summary>
        /// Initializes this result as a success with the given AST
        /// </summary>
        /// <param name="errors">The list of errors</param>
        /// <param name="text">The parsed text</param>
        /// <param name="ast">The produced AST</param>
        internal ParseResult(List<Error> errors, Text text, ParseTree ast)
        {
            this.errors = new ReadOnlyCollection<Error>(errors);
            this.text = text;
            this.ast = ast;
        }

        /// <summary>
        /// Gets whether the parser was successful
        /// </summary>
        public bool IsSucess { get { return (ast != null); } }

        /// <summary>
        /// Gets a list of the parsing errors
        /// </summary>
        public IList<Error> Errors { get { return errors; } }

        /// <summary>
        /// Gets the text that has been parse
        /// </summary>
        public Text Input { get { return text; } }

        /// <summary>
        /// Gets the root of the produced parse tree
        /// </summary>
        public ASTNode Root
        {
            get
            {
                if (ast == null)
                    return new ASTNode();
                return ast.Root;
            }
        }
    }
}