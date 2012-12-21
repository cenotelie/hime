using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    public class RNGLRParser : BaseLRParser
    {
        private delegate object GetSemObj(Symbols.Symbol symbol);
        private delegate object Reduce(LRProduction production);

        private struct Reduction
        {
            public GSSNode node;
            public LRProduction prod;
            public AST.SPPFNode first;
            public Reduction(GSSNode node, LRProduction prod, AST.SPPFNode first)
            {
                this.node = node;
                this.prod = prod;
                this.first = first;
            }
        }
        private struct Shift
        {
            public GSSNode node;
            public ushort state;
            public Shift(GSSNode node, ushort state)
            {
                this.node = node;
                this.state = state;
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
        /// Delegate reducer
        /// </summary>
        private Reduce reducer;
        /// <summary>
        /// Transforms a symbol to a semantic object
        /// </summary>
        private GetSemObj getSemObj;

        private Symbols.Token nextToken;
        private LinkedList<Reduction> R;
        private LinkedList<Shift> Q;
        private List<AST.SPPFNode> N;

        private AST.SPPFNode epsilon;
        private AST.SPPFNode[] nullables;

        /// <summary>
        /// Initializes a new instance of the LRkParser class with the given lexer
        /// </summary>
        /// <param name="automaton">The parser's automaton</param>
        /// <param name="variables">The parser's variables</param>
        /// <param name="virtuals">The parser's virtuals</param>
        /// <param name="actions">The parser's actions</param>
        /// <param name="lexer">The input lexer</param>
        public RNGLRParser(RNGLRAutomaton automaton, Symbols.Variable[] variables, Symbols.Virtual[] virtuals, SemanticAction[] actions, Lexer.TextLexer lexer)
            : base(variables, virtuals, actions, lexer)
        {
            this.parserAutomaton = automaton;
            this.input = new RewindableTokenStream(lexer);
            this.epsilon = new AST.SPPFNode(Symbols.Epsilon.Instance, 0);
            this.nullables = new AST.SPPFNode[variables.Length];
            for (ushort i = 0; i != parserAutomaton.Nullables.Length; i++)
            {
                LRProduction prod = parserAutomaton.GetProduction(i);
                this.nullables[i] = new AST.SPPFNode(parserVariables[prod.Head], 0, (AST.CSTAction)prod.HeadAction);
            }
            for (ushort i = 0; i != parserAutomaton.Nullables.Length; i++)
                BuildNullable(parserAutomaton.GetProduction(i));
        }

        private void BuildNullable(LRProduction production)
        {
            AST.SPPFNode head = nullables[production.Head];
            AST.SPPFFamily family = new AST.SPPFFamily(head);
            head.AddFamily(family);
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                ushort op = production.Bytecode[i];
                if (LRBytecode.IsSemAction(op))
                {
                    family.AddChild(new AST.SPPFNode(new Symbols.Action(parserActions[production.Bytecode[i + 1]]), 0));
                    i++;
                }
                else if (LRBytecode.IsAddVirtual(op))
                {
                    family.AddChild(new AST.SPPFNode(parserVirtuals[production.Bytecode[i + 1]], 0, LRBytecode.GetAction(op)));
                    i++;
                }
                else if (LRBytecode.IsAddNullVariable(op))
                {
                    family.AddChild(nullables[production.Bytecode[i + 1]]);
                    i++;
                }
            }
        }

        private void OnUnexpectedToken(List<GSSNode> Ui, Symbols.Token token)
        {
            List<int> indices = new List<int>();
            List<Symbols.Terminal> expected = new List<Symbols.Terminal>();
            foreach (GSSNode node in Ui)
            {
                List<int> temp = parserAutomaton.GetExpected(node.State, lexer.Terminals.Count);
                foreach (int index in temp)
                {
                    if (!indices.Contains(index))
                    {
                        indices.Add(index);
                        expected.Add(lexer.Terminals[index]);
                    }
                }
            }
            errors.Add(new UnexpectedTokenError(token, expected, lexer.CurrentLine, lexer.CurrentColumn));
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public override AST.CSTNode Parse()
        {
            //this.reducer = new Reduce(ReduceAST);
            this.getSemObj = new GetSemObj(GetSemCST);
            object result = Execute();
            if (result == null)
                return null;
            return (result as AST.SPPFNode).GetFirstTree().ApplyActions();
        }

        /// <summary>
        /// Parses the input and returns whether the input is recognized
        /// </summary>
        /// <returns>True if the input is recognized, false otherwise</returns>
        public override bool Recognize()
        {
            //this.reducer = new Reduce(ReduceSimple);
            this.getSemObj = new GetSemObj(GetSemNaked);
            return (Execute() != null);
        }

        private object GetSemCST(Symbols.Symbol symbol) { return new AST.CSTNode(symbol); }
        private object GetSemNaked(Symbols.Symbol symbol) { return symbol; }

        private AST.SPPFNode Execute()
        {
            nextToken = lexer.GetNextToken();
            if (nextToken.SymbolID == Symbols.Dollar.Instance.SymbolID)
            {
                // the input is empty!
                if (parserAutomaton.IsAcceptingState(0))
                    return nullables[parserAutomaton.Axiom];
                return null;
            }
            GSSNode v0 = new GSSNode(0, 0);
            List<GSSNode> Ui = new List<GSSNode>();
            Ui.Add(v0);
            R = new LinkedList<Reduction>();
            Q = new LinkedList<Shift>();
            N = new List<AST.SPPFNode>();

            for (int i = 0; i != parserAutomaton.GetActionsCount(0, nextToken.SymbolID); i++)
            {
                ushort action = 0;
                ushort data = parserAutomaton.GetAction(0, nextToken.SymbolID, i, out action);
                if (action == LRActions.Shift)
                    Q.AddLast(new Shift(v0, data));
                else if (action == LRActions.Reduce)
                    R.AddLast(new Reduction(v0, parserAutomaton.GetProduction(data), epsilon));
            }

            int generation = 0;
            while (nextToken.SymbolID != Symbols.Epsilon.Instance.SymbolID) // Wait for ε token
            {
                N.Clear();
                while (R.Count != 0)
                    Reducer(Ui, generation);
                Symbols.Token oldtoken = nextToken;
                nextToken = lexer.GetNextToken();
                List<GSSNode> Uj = Shifter(Ui, oldtoken, generation);
                generation++;
                if (Uj.Count == 0)
                {
                    // Generation is empty !
                    OnUnexpectedToken(Ui, oldtoken);
                    return null;
                }
                Ui = Uj;
            }

            foreach (GSSNode node in Ui)
            {
                if (parserAutomaton.IsAcceptingState(node.State))
                {
                    // Has reduction _Axiom_ -> axiom $ . on ε
                    List<List<GSSNode>> paths = node.GetPaths(2);
                    List<GSSNode> path = paths[0];
                    AST.SPPFNode root = path[path.Count - 2].Edges[path[path.Count - 1]];
                    return root;
                }
            }
            // At end of input but was still waiting for tokens
            return null;
        }

        private void Reducer(List<GSSNode> Ui, int generation)
        {
            GSSNode v = R.First.Value.node;
            LRProduction rule = R.First.Value.prod;
            Symbols.Variable X = parserVariables[rule.Head];
            ushort m = rule.ReductionLength;
            AST.SPPFNode y = R.First.Value.first;
            R.RemoveFirst();
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
                ushort k = u.State;
                ushort l = GetNextByVar(k, X.SymbolID);
                AST.SPPFNode z = null;
                if (m != 0)
                {
                    int c = u.Generation;
                    z = GetInSet(new AST.SPPFNode(X, c, (AST.CSTAction)rule.HeadAction));
                }
                GSSNode w = GetInSet(Ui, l);
                if (w != null)
                {
                    if (!w.Edges.ContainsKey(u))
                    {
                        w.AddEdge(u, z);
                        if (m != 0)
                        {
                            int ac = parserAutomaton.GetActionsCount(l, nextToken.SymbolID);
                            for (int i = 0; i != ac; i++)
                            {
                                ushort action = 0;
                                ushort data = parserAutomaton.GetAction(l, nextToken.SymbolID, i, out action);
                                if (action == LRActions.Reduce)
                                {
                                    LRProduction prod = parserAutomaton.GetProduction(data);
                                    if (prod.ReductionLength != 0)
                                        R.AddLast(new Reduction(u, prod, z));
                                }
                            }
                        }
                    }
                }
                else
                {
                    w = new GSSNode(l, generation);
                    Ui.Add(w);
                    w.AddEdge(u, z);
                    int ac = parserAutomaton.GetActionsCount(l, nextToken.SymbolID);
                    for (int i = 0; i != ac; i++)
                    {
                        ushort action = 0;
                        ushort data = parserAutomaton.GetAction(l, nextToken.SymbolID, i, out action);
                        if (action == LRActions.Shift)
                        {
                            Q.AddLast(new Shift(w, data));
                        }
                        else if (action == LRActions.Reduce)
                        {
                            LRProduction prod = parserAutomaton.GetProduction(data);
                            if (prod.ReductionLength == 0)
                                R.AddLast(new Reduction(w, prod, epsilon));
                            else if (m != 0)
                                R.AddLast(new Reduction(u, prod, z));
                        }
                    }
                }
                if (m != 0)
                {
                    ys.Reverse();
                    AddChildren(rule, z, ys);
                }
            }
        }

        private List<GSSNode> Shifter(List<GSSNode> Ui, Symbols.Token oldtoken, int generation)
        {
            List<GSSNode> Uj = new List<GSSNode>();
            LinkedList<Shift> Qp = new LinkedList<Shift>();
            AST.SPPFNode z = new AST.SPPFNode(oldtoken, generation);
            while (Q.Count != 0)
            {
                GSSNode v = Q.First.Value.node;
                ushort k = Q.First.Value.state;
                Q.RemoveFirst();
                GSSNode w = GetInSet(Uj, k);
                if (w != null)
                {
                    w.AddEdge(v, z);
                    for (int i = 0; i != parserAutomaton.GetActionsCount(k, nextToken.SymbolID); i++)
                    {
                        ushort action = 0;
                        ushort data = parserAutomaton.GetAction(k, nextToken.SymbolID, i, out action);
                        if (action == LRActions.Reduce)
                        {
                            LRProduction prod = parserAutomaton.GetProduction(data);
                            if (prod.ReductionLength != 0)
                                R.AddLast(new Reduction(v, prod, z));
                        }
                    }
                }
                else
                {
                    w = new GSSNode(k, v.Generation + 1);
                    w.AddEdge(v, z);
                    Uj.Add(w);
                    int ac = parserAutomaton.GetActionsCount(k, nextToken.SymbolID);
                    for (int i = 0; i != ac; i++)
                    {
                        ushort action = 0;
                        ushort data = parserAutomaton.GetAction(k, nextToken.SymbolID, i, out action);
                        if (action == LRActions.Shift)
                            Qp.AddLast(new Shift(w, data));
                        else if (action == LRActions.Reduce)
                        {
                            LRProduction prod = parserAutomaton.GetProduction(data);
                            if (prod.ReductionLength == 0)
                                R.AddLast(new Reduction(w, prod, epsilon));
                            else
                                R.AddLast(new Reduction(v, prod, z));
                        }
                    }
                }
            }
            Q = Qp;
            return Uj;
        }

        private void AddChildren(LRProduction rule, AST.SPPFNode y, List<AST.SPPFNode> ys)
        {
            AST.SPPFFamily family = new AST.SPPFFamily(y);
            y.AddFamily(family);
            int index = 0;
            for (int i = 0; i != rule.Bytecode.Length; i++)
            {
                ushort op = rule.Bytecode[i];
                if (LRBytecode.IsPop(op))
                {
                    family.AddChild(ys[index++]);
                }
                else if (LRBytecode.IsAddVirtual(op))
                {
                    family.AddChild(new AST.SPPFNode(parserVirtuals[rule.Bytecode[i + 1]], y.Generation));
                    i++;
                }
                else if (LRBytecode.IsSemAction(op))
                {
                    family.AddChild(new AST.SPPFNode(new Symbols.Action(parserActions[rule.Bytecode[i + 1]]), 0));
                    i++;
                }
                else if (LRBytecode.IsAddNullVariable(op))
                {
                    family.AddChild(nullables[rule.Bytecode[i + 1]]);
                    i++;
                }
            }
        }

        private ushort GetNextByVar(ushort state, ushort var)
        {
            int ac = parserAutomaton.GetActionsCount(state, var);
            for (int i=0; i!=ac; i++)
            {
                ushort action = 0;
                ushort data = parserAutomaton.GetAction(state, var, i, out action);
                if (action == LRActions.Shift)
                    return data;
            }
            return 0xFFFF;
        }

        private GSSNode GetInSet(List<GSSNode> set, ushort label)
        {
            foreach (GSSNode node in set)
                if (node.State == label)
                    return node;
            return null;
        }

        private AST.SPPFNode GetInSet(AST.SPPFNode node)
        {
            foreach (AST.SPPFNode potential in N)
                if (potential.EquivalentTo(node))
                    return potential;
            N.Add(node);
            return node;
        }
    }
}
