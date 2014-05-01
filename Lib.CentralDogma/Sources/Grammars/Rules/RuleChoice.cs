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

namespace Hime.CentralDogma.Grammars
{
	/// <summary>
	/// Represents a choice in a rule, i.e. the remainder of a rule's body
	/// </summary>
	public class RuleChoice
	{
		/// <summary>
		/// The elements in this body
		/// </summary>
		protected List<RuleBodyElement> parts;
		/// <summary>
		/// The FIRSTS set of terminals
		/// </summary>
        protected TerminalSet firsts;

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
		public TerminalSet Firsts { get { return firsts; } }

		/// <summary>
		/// Initializes this body as empty
		/// </summary>
		public RuleChoice()
		{
			this.parts = new List<RuleBodyElement>();
			this.firsts = new TerminalSet();
		}

		/// <summary>
		/// Initializes this body as containing a single element
		/// </summary>
		/// <param name="symbol">The single element's symbol</param>
		public RuleChoice(Symbol symbol)
		{
			this.parts = new List<RuleBodyElement>();
			this.parts.Add(new RuleBodyElement(symbol, Hime.Redist.TreeAction.None));
			this.firsts = new TerminalSet();
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
				return firsts.Add(Epsilon.Instance);

			Symbol symbol = parts[0].Symbol;
			// If the first symbol in the choice is a terminal : Add terminal as first and return
			if (symbol is Terminal)
				return firsts.Add(symbol as Terminal);

			// Here the first symbol in the current choice is a variable
			Variable variable = symbol as Variable;
			bool mod = false; // keep track of modifications
			// foreach first in the FIRSTS set of the variable
			foreach (Terminal first in variable.Firsts)
			{
				// If the symbol is ε
				if (first == Epsilon.Instance)
                    // Add the Firsts set of the next choice to the current Firsts set
					mod = mod || firsts.AddRange(next.firsts);
				else
                    // Symbol is not ε : Add the symbol to the Firsts set
					mod = mod || firsts.Add(first);
			}
			return mod;
		}
	}
}