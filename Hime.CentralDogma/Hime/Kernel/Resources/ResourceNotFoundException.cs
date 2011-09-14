namespace Hime.Kernel.Resources
{
    public sealed class ResourceNotFoundException : ResourceException
    {
        private string resourceName;

        public string ResourceName { get { return resourceName; } }

        public ResourceNotFoundException(string name)
            : base("No resource with name " + name + " found")
        {
            resourceName = name;
        }
    }
}
