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
            task.Namespace = "Hime.Kernel.CommandLine.Parser";
            task.ExportLog = true;
            task.ExportDoc = false;
            task.ExportVisuals = false;
            task.InputFiles.Add("Languages\\CommandLine.gram");
            task.ParserFile = "Parser.cs";
            task.Method = ParsingMethod.LALR1;
            task.DOTBinary = "C:\\Program Files\\Graphviz 2.28\\bin\\dot.exe";
            Compiler compiler = new Compiler();
            compiler.Execute(task);
        }
    }
}
