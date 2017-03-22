/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a reference to a Shared-Packed Parse Forest node in a specific version
	/// </summary>
	struct SPPFNodeRef
	{
		/// <summary>
		/// The identifier of the node
		/// </summary>
		private readonly int nodeId;
		/// <summary>
		/// The version to refer to
		/// </summary>
		private readonly int version;

		/// <summary>
		/// Gets the identifier of the node referred to
		/// </summary>
		public int NodeId { get { return nodeId; } }

		/// <summary>
		/// Gets the version of the node referred to
		/// </summary>
		public int Version { get { return version; } }

		/// <summary>
		/// Initializes this reference
		/// </summary>
		/// <param name="nodeId">The identifier of the node to refer to</param>
		/// <param name="version">The version of the node to refer to</param>
		public SPPFNodeRef(int nodeId, int version)
		{
			this.nodeId = nodeId;
			this.version = version;
		}
	}
}