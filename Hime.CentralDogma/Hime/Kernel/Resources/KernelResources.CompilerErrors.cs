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
        private object p_InnerError;
        private string p_Message;

        public object InnerException { get { return p_InnerError; } }
        public string Message { get { return p_Message; } }

        public CompilerError(object inner) { p_Message = inner.ToString(); p_InnerError = inner; }
        public CompilerError(string message) { p_Message = message; }
    }
}
