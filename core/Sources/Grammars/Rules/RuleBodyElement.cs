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
	/// Represents an element in the body of a grammar rule
	/// </summary>
	public class RuleBodyElement
	{
		/// <summary>
		/// The symbol of this element
		/// </summary>
		private Symbol symbol;
		/// <summary>
		/// The action applied on this element
		/// </summary>
		private Hime.Redist.TreeAction action;

		/// <summary>
		/// Gets the symbol of this element
		/// </summary>
		public Symbol Symbol { get { return symbol; } }

		/// <summary>
		/// Gets or sets the action applied on this element
		/// </summary>
		public Hime.Redist.TreeAction Action
		{
			get { return action; }
			set { action = value; }
		}

		/// <summary>
		/// Initializes this element
		/// </summary>
		/// <param name="symbol">The element's symbol</param>
		/// <param name="action">The action applied on the element</param>
		public RuleBodyElement(Symbol symbol, Hime.Redist.TreeAction action)
		{
			this.symbol = symbol;
			this.action = action;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.SDK.Grammars.RuleBodyElement"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.SDK.Grammars.RuleBodyElement"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="System.Object"/> to compare with the current <see cref="Hime.SDK.Grammars.RuleBodyElement"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.SDK.Grammars.RuleBodyElement"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			RuleBodyElement temp = obj as RuleBodyElement;
			if (temp == null)
				return false;
			return (this.symbol.Equals(temp.symbol) && this.action == temp.action);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.RuleBodyElement"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.RuleBodyElement"/>.
		/// </returns>
		public override string ToString()
		{
			string s = symbol.ToString();
			if (action == Hime.Redist.TreeAction.Promote)
				return (s + "^");
			else if (action == Hime.Redist.TreeAction.Drop)
				return (s + "!");
			else
				return s;
		}
	}
}