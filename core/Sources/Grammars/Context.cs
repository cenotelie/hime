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
	/// Represents the context of a loader
	/// </summary>
	public class Context
	{
		/// <summary>
		/// The loader
		/// </summary>
		private Loader loader;
		/// <summary>
		/// The current template rules
		/// </summary>
		private List<TemplateRule> templateRules;
		/// <summary>
		/// The binding of template parameters to their value
		/// </summary>
		private Dictionary<string, Symbol> references;

		/// <summary>
		/// Gets the loader to which this context is associated
		/// </summary>
		public Loader Loader { get { return loader; } }

		/// <summary>
		/// Initializes this context
		/// </summary>
		/// <param name="loader">The parent loader</param>
		public Context(Loader loader)
		{
			this.loader = loader;
			this.templateRules = new List<TemplateRule>();
			this.references = new Dictionary<string, Symbol>();
		}

		/// <summary>
		/// Initializes this context from a parent one
		/// </summary>
		/// <param name="copied">The context to copy</param>
		public Context(Context copied)
		{
			loader = copied.Loader;
			templateRules = new List<TemplateRule>(copied.templateRules);
			references = new Dictionary<string, Symbol>();
		}

		/// <summary>
		/// Adds a template rule to this context
		/// </summary>
		/// <param name="templateRule">A template rule</param>
		public void AddTemplateRule(TemplateRule templateRule)
		{
			templateRules.Add(templateRule);
		}

		/// <summary>
		/// Determines whether this context has a templat rule of the given name with the number of parameters
		/// </summary>
		/// <param name="name">A template rule's name</param>
		/// <param name="paramCount">The number of parameters</param>
		/// <returns><c>true</c> if a matching template rule is found</returns>
		public bool IsTemplateRule(string name, int paramCount)
		{
			foreach (TemplateRule tRule in templateRules)
				if ((tRule.HeadName == name) && (tRule.Parameters.Count == paramCount))
					return true;
			return false;
		}

		/// <summary>
		/// Gets the variable produced by a template rule's instantiation
		/// </summary>
		/// <param name="name">A template rule's name</param>
		/// <param name="parameters">The parameter values</param>
		/// <returns>The variable produced by the template rule's instatiation</returns>
		public Variable InstantiateMetaRule(string name, List<Symbol> parameters)
		{
			foreach (TemplateRule tRule in templateRules)
			{
				if ((tRule.HeadName == name) && (tRule.Parameters.Count == parameters.Count))
					return tRule.Instantiate(this, parameters);
			}
			return null;
		}

		/// <summary>
		/// Adds a binding to this context
		/// </summary>
		/// <param name="name">The bound name</param>
		/// <param name="symbol">The value associated to the name</param>
		public void AddBinding(string name, Symbol symbol)
		{
			references.Add(name, symbol);
		}

		/// <summary>
		/// Determines whether the given name is bound in this context
		/// </summary>
		/// <param name="name">A name</param>
		/// <returns><c>true</c> if the given name is bound in this context; otherwise, <c>false</c></returns>
		public bool IsBound(string name)
		{
			return references.ContainsKey(name);
		}

		/// <summary>
		/// Gets the value bound to the given name
		/// </summary>
		/// <param name="name">A name</param>
		/// <returns>The bound value</returns>
		public Symbol GetBinding(string name)
		{
			if (references.ContainsKey(name))
				return references[name];
			return null;
		}
	}
}
