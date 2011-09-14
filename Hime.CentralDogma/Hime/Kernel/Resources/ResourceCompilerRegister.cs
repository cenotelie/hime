/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Kernel.Resources
{
    class ResourceCompilerRegister
    {
        private Dictionary<string, IResourceCompiler> compilers;

        public ICollection<IResourceCompiler> Compilers { get { return compilers.Values; } }

        public ResourceCompilerRegister()
        {
            compilers = new Dictionary<string, IResourceCompiler>();
        }

        public void RegisterCompiler(IResourceCompiler compiler)
        {
            foreach (string name in compiler.ResourceNames)
                compilers.Add(name, compiler);
        }

        public IResourceCompiler GetCompilerFor(string resourceName) { return compilers[resourceName]; }
    }
}
