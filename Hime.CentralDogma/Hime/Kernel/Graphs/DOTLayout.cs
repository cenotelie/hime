using System.Collections.Generic;

namespace Hime.Kernel.Graphs
{
    public interface DOTLayoutManager
    {
        void Render(string dotFile, string svgFile);
    }

    public class DOTExternalLayoutManager : DOTLayoutManager
    {
        public static string executable;

        public void Render(string dotFile, string svgFile)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("-Tsvg");
            builder.Append(" -o");
            builder.Append(svgFile);
            builder.Append(" ");
            builder.Append(dotFile);
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(executable, builder.ToString());
            process.WaitForExit();
        }
    }
}