/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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
