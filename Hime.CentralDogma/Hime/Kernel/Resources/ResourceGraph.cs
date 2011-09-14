using System.Collections.Generic;

namespace Hime.Kernel.Resources
{
    class ResourceGraph
    {
        private Dictionary<Naming.Symbol, Resource> resources;

        public ICollection<Resource> Resources { get { return resources.Values; } }

        public ResourceGraph()
        {
            resources = new Dictionary<Naming.Symbol, Resource>();
        }

        public void AddResource(Resource Resource) { resources.Add(Resource.Symbol, Resource); }
        public Resource GetResource(Naming.Symbol Symbol) { return resources[Symbol]; }
    }
}
