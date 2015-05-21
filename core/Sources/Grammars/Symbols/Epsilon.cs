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

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents the epsilon symbol in a grammar, i.e. a terminal with an empty value
	/// </summary>
	public class Epsilon : Terminal
	{
		/// <summary>
		/// The singleton instance
		/// </summary>
		private static Epsilon instance;

		/// <summary>
		/// Initializes the singleton
		/// </summary>
		private Epsilon() : base(1, "ε", "ε", null, null)
		{
		}

		/// <summary>
		/// Gets the the singleton instance
		/// </summary>
		public static Epsilon Instance
		{
			get
			{
				if (instance == null)
					instance = new Epsilon();
				return instance;
			}
		}
	}
}