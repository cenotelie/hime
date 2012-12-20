/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class GLRSimulator : GraphInverse
    {
        public GLRSimulator(Graph graph) : base(graph) { }

        public GLRSimulatorState Simulate(GLRSimulatorState origin, Terminal lookahead)
        {
            // Nodes before reductions
            GLRSimulatorState before = new GLRSimulatorState(origin.Nodes);

            // Apply reductions
            for (int i = 0; i != before.Nodes.Count; i++)
            {
                GLRStackNode node = before.Nodes[i];
                foreach (StateActionReduce reduce in node.State.Reductions)
                {
                    if (reduce.Lookahead.SID != lookahead.SID)
                        continue;
                    GLRSimulatorState reduceOrigin = GetOrigin(node, reduce.ToReduceRule.CFBody.GetChoiceAt(0));
                    foreach (GLRStackNode nOrigin in reduceOrigin.Nodes)
                    {
                        if (nOrigin.State.Children.ContainsKey(reduce.ToReduceRule.Head))
                        {
                            State next = nOrigin.State.Children[reduce.ToReduceRule.Head];
                            GLRStackNode follower = before.Add(next);
                            follower.AddPrevious(reduce.ToReduceRule.Head, nOrigin);
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

            GLRSimulatorState origin = GetOrigin(pState, item.BaseRule.CFBody.GetChoiceAt(0));
            foreach (GLRStackNode node in origin.Nodes)
            {
                if (node.State.Children.ContainsKey(item.BaseRule.Head))
                {
                    State next = node.State.Children[item.BaseRule.Head];
                    GLRStackNode nNode = result.Add(next);
                    nNode.AddPrevious(item.BaseRule.Head, node);
                }
            }
            return Simulate(result, lookahead);
        }

        private GLRSimulatorState GetOrigin(GLRStackNode node, CFRuleBody definition)
        {
            GLRSimulatorState current = new GLRSimulatorState();
            current.Add(node);
            return GetOrigin(current, definition);
        }
        private GLRSimulatorState GetOrigin(GLRSimulatorState current, CFRuleBody definition)
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
                {
                    foreach (GLRStackNode previous in node.Previous[symbol].Values)
                        result.Add(previous);
                    continue;
                }
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


        public List<List<Terminal>> GetInputsFor(State state)
        {
            List<List<Terminal>> result = new List<List<Terminal>>();
            foreach (ICollection<Symbol> path in GetPaths(state))
            {
                List<Terminal> input = new List<Terminal>();
                foreach (Symbol s in path)
                {
                    if (s is Terminal)
                        input.Add(s as Terminal);
                    else
                        BuildInput(input, s as CFVariable, new Stack<CFRuleBody>());
                }
                result.Add(input);
            }
            return result;
        }

        // TODO: there is a bug in this method, it may call itself forever
        private void BuildInput(List<Terminal> sample, CFVariable var, Stack<CFRuleBody> stack)
        {
            if (var.Firsts.Contains(Epsilon.Instance))
                return;
            List<CFRuleBody> definitions = new List<CFRuleBody>();
            foreach (CFRule rule in var.Rules)
                definitions.Add(rule.CFBody.GetChoiceAt(0));
            CFRuleBody def = null;
            foreach (CFRuleBody d in definitions)
            {
                if (!stack.Contains(d))
                {
                    def = d;
                    break;
                }
            }
            if (def == null)
                def = definitions[0];
            CFRuleBody[] stackElements = stack.ToArray();
            stack.Push(def);
            foreach (RuleBodyElement part in def.Parts)
            {
                if (part.Symbol is Terminal)
                {
                    sample.Add(part.Symbol as Terminal);
                }
                else
                {
                	bool symbolFound = false;
                	// TODO: cleanup this code, this is really not a nice patch!!
                	// TODO: this is surely ineficient, but I don't understand the algorithm to do better yet
                	// The idea is to try and avoid infinite recursive call by checking the symbol was not already processed before
                	foreach (CFRuleBody definition in stackElements)
                	{
                		foreach (RuleBodyElement definitionPart in definition.Parts)
                		{
                			if (definitionPart.Symbol.Equals(part.Symbol))
                			{
                				symbolFound = true;
                			}
                		}
                	}
	                // if part.Symbol is not the same as another part.symbol found in a previous definition
                	if (!symbolFound) BuildInput(sample, part.Symbol as CFVariable, stack);
                }
            }
            stack.Pop();
        }

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

        private List<ICollection<Symbol>> GetPaths(State state)
        {
            Dictionary<int, SortedList<ushort, ENode>> visited = new Dictionary<int, SortedList<ushort, ENode>>();
            LinkedList<ENode> queue = new LinkedList<ENode>();
            List<ENode> goals = new List<ENode>();
            queue.AddFirst(new ENode(state, null, null));
            
            while (queue.Count != 0)
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
                        if (visited.ContainsKey(previous.ID))
                        {
                            if (visited[previous.ID].ContainsKey(s.SID))
                                continue;
                        }
                        else
                            visited.Add(previous.ID, new SortedList<ushort, ENode>());
                        ENode pnode = new ENode(previous, current, s);
                        visited[previous.ID].Add(s.SID, pnode);
                        if (previous.ID == 0)
                            goals.Add(pnode);
                        else
                            queue.AddLast(pnode);
                    }
                }
            }

            List<ICollection<Symbol>> paths = new List<ICollection<Symbol>>();
            foreach (ENode start in goals)
            {
                ENode node = start;
                LinkedList<Symbol> path = new LinkedList<Symbol>();
                while (node.next != null)
                {
                    path.AddLast(node.transition);
                    node = node.next;
                }
                paths.Add(path);
            }
            return paths;
        }
    }
}
