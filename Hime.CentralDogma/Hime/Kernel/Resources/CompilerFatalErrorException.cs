namespace Hime.Kernel.Resources
{
    [System.Serializable]
    class CompilerFatalErrorException : CompilerException
    {
        public CompilerFatalErrorException() : base() { }
        public CompilerFatalErrorException(string message) : base(message) { }
        public CompilerFatalErrorException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}