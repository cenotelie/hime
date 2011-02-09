namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents the kernel items for a set of items
    /// </summary>
    public class LRItemSetKernel
    {
        /// <summary>
        /// List of LR items
        /// </summary>
        private System.Collections.Generic.List<LRItem> p_Items;

        /// <summary>
        /// Get the kernel size
        /// </summary>
        /// <value>The kernel size</value>
        public int Size { get { return p_Items.Count; } }
        /// <summary>
        /// Get an enumeration of the kernel items
        /// </summary>
        /// <value>An enumeration of the kernel items</value>
        public System.Collections.Generic.IEnumerable<LRItem> Items { get { return p_Items; } }

        /// <summary>
        /// Construct the kernel
        /// </summary>
        public LRItemSetKernel()
        {
            p_Items = new System.Collections.Generic.List<LRItem>();
        }

        /// <summary>
        /// Add an item to the kernl
        /// </summary>
        /// <param name="Item">The item to add</param>
        /// <remarks>The item is not added if another one with the same value is already present</remarks>
        public void AddItem(LRItem Item)
        {
            if (!p_Items.Contains(Item))
                p_Items.Add(Item);
        }
        /// <summary>
        /// Check if the given item is in the kernel
        /// </summary>
        /// <param name="Item">The tested item</param>
        /// <returns>Returns true if the item is present, false otherwise</returns>
        public bool ContainsItem(LRItem Item) { return p_Items.Contains(Item); }

        /// <summary>
        /// Construct the closure for the current Kernel
        /// </summary>
        /// <returns>Returns the item's set corresponding to the closure</returns>
        public LRItemSet GetClosure()
        {
            // The set's items
            System.Collections.Generic.List<LRItem> Closure = new System.Collections.Generic.List<LRItem>(p_Items);
            // Close the set
            for (int i = 0; i != Closure.Count; i++)
                Closure[i].CloseTo(Closure);
            return new LRItemSet(this, Closure);
        }

        /// <summary>
        /// Test if two kernels are equals
        /// </summary>
        /// <param name="Kernel">The tested item</param>
        /// <returns>Returns true if the two kernels have the same items, false otherwise</returns>
        public bool Equals(LRItemSetKernel Kernel)
        {
            if (p_Items.Count != Kernel.p_Items.Count)
                return false;
            foreach (LRItem Item in p_Items)
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
            if (obj is LRItemSetKernel)
                return Equals((LRItemSetKernel)obj);
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
    public interface ILRItemSetAction
    {
        /// <summary>
        /// Get the action type
        /// </summary>
        /// <value>The action type</value>
        LRItemAction ActionType { get; }
        /// <summary>
        /// Get the lookahead needed for the action to be taken
        /// </summary>
        /// <value>The lookahead for the action</value>
        Symbol OnSymbol { get; }
        /// <summary>
        /// Get a XML node representing the action
        /// </summary>
        /// <param name="Doc">The parent document</param>
        /// <param name="Rules">A list of the grammar rules</param>
        /// <returns>The XMl node</returns>
        System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc, System.Collections.Generic.List<CFRule> Rules);
    }
    
    /// <summary>
    /// Represent a shifting action for a set of items
    /// </summary>
    public class LRItemSetActionShift : ILRItemSetAction
    {
        /// <summary>
        /// The lookahead needed for the action to be taken
        /// </summary>
        private Symbol p_Symbol;
        /// <summary>
        /// The child set attained after the shift transition
        /// </summary>
        private LRItemSet p_ChildSet;

        /// <summary>
        /// Get the action type
        /// </summary>
        /// <value>The action type</value>
        public LRItemAction ActionType { get { return LRItemAction.Shift; } }
        /// <summary>
        /// Get the lookahead needed for the action to be taken
        /// </summary>
        /// <value>The lookahead for the action</value>
        public Symbol OnSymbol { get { return p_Symbol; } }
        /// <summary>
        /// Get the child set attained after the shift transition
        /// </summary>
        /// <value>the child set</value>
        public LRItemSet ChildSet { get { return p_ChildSet; } }

        /// <summary>
        /// Constructs the action
        /// </summary>
        /// <param name="OnSymbol">The lookahead</param>
        /// <param name="ChildSet">The child set</param>
        public LRItemSetActionShift(Symbol Lookahead, LRItemSet ChildSet)
        {
            p_Symbol = Lookahead;
            p_ChildSet = ChildSet;
        }

        /// <summary>
        /// Get a XML node representing the action
        /// </summary>
        /// <param name="Doc">The parent document</param>
        /// <param name="Rules">A list of the grammar rules</param>
        /// <returns>The XMl node</returns>
        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc, System.Collections.Generic.List<CFRule> Rules) { return BuildXMLNode(Doc, p_Symbol, p_ChildSet); }

        /// <summary>
        /// Build a XML node representing a shift transition
        /// </summary>
        /// <param name="Doc">The parent document</param>
        /// <param name="Symbol">The matched symbol</param>
        /// <param name="Next">The next set of items</param>
        /// <returns>The XML node</returns>
        public static System.Xml.XmlNode BuildXMLNode(System.Xml.XmlDocument Doc, Symbol Symbol, LRItemSet Next)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Shift");
            Node.Attributes.Append(Doc.CreateAttribute("Symbol"));
            Node.Attributes.Append(Doc.CreateAttribute("Next"));
            Node.Attributes["Symbol"].Value = Symbol.SID.ToString("X");
            Node.Attributes["Next"].Value = Next.ID.ToString("X");
            return Node;
        }
    }

    /// <summary>
    /// Represent a reduction action for a set of items
    /// </summary>
    public class LRItemSetActionReduce : ILRItemSetAction
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
        public LRItemAction ActionType { get { return LRItemAction.Shift; } }
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
        public LRItemSetActionReduce(Terminal Lookahead, CFRule ToReduce)
        {
            p_Lookahead = Lookahead;
            p_ToReduce = ToReduce;
        }

        /// <summary>
        /// Get a XML node representing the action
        /// </summary>
        /// <param name="Doc">The parent document</param>
        /// <param name="Rules">A list of the grammar rules</param>
        /// <returns>The XML node</returns>
        public virtual System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc, System.Collections.Generic.List<CFRule> Rules)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Reduction");
            Node.Attributes.Append(Doc.CreateAttribute("Symbol"));
            Node.Attributes.Append(Doc.CreateAttribute("Index"));
            Node.Attributes["Symbol"].Value = p_Lookahead.SID.ToString("X");
            Node.Attributes["Index"].Value = Rules.IndexOf(p_ToReduce).ToString("X");
            return Node;
        }
    }

    


    /// <summary>
    /// Common interface for objects representing the actions for a set of items
    /// </summary>
    public interface ILRItemSetActions
    {
        /// <summary>
        /// Get an enumeration of the conflicts within the set
        /// </summary>
        /// <value>An enumeration of the conflicts within the set</value>
        System.Collections.Generic.IEnumerable<LRConflict> Conflicts { get; }

        /// <summary>
        /// Build the actions for the given set of items
        /// </summary>
        /// <param name="Set">The set of items</param>
        void Build(LRItemSet Set);

        /// <summary>
        /// Get the terminals associated with some action
        /// </summary>
        /// <returns>Returns a set of all the expected terminal</returns>
        TerminalSet GetExpectedTerminals();

        /// <summary>
        /// Get the XML node representing the current set
        /// </summary>
        /// <param name="Set">The set of items</param>
        /// <param name="Doc">Parent XML document</param>
        /// <param name="Rules">List of the grammar rules</param>
        /// <returns>Returns the XML node</returns>
        System.Xml.XmlNode GetXMLNode(LRItemSet Set, System.Xml.XmlDocument Doc, System.Collections.Generic.List<CFRule> Rules);
    }

    
    /// <summary>
    /// Represents a set of LR items
    /// </summary>
    public class LRItemSet : Kernel.Graph.IVertexData
    {
        /// <summary>
        /// Unique identifier for the current set
        /// </summary>
        protected int p_ID;
        /// <summary>
        /// Set's kernel
        /// </summary>
        protected LRItemSetKernel p_Kernel;
        /// <summary>
        /// List of the contained items
        /// </summary>
        protected System.Collections.Generic.List<LRItem> p_Items;
        /// <summary>
        /// Dictionnary associating children sets to symbols : represents transitions in the graph
        /// </summary>
        protected System.Collections.Generic.Dictionary<Symbol, LRItemSet> p_Children;
        /// <summary>
        /// Actions for the current set
        /// </summary>
        protected ILRItemSetActions p_Actions;
        
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
        public LRItemSetKernel Kernel { get { return p_Kernel; } }
        /// <summary>
        /// Get the actions for the current set
        /// </summary>
        /// <value>Actions for the current set</value>
        public ILRItemSetActions Actions { get { return p_Actions; } }
        /// <summary>
        /// Get the items in the current set
        /// </summary>
        /// <value>An enumeration of the items in the set</value>
        public System.Collections.Generic.IEnumerable<LRItem> Items { get { return p_Items; } }
        /// <summary>
        /// Get a dictionnary representing transitions
        /// </summary>
        /// <value>A dictionnary representing the transitions</value>
        public System.Collections.Generic.Dictionary<Symbol, LRItemSet> Children { get { return p_Children; } }
        /// <summary>
        /// Get an enumeration of the conflicts within the set
        /// </summary>
        /// <value>An enumeration of the conflicts within the set</value>
        public System.Collections.Generic.IEnumerable<LRConflict> Conflicts { get { return p_Actions.Conflicts; } }
        
        /// <summary>
        /// Constructs the set from its kernel and a list of items
        /// </summary>
        /// <param name="Kernel">Set's kernel</param>
        /// <param name="Items">Set's items</param>
        public LRItemSet(LRItemSetKernel Kernel, System.Collections.Generic.List<LRItem> Items)
        {
            p_Kernel = Kernel;
            p_Items = Items;
            p_Children = new System.Collections.Generic.Dictionary<Symbol, LRItemSet>();
        }

        /// <summary>
        /// Build the current set in the graph
        /// </summary>
        /// <param name="Graph">The LR graph to be completed</param>
        /// <remarks>This function build the children sets and add them to the graph</remarks>
        public void BuildGraph(LRGraph Graph)
        {
            // Shift dictionnary for the current set
            System.Collections.Generic.Dictionary<Symbol, LRItemSetKernel> Shifts = new System.Collections.Generic.Dictionary<Symbol, LRItemSetKernel>();
            // Build the children kernels from the shift actions
            foreach (LRItem Item in p_Items)
            {
                // Ignore reduce actions
                if (Item.Action == LRItemAction.Reduce)
                    continue;

                if (Shifts.ContainsKey(Item.NextSymbol))
                    Shifts[Item.NextSymbol].AddItem(Item.GetChild());
                else
                {
                    LRItemSetKernel Kernel = new LRItemSetKernel();
                    Kernel.AddItem(Item.GetChild());
                    Shifts.Add(Item.NextSymbol, Kernel);
                }
            }
            // Close the children and add them to the graph
            foreach (Symbol Next in Shifts.Keys)
            {
                LRItemSetKernel Kernel = Shifts[Next];
                LRItemSet Child = Graph.ContainsSet(Kernel);
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
        /// <param name="Actions">The actions to built</param>
        /// <remarks>This function does not build the children sets</remarks>
        public void BuildActions(ILRItemSetActions Actions)
        {
            p_Actions = Actions;
            p_Actions.Build(this);
        }
        

        public Hime.Kernel.Graph.Vertex CreateVertex(float LineSize, float CharacterWidth)
        {
            Hime.Kernel.Graph.Vertex Vertex = new Hime.Kernel.Graph.Vertex(this);
            float Height = (p_Items.Count + 1) * LineSize;
            int Length = 0;
            foreach (LRItem Item in p_Items)
            {
                string Value = Item.ToString(true);
                if (Value.Length > Length)
                    Length = Value.Length;
            }
            float Width = Length * CharacterWidth;
            Vertex.Visual = new Hime.Kernel.Graph.VertexVisualRectangle(Width, Height);
            return Vertex;
        }
        public void DrawContent(Hime.Kernel.Graph.VertexVisual Visual, Hime.Kernel.Graph.GraphVisual Material, float LineHeight)
        {
            float X = Visual.X;
            float Y = Visual.Y;
            Material.Graphic.DrawString("I" + p_ID.ToString(), Material.FontNormal, new System.Drawing.SolidBrush(System.Drawing.Color.Black), X, Y);
            Material.Graphic.DrawLine(Visual.MaterialContourPen, Visual.X, Visual.Y + LineHeight, Visual.X + Visual.BoundingWidth, Visual.Y + LineHeight);
            Y += LineHeight;
            foreach (LRItem Item in p_Items)
            {
                System.Drawing.Brush Brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                foreach (LRConflict Conflict in p_Actions.Conflicts)
                {
                    if (Conflict.ContainsItem(Item))
                    {
                        Brush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                        break;
                    }
                }
                Material.Graphic.DrawString(Item.ToString(true), Material.FontNormal, Brush, X, Y);
                Y += LineHeight;
            }
        }

        /// <summary>
        /// Get the XML node representing the current set
        /// </summary>
        /// <param name="Doc">Parent XML document</param>
        /// <param name="Rules">List of the grammar rules</param>
        /// <returns>Returns the XML node</returns>
        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc, System.Collections.Generic.List<CFRule> Rules)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("State");
            Node.Attributes.Append(Doc.CreateAttribute("ID"));
            Node.Attributes[0].Value = p_ID.ToString("X");

            // Generate data for items in the current set
            Node.AppendChild(Doc.CreateElement("Items"));
            foreach (LRItem Item in p_Items)
            {
                Node.LastChild.AppendChild(Doc.CreateElement("Item"));
                Node.LastChild.LastChild.InnerText = Item.ToString();
            }

            // Generate data for expected terminals
            TerminalSet Terminals = p_Actions.GetExpectedTerminals();
            foreach (Symbol Symbol in p_Children.Keys)
            {
                if (Symbol is Terminal)
                    Terminals.Add((Terminal)Symbol);
            }
            Node.AppendChild(Doc.CreateElement("Expected"));
            foreach (Terminal Terminal in Terminals)
            {
                Node.LastChild.AppendChild(Doc.CreateElement("Symbol"));
                Node.LastChild.LastChild.Attributes.Append(Doc.CreateAttribute("SID"));
                Node.LastChild.LastChild.Attributes.Append(Doc.CreateAttribute("Name"));
                Node.LastChild.LastChild.Attributes["SID"].Value = Terminal.SID.ToString("X");
                Node.LastChild.LastChild.Attributes["Name"].Value = Terminal.LocalName;
            }

            // Append actions data
            Node.AppendChild(p_Actions.GetXMLNode(this, Doc, Rules));
            return Node;
        }

        /// <summary>
        /// Compare two sets to determine equality
        /// </summary>
        /// <param name="Set">The set to compare</param>
        /// <returns>Returns true if the two sets are equal, false otherwise</returns>
        public bool Equals(LRItemSet Set)
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
            if (obj is LRItemSet)
                return Equals((LRItemSet)obj);
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
            foreach (LRItem Item in p_Items)
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