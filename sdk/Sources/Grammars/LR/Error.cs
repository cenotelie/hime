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

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents an error in a LR parser
	/// </summary>
	public class Error
	{
		/// <summary>
		/// The state raising this conflict
		/// </summary>
		protected readonly State errorState;
		/// <summary>
		/// The conflict's type
		/// </summary>
		protected readonly ErrorType type;
		/// <summary>
		/// The set of related items
		/// </summary>
		protected readonly List<Item> errorItems;
		/// <summary>
		/// The example of conflictuous input
		/// </summary>
		protected readonly List<Phrase> inputExamples;

		/// <summary>
		/// Gets the state raising this conflict
		/// </summary>
		public State State { get { return errorState; } }

		/// <summary>
		/// Gets the type of the error
		/// </summary>
		public ErrorType ErrorType { get { return type; } }

		/// <summary>
		/// Gets the list of related items
		/// </summary>
		public ROList<Item> Items { get { return new ROList<Item>(errorItems); } }

		/// <summary>
		/// Gets a list of examples of conflictuous examples
		/// </summary>
		public ROList<Phrase> Examples { get { return new ROList<Phrase>(inputExamples); } }

		/// <summary>
		/// Initializes this error
		/// </summary>
		/// <param name="state">The state raising the state</param>
		/// <param name="type">The type of error</param>
		public Error(State state, ErrorType type)
		{
			errorState = state;
			this.type = type;
			errorItems = new List<Item>();
			inputExamples = new List<Phrase>();
		}

		/// <summary>
		/// Adds a conflictuous item to this conflict
		/// </summary>
		/// <param name="item">The item to add</param>
		public void AddItem(Item item)
		{
			errorItems.Add(item);
		}

		/// <summary>
		/// Adds an input example to this conflict
		/// </summary>
		/// <param name="example">The example to add</param>
		public void AddExample(Phrase example)
		{
			inputExamples.Add(example);
		}
	}
}
