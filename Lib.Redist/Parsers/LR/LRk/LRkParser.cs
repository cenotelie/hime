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

        /// <summary>
        /// LR(k) parsing table and productions
        /// </summary>
        private LRkAutomaton parserAutomaton;
        /// <summary>
        /// Parser's input encapsulating the lexer
        /// </summary>
        private RewindableTokenStream input;
        /// <summary>
        /// Parser's stack
        /// </summary>
        private ushort[] stack;
        /// <summary>
        /// Semantic symbols' stack
        /// </summary>
        private object[] objects;
        /// <summary>
        /// Buffer for the rules' body
        /// </summary>
        private object[] buffer;
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

        /// <summary>
        /// Initializes a new instance of the LRkParser class with the given lexer
        /// </summary>
        /// <param name="automaton">The parser's automaton</param>
        /// <param name="variables">The parser's variables</param>
        /// <param name="virtuals">The parser's virtuals</param>
        /// <param name="actions">The parser's actions</param>
        /// <param name="lexer">The input lexer</param>
        public LRkParser(LRkAutomaton automaton, Symbols.Variable[] variables, Symbols.Virtual[] virtuals, SemanticAction[] actions, Lexer.ILexer lexer)
            : base(variables, virtuals, actions, lexer)
        {
            this.parserAutomaton = automaton;
            this.input = new RewindableTokenStream(lexer);
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

        private object ReduceAST(LRProduction production)
        {
            AST.CSTNode sub = new AST.CSTNode(parserVariables[production.Head], (AST.CSTAction)production.HeadAction);
            int nextBuffer = 0;
            int nextStack = 0;
            for (int i = 0; i != production.Bytecode.SizeData; i++)
            {
                ushort op = production.Bytecode[i];
                if (op == LRProduction.SemanticAction)
                {
                    ushort index = production.Bytecode[i + 1];
                    parserActions[index](sub, buffer, nextBuffer);
                    i++;
                }
                else if (op >= LRProduction.Virtual)
                {
                    ushort index = production.Bytecode[i + 1];
                    AST.CSTNode node = new AST.CSTNode(parserVirtuals[index], (AST.CSTAction)(op - 4));
                    sub.AppendChild(node);
                    buffer[nextBuffer] = node;
                    nextBuffer++;
                    i++;
                }
                else if (op == LRProduction.PopNoAction)
                {
                    AST.CSTNode node = objects[head + nextStack + 1] as AST.CSTNode;
                    sub.AppendChild(node);
                    buffer[nextBuffer] = node;
                    nextStack++;
                    nextBuffer++;
                }
                else
                {
                    AST.CSTNode node = objects[head + nextStack + 1] as AST.CSTNode;
                    sub.AppendChild(node, (AST.CSTAction)op);
                    buffer[nextBuffer] = node;
                    nextStack++;
                    nextBuffer++;
                }
            }
            return sub;
        }

        private object ReduceSimple(LRProduction production)
        {
            object sub = parserVariables[production.Head];
            int nextBuffer = 0;
            int nextStack = 0;
            for (int i = 0; i != production.BytecodeLength; i++)
            {
                ushort op = production.Bytecode[i];
                if (op == LRProduction.SemanticAction)
                {
                    ushort index = production.Bytecode[i + 1];
                    parserActions[index](sub, buffer, nextBuffer);
                    i++;
                }
                else if (op >= LRProduction.Virtual)
                {
                    ushort index = production.Bytecode[i + 1];
                    buffer[nextBuffer] = parserVirtuals[index];
                    nextBuffer++;
                    i++;
                }
                else
                {
                    buffer[nextBuffer] = objects[head + nextStack + 1];
                    nextStack++;
                    nextBuffer++;
                }
            }
            return sub;
        }

        private object Execute()
        {
            this.stack = new ushort[maxStackSize];
            this.objects = new object[maxStackSize];
            this.buffer = new object[maxBodyLength];
            Symbols.Token nextToken = input.GetNextToken();
            while (true)
            {
                int action = ExecuteOnToken(nextToken);
                if (action == LRkAutomaton.ActionShift)
                {
                    nextToken = input.GetNextToken();
                    continue;
                }
                if (action == LRkAutomaton.ActionAccept)
                    return objects[head - 1];
                nextToken = OnUnexpectedToken(nextToken);
                if (nextToken == null || errors.Count >= maxErrorCount)
                    return null;
            }
        }

        private int ExecuteOnToken(Symbols.Token token)
        {
            while (true)
            {
                ushort action = 0;
                ushort data = parserAutomaton.GetAction(stack[head], token.SymbolID, out action);
                if (action == 2)
                {
                    head++;
                    stack[head] = data;
                    objects[head] = getSemObj(token);
                    return action;
                }
                else if (action == 1)
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
