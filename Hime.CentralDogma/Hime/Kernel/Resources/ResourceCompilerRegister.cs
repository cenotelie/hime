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

        public void RegisterCompiler(IResourceCompiler Compiler)
        {
            foreach (string Name in Compiler.ResourceNames)
                compilers.Add(Name, Compiler);
        }

        public IResourceCompiler GetCompilerFor(string ResourceName) { return compilers[ResourceName]; }
    }
}
