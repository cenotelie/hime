using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class GLRStackNode
    {
        private int id;
        private State state;
        private Dictionary<Symbol, Dictionary<int, GLRStackNode>> previous;

        public int ID { get { return id; } }
        public State State { get { return state; } }
        public Dictionary<Symbol, Dictionary<int, GLRStackNode>> Previous { get { return previous; } }

        public GLRStackNode(State state)
        {
            this.id = GetHashCode();
            this.state = state;
            this.previous = new Dictionary<Symbol, Dictionary<int, GLRStackNode>>(Symbol.EqualityComparer.Instance);
        }

        public void AddPrevious(Symbol symbol, GLRStackNode node)
        {
            if (!previous.ContainsKey(symbol))
            {
                Dictionary<int, GLRStackNode> nodes = new Dictionary<int, GLRStackNode>();
                nodes.Add(node.id, node);
                previous.Add(symbol, nodes);
            }
            else
            {
                Dictionary<int, GLRStackNode> nodes = previous[symbol];
                if (nodes.ContainsKey(node.id))
                    return;
                nodes.Add(node.id, node);
            }
        }
    }
}