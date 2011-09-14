/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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