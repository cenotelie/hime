/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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

namespace Hime.SDK.Reflection
{
	/// <summary>
	/// Represents a tracer of the execution of the semantic actions in a parser
	/// </summary>
	public class ActionTracer
	{
		/// <summary>
		/// The semantic actions
		/// </summary>
		private Dictionary<string, SemanticAction> actions;
		/// <summary>
		/// The produced trace as a list of indices in the list of names
		/// </summary>
		private List<string> trace;

		/// <summary>
		/// Gets the produced trace as a list of semantic actions' names
		/// </summary>
		public ROList<string> Trace { get { return new ROList<string>(trace); } }

		/// <summary>
		/// Gets the semantic actions
		/// </summary>
		public Dictionary<string, SemanticAction> Actions { get { return actions; } }

		/// <summary>
		/// Initializes this tracer
		/// </summary>
		/// <param name="parserType">The parser's type</param>
		public ActionTracer(System.Type parserType)
		{
			this.actions = new Dictionary<string, SemanticAction>();
			this.trace = new List<string>();
			System.Type innerType = parserType.GetNestedType("Actions");
			System.Type typeSymbol = typeof(Hime.Redist.Symbol);
			System.Type typeBody = typeof(Hime.Redist.SemanticBody);
			foreach (System.Reflection.MethodInfo method in innerType.GetMethods())
			{
				System.Reflection.ParameterInfo[] parameters = method.GetParameters();
				if (parameters.Length != 2)
					continue;
				if (parameters[0].ParameterType != typeSymbol)
					continue;
				if (parameters[1].ParameterType != typeBody)
					continue;
				string name = method.Name;
				TracingAction<string> action = new TracingAction<string>(this.trace, name);
				this.actions.Add(name, new SemanticAction(action.Callback));
			}
		}
	}
}