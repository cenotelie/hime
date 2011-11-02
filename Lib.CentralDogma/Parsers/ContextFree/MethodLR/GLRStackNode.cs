/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    public class GLRStackNode
    {
        private int id;
        private State state;
        private Dictionary<GrammarSymbol, Dictionary<int, GLRStackNode>> previous;

        public int ID { get { return id; } }
        public State State { get { return state; } }
        public Dictionary<GrammarSymbol, Dictionary<int, GLRStackNode>> Previous { get { return previous; } }

        public GLRStackNode(State state)
        {
            this.id = GetHashCode();
            this.state = state;
            this.previous = new Dictionary<GrammarSymbol, Dictionary<int, GLRStackNode>>(GrammarSymbol.Comparer.Instance);
        }

        public void AddPrevious(GrammarSymbol symbol, GLRStackNode node)
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