/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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