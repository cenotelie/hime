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
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Container for the expected terminals for a LR state
	/// </summary>
	public sealed class LRExpected
	{
		/// <summary>
		/// The terminals expected for shift actions
		/// </summary>
		private readonly List<Symbol> shifts;
		/// <summary>
		/// The terminals expected for reduction actions
		/// </summary>
		private readonly List<Symbol> reductions;

		/// <summary>
		/// Gets the terminals expected for shift actions
		/// </summary>
		public List<Symbol> Shifts { get { return shifts; } }

		/// <summary>
		/// Gets the terminals expected for a reduction actions
		/// </summary>
		public List<Symbol> Reductions { get { return reductions; } }

		/// <summary>
		/// Initializes this container
		/// </summary>
		public LRExpected()
		{
			shifts = new List<Symbol>();
			reductions = new List<Symbol>();
		}

		/// <summary>
		/// Adds the specified terminal as expected on a shift action
		/// </summary>
		/// <param name="terminal">The terminal</param>
		/// <remarks>
		/// If the terminal is terminal is already added to the reduction collection it is removed from it.
		/// </remarks>
		public void AddUniqueShift(Symbol terminal)
		{
			reductions.Remove(terminal);
			if (!shifts.Contains(terminal))
				shifts.Add(terminal);
		}

		/// <summary>
		/// Adds the specified terminal as expected on a reduction action
		/// </summary>
		/// <param name="terminal">The terminal</param>
		/// <remarks>
		/// If the terminal is in the shift collection, nothing happens.
		/// </remarks>
		public void AddUniqueReduction(Symbol terminal)
		{
			if (!shifts.Contains(terminal) && !reductions.Contains(terminal))
				reductions.Add(terminal);
		}
	}
}
