using System.Collections.Generic;
using Hime.Kernel.Naming;
using Hime.Kernel.Reporting;
using Hime.Redist.Parsers;

namespace Hime.Kernel.Resources
{
    interface IResourceCompiler
    {
        string CompilerName { get; }
        int CompilerVersionMajor { get; }
        int CompilerVersionMinor { get; }
        string[] ResourceNames { get; }

        void CreateResource(Naming.Symbol container, SyntaxTreeNode syntaxNode, ResourceGraph graph, Reporter log);
        void CreateDependencies(Resource resource, ResourceGraph graph, Reporter log);
        int CompileSolveDependencies(Resource resource, Reporter log);
        bool Compile(Resource resource, Reporter log);
        string ToString();
    }
}
