/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public abstract class BaseRNGLR1Parser : IParser
    {
        protected delegate void Production(BaseRNGLR1Parser parser, SPPFNode root, List<SPPFNode> nodes);
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
            public SymbolTerminal[] Expected;
            public Dictionary<ushort, ushort> ShiftsOnTerminal;
            public Dictionary<ushort, ushort> ShiftsOnVariable;
            public List<Reduction> ReducsOnTerminal;
            public State(string[] items, SymbolTerminal[] expected, ushort[] st_keys, ushort[] st_val, ushort[] sv_keys, ushort[] sv_val, Reduction[] rt)
            {
                Items = items;
                Expected = expected;
                ShiftsOnTerminal = new Dictionary<ushort,ushort>();
                ShiftsOnVariable = new Dictionary<ushort,ushort>();
                ReducsOnTerminal = new List<Reduction>(rt.Length);
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
            public List<Reduction> GetReductions(ushort sid)
            {
                List<Reduction> Reductions = new List<Reduction>();
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
        protected SPPFNode[] nullVarsSPPF;
        protected SPPFNode[] nullChoicesSPPF;
        protected Rule[] rules;
        protected State[] states;
        protected ushort axiomID;
        protected ushort axiomNullSPPF;
        protected ushort axiomPrimeID;


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
        protected List<ParserError> errors;
        protected System.Collections.ObjectModel.ReadOnlyCollection<ParserError> readonlyErrors;
        protected ILexer lexer;
        protected SymbolToken nextToken;
        protected LinkedList<ParserReduction> reductions;
        protected LinkedList<ParserShift> q;
        protected List<SPPFNode> n;

        public ICollection<ParserError> Errors { get { return readonlyErrors; } }

        protected abstract void setup();

        public BaseRNGLR1Parser(ILexer input)
        {
            setup();
            this.errors = new List<ParserError>();
            this.readonlyErrors = new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(errors);
            this.lexer = input;
            this.nextToken = null;
            //this.lexer.SetErrorHandler(new AddLexicalError(OnLexicalError));
        }

        /// <summary>
        /// Adds the given lexical error emanating from the lexer to the list of errors
        /// </summary>
        /// <param name="error">Lexical error</param>
        private void OnLexicalError(ParserError error)
        {
            errors.Add(error);
        }

        public CSTNode Parse()
        {
            SPPFNode match = Match();
            if (match == null)
                return null;
            return match.GetFirstTree().ApplyActions();
        }

        protected GSSNode GetInSet(List<GSSNode> StateSet, ushort label)
        {
            foreach (GSSNode node in StateSet)
                if (node.DFAState == label)
                    return node;
            return null;
        }
        protected SPPFNode GetInSet(SPPFNode node)
        {
            foreach (SPPFNode potential in n)
                if (potential.EquivalentTo(node))
                    return potential;
            n.Add(node);
            return node;
        }

        protected SPPFNode Match()
        {
            nextToken = lexer.GetNextToken();
            if (nextToken.SymbolID == 2)
            {
                // Dollar token => empty input
                if (states[0].HasReduction(2, axiomID, 0))
                    return nullVarsSPPF[axiomNullSPPF];
                else
                    return null; //Grammar do not support empty input
            }

            GSSNode v0 = new GSSNode(0, 0);
            List<GSSNode> Ui = new List<GSSNode>();
            Ui.Add(v0);
            reductions = new LinkedList<ParserReduction>();
            q = new LinkedList<ParserShift>();
            n = new List<SPPFNode>();

            ushort k = states[0].GetNextByShiftOnTerminal(nextToken.SymbolID);
            if (k != 0xFFFF)
                q.AddLast(new ParserShift(v0, k));
            foreach (Reduction reduction in states[0].GetReductions(nextToken.SymbolID))
                reductions.AddLast(new ParserReduction(v0, reduction.ToReduce, 0, reduction.Rest, nullChoicesSPPF[0]));

            int generation = 0;
            while (nextToken.SymbolID != 1) // Wait for ε token
            {
                n.Clear();
                while (reductions.Count != 0)
                    Reducer(Ui, generation);
                SymbolToken oldtoken = nextToken;
                nextToken = lexer.GetNextToken();
                List<GSSNode> Uj = Shifter(Ui, oldtoken, generation);
                generation++;
                if (Uj.Count == 0)
                {
                    // Generation is empty !
                    List<ushort> present = new List<ushort>();
                    List<SymbolTerminal> expected = new List<SymbolTerminal>();
                    foreach (GSSNode node in Ui)
                    {
                        foreach (SymbolTerminal terminal in states[node.DFAState].Expected)
                        {
                            if (!present.Contains(terminal.SymbolID))
                            {
                                expected.Add(terminal);
                                present.Add(terminal.SymbolID);
                            }
                        }
                    }
                    //errors.Add(new UnexpectedTokenError(oldtoken, expected.ToArray(), lexer.CurrentLine, lexer.CurrentColumn));
                    return null;
                }
                Ui = Uj;
            }

            foreach (GSSNode state in Ui)
            {
                if (states[state.DFAState].HasReduction(1, axiomPrimeID, 2))
                {
                    // Has reduction _Axiom_ -> axiom $ . on ε
                    List<List<GSSNode>> paths = state.GetPaths(2);
                    List<GSSNode> path = paths[0];
                    SPPFNode root = path[path.Count - 2].Edges[path[path.Count - 1]];
                    return root;
                }
            }
            // At end of input but was still waiting for tokens
            return null;
        }

        protected void Reducer(List<GSSNode> Ui, int generation)
        {
            GSSNode v = reductions.First.Value.Node;
            Rule rule = reductions.First.Value.Rule;
            SymbolVariable X = rule.Head;
            ushort m = reductions.First.Value.Length;
            SPPFNode f = reductions.First.Value.NullChoice;
            SPPFNode y = reductions.First.Value.FirstSPPF;
            reductions.RemoveFirst();
            List<List<GSSNode>> chi = null;
            if (m == 0) chi = v.GetPaths(0);
            else chi = v.GetPaths(m - 1);
            foreach (List<GSSNode> path in chi)
            {
                List<SPPFNode> ys = new List<SPPFNode>();
                if (m != 0) ys.Add(y);
                for (int i = 0; i != path.Count - 1; i++)
                    ys.Add(path[i].Edges[path[i + 1]]);
                GSSNode u = path[path.Count - 1];
                ushort k = u.DFAState;
                ushort l = states[k].GetNextByShiftOnVariable(X.SymbolID);
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
                            foreach (Reduction r in states[l].GetReductions(nextToken.SymbolID))
                                if (r.Length != 0)
                                    reductions.AddLast(new ParserReduction(u, r.ToReduce, r.Length, r.Rest, z));
                    }
                }
                else
                {
                    w = new GSSNode(l, generation);
                    Ui.Add(w);
                    w.AddEdge(u, z);
                    ushort h = states[l].GetNextByShiftOnTerminal(nextToken.SymbolID);
                    if (h != 0xFFFF)
                        q.AddLast(new ParserShift(w, h));
                    foreach (Reduction r in states[l].GetReductions(nextToken.SymbolID))
                    {
                        if (r.Length == 0)
                            reductions.AddLast(new ParserReduction(w, r.ToReduce, 0, r.Rest, nullChoicesSPPF[0]));
                        else if (m != 0)
                            reductions.AddLast(new ParserReduction(u, r.ToReduce, r.Length, r.Rest, z));
                    }
                }
                if (m != 0)
                {
                    ys.Reverse();
                    AddChildren(rule, z, ys, f);
                }
            }
        }

        protected List<GSSNode> Shifter(List<GSSNode> Ui, SymbolToken oldtoken, int generation)
        {
            List<GSSNode> Uj = new List<GSSNode>();
            LinkedList<ParserShift> Qp = new LinkedList<ParserShift>();
            SPPFNode z = new SPPFNode(oldtoken, generation);
            while (q.Count != 0)
            {
                GSSNode v = q.First.Value.Node;
                ushort k = q.First.Value.State;
                q.RemoveFirst();
                GSSNode w = GetInSet(Uj, k);
                if (w != null)
                {
                    w.AddEdge(v, z);
                    foreach (Reduction r in states[k].GetReductions(nextToken.SymbolID))
                        if (r.Length != 0)
                            reductions.AddLast(new ParserReduction(v, r.ToReduce, r.Length, r.Rest, z));
                }
                else
                {
                    w = new GSSNode(k, v.Generation + 1);
                    w.AddEdge(v, z);
                    Uj.Add(w);
                    ushort h = states[k].GetNextByShiftOnTerminal(nextToken.SymbolID);
                    if (h != 0xFFFF)
                        Qp.AddLast(new ParserShift(w, h));
                    foreach (Reduction r in states[k].GetReductions(nextToken.SymbolID))
                    {
                        if (r.Length == 0)
                            reductions.AddLast(new ParserReduction(w, r.ToReduce, 0, r.Rest, nullChoicesSPPF[0]));
                        else
                            reductions.AddLast(new ParserReduction(v, r.ToReduce, r.Length, r.Rest, z));
                    }
                }
            }
            q = Qp;
            return Uj;
        }

        protected void AddChildren(Rule rule, SPPFNode y, List<SPPFNode> ys, SPPFNode f)
        {
            if (f != null)
                ys.Add(f);
            rule.OnReduction.Invoke(this, y, ys);
        }
    }
}
