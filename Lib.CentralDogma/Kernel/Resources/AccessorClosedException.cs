/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Resources
{
    class AccessorClosedException : System.Exception
    {
        public AccessorClosedException() : base("The session is already closed")
        {
        }
    }
}