using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class StateKernel
    {
        private Dictionary<CFRule, Dictionary<int, List<Item>>> dictItems;
        private List<Item> items;

        public int Size { get { return items.Count; } }
        public ICollection<Item> Items { get { return items; } }

        public StateKernel()
        {
            dictItems = new Dictionary<CFRule, Dictionary<int, List<Item>>>(CFRule.Comparer.Instance);
            items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            if (!dictItems.ContainsKey(item.BaseRule))
                dictItems.Add(item.BaseRule, new Dictionary<int, List<Item>>());
            Dictionary<int, List<Item>> sub = dictItems[item.BaseRule];
            if (!sub.ContainsKey(item.DotPosition))
                sub.Add(item.DotPosition, new List<Item>());
            sub[item.DotPosition].Add(item);
            items.Add(item);
        }
        public bool ContainsItem(Item item) {
            if (!dictItems.ContainsKey(item.BaseRule))
                return false;
            Dictionary<int, List<Item>> sub = dictItems[item.BaseRule];
            if (!sub.ContainsKey(item.DotPosition))
                return false;
            return sub[item.DotPosition].Contains(item);
        }

        public State GetClosure()
        {
            // The set's items
            Dictionary<CFRule, Dictionary<int, List<Item>>> map = new Dictionary<CFRule, Dictionary<int, List<Item>>>(CFRule.Comparer.Instance);
            foreach (CFRule rule in dictItems.Keys)
            {
                Dictionary<int, List<Item>> clone = new Dictionary<int, List<Item>>();
                Dictionary<int, List<Item>> original = dictItems[rule];
                map.Add(rule, clone);
                foreach (int position in original.Keys)
                {
                    List<Item> list = new List<Item>(original[position]);
                    clone.Add(position, list);
                }
            }
            List<Item> closure = new List<Item>(items);
            // Close the set
            for (int i = 0; i != closure.Count; i++)
                closure[i].CloseTo(closure, map);
            return new State(this, closure);
        }

        public bool Equals(StateKernel kernel)
        {
            if (this.items.Count != kernel.items.Count)
                return false;
            if (this.dictItems.Count != kernel.dictItems.Count)
                return false;
            foreach (CFRule rule in this.dictItems.Keys)
            {
                if (!kernel.dictItems.ContainsKey(rule))
                    return false;
                Dictionary<int, List<Item>> left = this.dictItems[rule];
                Dictionary<int, List<Item>> right = kernel.dictItems[rule];
                if (left.Count != right.Count)
                    return false;
                foreach (int position in left.Keys)
                {
                    if (!right.ContainsKey(position))
                        return false;
                    List<Item> l1 = left[position];
                    List<Item> l2 = right[position];
                    if (l1.Count != l2.Count)
                        return false;
                    foreach (Item item in l1)
                        if (!l2.Contains(item))
                            return false;
                }
            }
            return true;
        }
        public override bool Equals(object obj) { return Equals(obj as StateKernel); }
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

    abstract class StateReductions
    {
    	private List<StateActionReduce> content;
        protected List<Conflict> conflicts;
        public ICollection<Conflict> Conflicts { get { return conflicts; } }

        public StateReductions() 
        {
        	this.content = new List<StateActionReduce>();
        	this.conflicts = new List<Conflict>(); 
        }

        public abstract TerminalSet ExpectedTerminals { get; }
        public abstract void Build(State Set);
        
        public void Add(StateActionReduce action)
        {
        	this.content.Add(action);
        }
        
        public IEnumerator<StateActionReduce> GetEnumerator()
        {
        	return this.content.GetEnumerator();
        }
        
        public int Count { get { return this.content.Count; } }
        
        public StateActionReduce this[int index]
        {
        	get { return this.content[index]; }
        }
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
            Builder.Append(iD.ToString("X"));
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
