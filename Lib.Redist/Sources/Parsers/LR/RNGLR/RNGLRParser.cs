/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

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
            public int first;
            public Reduction(GSSNode node, LRProduction prod, int first)
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
        private GSS gss;
        private SPPF sppf;
        private int[] sppfNullables;
        private Token nextToken;
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
        public RNGLRParser(RNGLRAutomaton automaton, Symbol[] variables, Symbol[] virtuals, UserAction[] actions, Lexer.Lexer lexer)
            : base(variables, virtuals, actions, lexer)
        {
            this.parserAutomaton = automaton;
            this.gss = new GSS(automaton.StatesCount);
            this.sppf = new SPPF(lexer.Output, parserVariables, parserVirtuals);

            // Create the constant SPPF nodes for the nullable variables
            this.sppfNullables = new int[variables.Length];
            for (int i = 0; i != parserAutomaton.Nullables.Count; i++)
            {
                ushort index = parserAutomaton.Nullables[i];
                if (index != 0xFFFF)
                {
                    LRProduction prod = parserAutomaton.GetProduction(index);
                    Symbol variable = parserVariables[i];
                    this.sppfNullables[i] = sppf.CreateNode(variable.ID, ASTGraph.TypeVariable, i, prod.HeadAction);
                }
            }
            
            // Build the trees for the nullables variables
            for (int i = 0; i != parserAutomaton.Nullables.Count; i++)
            {
                ushort index = parserAutomaton.Nullables[i];
                if (index != 0xFFFF)
                {
                    LRProduction prod = parserAutomaton.GetProduction(index);
                    BuildSPPF(sppfNullables[i], prod, new int(), null);
                }
            }
        }

        private void OnUnexpectedToken(GSSGeneration gen, Token token)
        {
            List<int> indices = new List<int>();
            List<Symbol> expected = new List<Symbol>();
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
            allErrors.Add(new UnexpectedTokenError(token, expected, lexer.Output));
        }

        private void BuildSPPF(int subRoot, LRProduction production, int first, GSSPath path)
        {
            // Setups the stack index for the pop actions
            int stackNext = -1;
            if (production.ReductionLength > 0)
                stackNext = production.ReductionLength - 2;
            // Execute the bytecode
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                /*LROpCode op = production.Bytecode[i];
                if (op.IsSemAction)
                {
                    int child = sppfActions[production.Bytecode[i + 1].Value];
                    sppf.AddChild(subRoot, child, TreeAction.Semantic);
                    i++;
                }
                else if (op.IsAddVirtual)
                {
                    Symbols.Virtual symbol = parserVirtuals[production.Bytecode[i + 1].Value];
                    int child = sppf.CreateNode(symbol);
                    sppf.AddChild(subRoot, child, op.TreeAction);
                    i++;
                }
                else if (op.IsAddNullVar)
                {
                    int child = sppfNullables[production.Bytecode[i + 1].Value];
                    sppf.AddChild(subRoot, child, op.TreeAction);
                    i++;
                }
                else
                {
                    int child = new int();
                    if (stackNext < 0)
                        child = first;
                    else
                        child = path[stackNext--];
                    sppf.AddChild(subRoot, child, op.TreeAction);
                }*/
            }
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public override ParseResult Parse()
        {
            nextToken = lexer.GetNextToken();
            if (nextToken.SymbolID == Lexer.Lexer.sidDollar)
            {
                // the input is empty!
                if (parserAutomaton.IsAcceptingState(0))
                    return null; // return nullProds[parserAutomaton.Axiom].value;
                return null;
            }

            reductions = new Queue<Reduction>();
            shifts = new Queue<Shift>();
            GSSGeneration Ui = gss.GetNextGen();
            GSSNode v0 = Ui.CreateNode(0);

            int count = parserAutomaton.GetActionsCount(0, nextToken.SymbolID);
            for (int i = 0; i != count; i++)
            {
                LRAction action = parserAutomaton.GetAction(0, nextToken.SymbolID, i);
                if (action.Code == LRActionCode.Shift)
                    shifts.Enqueue(new Shift(v0, action.Data));
                else if (action.Code == LRActionCode.Reduce)
                    reductions.Enqueue(new Reduction(v0, parserAutomaton.GetProduction(action.Data), sppf.Epsilon));
            }

            while (nextToken.SymbolID != Lexer.Lexer.sidEpsilon) // Wait for ε token
            {
                Reducer(Ui);
                Token oldtoken = nextToken;
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
                    GSSPaths paths = gss.GetPaths(node, 2);
                    return null;
                    //return new ParseResult(allErrors, lexer.Input, sppf.BuildTreeFrom(paths[0][1]));
                }
            }
            // At end of input but was still waiting for tokens
            return new ParseResult(allErrors, lexer.Output);
        }

        private void Reducer(GSSGeneration generation)
        {
            sppf.ClearHistory();
            while (reductions.Count != 0)
                ExecuteReduction(generation, reductions.Dequeue());
        }

        private void ExecuteReduction(GSSGeneration generation, Reduction reduction)
        {
            // Get all path from the reduction node
            GSSPaths paths = new GSSPaths();
            if (reduction.prod.ReductionLength == 0)
                paths = gss.GetPaths(reduction.node, 0);
            else
                // The given GSS node is the second on the path, so start from it with length - 1
                paths = gss.GetPaths(reduction.node, reduction.prod.ReductionLength - 1);
            
            // Execute the reduction on all paths
            for (int i = 0; i != paths.Count; i++)
                ExecuteReduction(generation, reduction, paths[i]);
        }

        private void ExecuteReduction(GSSGeneration generation, Reduction reduction, GSSPath path)
        {
            // Get the rule's head
            Symbol head = parserVariables[reduction.prod.Head];
            // Resolve the sub-root
            int subRoot = sppf.Resolve(generation.Index, head.ID, reduction.prod.Head, reduction.prod.HeadAction);
            // Build the SPPF if this is not a new node
            if (!sppf.HistoryHit)
                BuildSPPF(subRoot, reduction.prod, reduction.first, path);

            // Get the target state by transition on the rule's head
            int to = GetNextByVar(path.Last.State, head.ID);
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
                            reductions.Enqueue(new Reduction(w, prod, sppf.Epsilon));
                        else if (reduction.prod.ReductionLength != 0)
                            reductions.Enqueue(new Reduction(path.Last, prod, subRoot));
                    }
                }
            }
        }

        private GSSGeneration Shifter(Token oldtoken)
        {
            // Create next generation
            GSSGeneration gen = gss.GetNextGen();
            // Create the AST for the old token
            int ast = sppf.CreateNode(oldtoken.SymbolID, ASTGraph.TypeToken, oldtoken.Index);

            // Execute all shifts in the queue at this point
            int count = shifts.Count;
            for (int i = 0; i != count; i++)
                ExecuteShift(gen, ast, shifts.Dequeue());
            return gen;
        }

        private void ExecuteShift(GSSGeneration gen, int ast, Shift shift)
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
                            reductions.Enqueue(new Reduction(w, prod, sppf.Epsilon));
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
