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
using Hime.Redist;

namespace Hime.CentralDogma.Automata
{
	/// <summary>
	/// Represents a reduction in a LR automaton
	/// </summary>
	public class LRReduction
	{
		/// <summary>
		/// The lookahead to reduce on
		/// </summary>
		private Symbol lookahead;
		/// <summary>
		/// The reduced variable
		/// </summary>
		private Symbol head;
		/// <summary>
		/// The reduction's length
		/// </summary>
		private int length;

		/// <summary>
		/// Gets the lookahead terminal to reduce on
		/// </summary>
		public Symbol Lookahead { get { return lookahead; } }
		/// <summary>
		/// Gets the head (variable) of the reduced rule
		/// </summary>
		public Symbol Head { get { return head; } }
		/// <summary>
		/// Gets the reduction's length
		/// </summary>
		public int Length { get { return length; } }

		/// <summary>
		/// Initializes this reduction
		/// </summary>
		/// <param name="lookahead">The lookahead to reduce on</param>
		/// <param name="head">The reduced variable</param>
		/// <param name="length">The reduction's length</param>
		public LRReduction(Symbol lookahead, Symbol head, int length)
		{
			this.lookahead = lookahead;
			this.head = head;
			this.length = length;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Automata.LRReduction"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Automata.LRReduction"/>.
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("[");
			builder.Append(head);
			builder.Append(" ->");
			for (int i=0; i!=length; i++)
				builder.Append(" ?");
			builder.Append("] on ");
			builder.Append(lookahead);
			return builder.ToString();
		}
	}
}