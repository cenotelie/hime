namespace Hime.Kernel.Resources
{
    public interface IResourceCompiler
    {
        string CompilerName { get; }
        int CompilerVersionMajor { get; }
        int CompilerVersionMinor { get; }
        string[] ResourceNames { get; }

        void CreateResource(Symbol Container, Parsers.SyntaxTreeNode SyntaxNode, ResourceGraph Graph, Logs.Log Log);
        void CreateDependencies(Resource Resource, ResourceGraph Graph, Logs.Log Log);
        int CompileSolveDependencies(Resource Resource, Logs.Log Log);
        void Compile(Resource Resource, Logs.Log Log);
        string ToString();
    }
}