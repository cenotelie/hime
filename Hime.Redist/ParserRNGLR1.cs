namespace Hime.Redist.Parsers
{
    public class GSSNode
    {
        private ushort p_Label;
        private System.Collections.Generic.List<GSSNode> p_Edges;

        public ushort Label { get { return p_Label; } }
        public System.Collections.Generic.List<GSSNode> Edges { get { return p_Edges; } }

        public GSSNode(ushort label)
        {
            p_Label = label;
            p_Edges = new System.Collections.Generic.List<GSSNode>();
        }

        public System.Collections.Generic.List<GSSNode> NodesAt(int length)
        {
            System.Collections.Generic.List<GSSNode> nodes = new System.Collections.Generic.List<GSSNode>();
            nodes.Add(this);
            while (length != 0)
            {
                System.Collections.Generic.List<GSSNode> nexts = new System.Collections.Generic.List<GSSNode>();
                foreach (GSSNode current in nodes)
                    foreach (GSSNode next in current.p_Edges)
                        if (!nexts.Contains(next))
                            nexts.Add(next);
                nodes = nexts;
                if (nodes.Count == 0)
                    return nodes;
                length--;
            }
            return nodes;
        }
    }



    public abstract class BaseRNGLR1Parser
    {
        protected delegate void Production(BaseRNGLR1Parser parser, System.Collections.Generic.List<SPPFNode> nodes, int length);

        // Parser automata data
        protected Production[] p_Rules;
        protected ushort[] p_RulesHeadID;
        protected string[] p_RulesHeadName;
        protected ushort[] p_RulesParserLength;
        protected ushort[][] p_StateExpectedIDs;
        protected string[][] p_StateExpectedNames;
        protected string[][] p_StateItems;
        protected ushort[][][] p_StateShiftsOnTerminal;
        protected ushort[][][] p_StateShiftsOnVariable;
        protected ushort[][][] p_StateReducsOnTerminal;
        protected ushort p_AxiomID;

        protected struct Reduction
        {
            public GSSNode Node;
            public ushort VarSID;
            public ushort Length;
            public Reduction(GSSNode node, ushort sid, ushort length)
            {
                Node = node;
                VarSID = sid;
                Length = length;
            }
        }
        protected struct Shift
        {
            public GSSNode Node;
            public ushort State;
            public Shift(GSSNode node, ushort state)
            {
                Node = node;
                State = state;
            }
        }

        // Parser state data
        protected System.Collections.Generic.List<ParserError> p_Errors;
        protected ILexer p_Lexer;
        protected SymbolToken p_NextToken;
        protected System.Collections.Generic.LinkedList<Reduction> p_R;
        protected System.Collections.Generic.LinkedList<Shift> p_Q;

        protected ushort GetNextByShiftOnTerminal(ushort state, ushort sid)
        {
            for (int i = 0; i != p_StateShiftsOnTerminal[state].Length; i++)
            {
                if (p_StateShiftsOnTerminal[state][i][0] == sid)
                    return p_StateShiftsOnTerminal[state][i][1];
            }
            return 0xFFFF;
        }
        protected ushort GetNextByShiftOnVariable(ushort state, ushort sid)
        {
            for (int i = 0; i != p_StateShiftsOnVariable[state].Length; i++)
            {
                if (p_StateShiftsOnVariable[state][i][0] == sid)
                    return p_StateShiftsOnVariable[state][i][1];
            }
            return 0xFFFF;
        }
        protected System.Collections.Generic.List<ushort[]> GetReductions(ushort state, ushort sid)
        {
            System.Collections.Generic.List<ushort[]> Reductions = new System.Collections.Generic.List<ushort[]>();
            for (int i = 0; i != p_StateReducsOnTerminal[state].Length; i++)
            {
                if (p_StateReducsOnTerminal[state][i][0] == sid)
                    Reductions.Add(p_StateReducsOnTerminal[state][i]);
            }
            return Reductions;
        }
        protected bool HasReduction(ushort state, ushort tokenID, ushort varID, ushort length)
        {
            ushort[][] ReductionsOnState = p_StateReducsOnTerminal[state];
            for (int i = 0; i != ReductionsOnState.Length; i++)
            {
                ushort[] reduction = ReductionsOnState[i];
                if (reduction[0] != tokenID)
                    continue;
                if (reduction[2] != length)
                    continue;
                ushort rule = reduction[1];
                if (p_RulesHeadID[rule] == varID)
                    return true;
            }
            return false;
        }
        
        protected GSSNode GetInSet(System.Collections.Generic.List<GSSNode> StateSet, ushort label)
        {
            foreach (GSSNode node in StateSet)
                if (node.Label == label)
                    return node;
            return null;
        }

        protected abstract void setup();

        public BaseRNGLR1Parser(ILexer input)
        {
            setup();
            p_Errors = new System.Collections.Generic.List<ParserError>();
            p_Lexer = input;
            p_NextToken = null;
        }

        public bool Match()
        {
            p_NextToken = p_Lexer.GetNextToken();
            if (p_NextToken.SymbolID == 2)
            {
                // Dollar token
                if (HasReduction(0, 2, p_AxiomID, 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            GSSNode v0 = new GSSNode(0);
            System.Collections.Generic.List<GSSNode> U0 = new System.Collections.Generic.List<GSSNode>();
            U0.Add(v0);
            p_R = new System.Collections.Generic.LinkedList<Reduction>();
            p_Q = new System.Collections.Generic.LinkedList<Shift>();

            ushort k = GetNextByShiftOnTerminal(0, p_NextToken.SymbolID);
            if (k != 0xFFFF)
                p_Q.AddLast(new Shift(v0, k));
            foreach (ushort[] reduction in GetReductions(0, p_NextToken.SymbolID))
                p_R.AddLast(new Reduction(v0, p_RulesHeadID[reduction[1]], 0));

            System.Collections.Generic.List<GSSNode> Ui = U0;
            while (p_NextToken.SymbolID != 1)
            {
                while (p_R.Count != 0)
                    Reducer(Ui);
                p_NextToken = p_Lexer.GetNextToken();
                if (p_NextToken.SymbolID != 2)
                    Ui = Shifter(Ui);
                if (Ui.Count == 0)
                    return false;
            }
            return true;
        }

        protected void Reducer(System.Collections.Generic.List<GSSNode> Ui)
        {
            GSSNode v = p_R.First.Value.Node;
            ushort X = p_R.First.Value.VarSID;
            ushort m = p_R.First.Value.Length;
            p_R.RemoveFirst();
            System.Collections.Generic.List<GSSNode> chi = null;
            if (m == 0) chi = v.NodesAt(0);
            else chi = v.NodesAt(m - 1);
            foreach (GSSNode u in chi)
            {
                ushort k = u.Label;
                ushort l = GetNextByShiftOnVariable(k, X);
                GSSNode w = GetInSet(Ui, l);
                if (w != null)
                {
                    if (!w.Edges.Contains(u))
                    {
                        w.Edges.Add(u);
                        if (m != 0)
                            foreach (ushort[] r in GetReductions(l, p_NextToken.SymbolID))
                                if (r[2] != 0)
                                    p_R.AddLast(new Reduction(u, p_RulesHeadID[r[1]], r[2]));
                    }
                }
                else
                {
                    w = new GSSNode(l);
                    Ui.Add(w);
                    w.Edges.Add(u);
                    ushort h = GetNextByShiftOnTerminal(l, p_NextToken.SymbolID);
                    if (h != 0xFFFF)
                        p_Q.AddLast(new Shift(w, h));
                    foreach (ushort[] r in GetReductions(l, p_NextToken.SymbolID))
                    {
                        if (r[2] == 0)
                            p_R.AddLast(new Reduction(w, p_RulesHeadID[r[1]], 0));
                        else if (m != 0)
                            p_R.AddLast(new Reduction(u, p_RulesHeadID[r[1]], r[2]));
                    }
                }
            }
        }

        protected System.Collections.Generic.List<GSSNode> Shifter(System.Collections.Generic.List<GSSNode> Ui)
        {
            System.Collections.Generic.List<GSSNode> Uj = new System.Collections.Generic.List<GSSNode>();
            System.Collections.Generic.LinkedList<Shift> Qp = new System.Collections.Generic.LinkedList<Shift>();
            while (p_Q.Count != 0)
            {
                GSSNode v = p_Q.First.Value.Node;
                ushort k = p_Q.First.Value.State;
                p_Q.RemoveFirst();
                GSSNode w = GetInSet(Uj, k);
                if (w != null)
                {
                    w.Edges.Add(v);
                    foreach (ushort[] r in GetReductions(k, p_NextToken.SymbolID))
                        if (r[2] != 0)
                            p_R.AddLast(new Reduction(v, p_RulesHeadID[r[1]], r[2]));
                }
                else
                {
                    w = new GSSNode(k);
                    w.Edges.Add(v);
                    Uj.Add(w);
                    ushort h = GetNextByShiftOnTerminal(k, p_NextToken.SymbolID);
                    if (h != 0xFFFF)
                        Qp.AddLast(new Shift(w, h));
                    foreach (ushort[] r in GetReductions(k, p_NextToken.SymbolID))
                    {
                        if (r[2] == 0) p_R.AddLast(new Reduction(w, p_RulesHeadID[r[1]], 0));
                        else p_R.AddLast(new Reduction(v, p_RulesHeadID[r[1]], r[2]));
                    }
                }
            }
            p_Q = Qp;
            return Uj;
        }
    }
}