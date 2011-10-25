/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a base for all LR(k) parsers
    /// </summary>
    public abstract class LRParser : IParser
    {
        /// <summary>
        /// Callback for rule productions
        /// </summary>
        /// <param name="parser">The reducing parser</param>
        public delegate SyntaxTreeNode Production(LRParser parser);

        /// <summary>
        /// Number of tokens to correctly match during an error recovery procedure
        /// </summary>
        protected int errorSimulationLength;
        /// <summary>
        /// Maximum number of errors before determining the parser definitely fails
        /// </summary>
        protected int maxErrorCount;
        /// <summary>
        /// List of the encountered syntaxic errors
        /// </summary>

        /// <summary>
        /// Rules of the LR(k) parser
        /// </summary>
        protected LRRule[] rules;
        /// <summary>
        /// List of the encountered syntaxic errors
        /// </summary>
        protected List<ParserError> errors;
        /// <summary>
        /// Read-only list of the errors
        /// </summary>
        protected System.Collections.ObjectModel.ReadOnlyCollection<ParserError> readonlyErrors;
        /// <summary>
        /// Lexer associated to this parser
        /// </summary>
        protected ILexer lexer;
        /// <summary>
        /// Parser's stack
        /// </summary>
        protected Stack<ushort> stack;
        /// <summary>
        /// ID of the parser's current state
        /// </summary>
        protected ushort state;
        /// <summary>
        /// Buffer for nodes of the AST being constructed
        /// </summary>
        protected LinkedList<SyntaxTreeNode> nodes;

        /// <summary>
        /// Gets a read-only collection of syntaxic errors encountered by the parser
        /// </summary>
        public ICollection<ParserError> Errors { get { return readonlyErrors; } }



        /// <summary>
        /// Initialization method to be overriden
        /// </summary>
        protected abstract void setup();
        
        /// <summary>
        /// Gets the automaton's state with the given id
        /// </summary>
        /// <param name="id">State's id</param>
        /// <returns>The automaton's state which has the given id, or null if no state with the given id is found</returns>
        protected abstract LRState GetState(int id);

        /// <summary>
        /// Acts when an unexpected token is encountered
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>The new next token if the error is resolved, null otherwise</returns>
        protected abstract SymbolToken OnUnexpectedToken(SymbolToken nextToken);

        /// <summary>
        /// Runs the parser for the given state and token
        /// </summary>
        /// <param name="token">Current token</param>
        /// <returns>true if the parser is able to consume the token, false otherwise</returns>
        protected abstract bool RunForToken(SymbolToken token);

        /// <summary>
        /// Initializes a new instance of the LRParser class with the given lexer
        /// </summary>
        /// <param name="input">Input lexer</param>
        public LRParser(ILexer input)
        {
            this.errorSimulationLength = 3;
            this.maxErrorCount = 100;
            setup();
            this.errors = new List<ParserError>();
            this.readonlyErrors = new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(errors);
            this.lexer = input;
            this.stack = new Stack<ushort>();
            this.state = 0x0000;
            this.nodes = new LinkedList<SyntaxTreeNode>();
            this.lexer.OnError = new OnErrorHandler(OnLexicalError);
        }

        /// <summary>
        /// Adds the given lexical error emanating from the lexer to the list of errors
        /// </summary>
        /// <param name="error">Lexical error</param>
        private void OnLexicalError(ParserError error)
        {
            errors.Add(error);
        }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public SyntaxTreeNode Analyse()
        {
            stack.Push(state);
            SymbolToken nextToken = GetNextToken(lexer, state);

            while (true)
            {
                if (RunForToken(nextToken))
                {
                    nextToken = GetNextToken(lexer, state);
                    continue;
                }
                else if (nextToken.SymbolID == 0x0001)
                    return nodes.First.Value.ApplyActions();
                else
                {
                    errors.Add(new UnexpectedTokenError(nextToken, GetState(state).GetExpectedNames(), lexer.CurrentLine, lexer.CurrentColumn));
                    if (errors.Count >= maxErrorCount)
                        throw new ParserException("Too many errors, parsing stopped.");
                    nextToken = OnUnexpectedToken(nextToken);
                    if (nextToken == null)
                    	return null;
                }
            }
        }
		
		/// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <param name="lexer">Base lexer for reading tokens</param>
        /// <param name="state">Parser's current state</param>
        /// <returns>The next token in the input</returns>
        internal protected SymbolToken GetNextToken(ILexer lexer, ushort state) 
		{ 
			return lexer.GetNextToken(); 
		}
    }
}
