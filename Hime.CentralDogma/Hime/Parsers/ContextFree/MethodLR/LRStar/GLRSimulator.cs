using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class GLRStackNode
    {
        private State state;
        private Dictionary<Symbol, List<GLRStackNode>> previous;

        public State State { get { return state; } }
        public Dictionary<Symbol, List<GLRStackNode>> Previous { get { return previous; } }

        public GLRStackNode(State state)
        {
            this.state = state;
            this.previous = new Dictionary<Symbol, List<GLRStackNode>>();
        }

        public void AddPrevious(Symbol symbol, GLRStackNode node)
        {
            if (!previous.ContainsKey(symbol))
                previous.Add(symbol, new List<GLRStackNode>());
            List<GLRStackNode> nodes = previous[symbol];
            if (nodes.Contains(node))
                return;
            nodes.Add(node);
        }
    }

    class GLRSimulatorState
    {
        private List<GLRStackNode> nodes;
        public List<GLRStackNode> Nodes { get { return nodes; } }
        public GLRSimulatorState() { nodes = new List<GLRStackNode>(); }
        public GLRStackNode Add(State state)
        {
            foreach (GLRStackNode potential in nodes)
            {
                if (potential.State.ID == state.ID)
                    return potential;
            }
            GLRStackNode node = new GLRStackNode(state);
            nodes.Add(node);
            return node;
        }
        public GLRStackNode Add(GLRStackNode node)
        {
            if (nodes.Contains(node))
                return node;
            nodes.Add(node);
            return node;
        }
    }


    class GLRSimulator
    {
        private Graph graph;
        private Dictionary<int, Dictionary<Symbol, List<State>>> inverseGraph;

        public GLRSimulator(Graph graph)
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

        public GLRSimulatorState Simulate(GLRSimulatorState origin, Terminal lookahead)
        {
            // Nodes before reductions
            GLRSimulatorState before = new GLRSimulatorState();
            before.Nodes.AddRange(origin.Nodes);

            // Apply reductions
            for (int i = 0; i != before.Nodes.Count; i++)
            {
                GLRStackNode node = before.Nodes[i];
                foreach (StateActionReduce reduce in node.State.Reductions)
                {
                    GLRSimulatorState reduceOrigin = GetOrigin(node, reduce.ToReduceRule.Definition.GetChoiceAtIndex(0));
                    foreach (GLRStackNode nOrigin in reduceOrigin.Nodes)
                    {
                        if (nOrigin.State.Children.ContainsKey(reduce.ToReduceRule.Variable))
                        {
                            State next = nOrigin.State.Children[reduce.ToReduceRule.Variable];
                            GLRStackNode follower = before.Add(next);
                            follower.AddPrevious(reduce.ToReduceRule.Variable, nOrigin);
                        }
                    }
                }
            }

            // Apply shift
            GLRSimulatorState result = new GLRSimulatorState();
            foreach (GLRStackNode node in before.Nodes)
            {
                if (node.State.Children.ContainsKey(lookahead))
                {
                    State child = node.State.Children[lookahead];
                    GLRStackNode nChild = result.Add(child);
                    nChild.AddPrevious(lookahead, node);
                }
            }
            return result;
        }

        public GLRSimulatorState Simulate(State state, Item item, Terminal lookahead)
        {
            GLRSimulatorState pState = new GLRSimulatorState();
            GLRStackNode pNode = pState.Add(state);
            GLRSimulatorState result = new GLRSimulatorState();

            if (item.Action == ItemAction.Shift)
            {
                GLRStackNode next = result.Add(state.Children[item.NextSymbol]);
                next.AddPrevious(item.NextSymbol, pNode);
                return result;
            }

            GLRSimulatorState origin = GetOrigin(pState, item.BaseRule.Definition.GetChoiceAtIndex(0));
            foreach (GLRStackNode node in origin.Nodes)
            {
                if (node.State.Children.ContainsKey(item.BaseRule.Variable))
                {
                    State next = node.State.Children[item.BaseRule.Variable];
                    GLRStackNode nNode = result.Add(next);
                    nNode.AddPrevious(item.BaseRule.Variable, node);
                }
            }
            return Simulate(result, lookahead);
        }

        private GLRSimulatorState GetOrigin(GLRStackNode node, CFRuleDefinition definition)
        {
            GLRSimulatorState current = new GLRSimulatorState();
            current.Add(node);
            return GetOrigin(current, definition);
        }
        private GLRSimulatorState GetOrigin(GLRSimulatorState current, CFRuleDefinition definition)
        {
            GLRSimulatorState result = current;
            int index = definition.Length - 1;
            while (index != -1)
            {
                Symbol symbol = definition.Parts[index].Symbol;
                result = GetOrigin(result, symbol);
                index--;
            }
            return result;
        }
        private GLRSimulatorState GetOrigin(GLRSimulatorState current, Symbol symbol)
        {
            GLRSimulatorState result = new GLRSimulatorState();
            foreach (GLRStackNode node in current.Nodes)
            {
                if (node.Previous.ContainsKey(symbol))
                    foreach (GLRStackNode previous in node.Previous[symbol])
                        result.Add(previous);
                if (!inverseGraph.ContainsKey(node.State.ID))
                    continue;
                Dictionary<Symbol, List<State>> inverses = inverseGraph[node.State.ID];
                if (!inverses.ContainsKey(symbol))
                    continue;
                foreach (State previous in inverses[symbol])
                {
                    GLRStackNode pNode = result.Add(previous);
                    node.AddPrevious(symbol, pNode);
                }
            }
            return result;
        }
    }
}
