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
    /// Delegate for a semantic action on the given subtree for a parser
    /// </summary>
    /// <param name="subTree">Sub-Tree on which the action is applied</param>
    public delegate void ParserAction(CSTNode subTree);

    /// <summary>
    /// Delegate for a semantic action on the given body for a recognizer
    /// </summary>
    /// <param name="body">The current body</param>
    /// <param name="length">The current body's length</param>
    public delegate void RecognizerAction(Symbol[] body, int length);

    /// <summary>
    /// Represents a base for all LR(k) parsers
    /// </summary>
    public abstract class LRkParser : IParser
    {
        /// <summary>
        /// Maximal size of the stack
        /// </summary>
        private const int stackMaxSize = 100;
        /// <summary>
        /// Maximum number of errors
        /// </summary>
        private const int maxErrorCount = 100;

        /// <summary>
        /// LR(k) parsing table and productions
        /// </summary>
        private LRkAutomaton parserAutomaton;
        /// <summary>
        /// Parser's variables
        /// </summary>
        private SymbolVariable[] parserVariables;
        /// <summary>
        /// Parser's virtuals
        /// </summary>
        private SymbolVirtual[] parserVirtuals;
        /// <summary>
        /// Parser's actions
        /// </summary>
        private ParserAction[] parserActions;
        /// <summary>
        /// Recognizer's actions
        /// </summary>
        private RecognizerAction[] recognizerActions;

        /// <summary>
        /// List of the encountered syntaxic errors
        /// </summary>
        private List<ParserError> errors;
        /// <summary>
        /// Read-only list of the errors
        /// </summary>
        private System.Collections.ObjectModel.ReadOnlyCollection<ParserError> readonlyErrors;
        /// <summary>
        /// Lexer associated to this parser
        /// </summary>
        private ILexer lexer;
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
        /// Gets a read-only collection of syntaxic errors encountered by the parser
        /// </summary>
        public ICollection<ParserError> Errors { get { return readonlyErrors; } }

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
        {
            this.parserAutomaton = automaton;
            this.parserVariables = variables;
            this.parserVirtuals = virtuals;
            this.parserActions = pactions;
            this.recognizerActions = ractions;
            this.errors = new List<ParserError>();
            this.readonlyErrors = new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(errors);
            this.lexer = input;
            this.head = 0;
            this.lexer.OnError += OnLexicalError;
        }

        /// <summary>
        /// Adds the given lexical error emanating from the lexer to the list of errors
        /// </summary>
        /// <param name="error">Lexical error</param>
        protected void OnLexicalError(ParserError error)
        {
            errors.Add(error);
        }

        /// <summary>
        /// Handles an unexpected token and returns whether is successfuly handled the error
        /// </summary>
        /// <param name="token">The unexpected token</param>
        protected void OnUnexpectedToken(SymbolToken token)
        {
            List<int> expectedIDs = parserAutomaton.GetExpected(stack[head], lexer.TerminalsCount);
            List<SymbolTerminal> expected = new List<SymbolTerminal>();
            foreach (int index in expectedIDs)
                expected.Add(lexer.Terminals[index]);
            errors.Add(new UnexpectedTokenError(token, expected, lexer.CurrentLine, lexer.CurrentColumn));
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public CSTNode Parse()
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
                OnUnexpectedToken(nextToken);
                if (errors.Count >= maxErrorCount)
                    return null;
            }
        }

        /// <summary>
        /// Parses the input and returns whether the input is recognized
        /// </summary>
        /// <returns>True if the input is recognized, false otherwise</returns>
        public bool Recognize()
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
                OnUnexpectedToken(nextToken);
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
                    LRkProduction production = parserAutomaton.GetProduction(data);
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
                    LRkProduction production = parserAutomaton.GetProduction(data);
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
