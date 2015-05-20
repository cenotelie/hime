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

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a node in a Graph-Structured Stack of a simulation of a GLR parser
	/// </summary>
	public class GLRStackNode
	{
		/// <summary>
		/// The state represented by this node
		/// </summary>
		private readonly State state;
		/// <summary>
		/// The edges from this node
		/// </summary>
		private readonly List<KeyValuePair<Symbol, GLRStackNode>> edges;

		/// <summary>
		/// Gets this node's identifier
		/// Same as the identifier of the LR state it represents
		/// </summary>
		public int ID { get { return state.ID; } }
		/// <summary>
		/// Gets the state represented by this node
		/// </summary>
		public State State { get { return state; } }

		/// <summary>
		/// Initializes a node
		/// </summary>
		/// <param name="state">The represented state</param>
		public GLRStackNode(State state)
		{
			this.state = state;
			edges = new List<KeyValuePair<Symbol, GLRStackNode>>();
		}

		/// <summary>
		/// Adds an edge to a previous node (if it does not exists already)
		/// </summary>
		/// <param name="symbol">The edge's label</param>
		/// <param name="node">The edge's target</param>
		public void AddEdge(Symbol symbol, GLRStackNode node)
		{
			foreach (KeyValuePair<Symbol, GLRStackNode> edge in edges)
			{
				if (edge.Key.ID == symbol.ID && edge.Value.state.ID == node.state.ID)
					return;
			}
			edges.Add(new KeyValuePair<Symbol, GLRStackNode>(symbol, node));
		}

		/// <summary>
		/// Gets the previous stack node by an edge labelled with the specified symbol
		/// </summary>
		/// <param name="symbol">A symbol</param>
		/// <returns>The previous stack node, or <c>null</c> if no edge with the specified symbol exists</returns>
		public GLRStackNode GetPreviousBy(Symbol symbol)
		{
			foreach (KeyValuePair<Symbol, GLRStackNode> edge in edges)
				if (edge.Key.ID == symbol.ID)
					return edge.Value;
			return null;
		}
	}
}