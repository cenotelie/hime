/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Kernel.Graphs
{
    public class DOTExternalLayoutManager : DOTLayoutManager
    {
        private string executable;

        public DOTExternalLayoutManager(string binary)
        {
            executable = binary;
        }

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