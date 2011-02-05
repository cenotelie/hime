namespace Hime.Kernel.Resources
{
    public interface IResourceCompiler
    {
        string CompilerName { get; }
        int CompilerVersionMajor { get; }
        int CompilerVersionMinor { get; }
        string[] ResourceNames { get; }

        void CreateResource(Symbol Container, Parsers.SyntaxTreeNode SyntaxNode, ResourceGraph Graph, log4net.ILog Log);
        void CreateDependencies(Resource Resource, ResourceGraph Graph, log4net.ILog Log);
        int CompileSolveDependencies(Resource Resource, log4net.ILog Log);
        void Compile(Resource Resource, log4net.ILog Log);
        string ToString();
    }
}