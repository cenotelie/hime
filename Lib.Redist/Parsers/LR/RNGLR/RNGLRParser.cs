using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    class RNGLRParser : BaseLRParser
    {
        private delegate object GetSemObj(Symbols.Symbol symbol);
        private delegate object Reduce(LRProduction production);

        private struct ParserReduction
        {
            public GSSNode Node;
            public Rule Rule;
            public ushort Length;
            public AST.SPPFNode NullChoice;
            public AST.SPPFNode FirstSPPF;
            public ParserReduction(GSSNode node, Rule rule, ushort length, AST.SPPFNode nullChoice, AST.SPPFNode firstSPPF)
            {
                Node = node;
                Rule = rule;
                Length = length;
                NullChoice = nullChoice;
                FirstSPPF = firstSPPF;
            }
        }
        private struct ParserShift
        {
            public GSSNode Node;
            public ushort State;
            public ParserShift(GSSNode node, ushort state)
            {
                Node = node;
                State = state;
            }
        }

        /// <summary>
        /// RNGLR parsing table and productions
        /// </summary>
        private RNGLRAutomaton parserAutomaton;
        /// <summary>
        /// Parser's input encapsulating the lexer
        /// </summary>
        private RewindableTokenStream input;
        /// <summary>
        /// Current stack's head
        /// </summary>
        private int head;
        /// <summary>
        /// Delegate reducer
        /// </summary>
        private Reduce reducer;
        /// <summary>
        /// Transforms a symbol to a semantic object
        /// </summary>
        private GetSemObj getSemObj;

        private Symbols.Token nextToken;
        private LinkedList<ParserReduction> reductions;
        private LinkedList<ParserShift> q;
        private List<AST.SPPFNode> n;


        private AST.SPPFNode[] nullables;

        /// <summary>
        /// Initializes a new instance of the LRkParser class with the given lexer
        /// </summary>
        /// <param name="automaton">The parser's automaton</param>
        /// <param name="variables">The parser's variables</param>
        /// <param name="virtuals">The parser's virtuals</param>
        /// <param name="actions">The parser's actions</param>
        /// <param name="lexer">The input lexer</param>
        public RNGLRParser(RNGLRAutomaton automaton, Symbols.Variable[] variables, Symbols.Virtual[] virtuals, SemanticAction[] actions, Lexer.ILexer lexer)
            : base(variables, virtuals, actions, lexer)
        {
            this.parserAutomaton = automaton;
            this.input = new RewindableTokenStream(lexer);

            this.nullables = new AST.SPPFNode[variables.Length];
            for (ushort i = 0; i != parserAutomaton.Nullables.Length; i++)
                BuildNullable(parserAutomaton.GetProduction(i));
        }

        private void BuildNullable(LRProduction production)
        {
            Symbols.Variable var = parserVariables[production.Head];
            if (nullables[production.Head] == null)
                nullables[production.Head] = new AST.SPPFNode(var, 0, (AST.CSTAction)production.HeadAction);
            AST.SPPFFamily family = new AST.SPPFFamily(nullables[production.Head]);
            nullables[production.Head].AddFamily(family);
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                if (production.Bytecode[i] == LRProduction.SemanticAction)
                    family.AddChild(new AST.SPPFNode(new Symbols.Action(parserActions[production.Bytecode[i + 1]]), 0));
            }
        }





        /// <summary>
        /// Handles an unexpected token and returns whether is successfuly handled the error
        /// </summary>
        /// <param name="token">The unexpected token</param>
        /// <returns>The next token</returns>
        protected override Symbols.Token OnUnexpectedToken(Symbols.Token token)
        {
            List<int> expectedIDs = parserAutomaton.GetExpected(stack[head], lexer.Terminals.Count);
            List<Symbols.Terminal> expected = new List<Symbols.Terminal>();
            foreach (int index in expectedIDs)
                expected.Add(lexer.Terminals[index]);
            errors.Add(new UnexpectedTokenError(token, expected, lexer.CurrentLine, lexer.CurrentColumn));
            return null;
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public override AST.CSTNode Parse()
        {
            this.reducer = new Reduce(ReduceAST);
            this.getSemObj = new GetSemObj(GetSemCST);
            object result = Execute();
            if (result == null)
                return null;
            return (result as AST.CSTNode).ApplyActions();
        }

        /// <summary>
        /// Parses the input and returns whether the input is recognized
        /// </summary>
        /// <returns>True if the input is recognized, false otherwise</returns>
        public override bool Recognize()
        {
            this.reducer = new Reduce(ReduceSimple);
            this.getSemObj = new GetSemObj(GetSemNaked);
            return (Execute() != null);
        }

        private object GetSemCST(Symbols.Symbol symbol) { return new AST.CSTNode(symbol); }
        private object GetSemNaked(Symbols.Symbol symbol) { return symbol; }


        public SyntaxTreeNode Analyse()
        {
            AST.SPPFNode match = Match();
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
        protected AST.SPPFNode GetInSet(AST.SPPFNode node)
        {
            foreach (AST.SPPFNode potential in n)
                if (potential.EquivalentTo(node))
                    return potential;
            n.Add(node);
            return node;
        }

        protected AST.SPPFNode Match()
        {
            nextToken = lexer.GetNextToken();
            if (nextToken.SymbolID == Symbols.Dollar.Instance.SymbolID)
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
            n = new List<AST.SPPFNode>();

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
                    errors.Add(new UnexpectedTokenError(oldtoken, expected.ToArray(), lexer.CurrentLine, lexer.CurrentColumn));
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
                    AST.SPPFNode root = path[path.Count - 2].Edges[path[path.Count - 1]];
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
            AST.SPPFNode f = reductions.First.Value.NullChoice;
            AST.SPPFNode y = reductions.First.Value.FirstSPPF;
            reductions.RemoveFirst();
            List<List<GSSNode>> chi = null;
            if (m == 0) chi = v.GetPaths(0);
            else chi = v.GetPaths(m - 1);
            foreach (List<GSSNode> path in chi)
            {
                List<AST.SPPFNode> ys = new List<AST.SPPFNode>();
                if (m != 0) ys.Add(y);
                for (int i = 0; i != path.Count - 1; i++)
                    ys.Add(path[i].Edges[path[i + 1]]);
                GSSNode u = path[path.Count - 1];
                ushort k = u.DFAState;
                ushort l = states[k].GetNextByShiftOnVariable(X.SymbolID);
                AST.SPPFNode z = null;
                if (m == 0)
                    z = f;
                else
                {
                    int c = u.Generation;
                    z = GetInSet(new AST.SPPFNode(X, c));
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
            AST.SPPFNode z = new AST.SPPFNode(oldtoken, generation);
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

        protected void AddChildren(Rule rule, AST.SPPFNode y, List<AST.SPPFNode> ys, AST.SPPFNode f)
        {
            if (f != null)
                ys.Add(f);
            rule.OnReduction.Invoke(this, y, ys);
        }
    }
}
