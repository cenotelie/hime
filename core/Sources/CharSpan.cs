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

namespace Hime.SDK
{
	/// <summary>
	/// Represents a range of characters
	/// </summary>
	public struct CharSpan
	{
		/// <summary>
		/// Beginning of the range (included)
		/// </summary>
		private readonly char spanBegin;

		/// <summary>
		/// End of the range (included)
		/// </summary>
		private readonly char spanEnd;

		/// <summary>
		/// Constant value for an invalid value
		/// </summary>
		public static readonly CharSpan NULL = new CharSpan((char)1, (char)0);

		/// <summary>
		/// Gets the first (included) character of the range
		/// </summary>
		public char Begin { get { return spanBegin; } }

		/// <summary>
		/// Gets the last (included) character of the range
		/// </summary>
		public char End { get { return spanEnd; } }

		/// <summary>
		/// Gets the range's length in number of characters
		/// </summary>
		public int Length { get { return spanEnd - spanBegin + 1; } }

		/// <summary>
		/// Initializes this character span
		/// </summary>
		/// <param name="begin">The first (included) character</param>
		/// <param name="end">The last (included) character</param>
		public CharSpan(char begin, char end)
		{
			spanBegin = begin;
			spanEnd = end;
		}

		/// <summary>
		/// Gets the intersection between two spans
		/// </summary>
		/// <param name="left">The left span</param>
		/// <param name="right">The right span</param>
		/// <returns>The intersection</returns>
		public static CharSpan Intersect(CharSpan left, CharSpan right)
		{
			if (left.spanBegin < right.spanBegin)
			{
				if (left.spanEnd < right.spanBegin)
					return NULL;
				if (left.spanEnd < right.spanEnd)
					return new CharSpan(right.spanBegin, left.spanEnd);
				return new CharSpan(right.spanBegin, right.spanEnd);
			}
			else
			{
				if (right.spanEnd < left.spanBegin)
					return NULL;
				if (right.spanEnd < left.spanEnd)
					return new CharSpan(left.spanBegin, right.spanEnd);
				return new CharSpan(left.spanBegin, left.spanEnd);
			}
		}

		/// <summary>
		/// Splits the original span with the given splitter
		/// </summary>
		/// <param name="original">The span to be split</param>
		/// <param name="splitter">The splitter</param>
		/// <param name="rest">The second part of the resulting split</param>
		/// <returns>The first part of the resulting split</returns>
		public static CharSpan Split(CharSpan original, CharSpan splitter, out CharSpan rest)
		{
			if (original.spanBegin == splitter.spanBegin)
			{
				rest = NULL;
				if (original.spanEnd == splitter.spanEnd)
					return NULL;
				return new CharSpan(System.Convert.ToChar(splitter.spanEnd + 1), original.spanEnd);
			}
			if (original.spanEnd == splitter.spanEnd)
			{
				rest = NULL;
				return new CharSpan(original.spanBegin, System.Convert.ToChar(splitter.spanBegin - 1));
			}
			rest = new CharSpan(System.Convert.ToChar(splitter.spanEnd + 1), original.spanEnd);
			return new CharSpan(original.spanBegin, System.Convert.ToChar(splitter.spanBegin - 1));
		}

		/// <summary>
		/// Compares the left and right spans for an increasing order sort
		/// </summary>
		/// <param name="left">The left span</param>
		/// <param name="right">The right span</param>
		/// <returns>The order between left and right</returns>
		public static int Compare(CharSpan left, CharSpan right)
		{
			return left.spanBegin.CompareTo(right.spanBegin);
		}

		/// <summary>
		/// Compares the left and right spans for a decreasing order sort
		/// </summary>
		/// <param name="left">The left span</param>
		/// <param name="right">The right span</param>
		/// <returns>The order between left and right</returns>
		public static int CompareReverse(CharSpan left, CharSpan right)
		{
			return right.spanBegin.CompareTo(left.spanBegin);
		}

		/// <summary>
		/// Gets the string representation of this span
		/// </summary>
		/// <returns>The string representation</returns>
		public override string ToString()
		{
			if (spanBegin > spanEnd)
				return string.Empty;
			if (spanBegin == spanEnd)
				return CharToString(spanBegin);
			return "[" + CharToString(spanBegin) + "-" + CharToString(spanEnd) + "]";
		}

		/// <summary>
		/// Gets a user-friendly representation of the character
		/// </summary>
		/// <param name="c">A character</param>
		/// <returns>The string representation</returns>
		private static string CharToString(char c)
		{
			System.Globalization.UnicodeCategory cat = char.GetUnicodeCategory(c);
			switch (cat)
			{
				case System.Globalization.UnicodeCategory.ModifierLetter:
				case System.Globalization.UnicodeCategory.NonSpacingMark:
				case System.Globalization.UnicodeCategory.SpacingCombiningMark:
				case System.Globalization.UnicodeCategory.EnclosingMark:
				case System.Globalization.UnicodeCategory.SpaceSeparator:
				case System.Globalization.UnicodeCategory.LineSeparator:
				case System.Globalization.UnicodeCategory.ParagraphSeparator:
				case System.Globalization.UnicodeCategory.Control:
				case System.Globalization.UnicodeCategory.Format:
				case System.Globalization.UnicodeCategory.Surrogate:
				case System.Globalization.UnicodeCategory.PrivateUse:
				case System.Globalization.UnicodeCategory.OtherNotAssigned:
					return CharToString_NonPrintable(c);
				default:
					return c.ToString();
			}
		}

		/// <summary>
		/// Gets the string representation for the given non-printable character
		/// </summary>
		/// <param name="c">A non-printable character</param>
		/// <returns>The string representation</returns>
		private static string CharToString_NonPrintable(char c)
		{
			string result = "U+" + System.Convert.ToUInt16(c).ToString("X");
			return result;
		}

		/// <summary>
		/// Determines whether the given obj is equal to this span
		/// </summary>
		/// <param name="obj">The object to compare</param>
		/// <returns>True if obj is equal to this span</returns>
		public override bool Equals(object obj)
		{
			if (obj is CharSpan)
			{
				CharSpan Span = (CharSpan)obj;
				return ((spanBegin == Span.spanBegin) && (spanEnd == Span.spanEnd));
			}
			return false;
		}

		/// <summary>
		/// Gets the hash-code for this span
		/// </summary>
		/// <returns>The span's hash-code</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}