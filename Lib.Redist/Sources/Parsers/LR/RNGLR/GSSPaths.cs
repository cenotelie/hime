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
	/// Represents a collection of paths in a GSS
	/// </summary>
    struct GSSPaths
    {
    	private int count;
    	private GSSPath[] buffer;
    	
    	/// <summary>
    	/// Gets the number of paths on this collection
    	/// </summary>
    	public int Count { get { return count; } }
    	/// <summary>
    	/// Gets the i-th path in this collection
    	/// </summary>
    	public GSSPath this[int index] { get { return buffer[index]; } }
    	
    	/// <summary>
    	/// Initializes the collection
    	/// </summary>
    	/// <param name="count">The number of paths</param>
    	/// <param name="buffer">The paths' data</param>
    	public GSSPaths(int count, GSSPath[] buffer)
    	{
    		this.count = count;
    		this.buffer = buffer;
    	}
    }
}