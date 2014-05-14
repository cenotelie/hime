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

package hime.redist;

public class Symbol {
	public static final int sidEpsilon = 1;
	public static final int sidDollar = 2;

	private int id;
	private String name;
	private String value;

	public int getID() { return id; }

	public String getName() { return name; }

	public String getValue() { return value; }

	public Symbol(int id, String name) {
		this.id = id;
		this.name = name;
		this.value = name;
	}

	public Symbol(int id, String name, String value) {
		this.id = id;
		this.name = name;
		this.value = value;
	}
	
	@Override
	public String toString() {
		return value;
	}
}
