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
            task.ExportDoc = true;
            task.ExportVisuals = true;
            task.InputFiles.Add("Languages\\Test.gram");
            task.ParserFile = "Test.cs";
            task.Method = EParsingMethod.LRStar;
            task.DOTBinary = "C:\\Program Files\\Graphviz 2.28\\bin\\dot.exe";
            task.Execute();
        }
    }
}
