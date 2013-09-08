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
using System.Text;
using System.Xml;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class State
    {
        protected StateKernel kernel;
        protected List<Item> items;
        protected Dictionary<Symbol, State> children;
        protected StateReductions reductions;

        public int ID { get; set; }
		public StateKernel Kernel { get { return kernel; } }
        public StateReductions Reductions { get { return reductions; } }
        public ICollection<Item> Items { get { return items; } }
        public Dictionary<Symbol, State> Children { get { return children; } }
        public ICollection<Conflict> Conflicts { get { return reductions.Conflicts; } }
        
        public State(StateKernel Kernel, List<Item> Items)
        {
            kernel = Kernel;
            items = Items;
            children = new Dictionary<Symbol, State>(Symbol.EqualityComparer.Instance);
        }

        public void BuildGraph(Graph Graph)
        {
            // Shift dictionnary for the current set
            Dictionary<Symbol, StateKernel> Shifts = new Dictionary<Symbol, StateKernel>();
            // Build the children kernels from the shift actions
            foreach (Item Item in items)
            {
                // Ignore reduce actions
                if (Item.Action == ItemAction.Reduce)
                    continue;

                if (Shifts.ContainsKey(Item.NextSymbol))
                    Shifts[Item.NextSymbol].AddItem(Item.GetChild());
                else
                {
                    StateKernel Kernel = new StateKernel();
                    Kernel.AddItem(Item.GetChild());
                    Shifts.Add(Item.NextSymbol, Kernel);
                }
            }
            // Close the children and add them to the graph
            foreach (Symbol Next in Shifts.Keys)
            {
                StateKernel Kernel = Shifts[Next];
                State Child = Graph.ContainsSet(Kernel);
                if (Child == null)
                {
                    Child = Kernel.GetClosure();
                    Graph.Add(Child);
                }
                children.Add(Next, Child);
            }
        }
        public void BuildReductions(StateReductions Reductions)
        {
            reductions = Reductions;
            reductions.Build(this);
        }
        

        public bool Equals(State Set) { return (kernel.Equals(Set.kernel)); }
        public override bool Equals(object obj) { return Equals(obj as State); }
        public override int GetHashCode() { return base.GetHashCode(); }
		
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder("S ");
            Builder.Append(this.ID.ToString("X"));
            Builder.Append(" = {");
            foreach (Item Item in items)
            {
                Builder.Append(" ");
                Builder.Append(Item.ToString(false));
                Builder.Append(" ");
            }
            Builder.Append("}");
            return Builder.ToString();
        }
		
		internal string ToStringForSerialization()
		{
			return this.ID.ToString("X");
		}

        internal XmlNode Serialize(XmlDocument document, GraphInverse inverse)
		{
            XmlNode root = document.CreateElement("ItemSet");
            root.Attributes.Append(document.CreateAttribute("SetID"));
            root.Attributes["SetID"].Value = this.ToStringForSerialization();
            foreach (Item item in this.Items)
                root.AppendChild(item.GetXMLNode(document, inverse, this));
            return root;
		}
    }
}
