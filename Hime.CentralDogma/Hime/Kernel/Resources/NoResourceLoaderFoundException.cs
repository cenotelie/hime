/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Resources
{
    [System.Serializable]
    class NoResourceLoaderFoundException : System.Exception
    {
        public NoResourceLoaderFoundException() : base() { }
        public NoResourceLoaderFoundException(string message) : base(message) { }
        public NoResourceLoaderFoundException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}