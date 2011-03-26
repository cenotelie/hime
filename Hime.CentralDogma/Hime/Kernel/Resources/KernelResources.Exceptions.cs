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
        private ResourceAccessor session;

        public ResourceAccessor Session { get { return session; } }

        public AccessorClosedException(ResourceAccessor session)
            : base("The session is already closed")
        {
            this.session = session;
        }
    }

    public sealed class ResourceNotFoundException : ResourceException
    {
        private string resourceName;

        public string ResourceName { get { return resourceName; } }

        public ResourceNotFoundException(string name)
            : base("No resource with name " + name + " found")
        {
            resourceName = name;
        }
    }
}
