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

using System.Collections.Generic;
using Hime.Redist.Utils;

namespace Hime.SDK
{
	/// <summary>
	/// Represents a Unicode category
	/// </summary>
	public class UnicodeCategory
	{
		/// <summary>
		/// The category's name
		/// </summary>
		private readonly string name;

		/// <summary>
		/// The list of character spans contained in this category
		/// </summary>
		private List<UnicodeSpan> spans;

		/// <summary>
		/// Gets this unicode category's name
		/// </summary>
		public string Name { get { return name; } }

		/// <summary>
		/// Gets the character spans contained by this category
		/// </summary>
		public ROList<UnicodeSpan> Spans { get { return new ROList<UnicodeSpan>(spans); } }

		/// <summary>
		/// Adds a span to this category
		/// </summary>
		/// <param name="begin">The span's beginning character</param>
		/// <param name="end">The span's ending character</param>
		public void AddSpan(int begin, int end)
		{
			spans.Add(new UnicodeSpan(begin, end));
		}

		/// <summary>
		/// Aggregate the specified category into this one
		/// </summary>
		/// <param name="category">The category to aggregate</param>
		public void Aggregate(UnicodeCategory category)
		{
			spans.AddRange(category.spans);
		}

		/// <summary>
		/// Initializes a new (empty) category
		/// </summary>
		/// <param name="name">The category's name</param>
		public UnicodeCategory(string name)
		{
			this.name = name;
			spans = new List<UnicodeSpan>();
		}

		/// <summary>
		/// Gets the string representation of this category
		/// </summary>
		/// <returns>The string representation</returns>
		public override string ToString()
		{
			return name;
		}
	}
}