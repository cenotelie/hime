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
using System.IO;
using System.Runtime.InteropServices;

namespace Hime.Redist.Utils
{
	/// <summary>
	/// Represents a blob of binary data that can be accessed as an array of items T
	/// </summary>
	/// <typeparam name="T">The type of the stored items</typeparam>
	class Blob<T>
	{
		/// <summary>
		/// The user data (backed by blob)
		/// </summary>
		private T[] data;
		/// <summary>
		/// The length of the data in bytes
		/// </summary>
		private int length;

		/// <summary>
		/// Gets the number of items in this blob
		/// </summary>
		public int Count { get { return data.Length; } }
		/// <summary>
		/// Gets the length of this blob in bytes
		/// </summary>
		public int Length { get { return length; } }
		/// <summary>
		/// Gets the i-th item in this blob
		/// </summary>
		/// <param name="index">Index in this blob</param>
		/// <returns>The item at the given index</returns>
		public T this[int index] { get { return data[index]; } }

		/// <summary>
		/// Initializes a new blob
		/// </summary>
		/// <param name="count">The number of items to store in this blob</param>
		/// <param name="size">The size of the an element in byte</param>
		public Blob(int count, int size)
		{
			this.data = new T[count];
			this.length = count * size;
		}

		/// <summary>
		/// Loads and fills this blob from the given binary reader
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <returns><c>true</c> if the operation was successful</returns>
		public bool LoadFrom(BinaryReader reader)
		{
			// load the data in a buffer
			byte[] buffer = Read(reader, length);
			if (buffer == null)
				return false;
			// copy them into the data set
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			System.IntPtr ptr = handle.AddrOfPinnedObject();
			Marshal.Copy(buffer, 0, ptr, length);
			handle.Free();
			return true;
		}

		/// <summary>
		/// Reads the the specified length of data and returns the buffer
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <param name="length">Number of bytes to read</param>
		/// <returns>The data in a buffer, or <c>null</c> if the read failed</returns>
		private static byte[] Read(BinaryReader reader, int length)
		{
			byte[] buffer = new byte[length]; 
			int current = 0;
			while (current < length)
			{
				int read = reader.Read(buffer, current, length - current);
				// If the end of the input is reached prematurely,
				if (read == 0)
					return null;
				current += read;
			}
			return buffer;
		}
	}
}
