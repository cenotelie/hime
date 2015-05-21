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
using Hime.Redist;
using Hime.Redist.Utils;

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents a template rule in a grammar
	/// </summary>
	public class TemplateRule
	{
		/// <summary>
		/// The existing instances of this template rule
		/// </summary>
		private readonly List<TemplateRuleInstance> instances;
		/// <summary>
		/// The parent grammar
		/// </summary>
		private readonly Grammar grammar;
		/// <summary>
		/// The name of the head variable
		/// </summary>
		private readonly string head;
		/// <summary>
		/// The list of the parameters
		/// </summary>
		private readonly List<string> parameters;
		/// <summary>
		/// The root AST for the definition of this rule
		/// </summary>
		private readonly ASTNode root;

		/// <summary>
		/// Gets the name of the head variable
		/// </summary>
		public string HeadName { get { return head; } }

		/// <summary>
		/// Gets the parameters of this rule
		/// </summary>
		public ROList<string> Parameters { get { return new ROList<string>(parameters); } }

		/// <summary>
		/// Gets root AST node for this rule
		/// </summary>
		public ASTNode RuleNode { get { return root; } }

		/// <summary>
		/// Gets AST node for this rule's definition (body)
		/// </summary>
		public ASTNode DefinitionNode { get { return root.Children[2]; } }

		/// <summary>
		/// Initializes this template rule
		/// </summary>
		/// <param name="grammar">The parent grammar</param>
		/// <param name="ruleNode">The root AST for this rule</param>
		public TemplateRule(Grammar grammar, ASTNode ruleNode)
		{
			instances = new List<TemplateRuleInstance>();
			this.grammar = grammar;
			head = ruleNode.Children[0].Value;
			parameters = new List<string>();
			root = ruleNode;
			foreach (ASTNode child in ruleNode.Children[1].Children)
				parameters.Add(child.Value);
		}

		/// <summary>
		/// Initializes this template rule as copy of another root
		/// </summary>
		/// <param name="copied">The copied template rule</param>
		/// <param name="grammar">The parent grammar</param>
		public TemplateRule(TemplateRule copied, Grammar grammar)
		{
			instances = new List<TemplateRuleInstance>();
			this.grammar = grammar;
			head = copied.head;
			parameters = new List<string>(copied.parameters);
			root = copied.root;
			foreach (TemplateRuleInstance instance in copied.instances)
			{
				List<Symbol> param = new List<Symbol>();
				foreach (Symbol symbol in instance.Parameters)
					param.Add(grammar.GetSymbol(symbol.Name));
				instances.Add(new TemplateRuleInstance(this, param, grammar));
			}
		}

		/// <summary>
		/// Instantiate this rule with the given context
		/// </summary>
		/// <param name="context">The context</param>
		/// <param name="parameters">The parameter values</param>
		/// <returns>The generated variable containing the instantiated rule</returns>
		public Variable Instantiate(Context context, List<Symbol> parameters)
		{
			foreach (TemplateRuleInstance instance in instances)
			{
				if (instance.MatchParameters(parameters))
					return instance.HeadVariable;
			}
			TemplateRuleInstance newInstance = new TemplateRuleInstance(this, parameters, grammar);
			instances.Add(newInstance);
			newInstance.Compile(context);
			return newInstance.HeadVariable;
		}
	}
}
