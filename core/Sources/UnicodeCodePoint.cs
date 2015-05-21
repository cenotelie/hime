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
using System;

namespace Hime.SDK
{
	/// <summary>
	/// Represents a Unicode code point
	/// </summary>
	public struct UnicodeCodePoint : IComparable<UnicodeCodePoint>
	{
		/// <summary>
		/// The code point value
		/// </summary>
		private readonly int value;

		/// <summary>
		/// Gets the code point value
		/// </summary>
		public int Value { get { return value; } }

		/// <summary>
		/// Gets a value indicating whether this codepoint is in Unicode plane 0
		/// </summary>
		public bool IsPlane0 { get { return (value <= 0xFFFF); } }

		/// <summary>
		/// Initializes the code point
		/// </summary>
		/// <param name="value">The code point value</param>
		/// <remarks>
		/// The valid Unicode character code points are in the follwing intervals:
		/// * U+0000 .. U+D7FF
		/// * U+E000 .. U+FFFF
		/// * U+10000 .. U+10FFFF
		/// </remarks>
		public UnicodeCodePoint(int value)
		{
			if (value < 0 || (value >= 0xD800 && value <= 0xDFFF) || value >= 0x110000)
				throw new ArgumentException("The value is not a valid Unicode character code point", "value");
			this.value = value;
		}

		/// <summary>
		/// Gets the UTF-16 encoding of this code point
		/// </summary>
		/// <returns>The UTF-16 encoding of this code point</returns>
		/// <remarks>No check is done in this method because the the value is assumed valid after construction</remarks>
		public char[] GetUTF16()
		{
			if (value <= 0xFFFF)
			{
				// plane 0
				return new [] { (char)value };
			}
			uint temp = (uint)value - 0x10000;
			uint lead = (temp >> 10) + 0xD800;
			uint trail = (temp & 0x03FF) + 0xDC00;
			return new [] { (char)lead, (char)trail };
		}

		/// <summary>
		/// Tests the equality of two Unicode code points
		/// </summary>
		/// <param name="cp1">A Unicode code point</param>
		/// <param name="cp2">A Unicode code point</param>
		/// <returns><c>true</c> if the two codepoints are equal</returns>
		public static bool operator==(UnicodeCodePoint cp1, UnicodeCodePoint cp2)
		{
			return (cp1.value == cp2.value);
		}

		/// <summary>
		/// Tests the difference of two Unicode code points
		/// </summary>
		/// <param name="cp1">A Unicode code point</param>
		/// <param name="cp2">A Unicode code point</param>
		/// <returns><c>true</c> if the two codepoints are different</returns>
		public static bool operator!=(UnicodeCodePoint cp1, UnicodeCodePoint cp2)
		{
			return (cp1.value != cp2.value);
		}

		/// <summary>
		/// Tests the order of two Unicode code points
		/// </summary>
		/// <param name="cp1">A Unicode code point</param>
		/// <param name="cp2">A Unicode code point</param>
		/// <returns><c>true</c> if codepoint 1 is strictly before codepoint 2</returns>
		public static bool operator<(UnicodeCodePoint cp1, UnicodeCodePoint cp2)
		{
			return (cp1.value < cp2.value);
		}

		/// <summary>
		/// Tests the order of two Unicode code points
		/// </summary>
		/// <param name="cp1">A Unicode code point</param>
		/// <param name="cp2">A Unicode code point</param>
		/// <returns><c>true</c> if codepoint 1 is strictly after codepoint 2</returns>
		public static bool operator>(UnicodeCodePoint cp1, UnicodeCodePoint cp2)
		{
			return (cp1.value > cp2.value);
		}

		/// <summary>
		/// Tests the order of two Unicode code points
		/// </summary>
		/// <param name="cp1">A Unicode code point</param>
		/// <param name="cp2">A Unicode code point</param>
		/// <returns><c>true</c> if codepoint 1 is before or equal to codepoint 2</returns>
		public static bool operator<=(UnicodeCodePoint cp1, UnicodeCodePoint cp2)
		{
			return (cp1.value <= cp2.value);
		}

		/// <summary>
		/// Tests the order of two Unicode code points
		/// </summary>
		/// <param name="cp1">A Unicode code point</param>
		/// <param name="cp2">A Unicode code point</param>
		/// <returns><c>true</c> if codepoint 1 is after or equal to codepoint 2</returns>
		public static bool operator>=(UnicodeCodePoint cp1, UnicodeCodePoint cp2)
		{
			return (cp1.value >= cp2.value);
		}

		/// <summary>
		/// Returns the sort order of the current instance compared to the specified object.
		/// </summary>
		/// <param name="other">The object to compare to</param>
		/// <returns>
		/// The sort order of the current instance compared to the specified object.
		/// </returns>
		public int CompareTo(UnicodeCodePoint other)
		{
			return value.CompareTo(other.value);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.SDK.UnicodeCodePoint"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Hime.SDK.UnicodeCodePoint"/>.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.SDK.UnicodeCodePoint"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is UnicodeCodePoint)
			{
				UnicodeCodePoint cp = (UnicodeCodePoint)obj;
				return (value == cp.value);
			}
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.SDK.UnicodeCodePoint"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return value;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.UnicodeCodePoint"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.UnicodeCodePoint"/>.
		/// </returns>
		public override string ToString()
		{
			// not in plane 0 => always give hexadecimal value
			if (value >= 0x10000)
				return "U+" + value.ToString("X");

			// in plane 0, give the character only is it is printable
			char c = (char)value;
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
				return "U+" + Convert.ToUInt16(c).ToString("X");
			default:
				return c.ToString();
			}
		}
	}
}