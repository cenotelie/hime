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
using System.Text;

namespace Hime.Redist
{
	/// <summary>
	/// Represents an unexpected character error in the input stream of a lexer
	/// </summary>
	public class UnexpectedCharError : ParseError
	{
		/// <summary>
		/// The unexpected character
		/// </summary>
		private readonly string unexpected;

		/// <summary>
		/// Gets the error's type
		/// </summary>
		public override ParseErrorType Type { get { return ParseErrorType.UnexpectedChar; } }

		/// <summary>
		/// Gets the error's length in the input (in number of characters)
		/// </summary>
		public override int Length { get { return unexpected.Length; } }

		/// <summary>
		/// Gets the error's message
		/// </summary>
		public override string Message { get { return BuildMessage(); } }

		/// <summary>
		/// Gets the unexpected char
		/// </summary>
		public string UnexpectedChar { get { return unexpected; } }

		/// <summary>
		/// Initializes this error
		/// </summary>
		/// <param name="unexpected">The errorneous character (as a string)</param>
		/// <param name="position">Error's position in the input</param>
		public UnexpectedCharError(string unexpected, TextPosition position)
			: base(position)
		{
			this.unexpected = unexpected;
		}

		/// <summary>
		/// Builds the message for this error
		/// </summary>
		/// <returns>The message for this error</returns>
		private string BuildMessage()
		{
			StringBuilder builder = new StringBuilder("Unexpected character '");
			builder.Append(unexpected);
			builder.Append("' (U+");
			if (unexpected.Length == 1)
			{
				builder.Append(((int)unexpected[0]).ToString("X"));
			}
			else
			{
				uint lead = unexpected[0];
				uint trail = unexpected[1];
				uint cp = ((trail - 0xDC00) | ((lead - 0xD800) << 10)) + 0x10000;
				builder.Append(cp.ToString("X"));
			}
			builder.Append(")");
			return builder.ToString();
		}
	}
}