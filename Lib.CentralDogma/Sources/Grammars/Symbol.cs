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
using System.Xml;

namespace Hime.CentralDogma.Grammars
{
    abstract class Symbol
    {
        public ushort SID { get; private set; }
        public string Name { get; private set; }

        protected abstract string Type { get; }

        public Symbol(ushort sid, string name)
        {
            this.SID = sid;
            this.Name = name;
        }
        
		public virtual XmlNode GetXMLNode(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Symbol");
            this.AddAttributeToNode(document, node, "type", Type);
            this.AddAttributeToNode(document, node, "name", ToString());
            this.AddAttributeToNode(document, node, "sid", SID.ToString("X"));
            return node;
        }
		
		protected void AddAttributeToNode(XmlDocument document, XmlNode node, string attributeName, string attributeValue)
		{
			XmlAttribute attribute = document.CreateAttribute(attributeName);
			node.Attributes.Append(attribute);
			node.Attributes[attributeName].Value = attributeValue;
		}

        public sealed class EqualityComparer : IEqualityComparer<Symbol>
        {
            public bool Equals(Symbol x, Symbol y) { return (x.SID == y.SID); }
            public int GetHashCode(Symbol obj) { return obj.SID; }
            private EqualityComparer() { }
            private static EqualityComparer instance = new EqualityComparer();
            public static EqualityComparer Instance { get { return instance; } }
        }

        public override string ToString() { return Name; }
    }
}