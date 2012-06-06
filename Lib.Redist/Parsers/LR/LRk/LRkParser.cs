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
    /// Delegate for a semantic action on the given subtree
    /// </summary>
    /// <param name="subTree">Sub-Tree on which the action is applied</param>
    public delegate void SemanticAction(SyntaxTreeNode subTree);

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
        private SemanticAction[] parserActions;

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
        private SyntaxTreeNode[] nodes;
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
        /// <param name="input">Input lexer</param>
        public LRkParser(LRkAutomaton automaton, SymbolVariable[] variables, SymbolVirtual[] virtuals, SemanticAction[] actions, ILexer input)
        {
            this.parserAutomaton = automaton;
            this.parserVariables = variables;
            this.parserVirtuals = virtuals;
            this.parserActions = actions;
            this.errors = new List<ParserError>();
            this.readonlyErrors = new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(errors);
            this.lexer = input;
            this.stack = new ushort[stackMaxSize];
            this.nodes = new SyntaxTreeNode[stackMaxSize];
            this.head = 0;
            this.lexer.OnError += RegisterError;
        }

        /// <summary>
        /// Adds the given lexical error emanating from the lexer to the list of errors
        /// </summary>
        /// <param name="error">Lexical error</param>
        protected void RegisterError(ParserError error)
        {
            errors.Add(error);
        }

        protected virtual bool OnUnexpectedToken(SymbolToken token)
        {
            //errors.Add(new UnexpectedTokenError(nextToken, GetState(state).expecteds, lexer.CurrentLine, lexer.CurrentColumn));
            return false;
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public SyntaxTreeNode Analyse()
        {
            SymbolToken nextToken = lexer.GetNextToken();
            while (true)
            {
                if (AnalyseOnToken(nextToken))
                {
                    nextToken = lexer.GetNextToken();
                    continue;
                }
                if (nextToken.SymbolID == 0x0001)
                    return nodes[1].ApplyActions();
                if (!OnUnexpectedToken(nextToken))
                    return null;
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
                return false;
            }
        }

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        private bool AnalyseOnToken(SymbolToken token)
        {
            while (true)
            {
                ushort action = 0;
                ushort data = parserAutomaton.GetAction(stack[head], token.SymbolID, out action);
                if (action == 2)
                {
                    head++;
                    stack[head] = data;
                    nodes[head] = new SyntaxTreeNode(token);
                    return true;
                }
                else if (action == 1)
                {
                    LRkProduction production = parserAutomaton.GetProduction(data);
                    SyntaxTreeNode sub = new SyntaxTreeNode(parserVariables[production.Head], (SyntaxTreeNodeAction)production.HeadAction);
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
                            sub.AppendChild(new SyntaxTreeNode(parserVirtuals[index], (SyntaxTreeNodeAction)(op - 4)));
                            i++;
                        }
                        else if (op == 0)
                        {
                            sub.AppendChild(nodes[head + count + 1]);
                            count++;
                        }
                        else
                        {
                            sub.AppendChild(nodes[head + count + 1], (SyntaxTreeNodeAction)op);
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
                    return true;
                }
                else if (action == 1)
                {
                    LRkProduction production = parserAutomaton.GetProduction(data);
                    ushort headID = parserVariables[production.Head].SymbolID;
                    head -= production.ReductionLength;
                    for (int i = 0; i != production.BytecodeLength; i++)
                    {
                        ushort op = production.Bytecode[i];
                        if (op == 8)
                        {
                            ushort index = production.Bytecode[i + 1];
                            parserActions[index](null);
                            i++;
                        }
                    }
                    data = parserAutomaton.GetAction(stack[head], headID, out action);
                    head++;
                    stack[head] = data;
                    continue;
                }
                return false;
            }
        }
    }
}
