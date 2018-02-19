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
using System.Text;
using Hime.Redist;
using Hime.Redist.Utils;

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents the instance of a template rule
	/// </summary>
	public class TemplateRuleInstance
	{
		/// <summary>
		/// The parent rule
		/// </summary>
		private readonly TemplateRule templateRule;
		/// <summary>
		/// The produced variable
		/// </summary>
		private readonly Variable variable;
		/// <summary>
		/// The parameter values
		/// </summary>
		private readonly List<Symbol> parameters;

		/// <summary>
		/// Gets the head variable for this rule
		/// </summary>
		public Variable HeadVariable { get { return variable; } }

		/// <summary>
		/// Gets the parameter values
		/// </summary>
		public ROList<Symbol> Parameters { get { return new ROList<Symbol>(parameters); } }

		/// <summary>
		/// Initializes this template rule instance
		/// </summary>
		/// <param name="tRule">The parent template rule</param>
		/// <param name="parameters">The values for the template rule parameters</param>
		/// <param name="grammar">The parent grammar</param>
		public TemplateRuleInstance(TemplateRule tRule, List<Symbol> parameters, Grammar grammar)
		{
			// Build the head variable name
			StringBuilder builder = new StringBuilder();
			builder.Append(tRule.HeadName);
			builder.Append("<");
			for (int i = 0; i != parameters.Count; i++)
			{
				if (i != 0)
					builder.Append(", ");
				builder.Append(parameters[i].Name);
			}
			builder.Append(">");
			string name = builder.ToString();
			// Create and add the variable to the grammar
			variable = grammar.AddVariable(name);
			// Copy parameters
			this.parameters = new List<Symbol>(parameters);
			// Set parent template rule
			templateRule = tRule;
		}

		/// <summary>
		/// Compile this rule and generate the associated grammar rule
		/// </summary>
		/// <param name="context">The current context</param>
		public void Compile(LoaderContext context)
		{
			// Create a new context for recognizing the rule
			LoaderContext newContext = new LoaderContext(context);
			// Add the parameters as references in the new context
			for (int i = 0; i != parameters.Count; i++)
				newContext.AddBinding(templateRule.Parameters[i], parameters[i]);
			// Recognize the rule with the new context
			RuleBodySet set = newContext.Loader.BuildDefinitions(newContext, templateRule.DefinitionNode);
			// Add recognized rules to the variable
			foreach (RuleBody def in set)
				variable.AddRule(new Rule(variable, TreeAction.None, def, 0));
		}

		/// <summary>
		/// Determines whether the given parameter values match this instance
		/// </summary>
		/// <param name="parameters">The parameter values to check</param>
		/// <returns><c>true</c> if the parameters match</returns>
		public bool MatchParameters(List<Symbol> parameters)
		{
			if (this.parameters.Count != parameters.Count)
				return false;
			for (int i = 0; i != this.parameters.Count; i++)
			{
				if (this.parameters[i].ID != parameters[i].ID)
					return false;
			}
			return true;
		}
	}
}