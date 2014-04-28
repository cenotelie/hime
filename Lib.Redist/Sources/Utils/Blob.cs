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
	[StructLayout(LayoutKind.Explicit)]
	class Blob<T>
	{
		/// <summary>
		/// The backend by storage
		/// </summary>
		[FieldOffset(0)]
		private byte[] blob;

		/// <summary>
		/// The user data (backed by blob)
		/// </summary>
		[FieldOffset(0)]
		private T[] data;

		/// <summary>
		/// The number of items in this blob
		/// </summary>
		[FieldOffset(8)]
		private int count;

		/// <summary>
		/// Gets the length of the blob in byte
		/// </summary>
		public int Length { get { return blob.Length; } }

		/// <summary>
		/// Gets the number of items in this blob
		/// </summary>
		public int Count { get { return count; } }

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
		/// <param name="size">The size in bytes of an individual item</param>
		public Blob(int count, int size)
		{
			this.blob = new byte[count * size];
			this.count = count;
		}

		/// <summary>
		/// Loads and fills this blob from the given binary reader
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <returns><c>true</c> if the operation was successful</returns>
		public bool LoadFrom(BinaryReader reader)
		{
			return ReadBinary(reader, blob, 0, blob.Length);
		}

		/// <summary>
		/// Reads the binary data from a reader and puts them in a buffer
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <param name="buffer">The buffer to put the read data in</param>
		/// <param name="index">Start index in the buffer</param>
		/// <param name="length">Number of bytes to read</param>
		/// <returns><c>true</c> if the operation was successful</returns>
		protected static bool ReadBinary(BinaryReader reader, byte[] buffer, int index, int length)
		{
			int current = index;
			int count = 0;
			while (count < length)
			{
				int read = reader.Read(buffer, current, length - count);
				// If the end of the input is reached prematurely,
				if (read == 0)
					return false;
				count += read;
				current += read;
			}
			return true;
		}
	}
}
