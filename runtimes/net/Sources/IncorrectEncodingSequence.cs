/**********************************************************************
* Copyright (c) 2015 Laurent Wouters and others
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
using System.Text;

namespace Hime.Redist
{
	/// <summary>
	/// Represents an incorrect encoding sequence error in the input of a lexer
	/// </summary>
	public class IncorrectEncodingSequence : ParseError
	{
		/// <summary>
		/// The incorrect sequence
		/// </summary>
		private readonly char[] sequence;
		/// <summary>
		/// The precise error type
		/// </summary>
		private readonly ParseErrorType type;

		/// <summary>
		/// Gets the error's type
		/// </summary>
		public override ParseErrorType Type { get { return type; } }

		/// <summary>
		/// Gets the error's length in the input (in number of characters)
		/// </summary>
		public override int Length { get { return sequence.Length; } }

		/// <summary>
		/// Gets the error's message
		/// </summary>
		public override string Message { get { return BuildMessage(); } }

		/// <summary>
		/// Gets the incorrect sequence
		/// </summary>
		public char[] Sequence { get { return (char[]) sequence.Clone(); } }

		/// <summary>
		/// Initializes this error
		/// </summary>
		/// <param name="position">Error's position in the input</param>
		/// <param name="sequence">The incorrect sequence</param>
		/// <param name="errorType">The precise error type</param>
		public IncorrectEncodingSequence(TextPosition position, char[] sequence, ParseErrorType errorType)
			: base(position)
		{
			this.sequence = sequence;
			type = errorType;
		}

		/// <summary>
		/// Builds the message for this error
		/// </summary>
		/// <returns>The message for this error</returns>
		private string BuildMessage()
		{
			StringBuilder builder = new StringBuilder("Incorrect encoding sequence:");
			for (int i = 0; i != sequence.Length; i++)
			{
				builder.Append(" 0x");
				builder.Append(((int)sequence[i]).ToString("X4"));
			}
			return builder.ToString();
		}
	}
}