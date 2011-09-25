using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;

namespace Hime.Demo.Tasks
{
    class Compile : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.Namespace = "Analyser";
            task.ExportLog = true;
            task.ExportDoc = false;
            task.ExportVisuals = false;
            task.InputFiles.Add("Languages\\ANSI_C.gram");
            task.Method = ParsingMethod.LRStar;
            task.DOTBinary = "C:\\Program Files\\Graphviz 2.28\\bin\\dot.exe";
            (new Compiler()).Execute(task);
        }
    }
}
