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
using Hime.Redist;

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents an ordered set of grammar rule bodies
	/// </summary>
	public class RuleBodySet : List<RuleBody>
	{
		/// <summary>
		/// Builds the union of the left and right set
		/// </summary>
		/// <param name="left">A set of rule bodies</param>
		/// <param name="right">A set of rule bodies</param>
		public static RuleBodySet Union(RuleBodySet left, RuleBodySet right)
		{
			RuleBodySet result = new RuleBodySet();
			result.AddRange(left);
			result.AddRange(right);
			return result;
		}

		/// <summary>
		/// Builds the product of the left and right set
		/// </summary>
		/// <param name="left">A set of rule bodies</param>
		/// <param name="right">A set of rule bodies</param>
		public static RuleBodySet Multiply(RuleBodySet left, RuleBodySet right)
		{
			RuleBodySet result = new RuleBodySet();
			foreach (RuleBody defLeft in left)
				foreach (RuleBody defRight in right)
					result.Add(RuleBody.Concatenate(defLeft, defRight));
			return result;
		}

		/// <summary>
		/// Applies the given action to all bodies in this set
		/// </summary>
		/// <param name="action">The action to apply</param>
		public void ApplyAction(TreeAction action)
		{
			foreach (RuleBody body in this)
				body.ApplyAction(action);
		}
	}
}