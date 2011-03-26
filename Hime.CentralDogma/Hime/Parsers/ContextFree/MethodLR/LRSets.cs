using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetKernel
    {
        private List<Item> p_Items;

        public int Size { get { return p_Items.Count; } }
        public ICollection<Item> Items { get { return p_Items; } }

        public ItemSetKernel()
        {
            p_Items = new List<Item>();
        }

        public void AddItem(Item Item)
        {
            if (!p_Items.Contains(Item))
                p_Items.Add(Item);
        }
        public bool ContainsItem(Item Item) { return p_Items.Contains(Item); }

        public ItemSet GetClosure()
        {
            // The set's items
            List<Item> Closure = new List<Item>(p_Items);
            // Close the set
            for (int i = 0; i != Closure.Count; i++)
                Closure[i].CloseTo(Closure);
            return new ItemSet(this, Closure);
        }

        public bool Equals(ItemSetKernel Kernel)
        {
            if (p_Items.Count != Kernel.p_Items.Count)
                return false;
            foreach (Item Item in p_Items)
            {
                if (!Kernel.p_Items.Contains(Item))
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
        private Symbol p_Symbol;
        private ItemSet p_ChildSet;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public Symbol OnSymbol { get { return p_Symbol; } }
        public ItemSet ChildSet { get { return p_ChildSet; } }

        public ItemSetActionShift(Symbol Lookahead, ItemSet ChildSet)
        {
            p_Symbol = Lookahead;
            p_ChildSet = ChildSet;
        }
    }

    class ItemSetActionReduce : ItemSetAction
    {
        protected Terminal p_Lookahead;
        protected CFRule p_ToReduce;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public Symbol OnSymbol { get { return p_Lookahead; } }
        public Terminal Lookahead { get { return p_Lookahead; } }
        public CFRule ToReduceRule { get { return p_ToReduce; } }

        public ItemSetActionReduce(Terminal Lookahead, CFRule ToReduce)
        {
            p_Lookahead = Lookahead;
            p_ToReduce = ToReduce;
        }
    }

    


    abstract class ItemSetReductions
    {
        protected List<Conflict> p_Conflicts;

        public ICollection<Conflict> Conflicts { get { return p_Conflicts; } }

        public ItemSetReductions() { p_Conflicts = new List<Conflict>(); }

        public abstract ICollection<ItemSetActionReduce> Reductions { get; }
        public abstract TerminalSet ExpectedTerminals { get; }

        public abstract void Build(ItemSet Set);
    }

    
    class ItemSet
    {
        protected int p_ID;
        protected ItemSetKernel p_Kernel;
        protected List<Item> p_Items;
        protected Dictionary<Symbol, ItemSet> p_Children;
        protected ItemSetReductions p_Reductions;
        
        public int ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }
        public ItemSetKernel Kernel { get { return p_Kernel; } }
        public ItemSetReductions Reductions { get { return p_Reductions; } }
        public ICollection<Item> Items { get { return p_Items; } }
        public Dictionary<Symbol, ItemSet> Children { get { return p_Children; } }
        public ICollection<Conflict> Conflicts { get { return p_Reductions.Conflicts; } }
        
        public ItemSet(ItemSetKernel Kernel, List<Item> Items)
        {
            p_Kernel = Kernel;
            p_Items = Items;
            p_Children = new Dictionary<Symbol, ItemSet>();
        }

        public void BuildGraph(Graph Graph)
        {
            // Shift dictionnary for the current set
            Dictionary<Symbol, ItemSetKernel> Shifts = new Dictionary<Symbol, ItemSetKernel>();
            // Build the children kernels from the shift actions
            foreach (Item Item in p_Items)
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
                p_Children.Add(Next, Child);
            }
        }
        public void BuildReductions(ItemSetReductions Reductions)
        {
            p_Reductions = Reductions;
            p_Reductions.Build(this);
        }
        

        public bool Equals(ItemSet Set)
        {
            return (p_Kernel.Equals(Set.p_Kernel));
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
            Builder.Append(p_ID.ToString());
            Builder.Append(" = {");
            foreach (Item Item in p_Items)
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
