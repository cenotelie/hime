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

using Hime.Redist.Utils;

namespace Hime.Redist
{
	/// <summary>
	/// Represents the output of a parser
	/// </summary>
	public class ParseResult
	{
		private ROList<ParseError> errors;
		private Text text;
		private AST ast;

		/// <summary>
		/// Initializes this result as a failure
		/// </summary>
		/// <param name="errors">The list of errors</param>
		/// <param name="text">The parsed text</param>
		internal ParseResult(ROList<ParseError> errors, Text text)
		{
			this.errors = errors;
			this.text = text;
		}

		/// <summary>
		/// Initializes this result as a success with the given AST
		/// </summary>
		/// <param name="errors">The list of errors</param>
		/// <param name="text">The parsed text</param>
		/// <param name="ast">The produced AST</param>
		internal ParseResult(ROList<ParseError> errors, Text text, AST ast)
		{
			this.errors = errors;
			this.text = text;
			this.ast = ast;
		}

		/// <summary>
		/// Gets whether the parser was successful
		/// </summary>
		public bool IsSuccess { get { return (ast != null); } }

		/// <summary>
		/// Gets a list of the parsing errors
		/// </summary>
		public ROList<ParseError> Errors { get { return errors; } }

		/// <summary>
		/// Gets the text that has been parsed
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

		/// <summary>
		/// Gets the token (if any) that contains the specified index in the input text
		/// </summary>
		/// <param name="index">An index within the input text</param>
		/// <returns>The token, if any</returns>
		public Token? FindTokenAt(int index)
		{
			if (ast == null)
				return null;
			return ast.FindTokenAt(index);
		}

		/// <summary>
		/// Gets the token (if any) that contains the specified position in the input text
		/// </summary>
		/// <param name="position">A position within the input text</param>
		/// <returns>The token, if any</returns>
		public Token? FindTokenAt(TextPosition position)
		{
			if (ast == null)
				return null;
			int index = text.GetLineIndex(position.Line) + position.Column - 1;
			return ast.FindTokenAt(index);
		}

		/// <summary>
		/// Gets the AST node (if any) that has the specified token as label
		/// </summary>
		/// <param name="token">The token to look for</param>
		/// <returns>The AST node, if any</returns>
		public ASTNode? FindNodeFor(Token token)
		{
			if (ast == null)
				return null;
			return ast.FindNodeFor(token);
		}

		/// <summary>
		/// Gets the AST node (if any) that has a token label that contains the specified index in the input text
		/// </summary>
		/// <param name="index">An index within the input text</param>
		/// <returns>The AST node, if any</returns>
		public ASTNode? FindNodeAt(int index)
		{
			if (ast == null)
				return null;
			Token? token = FindTokenAt(index);
			return token != null ? ast.FindNodeFor(token.Value) : null;
		}

		/// <summary>
		/// Gets the AST node (if any) that has a token label that contains the specified position in the input text
		/// </summary>
		/// <param name="position">A position within the input text</param>
		/// <returns>The AST node, if any</returns>
		public ASTNode? FindNodeAt(TextPosition position)
		{
			if (ast == null)
				return null;
			Token? token = FindTokenAt(position);
			return token != null ? ast.FindNodeFor(token.Value) : null;
		}
	}
}