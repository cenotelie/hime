using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an exception in a parser
    /// </summary>
    public class ParserException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the ParserException class
        /// </summary>
        public ParserException() : base() { }
        /// <summary>
        /// Initializes a new instance of the ParserException class with the given message
        /// </summary>
        /// <param name="message">The message conveyed by this exception</param>
        public ParserException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the ParserException class with the given message and exception
        /// </summary>
        /// <param name="message">The message conveyed by this exception</param>
        /// <param name="innerException">The inner catched exception</param>
        public ParserException(string message, System.Exception innerException) : base(message, innerException) { }
        protected ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Represents an error in a parser
    /// </summary>
    public interface ParserError
    {
        /// <summary>
        /// Gets the error's message
        /// </summary>
        string Message { get; }
    }

    /// <summary>
    /// Represents an unexpected token error in a parser
    /// </summary>
    public sealed class ParserErrorUnexpectedToken : ParserError
    {
        private SymbolToken token;
        private System.Collections.ObjectModel.Collection<string> expected;
        private System.Collections.ObjectModel.ReadOnlyCollection<string> readOnlyExpected;
        private string message;

        /// <summary>
        /// Gets the unexpected token
        /// </summary>
        public SymbolToken UnexpectedToken { get { return token; } }
        /// <summary>
        /// Gets a collection of the expected tokens
        /// </summary>
        public ICollection<string> ExpectedTokens { get { return readOnlyExpected; } }
        /// <summary>
        /// Gets the error's message
        /// </summary>
        public string Message { get { return message; } }

        /// <summary>
        /// initializes a new instance of the ParserErrorUnexpectedToken class with a token and an array of expected names
        /// </summary>
        /// <param name="Token">The unexpected token</param>
        /// <param name="Expected">The array of expected tokens' names</param>
        public ParserErrorUnexpectedToken(SymbolToken Token, string[] Expected)
        {
            token = Token;
            expected = new System.Collections.ObjectModel.Collection<string>(Expected);
            readOnlyExpected = new System.Collections.ObjectModel.ReadOnlyCollection<string>(expected);
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unexpected token ");
            Builder.Append(token.Value.ToString());
            Builder.Append(", expected : { ");
            for (int i = 0; i != expected.Count; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(expected[i]);
            }
            Builder.Append(" }.");
            message = Builder.ToString();
        }
        public override string ToString() { return "Parser Error : unexpected token"; }
    }

    /// <summary>
    /// Represents a parser
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Gets the errors encountered by the parser
        /// </summary>
        ICollection<ParserError> Errors { get; }
        /// <summary>
        /// Runs the parser and return the root of the abstract syntax tree
        /// </summary>
        /// <returns>The root of the abstract syntax tree representing the input, or null if errors when encountered</returns>
        SyntaxTreeNode Analyse();
    }
}
