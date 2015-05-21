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
using Hime.Redist.Utils;

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents a variable in a grammar
	/// </summary>
	public class Variable : Symbol
	{
		/// <summary>
		/// The rules for this variable
		/// </summary>
		private readonly List<Rule> rules;
		/// <summary>
		/// The FIRSTS set for this variable
		/// </summary>
		private readonly TerminalSet firsts;
		/// <summary>
		/// The FOLLOWERS set for this variable
		/// </summary>
		private readonly TerminalSet followers;

		/// <summary>
		/// Gets the rules for this variables
		/// </summary>
		public ROList<Rule> Rules { get { return new ROList<Rule>(rules); } }

		/// <summary>
		/// Gets the FIRSTS set of this variable
		/// </summary>
		public TerminalSet Firsts { get { return firsts; } }

		/// <summary>
		/// Gets the FOLLOWERS set of this variable
		/// </summary>
		public TerminalSet Followers { get { return followers; } }

		/// <summary>
		/// Initializes this symbol
		/// </summary>
		/// <param name="sid">The symbol's unique identifier</param>
		/// <param name="name">The symbol's name</param>
		public Variable(int sid, string name) : base(sid, name)
		{
			rules = new List<Rule>();
			firsts = new TerminalSet();
			followers = new TerminalSet();
		}

		/// <summary>
		/// Adds the given rule for this variable as a unique element
		/// </summary>
		/// <param name="rule">The rule to add</param>
		/// <returns>The given rule, or the equivalent one if it already existed in this variable</returns>
		public Rule AddRule(Rule rule)
		{
			int index = rules.IndexOf(rule);
			if (index != -1)
				return rules[index];
			rules.Add(rule);
			return rule;
		}

		/// <summary>
		/// Computes the FIRSTS set for this variable
		/// </summary>
		/// <returns><c>true</c> if there has been modifications</returns>
		public bool ComputeFirsts()
		{
			bool mod = false;
			foreach (Rule rule in rules)
			{
				TerminalSet rulefirsts = rule.Body.Firsts;
				if (rulefirsts != null)
					mod |= firsts.AddRange(rulefirsts);
				mod |= rule.Body.ComputeFirsts();
			}
			return mod;
		}

		/// <summary>
		/// Computes the FOLLOWERS sets, step 1
		/// </summary>
		public void ComputeFollowers_Step1()
		{
			foreach (Rule rule in rules)
				rule.Body.ComputeFollowers_Step1();
		}

		/// <summary>
		/// Computes the FOLLOWERS sets, step2
		/// </summary>
		/// <returns><c>true</c> if there has been modifications</returns>
		public bool ComputeFollowers_Step23()
		{
			bool mod = false;
			foreach (Rule rule in rules)
				mod |= rule.Body.ComputeFollowers_Step23(this);
			return mod;
		}
	}
}