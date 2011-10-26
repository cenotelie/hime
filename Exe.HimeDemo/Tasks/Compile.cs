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
            CompilationTask task = new CompilationTask(ParsingMethod.LRStar);
            task.Namespace = "Analyser";
            task.ExportLog = true;
            task.ExportDocumentation = false;
            task.ExportVisuals = false;
            task.InputFiles.Add("Languages\\ANSI_C.gram");
            task.DOTBinary = "C:\\Program Files\\Graphviz 2.28\\bin\\dot.exe";
            (new Compiler()).Execute(task);
        }
    }
}
