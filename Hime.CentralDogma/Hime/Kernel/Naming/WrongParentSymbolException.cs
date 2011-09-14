namespace Hime.Kernel.Naming
{
    public class WrongParentSymbolException : NamingException
    {
        private Symbol current;
        private System.Type givenType;
        private System.Type expectedType;

        public Symbol CurrentSymbol { get { return current; } }
        public System.Type GivenType { get { return givenType; } }
        public System.Type ExpectedType { get { return expectedType; } }

        public WrongParentSymbolException(Symbol current, System.Type givenType, System.Type expectedType)
            : base("Wrong parent type for " + current.CompleteName.ToString() + "; expected " + expectedType.Name)
        {
            this.current = current;
            this.givenType = givenType;
            this.expectedType = expectedType;
        }
    }
}