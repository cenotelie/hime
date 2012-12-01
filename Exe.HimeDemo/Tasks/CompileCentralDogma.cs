using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using System.IO;

namespace Hime.Demo.Tasks
{
    class CompileCentralDogma : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.Method = ParsingMethod.LALR1;
            task.Namespace = "Hime.Benchmark.Generated.CD";
            task.ExportLog = true;
            task.ExportDocumentation = true;
            task.InputFiles.Add("Languages\\FileCentralDogma.gram");
            task.GrammarName = "FileCentralDogma";
            task.Execute();
        }
    }
}
