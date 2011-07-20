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
        private List<string> expected;
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
        /// Initializes a new instance of the ParserErrorUnexpectedToken class with a token and an array of expected names
        /// </summary>
        /// <param name="token">The unexpected token</param>
        /// <param name="expected">The array of expected tokens' names</param>
        public ParserErrorUnexpectedToken(SymbolToken token, string[] expected)
        {
            this.token = token;
            this.expected = new List<string>(expected);
            readOnlyExpected = new System.Collections.ObjectModel.ReadOnlyCollection<string>(this.expected);
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unexpected token ");
            if (token is SymbolTokenText)
            {
                SymbolTokenText tt = token as SymbolTokenText;
                Builder.Append("@");
                Builder.Append(tt.Line);
                Builder.Append(" ");
            }
            Builder.Append(token.Name);
            Builder.Append(": ");
            Builder.Append(token.Value.ToString());
            Builder.Append("; expected: { ");
            for (int i = 0; i != this.expected.Count; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(this.expected[i]);
            }
            Builder.Append(" }.");
            message = Builder.ToString();
        }

        /// <summary>
        /// Returns the string representation of this error
        /// </summary>
        /// <returns>The string representation of this error</returns>
        public override string ToString() { return message; }
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
