using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using System.IO;

namespace Hime.Demo.Tasks
{
    class Compile : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.Method = ParsingMethod.LALR1;
            task.Namespace = "Hime.Demo.Generated.MathExp";
            task.ExportLog = false;
            task.ExportDocumentation = false;
            task.InputFiles.Add("Languages\\MathExp.gram");
            task.Execute();
        }
    }
}
