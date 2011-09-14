namespace Hime.Kernel.Graphs
{
    public interface DOTLayoutManager
    {
        void Render(string dotFile, string svgFile);
    }
}