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
        /// <summary>
        /// LR(k) parsing table and productions
        /// </summary>
        private LRkAutomaton parserAutomaton;
        
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
        /// <param name="input">The input lexer</param>
        public LRkParser(LRkAutomaton automaton, SymbolVariable[] variables, SymbolVirtual[] virtuals, ParserAction[] pactions, RecognizerAction[] ractions, ILexer input)
            : base(variables, virtuals, pactions, ractions, input)
        {
            this.parserAutomaton = automaton;
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
            return lexer.GetNextToken();
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public override CSTNode Parse()
        {
            this.stack = new ushort[stackMaxSize];
            this.nodes = new CSTNode[stackMaxSize];
            SymbolToken nextToken = lexer.GetNextToken();
            while (true)
            {
                if (ParseOnToken(nextToken))
                {
                    nextToken = lexer.GetNextToken();
                    continue;
                }
                if (nextToken.SymbolID == 0x0001)
                    return nodes[1].ApplyActions();
                nextToken = OnUnexpectedToken(nextToken);
                if (errors.Count >= maxErrorCount)
                    return null;
            }
        }

        /// <summary>
        /// Parses the input and returns whether the input is recognized
        /// </summary>
        /// <returns>True if the input is recognized, false otherwise</returns>
        public override bool Recognize()
        {
            this.stack = new ushort[stackMaxSize];
            this.symbols = new Symbol[stackMaxSize];
            this.body = new Symbol[stackMaxSize];
            SymbolToken nextToken = lexer.GetNextToken();
            while (true)
            {
                if (RecognizeOnToken(nextToken))
                {
                    nextToken = lexer.GetNextToken();
                    continue;
                }
                if (nextToken.SymbolID == 0x0001)
                    return true;
                nextToken = OnUnexpectedToken(nextToken);
                return false;
            }
        }

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        private bool ParseOnToken(SymbolToken token)
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
                    return true;
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
                        if (op == 8)
                        {
                            ushort index = production.Bytecode[i + 1];
                            parserActions[index](sub);
                            i++;
                        }
                        else if (op > 3)
                        {
                            ushort index = production.Bytecode[i + 1];
                            sub.AppendChild(new CSTNode(parserVirtuals[index], (CSTAction)(op - 4)));
                            i++;
                        }
                        else if (op == 0)
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
                return false;
            }
        }

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        private bool RecognizeOnToken(SymbolToken token)
        {
            while (true)
            {
                ushort action = 0;
                ushort data = parserAutomaton.GetAction(stack[head], token.SymbolID, out action);
                if (action == 2)
                {
                    head++;
                    stack[head] = data;
                    symbols[head] = token;
                    return true;
                }
                else if (action == 1)
                {
                    LRProduction production = parserAutomaton.GetProduction(data);
                    SymbolVariable var = parserVariables[production.Head];
                    head -= production.ReductionLength;
                    int nstack = 0;
                    int length = 0;
                    for (int i = 0; i != production.BytecodeLength; i++)
                    {
                        ushort op = production.Bytecode[i];
                        if (op == 8)
                        {
                            ushort index = production.Bytecode[i + 1];
                            recognizerActions[index](body, length);
                            i++;
                        }
                        else if (op > 3)
                        {
                            ushort index = production.Bytecode[i + 1];
                            body[length] = parserVariables[index];
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
                return false;
            }
        }
    }
}
