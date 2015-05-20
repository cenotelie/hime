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

namespace Hime.SDK.Reflection
{
	/// <summary>
	/// Serializes graphs in the DOT format
	/// </summary>
	public class DOTSerializer
	{
		/// <summary>
		/// The writer
		/// </summary>
		private readonly StreamWriter writer;

		/// <summary>
		/// Initializes a new instance of the serializer
		/// </summary>
		/// <param name="name">Name of the graph</param>
		/// <param name="file">File to serialize to</param>
		public DOTSerializer(string name, string file)
		{
			writer = new StreamWriter(file, false, System.Text.Encoding.UTF8);
			writer.WriteLine("digraph " + name + " {");
		}

		/// <summary>
		/// Writes a node with the given id
		/// </summary>
		/// <param name="id">Node's ID</param>
		public void WriteNode(string id)
		{
			WriteNode(id, id);
		}

		/// <summary>
		/// Writes a node with the given id
		/// </summary>
		/// <param name="id">Node's ID</param>
		/// <param name="label">Node's labe</param>
		public void WriteNode(string id, string label)
		{
			writer.WriteLine("    " + id + " [label=\"" + SanitizeString(label) + "\"];");
		}

		/// <summary>
		/// Writes a node with the given id and URL
		/// </summary>
		/// <param name="id">Node's ID</param>
		/// <param name="url">Node's url</param>
		public void WriteNodeURL(string id, string url)
		{
			writer.WriteLine("    " + id + " [label=\"" + SanitizeString(id) + "\", URL=\"" + url + "\"];");
		}

		/// <summary>
		/// Writes a node with the given id
		/// </summary>
		/// <param name="id">Node's ID</param>
		/// <param name="label">Node's labe</param>
		/// <param name="shape">Node's shape</param>
		public void WriteNode(string id, string label, DOTNodeShape shape)
		{
			writer.WriteLine("    " + id + " [label=\"" + SanitizeString(label) + "\",shape=" + shape + "];");
		}

		/// <summary>
		/// Writes an edge
		/// </summary>
		/// <param name="tail">ID of the starting node</param>
		/// <param name="head">ID of the ending node</param>
		/// <param name="label">Label for the edge</param>
		public void WriteEdge(string tail, string head, string label)
		{
			writer.WriteLine("    " + tail + " -> " + head + " [label=\"" + label + "\"];");
		}

		/// <summary>
		/// Writes a structure as a node
		/// </summary>
		/// <param name="id">Node's ID</param>
		/// <param name="label">Node's labe</param>
		/// <param name="items">Items of this structure</param>
		public void WriteStructure(string id, string label, string[] items)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("    ");
			builder.Append(id);
			builder.Append(" [label=\"{ ");
			builder.Append(SanitizeString(label));
			foreach (string item in items)
			{
				builder.Append(" | { | ");
				builder.Append(SanitizeString(item).Replace("|", "\\|").Replace("<", "\\<").Replace(">", "\\>").Replace("{", "\\{").Replace("}", "\\}"));
				builder.Append(" }");
			}
			builder.Append(" }\", shape=\"record\"];");
			writer.WriteLine(builder);
		}

		/// <summary>
		/// Closes this serializer
		/// </summary>
		public void Close()
		{
			writer.WriteLine("}");
			writer.Close();
		}

		/// <summary>
		/// Sanitizes the given string for the DOT format
		/// </summary>
		private static string SanitizeString(string original)
		{
			return original.Replace("\"", "\\\"").Replace("\\", "\\\\");
		}
	}
}
