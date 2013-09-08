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

using System;
using System.Collections.Generic;
using System.IO;

namespace Hime.CentralDogma.Documentation
{
    class DOTSerializer
    {
        private StreamWriter writer;

        public DOTSerializer(string name, string file)
        {
            writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
            writer.WriteLine("digraph " + name + " {");
        }

        public void WriteNode(string id)
        {
			this.WriteNode(id, id);
        }

        public void WriteNode(string id, string label)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(label) + "\"];");
        }

        public void WriteNodeURL(string id, string url)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(id) + "\", URL=\"" + url + "\"];");
        }

        public void WriteNode(string id, string label, DOTNodeShape shape)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(label) + "\",shape=" + shape.ToString() + "];");
        }

        public void WriteEdge(string tail, string head, string label)
        {
            writer.WriteLine("    _" + tail + " -> _" + head + " [label=\"" + label + "\"];");
        }

        public void Close()
        {
            writer.WriteLine("}");
            writer.Close();
        }

        private string SanitizeString(string original)
        {
            return original.Replace("\"", "\\\"").Replace("\\", "\\\\");
        }
    }
}
