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

namespace Hime.CentralDogma.Grammars
{
	/// <summary>
	/// Represents the absence of terminal, used as a marker by LR-related algorithms
	/// </summary>
	public class NullTerminal : Terminal
	{
		/// <summary>
		/// The singleton instance
		/// </summary>
		private static NullTerminal instance;
		/// <summary>
		/// Initializes the singleton
		/// </summary>
		private NullTerminal() : base(0, string.Empty, string.Empty, null)
		{
		}

		/// <summary>
		/// Gets the the singleton instance
		/// </summary>
		public static NullTerminal Instance
		{
			get
			{
				if (instance == null)
					instance = new NullTerminal();
				return instance;
			}
		}
	}
}