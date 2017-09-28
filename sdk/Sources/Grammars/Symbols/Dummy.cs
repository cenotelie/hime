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
	/// Represents a fake terminal, used as a marker by LR-related algorithms
	/// </summary>
	public class Dummy : Terminal
	{
		/// <summary>
		/// The singleton instance
		/// </summary>
		private static Dummy instance;

		/// <summary>
		/// Initializes the singleton
		/// </summary>
		private Dummy() : base(0, "#", "#", null, 0)
		{
		}

		/// <summary>
		/// Gets the the singleton instance
		/// </summary>
		public static Dummy Instance
		{
			get
			{
				if (instance == null)
					instance = new Dummy();
				return instance;
			}
		}
	}
}