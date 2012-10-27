using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using System.IO;

namespace Hime.Demo.Tasks
{
    class CompileC : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.Method = ParsingMethod.RNGLALR1;
            task.Namespace = "Hime.Demo.Generated.C";
            task.ExportLog = true;
            task.ExportDocumentation = true;
            task.InputFiles.Add("Languages\\ANSI_C.gram");
            task.Execute();
        }
    }
}
