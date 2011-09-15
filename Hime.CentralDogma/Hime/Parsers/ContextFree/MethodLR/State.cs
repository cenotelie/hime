/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    public class State
    {
        protected int id;
        protected StateKernel kernel;
        protected List<Item> items;
        protected Dictionary<Symbol, State> children;
        protected StateReductions reductions;
        
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public StateKernel Kernel { get { return kernel; } }
        public StateReductions Reductions { get { return reductions; } }
        public ICollection<Item> Items { get { return items; } }
        public Dictionary<Symbol, State> Children { get { return children; } }
        public ICollection<Conflict> Conflicts { get { return reductions.Conflicts; } }
        
        public State(StateKernel Kernel, List<Item> Items)
        {
            kernel = Kernel;
            items = Items;
            children = new Dictionary<Symbol, State>(Symbol.Comparer.Instance);
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
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("S ");
            Builder.Append(id.ToString("X"));
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
    }
}
