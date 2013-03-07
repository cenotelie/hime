/*
 * Author: Laurent Wouters
 * Date: 02/06/2012
 * Time: 10:15
 * 
 */
using System.IO;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base for all LR(k) parsers
    /// </summary>
    public abstract class LRkParser : BaseLRParser
    {
        private delegate object GetSemObj(Symbols.Symbol symbol);
        private delegate object Reduce(LRProduction production);

        private class Simulator : LRkSimulator
        {
            public Simulator(LRkParser parser)
            {
                this.parserAutomaton = parser.parserAutomaton;
                this.parserVariables = parser.parserVariables;
                this.input = parser.input;
                this.stack = new ushort[maxStackSize];
                this.head = parser.head;
                System.Array.Copy(parser.stack, this.stack, parser.head + 1);
            }
        }

        private LRkAutomaton parserAutomaton;
        private RewindableTokenStream input;
        private ushort[] stack;
        private object[] objects;
        private Symbols.Symbol[] buffer;
        private int head;
        private Reduce reducer;
        private GetSemObj getSemObj;

        /// <summary>
        /// Initializes a new instance of the LRkParser class with the given lexer
        /// </summary>
        /// <param name="automaton">The parser's automaton</param>
        /// <param name="variables">The parser's variables</param>
        /// <param name="virtuals">The parser's virtuals</param>
        /// <param name="actions">The parser's actions</param>
        /// <param name="lexer">The input lexer</param>
        public LRkParser(LRkAutomaton automaton, Symbols.Variable[] variables, Symbols.Virtual[] virtuals, SemanticAction[] actions, Lexer.TextLexer lexer)
            : base(variables, virtuals, actions, lexer)
        {
            this.parserAutomaton = automaton;
            this.input = new RewindableTokenStream(lexer);
        }

        private Symbols.Token OnUnexpectedToken(Symbols.Token token)
        {
            List<int> expectedIDs = parserAutomaton.GetExpected(stack[head], lexer.Terminals.Count);
            List<Symbols.Terminal> expected = new List<Symbols.Terminal>();
            foreach (int index in expectedIDs)
                expected.Add(lexer.Terminals[index]);
            errors.Add(new UnexpectedTokenError(token, expected, lexer.CurrentLine, lexer.CurrentColumn));
            if (!tryRecover) return null;
            if (TryDrop1Unexpected()) return input.GetNextToken();
            if (TryDrop2Unexpected()) return input.GetNextToken();
            foreach (Symbols.Terminal terminal in expected)
            {
                Symbols.TextToken dummy = new Symbols.TextToken(terminal.SymbolID, terminal.Name, string.Empty, 0, 0);
                if (TryInsertExpected(dummy))
                    return dummy;
            }
            return null;
        }
        private bool TryDrop1Unexpected()
        {
            int used = 0;
            bool success = (new Simulator(this)).TestForLength(3, null, out used);
            input.Rewind(used);
            return success;
        }
        private bool TryDrop2Unexpected()
        {
            input.GetNextToken();
            int used = 0;
            bool success = (new Simulator(this)).TestForLength(3, null, out used);
            input.Rewind(used);
            if (!success)
                input.Rewind(1);
            return success;
        }
        private bool TryInsertExpected(Symbols.Token terminal)
        {
            int used = 0;
            bool success = (new Simulator(this)).TestForLength(3, terminal, out used);
            input.Rewind(used);
            return success;
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
            return (result as AST.BuildNode).GetTree();
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

        private object GetSemCST(Symbols.Symbol symbol) { return new AST.BuildNode(symbol); }
        private object GetSemNaked(Symbols.Symbol symbol) { return symbol; }

        private object ReduceAST(LRProduction production)
        {
            Symbols.Variable var = parserVariables[production.Head];
            AST.BuildNode sub = new AST.BuildNode(var);
            sub.SetAction(production.HeadAction);
            int nextBuffer = 0;
            int nextStack = 0;
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                ushort op = production.Bytecode[i];
                if (LRBytecode.IsSemAction(op))
                {
                    parserActions[production.Bytecode[i + 1]](var, buffer, nextBuffer);
                    i++;
                }
                else if (LRBytecode.IsAddVirtual(op))
                {
                    Symbols.Symbol symbol = parserVirtuals[production.Bytecode[i + 1]];
                    AST.BuildNode node = new AST.BuildNode(symbol);
                    node.SetAction(op & LRBytecode.MaskAction);
                    sub.AppendChild(node);
                    buffer[nextBuffer] = symbol;
                    nextBuffer++;
                    i++;
                }
                else
                {
                    AST.BuildNode node = objects[head + nextStack + 1] as AST.BuildNode;
                    sub.AppendChild(node);
                    node.SetAction(op & LRBytecode.MaskAction);
                    buffer[nextBuffer] = node.Symbol;
                    nextStack++;
                    nextBuffer++;
                }
            }
            return sub;
        }

        private object ReduceSimple(LRProduction production)
        {
            Symbols.Variable var = parserVariables[production.Head];
            int nextBuffer = 0;
            int nextStack = 0;
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                ushort op = production.Bytecode[i];
                if (LRBytecode.IsSemAction(op))
                {
                    parserActions[production.Bytecode[i + 1]](var, buffer, nextBuffer);
                    i++;
                }
                else if (LRBytecode.IsAddVirtual(op))
                {
                    buffer[nextBuffer] = parserVirtuals[production.Bytecode[i + 1]];
                    nextBuffer++;
                    i++;
                }
                else
                {
                    buffer[nextBuffer] = objects[head + nextStack + 1] as Symbols.Symbol;
                    nextStack++;
                    nextBuffer++;
                }
            }
            return var;
        }

        private object Execute()
        {
            this.stack = new ushort[maxStackSize];
            this.objects = new object[maxStackSize];
            this.buffer = new Symbols.Symbol[maxBodyLength];
            Symbols.Token nextToken = input.GetNextToken();
            while (true)
            {
                ushort action = ExecuteOnToken(nextToken);
                if (action == LRActions.Shift)
                {
                    nextToken = input.GetNextToken();
                    continue;
                }
                if (action == LRActions.Accept)
                    return objects[head - 1];
                nextToken = OnUnexpectedToken(nextToken);
                if (nextToken == null || errors.Count >= maxErrorCount)
                    return null;
            }
        }

        private ushort ExecuteOnToken(Symbols.Token token)
        {
            while (true)
            {
                ushort action = LRActions.None;
                ushort data = parserAutomaton.GetAction(stack[head], token.SymbolID, out action);
                if (action == LRActions.Shift)
                {
                    head++;
                    stack[head] = data;
                    objects[head] = getSemObj(token);
                    return action;
                }
                else if (action == LRActions.Reduce)
                {
                    LRProduction production = parserAutomaton.GetProduction(data);
                    head -= production.ReductionLength;
                    object result = reducer(production);
                    data = parserAutomaton.GetAction(stack[head], parserVariables[production.Head].SymbolID, out action);
                    head++;
                    stack[head] = data;
                    objects[head] = result;
                    continue;
                }
                return action;
            }
        }
    }
}
