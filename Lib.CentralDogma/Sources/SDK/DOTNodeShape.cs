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

namespace Hime.CentralDogma.SDK
{
	/// <summary>
	/// Represents the shape of nodes in a graph in the DOT format
	/// </summary>
    public enum DOTNodeShape
    {
        box, polygon, ellipse, circle,
        point, egg, triangle, plaintext,
        diamond, trapezium, parallelogram, house,
        pentagon, hexagon, septagon, octagon,
        doublecircle, doubleoctagon, tripleoctagon, invtriangle,
        invtrapezium, invhouse, Mdiamond, Msquare,
        Mcircle, rect, rectangle, square,
        none, note, tab, folder,
        box3d, component
    }
}