/**********************************************************************
* Copyright (c) 2016 Laurent Wouters
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
using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a node in a Shared-Packed Parse Forest
	/// A node can have multiple versions
	/// </summary>
	class SPPFNodeNormal : SPPFNode
	{
		/// <summary>
		/// The size of the version buffer
		/// </summary>
		private const int VERSION_COUNT = 4;

		/// <summary>
		/// The label of this node
		/// </summary>
		private readonly TableElemRef original;
		/// <summary>
		/// The different versions of this node
		/// </summary>
		private SPPFNodeVersion[] versions;
		/// <summary>
		/// The number of versions of this node
		/// </summary>
		private int versionsCount;

		/// <summary>
		/// Gets whether this node must be replaced by its children
		/// </summary>
		public override bool IsReplaceable { get { return false; } }

		/// <summary>
		/// Gets the original symbol for this node
		/// </summary>
		public override TableElemRef OriginalSymbol { get { return original; } }

		/// <summary>
		/// Gets the default version of this node
		/// </summary>
		public SPPFNodeVersion DefaultVersion { get { return versions[0]; } }

		/// <summary>
		/// Gets a specific version of this node
		/// </summary>
		/// <param name="version">The version number</param>
		/// <returns>The requested version</returns>
		public SPPFNodeVersion GetVersion(int version)
		{
			return versions[version];
		}

		/// <summary>
		/// Initializes this node
		/// </summary>
		/// <param name="identifier">The identifier of this node</param>
		/// <param name="label">The label of this node</param>
		public SPPFNodeNormal(int identifier, TableElemRef label) : base(identifier)
		{
			this.original = label;
			this.versions = new SPPFNodeVersion[VERSION_COUNT];
			this.versions[0] = new SPPFNodeVersion(label);
			this.versionsCount = 1;
		}

		/// <summary>
		/// Initializes this node
		/// </summary>
		/// <param name="identifier">The identifier of this node</param>
		/// <param name="original">The original symbol of this node</param>
		/// <param name="label">The label on the first version of this node</param>
		/// <param name="childrenBuffer">A buffer for the children</param>
		/// <param name="childrenCount">The number of children</param>
		public SPPFNodeNormal(int identifier, TableElemRef original, TableElemRef label, SPPFNodeRef[] childrenBuffer, int childrenCount) : base(identifier)
		{
			this.original = original;
			this.versions = new SPPFNodeVersion[VERSION_COUNT];
			this.versions[0] = new SPPFNodeVersion(label, childrenBuffer, childrenCount);
			this.versionsCount = 1;
		}

		/// <summary>
		/// Adds a new version to this node
		/// </summary>
		/// <param name="label">The label for this version of the node</param>
		/// <param name="children">A buffer of children for this version of the node</param>
		/// <param name="childrenCount">The number of children</param>
		/// <returns>The reference to this new version</returns>
		public SPPFNodeRef NewVersion(TableElemRef label, SPPFNodeRef[] children, int childrenCount)
		{
			if (versionsCount == versions.Length)
				Array.Resize(ref versions, versions.Length + VERSION_COUNT);
			versions[versionsCount] = new SPPFNodeVersion(label, children, childrenCount);
			SPPFNodeRef result = new SPPFNodeRef(identifier, versionsCount);
			versionsCount++;
			return result;
		}
	}
}