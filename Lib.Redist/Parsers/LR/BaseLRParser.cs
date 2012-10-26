using System;
using System.Collections.Generic;
using System.Text;

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
    /// Represents a base LR parser
    /// </summary>
    public abstract class BaseLRParser : IParser
    {
        /// <summary>
        /// Maximal size of the stack
        /// </summary>
        protected const int stackMaxSize = 100;
        /// <summary>
        /// Maximum number of errors
        /// </summary>
        protected const int maxErrorCount = 100;

        /// <summary>
        /// Parser's variables
        /// </summary>
        protected Utils.SymbolDictionary<SymbolVariable> parserVariables;
        /// <summary>
        /// Parser's virtuals
        /// </summary>
        protected Utils.SymbolDictionary<SymbolVirtual> parserVirtuals;
        /// <summary>
        /// Parser's actions
        /// </summary>
        protected ParserAction[] parserActions;
        /// <summary>
        /// Recognizer's actions
        /// </summary>
        protected RecognizerAction[] recognizerActions;

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
        /// Gets the variable symbols used by this parser
        /// </summary>
        public Utils.SymbolDictionary<SymbolVariable> Variables { get { return parserVariables; } }

        /// <summary>
        /// Gets the virtual symbols used by this parser
        /// </summary>
        public Utils.SymbolDictionary<SymbolVirtual> Virtuals { get { return parserVirtuals; } }

        /// <summary>
        /// Gets a read-only collection of syntaxic errors encountered by the parser
        /// </summary>
        public ICollection<ParserError> Errors { get { return readonlyErrors; } }

        /// <summary>
        /// Initializes a new instance of the LRkParser class with the given lexer
        /// </summary>
        /// <param name="variables">The parser's variables</param>
        /// <param name="virtuals">The parser's virtuals</param>
        /// <param name="pactions">The parser's actions in parse mode</param>
        /// <param name="ractions">The parser's actions in recognize mode</param>
        /// <param name="input">The input lexer</param>
        public BaseLRParser(SymbolVariable[] variables, SymbolVirtual[] virtuals, ParserAction[] pactions, RecognizerAction[] ractions, ILexer input)
        {
            this.parserVariables = new Utils.SymbolDictionary<SymbolVariable>(variables);
            this.parserVirtuals = new Utils.SymbolDictionary<SymbolVirtual>(virtuals);
            this.parserActions = pactions;
            this.recognizerActions = ractions;
            this.errors = new List<ParserError>();
            this.readonlyErrors = new System.Collections.ObjectModel.ReadOnlyCollection<ParserError>(errors);
            this.lexer = input;
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
        /// <returns>The next token</returns>
        protected abstract SymbolToken OnUnexpectedToken(SymbolToken token);


        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        public abstract CSTNode Parse();

        /// <summary>
        /// Parses the input and returns whether the input is recognized
        /// </summary>
        /// <returns>True if the input is recognized, false otherwise</returns>
        public abstract bool Recognize();
    }
}
