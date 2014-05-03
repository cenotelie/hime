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

namespace Hime.CentralDogma.Grammars.LR
{
	/// <summary>
	/// Represents a generation in the simulation of a GLR parser
	/// </summary>
	public class GLRGeneration : IEnumerable<GLRStackNode>
	{
		/// <summary>
		/// The stack nodes in this generation
		/// </summary>
		private List<GLRStackNode> nodes;

		/// <summary>
		/// Gets the stack nodes in this generation
		/// </summary>
		public ROList<GLRStackNode> Nodes { get { return new ROList<GLRStackNode>(nodes); } }

		/// <summary>
		/// Initializes this generation as empty
		/// </summary>
		public GLRGeneration()
		{
			nodes = new List<GLRStackNode>();
		}

		/// <summary>
		/// Initializes this generation as a copy of the given one
		/// </summary>
		public GLRGeneration(GLRGeneration copied)
		{
			nodes = new List<GLRStackNode>(copied.nodes);
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator</returns>
		public IEnumerator<GLRStackNode> GetEnumerator() { return nodes.GetEnumerator(); }
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return nodes.GetEnumerator(); }

		/// <summary>
		/// Resolves a stack node in this generation for the given LR state
		/// </summary>
		/// <param name="state">A LR state</param>
		/// <returns>A stack node for the given LR state</returns>
		public GLRStackNode Resolve(State state)
		{
			foreach (GLRStackNode potential in nodes)
				if (state.ID == potential.ID)
					return potential;
			GLRStackNode node = new GLRStackNode(state);
			nodes.Add(node);
			return node;
		}
	}
}