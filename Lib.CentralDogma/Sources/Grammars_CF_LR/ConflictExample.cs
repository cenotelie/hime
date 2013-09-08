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

using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class ConflictExample
    {
        private List<Terminal> input;
        private Terminal lookahead;
        private List<Terminal> rest;

        public List<Terminal> Input { get { return input; } }
        public List<Terminal> Rest { get { return rest; } }

        public ConflictExample(Terminal l1)
        {
            input = new List<Terminal>();
            rest = new List<Terminal>();
            lookahead = l1;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode node = doc.CreateElement("Example");
            foreach (Terminal t in input)
                node.AppendChild(t.GetXMLNode(doc));
            node.AppendChild(doc.CreateElement("Dot"));
            node.AppendChild(lookahead.GetXMLNode(doc));
            foreach (Terminal t in rest)
                node.AppendChild(t.GetXMLNode(doc));
            return node;
        }
    }
}