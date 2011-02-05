namespace Hime.Kernel.Resources
{
    /// <summary>
    /// Base abstract exception for resources
    /// </summary>
    public abstract class ResourceException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the ResourceException class
        /// </summary>
        public ResourceException() : base() { }
        /// <summary>
        /// Initializes a new instance of the ResourceException class with an error message
        /// </summary>
        /// <param name="message">Message describing the error</param>
        public ResourceException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the ResourceExcetion class with an error message and a reference to the inner exception that causes the error
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="innerException">Inner exception causing the error</param>
        public ResourceException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Represents error when interactig with an AccessorSession that has already been closed
    /// </summary>
    public sealed class AccessorSessionClosedException : ResourceException
    {
        /// <summary>
        /// Rerefence to the closed session
        /// </summary>
        private AccessorSession p_Session;

        /// <summary>
        /// Get the closed session causing the error
        /// </summary>
        /// <value>The closed session causing the error</value>
        public AccessorSession Session { get { return p_Session; } }

        /// <summary>
        /// Initiliazes a new instance of the AccessorSessionClosedException class with a reference to the closed session
        /// </summary>
        /// <param name="session">The closed session causing the error</param>
        public AccessorSessionClosedException(AccessorSession session) : base("The session is already closed")
        {
            p_Session = session;
        }
    }

    /// <summary>
    /// Represents error when trying to checkout resources with a unregistered session
    /// </summary>
    public sealed class UnregisteredAccessorSessionException : ResourceException
    {
        /// <summary>
        /// Rerefence to the unregistered session
        /// </summary>
        private AccessorSession p_Session;

        /// <summary>
        /// Get the unregistered session causing the error
        /// </summary>
        /// <value>The unregistered session causing the error</value>
        public AccessorSession Session { get { return p_Session; } }

        /// <summary>
        /// Initiliazes a new instance of the UnregisteredAccessorSessionException class with a reference to the unregistered session
        /// </summary>
        /// <param name="session">The closed session causing the error</param>
        public UnregisteredAccessorSessionException(AccessorSession session)
            : base("The session is unregistered")
        {
            p_Session = session;
        }
    }

    /// <summary>
    /// Represents error when trying to checkout resources with a unregistered session
    /// </summary>
    public sealed class ResourceNotFoundException : ResourceException
    {
        /// <summary>
        /// Name of the unknown resource
        /// </summary>
        private string p_ResourceName;

        /// <summary>
        /// Get the name of the unknown resource
        /// </summary>
        /// <value>Name of the unknown resource</value>
        public string ResourceName { get { return p_ResourceName; } }

        /// <summary>
        /// Initiliazes a new instance of the ResourceNotFoundException class with the name of the unknown resource
        /// </summary>
        /// <param name="name">Name of the unknown resource</param>
        public ResourceNotFoundException(string name)
            : base("No resource with name " + name + " found")
        {
            p_ResourceName = name;
        }
    }
}