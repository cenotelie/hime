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
            public GSSNode from;
            public int to;
            public Shift(GSSNode from, int to)
            {
                this.from = from;
                this.to = to;
            }
        }

        private struct NodeDic
        {
            public int generation;
            public List<AST.SPPFNode> nodes;
        }

        private RNGLRAutomaton parserAutomaton;
        private AST.SPPFNode epsilon;
        private AST.SPPFNode[] nullProds;
        private Dictionary<int, AST.SPPFNode> nullVars;
        private Symbols.Token nextToken;
        private Queue<Reduction> queueReductions;
        private Queue<Shift> queueShifts;
        private List<NodeDic> objects;
        private AST.SPPFNode[] bufferNodes;
        private Symbols.Symbol[] bufferSymbols;

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
            this.epsilon = new AST.SPPFNode(Symbols.Epsilon.Instance);
            this.nullProds = new AST.SPPFNode[variables.Length];
            this.nullVars = new Dictionary<int, AST.SPPFNode>();
            this.bufferNodes = new AST.SPPFNode[maxBodyLength];
            this.bufferSymbols = new Symbols.Symbol[maxBodyLength];
            for (ushort i = 0; i != parserAutomaton.Nullables.Count; i++)
            {
                ushort index = parserAutomaton.Nullables[i];
                if (index != 0xFFFF)
                {
                    LRProduction prod = parserAutomaton.GetProduction(index);
                    nullProds[i] = new AST.SPPFNode(parserVariables[prod.Head]);
                    nullProds[i].SetAction(prod.HeadAction);
                    if (!nullVars.ContainsKey(nullProds[i].SymbolID))
                        nullVars.Add(nullProds[i].SymbolID, nullProds[i]);
                }
            }
            for (ushort i = 0; i != parserAutomaton.Nullables.Count; i++)
            {
                ushort index = parserAutomaton.Nullables[i];
                if (index != 0xFFFF)
                {
                    LRProduction prod = parserAutomaton.GetProduction(index);
                    BuildNullable(nullProds[i], prod);
                }
            }
        }

        private void BuildNullable(AST.SPPFNode subRoot, LRProduction production)
        {
            int nextBuffer = 0;
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                LROpCode op = production.Bytecode[i];
                if (op.IsSemAction)
                {
                    parserActions[production.Bytecode[i + 1].Value](subRoot.Value.Symbol as Symbols.Variable, bufferSymbols, nextBuffer);
                    i++;
                }
                else if (op.IsAddVirtual)
                {
                    Symbols.Symbol symbol = parserVirtuals[production.Bytecode[i + 1].Value];
                    AST.SPPFNode node = new AST.SPPFNode(symbol);
                    node.SetAction(op.TreeAction);
                    bufferSymbols[nextBuffer] = symbol;
                    bufferNodes[nextBuffer] = node;
                    nextBuffer++;
                    i++;
                }
                else if (op.IsAddNullVar)
                {
                    AST.SPPFNode node = nullProds[production.Bytecode[i + 1].Value];
                    node.SetAction(op.TreeAction);
                    bufferSymbols[nextBuffer] = node.Value.Symbol;
                    bufferNodes[nextBuffer] = node;
                    nextBuffer++;
                    i++;
                }
            }
            subRoot.Build(bufferNodes, nextBuffer);
        }

        private void OnUnexpectedToken(Dictionary<int, GSSNode> Ui, Symbols.Token token)
        {
            List<int> indices = new List<int>();
            List<Symbols.Terminal> expected = new List<Symbols.Terminal>();
            foreach (int state in Ui.Keys)
            {
                ICollection<int> temp = parserAutomaton.GetExpected(state, lexer.Terminals.Count);
                foreach (int index in temp)
                {
                    if (!indices.Contains(index))
                    {
                        indices.Add(index);
                        expected.Add(lexer.Terminals[index]);
                    }
                }
            }
            allErrors.Add(new UnexpectedTokenError(token, expected, lexer.CurrentLine, lexer.CurrentColumn));
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public override AST.ASTNode Parse()
        {
            nextToken = lexer.GetNextToken();
            if (nextToken.SymbolID == Symbols.Dollar.Instance.SymbolID)
            {
                // the input is empty!
                if (parserAutomaton.IsAcceptingState(0))
                    return nullProds[parserAutomaton.Axiom].Value;
                return null;
            }

            queueReductions = new Queue<Reduction>();
            queueShifts = new Queue<Shift>();
            objects = new List<NodeDic>();
            GSSNode v0 = new GSSNode(0, 0);
            Dictionary<int, GSSNode> Ui = new Dictionary<int, GSSNode>();
            Ui.Add(0, v0);

            int count = parserAutomaton.GetActionsCount(0, nextToken.SymbolID);
            for (int i = 0; i != count; i++)
            {
                LRAction action = parserAutomaton.GetAction(0, nextToken.SymbolID, i);
                if (action.Code == LRActionCode.Shift)
                    queueShifts.Enqueue(new Shift(v0, action.Data));
                else if (action.Code == LRActionCode.Reduce)
                    queueReductions.Enqueue(new Reduction(v0, parserAutomaton.GetProduction(action.Data), epsilon));
            }

            int generation = 0;
            while (nextToken.SymbolID != Symbols.Epsilon.Instance.SymbolID) // Wait for ε token
            {
                objects.Clear();
                Reducer(Ui, generation);
                Symbols.Token oldtoken = nextToken;
                nextToken = lexer.GetNextToken();
                Dictionary<int, GSSNode> Uj = Shifter(oldtoken);
                generation++;
                if (Uj.Count == 0)
                {
                    // Generation is empty !
                    OnUnexpectedToken(Ui, oldtoken);
                    return null;
                }
                Ui = Uj;
            }

            foreach (GSSNode node in Ui.Values)
            {
                if (parserAutomaton.IsAcceptingState(node.State))
                {
                    // Has reduction _Axiom_ -> axiom $ . on ε
                    GSSPath[] paths = node.GetPaths(2, out count);
                    return paths[0].labels[1].Value;
                }
            }
            // At end of input but was still waiting for tokens
            return null;
        }

        private void Reducer(Dictionary<int, GSSNode> Ui, int generation)
        {
            while (queueReductions.Count != 0)
                ExecuteReduction(Ui, generation, queueReductions.Dequeue());
        }

        private void ExecuteReduction(Dictionary<int, GSSNode> Ui, int generation, Reduction reduction)
        {
            // Get all path from the reduction node
            GSSPath[] paths = null;
            int count = 1;
            if (reduction.prod.ReductionLength == 0)
                paths = reduction.node.GetPaths0();
            else
                // The given GSS node is the second on the path, so start from it with length -1
                paths = reduction.node.GetPaths(reduction.prod.ReductionLength - 1, out count);
            // Execute the reduction on all paths
            for (int i = 0; i != count; i++)
                ExecuteReduction(Ui, generation, reduction, paths[i]);
        }

        private void ExecuteReduction(Dictionary<int, GSSNode> Ui, int generation, Reduction reduction, GSSPath path)
        {
            // Get the rule's head
            Symbols.Variable head = parserVariables[reduction.prod.Head];
            // Find or build the sub root SPPF
            AST.SPPFNode subRoot = null;
            bool isNewRoot = false;
            if (reduction.prod.ReductionLength != 0)
            {
                subRoot = ResolveSPPF(path.last.Generation, head, out isNewRoot);
                subRoot.SetAction(reduction.prod.HeadAction);
            }
            else
            {
                // find the nullable sub root
                subRoot = nullVars[head.SymbolID];
            }
            // Build the SPPF
            int nextBuffer = 0;
            int nextStack = 0;
            for (int i = 0; i != reduction.prod.Bytecode.Length; i++)
            {
                LROpCode op = reduction.prod.Bytecode[i];
                if (op.IsSemAction)
                {
                    parserActions[reduction.prod.Bytecode[i + 1].Value](head, bufferSymbols, nextBuffer);
                    i++;
                }
                else if (op.IsAddVirtual)
                {
                    Symbols.Symbol symbol = parserVirtuals[reduction.prod.Bytecode[i + 1].Value];
                    AST.SPPFNode node = new AST.SPPFNode(symbol);
                    node.SetAction(op.TreeAction);
                    bufferSymbols[nextBuffer] = symbol;
                    bufferNodes[nextBuffer] = node;
                    nextBuffer++;
                    i++;
                }
                else if (op.IsAddNullVar)
                {
                    AST.SPPFNode node = nullProds[reduction.prod.Bytecode[i + 1].Value];
                    node.SetAction(op.TreeAction);
                    bufferSymbols[nextBuffer] = node.Value.Symbol;
                    bufferNodes[nextBuffer] = node;
                    nextBuffer++;
                    i++;
                }
                else
                {
                    AST.SPPFNode node = null;
                    if (nextStack >= path.labels.Length) node = reduction.first;
                    else node = path.labels[path.labels.Length - nextStack - 1];
                    node.SetAction(op.TreeAction);
                    bufferSymbols[nextBuffer] = node.Value.Symbol;
                    bufferNodes[nextBuffer] = node;
                    nextStack++;
                    nextBuffer++;
                }
            }
            if (isNewRoot)
                subRoot.Build(bufferNodes, nextBuffer);

            // Get the target state by transition on the rule's head
            int to = GetNextByVar(path.last.State, head.SymbolID);
            if (Ui.ContainsKey(to))
            {
                // A node for the target state is already in the GSS
                GSSNode w = Ui[to];
                // But the new edge does not exist
                if (!w.HasEdgeTo(path.last))
                {
                    w.AddEdge(path.last, subRoot);
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
                                    queueReductions.Enqueue(new Reduction(path.last, prod, subRoot));
                            }
                        }
                    }
                }
            }
            else
            {
                // Create the new corresponding node in the GSS
                GSSNode w = new GSSNode(to, generation);
                Ui.Add(to, w);
                w.AddEdge(path.last, subRoot);
                // Look for all the reductions and shifts at this state
                int count = parserAutomaton.GetActionsCount(to, nextToken.SymbolID);
                for (int i = 0; i != count; i++)
                {
                    LRAction action = parserAutomaton.GetAction(to, nextToken.SymbolID, i);
                    if (action.Code == LRActionCode.Shift)
                    {
                        queueShifts.Enqueue(new Shift(w, action.Data));
                    }
                    else if (action.Code == LRActionCode.Reduce)
                    {
                        LRProduction prod = parserAutomaton.GetProduction(action.Data);
                        if (prod.ReductionLength == 0)
                            queueReductions.Enqueue(new Reduction(w, prod, epsilon));
                        else if (reduction.prod.ReductionLength != 0)
                            queueReductions.Enqueue(new Reduction(path.last, prod, subRoot));
                    }
                }
            }
        }

        private Dictionary<int, GSSNode> Shifter(Symbols.Token oldtoken)
        {
            // Create next generation
            Dictionary<int, GSSNode> Uj = new Dictionary<int, GSSNode>();
            // Create the AST for the old token
            AST.SPPFNode ast = new AST.SPPFNode(oldtoken);

            // Execute all shifts in the queue at this point
            int count = queueShifts.Count;
            for (int x = 0; x != count; x++)
                ExecuteShift(Uj, ast, queueShifts.Dequeue());
            return Uj;
        }

        private void ExecuteShift(Dictionary<int, GSSNode> Uj, AST.SPPFNode ast, Shift shift)
        {
            if (Uj.ContainsKey(shift.to))
            {
                // A node for the target state is already in the GSS
                GSSNode w = Uj[shift.to];
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
                            queueReductions.Enqueue(new Reduction(shift.from, prod, ast));
                    }
                }
            }
            else
            {
                // Create the new corresponding node in the GSS
                GSSNode w = new GSSNode(shift.to, shift.from.Generation + 1);
                Uj.Add(shift.to, w);
                w.AddEdge(shift.from, ast);
                // Look for all the reductions and shifts at this state
                int count = parserAutomaton.GetActionsCount(shift.to, nextToken.SymbolID);
                for (int i = 0; i != count; i++)
                {
                    LRAction action = parserAutomaton.GetAction(shift.to, nextToken.SymbolID, i);
                    if (action.Code == LRActionCode.Shift)
                        queueShifts.Enqueue(new Shift(w, action.Data));
                    else if (action.Code == LRActionCode.Reduce)
                    {
                        LRProduction prod = parserAutomaton.GetProduction(action.Data);
                        if (prod.ReductionLength == 0) // Length 0 => reduce from the head
                            queueReductions.Enqueue(new Reduction(w, prod, epsilon));
                        else // reduce from the second node on the path
                            queueReductions.Enqueue(new Reduction(shift.from, prod, ast));
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

        private AST.SPPFNode ResolveSPPF(int generation, Symbols.Symbol symbol, out bool isNew)
        {
            isNew = false;
            foreach (NodeDic dic in objects)
            {
                if (dic.generation == generation)
                {
                    foreach (AST.SPPFNode node in dic.nodes)
                    {
                        if (node.SymbolID == symbol.SymbolID)
                            return node;
                    }
                    isNew = true;
                    AST.SPPFNode sppf = new AST.SPPFNode(symbol);
                    dic.nodes.Add(sppf);
                    return sppf;
                }
            }
            isNew = true;
            AST.SPPFNode nn = new AST.SPPFNode(symbol);
            NodeDic nd = new NodeDic();
            nd.generation = generation;
            nd.nodes = new List<AST.SPPFNode>();
            nd.nodes.Add(nn);
            return nn;
        }
    }
}
