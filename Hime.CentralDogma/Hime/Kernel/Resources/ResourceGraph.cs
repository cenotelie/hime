using System.Collections.Generic;
using Hime.Kernel.Naming;

namespace Hime.Kernel.Resources
{
    class ResourceGraph
    {
        private Dictionary<Symbol, Resource> resources;

        public ICollection<Resource> Resources { get { return resources.Values; } }

        public ResourceGraph()
        {
            resources = new Dictionary<Symbol, Resource>();
        }

        public void AddResource(Resource resource) { resources.Add(resource.Symbol, resource); }
        public Resource GetResource(Symbol symbol) { return resources[symbol]; }
    }
}
