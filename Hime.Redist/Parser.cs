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
        private SymbolToken p_Token;
        private System.Collections.ObjectModel.Collection<string> p_Expected;
        private System.Collections.ObjectModel.ReadOnlyCollection<string> p_ReadOnlyExpected;
        private string p_Message;

        public SymbolToken UnexpectedToken { get { return p_Token; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<string> ExpectedTokens { get { return p_ReadOnlyExpected; } }
        public string Message { get { return p_Message; } }

        public ParserErrorUnexpectedToken(SymbolToken Token, string[] Expected)
        {
            p_Token = Token;
            p_Expected = new System.Collections.ObjectModel.Collection<string>(Expected);
            p_ReadOnlyExpected = new System.Collections.ObjectModel.ReadOnlyCollection<string>(p_Expected);
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unexpected token ");
            Builder.Append(p_Token.Value.ToString());
            Builder.Append(", expected : { ");
            for (int i = 0; i != p_Expected.Count; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(p_Expected[i]);
            }
            Builder.Append(" }.");
            p_Message = Builder.ToString();
        }
        public override string ToString() { return "Parser Error : unexpected token"; }
    }


    public interface IParser
    {
        System.Collections.Generic.List<ParserError> Errors { get; }
        SyntaxTreeNode Analyse();
    }
}