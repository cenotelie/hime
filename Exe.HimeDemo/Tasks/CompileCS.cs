using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using System.IO;

namespace Hime.Demo.Tasks
{
    class CompileCS : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.Method = ParsingMethod.RNGLALR1;
            task.Namespace = "Hime.Demo.Generated.CS2";
            task.ExportLog = false;
            task.ExportDocumentation = false;
            task.InputFiles.Add("Languages\\CSharp4.gram");
            task.Execute();
        }
    }
}
