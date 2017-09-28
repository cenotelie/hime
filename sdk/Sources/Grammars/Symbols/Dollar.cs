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

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents the dollar symbol in a grammar, i.e. the marker of end of input
	/// </summary>
	public class Dollar : Terminal
	{
		/// <summary>
		/// The singleton instance
		/// </summary>
		private static Dollar instance;

		/// <summary>
		/// Initializes the singleton
		/// </summary>
		private Dollar() : base(2, "$", "$", null, 0)
		{
		}

		/// <summary>
		/// Gets the the singleton instance
		/// </summary>
		public static Dollar Instance
		{
			get
			{
				if (instance == null)
					instance = new Dollar();
				return instance;
			}
		}
	}
}