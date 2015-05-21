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
using System.Collections.Generic;

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents a choice in a rule, i.e. the remainder of a rule's body
	/// </summary>
	public class RuleChoice : IEnumerable<RuleBodyElement>
	{
		/// <summary>
		/// The elements in this body
		/// </summary>
		protected readonly List<RuleBodyElement> parts;
		/// <summary>
		/// The FIRSTS set of terminals
		/// </summary>
		protected readonly TerminalSet setFirsts;

		/// <summary>
		/// Gets the length of this body
		/// </summary>
		public int Length { get { return parts.Count; } }

		/// <summary>
		/// Gets the element at the specified index.
		/// </summary>
		/// <param name="index">The index of an element</param>
		public RuleBodyElement this[int index] { get { return parts[index]; } }

		/// <summary>
		/// Gets the FIRSTS set
		/// </summary>
		public TerminalSet Firsts { get { return setFirsts; } }

		/// <summary>
		/// Initializes this body as empty
		/// </summary>
		public RuleChoice()
		{
			parts = new List<RuleBodyElement>();
			setFirsts = new TerminalSet();
		}

		/// <summary>
		/// Initializes this body as containing a single element
		/// </summary>
		/// <param name="symbol">The single element's symbol</param>
		public RuleChoice(Symbol symbol)
		{
			parts = new List<RuleBodyElement>();
			parts.Add(new RuleBodyElement(symbol, Hime.Redist.TreeAction.None));
			setFirsts = new TerminalSet();
		}

		/// <summary>
		/// Gets the enumerator of the inner parts
		/// </summary>
		/// <returns>The enumerator</returns>
		public IEnumerator<RuleBodyElement> GetEnumerator()
		{
			return parts.GetEnumerator();
		}

		/// <summary>
		/// Gets the enumerator of the inner parts
		/// </summary>
		/// <returns>The enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return parts.GetEnumerator();
		}

		/// <summary>
		/// Append the specified element
		/// </summary>
		/// <param name="element">An element</param>
		public void Append(Symbol element)
		{
			parts.Add(new RuleBodyElement(element, Hime.Redist.TreeAction.None));
		}

		/// <summary>
		/// Computes the FIRSTS set for this rule body
		/// </summary>
		/// <returns><c>true</c> if there has been modifications</returns>
		public bool ComputeFirsts(RuleChoice next)
		{
			// If the choice is empty : Add the ε to the Firsts and return
			if (parts.Count == 0)
				return setFirsts.Add(Epsilon.Instance);

			Symbol symbol = parts[0].Symbol;
			// If the first symbol in the choice is a terminal : Add terminal as first and return
			var terminal = symbol as Terminal;
			if (terminal != null)
				return setFirsts.Add(terminal);

			// Here the first symbol in the current choice is a variable
			Variable variable = symbol as Variable;
			bool mod = false; // keep track of modifications
			// foreach first in the FIRSTS set of the variable
			foreach (Terminal first in variable.Firsts)
			{
				// If the symbol is ε
				if (first == Epsilon.Instance)
                    // Add the Firsts set of the next choice to the current Firsts set
					mod = mod || setFirsts.AddRange(next.setFirsts);
				else
                    // Symbol is not ε : Add the symbol to the Firsts set
					mod = mod || setFirsts.Add(first);
			}
			return mod;
		}
	}
}