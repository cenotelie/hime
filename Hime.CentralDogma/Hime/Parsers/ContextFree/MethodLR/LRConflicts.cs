using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    enum ConflictType
    {
        ShiftReduce,
        ReduceReduce,
        None
    }

    class Conflict : Hime.Kernel.Reporting.Entry
    {
        private string component;
        private State state;
        private ConflictType type;
        private Terminal lookahead;
        private List<Item> items;
        private List<Terminal> inputSample;
        private bool isError;

        public Hime.Kernel.Reporting.Level Level {
            get
            {
                if (isError) return Kernel.Reporting.Level.Error;
                return Kernel.Reporting.Level.Warning;
            }
        }
        public string Component { get { return component; } }
        public State State { get { return state; } }
        public string Message { get { return ToString(); } }
        public ConflictType ConflictType { get { return type; } }
        public Terminal ConflictSymbol { get { return lookahead; } }
        public ICollection<Item> Items { get { return items; } }
        public List<Terminal> InputSample
        {
            get { return inputSample; }
            set { inputSample = value; }
        }
        public bool IsError
        {
            get { return isError; }
            set { isError = value; }
        }

        public Conflict(string component, State state, ConflictType type, Terminal lookahead)
        {
            this.component = component;
            this.state = state;
            this.type = type;
            this.lookahead = lookahead;
            this.isError = true;
            items = new List<Item>();
        }
        public Conflict(string component, State state, ConflictType type)
        {
            this.component = component;
            this.state = state;
            this.type = type;
            this.isError = true;
            items = new List<Item>();
        }

        public void AddItem(Item Item) { items.Add(Item); }
        public bool ContainsItem(Item Item) { return items.Contains(Item); }

        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Conflict ");
            if (type == ConflictType.ShiftReduce)
                Builder.Append("Shift/Reduce");
            else
                Builder.Append("Reduce/Reduce");
            Builder.Append(" in ");
            Builder.Append(state.ID.ToString("X"));
            if (lookahead != null)
            {
                Builder.Append(" on terminal '");
                Builder.Append(lookahead.ToString());
                Builder.Append("'");
            }
            Builder.Append(" for items {");
            foreach (Item Item in items)
            {
                Builder.Append(" ");
                Builder.Append(Item.ToString());
                Builder.Append(" ");
            }
            Builder.Append("}");
            if (inputSample != null)
            {
                Builder.Append("\nInput example:");
                foreach (Terminal t in inputSample)
                {
                    Builder.Append(" ");
                    Builder.Append(t.ToString());
                }
                Builder.Append(" ");
                Builder.Append(Item.dot);
                Builder.Append(" ");
                Builder.Append(lookahead.ToString());
            }
            return Builder.ToString();
        }
    }


    class ConflictExamplifier
    {
        private Graph graph;
        private Dictionary<int, Dictionary<Symbol, List<State>>> inverseGraph;

        public ConflictExamplifier(Graph graph)
        {
            this.graph = graph;
            this.inverseGraph = new Dictionary<int, Dictionary<Symbol, List<State>>>();
            BuildInverse();
        }

        private void BuildInverse()
        {
            foreach (State set in graph.Sets)
            {
                foreach (Symbol symbol in set.Children.Keys)
                {
                    State child = set.Children[symbol];
                    if (!inverseGraph.ContainsKey(child.ID))
                        inverseGraph.Add(child.ID, new Dictionary<Symbol, List<State>>());
                    Dictionary<Symbol, List<State>> inverses = inverseGraph[child.ID];
                    if (!inverses.ContainsKey(symbol))
                        inverses.Add(symbol, new List<State>());
                    List<State> parents = inverses[symbol];
                    parents.Add(set);
                }
            }
        }

        

        public List<Terminal> GetExample(State state)
        {
            ICollection<Symbol> path = GetPath(state);
            List<Terminal> sample = new List<Terminal>();
            foreach (Symbol s in path)
            {
                if (s is Terminal)
                    sample.Add(s as Terminal);
                else
                    BuildSample(sample, s as CFVariable, new Stack<CFRuleDefinition>());
            }
            return sample;
        }

        private void BuildSample(List<Terminal> sample, CFVariable var, Stack<CFRuleDefinition> stack)
        {
            if (var.Firsts.Contains(TerminalEpsilon.Instance))
                return;
            List<CFRuleDefinition> definitions = new List<CFRuleDefinition>();
            foreach (CFRule rule in var.Rules)
                definitions.Add(rule.Definition.GetChoiceAtIndex(0));
            //definitions.Sort(new System.Comparison<CFRuleDefinition>(CompareDefinition));
            CFRuleDefinition def = null;
            foreach (CFRuleDefinition d in definitions)
            {
                if (!stack.Contains(d))
                {
                    def = d;
                    break;
                }
            }
            if (def == null)
                def = definitions[0];
            stack.Push(def);
            foreach (RuleDefinitionPart part in def.Parts)
            {
                if (part.Symbol is Terminal)
                    sample.Add(part.Symbol as Terminal);
                else
                    BuildSample(sample, part.Symbol as CFVariable, stack);
            }
            stack.Pop();
        }

        private int CompareDefinition(CFRuleDefinition left, CFRuleDefinition right) { return left.Length - right.Length; }


        private class ENode
        {
            public State state;
            public ENode next;
            public Symbol transition;
            public ENode(State state, ENode next, Symbol transition)
            {
                this.state = state;
                this.next = next;
                this.transition = transition;
            }
        }

        private ICollection<Symbol> GetPath(State state)
        {
            LinkedList<ENode> queue = new LinkedList<ENode>();
            queue.AddFirst(new ENode(state, null, null));
            ENode goal = null;
            while (goal == null)
            {
                ENode current = queue.First.Value;
                queue.RemoveFirst();
                if (!inverseGraph.ContainsKey(current.state.ID))
                    continue;
                Dictionary<Symbol, List<State>> transitions = inverseGraph[current.state.ID];
                foreach (Symbol s in transitions.Keys)
                {
                    foreach (State previous in transitions[s])
                    {
                        if (previous.ID == 0)
                        {
                            goal = new ENode(previous, current, s);
                            break;
                        }
                        else
                            queue.AddLast(new ENode(previous, current, s));
                    }
                }
            }
            LinkedList<Symbol> result = new LinkedList<Symbol>();
            while (goal.next != null)
            {
                result.AddLast(goal.transition);
                goal = goal.next;
            }
            return result;
        }
    }
}
