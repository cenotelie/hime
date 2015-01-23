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
	/// Represents the body of a grammar rule
	/// </summary>
	public class RuleBody : RuleChoice
	{
		/// <summary>
		/// The choices in this body
		/// </summary>
		private List<RuleChoice> choices;

		/// <summary>
		/// Gets the choices in this rule
		/// </summary>
		public ROList<RuleChoice> Choices { get { return new ROList<RuleChoice>(choices); } }

		/// <summary>
		/// Initializes this body as empty
		/// </summary>
		public RuleBody() : base()
		{
		}

		/// <summary>
		/// Initializes this body as containing a single element
		/// </summary>
		/// <param name="symbol">The single element's symbol</param>
		public RuleBody(Symbol symbol) : base(symbol)
		{
		}

		/// <summary>
		/// Initializes this body as a copy of the given elements
		/// </summary>
		/// <param name="parts">The elements to copy</param>
		public RuleBody(ICollection<RuleBodyElement> parts) : base()
		{
			this.parts.AddRange(parts);
		}

		/// <summary>
		/// Computes the FIRSTS set for this rule body
		/// </summary>
		/// <returns><c>true</c> if there has been modifications</returns>
		public bool ComputeFirsts()
		{
			if (choices == null)
				ComputeChoices();

			bool mod = false;
			// for all choices in the reverse order : compute FIRSTS set for the choice
			for (int i = choices.Count - 1; i != -1; i--)
			{
				if (i == choices.Count - 1)
					mod = mod || choices[i].ComputeFirsts(null);
				else
					mod = mod || choices[i].ComputeFirsts(choices[i + 1]);
			}
			return mod;
		}

		/// <summary>
		/// Computes the choices for this rule body
		/// </summary>
		private void ComputeChoices()
		{
			// Create the choices set
			choices = new List<RuleChoice>();
			// For each part of the definition which is not a virtual symbol nor an action symbol
			foreach (RuleBodyElement part in parts)
			{
				if ((part.Symbol is Virtual) || (part.Symbol is Action))
					continue;
				// Append the symbol to all the choices definition
				foreach (RuleChoice choice in choices)
					choice.Append(part.Symbol);
				// Create a new choice with only the symbol
				choices.Add(new RuleChoice(part.Symbol));
			}
			// Create a new empty choice
			choices.Add(new RuleChoice());
			setFirsts = choices[0].Firsts;
		}

		/// <summary>
		/// Computes the FOLLOWERS sets, step 1
		/// </summary>
		public void ComputeFollowers_Step1()
		{
			// For all choices but the last (empty)
			for (int i = 0; i != choices.Count - 1; i++)
			{
				// TODO: is and casts are not nice => try to remove all of them. They shouldn't be necessary
				// If the first symbol of the choice is a variable
				if (choices[i][0].Symbol is Variable)
				{
					Variable var = choices[i][0].Symbol as Variable;
					// Add the FIRSTS set of the next choice to the variable followers except ε
					foreach (Terminal first in choices[i + 1].Firsts)
					{
						if (first != Epsilon.Instance)
							var.Followers.Add(first);
					}
				}
			}
		}

		/// <summary>
		/// Computes the FOLLOWERS sets, step2
		/// </summary>
		/// <param name='ruleVar'>The head variable of this rule's body</param>
		/// <returns><c>true</c> if there has been modifications</returns>
		public bool ComputeFollowers_Step23(Variable ruleVar)
		{
			bool mod = false;
			// For all choices but the last (empty)
			for (int i = 0; i != choices.Count - 1; i++)
			{
				// If the first symbol of the choice is a variable
				if (choices[i][0].Symbol is Variable)
				{
					Variable var = choices[i][0].Symbol as Variable;
					// If the next choice FIRSTS set contains ε
					// add the FOLLOWERS of the head variable to the FOLLOWERS of the found variable
					if (choices[i + 1].Firsts.Contains(Epsilon.Instance))
					if (var.Followers.AddRange(ruleVar.Followers))
						mod = true;
				}
			}
			return mod;
		}

		/// <summary>
		/// Applies the given action to all elements in this body
		/// </summary>
		/// <param name="action">The action to apply</param>
		public void ApplyAction(Hime.Redist.TreeAction action)
		{
			foreach (RuleBodyElement part in parts)
				part.Action = action;
		}

		/// <summary>
		/// Produces the concatenation of the left and right bodies
		/// </summary>
		/// <param name='left'>The left rule body</param>
		/// <param name='right'>The right rule body</param>
		public static RuleBody Concatenate(RuleBody left, RuleBody right)
		{
			RuleBody result = new RuleBody();
			result.parts.AddRange(left.parts);
			result.parts.AddRange(right.parts);
			return result;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.SDK.Grammars.RuleBody"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.SDK.Grammars.RuleBody"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="System.Object"/> to compare with the current <see cref="Hime.SDK.Grammars.RuleBody"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.SDK.Grammars.RuleBody"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			RuleBody temp = obj as RuleBody;
			if (temp == null)
				return false;
			if (this.parts.Count != temp.parts.Count)
				return false;
			for (int i = 0; i != this.parts.Count; i++)
				if (!this.parts[i].Equals(temp.parts[i]))
					return false;
			return true;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.RuleBody"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.RuleBody"/>.
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			foreach (RuleBodyElement part in parts)
			{
				builder.Append(" ");
				builder.Append(part.ToString());
			}
			return builder.ToString();
		}
	}
}
