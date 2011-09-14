namespace Hime.Kernel.Resources
{
    public sealed class AccessorClosedException : ResourceException
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