using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class StateKernel
    {
        private List<Item> items;

        public int Size { get { return items.Count; } }
        public ICollection<Item> Items { get { return items; } }

        public StateKernel()
        {
            items = new List<Item>();
        }

        public void AddItem(Item Item)
        {
            if (!items.Contains(Item))
                items.Add(Item);
        }
        public bool ContainsItem(Item Item) { return items.Contains(Item); }

        public State GetClosure()
        {
            // The set's items
            List<Item> Closure = new List<Item>(items);
            // Close the set
            for (int i = 0; i != Closure.Count; i++)
                Closure[i].CloseTo(Closure);
            return new State(this, Closure);
        }

        public bool Equals(StateKernel Kernel)
        {
            if (items.Count != Kernel.items.Count)
                return false;
            foreach (Item Item in items)
            {
                if (!Kernel.items.Contains(Item))
                    return false;
            }
            return true;
        }
        public override bool Equals(object obj)
        {
            if (obj is StateKernel)
                return Equals((StateKernel)obj);
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }


    interface StateAction
    {
        ItemAction ActionType { get; }
        Symbol OnSymbol { get; }
    }
    
    class StateActionShift : StateAction
    {
        private Symbol symbol;
        private State childSet;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public Symbol OnSymbol { get { return symbol; } }
        public State ChildSet { get { return childSet; } }

        public StateActionShift(Symbol Lookahead, State ChildSet)
        {
            symbol = Lookahead;
            childSet = ChildSet;
        }
    }

    class StateActionReduce : StateAction
    {
        protected Terminal lookahead;
        protected CFRule toReduce;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public Symbol OnSymbol { get { return lookahead; } }
        public Terminal Lookahead { get { return lookahead; } }
        public CFRule ToReduceRule { get { return toReduce; } }

        public StateActionReduce(Terminal Lookahead, CFRule ToReduce)
        {
            lookahead = Lookahead;
            toReduce = ToReduce;
        }
    }

    abstract class StateReductions : List<StateActionReduce>
    {
        protected List<Conflict> conflicts;
        public ICollection<Conflict> Conflicts { get { return conflicts; } }

        public StateReductions() { conflicts = new List<Conflict>(); }

        public abstract TerminalSet ExpectedTerminals { get; }
        public abstract void Build(State Set);
    }
    
    class State
    {
        protected int iD;
        protected StateKernel kernel;
        protected List<Item> items;
        protected Dictionary<Symbol, State> children;
        protected StateReductions reductions;
        
        public int ID
        {
            get { return iD; }
            set { iD = value; }
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
            children = new Dictionary<Symbol, State>();
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
        

        public bool Equals(State Set)
        {
            return (kernel.Equals(Set.kernel));
        }
        public override bool Equals(object obj)
        {
            if (obj is State)
                return Equals((State)obj);
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("I");
            Builder.Append(iD.ToString());
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
