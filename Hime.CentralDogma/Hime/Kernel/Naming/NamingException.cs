namespace Hime.Kernel.Naming
{
    public abstract class NamingException : System.Exception
    {
        public NamingException() : base() { }
        public NamingException(string message) : base(message) { }
        public NamingException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}