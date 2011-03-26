using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ConflictAnalyser
    {
        private GLRSimulator simulator;

        public ConflictAnalyser(Graph graph)
        {
            simulator = new GLRSimulator(graph);
        }

        public DeciderGraph Analyse(ItemSet lrset, Item item1, Item item2)
        {
            DeciderGraph graph = new DeciderGraph();
            // Add first state
            
            graph.Build(simulator);
            return graph;
        }

		/*public void Analyse(ItemSet lrset, Item item1, Item item2)
		{
			List<DeciderState> states = new List<DeciderState>();
			
			List<ItemSet> choice1 = ReduceShift(new ItemSet(item1));
			List<ItemSet> choice2 = ReduceShift(new ItemSet(item2));
			
			DeciderState initialState = new DeciderState(choice1, choice2); 
			// TODO: when creating state, do not forget to build lookaheads for which a decision is immediately possible
			
			List<DeciderState> todoList = new List<DeciderState>();
			todoList.Add(initialState);
			states.Add(initialState);
			
			while (todoList.Count != 0)
			{
				int index = todoList.Count - 1;
				DeciderState currentState = todoList[index];
				todoList.Remove(index);
				
				List<DeciderState> results = Next(currentState);
				foreach (DeciderState state in results)
				{
					if (states.Contains(state)) continue;
					states.Add(state);
				}
			}
		}
		
		private List<DeciderState> Next(DeciderState currentState)
		{
			List<DeciderState> result = new List<DeciderState>();
			foreach (Terminal commonLookahead in currentState.Lookaheads)
			{
				DeciderState successor = currentState.SimulateOnLookahead(lookahead, inverseGraph);
				result.Add(successor);
			}
		}*/
    }
}
