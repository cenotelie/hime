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
        private ResourceAccessor session;

        public ResourceAccessor Session { get { return session; } }

        public AccessorClosedException(ResourceAccessor session)
            : base("The session is already closed")
        {
            this.session = session;
        }
    }
}