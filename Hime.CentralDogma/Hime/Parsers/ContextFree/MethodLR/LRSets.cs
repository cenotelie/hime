using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetKernel
    {
        private List<Item> items;

        public int Size { get { return items.Count; } }
        public ICollection<Item> Items { get { return items; } }

        public ItemSetKernel()
        {
            items = new List<Item>();
        }

        public void AddItem(Item Item)
        {
            if (!items.Contains(Item))
                items.Add(Item);
        }
        public bool ContainsItem(Item Item) { return items.Contains(Item); }

        public ItemSet GetClosure()
        {
            // The set's items
            List<Item> Closure = new List<Item>(items);
            // Close the set
            for (int i = 0; i != Closure.Count; i++)
                Closure[i].CloseTo(Closure);
            return new ItemSet(this, Closure);
        }

        public bool Equals(ItemSetKernel Kernel)
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
            if (obj is ItemSetKernel)
                return Equals((ItemSetKernel)obj);
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }





    interface ItemSetAction
    {
        ItemAction ActionType { get; }
        Symbol OnSymbol { get; }
    }
    
    class ItemSetActionShift : ItemSetAction
    {
        private Symbol symbol;
        private ItemSet childSet;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public Symbol OnSymbol { get { return symbol; } }
        public ItemSet ChildSet { get { return childSet; } }

        public ItemSetActionShift(Symbol Lookahead, ItemSet ChildSet)
        {
            symbol = Lookahead;
            childSet = ChildSet;
        }
    }

    class ItemSetActionReduce : ItemSetAction
    {
        protected Terminal lookahead;
        protected CFRule toReduce;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public Symbol OnSymbol { get { return lookahead; } }
        public Terminal Lookahead { get { return lookahead; } }
        public CFRule ToReduceRule { get { return toReduce; } }

        public ItemSetActionReduce(Terminal Lookahead, CFRule ToReduce)
        {
            lookahead = Lookahead;
            toReduce = ToReduce;
        }
    }

    


    abstract class ItemSetReductions
    {
        protected List<Conflict> conflicts;

        public ICollection<Conflict> Conflicts { get { return conflicts; } }

        public ItemSetReductions() { conflicts = new List<Conflict>(); }

        public abstract ICollection<ItemSetActionReduce> Reductions { get; }
        public abstract TerminalSet ExpectedTerminals { get; }

        public abstract void Build(ItemSet Set);
    }

    
    class ItemSet
    {
        protected int iD;
        protected ItemSetKernel kernel;
        protected List<Item> items;
        protected Dictionary<Symbol, ItemSet> children;
        protected ItemSetReductions reductions;
        
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public ItemSetKernel Kernel { get { return kernel; } }
        public ItemSetReductions Reductions { get { return reductions; } }
        public ICollection<Item> Items { get { return items; } }
        public Dictionary<Symbol, ItemSet> Children { get { return children; } }
        public ICollection<Conflict> Conflicts { get { return reductions.Conflicts; } }
        
        public ItemSet(ItemSetKernel Kernel, List<Item> Items)
        {
            kernel = Kernel;
            items = Items;
            children = new Dictionary<Symbol, ItemSet>();
        }

        public void BuildGraph(Graph Graph)
        {
            // Shift dictionnary for the current set
            Dictionary<Symbol, ItemSetKernel> Shifts = new Dictionary<Symbol, ItemSetKernel>();
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
                    ItemSetKernel Kernel = new ItemSetKernel();
                    Kernel.AddItem(Item.GetChild());
                    Shifts.Add(Item.NextSymbol, Kernel);
                }
            }
            // Close the children and add them to the graph
            foreach (Symbol Next in Shifts.Keys)
            {
                ItemSetKernel Kernel = Shifts[Next];
                ItemSet Child = Graph.ContainsSet(Kernel);
                if (Child == null)
                {
                    Child = Kernel.GetClosure();
                    Graph.Add(Child);
                }
                children.Add(Next, Child);
            }
        }
        public void BuildReductions(ItemSetReductions Reductions)
        {
            reductions = Reductions;
            reductions.Build(this);
        }
        

        public bool Equals(ItemSet Set)
        {
            return (kernel.Equals(Set.kernel));
        }
        public override bool Equals(object obj)
        {
            if (obj is ItemSet)
                return Equals((ItemSet)obj);
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
