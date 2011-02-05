namespace Hime.Kernel.Resources
{
    public interface IResourceCompiler
    {
        string CompilerName { get; }
        int CompilerVersionMajor { get; }
        int CompilerVersionMinor { get; }
        string[] ResourceNames { get; }

        void CreateResource(Symbol Container, Parsers.SyntaxTreeNode SyntaxNode, ResourceGraph Graph, Hime.Kernel.Reporting.Reporter Log);
        void CreateDependencies(Resource Resource, ResourceGraph Graph, Hime.Kernel.Reporting.Reporter Log);
        int CompileSolveDependencies(Resource Resource, Hime.Kernel.Reporting.Reporter Log);
        void Compile(Resource Resource, Hime.Kernel.Reporting.Reporter Log);
        string ToString();
    }
}