using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public class ParserException : System.Exception
    {
        public ParserException() : base() { }
        public ParserException(string message) : base(message) { }
        public ParserException(string message, System.Exception innerException) : base(message, innerException) { }
        protected ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public interface ParserError
    {
        string Message { get; }
    }
    public class ParserErrorUnexpectedToken : ParserError
    {
        private SymbolToken token;
        private System.Collections.ObjectModel.Collection<string> expected;
        private System.Collections.ObjectModel.ReadOnlyCollection<string> readOnlyExpected;
        private string message;

        public SymbolToken UnexpectedToken { get { return token; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<string> ExpectedTokens { get { return readOnlyExpected; } }
        public string Message { get { return message; } }

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


    public interface IParser
    {
        System.Collections.ObjectModel.ReadOnlyCollection<ParserError> Errors { get; }
        SyntaxTreeNode Analyse();
    }
}
