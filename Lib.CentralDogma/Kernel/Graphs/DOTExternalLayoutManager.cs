/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hime.Kernel.Graphs
{
    public class DOTExternalLayoutManager : DOTLayoutManager
    {
        private string executable;

        public DOTExternalLayoutManager(string binary)
        {
            this.executable = binary;
        }

        public void Render(string dotFile, string svgFile)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("-Tsvg");
            builder.Append(" -o");
            builder.Append(svgFile);
            builder.Append(" ");
            builder.Append(dotFile);
            using (Process process = Process.Start(this.executable, builder.ToString()))
			{
            	process.WaitForExit();
			}
        }
    }
}