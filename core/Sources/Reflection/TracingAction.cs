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
using System.Collections.Generic;
using Hime.Redist;

namespace Hime.SDK.Reflection
{
	/// <summary>
	/// Encapsulates a parser semantic action used for tracing purposes
	/// </summary>
	/// <typeparam name="T">The payload's type</typeparam>
	public class TracingAction<T>
	{
		/// <summary>
		/// The common trace
		/// </summary>
		private List<T> trace;
		/// <summary>
		/// The payload for this semantic action
		/// </summary>
		private T payload;

		/// <summary>
		/// Initializes this action
		/// </summary>
		/// <param name="trace">The common trace</param>
		/// <param name="payload">The payload to be inserted in the trace</param>
		public TracingAction(List<T> trace, T payload)
		{
			this.trace = trace;
			this.payload = payload;
		}

		/// <summary>
		/// The callback for the semantic action
		/// </summary>
		/// <param name="head">The semantic object for the head</param>
		/// <param name="body">The current body at the time of the action</param>
		public void Callback(Symbol head, SemanticBody body)
		{
			trace.Add(payload);
		}
	}
}