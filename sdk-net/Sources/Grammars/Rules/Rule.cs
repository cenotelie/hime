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

using System.Text;
using Hime.Redist;

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents a grammar rule
	/// </summary>
	public class Rule
	{
		/// <summary>
		/// The rule's head variable
		/// </summary>
		private readonly Variable head;
		/// <summary>
		/// The rule's body
		/// </summary>
		private readonly RuleBody body;
		/// <summary>
		/// The action on the rule's head
		/// </summary>
		private readonly TreeAction headAction;
		/// <summary>
		/// The lexical context pushed by this rule
		/// </summary>
		private readonly int context;

		/// <summary>
		/// Gets the rule's head variable
		/// </summary>
		public Variable Head { get { return head; } }

		/// <summary>
		/// Gets the rule's body
		/// </summary>
		public RuleBody Body { get { return body; } }

		/// <summary>
		/// Gets the action on the rule's head
		/// </summary>
		public TreeAction HeadAction { get { return headAction; } }

		/// <summary>
		/// Gets the lexical context pushed by this rule
		/// </summary>
		public int Context { get { return context; } }

		/// <summary>
		/// Initializes this rule
		/// </summary>
		/// <param name='head'>The rule's head</param>
		/// <param name='headAction'>Whether this rule is generated</param>
		/// <param name='body'>The rule's body</param>
		/// <param name='context'>The lexical context pushed by this rule</param>
		public Rule(Variable head, TreeAction headAction, RuleBody body, int context)
		{
			this.head = head;
			this.headAction = headAction;
			this.body = body;
			this.context = context;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.SDK.Grammars.Rule"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.SDK.Grammars.Rule"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="System.Object"/> to compare with the current <see cref="Hime.SDK.Grammars.Rule"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.SDK.Grammars.Rule"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			Rule temp = obj as Rule;
			return (temp != null && head.Equals(temp.head) && body.Equals(temp.body));
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.Rule"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.Rule"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(head.Name);
			builder.Append(" ->");
			builder.Append(body.ToString());
			return builder.ToString();
		}
	}
}
