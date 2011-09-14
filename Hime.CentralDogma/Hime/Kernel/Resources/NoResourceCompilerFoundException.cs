namespace Hime.Kernel.Resources
{
    [System.Serializable]
    class NoResourceCompilerFoundException : CompilerException
    {
        public NoResourceCompilerFoundException() : base() { }
        public NoResourceCompilerFoundException(string message) : base(message) { }
        public NoResourceCompilerFoundException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}