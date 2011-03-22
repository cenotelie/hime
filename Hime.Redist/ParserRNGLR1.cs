namespace Hime.Redist.Parsers
{
    public abstract class BaseRNGLR1Parser : IParser
    {
        protected delegate void Production(BaseRNGLR1Parser parser, SPPFNode root, System.Collections.Generic.List<SPPFNode> nodes);
        protected struct Rule
        {
            public Production OnReduction;
            public SymbolVariable Head;
            public Rule(Production prod, SymbolVariable head)
            {
                OnReduction = prod;
                Head = head;
            }
        }
        protected struct Terminal
        {
            public string Name;
            public ushort SID;
            public Terminal(string name, ushort sid)
            {
                Name = name;
                SID = sid;
            }
        }
        protected struct Reduction
        {
            public ushort Lookahead;
            public Rule ToReduce;
            public ushort Length;
            public SPPFNode Rest;
            public Reduction(ushort lookahead, Rule rule, ushort length, SPPFNode rest)
            {
                Lookahead = lookahead;
                ToReduce = rule;
                Length = length;
                Rest = rest;
            }
        }
        protected struct State
        {
            public string[] Items;
            public Terminal[] Expected;
            public System.Collections.Generic.Dictionary<ushort, ushort> ShiftsOnTerminal;
            public System.Collections.Generic.Dictionary<ushort, ushort> ShiftsOnVariable;
            public System.Collections.Generic.List<Reduction> ReducsOnTerminal;
            public State(string[] items, Terminal[] expected, ushort[] st_keys, ushort[] st_val, ushort[] sv_keys, ushort[] sv_val, Reduction[] rt)
            {
                Items = items;
                Expected = expected;
                ShiftsOnTerminal = new System.Collections.Generic.Dictionary<ushort,ushort>();
                ShiftsOnVariable = new System.Collections.Generic.Dictionary<ushort,ushort>();
                ReducsOnTerminal = new System.Collections.Generic.List<Reduction>(rt.Length);
                for (int i = 0; i != st_keys.Length; i++)
                    ShiftsOnTerminal.Add(st_keys[i], st_val[i]);
                for (int i = 0; i != sv_keys.Length; i++)
                    ShiftsOnVariable.Add(sv_keys[i], sv_val[i]);
                for (int i = 0; i != rt.Length; i++)
                    ReducsOnTerminal.Add(rt[i]);
            }

            public ushort GetNextByShiftOnTerminal(ushort sid)
            {
                if (!ShiftsOnTerminal.ContainsKey(sid))
                    return 0xFFFF;
                return ShiftsOnTerminal[sid];
            }
            public ushort GetNextByShiftOnVariable(ushort sid)
            {
                if (!ShiftsOnVariable.ContainsKey(sid))
                    return 0xFFFF;
                return ShiftsOnVariable[sid];
            }
            public System.Collections.Generic.List<Reduction> GetReductions(ushort sid)
            {
                System.Collections.Generic.List<Reduction> Reductions = new System.Collections.Generic.List<Reduction>();
                foreach (Reduction reduction in ReducsOnTerminal)
                    if (reduction.Lookahead == sid)
                        Reductions.Add(reduction);
                return Reductions;
            }
            public bool HasReduction(ushort tokenID, ushort varID, ushort length)
            {
                foreach (Reduction reduction in ReducsOnTerminal)
                    if (reduction.Lookahead == tokenID && reduction.ToReduce.Head.SymbolID == varID && reduction.Length == length)
                        return true;
                return false;
            }
        }

        // Parser automata data
        protected SPPFNode[] p_NullVarsSPPF;
        protected SPPFNode[] p_NullChoicesSPPF;
        protected Rule[] p_Rules;
        protected State[] p_States;
        protected ushort p_AxiomID;
        protected ushort p_AxiomNullSPPF;
        protected ushort p_AxiomPrimeID;


        protected struct ParserReduction
        {
            public GSSNode Node;
            public Rule Rule;
            public ushort Length;
            public SPPFNode NullChoice;
            public SPPFNode FirstSPPF;
            public ParserReduction(GSSNode node, Rule rule, ushort length, SPPFNode nullChoice, SPPFNode firstSPPF)
            {
                Node = node;
                Rule = rule;
                Length = length;
                NullChoice = nullChoice;
                FirstSPPF = firstSPPF;
            }
        }
        protected struct ParserShift
        {
            public GSSNode Node;
            public ushort State;
            public ParserShift(GSSNode node, ushort state)
            {
                Node = node;
                State = state;
            }
        }

        // Parser state data
        protected System.Collections.Generic.List<ParserError> p_Errors;
        protected ILexer p_Lexer;
        protected SymbolToken p_NextToken;
        protected System.Collections.Generic.LinkedList<ParserReduction> p_R;
        protected System.Collections.Generic.LinkedList<ParserShift> p_Q;
        protected System.Collections.Generic.List<SPPFNode> p_N;

        public System.Collections.ObjectModel.ReadOnlyCollection<ParserError> Errors { get { return new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(p_Errors); } }

        protected abstract void setup();

        public BaseRNGLR1Parser(ILexer input)
        {
            setup();
            p_Errors = new System.Collections.Generic.List<ParserError>();
            p_Lexer = input;
            p_NextToken = null;
        }

        public SyntaxTreeNode Analyse()
        {
            SPPFNode match = Match();
            if (match == null)
                return null;
            return match.GetFirstTree().ApplyActions();
        }

        protected GSSNode GetInSet(System.Collections.Generic.List<GSSNode> StateSet, ushort label)
        {
            foreach (GSSNode node in StateSet)
                if (node.DFAState == label)
                    return node;
            return null;
        }
        protected SPPFNode GetInSet(SPPFNode node)
        {
            foreach (SPPFNode potential in p_N)
                if (potential.EquivalentTo(node))
                    return potential;
            p_N.Add(node);
            return node;
        }

        protected SPPFNode Match()
        {
            p_NextToken = p_Lexer.GetNextToken();
            if (p_NextToken.SymbolID == 2)
            {
                // Dollar token
                if (p_States[0].HasReduction(2, p_AxiomID, 0))
                    return p_NullVarsSPPF[p_AxiomNullSPPF];
                else
                    return null;
            }
            GSSNode v0 = new GSSNode(0, 0);
            System.Collections.Generic.List<GSSNode> Ui = new System.Collections.Generic.List<GSSNode>();
            Ui.Add(v0);
            p_R = new System.Collections.Generic.LinkedList<ParserReduction>();
            p_Q = new System.Collections.Generic.LinkedList<ParserShift>();
            p_N = new System.Collections.Generic.List<SPPFNode>();

            ushort k = p_States[0].GetNextByShiftOnTerminal(p_NextToken.SymbolID);
            if (k != 0xFFFF)
                p_Q.AddLast(new ParserShift(v0, k));
            foreach (Reduction reduction in p_States[0].GetReductions(p_NextToken.SymbolID))
                p_R.AddLast(new ParserReduction(v0, reduction.ToReduce, 0, reduction.Rest, p_NullChoicesSPPF[0]));

            int generation = 0;
            while (p_NextToken.SymbolID != 1)
            {
                p_N.Clear();
                while (p_R.Count != 0)
                    Reducer(Ui, generation);
                SymbolToken oldtoken = p_NextToken;
                p_NextToken = p_Lexer.GetNextToken();
                Ui = Shifter(Ui, oldtoken, generation);
                generation++;
                if (Ui.Count == 0)
                    return null;
            }
            foreach (GSSNode state in Ui)
            {
                if (p_States[state.DFAState].HasReduction(1, p_AxiomPrimeID, 2))
                {
                    System.Collections.Generic.List<System.Collections.Generic.List<GSSNode>> paths = state.GetPaths(2);
                    System.Collections.Generic.List<GSSNode> path = paths[0];
                    SPPFNode root = path[path.Count - 2].Edges[path[path.Count - 1]];
                    return root;
                }
            }
            return null;
        }

        protected void Reducer(System.Collections.Generic.List<GSSNode> Ui, int generation)
        {
            GSSNode v = p_R.First.Value.Node;
            Rule rule = p_R.First.Value.Rule;
            SymbolVariable X = rule.Head;
            ushort m = p_R.First.Value.Length;
            SPPFNode f = p_R.First.Value.NullChoice;
            SPPFNode y = p_R.First.Value.FirstSPPF;
            p_R.RemoveFirst();
            System.Collections.Generic.List<System.Collections.Generic.List<GSSNode>> chi = null;
            if (m == 0) chi = v.GetPaths(0);
            else chi = v.GetPaths(m - 1);
            foreach (System.Collections.Generic.List<GSSNode> path in chi)
            {
                System.Collections.Generic.List<SPPFNode> ys = new System.Collections.Generic.List<SPPFNode>();
                if (m != 0) ys.Add(y);
                for (int i = 0; i != path.Count - 1; i++)
                    ys.Add(path[i].Edges[path[i + 1]]);
                GSSNode u = path[path.Count - 1];
                ushort k = u.DFAState;
                ushort l = p_States[k].GetNextByShiftOnVariable(X.SymbolID);
                SPPFNode z = null;
                if (m == 0)
                    z = f;
                else
                {
                    int c = u.Generation;
                    z = GetInSet(new SPPFNode(X, c));
                }
                GSSNode w = GetInSet(Ui, l);
                if (w != null)
                {
                    if (!w.Edges.ContainsKey(u))
                    {
                        w.AddEdge(u, z);
                        if (m != 0)
                            foreach (Reduction r in p_States[l].GetReductions(p_NextToken.SymbolID))
                                if (r.Length != 0)
                                    p_R.AddLast(new ParserReduction(u, r.ToReduce, r.Length, r.Rest, z));
                    }
                }
                else
                {
                    w = new GSSNode(l, generation);
                    Ui.Add(w);
                    w.AddEdge(u, z);
                    ushort h = p_States[l].GetNextByShiftOnTerminal(p_NextToken.SymbolID);
                    if (h != 0xFFFF)
                        p_Q.AddLast(new ParserShift(w, h));
                    foreach (Reduction r in p_States[l].GetReductions(p_NextToken.SymbolID))
                    {
                        if (r.Length == 0)
                            p_R.AddLast(new ParserReduction(w, r.ToReduce, 0, r.Rest, p_NullChoicesSPPF[0]));
                        else if (m != 0)
                            p_R.AddLast(new ParserReduction(u, r.ToReduce, r.Length, r.Rest, z));
                    }
                }
                if (m != 0)
                {
                    ys.Reverse();
                    AddChildren(rule, z, ys, f);
                }
            }
        }

        protected System.Collections.Generic.List<GSSNode> Shifter(System.Collections.Generic.List<GSSNode> Ui, SymbolToken oldtoken, int generation)
        {
            System.Collections.Generic.List<GSSNode> Uj = new System.Collections.Generic.List<GSSNode>();
            System.Collections.Generic.LinkedList<ParserShift> Qp = new System.Collections.Generic.LinkedList<ParserShift>();
            SPPFNode z = new SPPFNode(oldtoken, generation);
            while (p_Q.Count != 0)
            {
                GSSNode v = p_Q.First.Value.Node;
                ushort k = p_Q.First.Value.State;
                p_Q.RemoveFirst();
                GSSNode w = GetInSet(Uj, k);
                if (w != null)
                {
                    w.AddEdge(v, z);
                    foreach (Reduction r in p_States[k].GetReductions(p_NextToken.SymbolID))
                        if (r.Length != 0)
                            p_R.AddLast(new ParserReduction(v, r.ToReduce, r.Length, r.Rest, z));
                }
                else
                {
                    w = new GSSNode(k, v.Generation + 1);
                    w.AddEdge(v, z);
                    Uj.Add(w);
                    ushort h = p_States[k].GetNextByShiftOnTerminal(p_NextToken.SymbolID);
                    if (h != 0xFFFF)
                        Qp.AddLast(new ParserShift(w, h));
                    foreach (Reduction r in p_States[k].GetReductions(p_NextToken.SymbolID))
                    {
                        if (r.Length == 0)
                            p_R.AddLast(new ParserReduction(w, r.ToReduce, 0, r.Rest, p_NullChoicesSPPF[0]));
                        else
                            p_R.AddLast(new ParserReduction(v, r.ToReduce, r.Length, r.Rest, z));
                    }
                }
            }
            p_Q = Qp;
            return Uj;
        }

        protected void AddChildren(Rule rule, SPPFNode y, System.Collections.Generic.List<SPPFNode> ys, SPPFNode f)
        {
            if (f != null)
                ys.Add(f);
            rule.OnReduction.Invoke(this, y, ys);
        }
    }
}