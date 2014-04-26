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

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents an edge in a GSS
	/// </summary>
    struct GSSEdge
    {
        private GSSNode to;
        private int label;
        
        /// <summary>
        /// Gets the target node of this edge
        /// </summary>
        public GSSNode To { get { return to; } }
        /// <summary>
        /// Gets the label attached to this edge
        /// </summary>
        public int Label { get { return label; } }
        
        /// <summary>
        /// Initializes this edge
        /// </summary>
        /// <param name="node">The edge's target</param>
        /// <param name="label">The edge's label</param>
        public GSSEdge(GSSNode node, int label)
        {
        	this.to = node;
        	this.label = label;
        }
    }
}
