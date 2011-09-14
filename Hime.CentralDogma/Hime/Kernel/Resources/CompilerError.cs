/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Resources
{
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
