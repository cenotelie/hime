namespace Hime.Kernel.Resources
{
    public abstract class ResourceException : System.Exception
    {
        public ResourceException() : base() { }
        public ResourceException(string message) : base(message) { }
        public ResourceException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}