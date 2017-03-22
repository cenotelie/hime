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

namespace Hime.SDK.Automata
{
	/// <summary>
	/// Represents a fake marker of a final state in an automaton
	/// </summary>
	/// <remarks>This class is a singleton</remarks>
	public class DummyItem : FinalItem
	{
		/// <summary>
		/// The single instance
		/// </summary>
		private static DummyItem instance;

		private DummyItem()
		{
		}

		/// <summary>
		/// Gets the single instance
		/// </summary>
		public static FinalItem Instance
		{
			get
			{
				if (instance == null)
					instance = new DummyItem();
				return instance;
			}
		}

		/// <summary>
		///  Gets the priority of this marker 
		/// </summary>
		public int Priority { get { return -1; } }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Automata.DummyItem"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Automata.DummyItem"/>.
		/// </returns>
		public override string ToString()
		{
			return "#";
		}
	}
}
