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
        /// Buffer for nodes of the AST being constructed
        /// </summary>
        private CSTNode[] nodes;
        /// <summary>
        /// Buffer for the symbols used by the recognizer
        /// </summary>
        private Symbol[] symbols;
        /// <summary>
        /// Buffer for recognizer's semantic actions
        /// </summary>
        private Symbol[] body;
        /// <summary>
        /// Current stack's head
        /// </summary>
        private int head;

        /// <summary>
        /// Initializes a new instance of the LRkParser class with the given lexer
        /// </summary>
        /// <param name="automaton">The parser's automaton</param>
        /// <param name="variables">The parser's variables</param>
        /// <param name="virtuals">The parser's virtuals</param>
        /// <param name="pactions">The parser's actions in parse mode</param>
        /// <param name="ractions">The parser's actions in recognize mode</param>
        /// <param name="lexer">The input lexer</param>
        public LRkParser(LRkAutomaton automaton, SymbolVariable[] variables, SymbolVirtual[] virtuals, ParserAction[] pactions, RecognizerAction[] ractions, ILexer lexer)
            : base(variables, virtuals, pactions, ractions, lexer)
        {
            this.parserAutomaton = automaton;
            this.input = new RewindableTokenStream(lexer);
        }

        /// <summary>
        /// Handles an unexpected token and returns whether is successfuly handled the error
        /// </summary>
        /// <param name="token">The unexpected token</param>
        /// <returns>The next token</returns>
        protected override SymbolToken OnUnexpectedToken(SymbolToken token)
        {
            List<int> expectedIDs = parserAutomaton.GetExpected(stack[head], lexer.Terminals.Count);
            List<SymbolTerminal> expected = new List<SymbolTerminal>();
            foreach (int index in expectedIDs)
                expected.Add(lexer.Terminals[index]);
            errors.Add(new UnexpectedTokenError(token, expected, lexer.CurrentLine, lexer.CurrentColumn));
            if (!tryRecover) return null;
            if (TryDrop1Unexpected()) return input.GetNextToken();
            if (TryDrop2Unexpected()) return input.GetNextToken();
            foreach (SymbolTerminal terminal in expected)
            {
                SymbolTokenText dummy = new SymbolTokenText(terminal.SymbolID, terminal.Name, string.Empty, 0, 0);
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

        private bool TryInsertExpected(SymbolToken terminal)
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
        public override CSTNode Parse()
        {
            this.stack = new ushort[maxStackSize];
            this.nodes = new CSTNode[maxStackSize];
            SymbolToken nextToken = input.GetNextToken();
            while (true)
            {
                int action = ParseOnToken(nextToken);
                if (action == LRkAutomaton.ActionShift)
                {
                    nextToken = input.GetNextToken();
                    continue;
                }
                if (action == LRkAutomaton.ActionAccept)
                    return nodes[1].ApplyActions();
                nextToken = OnUnexpectedToken(nextToken);
                if (nextToken == null || errors.Count >= maxErrorCount)
                    return null;
            }
        }

        /// <summary>
        /// Parses the input and returns whether the input is recognized
        /// </summary>
        /// <returns>True if the input is recognized, false otherwise</returns>
        public override bool Recognize()
        {
            this.stack = new ushort[maxStackSize];
            this.symbols = new Symbol[maxStackSize];
            this.body = new Symbol[maxStackSize];
            SymbolToken nextToken = input.GetNextToken();
            while (true)
            {
                int action = RecognizeOnToken(nextToken);
                if (action == LRkAutomaton.ActionShift)
                {
                    nextToken = input.GetNextToken();
                    continue;
                }
                if (action == LRkAutomaton.ActionAccept)
                    return true;
                nextToken = OnUnexpectedToken(nextToken);
                if (nextToken == null || errors.Count >= maxErrorCount)
                    return false;
            }
        }

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        private int ParseOnToken(SymbolToken token)
        {
            while (true)
            {
                ushort action = 0;
                ushort data = parserAutomaton.GetAction(stack[head], token.SymbolID, out action);
                if (action == 2)
                {
                    head++;
                    stack[head] = data;
                    nodes[head] = new CSTNode(token);
                    return action;
                }
                else if (action == 1)
                {
                    LRProduction production = parserAutomaton.GetProduction(data);
                    CSTNode sub = new CSTNode(parserVariables[production.Head], (CSTAction)production.HeadAction);
                    head -= production.ReductionLength;
                    int count = 0;
                    for (int i = 0; i != production.BytecodeLength; i++)
                    {
                        ushort op = production.Bytecode[i];
                        if (op == LRProduction.SemanticAction)
                        {
                            ushort index = production.Bytecode[i + 1];
                            parserActions[index](sub);
                            i++;
                        }
                        else if (op >= LRProduction.Virtual)
                        {
                            ushort index = production.Bytecode[i + 1];
                            sub.AppendChild(new CSTNode(parserVirtuals[index], (CSTAction)(op - 4)));
                            i++;
                        }
                        else if (op == LRProduction.PopNoAction)
                        {
                            sub.AppendChild(nodes[head + count + 1]);
                            count++;
                        }
                        else
                        {
                            sub.AppendChild(nodes[head + count + 1], (CSTAction)op);
                            count++;
                        }
                    }
                    data = parserAutomaton.GetAction(stack[head], sub.Symbol.SymbolID, out action);
                    head++;
                    stack[head] = data;
                    nodes[head] = sub;
                    continue;
                }
                return action;
            }
        }

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        private int RecognizeOnToken(SymbolToken token)
        {
            while (true)
            {
                ushort action = 0;
                ushort data = parserAutomaton.GetAction(stack[head], token.SymbolID, out action);
                if (action == LRkAutomaton.ActionShift)
                {
                    head++;
                    stack[head] = data;
                    symbols[head] = token;
                    return action;
                }
                else if (action == LRkAutomaton.ActionReduce)
                {
                    LRProduction production = parserAutomaton.GetProduction(data);
                    SymbolVariable var = parserVariables[production.Head];
                    head -= production.ReductionLength;
                    int nstack = 0;
                    int length = 0;
                    for (int i = 0; i != production.BytecodeLength; i++)
                    {
                        ushort op = production.Bytecode[i];
                        if (op == LRProduction.SemanticAction)
                        {
                            ushort index = production.Bytecode[i + 1];
                            recognizerActions[index](body, length);
                            i++;
                        }
                        else if (op >= LRProduction.Virtual)
                        {
                            ushort index = production.Bytecode[i + 1];
                            body[length] = parserVirtuals[index];
                            length++;
                            i++;
                        }
                        else
                        {
                            body[length] = symbols[head + nstack + 1];
                            length++;
                            nstack++;
                        }
                    }
                    data = parserAutomaton.GetAction(stack[head], var.SymbolID, out action);
                    head++;
                    stack[head] = data;
                    symbols[head] = var;
                    continue;
                }
                return action;
            }
        }
    }
}
