namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents the kernel items for a set of items
    /// </summary>
    public class ItemSetKernel
    {
        /// <summary>
        /// List of LR items
        /// </summary>
        private System.Collections.Generic.List<Item> p_Items;

        /// <summary>
        /// Get the kernel size
        /// </summary>
        /// <value>The kernel size</value>
        public int Size { get { return p_Items.Count; } }
        /// <summary>
        /// Get an enumeration of the kernel items
        /// </summary>
        /// <value>An enumeration of the kernel items</value>
        public System.Collections.Generic.IEnumerable<Item> Items { get { return p_Items; } }

        /// <summary>
        /// Construct the kernel
        /// </summary>
        public ItemSetKernel()
        {
            p_Items = new System.Collections.Generic.List<Item>();
        }

        /// <summary>
        /// Add an item to the kernl
        /// </summary>
        /// <param name="Item">The item to add</param>
        /// <remarks>The item is not added if another one with the same value is already present</remarks>
        public void AddItem(Item Item)
        {
            if (!p_Items.Contains(Item))
                p_Items.Add(Item);
        }
        /// <summary>
        /// Check if the given item is in the kernel
        /// </summary>
        /// <param name="Item">The tested item</param>
        /// <returns>Returns true if the item is present, false otherwise</returns>
        public bool ContainsItem(Item Item) { return p_Items.Contains(Item); }

        /// <summary>
        /// Construct the closure for the current Kernel
        /// </summary>
        /// <returns>Returns the item's set corresponding to the closure</returns>
        public ItemSet GetClosure()
        {
            // The set's items
            System.Collections.Generic.List<Item> Closure = new System.Collections.Generic.List<Item>(p_Items);
            // Close the set
            for (int i = 0; i != Closure.Count; i++)
                Closure[i].CloseTo(Closure);
            return new ItemSet(this, Closure);
        }

        /// <summary>
        /// Test if two kernels are equals
        /// </summary>
        /// <param name="Kernel">The tested item</param>
        /// <returns>Returns true if the two kernels have the same items, false otherwise</returns>
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
        /// <summary>
        /// Test if two objects are equal
        /// </summary>
        /// <param name="obj">The tested object</param>
        /// <returns>Returns true if the tested object is a kernel and the two kernels have the same items, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is ItemSetKernel)
                return Equals((ItemSetKernel)obj);
            return false;
        }
        /// <summary>
        /// Get the hash code for this object
        /// </summary>
        /// <returns>Returns the value from the base function</returns>
        public override int GetHashCode() { return base.GetHashCode(); }
    }





    /// <summary>
    /// Common interface for objects representing actions for a set of items
    /// </summary>
    public interface ItemSetAction
    {
        /// <summary>
        /// Get the action type
        /// </summary>
        /// <value>The action type</value>
        ItemAction ActionType { get; }
        /// <summary>
        /// Get the lookahead needed for the action to be taken
        /// </summary>
        /// <value>The lookahead for the action</value>
        Symbol OnSymbol { get; }
    }
    
    /// <summary>
    /// Represent a shifting action for a set of items
    /// </summary>
    public class ItemSetActionShift : ItemSetAction
    {
        /// <summary>
        /// The lookahead needed for the action to be taken
        /// </summary>
        private Symbol p_Symbol;
        /// <summary>
        /// The child set attained after the shift transition
        /// </summary>
        private ItemSet p_ChildSet;

        /// <summary>
        /// Get the action type
        /// </summary>
        /// <value>The action type</value>
        public ItemAction ActionType { get { return ItemAction.Shift; } }
        /// <summary>
        /// Get the lookahead needed for the action to be taken
        /// </summary>
        /// <value>The lookahead for the action</value>
        public Symbol OnSymbol { get { return p_Symbol; } }
        /// <summary>
        /// Get the child set attained after the shift transition
        /// </summary>
        /// <value>the child set</value>
        public ItemSet ChildSet { get { return p_ChildSet; } }

        /// <summary>
        /// Constructs the action
        /// </summary>
        /// <param name="OnSymbol">The lookahead</param>
        /// <param name="ChildSet">The child set</param>
        public ItemSetActionShift(Symbol Lookahead, ItemSet ChildSet)
        {
            p_Symbol = Lookahead;
            p_ChildSet = ChildSet;
        }
    }

    /// <summary>
    /// Represent a reduction action for a set of items
    /// </summary>
    public class ItemSetActionReduce : ItemSetAction
    {
        /// <summary>
        /// The lookahead needed for the action to be taken
        /// </summary>
        protected Terminal p_Lookahead;
        /// <summary>
        /// The rule to reduce
        /// </summary>
        protected CFRule p_ToReduce;

        /// <summary>
        /// Get the action type
        /// </summary>
        /// <value>The action type</value>
        public ItemAction ActionType { get { return ItemAction.Shift; } }
        /// <summary>
        /// Get the lookahead needed for the action to be taken
        /// </summary>
        /// <value>The lookahead for the action</value>
        public Symbol OnSymbol { get { return p_Lookahead; } }
        /// <summary>
        /// Get the lookahead needed for the action to be taken
        /// </summary>
        /// <value>The lookahead for the action as a terminal</value>
        public Terminal Lookahead { get { return p_Lookahead; } }
        /// <summary>
        /// Get the rule to reduce
        /// </summary>
        /// <value>The rule to reduce</value>
        public CFRule ToReduceRule { get { return p_ToReduce; } }

        /// <summary>
        /// Constructs the action
        /// </summary>
        /// <param name="OnSymbol">The lookahead</param>
        /// <param name="ToReduce">The rule to be reduced on lookahead</param>
        public ItemSetActionReduce(Terminal Lookahead, CFRule ToReduce)
        {
            p_Lookahead = Lookahead;
            p_ToReduce = ToReduce;
        }
    }

    


    /// <summary>
    /// Common interface for objects representing the actions for a set of items
    /// </summary>
    public abstract class ItemSetReductions
    {
        /// <summary>
        /// List of the conflicts within the set
        /// </summary>
        protected System.Collections.Generic.List<Conflict> p_Conflicts;

        /// <summary>
        /// Get an enumeration of the conflicts within the set
        /// </summary>
        /// <value>An enumeration of the conflicts within the set</value>
        public System.Collections.Generic.IEnumerable<Conflict> Conflicts { get { return p_Conflicts; } }

        public ItemSetReductions() { p_Conflicts = new System.Collections.Generic.List<Conflict>(); }

        public abstract System.Collections.Generic.IEnumerable<ItemSetAction> Actions { get; }
        public abstract TerminalSet ExpectedTerminals { get; }

        public abstract void Build(ItemSet Set);
    }

    
    /// <summary>
    /// Represents a set of LR items
    /// </summary>
    public class ItemSet
    {
        /// <summary>
        /// Unique identifier for the current set
        /// </summary>
        protected int p_ID;
        /// <summary>
        /// Set's kernel
        /// </summary>
        protected ItemSetKernel p_Kernel;
        /// <summary>
        /// List of the contained items
        /// </summary>
        protected System.Collections.Generic.List<Item> p_Items;
        /// <summary>
        /// Dictionnary associating children sets to symbols : represents transitions in the graph
        /// </summary>
        protected System.Collections.Generic.Dictionary<Symbol, ItemSet> p_Children;
        /// <summary>
        /// Actions for the current set
        /// </summary>
        protected ItemSetReductions p_Reductions;
        
        /// <summary>
        /// Get or Set the set's ID
        /// </summary>
        /// <value>The set's ID</value>
        public int ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }
        /// <summary>
        /// Get the kernel for the current set
        /// </summary>
        /// <value>The kernel for the current set</value>
        public ItemSetKernel Kernel { get { return p_Kernel; } }
        /// <summary>
        /// Get the actions for the current set
        /// </summary>
        /// <value>Actions for the current set</value>
        public ItemSetReductions Reductions { get { return p_Reductions; } }
        /// <summary>
        /// Get the items in the current set
        /// </summary>
        /// <value>An enumeration of the items in the set</value>
        public System.Collections.Generic.IEnumerable<Item> Items { get { return p_Items; } }
        /// <summary>
        /// Get a dictionnary representing transitions
        /// </summary>
        /// <value>A dictionnary representing the transitions</value>
        public System.Collections.Generic.Dictionary<Symbol, ItemSet> Children { get { return p_Children; } }
        /// <summary>
        /// Get an enumeration of the conflicts within the set
        /// </summary>
        /// <value>An enumeration of the conflicts within the set</value>
        public System.Collections.Generic.IEnumerable<Conflict> Conflicts { get { return p_Reductions.Conflicts; } }
        
        /// <summary>
        /// Constructs the set from its kernel and a list of items
        /// </summary>
        /// <param name="Kernel">Set's kernel</param>
        /// <param name="Items">Set's items</param>
        public ItemSet(ItemSetKernel Kernel, System.Collections.Generic.List<Item> Items)
        {
            p_Kernel = Kernel;
            p_Items = Items;
            p_Children = new System.Collections.Generic.Dictionary<Symbol, ItemSet>();
        }

        /// <summary>
        /// Build the current set in the graph
        /// </summary>
        /// <param name="Graph">The LR graph to be completed</param>
        /// <remarks>This function build the children sets and add them to the graph</remarks>
        public void BuildGraph(Graph Graph)
        {
            // Shift dictionnary for the current set
            System.Collections.Generic.Dictionary<Symbol, ItemSetKernel> Shifts = new System.Collections.Generic.Dictionary<Symbol, ItemSetKernel>();
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
        /// <summary>
        /// Build the current set with the given method
        /// </summary>
        /// <param name="Reductions">The actions to built</param>
        /// <remarks>This function does not build the children sets</remarks>
        public void BuildReductions(ItemSetReductions Reductions)
        {
            p_Reductions = Reductions;
            p_Reductions.Build(this);
        }
        

        /// <summary>
        /// Compare two sets to determine equality
        /// </summary>
        /// <param name="Set">The set to compare</param>
        /// <returns>Returns true if the two sets are equal, false otherwise</returns>
        public bool Equals(ItemSet Set)
        {
            return (p_Kernel.Equals(Set.p_Kernel));
        }
        /// <summary>
        /// Compare two objects to determine equality
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Returns true if the two objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is ItemSet)
                return Equals((ItemSet)obj);
            return false;
        }
        /// <summary>
        /// Get the object's hash code
        /// </summary>
        /// <returns>Returns the object's hash code</returns>
        public override int GetHashCode() { return base.GetHashCode(); }
        /// <summary>
        /// Get a string representation with lookaheads of the set
        /// </summary>
        /// <returns>Returns a string representation with lookaheads of the set</returns>
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