using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base for all RNGLR parsers
    /// </summary>
    public class RNGLRParser : BaseLRParser
    {
        // For reduction of length 0, the node is the GSS node on which it is applied, the first SPPF is epsilon
        // For others, the node is the SECOND GSS node on the path, not the head. The first SPPF is then the label on the transition from the head
        private struct Reduction
        {
            public GSSNode node;
            public LRProduction prod;
            public SubSPPF first;
            public Reduction(GSSNode node, LRProduction prod, SubSPPF first)
            {
                this.node = node;
                this.prod = prod;
                this.first = first;
            }
        }

        private struct Shift
        {
            public GSSNode from;
            public int to;
            public Shift(GSSNode from, int to)
            {
                this.from = from;
                this.to = to;
            }
        }

        private RNGLRAutomaton parserAutomaton;
        private GSS stack;
        private SPPFBuilder builder;
        private Symbols.Token nextToken;
        private Queue<Reduction> reductions;
        private Queue<Shift> shifts;

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
            this.stack = new GSS(automaton.StatesCount);
            this.builder = new SPPFBuilder(variables.Length);
            for (int i = 0; i != parserAutomaton.Nullables.Count; i++)
            {
                ushort index = parserAutomaton.Nullables[i];
                if (index != 0xFFFF)
                {
                    LRProduction prod = parserAutomaton.GetProduction(index);
                    builder.CreateNullProduction(prod, parserVariables[i]);
                }
            }
            for (int i = 0; i != parserAutomaton.Nullables.Count; i++)
            {
                ushort index = parserAutomaton.Nullables[i];
                if (index != 0xFFFF)
                {
                    LRProduction prod = parserAutomaton.GetProduction(index);
                    BuildNullable(i, prod);
                }
            }
        }

        private void BuildNullable(int index, LRProduction production)
        {
            builder.ReductionPrepare();
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                LROpCode op = production.Bytecode[i];
                if (op.IsSemAction)
                {
                    builder.ReductionSemantic(parserActions[production.Bytecode[i + 1].Value]);
                    i++;
                }
                else if (op.IsAddVirtual)
                {
                    Symbols.Virtual symbol = parserVirtuals[production.Bytecode[i + 1].Value];
                    builder.ReductionVirtual(symbol, op.TreeAction);
                    i++;
                }
                else if (op.IsAddNullVar)
                {
                    builder.ReductionNullVariable(parserVariables[production.Bytecode[i + 1].Value], op.TreeAction);
                    i++;
                }
                else
                {
                    builder.ReductionPop(op.TreeAction);
                }
            }
            builder.Reduce(index, production.HeadAction);
        }

        private void OnUnexpectedToken(GSSGeneration gen, Symbols.Token token)
        {
            List<int> indices = new List<int>();
            List<Symbols.Terminal> expected = new List<Symbols.Terminal>();
            foreach (GSSNode node in gen)
            {
                ICollection<int> temp = parserAutomaton.GetExpected(node.State, lexer.Terminals.Count);
                foreach (int index in temp)
                {
                    if (!indices.Contains(index))
                    {
                        indices.Add(index);
                        expected.Add(lexer.Terminals[index]);
                    }
                }
            }
            allErrors.Add(new UnexpectedTokenError(token, expected));
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public override ParseTree Parse()
        {
            nextToken = lexer.GetNextToken();
            if (nextToken.SymbolID == Symbols.Dollar.Instance.SymbolID)
            {
                // the input is empty!
                if (parserAutomaton.IsAcceptingState(0))
                    return null; // return nullProds[parserAutomaton.Axiom].value;
                return null;
            }

            reductions = new Queue<Reduction>();
            shifts = new Queue<Shift>();
            GSSGeneration Ui = stack.GetNextGen();
            GSSNode v0 = Ui.CreateNode(0);

            int count = parserAutomaton.GetActionsCount(0, nextToken.SymbolID);
            for (int i = 0; i != count; i++)
            {
                LRAction action = parserAutomaton.GetAction(0, nextToken.SymbolID, i);
                if (action.Code == LRActionCode.Shift)
                    shifts.Enqueue(new Shift(v0, action.Data));
                else if (action.Code == LRActionCode.Reduce)
                    reductions.Enqueue(new Reduction(v0, parserAutomaton.GetProduction(action.Data), builder.Epsilon));
            }

            while (nextToken.SymbolID != Symbols.Epsilon.Instance.SymbolID) // Wait for ε token
            {
                Reducer(Ui);
                Symbols.Token oldtoken = nextToken;
                nextToken = lexer.GetNextToken();
                GSSGeneration Uj = Shifter(oldtoken);
                if (Uj.Size == 0)
                {
                    // Generation is empty !
                    OnUnexpectedToken(Ui, oldtoken);
                    return null;
                }
                Ui.Sweep();
                Ui = Uj;
            }

            foreach (GSSNode node in Ui)
            {
                if (parserAutomaton.IsAcceptingState(node.State))
                {
                    // Has reduction _Axiom_ -> axiom $ . on ε
                    GSSPaths paths = stack.GetPaths(node, 2);
                    return builder.GetTree(paths[0][1]);
                }
            }
            // At end of input but was still waiting for tokens
            return null;
        }

        private void Reducer(GSSGeneration generation)
        {
            builder.ClearHistory();
            while (reductions.Count != 0)
                ExecuteReduction(generation, reductions.Dequeue());
        }

        private void ExecuteReduction(GSSGeneration generation, Reduction reduction)
        {
            // Get all path from the reduction node
            GSSPaths paths = new GSSPaths();
            if (reduction.prod.ReductionLength == 0)
                paths = stack.GetPaths(reduction.node, 0);
            else
                // The given GSS node is the second on the path, so start from it with length - 1
                paths = stack.GetPaths(reduction.node, reduction.prod.ReductionLength - 1);
            
            // Execute the reduction on all paths
            for (int i = 0; i != paths.Count; i++)
                ExecuteReduction(generation, reduction, paths[i]);
        }

        private void ExecuteReduction(GSSGeneration generation, Reduction reduction, GSSPath path)
        {
            // Get the rule's head
            Symbols.Variable head = parserVariables[reduction.prod.Head];
            // Build the SPPF
            builder.ReductionPrepare(path, reduction.first);
            for (int i = 0; i != reduction.prod.Bytecode.Length; i++)
            {
                LROpCode op = reduction.prod.Bytecode[i];
                if (op.IsSemAction)
                {
                    builder.ReductionSemantic(parserActions[reduction.prod.Bytecode[i + 1].Value]);
                    i++;
                }
                else if (op.IsAddVirtual)
                {
                    Symbols.Virtual symbol = parserVirtuals[reduction.prod.Bytecode[i + 1].Value];
                    builder.ReductionVirtual(symbol, op.TreeAction);
                    i++;
                }
                else if (op.IsAddNullVar)
                {
                    builder.ReductionNullVariable(parserVariables[reduction.prod.Bytecode[i + 1].Value], op.TreeAction);
                    i++;
                }
                else
                {
                    builder.ReductionPop(op.TreeAction);
                }
            }
            SubSPPF subRoot = builder.Reduce(head, reduction.prod.HeadAction);

            // Get the target state by transition on the rule's head
            int to = GetNextByVar(path.Last.State, head.SymbolID);
            if (generation.Contains(to))
            {
                // A node for the target state is already in the GSS
                GSSNode w = generation[to];
                // But the new edge does not exist
                if (!w.HasEdgeTo(path.Last))
                {
                    w.AddEdge(path.Last, subRoot);
                    // Look for the new reductions at this state
                    if (reduction.prod.ReductionLength != 0)
                    {
                        int count = parserAutomaton.GetActionsCount(to, nextToken.SymbolID);
                        for (int i = 0; i != count; i++)
                        {
                            LRAction action = parserAutomaton.GetAction(to, nextToken.SymbolID, i);
                            if (action.Code == LRActionCode.Reduce)
                            {
                                LRProduction prod = parserAutomaton.GetProduction(action.Data);
                                // length 0 reduction are not considered here because they already exist at this point
                                if (prod.ReductionLength != 0)
                                    reductions.Enqueue(new Reduction(path.Last, prod, subRoot));
                            }
                        }
                    }
                }
            }
            else
            {
                // Create the new corresponding node in the GSS
                GSSNode w = generation.CreateNode(to);
                w.AddEdge(path.Last, subRoot);
                // Look for all the reductions and shifts at this state
                int count = parserAutomaton.GetActionsCount(to, nextToken.SymbolID);
                for (int i = 0; i != count; i++)
                {
                    LRAction action = parserAutomaton.GetAction(to, nextToken.SymbolID, i);
                    if (action.Code == LRActionCode.Shift)
                    {
                        shifts.Enqueue(new Shift(w, action.Data));
                    }
                    else if (action.Code == LRActionCode.Reduce)
                    {
                        LRProduction prod = parserAutomaton.GetProduction(action.Data);
                        if (prod.ReductionLength == 0)
                            reductions.Enqueue(new Reduction(w, prod, builder.Epsilon));
                        else if (reduction.prod.ReductionLength != 0)
                            reductions.Enqueue(new Reduction(path.Last, prod, subRoot));
                    }
                }
            }
        }

        private GSSGeneration Shifter(Symbols.Token oldtoken)
        {
            // Create next generation
            GSSGeneration gen = stack.GetNextGen();
            // Create the AST for the old token
            SubSPPF ast = builder.AcquireSingleNode(oldtoken);

            // Execute all shifts in the queue at this point
            int count = shifts.Count;
            for (int i = 0; i != count; i++)
                ExecuteShift(gen, ast, shifts.Dequeue());
            return gen;
        }

        private void ExecuteShift(GSSGeneration gen, SubSPPF ast, Shift shift)
        {
            if (gen.Contains(shift.to))
            {
                // A node for the target state is already in the GSS
                GSSNode w = gen[shift.to];
                w.AddEdge(shift.from, ast);
                // Look for the new reductions at this state
                int count = parserAutomaton.GetActionsCount(shift.to, nextToken.SymbolID);
                for (int i = 0; i != count; i++)
                {
                    LRAction action = parserAutomaton.GetAction(shift.to, nextToken.SymbolID, i);
                    if (action.Code == LRActionCode.Reduce)
                    {
                        LRProduction prod = parserAutomaton.GetProduction(action.Data);
                        // length 0 reduction are not considered here because they already exist at this point
                        if (prod.ReductionLength != 0)
                            reductions.Enqueue(new Reduction(shift.from, prod, ast));
                    }
                }
            }
            else
            {
                // Create the new corresponding node in the GSS
                GSSNode w = gen.CreateNode(shift.to);
                w.AddEdge(shift.from, ast);
                // Look for all the reductions and shifts at this state
                int count = parserAutomaton.GetActionsCount(shift.to, nextToken.SymbolID);
                for (int i = 0; i != count; i++)
                {
                    LRAction action = parserAutomaton.GetAction(shift.to, nextToken.SymbolID, i);
                    if (action.Code == LRActionCode.Shift)
                        shifts.Enqueue(new Shift(w, action.Data));
                    else if (action.Code == LRActionCode.Reduce)
                    {
                        LRProduction prod = parserAutomaton.GetProduction(action.Data);
                        if (prod.ReductionLength == 0) // Length 0 => reduce from the head
                            reductions.Enqueue(new Reduction(w, prod, builder.Epsilon));
                        else // reduce from the second node on the path
                            reductions.Enqueue(new Reduction(shift.from, prod, ast));
                    }
                }
            }
        }

        private int GetNextByVar(int state, int var)
        {
            int ac = parserAutomaton.GetActionsCount(state, var);
            for (int i = 0; i != ac; i++)
            {
                LRAction action = parserAutomaton.GetAction(state, var, i);
                if (action.Code == LRActionCode.Shift)
                    return action.Data;
            }
            return 0xFFFF;
        }
    }
}
