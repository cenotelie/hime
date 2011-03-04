namespace Hime.Kernel.Resources
{
    /// <summary>
    /// Base exception for compilers
    /// </summary>
    [System.Serializable]
    class CompilerException : System.Exception
    {
        /// <summary>
        /// Constructs empty exception
        /// </summary>
        public CompilerException() : base() { }
        /// <summary>
        /// Constructs exception from the message
        /// </summary>
        /// <param name="message">Exception message</param>
        public CompilerException(string message) : base(message) { }
        /// <summary>
        /// Constructs exception from the message and the given inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public CompilerException(string message, System.Exception inner) : base(message, inner) { }
        protected CompilerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception thrown when a fatal error is encountered by a compiler
    /// </summary>
    [System.Serializable]
    class CompilerFatalErrorException : CompilerException
    {
        /// <summary>
        /// Constructs empty exception
        /// </summary>
        public CompilerFatalErrorException() : base() { }
        /// <summary>
        /// Constructs exception from the message
        /// </summary>
        /// <param name="message">Exception message</param>
        public CompilerFatalErrorException(string message) : base(message) { }
        /// <summary>
        /// Constructs exception from the message and the given inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public CompilerFatalErrorException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when a fatal error is encountered by a compiler
    /// </summary>
    [System.Serializable]
    class NoResourceCompilerFoundException : CompilerException
    {
        /// <summary>
        /// Constructs empty exception
        /// </summary>
        public NoResourceCompilerFoundException() : base() { }
        /// <summary>
        /// Constructs exception from the message
        /// </summary>
        /// <param name="message">Exception message</param>
        public NoResourceCompilerFoundException(string message) : base(message) { }
        /// <summary>
        /// Constructs exception from the message and the given inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public NoResourceCompilerFoundException(string message, System.Exception innerException) : base(message, innerException) { }
    }


    /// <summary>
    /// Represents a compiler error
    /// </summary>
    class CompilerError
    {
        /// <summary>
        /// Inner error (may be an exception)
        /// </summary>
        private object p_InnerError;
        /// <summary>
        /// Error message
        /// </summary>
        private string p_Message;

        /// <summary>
        /// Get the inner exception handled by the error
        /// </summary>
        /// <value>An error object (exception)</value>
        public object InnerException { get { return p_InnerError; } }
        /// <summary>
        /// Get the error message
        /// </summary>
        /// <value>The error message</value>
        public string Message { get { return p_Message; } }

        /// <summary>
        /// Constructs the error from the given error object
        /// </summary>
        /// <param name="inner">The inner error</param>
        public CompilerError(object inner) { p_Message = inner.ToString(); p_InnerError = inner; }
        /// <summary>
        /// Constructs the error from the given error message
        /// </summary>
        /// <param name="message">Error message</param>
        public CompilerError(string message) { p_Message = message; }
    }
}