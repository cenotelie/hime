using System.Collections.Generic;

namespace Hime.Kernel.Resources
{
    public abstract class ResourceException : System.Exception
    {
        public ResourceException() : base() { }
        public ResourceException(string message) : base(message) { }
        public ResourceException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    public sealed class AccessorClosedException : ResourceException
    {
        private ResourceAccessor p_Session;

        public ResourceAccessor Session { get { return p_Session; } }

        public AccessorClosedException(ResourceAccessor session)
            : base("The session is already closed")
        {
            p_Session = session;
        }
    }

    public sealed class ResourceNotFoundException : ResourceException
    {
        private string p_ResourceName;

        public string ResourceName { get { return p_ResourceName; } }

        public ResourceNotFoundException(string name)
            : base("No resource with name " + name + " found")
        {
            p_ResourceName = name;
        }
    }
}
