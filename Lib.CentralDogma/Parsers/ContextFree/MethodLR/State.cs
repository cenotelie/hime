/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Hime.Parsers.ContextFree.LR
{
    public class State
    {
        protected StateKernel kernel;
        protected List<Item> items;
        protected Dictionary<GrammarSymbol, State> children;
        protected StateReductions reductions;
        
		// TODO: at least set should be protected!!!!
        public int ID
        {
            get;
            set;
        }
		
        public StateKernel Kernel { get { return kernel; } }
        public StateReductions Reductions { get { return reductions; } }
        public ICollection<Item> Items { get { return items; } }
        public Dictionary<GrammarSymbol, State> Children { get { return children; } }
        public ICollection<Conflict> Conflicts { get { return reductions.Conflicts; } }
        
        public State(StateKernel Kernel, List<Item> Items)
        {
            kernel = Kernel;
            items = Items;
            children = new Dictionary<GrammarSymbol, State>(GrammarSymbol.Comparer.Instance);
        }

        public void BuildGraph(Graph Graph)
        {
            // Shift dictionnary for the current set
            Dictionary<GrammarSymbol, StateKernel> Shifts = new Dictionary<GrammarSymbol, StateKernel>();
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
            foreach (GrammarSymbol Next in Shifts.Keys)
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
