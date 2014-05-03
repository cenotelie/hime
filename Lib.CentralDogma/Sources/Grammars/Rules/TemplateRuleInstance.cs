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
	/// Represents the instance of a template rule
	/// </summary>
	public class TemplateRuleInstance
	{
		/// <summary>
		/// The parent rule
		/// </summary>
		private TemplateRule templateRule;
		/// <summary>
		/// The produced variable
		/// </summary>
		private Variable variable;
		/// <summary>
		/// The parameter values
		/// </summary>
		private List<Symbol> parameters;

		/// <summary>
		/// Gets the head variable for this rule
		/// </summary>
		public Variable HeadVariable { get { return variable; } }
		/// <summary>
		/// Gets the parameter values
		/// </summary>
		public ROList<Symbol> Parameters { get { return new ROList<Symbol>(parameters); } }

		public TemplateRuleInstance(TemplateRule tRule, List<Symbol> parameters, Grammar grammar)
		{
			// Build the head variable name
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
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
			this.variable = grammar.AddVariable(name);
			// Copy parameters
			this.parameters = new List<Symbol>(parameters);
			// Set parent template rule
			this.templateRule = tRule;
		}

		/// <summary>
		/// Compile this rule and generate the associated grammar rule
		/// </summary>
		/// <param name="data">The parent grammar</param>
		/// <param name="context">The current context</param>
		public void Compile(Grammar grammar, Context context)
		{
			// Create a new context for recognizing the rule
			Context newContext = new Context(context);
			// Add the parameters as references in the new context
			for (int i = 0; i != parameters.Count; i++)
				newContext.AddBinding(templateRule.Parameters[i], parameters[i]);
			// Recognize the rule with the new context
			RuleBodySet set = newContext.Loader.BuildDefinitions(newContext, templateRule.DefinitionNode);
			// Add recognized rules to the variable
			foreach (RuleBody def in set)
				variable.AddRule(new Rule(variable, def, false));
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