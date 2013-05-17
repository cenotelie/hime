using System.IO;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base for all LR(k) parsers
    /// </summary>
    public abstract class LRkParser : BaseLRParser
    {
        private class Simulator : LRkSimulator
        {
            public Simulator(LRkParser parser)
            {
                this.parserAutomaton = parser.parserAutomaton;
                this.parserVariables = parser.parserVariables;
                this.input = parser.input;
                this.stack = new int[parser.maxStackSize];
                this.head = parser.head;
                System.Array.Copy(parser.stack, this.stack, parser.head + 1);
            }
        }

        private LRkAutomaton parserAutomaton;
        private RewindableTokenStream input;
        private int[] stack;
        private AST.BuildNode[] objects;
        private AST.BuildNode[] bufferNodes;
        private Symbols.Symbol[] bufferSymbols;
        private int head;

        /// <summary>
        /// Initializes a new instance of the LRkParser class with the given lexer
        /// </summary>
        /// <param name="automaton">The parser's automaton</param>
        /// <param name="variables">The parser's variables</param>
        /// <param name="virtuals">The parser's virtuals</param>
        /// <param name="actions">The parser's actions</param>
        /// <param name="lexer">The input lexer</param>
        protected LRkParser(LRkAutomaton automaton, Symbols.Variable[] variables, Symbols.Virtual[] virtuals, SemanticAction[] actions, Lexer.TextLexer lexer)
            : base(variables, virtuals, actions, lexer)
        {
            this.parserAutomaton = automaton;
            this.input = new RewindableTokenStream(lexer);
        }

        private Symbols.Token OnUnexpectedToken(Symbols.Token token)
        {
            ICollection<int> expectedIDs = parserAutomaton.GetExpected(stack[head], lexer.Terminals.Count);
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
        public override AST.ASTNode Parse()
        {
            this.stack = new int[maxStackSize];
            this.objects = new AST.BuildNode[maxStackSize];
            this.bufferNodes = new AST.BuildNode[maxBodyLength];
            this.bufferSymbols = new Symbols.Symbol[maxBodyLength];
            Symbols.Token nextToken = input.GetNextToken();
            while (true)
            {
                LRActionCode action = ParseOnToken(nextToken);
                if (action == LRActionCode.Shift)
                {
                    nextToken = input.GetNextToken();
                    continue;
                }
                if (action == LRActionCode.Accept)
                    return objects[head - 1].Value;
                nextToken = OnUnexpectedToken(nextToken);
                if (nextToken == null || errors.Count >= maxErrorCount)
                    return null;
            }
        }

        private LRActionCode ParseOnToken(Symbols.Token token)
        {
            while (true)
            {
                LRAction action = parserAutomaton.GetAction(stack[head], token.SymbolID);
                if (action.Code == LRActionCode.Shift)
                {
                    head++;
                    stack[head] = action.Data;
                    objects[head] = new AST.BuildNode(token);
                    return action.Code;
                }
                else if (action.Code == LRActionCode.Reduce)
                {
                    LRProduction production = parserAutomaton.GetProduction(action.Data);
                    head -= production.ReductionLength;
                    AST.BuildNode result = Reduce(production);
                    action = parserAutomaton.GetAction(stack[head], parserVariables[production.Head].SymbolID);
                    head++;
                    stack[head] = action.Data;
                    objects[head] = result;
                    continue;
                }
                return action.Code;
            }
        }

        private AST.BuildNode Reduce(LRProduction production)
        {
            Symbols.Variable var = parserVariables[production.Head];
            AST.BuildNode sub = new AST.BuildNode(var);
            sub.SetAction(production.HeadAction);
            int nextBuffer = 0;
            int nextStack = 0;
            for (int i = 0; i != production.Bytecode.Length; i++)
            {
                LROpCode op = production.Bytecode[i];
                if (op.IsSemAction)
                {
                    parserActions[production.Bytecode[i + 1].Value](var, bufferSymbols, nextBuffer);
                    i++;
                }
                else if (op.IsAddVirtual)
                {
                    Symbols.Symbol symbol = parserVirtuals[production.Bytecode[i + 1].Value];
                    AST.BuildNode node = new AST.BuildNode(symbol);
                    node.SetAction(op.TreeAction);
                    bufferSymbols[nextBuffer] = symbol;
                    bufferNodes[nextBuffer] = node;
                    nextBuffer++;
                    i++;
                }
                else
                {
                    AST.BuildNode node = objects[head + nextStack + 1];
                    node.SetAction(op.TreeAction);
                    bufferSymbols[nextBuffer] = node.Value.Symbol;
                    bufferNodes[nextBuffer] = node;
                    nextStack++;
                    nextBuffer++;
                }
            }
            sub.Build(bufferNodes, nextBuffer);
            return sub;
        }
    }
}
