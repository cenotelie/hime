using System.Collections.Generic;

namespace Hime.Kernel.Resources
{
    [System.Serializable]
    class CompilerException : System.Exception
    {
        public CompilerException() : base() { }
        public CompilerException(string message) : base(message) { }
        public CompilerException(string message, System.Exception inner) : base(message, inner) { }
        protected CompilerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    class CompilerFatalErrorException : CompilerException
    {
        public CompilerFatalErrorException() : base() { }
        public CompilerFatalErrorException(string message) : base(message) { }
        public CompilerFatalErrorException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    [System.Serializable]
    class NoResourceCompilerFoundException : CompilerException
    {
        public NoResourceCompilerFoundException() : base() { }
        public NoResourceCompilerFoundException(string message) : base(message) { }
        public NoResourceCompilerFoundException(string message, System.Exception innerException) : base(message, innerException) { }
    }


    class CompilerError
    {
        private object innerError;
        private string message;

        public object InnerException { get { return innerError; } }
        public string Message { get { return message; } }

        public CompilerError(object inner) { message = inner.ToString(); innerError = inner; }
        public CompilerError(string message) { this.message = message; }
    }
}
