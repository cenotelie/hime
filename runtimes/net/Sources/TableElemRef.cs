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

namespace Hime.Redist
{
	/// <summary>
	/// Represents a compact reference to an element in a table
	/// </summary>
	struct TableElemRef
	{
		/// <summary>
		/// The backend data
		/// </summary>
		private readonly uint data;

		/// <summary>
		/// Gets the element's type
		/// </summary>
		public TableType Type { get { return (TableType)(data >> 30); } }

		/// <summary>
		/// Gets the element's index in its respective table
		/// </summary>
		public int Index { get { return (int)(data & 0x3FFFFFFF); } }

		/// <summary>
		/// Initializes this reference
		/// </summary>
		/// <param name="type">The element's type</param>
		/// <param name="index">The element's index in its table</param>
		public TableElemRef(TableType type, int index)
		{
			data = (((uint)type) << 30) | (uint)index;
		}

		public static bool operator==(TableElemRef left, TableElemRef right)
		{
			return (left.data == right.data);
		}

		public static bool operator!=(TableElemRef left, TableElemRef right)
		{
			return (left.data != right.data);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.Redist.TableElemRef"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return (int)data;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.Redist.TableElemRef"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="System.Object"/> to compare with the current <see cref="Hime.Redist.TableElemRef"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.Redist.TableElemRef"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return (data == ((TableElemRef)obj).data);
		}
	}
}